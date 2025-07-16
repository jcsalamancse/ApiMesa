using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MesaApi.Domain.Entities;

namespace MesaApi.Infrastructure.Data.Configurations;

public class WorkflowConfiguration : IEntityTypeConfiguration<Workflow>
{
    public void Configure(EntityTypeBuilder<Workflow> builder)
    {
        builder.ToTable("pro_workflow");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("idworkflow");

        builder.Property(e => e.Name)
            .HasColumnName("nombre")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasColumnName("descripcion")
            .HasMaxLength(500);

        builder.Property(e => e.Category)
            .HasColumnName("categoria")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .HasColumnName("activo")
            .HasDefaultValue(true);

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

        // Indexes
        builder.HasIndex(e => e.Name).IsUnique();
        builder.HasIndex(e => e.Category);
        builder.HasIndex(e => e.IsActive);
    }
}

public class WorkflowStepConfiguration : IEntityTypeConfiguration<WorkflowStep>
{
    public void Configure(EntityTypeBuilder<WorkflowStep> builder)
    {
        builder.ToTable("pro_workflow_paso");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("idpaso");

        builder.Property(e => e.WorkflowId)
            .HasColumnName("idworkflow")
            .IsRequired();

        builder.Property(e => e.StepName)
            .HasColumnName("nombre_paso")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.StepType)
            .HasColumnName("tipo_paso")
            .HasMaxLength(50);

        builder.Property(e => e.Order)
            .HasColumnName("orden")
            .IsRequired();

        builder.Property(e => e.RoleId)
            .HasColumnName("idrol");

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
        builder.HasOne(e => e.Workflow)
            .WithMany(w => w.Steps)
            .HasForeignKey(e => e.WorkflowId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Role)
            .WithMany()
            .HasForeignKey(e => e.RoleId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(e => e.WorkflowId);
        builder.HasIndex(e => e.RoleId);
        builder.HasIndex(e => e.Order);
    }
}