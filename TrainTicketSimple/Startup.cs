using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TrainTicektSimple.Startup))]
namespace TrainTicektSimple
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
