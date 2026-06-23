namespace MyApp.Domain.Entities;

/// <summary>
/// 文章 Entity。
/// </summary>
public class Post
{
    /// <summary>
    /// Primary Key。
    /// </summary>
    public int PostId { get; set; }

    /// <summary>
    /// 文章標題。
    /// 欄位長度與必填規則放在 PostConfiguration。
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 文章內容。
    /// 長文字欄位型態放在 PostConfiguration。
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 是否發布。
    ///
    /// 這裡是 C# 層級預設值。
    /// 資料庫層級預設值放在 PostConfiguration 的 HasDefaultValue。
    /// </summary>
    public bool IsPublished { get; set; } = false;

    /// <summary>
    /// 建立時間。
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Foreign Key。
    ///
    /// Post 是 dependent entity。
    /// Category 是 principal entity。
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Navigation property。
    ///
    /// null! 表示此屬性會由 EF Core 或程式邏輯設定，
    /// 避免 nullable reference warning。
    /// </summary>
    public Category Category { get; set; } = null!;
}
