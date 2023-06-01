using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WepApiCrudWithJwt.Identity;
using WepApiCrudWithJwt.Models;

namespace WepApiCrudWithJwt.Context
{
    public class AuthorBookDbContext : IdentityDbContext<AppIdentityUser>
    {
        public AuthorBookDbContext(DbContextOptions<AuthorBookDbContext> options): base(options)
        {
            
        }
        public DbSet<AppIdentityUser> AppIdentityUsers { get; set; }
        public DbSet<AppIdentityRole> AppIdentityRoles { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookSort> BookSorts { get; set; }
        public DbSet<Sort> Sorts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            SeedDataForKind(builder);
            AddDummyRoles(builder);
            base.OnModelCreating(builder);
        }
        private void SeedDataForKind(ModelBuilder builder)
        {
            builder.Entity<Sort>().HasData(new Sort { Id = 1, SortName = "Macera" });
            builder.Entity<Sort>().HasData(new Sort { Id = 2, SortName = "Roman" });
            builder.Entity<Sort>().HasData(new Sort { Id = 3, SortName = "BilimKurgu" });
            builder.Entity<Sort>().HasData(new Sort { Id = 4, SortName = "Gizem" });
            builder.Entity<Sort>().HasData(new Sort { Id = 5, SortName = "Biyografi" });
            builder.Entity<Sort>().HasData(new Sort { Id = 6, SortName = "Felsefe" });
        }
        private static void AddDummyRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppIdentityRole>().HasData(
                new AppIdentityRole()
                {
                    Name = "administrator",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "1",
                    Description = "Bu admin rolüdür."
                },
                new AppIdentityRole()
                {
                    Name = "editorielle design",
                    NormalizedName = "EDITOR",
                    ConcurrencyStamp = "2",
                    Description = "Bu editör rolüdür."
                },
                new AppIdentityRole()
                {
                    Name = "ordinary user",
                    NormalizedName = "USER",
                    ConcurrencyStamp = "10",
                    Description = "Bu user rolüdür."
                });
        }

    }
}
