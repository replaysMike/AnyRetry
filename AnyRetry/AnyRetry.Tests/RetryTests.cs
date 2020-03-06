using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AnyRetry.Tests
{
    [TestFixture]
    public class RetryTests
    {
        [Test]
        public void Retry_Static_ShouldRetryOnce()
        {
            var retriesPerformed = 0;
            const int maxRetries = 5;
            // fail on the first try, second try will succeed
            Retry.Do(() =>
            {
                retriesPerformed++;
                if (retriesPerformed == 1)
                    throw new RetryTestException();
            }, TimeSpan.FromMilliseconds(10), maxRetries);

            Assert.AreEqual(2, retriesPerformed);
        }

        [Test]
        public void Retry_Parameterized_Static_ShouldRetryOnce()
        {
            var retriesPerformed = 0;
            const int maxRetries = 5;
            // fail on the first try, second try will succeed
            Retry.Do((retryInterval, retryCount) =>
            {
                Assert.AreEqual(retriesPerformed, retryInterval);
                Assert.AreEqual(maxRetries, retryCount);
                retriesPerformed++;
                if (retriesPerformed == 1)
                    throw new RetryTestException();
            }, TimeSpan.FromMilliseconds(10), maxRetries);

            Assert.AreEqual(2, retriesPerformed);
        }

        [Test]
        public void Retry_Static_ShouldRetryUntilMax()
        {
            var retriesPerformed = 0;
            const int maxRetries = 5;
            // fail on all retries
            Assert.Throws<RetryTimeoutException>(() =>
            {
                Retry.Do(() =>
                {
                    retriesPerformed++;
                    throw new RetryTestException();
                }, TimeSpan.FromMilliseconds(10), maxRetries);
            });
            Assert.AreEqual(5, retriesPerformed);
        }

        [Test]
        public void Retry_Parameterized_Static_ShouldRetryUntilMax()
        {
            var retriesPerformed = 0;
            const int maxRetries = 5;
            // fail on all retries
            Assert.Throws<RetryTimeoutException>(() =>
            {
                Retry.Do((retryInterval, retryCount) =>
                {
                    Assert.AreEqual(retriesPerformed, retryInterval);
                    Assert.AreEqual(maxRetries, retryCount);
                    retriesPerformed++;
                    throw new RetryTestException();
                }, TimeSpan.FromMilliseconds(10), maxRetries);
            });
            Assert.AreEqual(5, retriesPerformed);
        }

        [Test]
        public void Retry_Static_ShouldRetryIntervalElapsed()
        {
            // fail on all retries
            var startTime = DateTime.Now;
            var timeBetweenRetries = TimeSpan.FromMilliseconds(150);
            Assert.Throws<RetryTimeoutException>(() =>
            {
                Retry.Do(() =>
                {
                    throw new RetryTestException();
                }, timeBetweenRetries, 1, RetryPolicy.StaticDelay);
            });
            Assert.Greater(DateTime.Now.Subtract(startTime), timeBetweenRetries);
        }

        [Test]
        public void Retry_Static_ShouldRetryUntilEvaluatesToTrue()
        {
            // fail on all retries
            // this will ignore the maxRetries limit, if the evaluation parameters return false
            var timeBetweenRetries = TimeSpan.FromMilliseconds(150);
            const int maxRetries = 1;
            var i = 0;
            Assert.Throws<RetryTimeoutException>(() =>
            {
                Retry.Do((iteration, max) =>
                {
                    i++;
                    throw new RetryTestException();
                }, timeBetweenRetries,
                    maxRetries,
                    RetryPolicy.StaticDelay,
                    RetryPolicyOptions.None,
                    // don't need an error handler
                    null,
                    // retry forever unless this evaluates to true (advanced usage)
                    () => { return i != 10; }
                );
            });
            Assert.AreEqual(10, i);
        }

        [Test]
        public void Retry_Static_ShouldRetryOnlyOnSpecifiedExceptions()
        {
            var timeBetweenRetries = TimeSpan.FromMilliseconds(150);
            const int maxRetries = 1;
            // we are only listening for RetryTestExceptions
            // if any other type of exception is thrown, it will be rethrown and no retry is performed
            Assert.Throws<InvalidOperationException>(() =>
            {
                Retry.Do((iteration, max) =>
                {
                    throw new InvalidOperationException();
                }, timeBetweenRetries,
                    maxRetries,
                    // indicate we only want to catch RetryTestExceptions
                    typeof(RetryTestException)
                );
            });
        }

        [Test]
        public void Retry_SineEaseIn_WithSteps_ShouldBeCorrect()
        {
            var testLog = new List<long>();
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Assert.Throws<RetryTimeoutException>(() =>
            {
                Retry.Do((retryIteration, retryLimit) =>
                {
                    if (retryIteration > 0)
                    {
                        testLog.Add(sw.ElapsedMilliseconds);
                        Console.WriteLine($"{sw.ElapsedMilliseconds}ms");
                    }
                    sw.Restart();
                    throw new Exception("test");
                }, TimeSpan.FromMilliseconds(100), 12, RetryPolicy.EasedBackoff, new RetryPolicyOptions
                {
                    EasingFunction = AnyRetry.Math.EasingFunction.SineEaseIn,
                    MaxRetryInterval = TimeSpan.FromMilliseconds(1000),
                    MaxRetrySteps = 10
                });
            });
            // make sure the test times are approximately valid
            // note: the very first attempt is not logged, so only 11 results instead of 12
            const int toleranceMs = 50; // this seems to be enough on a typical machine, but may vary by hardware
            Assert.That(testLog.Skip(0).First(), Is.EqualTo(100).Within(toleranceMs));
            Assert.That(testLog.Skip(1).First(), Is.EqualTo(115).Within(toleranceMs));
            Assert.That(testLog.Skip(2).First(), Is.EqualTo(160).Within(toleranceMs));
            Assert.That(testLog.Skip(3).First(), Is.EqualTo(230).Within(toleranceMs));
            Assert.That(testLog.Skip(4).First(), Is.EqualTo(330).Within(toleranceMs));
            Assert.That(testLog.Skip(5).First(), Is.EqualTo(460).Within(toleranceMs));
            Assert.That(testLog.Skip(6).First(), Is.EqualTo(600).Within(toleranceMs));
            Assert.That(testLog.Skip(7).First(), Is.EqualTo(760).Within(toleranceMs));
            Assert.That(testLog.Skip(8).First(), Is.EqualTo(930).Within(toleranceMs));
            Assert.That(testLog.Skip(9).First(), Is.EqualTo(1000).Within(toleranceMs));
            Assert.That(testLog.Skip(10).First(), Is.EqualTo(1000).Within(toleranceMs));
        }

    }
}