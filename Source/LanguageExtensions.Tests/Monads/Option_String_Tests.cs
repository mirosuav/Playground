using FluentAssertions;
using LanguageExtensions.Monads;

namespace LanguageExtensions.Tests.Monads;

public class Option_String_Tests
{
    [Fact]
    public void None_String_IsSome_IsFalse()
        => Option<string>.None.IsSome.Should().BeFalse();

    [Fact]
    public void None_NullableString_IsSome_IsFalse()
        => Option<string?>.None.IsSome.Should().BeFalse();

    [Fact]
    public void None_String_Value_IsNull()
        => Option<string>.None.Value.Should().BeNull();

    [Fact]
    public void None_NullableString_Value_IsNull()
        => Option<string?>.None.Value.Should().BeNull();




    [Fact]
    public void Create_StringEmpty_IsSome_IsTrue()
        => Option.Create<string>(string.Empty).IsSome.Should().BeTrue();

    [Fact]
    public void Create_StringNull_IsSome_IsFalse()
        => Option.Create<string>(null).IsSome.Should().BeFalse();


    [Fact]
    public void Create_NullableString_Null_IsSome_IsFalse()
        => Option.Create<string?>(null).IsSome.Should().BeFalse();

    [Fact]
    public void Create_NullableString_Empty_IsSome_IsTrue()
        => Option.Create<string?>(string.Empty).IsSome.Should().BeTrue();

    [Fact]
    public void Create_String_NonEmpty_IsSome_IsTrue()
        => Option.Create<string>("a").IsSome.Should().BeTrue();

    [Fact]
    public void Create_NullableString_NonEmpty_IsSome_IsTrue()
        => Option.Create<string?>("a").IsSome.Should().BeTrue();




    [Fact]
    public void Create_String_NonEmpty_Value_IsValue()
        => Option.Create<string>("a").Value.Should().Be("a");

    [Fact]
    public void Create_NullableString_NonEmpty_Value_IsValue()
        => Option.Create<string?>("a").Value.Should().Be("a");

    [Fact]
    public void Create_NullableString_Empty_Value_IsValue()
        => Option.Create<string?>(string.Empty).Value.Should().Be(string.Empty);

    [Fact]
    public void Create_NullableString_Null_Value_IsValue()
        => Option.Create<string?>(null).Value.Should().Be(null);
}
