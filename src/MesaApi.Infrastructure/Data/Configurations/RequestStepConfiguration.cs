using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MesaApi.Domain.Entities;
using MesaApi.Domain.Enums;

namespace MesaApi.Infrastructure.Data.Configurations;

public class RequestStepConfiguration : IEntityTypeConfiguration<RequestStep>
{
    public void Configure(EntityTypeBuilder<RequestStep> builder)
    {
        builder.ToTable("pro_pasosolicitud");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("idpaso");

        builder.Property(e => e.RequestId)
            .HasColumnName("idsolicitud")
            .IsRequired();

        builder.Property(e => e.StepName)
            .HasColumnName("pasoproceso")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.StepType)
            .HasColumnName("tipopaso")
            .HasMaxLength(50);

        builder.Property(e => e.Order)
            .HasColumnName("orden")
            .IsRequired();

        builder.Property(e => e.Status)
            .HasColumnName("estado")
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(StepStatus.Pending);

        builder.Property(e => e.AssignedToId)
            .HasColumnName("realizadopor");

        builder.Property(e => e.RoleId)
            .HasColumnName("idrol");

        builder.Property(e => e.CompletedAt)
            .HasColumnName("fecha");

        builder.Property(e => e.Notes)
            .HasColumnName("notas")
            .HasMaxLength(500);

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
            .WithMany(r => r.Steps)
            .HasForeignKey(e => e.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.AssignedTo)
            .WithMany(u => u.RequestSteps)
            .HasForeignKey(e => e.AssignedToId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(e => e.Role)
            .WithMany(r => r.RequestSteps)
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(e => e.RequestId);
        builder.HasIndex(e => e.AssignedToId);
        builder.HasIndex(e => e.RoleId);
        builder.HasIndex(e => e.Order);
    }
}