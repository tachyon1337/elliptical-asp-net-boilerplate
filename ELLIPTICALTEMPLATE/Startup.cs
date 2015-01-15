using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(EllipticalTemplate.Startup))]
namespace EllipticalTemplate
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
