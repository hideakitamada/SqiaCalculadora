using SqiaCalculadora.Models;
using FluentValidation;

namespace SqiaCalculadora.Validators;

public class InvestimentoRequestValidator : AbstractValidator<InvestimentoRequest>
{
    public InvestimentoRequestValidator()
    {
        RuleFor(x => x.ValorAplicado)
            .GreaterThan(0).WithMessage("O valor aplicado deve ser maior que zero.");

        RuleFor(x => x.DataFinal)
            .GreaterThanOrEqualTo(x => x.DataAplicacao).WithMessage("A data final deve ser a mesma ou após a data de aplicação.");
    }
}