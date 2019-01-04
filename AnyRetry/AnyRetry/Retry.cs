using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AnyRetry
{
    /// <summary>
    /// A general purpose retry class for retrying code blocks.
    /// </summary>
    public static partial class Retry
    {
        /// <summary>
        /// An synchronous retry action to perform
        /// </summary>
        public delegate void RetryAction();

        /// <summary>
        /// An synchronous retry action to perform
        /// </summary>
        /// <param name="retryIteration">The current retry iteration</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        public delegate void RetryActionWithParameters(int retryIteration, int retryLimit);

        /// <summary>
        /// An synchronous retry action to perform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="retryIteration">The current retry iteration</param>
        /// <param name="retryLimit">The maximum number of times that will be retried</param>
        /// <returns></returns>
        public delegate T RetryActionWithParametersAndResult<out T>(int retryIteration, int retryLimit);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action">The Action to call that will be retried until successful.</param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <param name="mustReturnTrueBeforeFail">Must evaluate to true for retry to fail. Evaluating to false will retry infinitely until true.</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">The options to provide your retry policy</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryActionWithParameters action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Action<Exception, int, int> onFailure, Func<bool> mustReturnTrueBeforeFail, params Type[] exceptionTypes)
        {
            PerformAction<object>((x, y) => { action.Invoke(x, y); return null; }, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, onFailure, mustReturnTrueBeforeFail, exceptionTypes);
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
        /// <param name="mustReturnTrueBeforeFail">Must evaluate to true for retry to fail</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Action<Exception, int, int> onFailure, Func<bool> mustReturnTrueBeforeFail, params Type[] exceptionTypes)
        {
            return PerformAction<T>((x, y) => action.Invoke(x, y), retryInterval, retryLimit, retryPolicy, retryPolicyOptions, onFailure, mustReturnTrueBeforeFail, exceptionTypes);
        }

        private static T PerformAction<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Action<Exception, int, int> onFailure, Func<bool> mustReturnTrueBeforeFail, params Type[] exceptionTypes)
        {
            var exceptions = new List<Exception>();
            var startTime = DateTime.Now;
            var mustContinue = false;

            var retryIteration = 0;
            do
            {
                try
                {
                    return action(retryIteration, retryLimit);
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
                            Thread.Sleep(sleepValue);
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
