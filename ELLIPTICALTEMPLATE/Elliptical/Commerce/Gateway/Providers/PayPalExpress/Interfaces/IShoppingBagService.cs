using System.Collections.Generic;
using Elliptical.Mvc.Commerce.Models;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    public interface IShoppingBagService<T>
    {
        PayPalBag GetPayPalBag(IShoppingBag<T> shoppingBag);
        List<ExpressCheckoutItem> LineItems(PayPalBag bag);
    }
}