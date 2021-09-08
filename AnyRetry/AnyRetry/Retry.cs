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
        /// The default number of times to retry
        /// </summary>
        public static int DefaultRetryLimit = int.MaxValue;

        /// <summary>
        /// The default retry interval (1 second)
        /// </summary>
        public static TimeSpan DefaultRetryInterval = TimeSpan.FromSeconds(1);

        /// <summary>
        /// A synchronous retry action to perform
        /// </summary>
        public delegate void RetryAction();

        /// <summary>
        /// A synchronous retry action to perform
        /// </summary>
        /// <param name="retryIteration">The current retry iteration</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        public delegate void RetryActionWithParameters(int retryIteration, int retryLimit);

        /// <summary>
        /// A synchronous retry action to perform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public delegate T RetryActionWithResult<out T>();

        /// <summary>
        /// A synchronous retry action to perform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="retryIteration">The current retry iteration</param>
        /// <param name="retryLimit">The maximum number of times that will be retried</param>
        /// <returns></returns>
        public delegate T RetryActionWithParametersAndResult<out T>(int retryIteration, int retryLimit);

        private static T PerformAction<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Action<Exception, int, int> onFailure, Func<bool> mustReturnTrueBeforeFail, params Type[] exceptionTypes)
        {
            var exceptions = new List<Exception>();
            var startTime = DateTime.Now;
            var retryIteration = 0;
            do
            {
                try
                {
                    // invoke the action
                    return action(retryIteration, retryLimit);
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    HandleException(ex, startTime, retryInterval, retryIteration, retryLimit, retryPolicy, retryPolicyOptions, onFailure, exceptionTypes);
                }
                retryIteration++;
            } while (MustContinue(mustReturnTrueBeforeFail) || retryIteration < retryLimit);

            throw new RetryTimeoutException(exceptions, retryLimit);
        }

        private static void HandleException(Exception ex, DateTime startTime, TimeSpan retryInterval, int retryIteration, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Action<Exception, int, int> onFailure, params Type[] exceptionTypes)
        {
            Task.Yield();
            onFailure?.Invoke(ex, retryIteration, retryLimit);
            WaitOrThrow(ex, startTime, retryInterval, retryIteration, retryLimit, retryPolicy, retryPolicyOptions, exceptionTypes);
        }

        private static void WaitOrThrow(Exception ex, DateTime startTime, TimeSpan retryInterval, int retryIteration, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, params Type[] exceptionTypes)
        {
            if (exceptionTypes == null || exceptionTypes.Length == 0 || exceptionTypes.Contains(ex.GetType()))
            {
                if (retryIteration - 1 < retryLimit)
                {
                    var sleepValue = RetryPolicyFactory
                        .Create(retryPolicy, retryPolicyOptions)
                        .ApplyPolicy(RetryParameters.Create(startTime, retryInterval, retryIteration, retryLimit));
                    if (sleepValue.TotalMilliseconds < 0)
                        throw new ArgumentOutOfRangeException(nameof(sleepValue));

                    // use Thread.Sleep for synchronous waits
                    Thread.Sleep(sleepValue);
                }
            }
            else
            {
                throw ex;
            }
        }

        private static bool MustContinue(Func<bool> checkMustContinue)
        {
            // if mustReturnTrueBeforeFail is provided and evaluates to false, retry counts are ignored
            if (checkMustContinue != null)
                return checkMustContinue.Invoke();

            // no must continue callback provided
            return false;
        }
    }
}
