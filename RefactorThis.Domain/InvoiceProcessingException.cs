using System;

namespace RefactorThis.Domain
{
    public class InvoiceProcessingException : InvalidOperationException
    {
        public InvoiceProcessingException(string message) : base(message) { }
    }
}
