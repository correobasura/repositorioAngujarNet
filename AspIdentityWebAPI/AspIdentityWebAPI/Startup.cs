using AspIdentityWebAPI.Infraestructura;
using Newtonsoft.Json.Serialization;
using Owin;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;

namespace AspIdentityWebAPI
{
    /// <summary>
    /// Clase encargada de establecer los parámetros iniciales, necesarios para la aplicación
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Método que realiza la configuración inicial de la aplicación
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration httpConfig = new HttpConfiguration();
            ConfigureOAuthTokenGeneration(app);
            ConfigureWebApi(httpConfig);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(httpConfig);
        }

        /// <summary>
        /// Método que realiza la creación de una nueva instancia de <AppDBContext> y <ManagerUserIdentity>
        /// por cada petición
        /// </summary>
        /// <param name="app">´Parámetro recibido en tiempo de ejecución</param>
        private void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(AppDBContext.Create);
            app.CreatePerOwinContext<ManagerUserIdentity>(ManagerUserIdentity.Create);

            // Plugin the OAuth bearer JSON Web Token tokens generation and Consumption will be here

        }

        /// <summary>
        /// Método que realiza la configuración de las rutas de webapi
        /// </summary>
        /// <param name="config">HttpConfiguration</param>
        private void ConfigureWebApi(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}