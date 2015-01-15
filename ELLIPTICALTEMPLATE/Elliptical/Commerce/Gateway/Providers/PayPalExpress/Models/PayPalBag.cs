using System.Collections.Generic;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    public class PayPalBag
    {
        public object Id { get; set; }
        public string Currency { get; set; }
        public string PurchaseDescription { get; set; }
        public List<PayPalBagItem> Items { get; set; }
        public decimal ItemAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string DiscountCode { get; set; }
    }

    public class PayPalBagItem
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
    }

    /// <summary>
    ///     NVP Field Format of a checkout item
    /// </summary>
    public class ExpressCheckoutItem : IExpressCheckoutItem
    {
        private readonly decimal amount;
        private readonly string name;
        private readonly string number;
        private readonly int quantity;

        public ExpressCheckoutItem(int quantity, decimal amount, string name, string number = null)
        {
            this.quantity = quantity;
            this.amount = amount;
            this.name = name;
            this.number = number;
        }

        public ExpressCheckoutItem(PayPalBagItem item)
        {
            quantity = item.Quantity;
            amount = item.Price;
            name = item.Name;
            number = item.Number;
        }

        // Note "_mIndex" gets replaced with the item number when this object gets serialized
        public string L_PAYMENTREQUEST_0_QTY_mIndex
        {
            get { return quantity.ToString(); }
        }

        public string L_PAYMENTREQUEST_0_AMT_mIndex
        {
            get { return amount.ToString("f2"); }
        }

        public string L_PAYMENTREQUEST_0_NAME_mIndex
        {
            get { return name; }
        }

        /// <summary>
        ///     Optional Item description
        /// </summary>
        public string L_PAYMENTREQUEST_0_NUMBER_mIndex
        {
            get { return number; }
        }
    }
}