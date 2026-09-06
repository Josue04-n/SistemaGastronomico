namespace SistemaGastronomico.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGastronomico.Domain.Entities;

public class CoordenadaConfiguration : IEntityTypeConfiguration<Coordenada>
{
    public void Configure(EntityTypeBuilder<Coordenada> builder)
    {
        builder.ToTable("Coordenadas");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Latitud)
            .IsRequired();

        builder.Property(c => c.Longitud)
            .IsRequired();

        builder.Property(c => c.DireccionReferencial)
            .HasMaxLength(250);

        builder.HasOne(c => c.Local)
            .WithOne(l => l.Coordenadas)
            .HasForeignKey<Coordenada>(c => c.LocalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
