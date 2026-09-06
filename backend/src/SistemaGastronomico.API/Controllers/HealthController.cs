namespace SistemaGastronomico.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using SistemaGastronomico.Infrastructure.Persistence;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HealthController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> CheckHealth()
    {
        bool dbCanConnect = false;
        try
        {
            dbCanConnect = await _context.Database.CanConnectAsync();
        }
        catch
        {
            dbCanConnect = false;
        }

        return Ok(new
        {
            Status = "Healthy",
            Service = "Sistema Gastronómico UTA - Backend API",
            Version = "1.0.0",
            DatabaseConnected = dbCanConnect,
            ServerTimeUtc = DateTime.UtcNow
        });
    }
}
