using System.Collections.Generic;
using System.Linq;
using RefactorThis.Persistence;

namespace RefactorThis.Domain
{
	public class InvoiceService
	{
		private readonly InvoiceRepository _invoiceRepository;

		public InvoiceService( InvoiceRepository invoiceRepository )
		{
			_invoiceRepository = invoiceRepository;
		}

        public string ProcessPayment(Payment payment)
        {
            var invoice = _invoiceRepository.GetInvoice(payment.Reference);

            if (invoice == null)
                throw new InvoiceProcessingException(InvoiceMessages.NoInvoiceFound);

            if (invoice.Amount == 0)
            {
                if (invoice.Payments == null || !invoice.Payments.Any())
                    return InvoiceMessages.NoPaymentNeeded;
                else
                    throw new InvoiceProcessingException(InvoiceMessages.InvalidInvoiceState);
            }

            decimal totalPaid = invoice.Payments?.Sum(p => p.Amount) ?? 0;
            decimal amountDue = invoice.Amount - invoice.AmountPaid;

            if (totalPaid > 0)
            {
                if (invoice.Amount == totalPaid)
                    return InvoiceMessages.AlreadyFullyPaid;

                if (payment.Amount > amountDue)
                    return InvoiceMessages.PaymentExceedsRemaining;
            }
            else
            {
                if (payment.Amount > invoice.Amount)
                    return InvoiceMessages.PaymentExceedsInvoice;
            }

            RecordInvoicePayment(invoice, payment);
            invoice.Save();

            return BuildPaymentStatusMessage(invoice);
        }

        private void RecordInvoicePayment(Invoice invoice, Payment payment)
        {
            invoice.AmountPaid += payment.Amount;

            if (invoice.Payments == null)
                invoice.Payments = new List<Payment>();

            invoice.Payments.Add(payment);

            if (invoice.Type == InvoiceType.Commercial)
                invoice.TaxAmount += payment.Amount * 0.14m;
        }

        private string BuildPaymentStatusMessage(Invoice invoice)
        {
            decimal remaining = invoice.Amount - invoice.AmountPaid;

            if (remaining == 0)
            {
                return invoice.Payments.Count == 1
                    ? InvoiceMessages.FullyPaid
                    : InvoiceMessages.FinalPartialPayment;
            }

            return invoice.Payments.Count == 1
                ? InvoiceMessages.PartiallyPaid
                : InvoiceMessages.AdditionalPartialPayment;
        }
    }
}