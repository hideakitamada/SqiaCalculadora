using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SqiaCalculadora.Controllers;
using SqiaCalculadora.Models;
using SqiaCalculadora.Services;

namespace SqiaCalculadora.Tests.Controllers;

public class InvestimentoControllerTests
{
    [Fact]
    public async Task Calcular_DeveRetornarOk_QuandoServicoRetornaSucesso()
    {
        var mockService = new Mock<IInvestimentoService>();
        var mockLogger = new Mock<ILogger<InvestimentoController>>();

        var request = new InvestimentoRequest
        {
            ValorAplicado = 1000m,
            DataAplicacao = DateTime.Today.AddDays(-3),
            DataFinal = DateTime.Today
        };

        var esperado = new InvestimentoResponse { FatorAcumulado = 1.05m, ValorAtualizado = 1050m };
        mockService.Setup(s => s.CalcularAsync(request)).ReturnsAsync(esperado);

        var controller = new InvestimentoController(mockService.Object, mockLogger.Object);
        var result = await controller.Calcular(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var valor = Assert.IsType<InvestimentoResponse>(okResult.Value);
        Assert.Equal(esperado.ValorAtualizado, valor.ValorAtualizado);
    }

    [Fact]
    public async Task Calcular_DeveRetornarBadRequest_QuandoServicoLancarExcecao()
    {
        var mockService = new Mock<IInvestimentoService>();
        var mockLogger = new Mock<ILogger<InvestimentoController>>();
        mockService.Setup(s => s.CalcularAsync(It.IsAny<InvestimentoRequest>()))
                   .ThrowsAsync(new Exception("Erro simulado"));

        var controller = new InvestimentoController(mockService.Object, mockLogger.Object);
        var result = await controller.Calcular(new InvestimentoRequest());

        Assert.IsType<BadRequestObjectResult>(result);
    }
}