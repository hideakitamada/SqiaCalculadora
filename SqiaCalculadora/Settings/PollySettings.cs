using System.Diagnostics.CodeAnalysis;

namespace SqiaCalculadora.Settings;

[ExcludeFromCodeCoverage]
public class PollySettings
{
    public int TimeoutSeconds { get; set; }
    public CircuitBreakerSettings CircuitBreaker { get; set; } = new();
}