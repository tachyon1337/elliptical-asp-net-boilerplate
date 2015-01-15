using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using EllipticalTemplate.Identity;
using EllipticalTemplate.Models;

namespace EllipticalTemplate
{
    public class IdentityInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                        .BasedOn(typeof(ApplicationDbContext))
                        .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                        .BasedOn(typeof(ApplicationSignInManager))
                        .WithServiceAllInterfaces());

            container.Register(Classes.FromThisAssembly()
                       .BasedOn(typeof(ApplicationUserManager))
                       .WithServiceAllInterfaces());

            container.Register(Component.For().UsingFactoryMethod((kernel, creationContext) => HttpContext.Current.GetOwinContext().Authentication).LifestylePerWebRequest());

            
        }
    }
}