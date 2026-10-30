# ASP.NET Core + EF Core + LibMan Template

[![CI](https://github.com/ObsDIan/dotnet-web-efcore-libman-template/actions/workflows/ci.yml/badge.svg)](https://github.com/ObsDIan/dotnet-web-efcore-libman-template/actions/workflows/ci.yml)

可直接 clone、build、test 的 .NET 10 LTS 多專案範本，示範 ASP.NET Core MVC、EF Core、SQL Server、Fluent API、依賴注入與 LibMan。

## 快速開始

需求：

- .NET 10 SDK
- SQL Server LocalDB（執行 migration 或 Web 專案時需要）

```bash
git clone https://github.com/ObsDIan/dotnet-web-efcore-libman-template.git
cd dotnet-web-efcore-libman-template
dotnet tool restore
cd src/MyApp.Web
dotnet libman restore
cd ../..
dotnet restore MyApp.sln
dotnet build MyApp.sln --no-restore
dotnet test MyApp.sln --no-build
dotnet run --project src/MyApp.Web
```

> `appsettings.json` 使用 Windows LocalDB。若使用其他環境，請調整 `ConnectionStrings:DefaultConnection`。

## 專案結構

這份範本示範一個常見的 .NET 專案拆法：

```text
src/
  MyApp.Web/             # 啟動專案，負責 Program.cs、appsettings.json、LibMan、wwwroot
  MyApp.Infrastructure/  # 類別庫，負責 DbContext、Configurations、Migrations
  MyApp.Domain/          # 類別庫，負責 Entity / Domain Model
```

## 分層職責

### MyApp.Web

這個專案會執行，所以放：

- `Program.cs`
- `appsettings.json`
- `libman.json`
- `wwwroot`
- Controllers / Views / Pages / Components
- 呼叫 `builder.Services.AddInfrastructure(...)`

### MyApp.Infrastructure

這是類別庫，本身不會執行，所以放：

- `AppDbContext`
- EF Core Configuration
- EF Core Migrations
- `DependencyInjection.cs`

類別庫不放 `Program.cs`。  
類別庫本身也不負責讀取 appsettings。  
它提供一個 `AddInfrastructure` 方法，給 Web 專案呼叫。

### MyApp.Domain

這是類別庫，放最單純的 Entity。

建議不要讓 Domain 依賴 EF Core。  
欄位長度、資料表名稱、索引、FK delete behavior 放到 Infrastructure 的 Configuration。

---

## 從零建立 solution

```bash
dotnet new sln -n MyApp

dotnet new mvc -n MyApp.Web -o src/MyApp.Web
dotnet new classlib -n MyApp.Infrastructure -o src/MyApp.Infrastructure
dotnet new classlib -n MyApp.Domain -o src/MyApp.Domain

dotnet sln add src/MyApp.Web/MyApp.Web.csproj
dotnet sln add src/MyApp.Infrastructure/MyApp.Infrastructure.csproj
dotnet sln add src/MyApp.Domain/MyApp.Domain.csproj
```

## 專案參考

```bash
dotnet add src/MyApp.Web/MyApp.Web.csproj reference src/MyApp.Infrastructure/MyApp.Infrastructure.csproj
dotnet add src/MyApp.Infrastructure/MyApp.Infrastructure.csproj reference src/MyApp.Domain/MyApp.Domain.csproj
```

## 套件安裝

Infrastructure：

```bash
dotnet add src/MyApp.Infrastructure/MyApp.Infrastructure.csproj package Microsoft.EntityFrameworkCore
dotnet add src/MyApp.Infrastructure/MyApp.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer
dotnet add src/MyApp.Infrastructure/MyApp.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design
```

Web：

```bash
dotnet add src/MyApp.Web/MyApp.Web.csproj package Microsoft.EntityFrameworkCore.Design
```

## EF Core Migration

因為 DbContext 在 `MyApp.Infrastructure`，但啟動專案是 `MyApp.Web`，所以指令要指定兩個專案：

```bash
dotnet ef migrations add InitialCreate \
  --project src/MyApp.Infrastructure \
  --startup-project src/MyApp.Web \
  --context AppDbContext
```

更新資料庫：

```bash
dotnet ef database update \
  --project src/MyApp.Infrastructure \
  --startup-project src/MyApp.Web \
  --context AppDbContext
```

## Package Manager Console 寫法

```powershell
Add-Migration InitialCreate `
  -Project MyApp.Infrastructure `
  -StartupProject MyApp.Web `
  -Context AppDbContext
```

```powershell
Update-Database `
  -Project MyApp.Infrastructure `
  -StartupProject MyApp.Web `
  -Context AppDbContext
```

---

## LibMan

LibMan 建議放在 Web 專案，因為一般 Class Library 不會執行，也通常不會有 `wwwroot`。

本範本放在：

```text
src/MyApp.Web/libman.json
```

建議 Git 提交：

```text
src/MyApp.Web/libman.json
.config/dotnet-tools.json
```

建議 Git 忽略：

```text
src/MyApp.Web/wwwroot/lib/
```

## 使用 local tool

第一次建立 tool manifest：

```bash
dotnet new tool-manifest
dotnet tool install Microsoft.Web.LibraryManager.Cli
```

其他人 clone 專案後：

```bash
dotnet tool restore
cd src/MyApp.Web
dotnet libman restore
```

也可以進入 Web 專案執行：

```bash
cd src/MyApp.Web
dotnet libman restore
```

---

## 設計原則

官方支援：

- `DbContext`
- `DbSet`
- Fluent API
- `IEntityTypeConfiguration<TEntity>`
- `ApplyConfigurationsFromAssembly`
- `AddDbContext`
- EF Core Migrations
- LibMan `libman.json`

團隊常見慣例，但不是官方強制：

- Entity 放 Domain
- DbContext 放 Infrastructure
- 每個 Entity 拆一個 Configuration
- Web 專案呼叫 Infrastructure 提供的 `AddInfrastructure`
- LibMan 放 Web，不放一般 Class Library
- `wwwroot/lib` 不提交 Git，透過 LibMan 還原

## 自動化驗證

GitHub Actions 會在 push 與 pull request 時執行工具還原、LibMan 還原、套件還原、Release build 與測試。

## 使用方式

這是教學與專案起始範本。建立新專案時，請重新命名 `MyApp`、調整 connection string，並依需求加入 authentication、logging、health checks 與部署設定。
