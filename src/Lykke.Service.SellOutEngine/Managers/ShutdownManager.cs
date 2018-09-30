using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;

namespace Lykke.Service.SellOutEngine.Managers
{
    [UsedImplicitly]
    public class ShutdownManager : IShutdownManager
    {
        public ShutdownManager()
        {
        }

        public Task StopAsync()
        {
            return Task.CompletedTask;
        }
    }
}
