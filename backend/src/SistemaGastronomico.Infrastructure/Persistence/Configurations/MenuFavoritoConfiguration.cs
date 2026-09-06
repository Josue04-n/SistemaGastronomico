namespace SistemaGastronomico.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGastronomico.Domain.Entities;

public class MenuFavoritoConfiguration : IEntityTypeConfiguration<MenuFavorito>
{
    public void Configure(EntityTypeBuilder<MenuFavorito> builder)
    {
        builder.ToTable("MenusFavoritos");

        builder.HasKey(mf => mf.Id);

        builder.HasIndex(mf => new { mf.EstudianteId, mf.LocalId })
            .IsUnique();

        builder.Property(mf => mf.FechaGuardado)
            .IsRequired();

        builder.HasOne(mf => mf.Estudiante)
            .WithMany(u => u.MenusFavoritos)
            .HasForeignKey(mf => mf.EstudianteId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(mf => mf.Local)
            .WithMany(l => l.Favoritos)
            .HasForeignKey(mf => mf.LocalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
