using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MesaApi.Domain.Entities;

namespace MesaApi.Infrastructure.Data.Configurations;

public class RequestDataConfiguration : IEntityTypeConfiguration<RequestData>
{
    public void Configure(EntityTypeBuilder<RequestData> builder)
    {
        builder.ToTable("pro_datossolicitud");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("iddato");

        builder.Property(e => e.RequestId)
            .HasColumnName("idsolicitud")
            .IsRequired();

        builder.Property(e => e.Name)
            .HasColumnName("nombre")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Value)
            .HasColumnName("dato")
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(e => e.DataType)
            .HasColumnName("tipo")
            .HasMaxLength(50);

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
            .WithMany(r => r.RequestData)
            .HasForeignKey(e => e.RequestId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => e.RequestId);
        builder.HasIndex(e => e.Name);
    }
}