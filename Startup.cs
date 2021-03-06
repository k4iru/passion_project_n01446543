using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(card_app.Startup))]
namespace card_app
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
