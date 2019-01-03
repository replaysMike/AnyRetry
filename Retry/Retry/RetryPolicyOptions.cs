using Retry.Math;

namespace Retry
{
    /// <summary>
    /// Options for a retry policy
    /// </summary>
    public class RetryPolicyOptions
    {
        /// <summary>
        /// The easing function to use for an EasedBackoffPolicy
        /// </summary>
        public EasingFunction EasingFunction { get; set; } = EasingFunction.ExponentialEaseIn;

        public RetryPolicyOptions()
        {

        }
    }
}
