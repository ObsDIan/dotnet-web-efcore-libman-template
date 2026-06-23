using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Entities;

namespace MyApp.Infrastructure.Data.Configurations;

/// <summary>
/// Post 的資料庫設定。
/// </summary>
public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> entity)
    {
        entity.ToTable("Posts");

        entity.HasKey(e => e.PostId);

        entity.Property(e => e.Title)
            .HasMaxLength(200)
            .IsRequired();

        entity.Property(e => e.Content)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        // 資料庫層級預設值。
        // 如果 INSERT 時沒有提供 IsPublished，資料庫會給 false。
        entity.Property(e => e.IsPublished)
            .HasDefaultValue(false);

        entity.Property(e => e.CreatedAt)
            .HasColumnType("datetime2");

        // 如果常用 Title 查詢，可以建立 index。
        // 注意：index 會提升查詢，但會增加新增/修改成本。
        entity.HasIndex(e => e.Title);

        // 一對多：
        // Category 1 ---- many Post
        entity.HasOne(e => e.Category)
            .WithMany(e => e.Posts)
            .HasForeignKey(e => e.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
