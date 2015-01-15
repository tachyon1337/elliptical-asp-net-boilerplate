using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Elliptical.Mvc.Configuration.Commerce;

namespace Elliptical.Mvc.Commerce.Gateway
{
    public static class Configuration
    {
        private static readonly CommerceSection config =
            ConfigurationManager.GetSection("ellipticalCommerce") as CommerceSection;

        public static string GetMode()
        {
            return config.PaymentGatewayConfiguration.Mode.ToLower();
        }

        public static IEnumerable<Provider> GetActiveModeProviders(string mode)
        {
            var providers = config.PaymentGatewayConfiguration.Providers.OfType<Provider>();
            return
                providers.Select(x => x)
                    .Where(x => x.ForMode.ToLower() == mode.ToLower() || x.ForMode.ToLower() == "any");
        }

        public static NameValueCollection GetProviderSettings(Provider provider)
        {
            var settings = provider.Settings.OfType<Add>();
            var namevalueColl = new NameValueCollection();
            foreach (var s in settings)
            {
                namevalueColl.Add(s.Key, s.Value);
            }

            return namevalueColl;
        }
    }
}