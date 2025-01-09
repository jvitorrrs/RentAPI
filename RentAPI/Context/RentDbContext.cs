using Microsoft.EntityFrameworkCore;
using RentAPI.Models;

public class RentDbContext : DbContext
{
    public RentDbContext() : base() { }

    public RentDbContext(DbContextOptions<RentDbContext> options) : base(options) { }

    public virtual DbSet<Cliente> Cliente { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.Property(c => c.Id)
            .ValueGeneratedOnAdd();

            entity.Property(c => c.Nome)
                .HasMaxLength(300)
                .IsRequired();

            entity.Property(c => c.Telefone)
                .HasMaxLength(12)
                .IsRequired();

            entity.Property(c => c.Endereco)
                .HasMaxLength(300)
                .IsRequired();

            entity.Property(c => c.Bairro)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(c => c.Cidade)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(c => c.Estado)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(c => c.Email)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(c => c.Documento)
                .HasMaxLength(14)
                .IsRequired();

            entity.Property(c => c.ValorContrato)
                .IsRequired();

            entity.Property(c => c.InicioContrato)
                .IsRequired();

            entity.Property(c => c.TerminoContrato)
                .IsRequired();

            entity.Property(c => c.DiaVencimento)
                .IsRequired();

            entity.Property(c => c.UltimoPgto)
                .HasMaxLength(50);
        });
    }
}
