using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AttendanceSystemFinal.Startup))]
namespace AttendanceSystemFinal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
