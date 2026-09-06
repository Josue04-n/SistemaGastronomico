namespace SistemaGastronomico.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGastronomico.Domain.Entities;

public class LocalConfiguration : IEntityTypeConfiguration<Local>
{
    public void Configure(EntityTypeBuilder<Local> builder)
    {
        builder.ToTable("Locales");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Nombre)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(l => l.Descripcion)
            .HasMaxLength(1000);

        builder.Property(l => l.Direccion)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(l => l.Referencia)
            .HasMaxLength(250);

        builder.Property(l => l.CampusCercano)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(l => l.TelefonoContacto)
            .HasMaxLength(20);

        builder.Property(l => l.WhatsApp)
            .HasMaxLength(20);

        builder.Property(l => l.HorarioApertura)
            .HasMaxLength(100);

        builder.Property(l => l.LogoUrl)
            .HasMaxLength(500);

        builder.Property(l => l.PortadaUrl)
            .HasMaxLength(500);

        builder.Property(l => l.Categoria)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.HasOne(l => l.Propietario)
            .WithMany(u => u.Locales)
            .HasForeignKey(l => l.PropietarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
