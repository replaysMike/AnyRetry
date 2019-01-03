using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Retry
{
    /// <summary>
    /// A general purpose retry class for retrying code blocks.
    /// </summary>
    public static class Retry
    {
        public static void Do(Action action)
            => Do(action, TimeSpan.FromSeconds(1), int.MaxValue, RetryPolicy.StaticDelay, null, null);
        public static void Do(Action action, int retryCount)
            => Do(action, TimeSpan.FromSeconds(1), retryCount, RetryPolicy.StaticDelay, null, null);
        public static void Do(Action action, TimeSpan retryInterval, int retryCount)
            => Do(action, retryInterval, retryCount, RetryPolicy.StaticDelay, null, null);
        public static void Do(Action action, TimeSpan retryInterval, int retryCount, RetryPolicy retryPolicy)
            => Do(action, retryInterval, retryCount, retryPolicy, null, null);
        public static void Do(Action action, TimeSpan retryInterval, int retryCount, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions)
            => Do(action, retryInterval, retryCount, retryPolicy, retryPolicyOptions, null);

        /// <summary>
        /// Perform a synchronous command that must be re-called if an exception occurs, up to a maximum retry limit.
        /// Example usage: Retry.Do(() => cmd.ExecuteNonQuery(), TimeSpan.FromSeconds(3), 10, typeof(System.Data.SqlClient.SqlException));
        /// </summary>
        /// <param name="action">The Action to call that will be retried until successful.</param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryCount">The maximum number of times to retry (Default 5)</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static void Do(Action action, TimeSpan retryInterval, int retryCount, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, Action<Exception, int, int> onFailure, params Type[] exceptionTypes)
        {
            var exceptions = new List<Exception>();
            var startTime = DateTime.Now;

            for (var retryIteration = 0; retryIteration < retryCount; retryIteration++)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception ex)
                {
                    onFailure?.Invoke(ex, retryIteration, retryCount);
                    exceptions.Add(ex);
                    if (exceptionTypes == null || exceptionTypes.Length == 0 || exceptionTypes.Contains<Type>(ex.GetType()))
                    {
                        if (retryIteration - 1 < retryCount)
                        {
                            var sleepValue = RetryPolicyFactory.Create(retryPolicy, retryPolicyOptions).ApplyPolicy(RetryParameters.Create(startTime, retryInterval, retryIteration, retryCount));
                            Thread.Sleep(sleepValue);
                        }

                    }
                    else
                    {
                        throw;
                    }
                }
            }

            throw new RetryTimeoutException(exceptions, retryCount);
        }

        /// <summary>
        /// Perform a synchronous command that must be re-called if an exception occurs, up to a maximum retry limit.
        /// Example usage: Retry.Do(() => cmd.ExecuteNonQuery(), TimeSpan.FromSeconds(3), 10, typeof(System.Data.SqlClient.SqlException));
        /// </summary>
        /// <param name="action">The Action to call that will be retried until successful.</param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryCount">The maximum number of times to retry (Default 5)</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <exception cref="RetryTimeoutException"></exception>
        public static async Task DoAsync(Func<Task> action, TimeSpan retryInterval, int retryCount = 5, Action<Exception, int, int> onFailure = null, RetryPolicy retryPolicy = RetryPolicy.StaticDelay, RetryPolicyOptions retryPolicyOptions = null, params Type[] exceptionType)
        {
            var exceptions = new List<Exception>();
            var startTime = DateTime.Now;

            for (var retryIteration = 0; retryIteration < retryCount; retryIteration++)
            {
                try
                {
                    await action().ConfigureAwait(false);
                    return;
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                    onFailure?.Invoke(ex, retryIteration++, retryCount);
                    if (exceptionType == null || exceptionType.Length == 0 || exceptionType.Contains<Type>(ex.GetType()))
                    {
                        if (retryIteration - 1 < retryCount)
                        {
                            var sleepValue = RetryPolicyFactory.Create(retryPolicy, retryPolicyOptions).ApplyPolicy(RetryParameters.Create(startTime, retryInterval, retryIteration, retryCount));
                            await Task.Delay(sleepValue);
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            throw new RetryTimeoutException(exceptions, retryCount);
        }

        /// <summary>
        /// Perform a synchronous command that must be re-called if an exception occurs, up to a maximum retry limit.
        /// Example usage: int result = Retry.Do<int>(() => cmd.ExecuteNonQuery(), TimeSpan.FromSeconds(3), 10, typeof(System.Data.SqlClient.SqlException));
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The Action to call that will be retried until successful.</param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryCount">The maximum number of times to retry (Default 5)</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static T Do<T>(Func<T> action, TimeSpan retryInterval, int retryCount, Action<Exception, int, int> onFailure, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, params Type[] exceptionType)
        {
            var exceptions = new List<Exception>();
            var startTime = DateTime.Now;

            for (var retryIteration = 0; retryIteration < retryCount; retryIteration++)
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    onFailure?.Invoke(ex, retryIteration, retryCount);
                    exceptions.Add(ex);
                    if (exceptionType == null || exceptionType.Length == 0 || exceptionType.Contains<Type>(ex.GetType()))
                    {
                        if (retryIteration - 1 < retryCount)
                        {
                            var sleepValue = RetryPolicyFactory.Create(retryPolicy, retryPolicyOptions).ApplyPolicy(RetryParameters.Create(startTime, retryInterval, retryIteration, retryCount));
                            Thread.Sleep(sleepValue);
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            throw new RetryTimeoutException(exceptions, retryCount);
        }

        /// <summary>
        /// Perform a synchronous command that must be re-called if an exception occurs, up to a maximum retry limit.
        /// Example usage: int result = Retry.Do<int>(() => cmd.ExecuteNonQuery(), TimeSpan.FromSeconds(3), 10, typeof(System.Data.SqlClient.SqlException));
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The Action to call that will be retried until successful.</param>
        /// <param name="retryInterval">How often to perform the retry.</param>
        /// <param name="retryCount">The maximum number of times to retry (Default 5)</param>
        /// <param name="onFailure">Will be called upon an exception thrown</param>
        /// <param name="retryPolicy">The retry policy to apply</param>
        /// <exception cref="RetryTimeoutException"></exception>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(Func<Task<T>> action, TimeSpan retryInterval, int retryCount, Action<Exception, int, int> onFailure, RetryPolicy retryPolicy, RetryPolicyOptions retryPolicyOptions, params Type[] exceptionType)
        {
            var exceptions = new List<Exception>();
            var startTime = DateTime.Now;

            for (var retryIteration = 0; retryIteration < retryCount; retryIteration++)
            {
                try
                {
                    return await action().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    onFailure?.Invoke(ex, retryIteration, retryCount);
                    exceptions.Add(ex);
                    if (exceptionType == null || exceptionType.Length == 0 || exceptionType.Contains<Type>(ex.GetType()))
                    {
                        if (retryIteration - 1 < retryCount)
                        {
                            var sleepValue = RetryPolicyFactory.Create(retryPolicy, retryPolicyOptions).ApplyPolicy(RetryParameters.Create(startTime, retryInterval, retryIteration, retryCount));
                            await Task.Delay(sleepValue);
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            throw new RetryTimeoutException(exceptions, retryCount);
        }
    }
}
