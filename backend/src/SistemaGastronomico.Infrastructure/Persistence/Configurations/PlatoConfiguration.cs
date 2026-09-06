namespace SistemaGastronomico.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGastronomico.Domain.Entities;

public class PlatoConfiguration : IEntityTypeConfiguration<Plato>
{
    public void Configure(EntityTypeBuilder<Plato> builder)
    {
        builder.ToTable("Platos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(p => p.Descripcion)
            .HasMaxLength(500);

        builder.Property(p => p.Precio)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.ImagenUrl)
            .HasMaxLength(500);

        builder.Property(p => p.Categoria)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.EstaDisponible)
            .HasDefaultValue(true);

        builder.Property(p => p.EsPlatoDelDia)
            .HasDefaultValue(false);

        builder.HasOne(p => p.Local)
            .WithMany(l => l.Platos)
            .HasForeignKey(p => p.LocalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
