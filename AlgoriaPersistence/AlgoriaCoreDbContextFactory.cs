using AlgoriaPersistence.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AlgoriaPersistence
{
    public class AlgoriaCoreDbContextFactory : DesignTimeDbContextFactoryBase<AlgoriaCoreDbContext>
    {
        protected override AlgoriaCoreDbContext CreateNewInstance(DbContextOptions<AlgoriaCoreDbContext> options)
        {
            return new AlgoriaCoreDbContext(options);
        }
    }
}
