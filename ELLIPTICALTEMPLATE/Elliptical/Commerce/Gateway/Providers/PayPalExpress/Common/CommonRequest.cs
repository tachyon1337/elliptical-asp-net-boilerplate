﻿using System;

namespace Elliptical.Mvc.Commerce.Gateway.Providers.PayPalExpress
{
    /// <summary>
    ///     All requests must contain these fields
    /// </summary>
    public abstract class CommonRequest
    {
        protected RequestType method { get; set; }

        public string VERSION
        {
            get { return Configuration.Version; }
        }

        public string METHOD
        {
            get { return method.ToString(); }
            set { method = (RequestType) Enum.Parse(typeof (RequestType), value); }
        }

        public string USER
        {
            get { return Configuration.Current.MerchantUserName; }
        }

        public string PWD
        {
            get { return Configuration.Current.MerchantPassword; }
        }

        public string SIGNATURE
        {
            get { return Configuration.Current.Signature; }
        }
    }
}