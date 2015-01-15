using System;
using System.Configuration;

namespace Elliptical.Mvc.Configuration.Identity
{
    public class IdentitySection : ConfigurationSection
    {
        [ConfigurationProperty("identifierString")]
        public string IdentifierString
        {
            get { return this["identifierString"].ToString(); }
            set { this["identifierString"] = value; }
        }

        [ConfigurationProperty("isAzureWebHost")]
        public bool IsAzureWebHost
        {
            get { return Convert.ToBoolean(this["isAzureWebHost"]); }
            set { this["isAzureWebHost"] = value; }
        }

        [ConfigurationProperty("useAccountApi")]
        public bool UseAccountApi
        {
            get { return Convert.ToBoolean(this["useAccountApi"]); }
            set { this["useAccountApi"] = value; }
        }

        [ConfigurationProperty("enableTwoFactorAuth")]
        public bool EnableTwoFactorAuth
        {
            get { return Convert.ToBoolean(this["enableTwoFactorAuth"]); }
            set { this["enableTwoFactorAuth"] = value; }
        }

        [ConfigurationProperty("enableOAuth")]
        public bool EnableOAuth
        {
            get { return Convert.ToBoolean(this["enableOAuth"]); }
            set { this["enableOAuth"] = value; }
        }

        [ConfigurationProperty("confirmEmail")]
        public bool ConfirmEmail
        {
            get { return Convert.ToBoolean(this["confirmEmail"]); }
            set { this["confirmEmail"] = value; }
        }


        [ConfigurationProperty("validation")]
        public Validation Validation
        {
            get { return (Validation) this["validation"]; }
            set { this["validation"] = value; }
        }

        [ConfigurationProperty("messages")]
        public Messages Messages
        {
            get { return (Messages) this["messages"]; }
            set { this["messages"] = value; }
        }

       
    }

    public class Validation : ConfigurationElement
    {
        
        [ConfigurationProperty("settings")]
        public Settings Settings
        {
            get { return (Settings) this["settings"]; }
            set { this["settings"] = value; }
        }
    }

    public class Messages : ConfigurationElement
    {
        [ConfigurationProperty("settings")]
        public Settings Settings
        {
            get { return (Settings) this["settings"]; }
            set { this["settings"] = value; }
        }
    }

    public class ServiceProviders : ConfigurationElementCollection
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

   

   
}