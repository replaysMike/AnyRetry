using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyRetry.Tests
{
    [TestFixture]
    public class RetryAsyncTests
    {
        [Test]
        public async Task RetryAsync_Static_ShouldRetryOnceAsync()
        {
            //var retriesPerformed = 0;
            //const int maxRetries = 5;
            //// fail on the first try, second try will succeed
            //await Retry.DoAsync(async () =>
            //{
            //    await Task.Delay(10);
            //    retriesPerformed++;
            //    if (retriesPerformed == 1)
            //        throw new RetryTestException();
            //}, TimeSpan.FromMilliseconds(10), maxRetries);

            //Assert.AreEqual(2, retriesPerformed);
        }

        [Test]
        public void RetryAsync_Static_ShouldRetryUntilMax()
        {
            var retriesPerformed = 0;
            const int maxRetries = 5;
            // fail on all retries
            Assert.ThrowsAsync<RetryTimeoutException>(async () =>
                {
                    await Retry.DoAsync(async () =>
                    {
                        await Task.Delay(10);
                        retriesPerformed++;
                        throw new RetryTestException();
                    }, TimeSpan.FromMilliseconds(10), maxRetries);
                }
            );
            Assert.AreEqual(5, retriesPerformed);
        }

        [Test]
        public void RetryAsync_Static_ShouldRetryIntervalElapsed()
        {
            // fail on all retries
            var startTime = DateTime.Now;
            var timeBetweenRetries = TimeSpan.FromMilliseconds(150);
            Assert.ThrowsAsync<RetryTimeoutException>(async () =>
            {
                await Retry.DoAsync(async () =>
                {
                    await Task.Delay(10);
                    throw new RetryTestException();
                }, timeBetweenRetries, 1, RetryPolicy.StaticDelay);
            });
            Assert.Greater(DateTime.Now.Subtract(startTime), timeBetweenRetries);
        }

        [Test]
        public void RetryAsync_Static_ShouldRetryUntilEvaluatesToTrue()
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
                    await Task.Delay(10);
                    i++;
                    throw new RetryTestException();
                }, timeBetweenRetries,
                    maxRetries,
                    RetryPolicy.StaticDelay,
                    RetryPolicyOptions.None,
                    // don't need cancellation token
                    null,
                    // don't need an error handler
                    null,
                    // retry forever unless this evaluates to true (advanced usage)
                    () => { return i != 10; }
                );
            });
            Assert.AreEqual(10, i);
        }

        [Test]
        public void RetryAsync_Static_ShouldRetryOnlyOnSpecifiedExceptions()
        {
            var timeBetweenRetries = TimeSpan.FromMilliseconds(150);
            const int maxRetries = 1;
            // we are only listening for RetryTestExceptions
            // if any other type of exception is thrown, it will be rethrown and no retry is performed
            Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await Retry.DoAsync(async (iteration, max) =>
                {
                    await Task.Delay(10);
                    throw new InvalidOperationException();
                }, timeBetweenRetries,
                    maxRetries,
                    // indicate we only want to catch RetryTestExceptions
                    typeof(RetryTestException)
                );
            });
        }

        [Test]
        public void RetryAsync_CancellationToken_ShouldCancel()
        {
            var retriesPerformed = 0;
            const int expectedRetries = 2;
            const int maxRetries = 5;
            var cancellationToken = new CancellationTokenSource();
            // fail on all retries, but cancel on the second failure
            Assert.ThrowsAsync<RetryTimeoutException>(async () =>
            {
                await Retry.DoAsync(async () =>
                {
                    await Task.Delay(10);
                    retriesPerformed++;
                    throw new RetryTestException();
                }, TimeSpan.FromMilliseconds(10), maxRetries, cancellationToken.Token, (ex, iteration, max) =>
                {
                    if (iteration == expectedRetries - 1)
                    {
                        cancellationToken.Cancel();
                    }
                });
            });

            // ensure we only retried twice and not the full maxRetries
            Assert.AreEqual(expectedRetries, retriesPerformed);
        }
    }
}