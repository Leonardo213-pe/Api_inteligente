using Microsoft.EntityFrameworkCore;
using ApiInteligenteWeb.Models;

namespace ApiInteligenteWeb.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Tarea> Tareas { get; set; }
    }
}