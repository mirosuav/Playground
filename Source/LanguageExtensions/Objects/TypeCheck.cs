using System.Reflection;
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static bool IsNull(T value) =>
        (IsReferenceType && value is null) || (IsNullableType && value.Equals(default));
}
