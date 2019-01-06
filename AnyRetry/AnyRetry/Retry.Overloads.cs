using System;

namespace AnyRetry
{
    public static partial class Retry
    {
        #region Do without parameters

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        public static void Do(RetryAction action)
            => Do((iteration, max) => action.Invoke(), TimeSpan.FromSeconds(1), int.MaxValue, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        public static void Do(RetryAction action, int retryLimit)
            => Do((iteration, max) => action.Invoke(), TimeSpan.FromSeconds(1), retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        public static void Do(RetryAction action, TimeSpan retryInterval, int retryLimit)
            => Do((iteration, max) => action.Invoke(), retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        public static void Do(RetryAction action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => Do((iteration, max) => action.Invoke(), retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, exceptionTypes);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        public static void Do(RetryAction action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => Do((iteration, max) => action.Invoke(), retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        public static void Do(RetryAction action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => Do((iteration, max) => action.Invoke(), retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null);

        #endregion

        #region Do with parameters

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        public static void Do(RetryActionWithParameters action)
            => Do(action, TimeSpan.FromSeconds(1), int.MaxValue, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        public static void Do(RetryActionWithParameters action, int retryLimit)
            => Do(action, TimeSpan.FromSeconds(1), retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        public static void Do(RetryActionWithParameters action, TimeSpan retryInterval, int retryLimit)
            => Do(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        public static void Do(RetryActionWithParameters action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => Do(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, exceptionTypes);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        public static void Do(RetryActionWithParameters action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => Do(action, retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        public static void Do(RetryActionWithParameters action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => Do(action, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null);
        #endregion

        #region Do with result

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action)
            => Do<T>(action, TimeSpan.FromSeconds(1), int.MaxValue, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, int retryLimit)
            => Do<T>(action, TimeSpan.FromSeconds(1), retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval, int retryLimit)
            => Do<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="exceptionTypes">A list of exceptions that will be retried gracefully. All other exceptions will be rethrown.</param>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval, int retryLimit, params Type[] exceptionTypes)
            => Do<T>(action, retryInterval, retryLimit, RetryPolicy.StaticDelay, RetryPolicyOptions.None, null, null, exceptionTypes);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy)
            => Do<T>(action, retryInterval, retryLimit, retryPolicy, RetryPolicyOptions.None, null, null);

        /// <summary>
        /// Perform an synchronous retry up to the maximum specified limit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryLimit">The maximum number of times to retry (Default 5)</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <param name="retryPolicyOptions">Options to specify further configuration for a retry policy</param>
        /// <returns></returns>
        public static T Do<T>(RetryActionWithParametersAndResult<T> action, TimeSpan retryInterval, int retryLimit, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => Do<T>(action, retryInterval, retryLimit, retryPolicy, retryPolicyOptions, null, null);

        #endregion
    }
}
