using System;

namespace Product.API.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class ConcurrencyOperationException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ConcurrencyOperationException(string message) : base(message)
        {

        }
    }
}
