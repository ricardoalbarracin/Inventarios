using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(inventarioImportaciones.Startup))]
namespace inventarioImportaciones
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}