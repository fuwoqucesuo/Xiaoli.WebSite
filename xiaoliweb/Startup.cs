using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(xiaoliweb.Startup))]
namespace xiaoliweb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
