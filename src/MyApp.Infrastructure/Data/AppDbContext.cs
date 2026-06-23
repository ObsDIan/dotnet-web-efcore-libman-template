using Microsoft.EntityFrameworkCore;
using MyApp.Domain.Entities;

namespace MyApp.Infrastructure.Data;

/// <summary>
/// EF Core DbContext。
///
/// 這個類別放在 Infrastructure 類別庫。
///
/// DbContext 的責任：
/// 1. 代表一次資料庫工作階段。
/// 2. 管理查詢、追蹤、儲存變更。
/// 3. 告訴 EF Core 有哪些 DbSet。
/// 4. 套用 Fluent API Configuration。
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// 讓 ASP.NET Core DI 可以注入 DbContextOptions。
    ///
    /// DbContextOptions 會在 Web 專案的 Program.cs，
    /// 透過 AddInfrastructure -> AddDbContext 建立。
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Categories 資料表。
    ///
    /// 使用 => Set<Category>() 的寫法，
    /// 可以避免 nullable warning。
    /// </summary>
    public DbSet<Category> Categories => Set<Category>();

    /// <summary>
    /// Posts 資料表。
    /// </summary>
    public DbSet<Post> Posts => Set<Post>();

    /// <summary>
    /// EF Core 建立模型時會呼叫。
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 自動套用 MyApp.Infrastructure assembly 裡
        // 所有 IEntityTypeConfiguration<TEntity>。
        //
        // 例如：
        // - CategoryConfiguration
        // - PostConfiguration
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
