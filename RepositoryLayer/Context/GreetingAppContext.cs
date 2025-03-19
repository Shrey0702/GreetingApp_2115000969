using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class GreetingAppContext : DbContext
    {
        public GreetingAppContext(DbContextOptions<GreetingAppContext> options) : base(options)
        {
        }

        public virtual DbSet<GreetingEntity> GreetingEntities { get; set; } = null!;
        public DbSet<UserEntity> UserEntities { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define Foreign Key Relationship
            modelBuilder.Entity<GreetingEntity>()
                .HasOne(g => g.User)        // GreetingEntity has one User
                .WithMany(u => u.Greetings) // UserEntity has many Greetings
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete if needed
        }
    }
}
