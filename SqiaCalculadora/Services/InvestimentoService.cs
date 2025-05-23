using SqiaCalculadora.Models;
using SqiaCalculadora.Repositories;
using SqiaCalculadora.Utils;

namespace SqiaCalculadora.Services;
public class InvestimentoService(ICotacaoRepository cotacaoRepository, ILogger<InvestimentoService> logger) : IInvestimentoService
{
    private readonly ICotacaoRepository _cotacaoRepository = cotacaoRepository;
    private readonly ILogger<InvestimentoService> _logger = logger;

    public async Task<InvestimentoResponse> CalcularAsync(InvestimentoRequest request)
    {
        _logger.LogInformation("Iniciando cálculo para valor {Valor}, de {Inicio} até {Fim}",
            request.ValorAplicado, request.DataAplicacao, request.DataFinal);

        var diasUteis = DiasUteisHelper.DiasUteisEntre(request.DataAplicacao, request.DataFinal).ToList();
        decimal fatorAcumulado = 1m;

        foreach (var dia in diasUteis)
        {
            var cotacao = await _cotacaoRepository.ObterCotacaoPorDataAsync(dia, "SQI");

            if (cotacao == null)
            {
                _logger.LogWarning("Sem cotação para o dia {DiaAnterior}", dia);
                throw new Exception($"Sem cotação para o dia {dia:yyyy-MM-dd}");
            }

            var taxaAnual = cotacao.Valor;
            var fatorDiario = Math.Round(Math.Pow(1 + (double)taxaAnual / 100, 1.0 / 252), 8);

            _logger.LogDebug("Dia: {Dia}, Base: {Base}, Taxa: {Taxa}, Fator: {Fator}",
                dia.ToShortDateString(), dia.ToShortDateString(), taxaAnual, fatorDiario);

            fatorAcumulado *= (decimal)fatorDiario;
        }

        fatorAcumulado = MathUtils.TruncarDecimal(fatorAcumulado, 16);
        var valorAtualizado = MathUtils.TruncarDecimal(request.ValorAplicado * fatorAcumulado, 8);

        _logger.LogInformation("Cálculo finalizado: Fator = {Fator}, Valor = {Valor}",
            fatorAcumulado, valorAtualizado);

        return new InvestimentoResponse
        {
            FatorAcumulado = fatorAcumulado,
            ValorAtualizado = valorAtualizado
        };
    }
}