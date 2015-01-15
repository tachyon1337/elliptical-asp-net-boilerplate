using System.Configuration;

namespace Elliptical.Mvc.Configuration.Commerce
{
    /// <summary>
    /// </summary>
    public class CommerceSection : ConfigurationSection
    {
        [ConfigurationProperty("shipping")]
        public ShippingConfiguration ShippingConfiguration
        {
            get { return (ShippingConfiguration) this["shipping"]; }
            set { this["shipping"] = value; }
        }

        [ConfigurationProperty("creditCard")]
        public CreditCardConfiguration CreditCardConfiguration
        {
            get { return (CreditCardConfiguration) this["creditCard"]; }
            set { this["creditCard"] = value; }
        }

        [ConfigurationProperty("paymentGateway")]
        public PaymentGatewayConfiguration PaymentGatewayConfiguration
        {
            get { return (PaymentGatewayConfiguration) this["paymentGateway"]; }
            set { this["paymentGateway"] = value; }
        }
    }
}