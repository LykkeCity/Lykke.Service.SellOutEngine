using System;

namespace Lykke.Service.SellOutEngine.Domain.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
            : base("Entity not found")
        {
        }
    }
}
