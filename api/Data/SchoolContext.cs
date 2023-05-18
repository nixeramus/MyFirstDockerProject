using Microsoft.EntityFrameworkCore;
namespace api.Data
{
   public class SchoolContext : DbContext
   {
      public SchoolContext(DbContextOptions<SchoolContext> options) : base(options) { }
      public DbSet<Models.Student>? Students { get; set; }
   }
}