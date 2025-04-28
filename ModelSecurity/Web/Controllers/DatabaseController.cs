using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entity.Context;

[Route("api/[controller]")]
[ApiController]
public class DatabaseController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DatabaseController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("test-connection")]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            // Intenta hacer una consulta simple a la base de datos
            var canConnect = await _context.Database.CanConnectAsync();

            if (canConnect)
                return Ok("✅ Conexión exitosa con la base de datos.");
            else
                return StatusCode(500, "❌ No se pudo conectar a la base de datos.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"❌ Error al conectar: {ex.Message}");
        }
    }
}
