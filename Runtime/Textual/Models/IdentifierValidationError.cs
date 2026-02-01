namespace EasyToolkit.Core.Textual
{
    /// <summary>
    /// Type of identifier validation error.
    /// </summary>
    public enum IdentifierValidationError
    {
        Empty,
        InvalidFirstCharacter,
        InvalidCharacter,
        ReservedKeyword
    }
}
