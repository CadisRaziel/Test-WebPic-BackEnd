using Microsoft.EntityFrameworkCore;
using UserApiWebPic.Domain;

namespace UserApiWebPic.Infra.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<User>().Property(p => p.Name).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.LastName).IsRequired();
            modelBuilder.Entity<User>().Property(p => p.Cpf).IsRequired().HasMaxLength(11);
            modelBuilder.Entity<User>().Property(p => p.Rg).IsRequired().HasMaxLength(9);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configuration)      
        {
            configuration.Properties<string>().HaveMaxLength(100); 
        }
    }
}
