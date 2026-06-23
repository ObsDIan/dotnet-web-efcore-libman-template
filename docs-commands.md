# 常用指令

## 還原 .NET 套件

```bash
dotnet restore
```

## 還原 local tools

```bash
dotnet tool restore
```

## 還原 LibMan

```bash
dotnet libman restore --manifest src/MyApp.Web/libman.json
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
