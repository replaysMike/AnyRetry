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
            var options = new RetryPolicyOptions { 
                MaxRetryInterval = maxTimeBetweenRetries,
            };
            var retryPolicy = RetryPolicyFactory.Create(RetryPolicy.ExponentialBackoff, options);

            for (var i = 0; i < maxRetries; i++)
                retryTimes.Add(retryPolicy.ApplyPolicy(RetryParameters.Create(startTime, timeBetweenRetries, i, maxRetries)));

            // there is a 20% randomness to the delay
            var randomDelay = TimeSpan.FromMilliseconds(timeBetweenRetries.Milliseconds * 0.2);
            // first timeout should have nearly no delay
            Assert.LessOrEqual(timeBetweenRetries - randomDelay, retryTimes.First());
            // exponential increase
            Assert.LessOrEqual(TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 2 - (timeBetweenRetries.TotalMilliseconds * 2 * 0.2)), retryTimes.Skip(1).First());
            Assert.LessOrEqual(TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 4 - (timeBetweenRetries.TotalMilliseconds * 4 * 0.2)), retryTimes.Skip(2).First());
            Assert.LessOrEqual(TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 8 - (timeBetweenRetries.TotalMilliseconds * 8 * 0.2)), retryTimes.Skip(3).First());
            Assert.LessOrEqual(TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 16 - (timeBetweenRetries.TotalMilliseconds * 16 * 0.2)), retryTimes.Skip(4).First());
            Assert.LessOrEqual(TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 32 - (timeBetweenRetries.TotalMilliseconds * 32 * 0.2)), retryTimes.Skip(5).First());
            Assert.LessOrEqual(TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 64 - (timeBetweenRetries.TotalMilliseconds * 64 * 0.2)), retryTimes.Skip(6).First());
            Assert.LessOrEqual(TimeSpan.FromMilliseconds(timeBetweenRetries.TotalMilliseconds * 128 - (timeBetweenRetries.TotalMilliseconds * 128 * 0.2)), retryTimes.Skip(7).First());
            Assert.AreEqual(TimeSpan.FromMilliseconds(10000), retryTimes.Skip(8).First());
            Assert.AreEqual(TimeSpan.FromMilliseconds(10000), retryTimes.Skip(9).First());
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

            // elastic ease in
            var toleranceSeconds = 0.001;
            Assert.That(retryTimes.Skip(0).First().TotalSeconds, Is.EqualTo(0.05).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(1).First().TotalSeconds, Is.EqualTo(0.3185313).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(2).First().TotalSeconds, Is.EqualTo(0.4428445).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(3).First().TotalSeconds, Is.EqualTo(0.5).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(4).First().TotalSeconds, Is.EqualTo(0.5).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(5).First().TotalSeconds, Is.EqualTo(0.5).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(6).First().TotalSeconds, Is.EqualTo(0.5).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(7).First().TotalSeconds, Is.EqualTo(0.5).Within(toleranceSeconds));
            // last timeout should be equal to max
            Assert.AreEqual(retryTimes.Skip(9).First(), TimeSpan.FromMilliseconds(500));
        }

        [Test]
        public void RetryPolicy_EasedLinear_WithSteps_ShouldBeCorrect()
        {
            // fail on all retries
            const int maxRetrySteps = 5;
            const int maxRetries = 10;
            var startTime = DateTime.Now;
            var timeBetweenRetries = TimeSpan.FromMilliseconds(500);
            var maxTimeBetweenRetries = TimeSpan.FromMilliseconds(10000);
            var retryTimes = new List<TimeSpan>();
            var options = new RetryPolicyOptions
            {
                EasingFunction = AnyRetry.Math.EasingFunction.Linear,
                MaxRetryInterval = maxTimeBetweenRetries,
                MaxRetrySteps = maxRetrySteps
            };
            var retryPolicy = RetryPolicyFactory.Create(RetryPolicy.EasedBackoff, options);

            for (var i = 0; i < maxRetries; i++)
                retryTimes.Add(retryPolicy.ApplyPolicy(RetryParameters.Create(startTime, timeBetweenRetries, i, maxRetries)));

            // linear increase up to a max of maxRetrySteps then it should level off
            var toleranceSeconds = 0.0;
            Assert.That(retryTimes.Skip(0).First().TotalSeconds, Is.EqualTo(0.5).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(1).First().TotalSeconds, Is.EqualTo(3.0).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(2).First().TotalSeconds, Is.EqualTo(5.5).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(3).First().TotalSeconds, Is.EqualTo(8.0).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(4).First().TotalSeconds, Is.EqualTo(10.0).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(5).First().TotalSeconds, Is.EqualTo(10.0).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(6).First().TotalSeconds, Is.EqualTo(10.0).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(7).First().TotalSeconds, Is.EqualTo(10.0).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(8).First().TotalSeconds, Is.EqualTo(10.0).Within(toleranceSeconds));
            Assert.That(retryTimes.Skip(9).First().TotalSeconds, Is.EqualTo(10.0).Within(toleranceSeconds));
        }

        [Test]
        public void RetryPolicy_SineEaseIn_WithSteps_ShouldBeCorrect()
        {
            // fail on all retries
            const int maxRetrySteps = 5;
            const int maxRetries = 30;
            var startTime = DateTime.Now;
            var timeBetweenRetries = TimeSpan.FromMilliseconds(500);
            var maxTimeBetweenRetries = TimeSpan.FromMilliseconds(2000);
            var retryTimes = new List<TimeSpan>();
            var options = new RetryPolicyOptions
            {
                EasingFunction = AnyRetry.Math.EasingFunction.SineEaseIn,
                MaxRetryInterval = maxTimeBetweenRetries,
                MaxRetrySteps = maxRetrySteps
            };
            var retryPolicy = RetryPolicyFactory.Create(RetryPolicy.EasedBackoff, options);

            for (var i = 0; i < maxRetries; i++)
                retryTimes.Add(retryPolicy.ApplyPolicy(RetryParameters.Create(startTime, timeBetweenRetries, i, maxRetries)));

            // linear increase up to a max of maxRetrySteps then it should level off
            Assert.AreEqual(0.5, System.Math.Round(retryTimes.Skip(0).First().TotalSeconds, 2));
            Assert.AreEqual(0.6522409, retryTimes.Skip(1).First().TotalSeconds);
            Assert.AreEqual(1.0857864, retryTimes.Skip(2).First().TotalSeconds);
            Assert.AreEqual(1.7346331, retryTimes.Skip(3).First().TotalSeconds);
            Assert.AreEqual(2.0, retryTimes.Skip(4).First().TotalSeconds);
            Assert.AreEqual(2.0, retryTimes.Skip(5).First().TotalSeconds);
            Assert.AreEqual(2.0, retryTimes.Skip(6).First().TotalSeconds);
            Assert.AreEqual(2.0, retryTimes.Skip(7).First().TotalSeconds);
            Assert.AreEqual(2.0, retryTimes.Skip(8).First().TotalSeconds);
            Assert.AreEqual(2.0, retryTimes.Skip(10).First().TotalSeconds);
            Assert.AreEqual(2.0, retryTimes.Skip(11).First().TotalSeconds);
            Assert.AreEqual(2.0, retryTimes.Skip(12).First().TotalSeconds);
            Assert.AreEqual(2.0, retryTimes.Skip(13).First().TotalSeconds);
            Assert.AreEqual(2.0, retryTimes.Skip(14).First().TotalSeconds);
            Assert.AreEqual(2.0, retryTimes.Skip(15).First().TotalSeconds);
            Assert.AreEqual(2.0, retryTimes.Skip(16).First().TotalSeconds);
        }
    }
}
