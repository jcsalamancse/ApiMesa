using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MesaApi.Domain.Entities;

namespace MesaApi.Infrastructure.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("pro_comentarios");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("idcomentario");

        builder.Property(e => e.RequestId)
            .HasColumnName("idsolicitud")
            .IsRequired();

        builder.Property(e => e.UserId)
            .HasColumnName("idusuario")
            .IsRequired();

        builder.Property(e => e.Content)
            .HasColumnName("comentario")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(e => e.IsInternal)
            .HasColumnName("es_interno")
            .HasDefaultValue(false);

        builder.Property(e => e.CreatedAt)
            .HasColumnName("fecha")
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(e => e.UpdatedAt)
            .HasColumnName("updated_at");

        builder.Property(e => e.CreatedBy)
            .HasColumnName("created_by")
            .HasMaxLength(50);

        builder.Property(e => e.UpdatedBy)
            .HasColumnName("updated_by")
            .HasMaxLength(50);

        builder.Property(e => e.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        // Relationships
        builder.HasOne(e => e.Request)
            .WithMany(r => r.Comments)
            .HasForeignKey(e => e.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(e => e.RequestId);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.CreatedAt);
    }
}