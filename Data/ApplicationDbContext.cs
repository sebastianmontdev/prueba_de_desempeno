using Microsoft.EntityFrameworkCore;
using VetClinic.Models;

namespace VetClinic.Data
{
    // El DbContext es el corazón de la conexión con la base de datos
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Definimos las 3 tablas principales de nuestro sistema deportivo
        // Esto le dice a Entity Framework que estas clases son tablas en MySQL
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<EspacioDeportivo> EspaciosDeportivos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
    } 
}
