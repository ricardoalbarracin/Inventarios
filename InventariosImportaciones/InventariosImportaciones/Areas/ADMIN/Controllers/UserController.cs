using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using inventarioImportaciones.Models;
using DevExpress.Web;
using App.Security;

namespace InventariosImportaciones.Areas.ADMIN.Controllers
{
    public partial class UserController : Controller
    {
        public enum EnumBotonesGrid : int
        {
            None = 0,
            Edit = 1,
            Add = 2,
            Delete = 4,
            All = 255
        }

        // GET: ADMIN/User
        [HasPermission("leo")]
        public ActionResult Usuarios()
        {
            return View();
        }

        inventarioImportaciones.Models.CCDBContext db = new inventarioImportaciones.Models.CCDBContext();

        [ValidateInput(false)]
        public ActionResult UsuariosGridView()
        {
            var model = db.usuarios_List;
            List<Usuarios> listusers = model.ToList();
            return PartialView("_UsuariosGridView", listusers);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UsuariosGridViewAddNew(inventarioImportaciones.Models.Usuarios item)
        {
            var model = db.usuarios_List;
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
            return PartialView("_UsuariosGridView", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UsuariosGridViewUpdate(inventarioImportaciones.Models.Usuarios item)
        {
            var model = db.usuarios_List;
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
            return PartialView("_UsuariosGridView", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult UsuariosGridViewDelete(string Id)
        {
            var model = db.usuarios_List;
            
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
            
            return PartialView("_UsuariosGridView", model.ToList());
        }

        public static void ConfigurarGridView(GridViewSettings settings, bool isAdmin, string nameSummary, bool ShowFilterRow = true, bool ShowGroupPanel = true, System.Web.Mvc.ViewDataDictionary ViewData = null, int botones = 255, string toolTipBotonEliminar = "Eliminar", bool isMasterGrid = true)
        {
            try
            {
                settings.SettingsCommandButton.NewButton.ButtonType = GridCommandButtonRenderMode.Image;
                settings.SettingsCommandButton.NewButton.Image.IconID = DevExpress.Web.ASPxThemes.IconID.EditNew16x16gray;
                settings.SettingsCommandButton.EditButton.ButtonType = GridCommandButtonRenderMode.Image;
                settings.SettingsCommandButton.EditButton.Image.IconID = DevExpress.Web.ASPxThemes.IconID.EditEdit16x16gray;
                settings.SettingsCommandButton.DeleteButton.ButtonType = GridCommandButtonRenderMode.Image;
                settings.SettingsCommandButton.DeleteButton.Image.IconID = DevExpress.Web.ASPxThemes.IconID.ActionsDeleteitem16x16gray;
                settings.SettingsCommandButton.UpdateButton.Image.IconID = DevExpress.Web.ASPxThemes.IconID.ActionsSave32x32devav;
                settings.SettingsCommandButton.CancelButton.Image.IconID = DevExpress.Web.ASPxThemes.IconID.ActionsCancel32x32gray;

                settings.Settings.VerticalScrollBarMode = ScrollBarMode.Auto;
                settings.Width = System.Web.UI.WebControls.Unit.Percentage(100);
              
                settings.SettingsPager.PageSize = 50;
                settings.SettingsPager.NumericButtonCount = 6;
                settings.SettingsPager.EnableAdaptivity = true;

                settings.CommandColumn.ButtonType = GridCommandButtonRenderMode.Image;
                settings.CommandColumn.Visible = (isAdmin && botones > 0);
                settings.CommandColumn.ShowNewButton = false;// (isAdmin && ((botones & (int)EnumBotonesGrid.Add) == (int)EnumBotonesGrid.Add));
                settings.CommandColumn.ShowDeleteButton = true;
                settings.CommandColumn.ShowEditButton = (isAdmin && ((botones & (int)EnumBotonesGrid.Edit) == (int)EnumBotonesGrid.Edit));



               
                

              

                settings.SettingsEditing.Mode = GridViewEditingMode.PopupEditForm;
                settings.SettingsPopup.EditForm.HorizontalAlign = PopupHorizontalAlign.WindowCenter;
                settings.SettingsPopup.EditForm.VerticalAlign = PopupVerticalAlign.WindowCenter;
                settings.SettingsPopup.EditForm.Modal = false;

                settings.SettingsPager.PageSize = 50;
                //settings.SettingsPager.SEOFriendly = SEOFriendlyMode.Enabled;
                settings.SettingsPager.PageSizeItemSettings.Visible = true;
                settings.SettingsPager.PageSizeItemSettings.ShowAllItem = true;
                settings.SettingsPager.Position = System.Web.UI.WebControls.PagerPosition.TopAndBottom;

                settings.SettingsPager.FirstPageButton.Visible = true;
                settings.SettingsPager.LastPageButton.Visible = true;


                settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Count, nameSummary);
                settings.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, nameSummary);

                settings.Settings.ShowFooter = true;
                settings.Settings.ShowHeaderFilterButton = true;

                settings.SettingsPager.Visible = true;
                settings.Settings.ShowGroupPanel = ShowGroupPanel;
                settings.Settings.ShowFilterRow = ShowFilterRow;
                settings.SettingsBehavior.AllowSelectByRowClick = true;

                settings.SettingsExport.Landscape = true;

                foreach (GridViewDataColumn column in settings.Columns)
                {
                    column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                    column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                }
                System.Web.HttpContext.Current.Session[settings.Name] = settings;
                settings.SettingsExport.TopMargin = 0;
                settings.SettingsExport.LeftMargin = 0;
            }
            catch (Exception ex)
            {
                string error = ex.ToString();
            }
        }

        
    }
}