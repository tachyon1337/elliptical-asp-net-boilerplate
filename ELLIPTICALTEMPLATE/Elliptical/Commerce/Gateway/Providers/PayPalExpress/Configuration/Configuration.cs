using System;
using System.Globalization;
using Elliptical.Mvc.Configuration.Commerce;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    public class Configuration
    {
        // Static PayPal properties
        public const string Version = "94.0";
        private const string LiveUrl = "https://api-3t.paypal.com/nvp";
        private const string RedirectLiveUrl = "https://www.paypal.com/webscr?cmd=_express-checkout&token={0}";
        private const string SandboxUrl = "https://api-3t.sandbox.paypal.com/nvp";

        private const string RedirectSandboxUrl =
            "https://www.sandbox.paypal.com/webscr?cmd=_express-checkout&token={0}";

        public static CultureInfo CultureForTransactionEncoding = new CultureInfo("en-US");
        private static Configuration currentConfiguration;
        private string cancelAction;
        private string merchantPassword;
        // Application specific properties
        private string merchantUserName;
        private string returnAction;
        private string signature;
        private string storeTitle;

        /// <summary>
        ///     User name. This is required.
        /// </summary>
        public string MerchantUserName
        {
            get
            {
                if (string.IsNullOrEmpty(merchantUserName))
                    throw new ArgumentNullException("MerchantUserName",
                        "MerchantUserName must be specified in the configuration.");
                return merchantUserName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("MerchantUserName",
                        "MerchantUserName must be specified in the configuration.");
                merchantUserName = value;
            }
        }

        /// <summary>
        ///     User Password. This is required.
        /// </summary>
        public string MerchantPassword
        {
            get
            {
                if (string.IsNullOrEmpty(merchantPassword))
                    throw new ArgumentNullException("MerchantPassword",
                        "MerchantPassword must be specified in the configuration.");
                return merchantPassword;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("MerchantPassword",
                        "MerchantPassword must be specified in the configuration.");
                merchantPassword = value;
            }
        }

        /// <summary>
        ///     User Signature. This is required.
        /// </summary>
        public string Signature
        {
            get
            {
                if (string.IsNullOrEmpty(signature))
                    throw new ArgumentNullException("Signature", "Signature must be specified in the configuration.");
                return signature;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("Signature", "Signature must be specified in the configuration.");
                signature = value;
            }
        }

        /// <summary>
        ///     Cancel Action. This is required.
        /// </summary>
        public string CancelAction
        {
            get
            {
                if (string.IsNullOrEmpty(cancelAction))
                    throw new ArgumentNullException("CancelAction",
                        "CancelAction must be specified in the configuration.");
                return cancelAction;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("CancelAction",
                        "CancelAction must be specified in the configuration.");
                cancelAction = value;
            }
        }

        /// <summary>
        ///     Return Action. This is required.
        /// </summary>
        public string ReturnAction
        {
            get
            {
                if (string.IsNullOrEmpty(returnAction))
                    throw new ArgumentNullException("ReturnAction",
                        "ReturnAction must be specified in the configuration.");
                return returnAction;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ArgumentNullException("ReturnAction",
                        "ReturnAction must be specified in the configuration.");
                returnAction = value;
            }
        }

        /// <summary>
        ///     Site Mode
        /// </summary>
        public string Mode { get; set; }

        public string StoreTitle { get; set; }

        /// <summary>
        ///     Gets the current configuration. If none has been specified using Configuration.Configure, it is loaded from the
        ///     web.config
        /// </summary>
        public static Configuration Current
        {
            get
            {
                if (currentConfiguration == null)
                    currentConfiguration = LoadConfigurationFromConfigFile();

                return currentConfiguration;
            }
        }

        /// <summary>
        ///     The PayPal Server URL
        /// </summary>
        public string PayPalAPIUrl
        {
            get { return (Mode == "live") ? LiveUrl : SandboxUrl; }
        }

        /// <summary>
        ///     The PayPal Redirect URL
        /// </summary>
        public string PayPalRedirectUrl
        {
            get { return (Mode == "live") ? RedirectLiveUrl : RedirectSandboxUrl; }
        }

        /// <summary>
        ///     Sets up the configuration using a manually generated Configuration instance rather than using the Web.config file.
        /// </summary>
        /// <param name="configuration"></param>
        public static void Configure(Configuration configuration)
        {
            currentConfiguration = configuration;
        }

        private static Configuration LoadConfigurationFromConfigFile()
        {
            var mode = Gateway.Configuration.GetMode();
            var providers = Gateway.Configuration.GetActiveModeProviders(mode);
            Provider payPalProvider = null;
            foreach (var p in providers)
            {
                if (p.Id.ToLower() == "paypalexpress")
                {
                    payPalProvider = p;
                    break;
                }
            }

            if (payPalProvider == null)
            {
                throw new Exception(
                    "PayPal Express Provider Required...Please update the site's web.config file with the PayPalExpress provider settings");
            }

            string _merchantUserName = null;
            string _merchantPassword = null;
            string _signature = null;
            string _cancelAction = null;
            string _returnAction = null;
            string _storeTitle = null;

            var settings = Gateway.Configuration.GetProviderSettings(payPalProvider);
            _returnAction = settings["ReturnAction"];
            _cancelAction = settings["CancelAction"];
            _signature = settings[mode + ".ApiSignature"];
            _storeTitle = settings["PayPalStoreTitle"];
            if (_storeTitle == null)
            {
                _storeTitle = "PayPal Store";
            }

            if (string.IsNullOrEmpty(_signature))
            {
                _signature = settings["ApiSignature"];
            }
            _merchantUserName = settings[mode + ".ApiLogin"];
            if (string.IsNullOrEmpty(_merchantUserName))
            {
                _merchantUserName = settings["ApiLogin"];
            }
            _merchantPassword = settings[mode + ".ApiPassword"];
            if (string.IsNullOrEmpty(_merchantPassword))
            {
                _merchantPassword = settings["ApiPassword"];
            }

            var configuration = new Configuration
            {
                Mode = mode,
                MerchantUserName = _merchantUserName,
                MerchantPassword = _merchantPassword,
                Signature = _signature,
                CancelAction = _cancelAction,
                ReturnAction = _returnAction,
                StoreTitle = _storeTitle
            };

            return configuration;
        }
    }
}