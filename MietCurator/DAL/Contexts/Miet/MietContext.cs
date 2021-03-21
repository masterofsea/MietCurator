using Microsoft.EntityFrameworkCore;

namespace MietCurator.DAL.Contexts.Miet
{
    public class MietContext : DbContext
    {
        public MietContext()
        {
        }

        public MietContext(DbContextOptions<MietContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Groups> Groups { get; set; }
    }
}