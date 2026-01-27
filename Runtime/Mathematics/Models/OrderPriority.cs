using System;

namespace EasyToolKit.Core.Mathematics
{
    /// <summary>
    /// Represents a priority value for ordering operations with floating-point comparison support.
    /// </summary>
    public struct OrderPriority : IEquatable<OrderPriority>, IComparable<OrderPriority>, IComparable
    {
        /// <summary>
        /// The default priority value with a value of 0.0.
        /// </summary>
        public static readonly OrderPriority Default = new OrderPriority(0.0);

        /// <summary>
        /// Gets the priority value.
        /// </summary>
        public readonly double Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderPriority"/> struct with the specified value.
        /// </summary>
        /// <param name="value">The priority value.</param>
        public OrderPriority(double value)
        {
            Value = value;
        }

        /// <summary>
        /// Determines whether two <see cref="OrderPriority"/> instances are equal using approximate comparison.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><c>true</c> if the values are approximately equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(OrderPriority left, OrderPriority right)
        {
            return left.Value.IsApproximatelyOf(right.Value);
        }

        /// <summary>
        /// Determines whether two <see cref="OrderPriority"/> instances are not equal.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><c>true</c> if the values are not approximately equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(OrderPriority left, OrderPriority right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether the left <see cref="OrderPriority"/> is greater than the right one.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><c>true</c> if the left value is greater than the right value and not approximately equal; otherwise, <c>false</c>.</returns>
        public static bool operator >(OrderPriority left, OrderPriority right)
        {
            return left.Value > right.Value && !(left == right);
        }

        /// <summary>
        /// Determines whether the left <see cref="OrderPriority"/> is less than the right one.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><c>true</c> if the left value is less than the right value and not approximately equal; otherwise, <c>false</c>.</returns>
        public static bool operator <(OrderPriority left, OrderPriority right)
        {
            return left.Value < right.Value && !(left == right);
        }

        /// <summary>
        /// Determines whether the left <see cref="OrderPriority"/> is greater than or approximately equal to the right one.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><c>true</c> if the left value is greater than or approximately equal to the right value; otherwise, <c>false</c>.</returns>
        public static bool operator >=(OrderPriority left, OrderPriority right)
        {
            return left.Value > right.Value || left == right;
        }

        /// <summary>
        /// Determines whether the left <see cref="OrderPriority"/> is less than or approximately equal to the right one.
        /// </summary>
        /// <param name="left">The left operand.</param>
        /// <param name="right">The right operand.</param>
        /// <returns><c>true</c> if the left value is less than or approximately equal to the right value; otherwise, <c>false</c>.</returns>
        public static bool operator <=(OrderPriority left, OrderPriority right)
        {
            return left.Value < right.Value || left == right;
        }

        /// <summary>
        /// Returns the hash code for the current <see cref="OrderPriority"/>.
        /// </summary>
        /// <returns>A hash code for the current <see cref="OrderPriority"/>.</returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="OrderPriority"/>.
        /// </summary>
        /// <param name="obj">The object to compare with the current <see cref="OrderPriority"/>.</param>
        /// <returns><c>true</c> if the specified object is equal to the current <see cref="OrderPriority"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return obj is OrderPriority priority && this == priority;
        }

        /// <summary>
        /// Determines whether the specified <see cref="OrderPriority"/> is equal to the current <see cref="OrderPriority"/>.
        /// </summary>
        /// <param name="other">The <see cref="OrderPriority"/> to compare with the current <see cref="OrderPriority"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="OrderPriority"/> is equal to the current <see cref="OrderPriority"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(OrderPriority other)
        {
            return this == other;
        }

        /// <summary>
        /// Compares the current <see cref="OrderPriority"/> with another <see cref="OrderPriority"/>.
        /// </summary>
        /// <param name="other">The <see cref="OrderPriority"/> to compare with.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        public int CompareTo(OrderPriority other)
        {
            if (this == other)
            {
                return 0;
            }

            return this > other ? 1 : -1;
        }

        /// <summary>
        /// Compares the current <see cref="OrderPriority"/> with another object.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns>A value that indicates the relative order of the objects being compared.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="obj"/> is not an <see cref="OrderPriority"/>.</exception>
        int IComparable.CompareTo(object obj)
        {
            if (obj is null)
            {
                return 1;
            }

            if (obj is OrderPriority other)
            {
                return CompareTo(other);
            }

            throw new ArgumentException($"Object must be of type {nameof(OrderPriority)}", nameof(obj));
        }

        /// <summary>
        /// Returns the string representation of the current <see cref="OrderPriority"/>.
        /// </summary>
        /// <returns>A string that represents the current <see cref="OrderPriority"/>.</returns>
        public override string ToString()
        {
            return Value.ToString();
        }

        /// <summary>
        /// Creates a new <see cref="OrderPriority"/> with the specified value.
        /// </summary>
        /// <param name="value">The priority value.</param>
        /// <returns>A new <see cref="OrderPriority"/> instance.</returns>
        public static OrderPriority Of(double value)
        {
            return new OrderPriority(value);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="double"/> to <see cref="OrderPriority"/>.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>An <see cref="OrderPriority"/> instance with the specified value.</returns>
        public static implicit operator OrderPriority(double value)
        {
            return new OrderPriority(value);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="OrderPriority"/> to <see cref="double"/>.
        /// </summary>
        /// <param name="priority">The priority to convert.</param>
        /// <returns>The value of the priority.</returns>
        public static explicit operator double(in OrderPriority priority)
        {
            return priority.Value;
        }
    }
}
