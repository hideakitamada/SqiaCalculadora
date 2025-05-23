using SqiaCalculadora.Models;

namespace SqiaCalculadora.Repositories;

public interface ICotacaoRepository
{
    Task<Cotacao?> ObterCotacaoPorDataAsync(DateTime data, string indexador);
}