using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace InsuredTraveling.DI
{
    public class UserService : IUserService
    {
        InsuredTravelingEntity _db = new InsuredTravelingEntity();

        public IQueryable<SelectListItem> GetPolicyNumberListByUsername(string Username)
        {
            string ID = _db.aspnetusers.Where(a => a.UserName == Username).Single().Id;
            var PolicyNumbers =  _db.travel_policy.Where(p => p.Created_By == ID).Select(p => new SelectListItem
           {
               Text = p.Policy_Number,
               Value = p.ID.ToString()
           });

            return PolicyNumbers;
        }

        public List<SelectListItem> GetPolicyNumberListByUsernameToList(string Username)
        {
            string ID = _db.aspnetusers.Where(a => a.UserName == Username).Single().Id;
            var PolicyNumbers = _db.travel_policy.Where(p => p.Created_By == ID && p.Payment_Status == true).Select(p => new SelectListItem
            {
                Text = p.Policy_Number,
                Value = p.ID.ToString()
            }).ToList();

            return PolicyNumbers;
        }

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
            return _db.aspnetusers.Where(x => x.UserName == Username).Select(x => x.Id).FirstOrDefault();
        }

        public string GetUserSsnByUsername(string username)
        {
            return _db.aspnetusers.Where(x => x.UserName == username).Select(x => x.EMBG).SingleOrDefault();
           
        }

        public bool IsSameLoggedUserAndInsured(string UsernameLoggedUser, int SelectedInsuredId)
        {
            string SSNLoggedUser = _db.aspnetusers.Where(x => x.UserName == UsernameLoggedUser).Select(x => x.EMBG).First(); 
            
            string SSNSelectedInsured = _db.insureds.Where(x => x.ID == SelectedInsuredId).Select(x => x.SSN).First();

            return SSNLoggedUser.Equals(SSNSelectedInsured);

        }
    }
}
