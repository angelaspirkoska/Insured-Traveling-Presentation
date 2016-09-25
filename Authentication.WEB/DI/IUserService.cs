using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace InsuredTraveling.DI
{
    public interface IUserService
    {
        aspnetuser GetUserDataByUsername(string Username);

        aspnetuser GetUserById(string id);

        string GetUserIdByUsername(string Username);
    }
}
