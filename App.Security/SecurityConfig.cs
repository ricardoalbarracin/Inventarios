using System.Collections.Generic;
using AspNet.Identity.MySQL;
using System.Diagnostics;
using System.Web;
using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Data;
 
namespace App.Security {
    /// <summary>
    /// Componente encargado de la configuración del esquema de seguridad asociado a ASP.NET Identity
    /// </summary>
    public class SecurityConfig {

        /// <summary>
        /// Metodo que inicializa los componentes que manejan los permisos, roles usuario y variable de session
        /// </summary>
        public static void RegisterSecurity() {
            var session = HttpContext.Current.Session;

            // Generación de ID de session
            var sessionId = Guid.NewGuid().ToString();
            session.Add("sessionId", sessionId);

            // permisos
            var permissions = new Dictionary<string, Permission>();
            session.Add("permissions", permissions);

            // roles
            var roles = new Dictionary<string, Role>();
            session.Add("roles", roles);

            // user
            var user = new Dictionary<string, object>();
            session.Add("user", user);
        }

        /// <summary>
        /// Metodo que define la carga inicial de roles, permisos y detalles del usuario
        /// </summary>
        /// <param name="username"></param>
        /// <param name="uorgId"></param>
        public static void RegisterRolesAndPermissions(string username) {
            GetPermisosUsuario(username);
            GetRolesUsuario(username);
            RegisterUserDetails(username);
        }

        /// <summary>
        /// Metodo que carga los permisos de un usuario dependiendo del establecimiento
        /// </summary>
        /// <param name="username"></param>
        /// <param name="uorgId"></param>
        private static void GetPermisosUsuario(string username) {
            // Get Session
            var session = HttpContext.Current.Session;

            // Access to Database
            var db = new MySQLDatabase();
            //MySqlCommand cmd = db.Connection.CreateCommand();

            /////

            MySqlCommand cmd = new MySqlCommand("SELECT p.`Codigo`,p.`Nombre` FROM cif_bd.Usuariospermisos u  INNER JOIN cif_bd.usuarios u1 ON(u.`IdUsuario` = u1.`Id`  ) INNER JOIN cif_bd.permisos p ON(u.`IdPermiso` = p.`Id`  ) where u1.`UserName` = @UserName UNION select per.Codigo, per.Nombre from rolespermisos rp INNER JOIN cif_bd.permisos per on (rp.IdPermiso = per.Id) where IdRole in (SELECT u.`idrole` FROM cif_bd.usuariosroles u INNER JOIN cif_bd.usuarios u1 ON(u.`Idusuario` = u1.`Id`  )  where u1.`UserName` = @UserName ) ; ", db.Connection);
            
            /////

            try {


                // Procedure and Params   

                cmd.Parameters.AddWithValue("@username", username);
               
                // Execute
                db.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                // Read
                var permissions = session["permissions"] as IDictionary<string, Permission>;
                string permissionCode, permissionName;

                Debug.WriteLine("----- Permisos -----");
                while (reader.Read()) {
                    permissionName = reader["NOMBRE"].ToString();
                    permissionCode = reader["CODIGO"].ToString();

                    permissions.Add(permissionCode, new Permission {
                        Code = permissionCode,
                        Name = permissionName
                    });                    
                    Debug.WriteLine("Codigo: {0}, Nombre: {1}", permissionCode, permissionName);
                }

                reader.Dispose();
                

            } catch (MySqlException) {
                throw;

            } catch (FormatException) {
                throw;

            } finally {
                // Free memory
                db.Close();
                cmd.Dispose();
                db.Dispose();
            }
        }

        /// <summary>
        /// Metodo que carga los roles de un usuario dependiendo del establecimiento
        /// </summary>
        /// <param name="username"></param>
        /// <param name="uorgId"></param>
        private static void GetRolesUsuario(string username) {
            // Get Session
            var session = HttpContext.Current.Session;

            // Access to Database
            var db = new MySQLDatabase();
            //MySqlCommand cmd = db.Connection.CreateCommand();
            MySqlCommand cmd = new MySqlCommand("SELECT r.`Nombre`,r.`Codigo` FROM cif_bd.usuariosroles u INNER JOIN cif_bd.usuarios u1 ON(u.`Idusuario` = u1.`Id`  ) INNER JOIN cif_bd.roles r ON(u.`idrole` = r.`Id`  )  where u1.`UserName` = @UserName ; ", db.Connection);
            try {
                // Procedure and Params   

                cmd.Parameters.AddWithValue("@username", username);
                // Execute
                db.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                // Read
                var roles = session["roles"] as IDictionary<string, Role>; 
                string roleName, roleCode;

                Debug.WriteLine("----- Roles -----");
                while (reader.Read()) {
                    roleName = reader["NOMBRE"].ToString();
                    roleCode = reader["CODIGO"].ToString();
                    roles.Add(roleCode, new Role {
                        Code = roleCode,
                        Name = roleName
                    });
                    
                    Debug.WriteLine("Codigo: {0}, Nombre: {1}", roleCode, roleName);
                }

                reader.Dispose();
                //p1.Dispose();

            } catch (MySqlException) {
                throw;

            } catch (FormatException) {
                throw;

            } finally {
                db.Close();
                cmd.Dispose();
                db.Dispose();
            }
        }
        
        
        /// <summary>
        /// Metodo que carga los detalles del usuario logueado para su posterior uso
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="uorgId">Id de unidad organizacional en logueada</param>
        private static void RegisterUserDetails(string username)
        {
            // Get Session
            var session = HttpContext.Current.Session;

            // Access to Database
            var db = new MySQLDatabase();
            MySqlCommand cmd = db.Connection.CreateCommand();

            try
            {
                // Procedure and Params
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SEG.GetUserDetails";
                cmd.Parameters.Add("usuario_", MySqlDbType.VarChar, 4000).Value = username;
                cmd.Parameters.Add("uorg_id_", MySqlDbType.Int64, 4000).Value = 0;
                cmd.Parameters.Add("id_", MySqlDbType.VarChar, 4000).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("email_", MySqlDbType.VarChar, 4000).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("nombres_", MySqlDbType.VarChar, 4000).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("apellidos_", MySqlDbType.VarChar, 4000).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("entaportantes_id_", MySqlDbType.Int64).Direction = ParameterDirection.Output;

                // Execute
                db.Open();
                cmd.ExecuteNonQuery();

                // Read
                var user = session["user"] as IDictionary<string, object>;
                user.Add("UserName", username);
                user.Add("UorgId", 0);
                user.Add("UserId", cmd.Parameters["id_"].Value);
                user.Add("Email", cmd.Parameters["email_"].Value);
                user.Add("FirstNames", cmd.Parameters["nombres_"].Value);
                user.Add("LastNames", cmd.Parameters["apellidos_"].Value);
                user.Add("EntAportanteId", cmd.Parameters["entaportantes_id_"].Value);
                
            } catch (MySqlException) {
                throw;
            } catch (FormatException) {
                throw;
            } finally {
                db.Close();
                cmd.Dispose();
                db.Dispose();
            }
        }
    }

    

    

}
