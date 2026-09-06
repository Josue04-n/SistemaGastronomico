namespace SistemaGastronomico.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGastronomico.Domain.Entities;

public class ResenaConfiguration : IEntityTypeConfiguration<Resena>
{
    public void Configure(EntityTypeBuilder<Resena> builder)
    {
        builder.ToTable("Resenas");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Calificacion)
            .IsRequired();

        builder.Property(r => r.Comentario)
            .HasMaxLength(1000);

        builder.Property(r => r.Fecha)
            .IsRequired();

        builder.HasOne(r => r.Local)
            .WithMany(l => l.Resenas)
            .HasForeignKey(r => r.LocalId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.Estudiante)
            .WithMany(u => u.Resenas)
            .HasForeignKey(r => r.EstudianteId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
