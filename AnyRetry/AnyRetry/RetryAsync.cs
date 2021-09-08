using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AnyRetry
{
    /// <summary>
    /// A general purpose retry class for retrying code blocks
    /// </summary>
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
        /// <returns></returns>
        public delegate Task<T> RetryActionWithResultAsync<T>();

        /// <summary>
        /// An asynchronous retry action to perform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="retryInterval">The current retry iteration</param>
        /// <param name="retryLimit">The maximum number of times that will be retried</param>
        /// <returns></returns>
        public delegate Task<T> RetryActionWithParametersAndResultAsync<T>(int retryInterval, int retryLimit);

        private static async Task<T> PerformActionAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, CancellationToken? cancellationToken, Func<Exception, int, int, Task> onFailure, Func<bool> mustReturnTrueBeforeFail, params Type[] exceptionTypes)
        {
            var exceptions = new List<Exception>();
            var startTime = DateTime.Now;
            var retryIteration = 0;
            do
            {
                try
                {
                    // invoke the action
                    return await InvokeAsync(action, retryIteration, retryLimit, cancellationToken);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    await HandleExceptionAsync(ex, startTime, retryInterval, retryIteration, retryLimit, retryPolicy, retryPolicyOptions, onFailure, exceptionTypes);
                }
                retryIteration++;
            } while (MustContinue(mustReturnTrueBeforeFail) || (retryIteration < retryLimit && (!cancellationToken.HasValue || !cancellationToken.Value.IsCancellationRequested)));

            throw new RetryTimeoutException(exceptions, retryLimit);
        }

        private static async Task<T> PerformActionAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, CancellationToken? cancellationToken, Action<Exception, int, int> onFailure, Func<bool> mustReturnTrueBeforeFail, params Type[] exceptionTypes)
        {
            var exceptions = new List<Exception>();
            var startTime = DateTime.Now;
            var retryIteration = 0;
            do
            {
                try
                {
                    // invoke the action
                    return await InvokeAsync(action, retryIteration, retryLimit, cancellationToken);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    await HandleExceptionAsync(ex, startTime, retryInterval, retryIteration, retryLimit, retryPolicy, retryPolicyOptions, onFailure, exceptionTypes);
                }
                retryIteration++;
            } while (MustContinue(mustReturnTrueBeforeFail) || (retryIteration < retryLimit && (!cancellationToken.HasValue || !cancellationToken.Value.IsCancellationRequested)));

            throw new RetryTimeoutException(exceptions, retryLimit);
        }

        private static async Task<T> InvokeAsync<T>(RetryActionWithParametersAndResultAsync<T> action, int retryIteration, int retryLimit, CancellationToken? cancellationToken)
        {
            if (cancellationToken.HasValue)
            {
                return await Task.Run(() => action.Invoke(retryIteration, retryLimit), cancellationToken.Value);
            }

            return await action.Invoke(retryIteration, retryLimit);
        }

        private static async Task HandleExceptionAsync(Exception ex, DateTime startTime, TimeSpan retryInterval, int retryIteration, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Action<Exception, int, int> onFailure, params Type[] exceptionTypes)
        {
            await Task.Yield();
            onFailure?.Invoke(ex, retryIteration, retryLimit);
            await WaitOrThrowAsync(ex, startTime, retryInterval, retryIteration, retryLimit, retryPolicy, retryPolicyOptions, exceptionTypes);
        }

        private static async Task HandleExceptionAsync(Exception ex, DateTime startTime, TimeSpan retryInterval, int retryIteration, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Func<Exception, int, int, Task> onFailure, params Type[] exceptionTypes)
        {
            await Task.Yield();
            if (onFailure != null)
                await onFailure(ex, retryIteration, retryLimit);
            await WaitOrThrowAsync(ex, startTime, retryInterval, retryIteration, retryLimit, retryPolicy, retryPolicyOptions, exceptionTypes);
        }

        private static async Task WaitOrThrowAsync(Exception ex, DateTime startTime, TimeSpan retryInterval, int retryIteration, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, params Type[] exceptionTypes)
        {
            if (exceptionTypes == null || exceptionTypes.Length == 0 || exceptionTypes.Contains(ex.GetType()))
            {
                if (retryIteration - 1 < retryLimit)
                {
                    var sleepValue = RetryPolicyFactory
                        .Create(retryPolicy, retryPolicyOptions)
                        .ApplyPolicy(RetryParameters.Create(startTime, retryInterval, retryIteration, retryLimit));
                    if (sleepValue.TotalMilliseconds < 0)
                        throw new ArgumentOutOfRangeException();

                    // use Task.Delay for asynchronous waits
                    await Task.Delay(sleepValue);
                }
            }
            else
            {
                throw ex;
            }
        }
    }
}
