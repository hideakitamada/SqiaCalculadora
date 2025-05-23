using System.Diagnostics.CodeAnalysis;

namespace SqiaCalculadora.Settings;

[ExcludeFromCodeCoverage]
public class RetryPolicyOptions
{
    public int RetryCount { get; set; }
    public int InitialDelayMilliseconds { get; set; }
}