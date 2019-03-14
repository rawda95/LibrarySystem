using System;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using LibraryProject.Identity;
using LibraryProject.Models;
using LibraryProject.IRepository;
//using Unity.AspNet.Mvc;
//using System.Web.Mvc;
using LibraryProject.Repository;

namespace LibraryProject
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
       

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        #endregion

        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }


        private static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ICategoryRepository, CategoryRepository>();
            container.RegisterType<IPublisherRepository, PublisherRepository>();
            container.RegisterType<IAuthorRepository, AuthorRepository>();

            container.RegisterType<ApplicationDbContext>();
            container.RegisterType<ApplicationSignInManager>();
            container.RegisterType<ApplicationUserManager>();
            container.RegisterType<EmailService>();
            //container.RegisterType<AccountController>();
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(c => HttpContext.Current.GetOwinContext().Authentication));

            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(
                new InjectionConstructor(typeof(ApplicationDbContext)));




        }
        public static void RegisterComponents()
        {
            //var container = new UnityContainer();


            //container.RegisterType<ICategoryRepository, CategoryRepository>();
            //container.RegisterType<IPublisherRepository, PublisherRepository>();




            //DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

    }
}