namespace EasyToolkit.Core.Mathematics
{
    /// <summary>
    /// Defines the comparison mode for numeric comparisons in behavior tree conditions.
    /// </summary>
    public enum ComparisonMode
    {
        /// <summary>
        /// Checks if the value is less than the target.
        /// </summary>
        LessThan,

        /// <summary>
        /// Checks if the value is less than or equal to the target.
        /// </summary>
        LessThanOrEqualTo,

        /// <summary>
        /// Checks if the value is equal to the target.
        /// </summary>
        EqualTo,

        /// <summary>
        /// Checks if the value is not equal to the target.
        /// </summary>
        NotEqualTo,

        /// <summary>
        /// Checks if the value is greater than or equal to the target.
        /// </summary>
        GreaterThanOrEqualTo,

        /// <summary>
        /// Checks if the value is greater than the target.
        /// </summary>
        GreaterThan
    }
}
