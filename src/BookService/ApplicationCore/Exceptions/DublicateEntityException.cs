using System;

namespace Pillow.ApplicationCore.Exceptions
{
    public class DublicateEntityException: ApplicationException
    {
        public DublicateEntityException(string message, string entityCode)
            : base(message)
        {
            EntityId = entityCode;
        }

        public string EntityId { get; }
    }
}