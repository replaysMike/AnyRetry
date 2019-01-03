using NUnit.Framework;
using System;

namespace Retry.Tests
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
    }
}