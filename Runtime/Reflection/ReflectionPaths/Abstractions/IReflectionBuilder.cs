namespace EasyToolkit.Core.Reflection
{
    public interface IReflectionBuilder
    {
        /// <summary>
        /// Gets the member path this accessor operates on.
        /// </summary>
        string MemberPath { get; }
    }
}
