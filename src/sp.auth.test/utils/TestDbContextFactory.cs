using System;
using Microsoft.EntityFrameworkCore;
using sp.auth.app.infra.ef;

namespace sp.auth.test.utils
{
    public class TestDbContextFactory
    {
        public static AuthDataContext Create()
        {
            var options = new DbContextOptionsBuilder<AuthDataContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AuthDataContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        public static void Destroy(AuthDataContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}