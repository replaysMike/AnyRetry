using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace AnyRetry.Tests
{
    [TestFixture]
    public class RetryAsyncTests
    {
        [Test]
        public async Task RetryAsyc_Static_ShouldRetryOnceAsync()
        {
            var retriesPerformed = 0;
            const int maxRetries = 5;
            // fail on the first try, second try will succeed
            await Retry.DoAsync(async () =>
            {
                retriesPerformed++;
                if (retriesPerformed == 1)
                    throw new RetryTestException();
            }, TimeSpan.FromMilliseconds(10), maxRetries);

            Assert.AreEqual(2, retriesPerformed);
        }

        [Test]
        public async Task RetryAsyc_Static_ShouldRetryUntilMaxAsync()
        {
            var retriesPerformed = 0;
            const int maxRetries = 5;
            // fail on all retries
            Assert.ThrowsAsync<RetryTimeoutException>(async () =>
                {
                    await Retry.DoAsync(async () =>
                    {
                        retriesPerformed++;
                        throw new RetryTestException();
                    }, TimeSpan.FromMilliseconds(10), maxRetries);
                }
            );
            Assert.AreEqual(5, retriesPerformed);
        }

        [Test]
        public async Task RetryAsyc_Static_ShouldRetryIntervalElapsedAsync()
        {
            // fail on all retries
            var startTime = DateTime.Now;
            var timeBetweenRetries = TimeSpan.FromMilliseconds(150);
            Assert.ThrowsAsync<RetryTimeoutException>(async () =>
            {
                await Retry.DoAsync(async () =>
                {
                    throw new RetryTestException();
                }, timeBetweenRetries, 1, RetryPolicy.StaticDelay);
            });
            Assert.Greater(DateTime.Now.Subtract(startTime), timeBetweenRetries);
        }

        [Test]
        public async Task RetryAsyc_Static_ShouldRetryUntilEvaluatesToTrueAsync()
        {
            // fail on all retries
            // this will ignore the maxRetries limit, if the evaluation parameters return false
            var timeBetweenRetries = TimeSpan.FromMilliseconds(150);
            const int maxRetries = 1;
            var i = 0;
            Assert.ThrowsAsync<RetryTimeoutException>(async () =>
            {
                await Retry.DoAsync(async (iteration, max) =>
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
        public async Task RetryAsyc_Static_ShouldRetryOnlyOnSpecifiedExceptionsAsync()
        {
            var timeBetweenRetries = TimeSpan.FromMilliseconds(150);
            const int maxRetries = 1;
            // we are only listening for RetryTestExceptions
            // if any other type of exception is thrown, it will be rethrown and no retry is performed
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await Retry.DoAsync(async (iteration, max) =>
                {
                    throw new InvalidOperationException();
                }, timeBetweenRetries,
                    maxRetries,
                    // indicate we only want to catch RetryTestExceptions
                    typeof(RetryTestException)
                );
            });
        }
    }
}