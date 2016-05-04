using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio
{
    /// <summary>
    /// Wraps an exception to know when it happens in an async method.
    /// </summary>
    /// <typeparam name="TException">Any type of exception.</typeparam>
    class AsyncException<TException>
    {
        /// <summary>
        /// Where the Exception is wrapped.
        /// </summary>
        public TException Error { get; set; }
        /// <summary>
        /// Remember to set to true in the catch, and false when handled.
        /// </summary>
        public bool WasRaised { get; set; }
    }
}
