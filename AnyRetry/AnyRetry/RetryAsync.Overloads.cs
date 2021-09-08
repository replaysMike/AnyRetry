using System;
using System.Threading;
using System.Threading.Tasks;

namespace AnyRetry
{
    /// <summary>
    /// A general purpose retry class for retrying code blocks
    /// </summary>
    public static partial class Retry
    {
        #region Do without parameters
        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action)
            => await DoAsync(async (iteration, max) => await action.Invoke(), DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, CancellationToken cancellationToken)
            => await DoAsync(async (iteration, max) => await action.Invoke(), DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, cancellationToken, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan retryInterval)
            => await DoAsync(async (iteration, max) => await action.Invoke(), retryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, int retryLimit)
            => await DoAsync(async (iteration, max) => await action.Invoke(), DefaultRetryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan retryInterval, int retryLimit)
            => await DoAsync(async (iteration, max) => await action.Invoke(), retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan retryInterval, int retryLimit, CancellationToken cancellationToken)
            => await DoAsync(async (iteration, max) => await action.Invoke(), retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, cancellationToken, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        public static async Task DoAsync(RetryActionAsync action, Action<Exception, int, int> onFailure)
            => await DoAsync(async (iteration, max) => await action.Invoke(), DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, onFailure, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan retryInterval, int retryLimit, Action<Exception, int, int> onFailure)
            => await DoAsync(async (iteration, max) => await action.Invoke(), retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, onFailure, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan retryInterval, int retryLimit, CancellationToken cancellationToken, Action<Exception, int, int> onFailure)
            => await DoAsync(async (iteration, max) => await action.Invoke(), retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, cancellationToken, onFailure, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => await DoAsync(async (iteration, max) => await action.Invoke(), retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null, exceptionTypes);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => await DoAsync(async (iteration, max) => await action.Invoke(), retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => await DoAsync(async (iteration, max) => await action.Invoke(), retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionAsync action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, CancellationToken cancellationToken)
            => await DoAsync(async (iteration, max) => await action.Invoke(), retryInterval, retryLimit, retryPolicy, retryPolicyOptions, cancellationToken, null, null);

        #endregion

        #region Do with parameters

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action)
            => await DoAsync(action, DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, CancellationToken cancellationToken)
            => await DoAsync(action, DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, cancellationToken, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval)
            => await DoAsync(action, retryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, int retryLimit)
            => await DoAsync(action, DefaultRetryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval, int retryLimit)
            => await DoAsync(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval, int retryLimit, CancellationToken cancellationToken)
            => await DoAsync(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, cancellationToken, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval, int retryLimit, Action<Exception, int, int> onFailure)
            => await DoAsync(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, onFailure, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval, int retryLimit, CancellationToken cancellationToken, Action<Exception, int, int> onFailure)
            => await DoAsync(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, cancellationToken, onFailure, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, params Type[] exceptionTypes)
            => await DoAsync(action, DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null, exceptionTypes);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => await DoAsync(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null, exceptionTypes);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => await DoAsync(action, retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => await DoAsync(action, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <returns></returns>
        public static async Task DoAsync(RetryActionWithParametersAsync action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, CancellationToken cancellationToken)
            => await DoAsync(action, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, cancellationToken, null, null);

        #endregion

        #region Do with result and without parameters

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithResultAsync<T> action)
            => await DoAsync<T>(action, DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithResultAsync<T> action, CancellationToken cancellationToken)
            => await DoAsync<T>(action, DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, cancellationToken, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithResultAsync<T> action, TimeSpan retryInterval)
            => await DoAsync<T>(action, retryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithResultAsync<T> action, int retryLimit)
            => await DoAsync<T>(action, DefaultRetryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        public static async Task<T> DoAsync<T>(RetryActionWithResultAsync<T> action, TimeSpan retryInterval, int retryLimit, Action<Exception, int, int> onFailure)
            => await DoAsync<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, onFailure, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        public static async Task<T> DoAsync<T>(RetryActionWithResultAsync<T> action, TimeSpan retryInterval, int retryLimit, CancellationToken cancellationToken, Action<Exception, int, int> onFailure)
            => await DoAsync<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, cancellationToken, onFailure, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithResultAsync<T> action, TimeSpan retryInterval, int retryLimit)
            => await DoAsync<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithResultAsync<T> action, TimeSpan retryInterval, int retryLimit, CancellationToken cancellationToken)
            => await DoAsync<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, cancellationToken, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <returns></returns>
        public
            static async Task<T> DoAsync<T>(RetryActionWithResultAsync<T> action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => await DoAsync<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null, exceptionTypes);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithResultAsync<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => await DoAsync<T>(action, retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithResultAsync<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => await DoAsync<T>(action, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithResultAsync<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, CancellationToken cancellationToken)
            => await DoAsync<T>(action, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, cancellationToken, null, null);

        #endregion

        #region Do with parameters and result

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action)
            => await DoAsync<T>(action, DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, CancellationToken cancellationToken)
            => await DoAsync<T>(action, DefaultRetryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, cancellationToken, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval)
            => await DoAsync<T>(action, retryInterval, DefaultRetryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, int retryLimit)
            => await DoAsync<T>(action, DefaultRetryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        public static async Task<T> DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, Action<Exception, int, int> onFailure)
            => await DoAsync<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, onFailure, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        public static async Task<T> DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, CancellationToken cancellationToken, Action<Exception, int, int> onFailure)
            => await DoAsync<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, cancellationToken, onFailure, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit)
            => await DoAsync<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, CancellationToken cancellationToken)
            => await DoAsync<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, cancellationToken, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => await DoAsync<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, null, exceptionTypes);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => await DoAsync<T>(action, retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => await DoAsync<T>(action, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null, null);

        /// <summary>
        /// Perform an asynchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <param name="cancellationToken">An asynchronous cancellation token to cancel the pending retry operation</param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(RetryActionWithParametersAndResultAsync<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, CancellationToken cancellationToken)
            => await DoAsync<T>(action, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, cancellationToken, null, null);

        #endregion
    }
}
