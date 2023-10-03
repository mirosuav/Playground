namespace LanguageExtensions.Monads.Extensions;

public static class OptionExtensions
{
    public static void Do<TX>(this Option<TX> option, Action<TX> action)
    {
        if (option.IsSome)
            action(option.Value);
    }

    public static ValueTask Do<TX>(this Option<TX> option, Func<TX, ValueTask> action)
    {
        if (option.IsSome)
            return action(option.Value);

        return ValueTask.CompletedTask;
    }

    public static Task Do<TX>(this Option<TX> option, Func<TX, Task> action)
    {
        if (option.IsSome)
            return action(option.Value);

        return Task.CompletedTask;
    }
}
