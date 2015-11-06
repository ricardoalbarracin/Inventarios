using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using inventarioImportaciones.Models;
using DevExpress.Web;

namespace InventariosImportaciones.Areas.ADMIN.Controllers
{
    public partial class UserController : Controller
    {
       

        [Authorize]
        public ActionResult Roles()
        {
            return View();
        }


       

        [ValidateInput(false)]
        public ActionResult RolesGridView()
        {
            var model = db.roles_List;
            return PartialView("_RolesGridView", model.ToList());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult RolesGridViewAddNew(inventarioImportaciones.Models.Roles item)
        {
            var model = db.roles_List;
            if (ModelState.IsValid)
            {
                try
                {
                    model.Add(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_RolesGridView", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult RolesGridViewUpdate(inventarioImportaciones.Models.Roles item)
        {
            var model = db.roles_List;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.Id == item.Id);
                    if (modelItem != null)
                    {
                        this.UpdateModel(modelItem);
                        db.SaveChanges();
                    }
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_RolesGridView", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult RolesGridViewDelete(System.String Id)
        {
            var model = db.roles_List;
            if (Id != null)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.Id == Id);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_RolesGridView", model.ToList());
        }
    }
}