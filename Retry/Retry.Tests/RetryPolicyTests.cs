using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Retry.Tests
{
    [TestFixture]
    public class RetryPolicyTests
    {
        [Test]
        public void Retry_Static_ShouldRetryIntervalElapsed()
        {
            // fail on all retries
            var lastTime = DateTime.Now;
            var timeBetweenRetries = TimeSpan.FromMilliseconds(100);
            var retryTimes = new List<TimeSpan>();
            Assert.Throws<RetryTimeoutException>(() =>
            {
                Retry.Do(() =>
                {
                    retryTimes.Add(DateTime.Now.Subtract(lastTime));
                    lastTime = DateTime.Now;
                    throw new RetryTestException();
                }, timeBetweenRetries, 3, RetryPolicy.ExponentialBackoff);
            });
            Assert.AreEqual(3, retryTimes.Count);

            // introduce a delay as the thread timing isn't millisecond accurate
            var threadPoolDelay = TimeSpan.FromMilliseconds(8);
            // exponentially growing timeouts
            Assert.Less(retryTimes.First(), timeBetweenRetries);
            Assert.Greater(retryTimes.Skip(1).First() + threadPoolDelay, timeBetweenRetries, $"Second retry didn't wait long enough ({(retryTimes.Skip(1).First() + threadPoolDelay).TotalMilliseconds})");
            Assert.Greater(retryTimes.Skip(2).First() + threadPoolDelay, timeBetweenRetries * 2, $"Third retry didn't wait long enough ({(retryTimes.Skip(2).First() + threadPoolDelay).TotalMilliseconds})");
        }
    }
}
