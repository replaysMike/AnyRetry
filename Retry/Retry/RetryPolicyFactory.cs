using Retry.RetryPolicies;

namespace Retry
{
    /// <summary>
    /// Factory for Retry Policies
    /// </summary>
    public static class RetryPolicyFactory
    {
        /// <summary>
        /// Create a retry policy
        /// </summary>
        /// <param name="policy"></param>
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
