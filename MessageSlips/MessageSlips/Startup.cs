using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MessageSlips.Startup))]
namespace MessageSlips
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {

        }
    }
}
