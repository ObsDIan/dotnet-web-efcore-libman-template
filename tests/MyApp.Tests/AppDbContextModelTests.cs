using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApp.Domain.Entities;
using MyApp.Infrastructure;
using MyApp.Infrastructure.Data;

namespace MyApp.Tests;

public class AppDbContextModelTests
{
    [Fact]
    public void Model_applies_entity_configurations()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=ModelTest;Trusted_Connection=True")
            .Options;

        using var context = new AppDbContext(options);

        var category = context.Model.FindEntityType(typeof(Category));
        var post = context.Model.FindEntityType(typeof(Post));

        Assert.NotNull(category);
        Assert.NotNull(post);
        Assert.Equal("Categories", category.GetTableName());
        Assert.Equal("Posts", post.GetTableName());
        Assert.Equal(100, category.FindProperty(nameof(Category.Name))?.GetMaxLength());
        Assert.Equal(200, post.FindProperty(nameof(Post.Title))?.GetMaxLength());
        Assert.Contains(category.GetIndexes(), index => index.IsUnique);
    }

    [Fact]
    public void AddInfrastructure_registers_AppDbContext()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] =
                    "Server=(localdb)\\MSSQLLocalDB;Database=RegistrationTest;Trusted_Connection=True"
            })
            .Build();

        var services = new ServiceCollection();
        services.AddInfrastructure(configuration);

        using var provider = services.BuildServiceProvider();
        using var scope = provider.CreateScope();

        Assert.NotNull(scope.ServiceProvider.GetService<AppDbContext>());
    }
}
