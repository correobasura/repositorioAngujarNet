using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AppEjemploAngularBasico.Startup))]
namespace AppEjemploAngularBasico
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
