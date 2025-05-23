using Microsoft.AspNetCore.Mvc;
using SqiaCalculadora.Models;
using SqiaCalculadora.Services;

namespace SqiaCalculadora.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvestimentoController(IInvestimentoService service, ILogger<InvestimentoController> logger) : ControllerBase
{
    private readonly IInvestimentoService _service = service;
    private readonly ILogger<InvestimentoController> _logger = logger;

    [HttpPost]
    public async Task<IActionResult> Calcular([FromBody] InvestimentoRequest request)
    {
        _logger.LogInformation("[InvestimentoController] Requisição recebida: {@Request}", request);

        try
        {
            var resultado = await _service.CalcularAsync(request);
            _logger.LogInformation("[InvestimentoController] Resultado calculado: {@Resultado}", resultado);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao calcular investimento para requisição: {@Request}", request);
            return BadRequest(new { erro = ex.Message });
        }
    }
}