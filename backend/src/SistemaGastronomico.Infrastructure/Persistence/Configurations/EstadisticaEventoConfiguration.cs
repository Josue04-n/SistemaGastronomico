namespace SistemaGastronomico.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGastronomico.Domain.Entities;

public class EstadisticaEventoConfiguration : IEntityTypeConfiguration<EstadisticaEvento>
{
    public void Configure(EntityTypeBuilder<EstadisticaEvento> builder)
    {
        builder.ToTable("EstadisticasEventos");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.TipoEvento)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.FechaRegistro)
            .IsRequired();

        builder.Property(e => e.Detalle)
            .HasMaxLength(500);

        builder.HasOne(e => e.Local)
            .WithMany(l => l.Estadisticas)
            .HasForeignKey(e => e.LocalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
