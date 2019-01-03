using Retry.Math;
using System;

namespace Retry.RetryPolicies
{
    /// <summary>
    /// Retry policy that will apply an easing operation to backoff the retry delay
    /// </summary>
    public class EasedBackoffPolicy : IRetryPolicy
    {
        private RetryPolicyOptions _options;

        public EasedBackoffPolicy(RetryPolicyOptions options)
        {
            _options = options;
        }

        public TimeSpan ApplyPolicy(RetryParameters retryParameters)
        {
            // todo: what to use for maxDelay?
            return GetRetryTimeoutSeconds(retryParameters.RetryIteration, retryParameters.RetryCount, retryParameters.RetryInterval, retryParameters.RetryInterval, _options.EasingFunction);
        }

        /// <summary>
        /// Get the retry timeout for a specific attempt number
        /// </summary>
        /// <param name="retryIteration">The current retry iteration</param>
        /// <param name="retryCount">The maximum number of retries</param>
        /// <param name="minDelay">The minimum delay to use</param>
        /// <param name="maxDelay">The maximum delay to use</param>
        /// <param name="easingFunction"></param>
        /// <returns></returns>
        private TimeSpan GetRetryTimeoutSeconds(int retryIteration, int retryCount, TimeSpan minDelay, TimeSpan maxDelay, EasingFunction easingFunction)
        {
            if (retryIteration <= 1)
                return minDelay;
            if (retryIteration >= retryCount)
                return maxDelay;

            // use quadratic easing
            // produce a number between 0 and 1.0
            var t = (double)retryIteration / retryCount;
            var easeVal = Easings.Interpolate(t, easingFunction);
            // easeVal is a number between 0.0 and 1.0
            var diff = maxDelay - minDelay;

            return TimeSpan.FromTicks((long)(diff.Ticks * easeVal));
        }
    }
}
