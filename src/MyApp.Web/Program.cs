using MyApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// MVC。
builder.Services.AddControllersWithViews();

// 註冊 Infrastructure。
// DbContext 實際在 MyApp.Infrastructure 類別庫裡，
// 但類別庫不會執行，所以由 Web 啟動專案呼叫註冊方法。
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapDefaultControllerRoute();

app.Run();
