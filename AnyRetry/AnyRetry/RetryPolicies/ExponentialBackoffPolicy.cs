using System;
using System.Threading;

namespace AnyRetry.RetryPolicies
{
    /// <summary>
    /// Retry policy that will apply an exponential operation to backoff the retry delay
    /// </summary>
    public class ExponentialBackoffPolicy : IRetryPolicy
    {
        /// <summary>
        /// e.g. 20% spread. e.g. a 2000ms delay could actually delay between 1600 and 2400 ms, randomly
        /// </summary>
        private const double _defaultMaxRelativeRandomSpreadPercent = 20.0;
        private static readonly double _minDelayMs = 1.0;
        private static readonly double _maxDelayMsForExponentialBackoff = 10 * 1000.0;

        private static int _seed = Environment.TickCount;
        private static readonly ThreadLocal<Random> _random;

        private readonly RetryPolicyOptions _options;

        static ExponentialBackoffPolicy()
        {
            // thread safe random numbers
            _random = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));
        }

        /// <summary>
        /// Retry policy options
        /// </summary>
        /// <param name="options"></param>
        public ExponentialBackoffPolicy(RetryPolicyOptions options)
        {
            _options = options;
            if(_options == null || _options.Equals(RetryPolicyOptions.None))
                _options = new RetryPolicyOptions { MaxRetryInterval = TimeSpan.FromMilliseconds(_maxDelayMsForExponentialBackoff) };
        }

        /// <summary>
        /// Apply the retry policy
        /// </summary>
        /// <param name="retryParameters"></param>
        /// <returns></returns>
        public TimeSpan ApplyPolicy(RetryParameters retryParameters)
        {
            return SleepIntervalMs(retryParameters.RetryInterval, retryParameters.RetryIteration, _defaultMaxRelativeRandomSpreadPercent);
        }

        /// <summary>
        /// Use a random spread, so that N racing threads will each get a randomly assigned delay added/subtracted to their wait time
        /// to make concurrency based exceptions less likely to reoccur
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="iteration"></param>
        /// <param name="maxRelativeRandomSpreadPercent"></param>
        /// <returns></returns>
        private TimeSpan SleepIntervalMs(TimeSpan interval, int iteration, double maxRelativeRandomSpreadPercent)
        {
            var waitTimeMs = System.Math.Pow(2.0, iteration) * interval.TotalMilliseconds;
            var maxRelativeRandomSpreadMs = waitTimeMs * (maxRelativeRandomSpreadPercent / 100.0);
            var sleepIntervalMs = (int)System.Math.Max(waitTimeMs + (_random.Value.NextDouble() - 0.5) * maxRelativeRandomSpreadMs, _minDelayMs);
            if (iteration > 0)
            {
                // for exponential backoff, to prevent very long delays not suitable for a web application, we cap
                // the delay per retry to 10 seconds OR the specified interval, whichever is higher
                sleepIntervalMs = System.Math.Min(sleepIntervalMs, (int)System.Math.Max(_options.MaxRetryInterval.TotalMilliseconds, interval.TotalMilliseconds));
            }
            var result = TimeSpan.FromMilliseconds(sleepIntervalMs);
            return result;
        }
    }
}
