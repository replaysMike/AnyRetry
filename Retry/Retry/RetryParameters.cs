using System;

namespace Retry
{
    /// <summary>
    /// The parameters of a retry operation
    /// </summary>
    public class RetryParameters
    {
        /// <summary>
        /// The start time of the original operation
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        /// The interval between retry operations
        /// </summary>
        public TimeSpan RetryInterval { get; }

        /// <summary>
        /// The current iteration of the retry operation
        /// </summary>
        public int RetryIteration { get; }

        /// <summary>
        /// The maximum number of retries to be performed
        /// </summary>
        public int RetryCount { get; }

        /// <summary>
        /// The parameters of a retry operation
        /// </summary>
        /// <param name="startTime">The start time of the original operation</param>
        /// <param name="retryInterval">The interval between retry operations</param>
        /// <param name="retryIteration">The current iteration of the retry operation</param>
        /// <param name="retryCount">The maximum number of retries to be performed</param>
        public RetryParameters(DateTime startTime, TimeSpan retryInterval, int retryIteration, int retryCount)
        {
            StartTime = startTime;
            RetryInterval = retryInterval;
            RetryIteration = retryIteration;
            RetryCount = retryCount;
        }

        /// <summary>
        /// Create retry parameters
        /// </summary>
        /// <param name="startTime">The start time of the original operation</param>
        /// <param name="retryInterval">The interval between retry operations</param>
        /// <param name="retryIteration">The current iteration of the retry operation</param>
        /// <param name="retryCount">The maximum number of retries to be performed</param>
        public static RetryParameters Create(DateTime startTime, TimeSpan retryInterval, int retryIteration, int retryCount)
        {
            return new RetryParameters(startTime, retryInterval, retryIteration, retryCount);
        }
    }
}
