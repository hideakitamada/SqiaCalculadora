using System.Diagnostics.CodeAnalysis;

namespace SqiaCalculadora.Settings;

[ExcludeFromCodeCoverage]
public class CircuitBreakerSettings
{
    public int HandledEventsAllowedBeforeBreaking { get; set; }
    public int DurationOfBreakSeconds { get; set; }
}