using System;
namespace PromoCode.API.Infrastructure.Exceptions
{
    public class AlreadyUsedPromoCodeException : ApplicationException
    {
        public AlreadyUsedPromoCodeException(string message) : base(message)
        {
        }
    }
}