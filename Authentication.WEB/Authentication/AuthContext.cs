using InsuredTraveling.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace InsuredTraveling
{

    public class AuthContext : IdentityDbContext<ApplicationUser>
    {
        public AuthContext() : base("AuthContext3", throwIfV1Schema: true)
        {

        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}