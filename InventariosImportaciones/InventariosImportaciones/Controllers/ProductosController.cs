using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;


using System.Text;
using System.IO;
using AppLimit.CloudComputing.SharpBox;
using AppLimit.CloudComputing.SharpBox.StorageProvider.DropBox;
using inventarioImportaciones.Models;

namespace InventariosImportaciones.Controllers
{
    public class ProductosController : Controller
    {
        // GET: Productos
        public ActionResult Index()
        {
            

            return View();
        }

        inventarioImportaciones.Models.CCDBContext db = new inventarioImportaciones.Models.CCDBContext();

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            var model = new List<productos>();
            return PartialView("_GridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew(inventarioImportaciones.Models.productos item)
        {
            var model = db.productos_List;
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
            return PartialView("_GridViewPartial", model.ToList());
        }

        public ActionResult BinaryImageColumnPhotoUpdate()
        {
            return BinaryImageEditExtension.GetCallbackResult();
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate(inventarioImportaciones.Models.productos item)
        {
            var model = db.productos_List;
            if (ModelState.IsValid)
            {
                try
                {
                    var modelItem = model.FirstOrDefault(it => it.Codigo == item.Codigo);
                    if (item.Imagen.Length!=item.ImagenSelect.Length)
                    {
                        item.DirImagen = "";
                        string path = @"C:\Visual .net\DanielAsp";
                        string filename = path+"\\"+item.Nombre;
                        var fsC = new BinaryWriter(new FileStream( filename + ".jpg", FileMode.Append, FileAccess.Write));
                        fsC.Write(item.ImagenSelect);
                        fsC.Close();

                        CloudStorage dropBoxStorage = new CloudStorage();
                        var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);
                        ICloudStorageAccessToken accessToken = null;
                        // load a valid security token from file
                        using (FileStream fs = System.IO.File.Open(@"C:\Visual .net\InventariosImportaciones\InventariosImportaciones\SharpDropBox.Token",
                        FileMode.Open, FileAccess.Read,
                        FileShare.None))
                        {
                            accessToken = dropBoxStorage.DeserializeSecurityToken(fs);
                        }

                        var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);

                        var publicFolder = dropBoxStorage.GetFolder("/");
                        // GetFolder("/Public");
                        foreach (var fof in publicFolder)
                        {
                            // check if we have a directory
                            Boolean bIsDirectory = fof is ICloudDirectoryEntry;
                            // output the info
                            Console.WriteLine("{0}: {1}", bIsDirectory ? "DIR" : "FIL", fof.Name);
                        }
                       

                        String srcFile = Environment.ExpandEnvironmentVariables(filename + ".jpg");
                      ICloudFileSystemEntry fileUploaded=  dropBoxStorage.UploadFile(srcFile, publicFolder);

                      ICloudDirectoryEntry fEntry = dropBoxStorage.GetFolder("/");
                      ICloudFileSystemEntry fszz = dropBoxStorage.GetFileSystemObject("Gamma Quick Kids 21in.jpg", fEntry);

                      string d = DropBoxStorageProviderTools.GetPublicObjectUrl(storageToken, fszz).AbsoluteUri;
                      Console.WriteLine(d);
                        dropBoxStorage.DownloadFile(publicFolder, item.Nombre + ".jpg", Environment.ExpandEnvironmentVariables(path));
                      
                        dropBoxStorage.Close();
                    }
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
            return PartialView("_GridViewPartial", model.ToList());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete(System.Int32 Codigo)
        {
            var model = db.productos_List;
            if (Codigo >= 0)
            {
                try
                {
                    var item = model.FirstOrDefault(it => it.Codigo == Codigo);
                    if (item != null)
                        model.Remove(item);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GridViewPartial", model.ToList());
        }

        public static byte[] GetBytesFromFile(string fullFilePath)
        {
            // this method is limited to 2^32 byte files (4.2 GB)

            FileStream fs = null;
            try
            {
                fs = System.IO.File.OpenRead(fullFilePath);
                byte[] bytes = new byte[fs.Length];
                fs.Read(bytes, 0, Convert.ToInt32(fs.Length));
                return bytes;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }

        }


    }
}