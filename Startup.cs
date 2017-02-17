using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RegistroJATICS.Startup))]
namespace RegistroJATICS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
