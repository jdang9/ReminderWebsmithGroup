using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReminderWG.Startup))]
namespace ReminderWG
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
