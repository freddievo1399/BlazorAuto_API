
using BlazorAuto_API.AbstractServer;
using BlazorAuto_API.Shopping.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlazorAuto_API.BlazorAuto_API.Shopping.Server;
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
    [HttpGet(nameof(returnString))]
    public async Task<string> returnString(int input)
    {
        await Task.Delay(10);
        var abc = await _applicationDbContext.Set<EntityProduct>().ToListAsync();
        return input.ToString();
    }
}
