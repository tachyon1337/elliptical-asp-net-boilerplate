using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Elliptical.Mvc.Commerce.Models
{
    public class ShoppingBag<T> : IShoppingBag<T>
    {
        public ShoppingBag()
        {
            CreatedAt = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            UpdatedAt = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            Items = new List<BagItem<T>>();
            Subtotal = 0;
            Tax = null;
            Shipping = null;
            Discount = null;
            Total = 0;
            Code = "";
            Notes = "";
            HeaderLabel = "Shopping Bag";
        }

        public T Id { get; set; }
        public T UserId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal? Shipping { get; set; }
        public decimal Total { get; set; }
        public string Code { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<BagItem<T>> Items { get; set; }
        public string HeaderLabel { get; set; }
    }


    public class BagItem<T> : IBagItem<T>
    {
        public BagItem()
        {
            Descriptions = new List<BagItemDescription>();
        }

        public T Id { get; set; }
        public string Sku { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public string Url { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
        public int VariantId { get; set; }
        public string Note { get; set; }
        public List<BagItemDescription> Descriptions { get; set; }
    }


    public class BagItemDescription : IBagItemDescription
    {
        public string Label { get; set; }
        public string Text { get; set; }
    }

    public class BagInsert<T> : IBagInsert<T>
    {
        public T ProductId { get; set; }
        public List<BagItemDescription> Descriptions { get; set; }
        public int Quantity { get; set; }
        public string ProductUrl { get; set; }
    }

    public class BagUpdate<T> : IBagUpdate<T>
    {
        public T Id { get; set; }
        public int Quantity { get; set; }
    }

    public enum BagType
    {
        ShoppingBag,
        Wishlist
    }

    public static class BagLabel
    {
        public static string label(BagType bagType)
        {
            return (bagType == BagType.ShoppingBag) ? "Bag" : "Wishlist";
        }
    }

    public class CheckoutModel<T> : ICheckoutModel<T>
    {
        public CheckoutModel()
        {
            ShippingAddresses = new List<Address<T>>();
            ShoppingBag = new ShoppingBag<T>();
            AcceptedCreditCards = new List<CreditCard>();
            Transaction = new Transaction<T>();
        }

        public string Id { get; set; }
        public Address<T> BillingAddress { get; set; }
        public List<Address<T>> ShippingAddresses { get; set; }
        public List<ShippingMethod> ShippingMethod { get; set; }
        public ShoppingBag<T> ShoppingBag { get; set; }
        public List<CreditCard> AcceptedCreditCards { get; set; }
        public ITransaction<T> Transaction { get; set; }
    }

    [DataContract]
    public class Transaction<T> : ITransaction<T>
    {
        public Transaction()
        {
            ShippingItem = new ShippingItem();
            CreditCardItem = new CreditCardItem();
            TaxItem = new TaxItem();
            PaymentItem = new PaymentItem<T>();
            Approved = false;
            AllowLastTransactionCard = false;
            SubmitLastTransactionCard = false;
        }

        [DataMember]
        public ShoppingBag<T> ShoppingBag { get; set; }

        [DataMember]
        public Address<T> BillingAddress { get; set; }

        [DataMember]
        public Address<T> ShippingAddress { get; set; }

        [DataMember]
        public ShippingItem ShippingItem { get; set; }

        [DataMember]
        public CreditCardItem CreditCardItem { get; set; }

        [DataMember]
        public TaxItem TaxItem { get; set; }

        [DataMember]
        public PaymentItem<T> PaymentItem { get; set; }

        [DataMember]
        public bool Approved { get; set; }

        [DataMember]
        public bool AllowLastTransactionCard { get; set; }

        [DataMember]
        public bool SubmitLastTransactionCard { get; set; }
    }


    public class Address<T> : IAddress<T>
    {
        public T Id { get; set; }
        public T UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Street { get; set; }
        public string Street2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }


    public class ShippingMethod : IShippingMethod
    {
        public ShippingMethod()
        {
            Items = new List<GenericItem>();
            DescriptionOnly = false;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string AltPriceText { get; set; }
        public List<GenericItem> Items { get; set; }
        public bool DescriptionOnly { get; set; }
    }

    public class ShippingItem : ShippingMethod, IShippingItem
    {
        public bool Shipped { get; set; }
        public DateTime ShippedDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime ArrivalDate { get; set; }
    }

    public class CreditCard : ICreditCard
    {
        public CreditCard()
        {
            RedirectAction = null;
        }

        public string Id { get; set; }
        public string Label { get; set; }
        public string RedirectAction { get; set; }
    }


    public class CreditCardItem : CreditCard, ICreditCardItem
    {
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public string CVVCode { get; set; }
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }

        public void Encrypt()
        {
            CardNumber = ApplicationCreditCard.Encrypt(CardNumber);
        }

        public void decrypt()
        {
            CardNumber = ApplicationCreditCard.Decrypt(CardNumber);
        }

        public bool isValidDate()
        {
            var month = ExpMonth;
            var year = ExpYear;
            if (month != null && year != null)
            {
                var dt = Convert.ToDateTime(month + "/" + year);
                return (dt > DateTime.Now);
            }
            return false;
        }
    }

    public class PaymentItem<T> : IPaymentItem<T>
    {
        public string InvoiceNo { get; set; }
        public T OrderId { get; set; }
        public string OrderDate { get; set; }
        public string AuthCode { get; set; }
        public string TransactionId { get; set; }
        public string PaymentProvider { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal TransactionFee { get; set; }
        public string CardType { get; set; }
        public string FailureCode { get; set; }
        public string FailureReason { get; set; }
        public string Token { get; set; }
        public string PayerId { get; set; }
    }

    public class PromotionItem : IPromotionItem
    {
        public string Code { get; set; }
        public decimal Discount { get; set; }
    }


    public class PromotionItems : IPromotionItems
    {
        public PromotionItems()
        {
            Total = 0;
            Items = new List<PromotionItem>();
        }

        public decimal Total { get; set; }
        public List<PromotionItem> Items { get; set; }
    }

    public class TaxRate : ITaxRate
    {
        public string Province { get; set; }
        public decimal Rate { get; set; }
    }

    public class TaxItem : TaxRate, ITaxItem
    {
        public decimal TaxableAmount { get; set; }
        public decimal Tax { get; set; }
    }

    public class Order<T> : IOrder<T>
    {
        public Order()
        {
            BillingAddress = new Address<T>();
            ShippingAddress = new Address<T>();
            Items = new List<BagItem<T>>();
            ShippingItem = new ShippingItem();
            CreditCardItem = new CreditCardItem();
            PaymentItem = new PaymentItem<T>();
            PromotionItems = new PromotionItems();
            TaxItem = new TaxItem();
        }

        public T Id { get; set; }
        public string UserId { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string Code { get; set; }
        public string Notes { get; set; }
        public string ShipDate { get; set; }
        public Address<T> BillingAddress { get; set; }
        public Address<T> ShippingAddress { get; set; }
        public List<BagItem<T>> Items { get; set; }
        public ShippingItem ShippingItem { get; set; }
        public CreditCardItem CreditCardItem { get; set; }
        public PaymentItem<T> PaymentItem { get; set; }
        public PromotionItems PromotionItems { get; set; }
        public TaxItem TaxItem { get; set; }
    }

    public class GenericItem : IGenericItem
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }
}