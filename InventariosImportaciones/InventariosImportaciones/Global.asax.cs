using App.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace InventariosImportaciones
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AuthConfig.RegisterAuth();
            BundleConfig.RegisterBundles(BundleTable.Bundles); 
            ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();

            DevExpress.Web.ASPxWebControl.CallbackError += Application_Error;
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            // Configuraci�n inicial de seguridad
            SecurityConfig.RegisterSecurity();
            // Configuraci�n parametros globales de la aplicaci�n
           // GeneralConfig.RegisterGeneral();
            // Configuraci�n de procedimientos almacenados en la base de datos
            //DynamicDALConfig.RegisterSps();
            // Configuraci�n de los mensajes que se utilizaran en la aplicaci�n
            //MessageConfig.RegisterMessages("ES");

            // Asegura que ningun usuario este autenticado
            //Response.RedirectToRoute("LogOff");
        }

        protected void Application_Error(object sender, EventArgs e) 
        {
            Exception exception = System.Web.HttpContext.Current.Server.GetLastError();

            //TODO: Handle Exception
        }
    }
}