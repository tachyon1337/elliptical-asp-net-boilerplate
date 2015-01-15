using System;
using System.Web.Mvc;
using Elliptical.Mvc.Commerce.Models;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    //Decorate the PalPalExpress Return Controller with this attribute
    public class PayPalAuthorized : ActionFilterAttribute
    {
        private const string ckPayPalTrackingRef = "PayPalTrackingRef";
        private const string ckTransaction = "transaction";
        private static readonly TransactionService<string> transactionService = new TransactionService<string>();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            if (string.IsNullOrEmpty(httpContext.User.Identity.Name))
            {
                var redirectUrl = Utils.LoginRedirectUrl(httpContext);
                httpContext.Response.Redirect(redirectUrl);
            }

            var strTransaction = httpContext.Request.Unvalidated.Cookies[ckTransaction].Value;
            var trackingRef = httpContext.Request.Cookies[ckPayPalTrackingRef].Value;
            var transaction = Json.DeserializeObjectSync<Transaction<string>>(strTransaction);
            transaction.PaymentItem.InvoiceNo = trackingRef;
            filterContext.ActionParameters["transaction"] = transaction;
            filterContext.Controller.TempData["transaction"] = transaction;
            //remove transaction cookies
            httpContext.Response.Cookies[ckTransaction].Expires = DateTime.Now.AddDays(-1);
            httpContext.Response.Cookies[ckPayPalTrackingRef].Expires = DateTime.Now.AddDays(-1);

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var token = filterContext.HttpContext.Request.QueryString["token"];
            var transactionResponse = transactionService.SendPayPalGetExpressCheckoutDetailsRequest(token);
            if (transactionResponse == null || transactionResponse.ResponseStatus != ResponseType.Success)
            {
                var errorResponse = (transactionResponse == null)
                    ? "Null Transaction Response"
                    : transactionResponse.ErrorToString;
                var errorMessage = "Error initiating PayPal GetExpressCheckoutDetails transaction. Error: " +
                                   errorResponse;
                throw new Exception(errorMessage);
            }
            base.OnActionExecuted(filterContext);
        }
    }

    //Decorate the PalPalExpress Cancel Controller with this attribute
    public class PayPalCancelled : ActionFilterAttribute
    {
        private const string ckPayPalTrackingRef = "PayPalTrackingRef";
        private const string ckTransaction = "transaction";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var httpContext = filterContext.HttpContext;
            if (string.IsNullOrEmpty(httpContext.User.Identity.Name))
            {
                var redirectUrl = Utils.LoginRedirectUrl(httpContext);
                httpContext.Response.Redirect(redirectUrl);
            }

            var strTransaction = httpContext.Request.Unvalidated.Cookies[ckTransaction].Value;
            var trackingRef = httpContext.Request.Cookies[ckPayPalTrackingRef].Value;
            var transaction = Json.DeserializeObjectSync<Transaction<string>>(strTransaction);

            //remove transaction cookies
            httpContext.Response.Cookies[ckTransaction].Expires = DateTime.Now.AddDays(-1);
            httpContext.Response.Cookies[ckPayPalTrackingRef].Expires = DateTime.Now.AddDays(-1);

            base.OnActionExecuting(filterContext);
        }
    }

    //Controller that handles the "Pay with PayPal" redirect should simply inherit this Controller class
    public class PayPalExpressCheckoutController<T> : Controller
    {
        private const string ckPayPalTrackingRef = "PayPalTrackingRef";
        private const string ckTransaction = "transaction";
        private const string invTrackingPrefix = "PayPal ";
        private static readonly TransactionService<T> transactionService = new TransactionService<T>();

        public virtual ActionResult Index()
        {
            var strTransaction = Request.Unvalidated.Cookies[ckTransaction].Value;
            var transaction = Mvc.Json.DeserializeObjectSync<Transaction<T>>(strTransaction);
            IShoppingBag<T> shoppingBag = transaction.ShoppingBag;
            IAddress<T> address = transaction.ShippingAddress;
            IAddress<T> billing = transaction.BillingAddress;
            var userEmail = billing.Email;
            var payPalBagService = new ShoppingBagService<T>();
            var payPalBag = payPalBagService.GetPayPalBag(shoppingBag);
            var serverUrl = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority);
            var paymentDescription = Configuration.Current.StoreTitle;
            var trackingReference = invTrackingPrefix + Utils.GenerateRandomString(8);
            var transactionResponse = transactionService.SendPayPalSetExpressCheckoutRequest(payPalBag, address,
                serverUrl, userEmail, paymentDescription, trackingReference);
            if (transactionResponse == null || transactionResponse.ResponseStatus != ResponseType.Success)
            {
                var errorMessage = (transactionResponse == null)
                    ? "Null Transaction Response"
                    : transactionResponse.ErrorToString;
                var error = "Error initiating PayPal SetExpressCheckout transaction. Error: " + errorMessage;
                throw new Exception(error);
            }

            //write the trackingRef to a cookie
            Response.Cookies[ckPayPalTrackingRef].Value = trackingReference;
            return Redirect(string.Format(Configuration.Current.PayPalRedirectUrl, transactionResponse.TOKEN));
        }
    }
}