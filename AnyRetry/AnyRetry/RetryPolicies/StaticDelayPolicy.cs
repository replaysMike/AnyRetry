using System;

namespace AnyRetry.RetryPolicies
{
    /// <summary>
    /// A static amount of time to delay
    /// </summary>
    public class StaticDelayPolicy : IRetryPolicy
    {
        /// <summary>
        /// New static delay policy
        /// </summary>
        /// <param name="options"></param>
        public StaticDelayPolicy(RetryPolicyOptions options)
        {

        }

        /// <summary>
        /// Apply the policy
        /// </summary>
        /// <param name="retryParameters"></param>
        /// <returns></returns>
        public TimeSpan ApplyPolicy(RetryParameters retryParameters)
        {
            return retryParameters.RetryInterval;
        }
    }
}
