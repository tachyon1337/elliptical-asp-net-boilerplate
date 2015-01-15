using System;
using System.Collections.Generic;

namespace Elliptical.Mvc.Commerce.Models
{
    public interface IAddress<T>
    {
        T Id { get; set; }
        T UserId { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Phone { get; set; }
        string Phone2 { get; set; }
        string Street { get; set; }
        string Street2 { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Zip { get; set; }
    }

    public interface IShoppingBag<T>
    {
        T Id { get; set; }
        T UserId { get; set; }
        decimal Subtotal { get; set; }
        decimal? Discount { get; set; }
        decimal? Tax { get; set; }
        decimal? Shipping { get; set; }
        decimal Total { get; set; }
        string Code { get; set; }
        string Notes { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
        List<BagItem<T>> Items { get; set; }
        string HeaderLabel { get; set; }
    }

    public interface IBagItem<T>
    {
        T Id { get; set; }
        string Sku { get; set; }
        string Name { get; set; }
        string ImgUrl { get; set; }
        string Url { get; set; }
        decimal Price { get; set; }
        int Quantity { get; set; }
        decimal Subtotal { get; set; }
        int VariantId { get; set; }
        string Note { get; set; }
        List<BagItemDescription> Descriptions { get; set; }
    }

    public interface IBagItemDescription
    {
        string Label { get; set; }
        string Text { get; set; }
    }

    public interface IBagInsert<T>
    {
        T ProductId { get; set; }
        List<BagItemDescription> Descriptions { get; set; }
        int Quantity { get; set; }
        string ProductUrl { get; set; }
    }

    public interface IBagUpdate<T>
    {
        T Id { get; set; }
        int Quantity { get; set; }
    }

    public interface IShippingMethod
    {
        string Id { get; set; }
        string Name { get; set; }
        decimal Price { get; set; }
        string Description { get; set; }
        string AltPriceText { get; set; }
        List<GenericItem> Items { get; set; }
        bool DescriptionOnly { get; set; }
    }

    public interface IShippingItem
    {
        bool Shipped { get; set; }
        DateTime ShippedDate { get; set; }
        DateTime DeliveryDate { get; set; }
        DateTime ArrivalDate { get; set; }
    }

    public interface ICreditCard
    {
        string Id { get; set; }
        string Label { get; set; }
        string RedirectAction { get; set; }
    }

    public interface ICreditCardItem
    {
        int ExpMonth { get; set; }
        int ExpYear { get; set; }
        string CVVCode { get; set; }
        string NameOnCard { get; set; }
        string CardNumber { get; set; }
    }

    public interface IPaymentItem<T>
    {
        T OrderId { get; set; }
        string OrderDate { get; set; }
        string AuthCode { get; set; }
        string TransactionId { get; set; }
        string PaymentProvider { get; set; }
        decimal PaymentAmount { get; set; }
        decimal TransactionFee { get; set; }
        string CardType { get; set; }
        string FailureCode { get; set; }
        string FailureReason { get; set; }
        string Token { get; set; }
        string PayerId { get; set; }
    }

    public interface IPromotionItem
    {
        string Code { get; set; }
        decimal Discount { get; set; }
    }

    public interface IPromotionItems
    {
        decimal Total { get; set; }
        List<PromotionItem> Items { get; set; }
    }

    public interface ITaxRate
    {
        string Province { get; set; }
        decimal Rate { get; set; }
    }

    public interface ITaxItem
    {
        decimal TaxableAmount { get; set; }
        decimal Tax { get; set; }
    }

    public interface IGenericItem
    {
        string Label { get; set; }
        string Value { get; set; }
    }

    public interface ICheckoutModel<T>
    {
        string Id { get; set; }
        Address<T> BillingAddress { get; set; }
        List<Address<T>> ShippingAddresses { get; set; }
        List<ShippingMethod> ShippingMethod { get; set; }
        ShoppingBag<T> ShoppingBag { get; set; }
        List<CreditCard> AcceptedCreditCards { get; set; }
        ITransaction<T> Transaction { get; set; }
    }

    public interface ITransaction<T>
    {
        ShoppingBag<T> ShoppingBag { get; set; }
        Address<T> BillingAddress { get; set; }
        Address<T> ShippingAddress { get; set; }
        ShippingItem ShippingItem { get; set; }
        CreditCardItem CreditCardItem { get; set; }
        TaxItem TaxItem { get; set; }
        PaymentItem<T> PaymentItem { get; set; }
        bool Approved { get; set; }
        bool AllowLastTransactionCard { get; set; }
        bool SubmitLastTransactionCard { get; set; }
    }

    public interface IOrder<T>
    {
        T Id { get; set; }
        string UserId { get; set; }
        decimal Subtotal { get; set; }
        decimal Discount { get; set; }
        decimal Shipping { get; set; }
        decimal Tax { get; set; }
        decimal Total { get; set; }
        DateTime OrderDate { get; set; }
        string OrderStatus { get; set; }
        string Code { get; set; }
        string Notes { get; set; }
        string ShipDate { get; set; }
        Address<T> BillingAddress { get; set; }
        Address<T> ShippingAddress { get; set; }
        List<BagItem<T>> Items { get; set; }
        ShippingItem ShippingItem { get; set; }
        CreditCardItem CreditCardItem { get; set; }
        PaymentItem<T> PaymentItem { get; set; }
        PromotionItems PromotionItems { get; set; }
        TaxItem TaxItem { get; set; }
    }
}