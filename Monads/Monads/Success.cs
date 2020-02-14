using System;

namespace Monads
{
    /// <summary>
    /// Success{T} is a <see cref="Try{T}"/> subclass that holds a value of type {T} after successful invoking a <see cref="Try{T}.Map{U}"/>.
    /// </summary>
    /// <typeparam name="T">asd</typeparam>
    public class Success<T> : Try<T>
    {
        private readonly T value;

        public Success(T value)
        {
            this.value = value;
        }

        public override T Value
        {
            get { return value; }
        }

        public override bool IsFailure
        {
            get { return false; }
        }

        public override bool IsSuccess
        {
            get { return true; }
        }

        public override T Get()
        {
            return value;
        }

        public override T GetOrDefault()
        {
            return value;
        }

        public override T GetOrElse(T other)
        {
            return value;
        }

        public override T GetOrException(Exception exception)
        {
            return value;
        }

        public override T GetOrException(string message)
        {
            return value;
        }

        public override Try<T> OrElse(Func<T> other)
        {
            return this;
        }

        public override Try<U> Map<U>(Func<T, U> mapper)
        {
            return Maybe.Invoke(() => mapper.Invoke(value));
        }

        public override Try<U> FlatMap<U>(Func<T, Try<U>> mapper)
        {
            return Maybe.Invoke(() => mapper.Invoke(value).Get());
        }

        public override Try<T> Recover(Func<Exception, T> recover)
        {
            return this;
        }

        public override Try<T> Recover<U>(Func<Exception, T> recover)
        {
            return this;
        }

        public override Try<T> Filter(Predicate<T> predicate)
        {
            try
            {
                var result = predicate.Invoke(value);

                if (result)
                {
                    return this;
                }

                return new Failure<T>("There are no results from applying the predicate");
            }
            catch
            {
                return new Failure<T>("There are no results from applying the predicate");
            }
        }

        public override Try<T> Set(Action<T> setter)
        {
            try
            {
                setter.Invoke(value);

                return this;
            }
            catch (Exception ex)
            {
                return new Failure<T>(ex);
            }
        }

        public override Try<T> TrySet(Action<T> setter)
        {
            try
            {
                setter.Invoke(value);

                return this;
            }
            catch (Exception)
            {
                return this;
            }
        }

        public override Try<T> Is(Predicate<T> predicate, Action<T> setter)
        {
            try
            {
                if (!this.IsEvaluated && predicate(value))
                {
                    setter(value);
                    this.isEvaluated = true;
                }

                return this;
            }
            catch (Exception ex)
            {
                return new Failure<T>(ex);
            }
        }

        public override Try<T> Is(Predicate<T> predicate, Exception exception)
        {
            if (!this.IsEvaluated && predicate(value))
            {
                this.isEvaluated = true;
                return new Failure<T>(exception);
            }

            return this;
        }
    }
}
