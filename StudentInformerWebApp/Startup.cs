using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(StudentInformerWebApp.Startup))]
namespace StudentInformerWebApp
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
