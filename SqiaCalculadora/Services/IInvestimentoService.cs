using SqiaCalculadora.Models;

namespace SqiaCalculadora.Services;

public interface IInvestimentoService
{
    Task<InvestimentoResponse> CalcularAsync(InvestimentoRequest request);
}