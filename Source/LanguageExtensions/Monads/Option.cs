using LanguageExtensions.Objects;
using System.Runtime.CompilerServices;

namespace LanguageExtensions.Monads;

/// <summary>
/// Optional object monad. Supports reference types, Value types and Nullable types.
/// </summary>
/// <typeparam name="TX">Type of contained object</typeparam>
public record struct Option<TX>
{
    public readonly bool IsSome;

    internal readonly TX Value;

    private Option(TX value)
    {
        Value = value;
        IsSome = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<TY> Map<TY>(Func<TX, TY> mapper) => IsSome ? Option<TY>.Create(mapper(Value)) : Option<TY>.None;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TX Reduce(TX @default) => IsSome ? Value : @default;



    public static readonly Option<TX> None = default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TX> Create(TX? value) => TypeCheck<TX>.IsNull(value) ? None : new(value!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TX> Some(TX value) => new(value);

}


