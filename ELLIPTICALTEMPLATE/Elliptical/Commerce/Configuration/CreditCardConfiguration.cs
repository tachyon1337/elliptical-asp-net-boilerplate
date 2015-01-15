using System;
using System.Configuration;

namespace Elliptical.Mvc.Configuration.Commerce
{
    public class CreditCardConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("encryptLastTransaction")]
        public bool EncryptLastTransaction
        {
            get { return Convert.ToBoolean(this["encryptLastTransaction"]); }
            set { this["encryptLastTransaction"] = value; }
        }

        [ConfigurationProperty("encryptionKey")]
        public string EncryptionKey
        {
            get { return this["encryptionKey"].ToString(); }
            set { this["encryptionKey"] = value; }
        }

        [ConfigurationProperty("acceptedCards")]
        public AcceptedCards AcceptedCards
        {
            get { return (AcceptedCards) this["acceptedCards"]; }
            set { this["acceptedCards"] = value; }
        }
    }


    /// <summary>
    /// </summary>
    public class AcceptedCards : ConfigurationElementCollection
    {
        protected override string ElementName
        {
            get { return "card"; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        public Card this[int index]
        {
            get { return BaseGet(index) as Card; }
        }

        public new Card this[string key]
        {
            get { return BaseGet(key) as Card; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Card();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Card) element).Id;
        }

        protected override bool IsElementName(string elementName)
        {
            return !String.IsNullOrEmpty(elementName) && elementName == "card";
        }
    }

    /// <summary>
    /// </summary>
    public class Card : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get { return this["id"].ToString(); }
            set { this["id"] = value; }
        }

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("redirectAction")]
        public string RedirectAction
        {
            get { return this["redirectAction"].ToString(); }
            set { this["redirectAction"] = value; }
        }
    }
}