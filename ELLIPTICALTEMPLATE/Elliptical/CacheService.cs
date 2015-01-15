using System;
using System.Collections;
using System.Net.Http;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;

namespace Elliptical.Mvc
{
    public class CacheService
    {
        public CacheService(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                context = ((HttpContextWrapper) request.Properties["MS_HttpContext"]);
            }
            else if (HttpContext.Current != null)
            {
                context = new HttpContextWrapper(HttpContext.Current);
            }
            else
            {
                context = null;
            }
        }

        public CacheService(HttpContextBase context)
        {
            this.context = context;
        }

        private HttpContextBase context { get; set; }

        public T Get<T>(string key)
        {
            return (T) context.Cache[key];
        }

        public void Set<T>(string key, T model)
        {
            var t = Convert.ToDouble(WebConfigurationManager.AppSettings["Elliptical.Cache.Expiration"]);
            if (t > 0)
            {
                context.Cache.Insert(key, model, null, DateTime.Now.AddMinutes(t), Cache.NoSlidingExpiration);
            }
            else
            {
                context.Cache[key] = model;
            }
        }

        public static void Set<T>(HttpContextBase context, string key, T model)
        {
            var t = Convert.ToDouble(WebConfigurationManager.AppSettings["Elliptical.Cache.Expiration"]);
            if (t > 0)
            {
                context.Cache.Insert(key, model, null, DateTime.Now.AddMinutes(t), Cache.NoSlidingExpiration);
            }
            else
            {
                context.Cache[key] = model;
            }
        }

        public void Clear()
        {
            var cache = context.Cache;
            foreach (DictionaryEntry e in cache)
            {
                cache.Remove(e.Key.ToString());
            }
        }
    }
}