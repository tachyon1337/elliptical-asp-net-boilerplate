using System.Web;
using System.Web.Optimization;
using Elliptical.Mvc.Optimization;
namespace EllipticalTemplate
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true; //overrrides we.config compile setting

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/scripts/jquery.js"
            )
            .IncludeDirectory("~/scripts/jquery","*.js",true)
            );

            bundles.Add(new PlatformBundle());
            bundles.Add(new FrameworkBundle());
            bundles.Add(new CssBundle());
            bundles.Add(new AppBundle());
            
        }
    }
}
