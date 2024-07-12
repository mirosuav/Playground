namespace ModuleOne;

public interface IModuleOneSimpleRequest
{
    int Arg { get; init; }
}

public record ModuleOneSimpleRequest(int Arg) : IModuleOneSimpleRequest;
