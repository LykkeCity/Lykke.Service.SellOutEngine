using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Sdk;

namespace Lykke.Service.SellOutEngine.Managers
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        public StartupManager()
        {
        }

        public Task StartAsync()
        {
            return Task.CompletedTask;
        }
    }
}
