using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using AppLimit.CloudComputing.SharpBox;
using System.IO;
using InventariosImportaciones.Controllers;


namespace inventarioImportaciones.Models
{
   
    public partial class CCDBContext : DbContext
    {
        public DbSet<productos> productos_List { get; set; }
        public DbSet<Usuarios> usuarios_List { get; set; }
        public DbSet<Roles> roles_List { get; set; }
    }


    [Table("productos")]
    public class productos
    {
        [Key]
        public int Codigo { get; set; }
        [Required(ErrorMessage="El {0} es requerido.")]
        public string Nombre { get; set; }
        public int CostoUnitario { get; set; }
        public int Cantidad { get; set; }
        public int CostoTotal { get; set; }
        public DateTime FechaCompra { get; set; }
        public int PrecioVenta { get; set; }
        public int Tipo { get; set; }
        public int SubTipo { get; set; }
        public string DirImagen { get; set; }
        public byte[] ImagenSelect;
        public byte[] Imagen
        {
            get
            {
                if ((DirImagen != null)&&(DirImagen.Trim().Length>0))
                {
                    try
                    {
                        var webClient = new WebClient();
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

                        var publicFolder = dropBoxStorage.GetRoot();

                        string path = @"C:\Visual .net";
                        dropBoxStorage.DownloadFile(publicFolder, Nombre+".jpg", Environment.ExpandEnvironmentVariables(path));
                        dropBoxStorage.Close();

                        return ProductosController.GetBytesFromFile(path + Nombre + ".jpg");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return new byte[0];
                    }
                    
                }
                else
                {
                    return new byte[0];
                }
            }
            set { ImagenSelect = value; }
        }
    }

    [Table("users")]
    public class Usuarios
    {
        [Key]
        public string Id { get; set; }
        public string Email { get; set; }
        public int EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public int PhoneNumberConfirmed { get; set; }
        public int TwoFactorEnabled { get; set; }
       
        public int LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
    }

    [Table("aspnetroles")]
    public class Roles
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        
    }


}