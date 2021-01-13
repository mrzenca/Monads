﻿using System;

namespace Monads
{
    /// <summary>
    /// Exception used by family of <see cref="Try{T}"/> classes to throw it's own exceptions.
    /// </summary>
    public class TryException : Exception
    {
        /// <param name="message">Message for exception, formatted with the <paramref name="p"/> parameters</param>
        /// <param name="p">Parameters used in <paramref name="message"/></param>
        public TryException(string message, params object[] p)
            : base(string.Format(message, p))
        {
        }
    }
}
