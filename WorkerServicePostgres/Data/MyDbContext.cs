using Microsoft.EntityFrameworkCore;
using WorkerServicePostgres.Entities;

namespace WorkerServicePostgres.Data;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    public DbSet<Carro> Carros { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carro>(entity =>
        {
            entity.ToTable("carros");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                  .HasColumnName("id")
                  .HasColumnType("uuid")
                  .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Marca)
                  .HasColumnName("marca")
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Modelo)
                  .HasColumnName("modelo")
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.Ano)
                  .HasColumnName("ano")
                  .IsRequired();
        });
    }
}