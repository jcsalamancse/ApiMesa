using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MesaApi.Domain.Entities;

namespace MesaApi.Infrastructure.Data.Configurations;

public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
{
    public void Configure(EntityTypeBuilder<Attachment> builder)
    {
        builder.ToTable("pro_adjuntos");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("idadjunto");

        builder.Property(e => e.RequestId)
            .HasColumnName("idsolicitud")
            .IsRequired();

        builder.Property(e => e.FileName)
            .HasColumnName("nombre_archivo")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.FilePath)
            .HasColumnName("ruta_archivo")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.ContentType)
            .HasColumnName("tipo_contenido")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.FileSize)
            .HasColumnName("tamano")
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasColumnName("created_at")
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
            .WithMany(r => r.Attachments)
            .HasForeignKey(e => e.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => e.RequestId);
        builder.HasIndex(e => e.FileName);
    }
}