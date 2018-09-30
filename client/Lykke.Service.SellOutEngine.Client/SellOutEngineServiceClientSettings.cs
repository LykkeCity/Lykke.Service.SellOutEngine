using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.SellOutEngine.Client
{
    /// <summary>
    /// Settings for sell out engine service client.
    /// </summary>
    [PublicAPI]
    public class SellOutEngineServiceClientSettings
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl { get; set; }
    }
}
