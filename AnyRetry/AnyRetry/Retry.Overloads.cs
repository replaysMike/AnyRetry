using System;

namespace AnyRetry
{
    /// <summary>
    /// A general purpose retry class for retrying code blocks
    /// </summary>
    public static partial class Retry
    {
        #region Do without parameters

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryAction action)
            => Do((iteration, max) => action.Invoke(), DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryAction action, TimeSpan retryInterval)
            => Do((iteration, max) => action.Invoke(), retryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryAction action, int retryLimit)
            => Do((iteration, max) => action.Invoke(), DefaultRetryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryAction action, TimeSpan retryInterval, int retryLimit)
            => Do((iteration, max) => action.Invoke(), retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryAction action, TimeSpan retryInterval, int retryLimit, Action<Exception, int, int> onFailure)
            => Do((iteration, max) => action.Invoke(), retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, onFailure, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryAction action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => Do((iteration, max) => action.Invoke(), retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, exceptionTypes);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryAction action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => Do((iteration, max) => action.Invoke(), retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryAction action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => Do((iteration, max) => action.Invoke(), retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null);

        #endregion

        #region Do with parameters

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryActionWithParameters action)
            => Do(action, TimeSpan.FromSeconds(1), DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryActionWithParameters action, TimeSpan retryInterval)
            => Do(action, retryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryActionWithParameters action, int retryLimit)
            => Do(action, DefaultRetryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryActionWithParameters action, TimeSpan retryInterval, int retryLimit)
            => Do(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryActionWithParameters action, Action<Exception, int, int> onFailure)
            => Do(action, DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, onFailure, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryActionWithParameters action, TimeSpan retryInterval, int retryLimit, Action<Exception, int, int> onFailure)
            => Do(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, onFailure, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryActionWithParameters action, params Type[] exceptionTypes)
            => Do(action, DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, exceptionTypes);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryActionWithParameters action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => Do(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, exceptionTypes);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryActionWithParameters action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => Do(action, retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryActionWithParameters action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => Do(action, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null);
        #endregion

        #region Do with result and without parameters

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithResult<T> action)
            => Do(action, DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithResult<T> action, TimeSpan retryInterval)
            => Do(action, retryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithResult<T> action, int retryLimit)
            => Do(action, DefaultRetryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithResult<T> action, TimeSpan retryInterval, int retryLimit)
            => Do(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static T Do<T>(RetryActionWithResult<T> action, TimeSpan retryInterval, int retryLimit, Action<Exception, int, int> onFailure)
            => Do(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, onFailure, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithResult<T> action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => Do(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, exceptionTypes);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithResult<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => Do(action, retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithResult<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => Do(action, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null);

        #endregion

        #region Do with parameters and result

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action)
            => Do(action, DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval)
            => Do(action, retryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, int retryLimit)
            => Do(action, DefaultRetryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval, int retryLimit)
            => Do(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval, int retryLimit, Action<Exception, int, int> onFailure)
            => Do(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, onFailure, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => Do(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, exceptionTypes);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => Do(action, retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => Do(action, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null);

        #endregion

        /// <summary>
        /// Perform a synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action">The Action to call that will be retried until successful.</param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <param name="mustReturnTrueBeforeFail">Must evaluate to true for retry to fail. Evaluating to false will retry infinitely until true.</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">The options to provide your retry policy</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(RetryActionWithParameters action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Action<Exception, int, int> onFailure, Func<bool> mustReturnTrueBeforeFail, params Type[] exceptionTypes) 
            => PerformAction<object>((x, y) => { action.Invoke(x, y); return null; }, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, onFailure, mustReturnTrueBeforeFail, exceptionTypes);

        /// <summary>
        /// Perform a synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The Action to call that will be retried until successful.</param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">The options to provide your retry policy</param>
        /// <param name="mustReturnTrueBeforeFail">Must evaluate to true for retry to fail</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithResult<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Action<Exception, int, int> onFailure, Func<bool> mustReturnTrueBeforeFail, params Type[] exceptionTypes) 
            => PerformAction((x, y) => action.Invoke(), retryInterval, retryLimit, retryPolicy, retryPolicyOptions, onFailure, mustReturnTrueBeforeFail, exceptionTypes);

        /// <summary>
        /// Perform a synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The Action to call that will be retried until successful.</param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">The options to provide your retry policy</param>
        /// <param name="mustReturnTrueBeforeFail">Must evaluate to true for retry to fail</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Action<Exception, int, int> onFailure, Func<bool> mustReturnTrueBeforeFail, params Type[] exceptionTypes) 
            => PerformAction((x, y) => action.Invoke(x, y), retryInterval, retryLimit, retryPolicy, retryPolicyOptions, onFailure, mustReturnTrueBeforeFail, exceptionTypes);
    }
}
