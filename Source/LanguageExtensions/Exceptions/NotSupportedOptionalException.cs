namespace LanguageExtensions.Exceptions;

public class NotSupportedOptionalException : NotSupportedException
{
    public static readonly NotSupportedOptionalException Instance = new NotSupportedOptionalException();

    const string _message = "Given Optional object is not supported in current context!";
    public NotSupportedOptionalException() : base(_message)
    {
    }
}
