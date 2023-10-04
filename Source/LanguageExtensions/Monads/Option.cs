using LanguageExtensions.Objects;
using System.Runtime.CompilerServices;

namespace LanguageExtensions.Monads;

public static class Option
{
    public static Option<TX> None<TX>() => Option<TX>.None;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TX> Create<TX>(TX? value) 
        => TypeCheck<TX>.IsNull(value) 
        ? None<TX>() 
        : new Option<TX>(value!);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TX> Some<TX>(TX value) 
        => new Option<TX>(value);
}


