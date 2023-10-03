using FluentAssertions;
using LanguageExtensions.Monads;

namespace LanguageExtensions.Tests.Monads;

public class Option_ReferenceTypes_Tests
{
    private record Person(string Name, int Age);


    [Fact]
    public void None_Person_IsSome_IsFalse()
        => Option<Person>.None.IsSome.Should().BeFalse();

    [Fact]
    public void None_NullablePerson_IsSome_IsFalse()
        => Option<Person?>.None.IsSome.Should().BeFalse();

    [Fact]
    public void None_Person_Value_IsNull()
        => Option<Person>.None.Value.Should().BeNull();

    [Fact]
    public void None_NullablePerson_Value_IsNull()
        => Option<Person?>.None.Value.Should().BeNull();



    [Fact]
    public void Create_PersonNull_IsSome_IsFalse()
        => Option<Person>.Create(null).IsSome.Should().BeFalse();


    [Fact]
    public void Create_NullablePerson_Null_IsSome_IsFalse()
        => Option<Person?>.Create(null).IsSome.Should().BeFalse();

    [Fact]
    public void Create_Person_NonEmpty_IsSome_IsTrue()
        => Option<Person>.Create(new Person("a",1)).IsSome.Should().BeTrue();

    [Fact]
    public void Create_NullablePerson_NonEmpty_IsSome_IsTrue()
        => Option<Person?>.Create(new Person("a", 1)).IsSome.Should().BeTrue();




    [Fact]
    public void Create_Person_NonEmpty_Value_IsValue()
        => Option<Person>.Create(new Person("a", 1)).Value.Should().Be(new Person("a", 1));

    [Fact]
    public void Create_NullablePerson_NonEmpty_Value_IsValue()
        => Option<Person?>.Create(new Person("a", 1)).Value.Should().Be(new Person("a", 1));

    [Fact]
    public void Create_NullablePerson_Null_Value_IsValue()
        => Option<Person?>.Create(null).Value.Should().Be(null);
}
