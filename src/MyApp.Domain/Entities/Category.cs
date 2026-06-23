namespace MyApp.Domain.Entities;

/// <summary>
/// 分類 Entity。
///
/// Domain 專案建議保持乾淨：
/// 1. 不依賴 EF Core。
/// 2. 不寫資料表名稱。
/// 3. 不寫欄位長度。
/// 4. 不寫索引。
///
/// 這些資料庫規則會放在 Infrastructure 的 Configuration。
/// </summary>
public class Category
{
    /// <summary>
    /// Primary Key。
    ///
    /// EF Core convention 通常會把 CategoryId 視為主鍵。
    /// 不過我們仍會在 Configuration 裡明確寫 HasKey。
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// 分類名稱。
    ///
    /// string.Empty 是 C# 層級預設值，
    /// 用來避免 nullable reference warning。
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// C# 層級的建立時間預設值。
    ///
    /// 注意：
    /// 這不是資料庫 default constraint。
    /// 若要資料庫自動產生時間，請在 Configuration 設定 HasDefaultValueSql。
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// 一個 Category 有多篇 Post。
    /// </summary>
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
