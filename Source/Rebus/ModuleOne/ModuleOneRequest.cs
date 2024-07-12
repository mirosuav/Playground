namespace ModuleOne;

public interface IModuleOneRequest
{
    int A { get; init; }
    int B { get; init; }
}

public record ModuleOneRequest(int A, int B) : IModuleOneRequest;