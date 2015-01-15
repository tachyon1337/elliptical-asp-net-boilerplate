using System.Collections.Generic;
using Elliptical.Mvc.Commerce.Models;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    public class ShoppingBagService<T> : IShoppingBagService<T>
    {
        public PayPalBag GetPayPalBag(IShoppingBag<T> shoppingBag)
        {
            var payPalBag = new PayPalBag();
            payPalBag.Currency = "USD";
            payPalBag.PurchaseDescription = "Test Shopping Bag";
            payPalBag.TotalAmount = shoppingBag.Total;
            payPalBag.Id = shoppingBag.Id;
            payPalBag.DiscountCode = shoppingBag.Code;
            payPalBag.DiscountAmount = shoppingBag.Discount;
            payPalBag.ShippingAmount = (decimal) shoppingBag.Shipping;
            payPalBag.TaxAmount = (decimal) shoppingBag.Tax;
            payPalBag.ItemAmount = shoppingBag.Subtotal;
            if (shoppingBag.Discount != null && shoppingBag.Discount > 0)
            {
                payPalBag.ItemAmount = payPalBag.ItemAmount - (decimal) shoppingBag.Discount;
            }
            var items = new List<PayPalBagItem>();
            var bagItems = shoppingBag.Items;
            foreach (var bagItem in bagItems)
            {
                var item = new PayPalBagItem();
                item.Number = bagItem.Sku;
                item.Name = bagItem.Name;
                item.Price = bagItem.Price;
                item.Quantity = bagItem.Quantity;

                items.Add(item);
            }

            payPalBag.Items = items;

            return payPalBag;
        }

        public List<ExpressCheckoutItem> LineItems(PayPalBag bag)
        {
            var expressCheckoutItems = new List<ExpressCheckoutItem>();
            if (bag.Items != null)
            {
                expressCheckoutItems = new List<ExpressCheckoutItem>();
                foreach (var item in bag.Items)
                    expressCheckoutItems.Add(new ExpressCheckoutItem(item.Quantity, item.Price,
                        item.Name.ToHtmlFilteredString(true), item.Number));


                //if applicable, add discount line item
                var discount = bag.DiscountAmount;
                if (discount != null && discount != 0)
                {
                    var ddiscount = -1*(decimal) discount;
                    expressCheckoutItems.Add(new ExpressCheckoutItem(1, ddiscount, "Discount", bag.DiscountCode));
                }

                return expressCheckoutItems;
            }

            return null;
        }
    }
}