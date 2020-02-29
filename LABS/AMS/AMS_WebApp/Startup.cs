using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ASM_UI.Startup))]
namespace ASM_UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
