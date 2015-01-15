using System.Collections.Generic;
using System.Collections.Specialized;
using Elliptical.Mvc.Commerce.Models;

namespace Elliptical.Mvc.Commerce.Gateway
{
    /// <summary>
    /// </summary>
    public interface ICardTransaction<T>
    {
        CreditCardItem CardItem { get; set; }
        T UserId { get; set; }
        CreditCardItem GetCard();
    }

    /// <summary>
    /// </summary>
    public interface IPaymentGateway<T>
    {
        ITransaction<T> Transaction { get; set; }
        object UserId { get; set; }
        decimal Amount { get; set; }
        string CardNumber { get; set; }
        string CardProvider { get; set; }
        string CCCode { get; set; }
        string sCardType { get; set; }
        CreditCardType CardType { get; set; }
        int Month { get; set; }
        int Year { get; set; }
        string Address { get; set; }
        string City { get; set; }
        string StateProvince { get; set; }
        string Country { get; set; }
        string ZipPostal { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Login { get; set; }
        string Password { get; set; }
        string TransactionKey { get; set; }
        string AuthCode { get; set; }
        string TransactionId { get; set; }
        string ErrorCode { get; set; }
        string ErrorMessage { get; set; }
        string LogMessage { get; set; }
        string Mode { get; set; }
        string Phone { get; set; }
        bool Validate { get; set; }
        bool Log { get; set; }
        TransactionType TransactionType { get; set; }
        NameValueCollection Settings { get; set; }
        Dictionary<string, string> RequestFields { get; set; }
        Dictionary<string, string> ResponseFields { get; set; }
        object PaymentTransaction { get; set; }
        TransactionResult charge();
    }

    public interface IClient<T>
    {
        IPaymentGateway<T> resolveType();
    }
}