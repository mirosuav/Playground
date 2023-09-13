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

public class Entity
{
    public int Id { get; set; }
}

[SimpleJob(RuntimeMoniker.Net60, baseline: true)]
[SimpleJob(RuntimeMoniker.Net70)]
[MemoryDiagnoser]
public class ImmutableListBenchmark
{
    private List<Entity>? ListData;
    private ImmutableList<Entity>? ImmutableListData;


    [GlobalSetup]
    public void Setup()
    {
        ListData = Enumerable.Range(1, 100_000).Select(i => new Entity { Id = i }).ToList();
        ImmutableListData = Enumerable.Range(1, 100_000).Select(i => new Entity { Id = i }).ToImmutableList();
    }

    [Benchmark]
    public bool ListBench() => ListData!.Any(x => x.Id < 0);

    [Benchmark]
    public bool ImmutableListBench() => ImmutableListData!.Any(x => x.Id < 0);
}

/*
 

| Method             | Job      | Runtime  | N      | Mean         | Error      | StdDev       | Median       | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |--------- |--------- |------- |-------------:|-----------:|-------------:|-------------:|------:|--------:|----------:|------------:|
| ListBench          | .NET 6.0 | .NET 6.0 | 1000   |     28.88 us |   0.825 us |     2.432 us |     29.56 us |  1.00 |    0.00 |      40 B |        1.00 |
| ListBench          | .NET 7.0 | .NET 7.0 | 1000   |     23.56 us |   1.081 us |     3.188 us |     23.54 us |  0.82 |    0.14 |      40 B |        1.00 |
|                    |          |          |        |              |            |              |              |       |         |           |             |
| ImmutableListBench | .NET 6.0 | .NET 6.0 | 1000   |    131.94 us |   6.720 us |    19.815 us |    129.05 us |  1.00 |    0.00 |      72 B |        1.00 |
| ImmutableListBench | .NET 7.0 | .NET 7.0 | 1000   |    122.77 us |   5.837 us |    17.211 us |    121.72 us |  0.95 |    0.21 |      72 B |        1.00 |
|                    |          |          |        |              |            |              |              |       |         |           |             |
| ListBench          | .NET 6.0 | .NET 6.0 | 100000 |  2,492.93 us | 133.211 us |   388.582 us |  2,502.30 us |  1.00 |    0.00 |      42 B |        1.00 |
| ListBench          | .NET 7.0 | .NET 7.0 | 100000 |  2,385.45 us | 123.632 us |   364.533 us |  2,331.97 us |  0.98 |    0.19 |      42 B |        1.00 |
|                    |          |          |        |              |            |              |              |       |         |           |             |
| ImmutableListBench | .NET 6.0 | .NET 6.0 | 100000 | 15,355.86 us | 626.190 us | 1,846.336 us | 15,402.49 us |  1.00 |    0.00 |      83 B |        1.00 |
| ImmutableListBench | .NET 7.0 | .NET 7.0 | 100000 | 14,259.87 us | 557.927 us | 1,645.060 us | 14,287.87 us |  0.94 |    0.15 |      91 B |        1.10 |



| Method             | Job      | Runtime  | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Allocated | Alloc Ratio |
|------------------- |--------- |--------- |----------:|----------:|----------:|----------:|------:|--------:|----------:|------------:|
| ListBench          | .NET 6.0 | .NET 6.0 |  3.147 ms | 0.1131 ms | 0.3336 ms |  3.250 ms |  1.00 |    0.00 |      41 B |        1.00 |
| ListBench          | .NET 7.0 | .NET 7.0 |  2.093 ms | 0.1002 ms | 0.2939 ms |  2.129 ms |  0.68 |    0.14 |      42 B |        1.02 |
|                    |          |          |           |           |           |           |       |         |
 |             |
| ImmutableListBench | .NET 6.0 | .NET 6.0 | 14.172 ms | 0.4034 ms | 1.1703 ms | 13.958 ms |  1.00 |    0.00 |      82 B |        1.00 |
| ImmutableListBench | .NET 7.0 | .NET 7.0 | 13.238 ms | 0.4784 ms | 1.4032 ms | 13.046 ms |  0.94 |    0.13 |      81 B |        0.99 |





*/
