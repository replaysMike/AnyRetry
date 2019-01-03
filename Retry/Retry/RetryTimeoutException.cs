using System;
using System.Collections.Generic;
using System.Linq;

namespace Retry
{
    /// <summary>
    /// Retry timeout exception
    /// </summary>
    public class RetryTimeoutException : Exception
    {
        /// <summary>
        /// List of exceptions
        /// </summary>
        public ICollection<Exception> Exceptions { get; } = new List<Exception>();

        /// <summary>
        /// The retry count
        /// </summary>
        public int RetryCount { get; }

        /// <summary>
        /// The exception message from inner exceptions
        /// </summary>
        public override string Message
        {
            get
            {
                return GetMessage(RetryCount, Exceptions);
            }
        }

        /// <summary>
        /// Create a retry timeout exception
        /// </summary>
        /// <param name="exceptions"></param>
        /// <param name="retryCount"></param>
        public RetryTimeoutException(ICollection<Exception> exceptions, int retryCount) : base(GetMessage(retryCount, exceptions), exceptions.First())
        {
            Exceptions = exceptions;
            RetryCount = retryCount;
        }

        /// <summary>
        /// Get the exception message from inner exceptions
        /// </summary>
        /// <param name="retryCount"></param>
        /// <param name="exceptions"></param>
        /// <returns></returns>
        private static string GetMessage(int retryCount, ICollection<Exception> exceptions)
        {
            if (exceptions == null || exceptions.Count == 0)
                throw new ArgumentNullException(nameof(exceptions));
            return $"Command retry [{retryCount}] exceeded. Please see the inner exception for details. {(exceptions.Count > 0 ? exceptions.First().Message : "No inner exception available.")}";
        }
    }
}
