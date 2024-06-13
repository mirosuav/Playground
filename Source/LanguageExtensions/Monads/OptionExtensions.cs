using LanguageExtensions.Exceptions;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LanguageExtensions.Monads;

[DebuggerStepThrough]
public static partial class Option
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Option<T> None<T>() => Option<T>.None;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Option<T> For<T>(T value) => Option<T>.Create(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Option<T> For<T>(T? value) where T : struct => Option<T>.Create(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Option<T> ToOption<T>(this T value) => Option<T>.Create(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Option<T> ToOption<T>(this T? value) where T : struct => Option<T>.Create(value);


    /// <summary>
    /// Map Option<T> to Option<Y> only if provided condition is met. Returns None<Y> otherwise
    /// </summary>
    public static Option<Y> MapIf<T, Y>(this Option<T> option, Func<T, bool> condition, Func<T, Y> mapper)
    {
        if (option.IsSome)
        {
            if (condition(option.Value))
                return Option<Y>.Create(mapper(option.Value));
        }
        return Option<Y>.None;
    }

    /// <summary>
    /// Tests the Optional value for condition and throws when condition is met otherwise returns original Option. For None option skips testing and returns.
    /// </summary>
    public static Option<T> ThrowIf<T>(this Option<T> option, Func<T, bool> throwCondition)
    {
        if (option.IsSome && throwCondition(option.Value))
            throw OptionalObjectException.InvalidObject;

        return option;
    }

    /// <summary>
    /// Returns None<T> when provided condition is met otherwise returns original Option. Skips checking if option is already None.
    /// </summary>
    public static Option<T> NoneIf<T>(this Option<T> option, Func<T, bool> noneCondition)
    {
        if (option.IsSome && noneCondition(option.Value))
            return None<T>();

        return option;
    }
    public static Option<T> NoneIfEmpty<T>(this Option<T> option) where T : ICollection
    {
        if (option.IsSome && option.Value.Count == 0)
            return None<T>();

        return option;
    }

    public static void Do<T>(this Option<T> option, Action<T> action)
    {
        if (option.IsSome)
            action(option.Value);
    }

    public static ValueTask Do<T>(this Option<T> option, Func<T, ValueTask> action)
    {
        if (option.IsSome)
            return action(option.Value);

        return ValueTask.CompletedTask;
    }

    public static Task Do<T>(this Option<T> option, Func<T, Task> action)
    {
        if (option.IsSome)
            return action(option.Value);

        return Task.CompletedTask;
    }
}
