using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VaultSystem.Startup))]
namespace VaultSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
