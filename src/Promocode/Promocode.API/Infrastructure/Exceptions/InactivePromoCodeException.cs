using System;

namespace PromoCode.API.Infrastructure.Exceptions
{
    public class InactivePromoCodeException : ApplicationException
    {
        public InactivePromoCodeException(string message) : base(message)
        {
        }
    }
}