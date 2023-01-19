using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Pizzeria.Models;

namespace Pizzeria.Database
{
    public class PizzaContext : DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Database=PizzeriaV2;" +
            "Integrated Security=True;TrustServerCertificate=True");
        }
    }
}
