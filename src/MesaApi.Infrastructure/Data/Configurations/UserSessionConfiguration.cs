using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MesaApi.Domain.Entities;

namespace MesaApi.Infrastructure.Data.Configurations;

public class UserSessionConfiguration : IEntityTypeConfiguration<UserSession>
{
    public void Configure(EntityTypeBuilder<UserSession> builder)
    {
        builder.ToTable("sys_sessiones");

        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("idsession");

        builder.Property(e => e.UserId)
            .HasColumnName("idusuario")
            .IsRequired();

        builder.Property(e => e.SessionId)
            .HasColumnName("sid")
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(e => e.Login)
            .HasColumnName("login")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.ExpiresAt)
            .HasColumnName("expira")
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

        // Relationships
        builder.HasOne(e => e.User)
            .WithMany(u => u.Sessions)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(e => e.SessionId);
        builder.HasIndex(e => e.Login);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.IsActive);
    }
}