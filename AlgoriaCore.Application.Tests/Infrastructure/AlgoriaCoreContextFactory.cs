using AlgoriaPersistence;
using Microsoft.EntityFrameworkCore;
using System;

namespace AlgoriaCore.Application.Tests.Infrastructure
{
    public static class AlgoriaCoreContextFactory
    {
        public static AlgoriaCoreDbContext Create()
        {
            var options = new DbContextOptionsBuilder<AlgoriaCoreDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AlgoriaCoreDbContext(options);

            AlgoriaCoreInitializer.Initialize(context);

            return context;
        }

        public static void Destroy(AlgoriaCoreDbContext context)
        {
            context.Database.EnsureDeleted();

            context.Dispose();
        }
    }
}