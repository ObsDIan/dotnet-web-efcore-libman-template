using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyApp.Domain.Entities;

namespace MyApp.Infrastructure.Data.Configurations;

/// <summary>
/// Category 的資料庫設定。
///
/// 建議固定順序：
/// 1. ToTable
/// 2. HasKey
/// 3. Property
/// 4. HasIndex
/// 5. Relationship
/// 6. HasData
/// </summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> entity)
    {
        // 資料表名稱。
        entity.ToTable("Categories");

        // Primary Key。
        entity.HasKey(e => e.CategoryId);

        // 分類名稱。
        entity.Property(e => e.Name)
            .HasMaxLength(100)
            .IsRequired();

        // 分類名稱不重複。
        entity.HasIndex(e => e.Name)
            .IsUnique();

        // 使用 datetime2，比 SQL Server 舊 datetime 更精準。
        entity.Property(e => e.CreatedAt)
            .HasColumnType("datetime2");

        // 固定種子資料可以用 HasData。
        // 注意：HasData 比較適合固定資料，例如角色、狀態碼、分類常數。
        entity.HasData(
            new Category
            {
                CategoryId = 1,
                Name = "General",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Category
            {
                CategoryId = 2,
                Name = "News",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
