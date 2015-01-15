using System;
using System.Configuration;
using System.Xml;

namespace Elliptical.Mvc.Configuration.Commerce
{
    /// <summary>
    /// </summary>
    public class ShippingConfiguration : ConfigurationElementCollection
    {
        protected override string ElementName
        {
            get { return "shipMethod"; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        public ShipMethod this[int index]
        {
            get { return BaseGet(index) as ShipMethod; }
        }

        public new ShipMethod this[string key]
        {
            get { return BaseGet(key) as ShipMethod; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ShipMethod();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ShipMethod) element).Id;
        }

        protected override bool IsElementName(string elementName)
        {
            return !String.IsNullOrEmpty(elementName) && elementName == "shipMethod";
        }
    }

    /// <summary>
    /// </summary>
    public class ShipMethod : ConfigurationElement
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

        [ConfigurationProperty("descriptionOnly")]
        public bool DescriptionOnly
        {
            get { return Convert.ToBoolean(this["descriptionOnly"]); }
            set { this["descriptionOnly"] = value; }
        }

        [ConfigurationProperty("description")]
        public Description Description
        {
            get { return (Description) this["description"]; }
            set { this["description"] = value; }
        }

        [ConfigurationProperty("priceMatrix")]
        public PriceMatrix PriceMatrix
        {
            get { return (PriceMatrix) this["priceMatrix"]; }
            set { this["priceMatrix"] = value; }
        }

        [ConfigurationProperty("price")]
        public decimal Price
        {
            get { return Convert.ToDecimal(this["price"]); }
            set { this["price"] = value; }
        }

        [ConfigurationProperty("altPriceText")]
        public string AltPriceText
        {
            get { return this["altPriceText"].ToString(); }
            set { this["altPriceText"] = value; }
        }
    }

    /// <summary>
    /// </summary>
    public class Description : ConfigurationElement
    {
        public string Value { get; private set; }
        //allow CDATA
        protected override void DeserializeElement(XmlReader reader, bool s)
        {
            Value = reader.ReadElementContentAs(typeof (string), null) as string;
        }
    }

    /// <summary>
    /// </summary>
    public class PriceMatrix : ConfigurationElementCollection
    {
        protected override string ElementName
        {
            get { return "item"; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        public Item this[int index]
        {
            get { return BaseGet(index) as Item; }
        }

        public new Item this[string key]
        {
            get { return BaseGet(key) as Item; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new Item();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((Item) element).Id;
        }

        protected override bool IsElementName(string elementName)
        {
            return !String.IsNullOrEmpty(elementName) && elementName == "item";
        }
    }

    /// <summary>
    /// </summary>
    public class Item : ConfigurationElement
    {
        [ConfigurationProperty("id", IsRequired = true)]
        public string Id
        {
            get { return this["id"].ToString(); }
            set { this["id"] = value; }
        }

        [ConfigurationProperty("startValue", IsRequired = true)]
        public decimal StartValue
        {
            get { return Convert.ToDecimal(this["startValue"]); }
            set { this["startValue"] = value; }
        }

        [ConfigurationProperty("endValue")]
        public decimal EndValue
        {
            get { return Convert.ToDecimal(this["endValue"]); }
            set { this["endValue"] = value; }
        }

        [ConfigurationProperty("price", IsRequired = true)]
        public decimal Price
        {
            get { return Convert.ToDecimal(this["price"]); }
            set { this["price"] = value; }
        }

        [ConfigurationProperty("label")]
        public string Label
        {
            get { return this["label"].ToString(); }
            set { this["label"] = value; }
        }

        [ConfigurationProperty("altPriceText")]
        public string AltPriceText
        {
            get { return this["altPriceText"].ToString(); }
            set { this["altPriceText"] = value; }
        }
    }
}