using System;

namespace Monads
{
    /// <summary>
    /// Abstract monad class that holds a value (in a <see cref="Success{T}"/> or an exception (in a <see cref="Failure{T}"/>.
    /// </summary>
    /// <typeparam name="T">asd</typeparam>
    public abstract class Try<T>
    {
        protected bool isEvaluated = false;

        public bool IsEvaluated
        {
            get
            {
                return isEvaluated;
            }
        }

        /// <summary>
        /// Gets a value indicating whether returns whether a <see cref="Try{T}"/> is a <see cref="Success{T}"/> (true) or a <see cref="Failure{T}"/>  (false)
        /// </summary>
        public abstract bool IsSuccess { get; }

        /// <summary>
        /// Gets a value indicating whether returns whether a <see cref="Try{T}"/>is a <see cref="Failure{T}"/> (true) or a <see cref="Success{T}"/> (false)
        /// </summary>
        public abstract bool IsFailure { get; }

        /// <summary>
        /// Gets the value of the <see cref="Try{T}"/> if it is a <see cref="Success{T}"/>, and throws an exception if it is a <see cref="Failure{T}"/>
        /// </summary>
        /// <returns>Value if it is a <see cref="Success{T}"/> or the exception if it is a <see cref="Failure{T}"/></returns>
        public abstract T Value { get; }

        /// <summary>
        /// Checks conditions in given <see cref="Predicate{T}"/> depending on result it resolves or not <see cref="Action{T}"/>
        /// </summary>
        /// <returns>A current instance of <see cref="Try{T}"/> if the function does not throw an exception, A <see cref="Failure{T}"/> if it does, holding the exception</returns>
        public abstract Try<T> Is(Predicate<T> predicate, Action<T> setter);

        /// <summary>
        /// Checks conditions in given <see cref="Predicate{T}"/> depending on result it resolves or not <see cref="Action{T}"/>
        /// </summary>
        /// <returns>A current instance of <see cref="Try{T}"/> if the function does not throw an exception, if it dooes, it throws defined in input parameter exception</returns>
        public abstract Try<T> Is(Predicate<T> predicate, Exception exception);

        /// <summary>
        /// Gets the value of the <see cref="Try{T}"/> if it is a <see cref="Success{T}"/>, and throws an exception if it is a <see cref="Failure{T}"/>
        /// </summary>
        /// <returns>Value if it is a <see cref="Success{T}"/> or the exception if it is a <see cref="Failure{T}"/></returns>
        public abstract T Get();

        /// <summary>
        /// Gets the value of the <see cref="Try{T}"/> if it is a <see cref="Success{T}"/>, and the default value of T if it is a <see cref="Failure{T}"/>
        /// </summary>
        /// <returns>Value if it is a <see cref="Success{T}"/> or the default of T if it is a <see cref="Failure{T}"/></returns>
        public abstract T GetOrDefault();

        /// <summary>
        /// Gets the value if it is a <see cref="Success{T}"/> or the <paramref name="other"/> otherwise
        /// </summary>
        /// <param name="other">asd</param>
        /// <returns>Value or <paramref name="other"/> if this is a <see cref="Failure{T}"/></returns>
        public abstract T GetOrElse(T other);

        /// <summary>
        /// Get value or throw Exception with given message and inner exception from Failure.cs property
        /// </summary>
        /// <param name="message">string</param>
        public abstract T GetOrException(string message);

        /// <summary>
        /// Get value or throw specific exception
        /// </summary>
        /// <param name="exception">Exception</param>
        /// <returns>asd</returns>
        public abstract T GetOrException(Exception exception);

        /// <summary>
        /// Return this <see cref="Try{T}"/> if it is a <see cref="Success{T}"/> or the result of <paramref name="other"/> if it is a <see cref="Failure{T}"/>
        /// </summary>
        /// <param name="other">asd</param>
        public abstract Try<T> OrElse(Func<T> other);

        /// <summary>
        /// Maps an instance of type T to an instance of type U by invoking function mapper
        /// </summary>
        /// <typeparam name="U">Any type that is the result of invoking the function</typeparam>
        /// <param name="mapper">Function on type T, which is the type of the current value</param>
        /// <returns>A <see cref="Success{T}"/> if the function does not throw an exception, A <see cref="Failure{T}"/> if it does, holding the exception</returns>
        public abstract Try<U> Map<U>(Func<T, U> mapper);

        /// <summary>
        /// Maps an instance of type T to an instance of type <see cref="Try{T}"/> by invoking the function mapper
        /// </summary>
        /// <typeparam name="U">Any type that is the result of invoking the function</typeparam>
        /// <param name="mapper">Function on type T, which is the type of the current value</param>
        /// <returns>A <see cref="Success{T}"/> if the function does not throw an exception, A <see cref="Failure{T}"/> if it does, holding the exception</returns>
        public abstract Try<U> FlatMap<U>(Func<T, Try<U>> mapper);

        /// <summary>
        /// Recover from a Failure holding any exception to a <see cref="Try{T}"/>
        /// </summary>
        /// <param name="recover">Function returning an instance of type T</param>
        /// <returns><see cref="Success{T}"/> if invoking function does not throw an exception, a <see cref="Failure{T}"/> otherwise</returns>
        public abstract Try<T> Recover(Func<Exception, T> recover);

        /// <summary>
        /// Recover from a Failure holding an exception of type U to a <see cref="Try{T}"/>
        /// </summary>
        /// <param name="recover">Function returning an instance of type T</param>
        /// <returns><see cref="Success{T}"/> if invoking function does not throw an exception, a <see cref="Failure{T}"/> otherwise</returns>
        public abstract Try<T> Recover<U>(Func<Exception, T> recover)
            where U : Exception;

        /// <summary>
        /// Filters using a predicate
        /// </summary>
        /// <param name="predicate">Predicate to test on T</param>
        /// <returns><see cref="Success{T}"/> if predicate returns true, <see cref="Failure{T}"/> if it returns false</returns>
        public abstract Try<T> Filter(Predicate<T> predicate);

        /// <summary>
        /// Set value on an object and return instance of type T
        /// </summary>
        /// <param name="setter">Action on type T, which is the type of the current value</param>
        /// <returns>A <see cref="Success{T}"/> if the function does not throw an exception, A <see cref="Failure{T}"/> if it does, holding the exception</returns>
        public abstract Try<T> Set(Action<T> setter);

        /// <summary>
        /// Can set value on an object and return is
        /// </summary>
        /// <param name="setter">Action on type T, which is the type of the current value</param>
        /// <returns>A <see cref="Success{T}"/> if the function does not throw an exception, A <see cref="Success{T}"/> if it does, holding the previous</returns>
        public abstract Try<T> TrySet(Action<T> setter);
    }
}
