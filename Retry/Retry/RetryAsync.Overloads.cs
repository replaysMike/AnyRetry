using System;
using System.Threading.Tasks;

namespace Retry
{
    public static partial class Retry
    {
        #region Do without parameters
        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action)
            => await DoAsync(async (iteration, max) => await action.Invoke(), TimeSpan.FromSeconds(1), int.MaxValue, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, int retryLimit)
            => await DoAsync(async (iteration, max) => await action.Invoke(), TimeSpan.FromSeconds(1), retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan retryInterval, int retryLimit)
            => await DoAsync(async (iteration, max) => await action.Invoke(), retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => await DoAsync(async (iteration, max) => await action.Invoke(), retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, exceptionTypes);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="minRetryInterval"></param>
        /// <param name="maxRetryInterval"></param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan minRetryInterval, TimeSpan maxRetryInterval, int retryLimit)
            => await DoAsync(async (iteration, max) => await action.Invoke(), minRetryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => await DoAsync(async (iteration, max) => await action.Invoke(), retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => await DoAsync(async (iteration, max) => await action.Invoke(), retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null);

        #endregion

        #region Do with parameters

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action)
            => await DoAsync(action, TimeSpan.FromSeconds(1), int.MaxValue, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, int retryLimit)
            => await DoAsync(action, TimeSpan.FromSeconds(1), retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval, int retryLimit)
            => await DoAsync(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => await DoAsync(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, exceptionTypes);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="minRetryInterval"></param>
        /// <param name="maxRetryInterval"></param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan minRetryInterval, TimeSpan maxRetryInterval, int retryLimit)
            => await DoAsync(action, minRetryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => await DoAsync(action, retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => await DoAsync(action, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null);

        #endregion

        #region Do with result

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action)
            => await DoAsync<T>(action, TimeSpan.FromSeconds(1), int.MaxValue, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <returns></returns>
        public static async Task DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, int retryLimit)
            => await DoAsync<T>(action, TimeSpan.FromSeconds(1), retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <returns></returns>
        public static async Task DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit)
            => await DoAsync<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <returns></returns>
        public static async Task DoAsyn<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => await DoAsync<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, exceptionTypes);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="minRetryInterval"></param>
        /// <param name="maxRetryInterval"></param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <returns></returns>
        public static async Task DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan minRetryInterval, TimeSpan maxRetryInterval, int retryLimit)
            => await DoAsync<T>(action, minRetryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <returns></returns>
        public static async Task DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => await DoAsync<T>(action, retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <returns></returns>
        public static async Task DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => await DoAsync<T>(action, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null);

        #endregion
    }
}
