namespace ModuleOne;

public interface IQuery1
{
    int A { get; init; }
    int B { get; init; }
}
public interface ICommand1
{
    int A { get; init; }
    int B { get; init; }
}

public interface IResponse1
{
    int I { get; init; }
    int C { get; init; }
}

public record Query1(int A, int B) : IQuery1;
public record Command1(int A, int B) : IQuery1;
public record Response1(int I, int C) : IResponse1;