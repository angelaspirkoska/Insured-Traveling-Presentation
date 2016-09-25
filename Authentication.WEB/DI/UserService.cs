using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsuredTraveling.DI
{
    public class UserService : IUserService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public aspnetuser GetUserById(string id)
        {
          return  _db.aspnetusers.Where(x => x.Id == id).ToArray().Last();
        }

        public aspnetuser GetUserDataByUsername(string username)
        {
            aspnetuser a = _db.aspnetusers.Where(x => x.UserName == username).ToArray().First();
            return a;
        }

        public string GetUserIdByUsername(string Username)
        {
           return _db.aspnetusers.Where(x => x.UserName == Username).Select(x => x.Id).First();
        }
    }
}
