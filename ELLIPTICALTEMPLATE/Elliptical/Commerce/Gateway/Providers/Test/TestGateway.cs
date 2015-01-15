using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.Test
{
    public class TestGateway<T> : PaymentGateway<T>
    {
        public TestGateway() : base() { }

        public override TransactionResult charge()
        {
            //
            this.AuthCode = Utils.GenerateRandomString(6);
            this.TransactionId = Utils.GenerateRandomString(10);
            return TransactionResult.Approved;
        }

       

    }
}