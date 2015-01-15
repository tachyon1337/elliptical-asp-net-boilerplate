using System;
using System.Web;

namespace Elliptical.Mvc
{
    public class SerializedHttpCookie<T>
    {
        public string Name { get; set; }
        public DateTime Expires { get; set; }
        public T Value { get; set; }

        public SerializedHttpCookie(string name, T value)
        {
            this.Name = name;
            this.Value = value;
        }

        public SerializedHttpCookie(string name, T value, DateTime expires)
        {
            this.Name = name;
            this.Value = value;
            this.Expires = expires;
        }
    }
}