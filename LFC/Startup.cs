using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LFC.Startup))]
namespace LFC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
