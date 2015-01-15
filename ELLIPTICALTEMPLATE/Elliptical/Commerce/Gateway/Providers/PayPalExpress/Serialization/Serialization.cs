using System;
using System.Reflection;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    public class FormatAttribute : Serialization.FormatAttribute
    {
        public FormatAttribute(string format) : base(format)
        {
        }
    }

    public class HttpPostSerializer : Serialization.HttpPostSerializer
    {
    }

    public class OptionalAttribute : Serialization.OptionalAttribute
    {
    }

    public class UnencodedAttribute : Serialization.UnencodedAttribute
    {
    }


    public class ResponseSerializer : Serialization.ResponseSerializer
    {
        public override void Deserialize(Type type, string input, object objectToDeserializeInto)
        {
            if (string.IsNullOrEmpty(input)) return;

            var bits = input.Split(new[] {"&"}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var nameValuePairCombined in bits)
            {
                var index = nameValuePairCombined.IndexOf('=');
                if (index < 0)
                {
                    Logging.LogMessage("Could not deserialize NameValuePair: " + nameValuePairCombined);
                    continue;
                }
                var name = nameValuePairCombined.Substring(0, index);
                var value = nameValuePairCombined.Substring(index + 1);
                var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);

                if (prop == null)
                {
                    // Ignore any additional NVPs that we don't have properties for instead of throwing exception
                    // This does mean we only capture the first of any errors returned into L_ERRORCODE0 etc
                    continue;
                }

                object convertedValue;

                if (prop.PropertyType == typeof (ResponseType))
                    convertedValue = ResponseTypes.ConvertStringToPayPalResponseType(value);
                else if (prop.PropertyType == typeof (CheckoutStatus))
                    convertedValue = CheckoutStatuses.ConvertStringToPayPalCheckoutStatus(value);
                else if (prop.PropertyType == typeof (PaymentStatus))
                    convertedValue = PaymentStatuses.ConvertStringToPayPalCheckoutStatus(value);
                else
                    convertedValue = Convert.ChangeType(value, prop.PropertyType);

                prop.SetValue(objectToDeserializeInto, convertedValue, null);
            }
        }
    }
}