using CollecumentData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace CollecumentBl
{
    public class Logic
    {

        public static User GetUserByTz(string tz)
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                return context.Users.FirstOrDefault(x => x.TZ.Equals(tz));
            }

        }

       
        public static List<Permission> GetAllPermissions()
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                return context.Permissions.ToList();
            }
        }

       
        public static List<User> GetAllUsers()
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                return context.Users.ToList();
            }
        }

        public static string GetUserByName(string creatorName)
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                return context.Users.FirstOrDefault(x=>x.Name==creatorName).TZ;
            }
        }

        public static List<Category> GetAllCategories()
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                return context.Categories.ToList();
            }
        }

        public static User CheckExistsUserByTZAndPwd(string tZ, string password)
        {
            using (DBCollecumentEntities context = new DBCollecumentEntities())
            {
                return context.Users.FirstOrDefault(x => x.TZ == tZ && x.Password == password);
            }
        }

    }
}
