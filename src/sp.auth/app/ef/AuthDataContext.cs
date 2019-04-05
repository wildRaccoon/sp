using Microsoft.EntityFrameworkCore;
using sp.auth.domain.account;

namespace sp.auth.app.ef
{
    public class AuthDataContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountSession> AccountSessions { get; set; }

        public AuthDataContext(DbContextOptions opt) : base(opt)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<AccountSession>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();
        }
    }
}
