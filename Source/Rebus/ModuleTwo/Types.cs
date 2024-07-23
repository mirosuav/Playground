namespace ModuleTwo;

public interface IQuery2
{
    int A { get; init; }
    int B { get; init; }
}
public interface ICommand2
{
    int A { get; init; }
    int B { get; init; }
}

public interface IResponse2
{
    int I { get; init; }
    int C { get; init; }
}

public record Query2(int A, int B) : IQuery2;
public record Command2(int A, int B) : IQuery2;
public record Response2(int I, int C) : IResponse2;