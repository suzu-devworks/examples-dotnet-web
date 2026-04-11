namespace Examples.Web.Infrastructure.GrpcClient.Internals;

/// <summary>
/// Extension methods for TimeProvider to calculate gRPC call deadlines based on configured timeouts.
/// </summary>
internal static class TimeProviderExtensions
{
    /// <summary>
    /// Calculates the deadline for a gRPC call based on the current time and the specified timeout.
    /// </summary>
    /// <param name="timeProvider">The time provider to use for getting the current time.</param>
    /// <param name="timeout">The timeout for the gRPC call.</param>
    /// <returns>The calculated deadline as a <see cref="DateTime"/> if a timeout is specified; otherwise, <c>null</c>.</returns>
    public static DateTime? GetDeadline(this TimeProvider timeProvider, TimeSpan? timeout)
    {
        var now = timeProvider.GetUtcNow();
        if (timeout is null || timeout <= TimeSpan.Zero)
        {
            return null; // No deadline
        }

        return now.Add(timeout.Value).UtcDateTime;
    }
}
