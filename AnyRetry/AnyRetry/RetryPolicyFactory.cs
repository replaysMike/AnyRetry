using AnyRetry.RetryPolicies;

namespace AnyRetry
{
    /// <summary>
    /// Factory for Retry Policies
    /// </summary>
    public static class RetryPolicyFactory
    {
        /// <summary>
        /// Create a retry policy
        /// </summary>
        /// <param name="policy">Retry policy to use</param>
        /// <param name="options">Retry policy options</param>
        /// <returns></returns>
        public static IRetryPolicy Create(RetryPolicy policy, RetryPolicyOptions options)
        {
            switch (policy)
            {
                case RetryPolicy.ExponentialBackoff:
                    return new ExponentialBackoffPolicy(options);
                case RetryPolicy.EasedBackoff:
                    return new EasedBackoffPolicy(options);
                default:
                    return new StaticDelayPolicy(options);
            }
        }
    }
}
