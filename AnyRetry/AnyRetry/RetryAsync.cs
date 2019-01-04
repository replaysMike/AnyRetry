using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnyRetry
{
    public static partial class Retry
    {
        /// <summary>
        /// An asynchronous retry action to perform
        /// </summary>
        /// <returns></returns>
        public delegate Task RetryActionAsync();

        /// <summary>
        /// An asynchronous retry action to perform
        /// </summary>
        /// <param name="retryIteration">The current retry iteration</param>
        /// <param name="retryLimit">The maximum number of times that will be retried</param>
        /// <returns></returns>
        public delegate Task RetryActionWithParametersAsync(int retryIteration, int retryLimit);

        /// <summary>
        /// An asynchronous retry action to perform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="retryInterval">The current retry iteration</param>
        /// <param name="retryLimit">The maximum number of times that will be retried</param>
        /// <returns></returns>
        public delegate Task<T> RetryActionWithParametersAndResultAsync<T>(int retryInterval, int retryLimit);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action">The Action to call that will be retried until successful.</param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <param name="mustReturnTrueBeforeFail">Must evaluate to true for retry to fail</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Action<Exception, int, int> onFailure, Func<bool> mustReturnTrueBeforeFail, params Type[] exceptionTypes)
        {
            await PerformActionAsync<object>(async (x, y) => { await action.Invoke(x, y); return Task.FromResult<object>(default(object)); }, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, onFailure, mustReturnTrueBeforeFail, exceptionTypes);
        }

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The Action to call that will be retried until successful.</param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <param name="mustReturnTrueBeforeFail">Must evaluate to true for retry to fail</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Action<Exception, int, int> onFailure, Func<bool> mustReturnTrueBeforeFail, params Type[] exceptionTypes)
        {
            return await PerformActionAsync<T>(async (x, y) => await action.Invoke(x, y), retryInterval, retryLimit, retryPolicy, retryPolicyOptions, onFailure, mustReturnTrueBeforeFail, exceptionTypes);
        }

        private static async Task<T> PerformActionAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Action<Exception, int, int> onFailure, Func<bool> mustReturnTrueBeforeFail, params Type[] exceptionTypes)
        {
            var exceptions = new List<Exception>();
            var startTime = DateTime.Now;
            var mustContinue = false;

            var retryIteration = 0;
            do
            {
                try
                {
                    var result = await action.Invoke(retryIteration, retryLimit);
                    return result;
                }
                catch (Exception ex)
                {
                    onFailure?.Invoke(ex, retryIteration, retryLimit);
                    exceptions.Add(ex);
                    if (exceptionTypes == null || exceptionTypes.Length == 0 || exceptionTypes.Contains<Type>(ex.GetType()))
                    {
                        if (retryIteration - 1 < retryLimit)
                        {
                            var sleepValue = RetryPolicyFactory.Create(retryPolicy, retryPolicyOptions).ApplyPolicy(RetryParameters.Create(startTime, retryInterval, retryIteration, retryLimit));
                            if (sleepValue.TotalMilliseconds < 0)
                                throw new ArgumentOutOfRangeException();
                            await Task.Delay(sleepValue);
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
                retryIteration++;
                // if mustReturnTrueBeforeFail is provided and evaluates to false, retry counts are ignored
                if (mustReturnTrueBeforeFail != null)
                    mustContinue = mustReturnTrueBeforeFail.Invoke();
            } while (mustContinue || retryIteration < retryLimit);

            throw new RetryTimeoutException(exceptions, retryLimit);
        }
    }
}
