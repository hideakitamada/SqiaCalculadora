namespace SqiaCalculadora.Utils;

public static class MathUtils
{
    public static decimal TruncarDecimal(decimal valor, int casasDecimais)
    {
        var fator = (decimal)Math.Pow(10, casasDecimais);
        return Math.Truncate(valor * fator) / fator;
    }
}
