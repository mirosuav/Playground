namespace ModuleTwo;

public interface IModuleTwoRequest
{
    int A { get; init; }
    int B { get; init; }
}

public record ModuleTwoRequest(int A, int B) : IModuleTwoRequest;