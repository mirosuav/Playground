using LanguageExtensions.Objects;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LanguageExtensions.Monads;


/// <summary>
/// Optional object monad. Supports reference types, Value types and Nullable types.
/// </summary>
/// <typeparam name="T">Type of contained object</typeparam>
[DebuggerStepThrough]
public record struct Option<T>
{
    private const string NoneString = "None";

    /// <summary>
    /// Indicates none/empty value
    /// </summary>
    public static readonly Option<T> None = default;

    private readonly T _value;
    private readonly bool _isSome;

    /// <summary>
    /// Access raw value. It can be null.
    /// </summary>
    public T Value => _value;

    /// <summary>
    /// Has non-null value
    /// </summary>
    public bool IsSome => _isSome;

    /// <summary>
    /// IS empty/none
    /// </summary>
    public bool IsNone => !_isSome;
    private Option(T value, bool isSome) => (_value, _isSome) = (value, isSome);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TType> Create<TType>(TType value)
        => TypeCheck<TType>.IsNull(value) ? Option<TType>.None : new Option<TType>(value, true);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TStruct> Create<TStruct>(TStruct? value) where TStruct : struct
        => value.HasValue ? Option<TStruct>.Create(value.Value) : Option<TStruct>.None;

    /// <summary>
    /// Creates Option with value but that is signaled as None
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static Option<T> Artificial(T value) => new Option<T>(value, false);

    /// <summary>
    /// Call provided mapper with concrete X value if instance is Some<X>, returns None<Y> otherwise.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Option<Y> Map<Y>(Func<T, Y> mapper) => IsSome ? Option<Y>.Create(mapper(_value)) : Option<Y>.None;

    /// <summary>
    /// Extract encapsulated value if instance is Some<X>, returns provided default otherwise.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Reduce(T @default = default!) => IsSome ? _value : @default;

    /// <summary>
    /// Reduce value is exists and cast it to target type <typeparamref name="TCast"/>. Return None<<typeparamref name="TCast"/>> is fails.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TCast ReduceAs<TCast>(TCast @default = default!) => IsSome && _value is TCast outVal ? outVal : @default;

    public override string? ToString() => IsSome ? _value!.ToString() : NoneString;

    public static implicit operator Option<T>(T value) => Create(value);
}
