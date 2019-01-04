using AnyRetry.Math;
using System;

namespace AnyRetry.RetryPolicies
{
    /// <summary>
    /// Retry policy that will apply an easing operation to backoff the retry delay
    /// </summary>
    public class EasedBackoffPolicy : IRetryPolicy
    {
        private readonly RetryPolicyOptions _options;

        public EasedBackoffPolicy(RetryPolicyOptions options)
        {
            _options = options ?? new RetryPolicyOptions { EasingFunction = EasingFunction.ElasticEaseIn, MaxRetryInterval = TimeSpan.MinValue };
        }

        public TimeSpan ApplyPolicy(RetryParameters retryParameters)
        {
            var maxInterval = _options.MaxRetryInterval;
            if (maxInterval == TimeSpan.MinValue)
                maxInterval = TimeSpan.FromMilliseconds(retryParameters.RetryInterval.TotalMilliseconds * retryParameters.RetryIteration);
            var minInterval = retryParameters.RetryInterval;
            return GetRetryTimeoutSeconds(retryParameters.RetryIteration, retryParameters.RetryCount, minInterval, maxInterval, _options.EasingFunction);
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
            // produce a number between 0 and 1.0
            var t = (double)retryIteration / (retryCount - 1);
            // easeVal contains number between 0 and 1.0 but with easing applied
            var easeVal = Easings.Interpolate(t, easingFunction);

            if (easeVal == 0)
                return minDelay;

            var result = TimeSpan.FromTicks((long)(maxDelay.Ticks * easeVal));
            return result;
        }
    }
}
