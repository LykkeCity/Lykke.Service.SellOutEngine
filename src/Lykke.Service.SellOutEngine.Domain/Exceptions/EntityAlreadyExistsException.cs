using System;

namespace Lykke.Service.SellOutEngine.Domain.Exceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException()
            : base("Entity already exists")
        {
        }

        public EntityAlreadyExistsException(Exception innerException)
            : base("Entity already exists", innerException)
        {
        }
    }
}
