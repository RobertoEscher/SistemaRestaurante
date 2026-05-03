using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SistemaRestaurante.Infrastructure.Context
{
    public class RestauranteDbContextFactory : IDesignTimeDbContextFactory<RestauranteDbContext>
    {
        public RestauranteDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RestauranteDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost,1433;Database=RestauranteDb;User Id=sa;Password=1q2w3e4r@#$;TrustServerCertificate=True;");

            return new RestauranteDbContext(optionsBuilder.Options);
        }
    }
}