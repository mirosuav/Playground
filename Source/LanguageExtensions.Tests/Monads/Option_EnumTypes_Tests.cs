using FluentAssertions;
using LanguageExtensions.Monads;

namespace LanguageExtensions.Tests.Monads;

public class Option_EnumTypes_Tests
{
    internal enum ResultState
    {
        None,
        Success,
        Failure
    }

    [Fact]
    public void None_Enum_IsSome_IsFalse()
        => Option.None<ResultState>().IsSome.Should().BeFalse();

    [Fact]
    public void None_NullableEnum_IsSome_IsFalse()
        => Option.None<ResultState?>().IsSome.Should().BeFalse();

    [Fact]
    public void None_Enum_Value_IsZero()
        => Option.None<ResultState>().Reduce().Should().Be(ResultState.None);

    [Fact]
    public void None_NullableEnum_Value_IsNull()
        => Option.None<ResultState?>().Reduce().Should().BeNull();




    [Fact]
    public void Create_EnumZero_IsSome_IsTrue()
        => Option.For(ResultState.None).IsSome.Should().BeTrue();

    [Fact]
    public void Create_NullableEnumNull_IsSome_IsFalse()
        => Option.For<ResultState?>(null).IsSome.Should().BeFalse();

    [Fact]
    public void Create_EnumNonZero_IsSome_IsTrue()
        => ResultState.Success.ToOption().IsSome.Should().BeTrue();

    [Fact]
    public void Create_NullableEnumNonZero_IsSome_IsTrue()
        => Option.For<ResultState?>(ResultState.Success).IsSome.Should().BeTrue();




    [Fact]
    public void Create_EnumNonZero_Value_IsValue()
        => Option.For(ResultState.Success)
        .Reduce().Should().Be(ResultState.Success);

    [Fact]
    public void Create_NullableEnumNonZero_Value_IsValue()
        => Option.For<ResultState?>(ResultState.Success)
        .Reduce().Should().Be(ResultState.Success);
}
