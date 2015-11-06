using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Security.Principal;
using System.Runtime.Caching;
using System.Web;

namespace App.Security {
    /// <summary>
    /// Componente encargado de extender las caracteristicas del usuario logueado
    /// </summary>
    public static class UserHelper {
        
        /// <summary>
        /// Metodo que valida si el usuario logueado tiene el permiso(s) especificado(s)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="permission">Permiso(s)</param>
        /// <returns></returns>        
        public static bool HasPermission(this IPrincipal user, string permissionsString) {
            // Obtiene permisos de cache
            var session = HttpContext.Current.Session;
            var permissions = session["permissions"] as IDictionary<string, Permission>;
            
            // Valida si tiene algun permiso asignado
            if (permissions.Count == 0) {
                return false;
            }

            // Covierte permisos en lista
            string[] permissionsList = permissionsString.Split(Convert.ToChar(","));

            // Valida si tiene alguno de los permisos especificados
            string permission;
            for (int i = 0; i < permissionsList.Length; i++) {
                permission = permissionsList[i];
                if (permissions.ContainsKey(permission)) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Metodo que valida si el usuario logueado tiene el role(s) especificado(s)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roleName">Role(s)</param>
        /// <returns></returns>
        public static bool HasRole(this IPrincipal user, string rolesString)
        {
            // Obtiene permisos de cache
            var session = HttpContext.Current.Session;
            var roles = session["roles"] as IDictionary<string, Role>;

            // Valida si tiene algun permiso asignado
            if (roles.Count == 0)
            {
                return false;
            }

            // Covierte permisos en lista
            string[] rolesList = rolesString.Split(Convert.ToChar(","));

            // Valida si tiene alguno de los permisos especificados
            string role;
            for (int i = 0; i < rolesList.Length; i++)
            {
                role = rolesList[i];
                if (roles.ContainsKey(role))
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Metodo que retorna la lista de permisos del usuario logueado
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static IList<Permission> GetListPermissions(this IPrincipal user) {
            var session = HttpContext.Current.Session;
            var permissions = session["permissions"] as IDictionary<string, Permission>;
            return permissions.Values.ToList();
        }

        /// <summary>
        /// Metodo extendido que retorna lista de roles del usuario logueado
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static IList<Role> GetListRoles(this IPrincipal user) {
            var session = HttpContext.Current.Session;
            var roles = session["roles"] as IDictionary<string, Role>;
            return roles.Values.ToList();
        }

        /// <summary>
        /// Metodo privado que retorna un atributo especifico del un usuario
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetUserDetailValue(string key) {
            var session = HttpContext.Current.Session;
            var _user = session["user"] as IDictionary<string, object>;
            return _user.ContainsKey(key) ? _user[key].ToString() : string.Empty;
        }

        /// <summary>
        /// Metodo privado que retorna el nombre de usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string UserName(this IPrincipal user) {
            return GetUserDetailValue("UserName");
        }

        /// <summary>
        /// Metodo que retorna el ID de la unidad organizacional en la que esta logueado
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string UorgId(this IPrincipal user) {
            return GetUserDetailValue("UorgId");
        }

        /// <summary>
        /// Metodo que retorna el ID del usuario 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string UserId(this IPrincipal user) {
            return GetUserDetailValue("UserId");
        }

        /// <summary>
        /// Metodo que retorna el Email del usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string Email(this IPrincipal user) {
            return GetUserDetailValue("Email");
        }

        /// <summary>
        /// Metodo que retorna la entidad aportante del usuario (solo aplica en algunos casos)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string EntAportanteId(this IPrincipal user) {
            return GetUserDetailValue("EntAportanteId");
        }

        /// <summary>
        /// Metodo extendido que retorna los primeros nombres de usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string FirstNames(this IPrincipal user) {
            return GetUserDetailValue("FirstNames");
        }

        /// <summary>
        /// Metodo extendido que retorna los apellidos del usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static string LastNames(this IPrincipal user) {
            return GetUserDetailValue("LastNames");
        }

    }

    /// <summary>
    /// Filtro para determinar la existencia de un permiso o un rol
    /// </summary>
    public class HasPermissionAttribute : ActionFilterAttribute {
        private string _permissionsString;

        public HasPermissionAttribute(string permissionsString) {


            this._permissionsString = permissionsString;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            // Valida si tiene permisos 
            bool hasPermission = filterContext.HttpContext.User.HasPermission(_permissionsString);

            if (!hasPermission) {
                var url = new UrlHelper(filterContext.RequestContext);
                // TODO: esta redirección debe corresponder a un patrón de logica de ejecución
                var loginUrl = url.Content("/"); 
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
        }
    }

    public class HasRoleAttribute : ActionFilterAttribute {
        private string _roleString;

        public HasRoleAttribute(string roleString) {
            this._roleString = roleString;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            // Valida si tiene el rol
            bool hasRole = filterContext.HttpContext.User.HasRole(_roleString);

            if (!hasRole) {
                var url = new UrlHelper(filterContext.RequestContext);
                // TODO: esta redirección debe corresponder a un patrón de logica de ejecución
                var loginUrl = url.Content("/"); 
                filterContext.HttpContext.Response.Redirect(loginUrl, true);
            }
        }
    }

}