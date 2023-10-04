using LanguageExtensions.Objects;
using System.Runtime.CompilerServices;

namespace LanguageExtensions.Monads;

/// <summary>
/// Optional object monad. Supports reference types, Value types and Nullable types.
/// </summary>
/// <typeparam name="X">Type of contained object</typeparam>
public record struct Option<X>
{
    public static readonly Option<X> None = default;

    private readonly X _value;
    private readonly bool _isSome;

    public bool IsSome => _isSome;
    public bool IsNone => !_isSome;
    private Option(X value, bool isSome) => (_value, _isSome) = (value, isSome);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Create<T>(T value) => TypeCheck<T>.IsNull(value) ? Option<T>.None : new Option<T>(value, true);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Create<T>(T? value) where T : struct
        => value.HasValue ? Option<T>.Create(value.Value) : Option<T>.None;

    /// <summary>
    /// Creates Option with value but that is signaled as None
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Option<T> Artificial<T>(T value) => new Option<T>(value, false);

    /// <summary>
    /// Call provided mapper with concrete X value if instance is Some<X>, returns None<Y> otherwise.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<Y> Map<Y>(Func<X, Y> mapper) => IsSome ? Option<Y>.Create(mapper(_value)) : Option<Y>.None;

    /// <summary>
    /// Extract encapsulated value if instance is Some<X>, returns provided default otherwise.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public X Reduce(X @default = default!) => IsSome ? _value : @default;



}
