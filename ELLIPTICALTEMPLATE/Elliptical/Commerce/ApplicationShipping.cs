using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Elliptical.Mvc.Commerce.Models;
using Elliptical.Mvc.Configuration.Commerce;

namespace Elliptical.Mvc.Commerce
{
    public static class ApplicationShipping
    {
        private static readonly CommerceSection config =
            ConfigurationManager.GetSection("ellipticalCommerce") as CommerceSection;

        /// <summary>
        ///     get list of shipping methods
        /// </summary>
        /// <returns></returns>
        public static List<ShippingMethod> GetShippingMethods()
        {
            var methods = new List<ShippingMethod>();
            var shipping = config.ShippingConfiguration;
            var shipMethods = shipping.OfType<ShipMethod>();
            foreach (var sm in shipMethods)
            {
                var shipMethod = new ShippingMethod();
                shipMethod.Id = sm.Id;
                shipMethod.Name = sm.Name;
                shipMethod.Description = sm.Description.Value;
                if (sm.PriceMatrix.Count > 0)
                {
                    var items_ = sm.PriceMatrix.OfType<Item>();
                    shipMethod.Items = priceMatrix(items_);
                }
                else
                {
                    shipMethod.AltPriceText = sm.AltPriceText;
                }
                if (sm.DescriptionOnly)
                {
                    shipMethod.DescriptionOnly = sm.DescriptionOnly;
                }

                methods.Add(shipMethod);
            }

            return methods;
        }
       
       /// <summary>
        /// overload method, returns priced shipping method list(where applicable, a shipping method is assigned a price, not a
        ///     price matrix)  by shopping bag subtotal
       /// </summary>
       /// <param name="bagTotal"></param>
       /// <returns></returns>
        public static List<ShippingMethod> GetShippingMethods(decimal bagTotal)
        {
            var methods = new List<ShippingMethod>();
            var shipping = config.ShippingConfiguration;
            var shipMethods = getPricedShippingMethods(shipping.OfType<ShipMethod>());

            foreach (var sm in shipMethods)
            {
                var shipMethod = new ShippingMethod {Id = sm.Id, Name = sm.Name, Description = sm.Description.Value};
                if (sm.PriceMatrix.Count > 0)
                {
                    var items_ = sm.PriceMatrix.OfType<Item>();
                    var shipPrice = getMethodPriceByPriceMatrix(items_, bagTotal);
                    shipMethod.AltPriceText = shipPrice.AltPriceText;
                    shipMethod.Price = shipPrice.Price;
                }
                else
                {
                    shipMethod.AltPriceText = sm.AltPriceText;
                    shipMethod.Price = sm.Price;
                }

                methods.Add(shipMethod);
            }

            return methods;
        }

        /// <summary>
        /// </summary>
        /// <param name="pm"></param>
        /// <returns></returns>
        private static List<GenericItem> priceMatrix(IEnumerable<Item> pm)
        {
            return pm.Select(itm => new GenericItem {Label = itm.Label, Value = itm.AltPriceText}).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="methods"></param>
        /// <returns></returns>
        private static List<ShipMethod> getPricedShippingMethods(IEnumerable<ShipMethod> methods)
        {
            return methods.Where(m => !m.DescriptionOnly).ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <param name="bagTotal"></param>
        /// <returns></returns>
        private static ShipPrice getMethodPriceByPriceMatrix(IEnumerable<Item> items, decimal bagTotal)
        {
            var shipPrice = new ShipPrice();
            foreach (var item in items)
            {
                var startValue = item.StartValue;
                decimal endValue;
                if (item.GetType().GetProperty("EndValue") != null)
                {
                    endValue = item.EndValue;
                    if (endValue == 0)
                    {
                        endValue = Decimal.MaxValue;
                    }
                }
                else
                {
                    endValue = Decimal.MaxValue;
                }

                if (bagTotal >= startValue && bagTotal <= endValue)
                {
                    shipPrice.Price = item.Price;
                    shipPrice.AltPriceText = item.AltPriceText;

                    break;
                }
            }

            return shipPrice;
        }
    }

    internal class ShipPrice
    {
        public string AltPriceText { get; set; }
        public decimal Price { get; set; }
    }
}