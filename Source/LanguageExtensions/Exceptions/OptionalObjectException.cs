namespace LanguageExtensions.Exceptions;

public class OptionalObjectException : NotSupportedException
{
    public static readonly OptionalObjectException InvalidObject = new OptionalObjectException();

    const string _message = "Given Optional object is invalid in current context!";
    public OptionalObjectException() : base(_message)
    {
    }
}
