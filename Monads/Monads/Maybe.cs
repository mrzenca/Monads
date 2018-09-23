using System;

namespace Monads
{
    public static class Maybe
    {
        /// <summary>
        /// Invokes a function <paramref name="mapper"/> on type <typeparamref name="U"/>
        /// </summary>
        /// <typeparam name="U">Any type</typeparam>
        /// <param name="mapper">Function on type U</param>
        /// <returns>A <see cref="Success{T}"/> if the function does not throw an exception, a <see cref="Failure{T}"/> if it does, holding the exception</returns>
        public static Try<U> Invoke<U>(Func<U> mapper)
        {
            try
            {
                return new Success<U>(mapper.Invoke());
            }
            catch (Exception e)
            {
                return new Failure<U>(e);
            }
        }
    }
}
