using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Infrastructure.Data;

namespace MyApp.Infrastructure;

/// <summary>
/// Infrastructure 專案提供給啟動專案呼叫的 DI 註冊入口。
///
/// 類別庫本身不會執行，所以不會有 Program.cs。
/// Web 專案才會在 Program.cs 呼叫：
///
/// builder.Services.AddInfrastructure(builder.Configuration);
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// 註冊 Infrastructure 相關服務。
    ///
    /// 目前包含：
/// - AppDbContext
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString,

                // 指定 Migrations 要放在 AppDbContext 所在的 assembly。
                // 也就是 MyApp.Infrastructure。
                //
                // 如果不指定，有時 EF Core 會依照啟動專案或 provider 設定推斷，
                // 對多專案架構來說明確寫出來比較不容易混亂。
                sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            );

            // 開發階段需要看更詳細錯誤時可以打開。
            // 正式環境不建議開啟 SensitiveDataLogging，
            // 因為可能把密碼、token、個資印到 log。
            //
            // options.EnableDetailedErrors();
            // options.EnableSensitiveDataLogging();
        });

        return services;
    }
}
