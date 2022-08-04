using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UIDO.Domain.Indentity;
using UIDO.Domain.Protocolo;


namespace UIDO.Database
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            ModelConfig(builder);


        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }


        public DbSet<Protocolo> Protocolo { get; set; }

        private void ModelConfig(ModelBuilder modelBuilder)
        {

            var tipo = ApplicationUser.GetType().ToString();
            modelBuilder.Entity<ApplicationUser>().HasDiscriminator().HasValue("ApplicationUser");
        }
    }


}