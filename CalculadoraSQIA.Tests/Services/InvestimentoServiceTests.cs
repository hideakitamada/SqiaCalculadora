using Microsoft.Extensions.Logging;
using Moq;
using SqiaCalculadora.Models;
using SqiaCalculadora.Repositories;
using SqiaCalculadora.Services;

namespace SqiaCalculadora.Tests.Services;

public class InvestimentoServiceTests
{
    private readonly Mock<ICotacaoRepository> _cotacaoRepositoryMock;
    private readonly Mock<ILogger<InvestimentoService>> _loggerMock;
    private readonly InvestimentoService _service;

    public InvestimentoServiceTests()
    {
        _cotacaoRepositoryMock = new Mock<ICotacaoRepository>();
        _loggerMock = new Mock<ILogger<InvestimentoService>>();
        _service = new InvestimentoService(_cotacaoRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CalcularAsync_DeveRetornarValorEsperado()
    {
        var request = new InvestimentoRequest
        {
            ValorAplicado = 10000m,
            DataAplicacao = new DateTime(2025, 3, 13),
            DataFinal = new DateTime(2025, 3, 13)
        };

        _cotacaoRepositoryMock.Setup(r => r.ObterCotacaoPorDataAsync(
                new DateTime(2025, 3, 13), "SQI"))
            .ReturnsAsync(new Cotacao { Valor = 12.50m });

        var resultado = await _service.CalcularAsync(request);

        Assert.NotNull(resultado);
        Assert.Equal(1, resultado.FatorAcumulado);
        Assert.Equal(resultado.ValorAtualizado, request.ValorAplicado);
    }

    [Fact]
    public async Task CalcularAsync_DeveLancarExcecao_SeCotacaoForNula()
    {
        var request = new InvestimentoRequest
        {
            ValorAplicado = 10000m,
            DataAplicacao = new DateTime(2025, 5, 13),
            DataFinal = new DateTime(2025, 5, 20)
        };

        _cotacaoRepositoryMock.Setup(r => r.ObterCotacaoPorDataAsync(
                It.IsAny<DateTime>(), "SQI"))
            .ReturnsAsync((Cotacao?)null);

        var resultado = _service.CalcularAsync(request);
        
        Assert.NotNull(resultado.Exception);
        Assert.Equal("One or more errors occurred. (Sem cotação para o dia 2025-05-13)", resultado.Exception.Message);
    }

    [Fact]
    public async Task CalcularAsync_DeveManterPrecisaoDeTruncamento()
    {
        var request = new InvestimentoRequest
        {
            ValorAplicado = 10000m,
            DataAplicacao = new DateTime(2025, 3, 13),
            DataFinal = new DateTime(2025, 3, 30)
        };

        _cotacaoRepositoryMock.Setup(r => r.ObterCotacaoPorDataAsync(It.IsAny<DateTime>(), "SQI"))
            .ReturnsAsync(new Cotacao { Valor = 9.75m });

        var result = await _service.CalcularAsync(request);

        Assert.NotNull(result);
        Assert.True(result.FatorAcumulado > 1);
        Assert.Equal(Math.Truncate(result.ValorAtualizado * 100000000) / 100000000, result.ValorAtualizado);
    }

    [Fact]
    public async Task CalcularAsync_DeveLancarExcecao_SeCotacaoNaoExistir()
    {
        var request = new InvestimentoRequest
        {
            ValorAplicado = 8000m,
            DataAplicacao = new DateTime(2025, 5, 13),
            DataFinal = new DateTime(2025, 5, 30)
        };

        _cotacaoRepositoryMock.Setup(r => r.ObterCotacaoPorDataAsync(It.IsAny<DateTime>(), "SQI"))
            .ReturnsAsync((Cotacao?)null);

        var ex = await Assert.ThrowsAsync<Exception>(() => _service.CalcularAsync(request));
        Assert.Contains("Sem cotação", ex.Message);
    }
}