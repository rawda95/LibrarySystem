using LibraryProject.Identity;
using LibraryProject.IRepository;
using LibraryProject.Repository;
using Microsoft.Owin;
using Owin;
//using static LibraryProject.ApplicationSignInManager;

[assembly: OwinStartupAttribute(typeof(LibraryProject.Startup))]
namespace LibraryProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //ApplicationRoleManager.CreateRoles();

        }
    }
}
