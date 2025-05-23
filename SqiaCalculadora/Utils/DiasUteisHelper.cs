namespace SqiaCalculadora.Utils;

public static class DiasUteisHelper
{
    public static IEnumerable<DateTime> DiasUteisEntre(DateTime inicio, DateTime fim)
    {
        for (var date = inicio; date < fim; date = date.AddDays(1))
        {
            if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                yield return date;
        }
    }
}
