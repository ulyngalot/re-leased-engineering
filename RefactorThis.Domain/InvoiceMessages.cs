﻿namespace RefactorThis.Domain
{
    public static class InvoiceMessages
    {
        public const string NoInvoiceFound = "There is no invoice matching this payment";
        public const string NoPaymentNeeded = "no payment needed";
        public const string InvalidInvoiceState = "The invoice is in an invalid state, it has an amount of 0 and it has payments.";
        public const string AlreadyFullyPaid = "invoice was already fully paid";
        public const string PaymentExceedsRemaining = "the payment is greater than the partial amount remaining";
        public const string PaymentExceedsInvoice = "the payment is greater than the invoice amount";
        public const string FullyPaid = "invoice is now fully paid";
        public const string FinalPartialPayment = "final partial payment received, invoice is now fully paid";
        public const string PartiallyPaid = "invoice is now partially paid";
        public const string AdditionalPartialPayment = "another partial payment received, still not fully paid";
    }
}
