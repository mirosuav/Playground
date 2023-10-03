using LanguageExtensions.Objects;
using System.Runtime.CompilerServices;

namespace LanguageExtensions.Monads;

public record struct Option<X>
{
    private readonly bool IsSome;
    private readonly X Value;
    internal Option(X value)
    {
        Value = value;
        IsSome = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<Y> Map<Y>(Func<X, Y> mapper)
        => IsSome
        ? Option<Y>.Create(mapper(Value))
        : Option<Y>.None;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public X Reduce(X @default) => IsSome ? Value : @default;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<X> Create(X value)
        => TypeCheck<X>.IsNull(value)
        ? None
        : Some(value);

    public static readonly Option<X> None = default;
    public static Option<X> Some(X value) => new(value);
}


