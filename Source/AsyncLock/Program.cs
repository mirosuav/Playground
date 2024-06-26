
AsyncLocal<string> _asyncLocalString = new AsyncLocal<string>();

_asyncLocalString.Value = "Val1";

Console.WriteLine($"Starting tid: {Environment.CurrentManagedThreadId} asyncLoc: {_asyncLocalString.Value}");


await Task.Delay(10).ConfigureAwait(false);
await M1("M1_1");
await M1("M1_2");

async Task M1(string value)
{
    await File.AppendAllTextAsync("M1.txt", $"tid: {Environment.CurrentManagedThreadId}");

    _asyncLocalString.Value = value;

    Console.WriteLine($"M1 tid: {Environment.CurrentManagedThreadId} asyncLoc: {_asyncLocalString.Value}");

    await M2().ConfigureAwait(false);
}

async Task M2()
{
    await File.AppendAllTextAsync("M2.txt", $"tid: {Environment.CurrentManagedThreadId}");

    Console.WriteLine($"M2 tid: {Environment.CurrentManagedThreadId} asyncLoc: {_asyncLocalString.Value}");

    await M3().ConfigureAwait(false);
}
async Task M3()
{
    await File.AppendAllTextAsync("M3.txt", $"tid: {Environment.CurrentManagedThreadId}").ConfigureAwait(false);

    Console.WriteLine($"M3 tid: {Environment.CurrentManagedThreadId} asyncLoc: {_asyncLocalString.Value}");

}