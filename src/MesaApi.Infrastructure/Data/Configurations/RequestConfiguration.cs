using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Enums;

namespace MesaApi.Infrastructure.Data.Configurations;

public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.ToTable("pro_solicitud");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("idsolicitud");

        builder.Property(e => e.Description)
            .HasColumnName("descripcion")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(e => e.Status)
            .HasColumnName("estado")
            .HasConversion<int>()
            .HasDefaultValue(RequestStatus.Pending);

        builder.Property(e => e.Priority)
            .HasColumnName("prioridad")
            .HasConversion<int>()
            .HasDefaultValue(RequestPriority.Medium);

        builder.Property(e => e.Category)
            .HasColumnName("categoria")
            .HasMaxLength(100);

        builder.Property(e => e.SubCategory)
            .HasColumnName("subcategoria")
            .HasMaxLength(100);

        builder.Property(e => e.RequesterId)
            .HasColumnName("idusuario_solicitante")
            .IsRequired();

        builder.Property(e => e.AssignedToId)
            .HasColumnName("idusuario_asignado");

        builder.Property(e => e.DueDate)
            .HasColumnName("fecha_vencimiento");

        builder.Property(e => e.CompletedAt)
            .HasColumnName("fecha_completado");

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
        builder.HasOne(e => e.Requester)
            .WithMany(u => u.Requests)
            .HasForeignKey(e => e.RequesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.AssignedTo)
            .WithMany()
            .HasForeignKey(e => e.AssignedToId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.Priority);
        builder.HasIndex(e => e.RequesterId);
        builder.HasIndex(e => e.AssignedToId);
        builder.HasIndex(e => e.CreatedAt);
    }
}