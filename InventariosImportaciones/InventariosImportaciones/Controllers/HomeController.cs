using App.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventariosImportaciones.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            bool r = User.HasPermission("PB");
            if (r)
                Console.WriteLine("");
            return View();    
            
        }
       
        
    }
}