﻿using FluentAssertions;
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
        => Option.None<ResultState>().Value.Should().Be(ResultState.None);

    [Fact]
    public void None_NullableEnum_Value_IsNull()
        => Option.None<ResultState?>().Value.Should().BeNull();




    [Fact]
    public void Create_EnumZero_IsSome_IsTrue()
        => Option.Create(ResultState.None).IsSome.Should().BeTrue();

    [Fact]
    public void Create_NullableEnumNull_IsSome_IsFalse()
        => Option.Create<ResultState?>(null).IsSome.Should().BeFalse();

    [Fact]
    public void Create_EnumNonZero_IsSome_IsTrue()
        => Option.Create<ResultState>(ResultState.Success).IsSome.Should().BeTrue();

    [Fact]
    public void Create_NullableEnumNonZero_IsSome_IsTrue()
        => Option.Create<ResultState?>(ResultState.Success).IsSome.Should().BeTrue();




    [Fact]
    public void Create_EnumNonZero_Value_IsValue()
        => Option.Create<ResultState>(ResultState.Success)
        .Value.Should().Be(ResultState.Success);

    [Fact]
    public void Create_NullableEnumNonZero_Value_IsValue()
        => Option.Create<ResultState?>(ResultState.Success)
        .Value.Should().Be(ResultState.Success);
}
