using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    public enum PaymentAction
    {
        Order,
        Authorization,
        Sale
    }
}