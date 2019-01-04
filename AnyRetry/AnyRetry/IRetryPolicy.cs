using System;

namespace AnyRetry
{
    /// <summary>
    /// Retry policy controls how to delay between retries
    /// </summary>
    public interface IRetryPolicy
    {
        /// <summary>
        /// Apply a retry policy
        /// </summary>
        /// <param name="retryParameters">The parameters of a retry operation</param>
        /// <returns>The amount of time to delay</returns>
        TimeSpan ApplyPolicy(RetryParameters retryParameters);
    }
}
