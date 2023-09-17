using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrincipalObjects.Objects
{
    public class User
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EsAdmin { get; set; }
        public bool Delete { get; set; }

        #region dbObject
        string TableName = "oUsers";
        string[] ColNames = new string[5] {
            "id", 
            "username", 
            "password",
            "isAdmin", 
            "Deleted" 
        };
        #endregion

        public User() { }

        //CONSTRUCTORS
        public User GetUserById(long id)
        {
            dynamic userFromDB = SQLInteract.GetDataFromDataBase((false,-1),ColNames, TableName, (true,new string[1] { "where id = " + id }),(false,"",false));
            try
            {
                User user = new User()
                {
                    Id = Convert.ToInt64(userFromDB.rows[0].id.Value),
                    UserName = userFromDB.rows[0].username.Value.ToString(),
                    Password = userFromDB.rows[0].password.Value.ToString(),
                    EsAdmin = Convert.ToBoolean(userFromDB.rows[0].isAdmin.Value.ToString()),
                    Delete = Convert.ToBoolean(userFromDB.rows[0].Deleted.Value.ToString())
                };

                return user;
            }catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return null;
            }
        }

        public List<User> GetUsers()
        {
            dynamic userFromDB = SQLInteract.GetDataFromDataBase((false, -1), ColNames, TableName, (false, new string[0] {}), (false, "", false));
            List<User> users = new List<User>();

            try
            {
                foreach (dynamic user in userFromDB.rows)
                {
                    try
                    {
                        User userToList = new User()
                        {
                            Id = Convert.ToInt64(user.id.Value),
                            UserName = user.username.Value.ToString(),
                            Password = user.password.Value.ToString(),
                            EsAdmin = Convert.ToBoolean(user.isAdmin.Value.ToString()),
                            Delete = Convert.ToBoolean(user.Deleted.Value.ToString())
                        };
                        users.Add(userToList);
                    }
                    catch (Exception ex)
                    {
                        Utilities.WriteLog("FALLO AL CREAR USUARIO: " + ex.Message);
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                Utilities.WriteLog(ex.Message);
                return new List<User>(); //SI LA LISTA VIENE VACIA, ES POR QUE ALGO ESTA MAL
            }
        }
    }
}
