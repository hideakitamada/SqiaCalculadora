using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Polly;
using SqiaCalculadora.Data;
using SqiaCalculadora.Models;
using SqiaCalculadora.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SqiaCalculadora.Tests.Repositories;

public class CotacaoRepositoryTests
{
    private readonly ApplicationDbContext _context;
    private readonly CotacaoRepository _repository;

    public CotacaoRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "CotacaoTestDb")
            .Options;

        _context = new ApplicationDbContext(options);
        _context.Cotacoes.Add(new Cotacao
        {
            Data = new DateTime(2025, 5, 13),
            Indexador = "SQI",
            Valor = 12.5m
        });
        _context.SaveChanges();

        var logger = new Mock<ILogger<CotacaoRepository>>();
        _repository = new CotacaoRepository(_context, logger.Object);
    }

    [Fact]
    public async Task ObterCotacaoPorDataAsync_DeveRetornarCotacaoExistente()
    {
        var cotacao = await _repository.ObterCotacaoPorDataAsync(new DateTime(2025, 5, 13), "SQI");
        Assert.NotNull(cotacao);
        Assert.Equal(12.5m, cotacao!.Valor);
        Assert.True(cotacao.Id > 0);
    }

    [Fact]
    public async Task ObterCotacaoPorDataAsync_DeveRetornarNull_SeNaoExistir()
    {
        var cotacao = await _repository.ObterCotacaoPorDataAsync(new DateTime(2020, 1, 1), "SQI");
        Assert.Null(cotacao);
    }
}