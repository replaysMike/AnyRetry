using AnyRetry.Math;
using System;

namespace AnyRetry
{
    /// <summary>
    /// Options for a retry policy
    /// </summary>
    public class RetryPolicyOptions : IEquatable<RetryPolicyOptions>
    {
        /// <summary>
        /// The easing function to use for an EasedBackoffPolicy
        /// </summary>
        public EasingFunction EasingFunction { get; set; } = EasingFunction.ExponentialEaseOut;

        /// <summary>
        /// The maximum retry interval for easing policy
        /// </summary>
        public TimeSpan MaxRetryInterval { get; set; }

        /// <summary>
        /// Create a empty policy options
        /// </summary>
        public static RetryPolicyOptions None => new RetryPolicyOptions();

        public override int GetHashCode()
        {
            return MaxRetryInterval.GetHashCode() ^ EasingFunction.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(RetryPolicyOptions))
                return false;
            var objTyped = (RetryPolicyOptions)obj;
            return Equals(objTyped);
        }

        public bool Equals(RetryPolicyOptions other)
        {
            if (other == null)
                return false;
            return MaxRetryInterval == other.MaxRetryInterval
                && EasingFunction == other.EasingFunction;
        }
    }
}
