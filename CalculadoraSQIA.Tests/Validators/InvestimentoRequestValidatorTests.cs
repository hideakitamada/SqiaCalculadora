using FluentValidation.TestHelper;
using SqiaCalculadora.Models;
using SqiaCalculadora.Validators;

namespace SqiaCalculadora.Tests.Validators;

public class InvestimentoRequestValidatorTests
{
    private readonly InvestimentoRequestValidator _validator = new();

    [Fact]
    public void Deve_Falhar_QuandoValorAplicadoForZero()
    {
        var model = new InvestimentoRequest
        {
            ValorAplicado = 0,
            DataAplicacao = DateTime.Today,
            DataFinal = DateTime.Today.AddDays(1)
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ValorAplicado);
    }

    [Fact]
    public void Deve_Falhar_QuandoDataFinalForAntesDaAplicacao()
    {
        var model = new InvestimentoRequest
        {
            ValorAplicado = 1000,
            DataAplicacao = DateTime.Today,
            DataFinal = DateTime.Today.AddDays(-1)
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DataFinal);
    }

    [Fact]
    public void Deve_Passar_QuandoDadosForemValidos()
    {
        var model = new InvestimentoRequest
        {
            ValorAplicado = 5000,
            DataAplicacao = DateTime.Today,
            DataFinal = DateTime.Today.AddDays(2)
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}