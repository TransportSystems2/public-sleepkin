using System;

namespace PromoCode.API.Infrastructure.Exceptions
{
    public class ExceededUsageLimitException : ApplicationException
    {
        public ExceededUsageLimitException(string message) : base(message)
        {
        }
    }
}