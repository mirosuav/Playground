using System.Runtime.CompilerServices;

namespace LanguageExtensions.Monads;

public static partial class Option
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> None<T>() => Option<T>.None;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> For<T>(T value) => value.ToOption();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> For<T>(T? value) where T : struct => value.ToOption();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> ToOption<T>(this T value) => Option<T>.Create(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> ToOption<T>(this T? value) where T : struct => Option<T>.Create(value);

    /// <summary>
    /// Tests the Optional value for condition and throws or returns None when condition is not met.
    /// </summary>
    public static Option<T> Test<T>(this Option<T> option, Predicate<T> predicate, bool throwOnFalse = false)
    {
        if (option.IsSome && predicate(option.Reduce()))
        {
            if (throwOnFalse)
                throw new ApplicationException($"Option<{typeof(T).Name}> condition failed.");
            else
                return Option.None<T>();
        }

        return option;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Do<T>(this Option<T> option, Action<T> action)
    {
        if (option.IsSome)
            action(option.Reduce());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ValueTask Do<T>(this Option<T> option, Func<T, ValueTask> action)
    {
        if (option.IsSome)
            return action(option.Reduce());

        return ValueTask.CompletedTask;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task Do<T>(this Option<T> option, Func<T, Task> action)
    {
        if (option.IsSome)
            return action(option.Reduce());

        return Task.CompletedTask;
    }
}
