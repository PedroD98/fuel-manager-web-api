using Microsoft.EntityFrameworkCore;

namespace fuel_manager_web_api.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<Consumo> Consumos { get; set; }
}