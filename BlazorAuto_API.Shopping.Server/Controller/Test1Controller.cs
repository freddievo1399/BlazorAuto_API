
using System.Data;
using System.Linq;
using BlazorAuto_API.AbstractServer;
using BlazorAuto_API.Shopping.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace BlazorAuto_API.Shopping.Server;
[ApiController]
[Route("/api/base/[controller]")]
public class Test1Controller : ControllerBase, ITest1
{
    private readonly ILogger<Test1Controller> _logger;
    ApplicationDbContext _applicationDbContext;
    public Test1Controller(ILogger<Test1Controller> logger, ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        _logger = logger;
    }
    [HttpGet(nameof(ReturnString))]
    public async Task<string> ReturnString(int input)
    {
        await Task.Delay(10);
        var abc = await _applicationDbContext.Set<EntityProduct>().ToListAsync();
        return input.ToString();
    }
    [HttpPost(nameof(TestQuery))]
    public async Task<string> TestQuery([FromBody] DataManagerRequest input)
    {
        List<EntityProduct> products = new List<EntityProduct>();
        using (var db = _applicationDbContext.Connection())
        {
            products = await db.GetData<EntityProduct>(input).ToListAsync();
            if (products.Count == 0)
            {
                await db.Set<EntityProduct>().AddAsync(new()
                {
                    CreatedAt = DateTime.Now,
                    CreatedBy = "System",
                    DeletedAt = null,
                    DeletedBy = null,
                    Description = "Test Product",
                    Name = "Test Product",
                     Guid = Guid.NewGuid(),
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = "System",
                });
                return await TestQuery(input);
            }
        }
        return products.Count.ToString();
    }
}
