using ShortURLDTE.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Authorization;

namespace FacturaShortener.API.Controllers
{
[ApiController]
[Route("api/facturas")]
public class FacturaController : ControllerBase
{
    private readonly ISubirFacturaUseCase _subirFactura;
    private readonly IConsultarFacturaUseCase _consultarFactura;

    public FacturaController(ISubirFacturaUseCase subirFactura, IConsultarFacturaUseCase consultarFactura)
    {
        _subirFactura = subirFactura;
        _consultarFactura = consultarFactura;
    }

    [HttpPost("subir")]
    [Consumes("multipart/form-data")]
    [Authorize]
    public async Task<IActionResult> SubirFactura([FromForm] IFormFile archivoXml)
    {
        if (archivoXml == null || archivoXml.Length == 0)
            return BadRequest("Debes subir un archivo XML válido.");

            var result = await _subirFactura.HandleAsync(archivoXml);
        if (!result.Success)
            return BadRequest(result.ErrorMessage);

        return Ok(new { shortUrl = result.ShortUrl });
    }

    [HttpGet("{shortUrl}")]
    [Authorize] // para validar que se encuentre autenticado con Token JWT
    public async Task<IActionResult> ConsultarFactura(string shortUrl)
    {
        var factura = await _consultarFactura.HandleAsync(shortUrl);
        if (factura == null)
            return NotFound("Factura no encontrada o límite de usos alcanzado");

        return Ok(factura);
    }
}
}

