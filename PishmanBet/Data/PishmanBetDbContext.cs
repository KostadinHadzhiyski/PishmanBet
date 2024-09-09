using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PishmanBet.Data
{
    public class PishmanBetDbContext : IdentityDbContext
    {
        public PishmanBetDbContext(DbContextOptions<PishmanBetDbContext> options)
            : base(options)
        {
        }
    }
}