using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImmutableBenchmark;


[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
public class HashsetBenchmark
{
    private List<Entity>? ListData;
    private HashSet<Entity>? HashSetData;
   
    [GlobalSetup]
    public void Setup()
    {
        ListData = Enumerable.Range(1, 100_000).Select(i => new Entity { Id = i }).ToList();
        HashSetData = Enumerable.Range(1, 100_000).Select(i => new Entity { Id = i }).ToHashSet();
    }

    [Benchmark]
    public bool ListBench() => ListData!.Any(x => x.Id < 0);

    [Benchmark]
    public bool HashsetBench() => HashSetData!.Any(x => x.Id < 0);
}

/*
 
| Method       | Job      | Runtime  | Mean     | Error     | StdDev    | Median   | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------- |--------- |--------- |---------:|----------:|----------:|---------:|------:|--------:|----------:|------------:|
| ListBench    | .NET 6.0 | .NET 6.0 | 3.092 ms | 0.0953 ms | 0.2809 ms | 3.175 ms |  1.00 |    0.00 |      42 B |        1.00 |
| ListBench    | .NET 7.0 | .NET 7.0 | 2.395 ms | 0.1337 ms | 0.3942 ms | 2.277 ms |  0.78 |    0.16 |      42 B |        1.00 |
|              |          |          |          |           |           |          |       |         |           |             |
| HashsetBench | .NET 6.0 | .NET 6.0 | 2.444 ms | 0.0880 ms | 0.2595 ms | 2.482 ms |  1.00 |    0.00 |      41 B |        1.00 |
| HashsetBench | .NET 7.0 | .NET 7.0 | 2.540 ms | 0.1561 ms | 0.4603 ms | 2.496 ms |  1.05 |    0.21 |      42 B |        1.02 |



*/
