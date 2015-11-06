using System.Collections.Generic;

namespace AspNet.Identity.MySQL
{
    /// <summary>
    /// Class that represents the UserRoles table in the MySQL Database
    /// </summary>
    public class UserRolesTable
    {
        private MySQLDatabase _database;

        /// <summary>
        /// Constructor that takes a MySQLDatabase instance 
        /// </summary>
        /// <param name="database"></param>
        public UserRolesTable(MySQLDatabase database)
        {
            _database = database;
        }

        /// <summary>
        /// Returns a list of user's roles
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public List<string> FindByUserId(string userId)
        {
            List<string> roles = new List<string>();
            string commandText = "Select Roles.nombre from usuariosRoles, Roles where usuariosRoles.idusuario = @idusuario and usuariosRoles.idrole = Roles.Id";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@idusuario", userId);

            var rows = _database.Query(commandText, parameters);
            foreach(var row in rows)
            {
                roles.Add(row["nombre"]);
            }

            return roles;
        }

        /// <summary>
        /// Deletes all roles from a user in the UserRoles table
        /// </summary>
        /// <param name="userId">The user's id</param>
        /// <returns></returns>
        public int Delete(string userId)
        {
            string commandText = "Delete from usuariosRoles where idusuario = @idusuario";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("idusuario", userId);

            return _database.Execute(commandText, parameters);
        }

        /// <summary>
        /// Inserts a new role for a user in the UserRoles table
        /// </summary>
        /// <param name="user">The User</param>
        /// <param name="roleId">The Role's id</param>
        /// <returns></returns>
        public int Insert(IdentityUser user, string roleId)
        {
            string commandText = "Insert into usuariosRoles (idusuario, idrole) values (@idusuario, @idrole)";
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("idusuario", user.Id);
            parameters.Add("idrole", roleId);

            return _database.Execute(commandText, parameters);
        }
    }
}
