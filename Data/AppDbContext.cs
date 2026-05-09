using Kaizen.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<KaizenIdea> KaizenIdeas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Empleado>()
            .HasKey(e => e.NoEmpleado);

        modelBuilder.Entity<KaizenIdea>()
            .HasOne(k => k.Empleado)
            .WithMany()
            .HasForeignKey(k => k.NoEmpleado);
    }
}