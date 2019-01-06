using AnyRetry.Math;
using NUnit.Framework;
using System;

namespace AnyRetry.Tests
{
    [TestFixture]
    public class RetryPolicyOptionsTests
    {
        [Test]
        public void Should_RetryPolicyOptions_EqualOther()
        {
            var policy1 = new RetryPolicyOptions { EasingFunction = EasingFunction.BackEaseIn, MaxRetryInterval = TimeSpan.MaxValue };
            var policy2 = new RetryPolicyOptions { EasingFunction = EasingFunction.BackEaseIn, MaxRetryInterval = TimeSpan.MaxValue };

            Assert.AreEqual(policy1, policy2);
            Assert.IsTrue(policy1.Equals(policy2));
        }

        [Test]
        public void Should_RetryPolicyOptions_NotEqualOther()
        {
            var policy1 = new RetryPolicyOptions { EasingFunction = EasingFunction.BackEaseIn, MaxRetryInterval = TimeSpan.MaxValue };
            var policy2 = new RetryPolicyOptions { EasingFunction = EasingFunction.BackEaseInOut, MaxRetryInterval = TimeSpan.MaxValue };

            Assert.AreNotEqual(policy1, policy2);
            Assert.IsFalse(policy1.Equals(policy2));
        }

        [Test]
        public void Should_RetryPolicyOptions_GetHashcodeEqual()
        {
            var policy1 = new RetryPolicyOptions { EasingFunction = EasingFunction.BackEaseIn, MaxRetryInterval = TimeSpan.MaxValue };
            var policy2 = new RetryPolicyOptions { EasingFunction = EasingFunction.BackEaseIn, MaxRetryInterval = TimeSpan.MaxValue };

            Assert.AreEqual(policy1.GetHashCode(), policy2.GetHashCode());
        }

        [Test]
        public void Should_RetryPolicyOptions_GetHashcodeNotEqual()
        {
            var policy1 = new RetryPolicyOptions { EasingFunction = EasingFunction.BackEaseIn, MaxRetryInterval = TimeSpan.MaxValue };
            var policy2 = new RetryPolicyOptions { EasingFunction = EasingFunction.BackEaseInOut, MaxRetryInterval = TimeSpan.MaxValue };

            Assert.AreNotEqual(policy1.GetHashCode(), policy2.GetHashCode());
        }
    }
}
