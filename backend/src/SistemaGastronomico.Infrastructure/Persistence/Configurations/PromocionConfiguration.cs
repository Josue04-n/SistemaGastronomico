namespace SistemaGastronomico.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGastronomico.Domain.Entities;

public class PromocionConfiguration : IEntityTypeConfiguration<Promocion>
{
    public void Configure(EntityTypeBuilder<Promocion> builder)
    {
        builder.ToTable("Promociones");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Titulo)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Descripcion)
            .HasMaxLength(500);

        builder.Property(p => p.PrecioOriginal)
            .HasColumnType("decimal(18,2)");

        builder.Property(p => p.PrecioPromocional)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.ImagenUrl)
            .HasMaxLength(500);

        builder.Property(p => p.EstaActiva)
            .HasDefaultValue(true);

        builder.HasOne(p => p.Local)
            .WithMany(l => l.Promociones)
            .HasForeignKey(p => p.LocalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
