using System;

namespace PromoCode.API.Infrastructure.Exceptions
{
    public class NotFoundPromoCodeException : ApplicationException
    {
        public NotFoundPromoCodeException(string message) : base(message)
        {
        }
    }
}