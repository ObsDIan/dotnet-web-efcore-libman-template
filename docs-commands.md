# 常用指令

## 還原 .NET 套件

```bash
dotnet restore MyApp.sln
```

## 還原 local tools

```bash
dotnet tool restore
```

## 還原 LibMan

```bash
cd src/MyApp.Web
dotnet libman restore
cd ../..
```

## 建立 migration

```bash
dotnet ef migrations add InitialCreate \
  --project src/MyApp.Infrastructure \
  --startup-project src/MyApp.Web \
  --context AppDbContext
```

## 更新資料庫

```bash
dotnet ef database update \
  --project src/MyApp.Infrastructure \
  --startup-project src/MyApp.Web \
  --context AppDbContext
```

## 執行 Web 專案

```bash
dotnet run --project src/MyApp.Web
```

## 建置與測試

```bash
dotnet build MyApp.sln --configuration Release --no-restore
dotnet test MyApp.sln --configuration Release --no-build
```
