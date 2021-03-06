using System;
using System.Linq.Expressions;
using System.Reflection;
using Checkk.Exceptions;

namespace Checkk
{
    /// <summary>
    /// Methods for checking a generic instance of T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CheckGenericInvariant<T>
    {
        protected readonly Expression<Func<T>> Target;

        public CheckGenericInvariant(Expression<Func<T>> target)
        {
            Target = target;
        }

        /// <summary>
        /// Check that the target value is not null.
        /// Throws an InvariantShouldNotBeNullException if the target value is null.
        /// </summary>
        /// <param name="message">An optional message that overrides the automatically generated one if the check fails</param>
        public void IsNotNull(string message = null)
        {
            if (object.ReferenceEquals(TargetValue, null))
            {
                throw new InvariantShouldNotBeNullException(FieldName, message);
            }
        }

        protected T TargetValue { get { return Target.Compile()(); } }

        protected string FieldName
        {
            get
            {
                var memberExpression = Target.Body as MemberExpression;
                if (memberExpression == null)
                {
                    throw new ArgumentException(string.Format(
                        "Expression '{0}' refers to a method, not a field",
                        Target));
                }

                var fieldInfo = memberExpression.Member as FieldInfo;
                if (fieldInfo == null)
                {
                    throw new ArgumentException(string.Format(
                        "Expression '{0}' refers to a method, not a field",
                        Target));
                }

                return fieldInfo.Name;
            }
        }
    }
}