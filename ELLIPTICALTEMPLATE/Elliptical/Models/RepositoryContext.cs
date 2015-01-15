using System.Net.Http;
using System.Web;
using System.Web.Routing;

namespace Elliptical.Mvc
{
    public class RepositoryContext
    {
        //pass in request to get context
        //primarily for web api controllers, which have access to request but not context
        public RepositoryContext(HttpRequestMessage request)
        {
            _setContext(request);
        }

        //overload constructor, pass in context directly
        //Mvc controllers
        public RepositoryContext(HttpContextBase context)
        {
            Context = context;
        }

        //overload constructor
        public RepositoryContext()
        {
            Context = null;
        }

        public HttpContextBase Context { get; set; }
        public RouteData RouteData { get; set; }

        /// <summary>
        ///     get http context
        /// </summary>
        /// <returns></returns>
        public HttpContextBase GetHttpContext()
        {
            return Context;
        }

        /// <summary>
        ///     is authenticated
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated()
        {
            return (!string.IsNullOrEmpty(Context.User.Identity.Name));
        }

        private void _setContext(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                Context = ((HttpContextWrapper) request.Properties["MS_HttpContext"]);
            }
            else if (HttpContext.Current != null)
            {
                Context = new HttpContextWrapper(HttpContext.Current);
            }
            else
            {
                Context = null;
            }
        }
    }
}