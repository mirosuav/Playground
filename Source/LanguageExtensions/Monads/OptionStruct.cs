using LanguageExtensions.Objects;
using System.Runtime.CompilerServices;

namespace LanguageExtensions.Monads;

/// <summary>
/// Optional object monad. Supports reference types, Value types and Nullable types.
/// </summary>
/// <typeparam name="TX">Type of contained object</typeparam>
public record struct Option<TX>
{
    public static readonly Option<TX> None = default;
    public readonly bool IsSome;

    internal readonly TX Value;

    internal Option(TX value)
    {
        Value = value;
        IsSome = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<TY> Map<TY>(Func<TX, TY> mapper) 
        => IsSome ? Option.Create(mapper(Value)) : Option<TY>.None;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TX Reduce(TX @default) => IsSome ? Value : @default;

}


