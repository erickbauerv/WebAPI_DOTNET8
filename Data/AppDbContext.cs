using Microsoft.EntityFrameworkCore;
using WebAPI_DOTNET8.Models;

namespace WebAPI_DOTNET8.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AuthorModel> Authors { get; set; }
        public DbSet<BookModel> Books { get; set; }
        public DbSet<UserLoginModel> UserLogin { get; set; }
    }
}
