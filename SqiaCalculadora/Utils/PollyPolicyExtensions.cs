using Polly;
using SqiaCalculadora.Settings;
using System.Diagnostics.CodeAnalysis;

namespace SqiaCalculadora.Utils;

[ExcludeFromCodeCoverage]
public static class PollyPolicyExtensions
{
    public static IAsyncPolicy<HttpResponseMessage> GetResiliencePolicy(PollySettings settings)
    {
        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(settings.TimeoutSeconds));

        var circuitBreakerPolicy = Policy<HttpResponseMessage>
        .HandleResult(response =>
            (int)response.StatusCode >= 500 || // Server errors
            response.StatusCode == System.Net.HttpStatusCode.RequestTimeout
        )
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: settings.CircuitBreaker.HandledEventsAllowedBeforeBreaking,
            durationOfBreak: TimeSpan.FromSeconds(settings.CircuitBreaker.DurationOfBreakSeconds)
        );

        return Policy.WrapAsync(timeoutPolicy, circuitBreakerPolicy);
    }
}