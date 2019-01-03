namespace Retry
{
    /// <summary>
    /// The retry policy to use
    /// </summary>
    public enum RetryPolicy
    {
        /// <summary>
        /// A static delay
        /// </summary>
        StaticDelay,
        /// <summary>
        /// Exponential backoff
        /// </summary>
        ExponentialBackoff,
        /// <summary>
        /// Eased backoff
        /// </summary>
        EasedBackoff,
    }
}
