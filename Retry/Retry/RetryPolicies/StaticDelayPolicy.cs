using System;

namespace Retry.RetryPolicies
{
    /// <summary>
    /// A static amount of time to delay
    /// </summary>
    public class StaticDelayPolicy : IRetryPolicy
    {
        public StaticDelayPolicy(RetryPolicyOptions options)
        {

        }

        public TimeSpan ApplyPolicy(RetryParameters retryParameters)
        {
            return retryParameters.RetryInterval;
        }
    }
}
