using Kaizen.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Empleado> Empleados { get; set; }
    public DbSet<KaizenIdea> KaizenIdeas { get; set; }
    public DbSet<KaizenComentario> KaizenComentarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // 🔑 MUY IMPORTANTE para que Identity configure sus tablas

        modelBuilder.Entity<Empleado>()
            .HasKey(e => e.NoEmpleado);

        modelBuilder.Entity<KaizenIdea>()
            .HasOne(k => k.Empleado)
            .WithMany()
            .HasForeignKey(k => k.NoEmpleado);
    }
}