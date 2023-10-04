using FluentAssertions;
using LanguageExtensions.Monads;

namespace LanguageExtensions.Tests.Monads;

public class Option_ValueTypes_Tests
{
    [Fact]
    public void None_Int_IsSome_IsFalse()
        => Option<int>.None.IsSome.Should().BeFalse();

    [Fact]
    public void None_NullableInt_IsSome_IsFalse()
        => Option<int?>.None.IsSome.Should().BeFalse();

    [Fact]
    public void None_Int_Value_IsZero()
        => Option<int>.None.Reduce().Should().Be(0);

    [Fact]
    public void None_NullableInt_Value_IsNull()
        => Option<int?>.None.Reduce().Should().BeNull();




    [Fact]
    public void Create_IntZero_IsSome_IsTrue()
        => Option.For<int>(0).IsSome.Should().BeTrue();

    [Fact]
    public void Create_NullableIntNull_IsSome_IsFalse()
        => Option.For<int?>(null).IsSome.Should().BeFalse();

    [Fact]
    public void Create_IntNonZero_IsSome_IsTrue()
        => Option.For(1).IsSome.Should().BeTrue();

    [Fact]
    public void Create_NullableIntNonZero_IsSome_IsTrue()
        => Option.For<int?>(1).IsSome.Should().BeTrue();




    [Fact]
    public void Create_IntNonZero_Value_IsValue()
        => Option.For(1).Reduce().Should().Be(1);

    [Fact]
    public void Create_NullableIntNonZero_Value_IsValue()
        => Option.For<int?>(1).Reduce().Should().Be(1);



}
