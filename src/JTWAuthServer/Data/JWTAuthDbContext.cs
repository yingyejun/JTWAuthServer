using JTWAuthServer.Services;
using Microsoft.EntityFrameworkCore;

namespace JTWAuthServer.Data {
    public class JWTAuthDbContext : DbContext {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Filename=JWTAuth.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<JWTClient>().ToTable("JWTApplication");
        }
    }
}
