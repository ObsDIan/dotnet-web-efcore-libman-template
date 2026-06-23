using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;

namespace MyApp.Web.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _dbContext;

    public HomeController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        // 簡單示範從 DbContext 查資料。
        // Include 會一起載入 Category 底下的 Posts。
        var categories = await _dbContext.Categories
            .Include(e => e.Posts)
            .ToListAsync();

        return View(categories);
    }
}
