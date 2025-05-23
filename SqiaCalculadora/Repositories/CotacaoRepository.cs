using Microsoft.EntityFrameworkCore;
using SqiaCalculadora.Data;
using SqiaCalculadora.Models;

namespace SqiaCalculadora.Repositories;

public class CotacaoRepository(ApplicationDbContext context, ILogger<CotacaoRepository> logger) : ICotacaoRepository
{
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<CotacaoRepository> _logger = logger;

    public async Task<Cotacao?> ObterCotacaoPorDataAsync(DateTime data, string indexador)
    {
        try
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
            return await _context.Cotacoes
                .FirstOrDefaultAsync(c => c.Data == data && c.Indexador == indexador, cts.Token);
        }
        catch (Exception)
        {
            _logger.LogWarning("Erro ao consultar cotação para {Data}", data);
            return null;
        }
    }
}