using System;
using System.Configuration;

namespace Elliptical.Mvc.Configuration.Commerce
{
    public class PaymentGatewayConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("mode")]
        public string Mode
        {
            get { return this["mode"].ToString(); }
            set { this["mode"] = value; }
        }

        [ConfigurationProperty("id")]
        public string Id
        {
            get { return this["id"].ToString(); }
            set { this["id"] = value; }
        }

        [ConfigurationProperty("providers")]
        public Providers Providers
        {
            get { return (Providers) this["providers"]; }
            set { this["providers"] = value; }
        }
    }

    public class Providers : ConfigurationElementCollection
    {
        protected override string ElementName
        {
            get { return "provider"; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        public Provider this[int index]
        {
            get { return BaseGet(index) as Provider; }
        }

        public new Provider this[string key]
        {
            get { return BaseGet(key) as Provider; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Provider();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Provider) element).Id;
        }

        protected override bool IsElementName(string elementName)
        {
            return !String.IsNullOrEmpty(elementName) && elementName == "provider";
        }
    }

    public class Provider : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get { return this["id"].ToString(); }
            set { this["id"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return this["type"].ToString(); }
            set { this["type"] = value; }
        }

        [ConfigurationProperty("processedPaymentTypes")]
        public string ProcessedPaymentTypes
        {
            get { return this["processedPaymentTypes"].ToString(); }
            set { this["processedPaymentTypes"] = value; }
        }

        [ConfigurationProperty("forMode")]
        public string ForMode
        {
            get { return this["forMode"].ToString(); }
            set { this["forMode"] = value; }
        }

        [ConfigurationProperty("settings")]
        public Settings Settings
        {
            get { return (Settings) this["settings"]; }
            set { this["settings"] = value; }
        }
    }

    public class Settings : ConfigurationElementCollection
    {
        protected override string ElementName
        {
            get { return "add"; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        public Add this[int index]
        {
            get { return BaseGet(index) as Add; }
        }

        public new Add this[string key]
        {
            get { return BaseGet(key) as Add; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Add();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Add) element).Key;
        }

        protected override bool IsElementName(string elementName)
        {
            return !String.IsNullOrEmpty(elementName) && elementName == "add";
        }
    }

    public class Add : ConfigurationElement
    {
        [ConfigurationProperty("key", IsRequired = true)]
        public string Key
        {
            get { return this["key"].ToString(); }
            set { this["key"] = value; }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public string Value
        {
            get { return this["value"].ToString(); }
            set { this["value"] = value; }
        }
    }
}