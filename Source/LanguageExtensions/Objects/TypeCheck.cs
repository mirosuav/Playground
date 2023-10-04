using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LanguageExtensions.Objects;

internal static class TypeCheck<T>
{
    private static readonly bool IsReferenceType;
    private static readonly bool IsNullableType;

    static TypeCheck()
    {
        //T is Nullable type
        IsNullableType = Nullable.GetUnderlyingType(typeof(T)) is not null;
        //T is reference type
        IsReferenceType = !typeof(T).IsValueType;
    }

    /// <summary>
    /// Checks if value is null for Reference or Nullable types. 
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerStepThrough]
    internal static bool IsNull(T value)
        => IsReferenceType ? value is null
        : IsNullableType ? value!.Equals(default)
        : false;
}

