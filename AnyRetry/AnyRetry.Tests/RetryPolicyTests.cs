using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnyRetry.Tests
{
    [TestFixture]
    public class RetryPolicyTests
    {
        [Test]
        public void Retry_Static_ShouldRetryIntervalElapsed()
        {
            // fail on all retries
            var lastTime = DateTime.Now;
            var timeBetweenRetries = TimeSpan.FromMilliseconds(50);
            var retryTimes = new List<TimeSpan>();
            Assert.Throws<RetryTimeoutException>(() =>
            {
                Retry.Do((interval, count) =>
                {
                    retryTimes.Add(DateTime.Now.Subtract(lastTime));
                    lastTime = DateTime.Now;
                    throw new RetryTestException();
                }, timeBetweenRetries, 3, RetryPolicy.StaticDelay);
            });
            Assert.AreEqual(3, retryTimes.Count);

            // exponentially growing timeouts
            Assert.Less(retryTimes.First(), timeBetweenRetries);
            Assert.Greater(retryTimes.Skip(1).First(), timeBetweenRetries, $"Second retry didn't wait long enough ({(retryTimes.Skip(1).First()).TotalMilliseconds})");
            Assert.Greater(retryTimes.Skip(2).First(), timeBetweenRetries, $"Third retry didn't wait long enough ({(retryTimes.Skip(2).First()).TotalMilliseconds})");
        }

        [Test]
        public void Retry_Exponential_ShouldRetryIntervalElapsed()
        {
            // fail on all retries
            var lastTime = DateTime.Now;
            var timeBetweenRetries = TimeSpan.FromMilliseconds(50);
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

            // there is a 20% randomness to the delay
            // exponentially growing timeouts
            Assert.Less(retryTimes.First(), timeBetweenRetries);
            Assert.Greater(retryTimes.Skip(1).First(), TimeSpan.FromMilliseconds(System.Math.Floor(timeBetweenRetries.TotalMilliseconds - (timeBetweenRetries.TotalMilliseconds * 0.2))));
            Assert.Greater(retryTimes.Skip(2).First(), TimeSpan.FromMilliseconds(System.Math.Floor((timeBetweenRetries.TotalMilliseconds * 2) - (timeBetweenRetries.TotalMilliseconds * 2) * 0.2)));
        }

        [Test]
        public void RetryPolicy_Exponential_ShouldBeCorrect()
        {
            // fail on all retries
            var startTime = DateTime.Now;
            var timeBetweenRetries = TimeSpan.FromMilliseconds(50);
            var maxTimeBetweenRetries = TimeSpan.FromMilliseconds(10000);
            var maxRetries = 10;
            var retryTimes = new List<TimeSpan>();
            var retryPolicy = RetryPolicyFactory.Create(RetryPolicy.ExponentialBackoff, new RetryPolicyOptions { MaxRetryInterval = maxTimeBetweenRetries });

            for (var i = 0; i < maxRetries; i++)
                retryTimes.Add(retryPolicy.ApplyPolicy(RetryParameters.Create(startTime, timeBetweenRetries, i, maxRetries)));

            // there is a 20% randomness to the delay
            var randomDelay = TimeSpan.FromMilliseconds(timeBetweenRetries.Milliseconds * 0.2);
            // first timeout should have nearly no delay
            Assert.Greater(retryTimes.First(), timeBetweenRetries - randomDelay);
            // exponential increase
            Assert.Greater(retryTimes.Skip(1).First(), TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 2 - (timeBetweenRetries.TotalMilliseconds * 2 * 0.2)));
            Assert.Greater(retryTimes.Skip(2).First(), TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 4 - (timeBetweenRetries.TotalMilliseconds * 4 * 0.2)));
            Assert.Greater(retryTimes.Skip(3).First(), TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 8 - (timeBetweenRetries.TotalMilliseconds * 8 * 0.2)));
            Assert.Greater(retryTimes.Skip(4).First(), TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 16 - (timeBetweenRetries.TotalMilliseconds * 16 * 0.2)));
            Assert.Greater(retryTimes.Skip(5).First(), TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 32 - (timeBetweenRetries.TotalMilliseconds * 32 * 0.2)));
            Assert.Greater(retryTimes.Skip(6).First(), TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 64 - (timeBetweenRetries.TotalMilliseconds * 64 * 0.2)));
            Assert.Greater(retryTimes.Skip(7).First(), TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 128 - (timeBetweenRetries.TotalMilliseconds * 128 * 0.2)));
            Assert.AreEqual(retryTimes.Skip(8).First(), TimeSpan.FromMilliseconds(10000));
            Assert.AreEqual(retryTimes.Skip(9).First(), TimeSpan.FromMilliseconds(10000));
        }

        [Test]
        public void RetryPolicy_ExponentialEaseIn_ShouldBeCorrect()
        {
            // fail on all retries
            var startTime = DateTime.Now;
            var timeBetweenRetries = TimeSpan.FromMilliseconds(50);
            var maxTimeBetweenRetries = TimeSpan.FromMilliseconds(500);
            var maxRetries = 10;
            var retryTimes = new List<TimeSpan>();
            var retryPolicy = RetryPolicyFactory.Create(RetryPolicy.EasedBackoff, new RetryPolicyOptions { EasingFunction = AnyRetry.Math.EasingFunction.ExponentialEaseOut, MaxRetryInterval = maxTimeBetweenRetries });

            for(var i = 0; i < maxRetries; i++)
                retryTimes.Add(retryPolicy.ApplyPolicy(RetryParameters.Create(startTime, timeBetweenRetries, i, maxRetries)));

            // first timeout should have nearly no delay
            Assert.AreEqual(retryTimes.First(), timeBetweenRetries);
            // elastic ease in
            Assert.Greater(retryTimes.Skip(1).First(), TimeSpan.FromMilliseconds(268));
            Assert.Greater(retryTimes.Skip(2).First(), TimeSpan.FromMilliseconds(392));
            Assert.Greater(retryTimes.Skip(3).First(), TimeSpan.FromMilliseconds(450));
            Assert.Greater(retryTimes.Skip(4).First(), TimeSpan.FromMilliseconds(477));
            Assert.Greater(retryTimes.Skip(5).First(), TimeSpan.FromMilliseconds(489));
            Assert.Greater(retryTimes.Skip(6).First(), TimeSpan.FromMilliseconds(495));
            Assert.Greater(retryTimes.Skip(7).First(), TimeSpan.FromMilliseconds(497));
            Assert.Greater(retryTimes.Skip(8).First(), TimeSpan.FromMilliseconds(498));
            // last timeout should be equal to max
            Assert.AreEqual(retryTimes.Skip(9).First(), TimeSpan.FromMilliseconds(500));
        }
    }
}
