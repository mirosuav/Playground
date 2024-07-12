using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using ModuleOne;
using Rebus;
using Rebus.Bus;

namespace Runner;

[MemoryDiagnoser]
public class RebusBenchmarks
{
    private IBus _bus = null!;

    [GlobalSetup]
    public void Setup()
    {
        Program.ConfigureAllModules();
        _bus = Program.HostIoc.GetRequiredService<IBus>();
    }

    [Benchmark]
    public Task<int> SendRequest() => _bus.SendRequest<int>(new ModuleOneRequest(1, 3));

}

/*

Results ModuleOneSimpleRequest

Run 1.

   | Method      | Mean     | Error   | StdDev  | Allocated |
   |------------ |---------:|--------:|--------:|----------:|
   | SendRequest | 213.9 ms | 4.23 ms | 6.59 ms |  69.15 KB |


Run 2. With await Send

   | Method      | Mean     | Error   | StdDev  | Allocated |
   |------------ |---------:|--------:|--------:|----------:|
   | SendRequest | 213.5 ms | 4.25 ms | 7.87 ms |  69.66 KB |

Run 3. With await and ConfigureAwait(false)

   | Method      | Mean     | Error   | StdDev  | Allocated |
   |------------ |---------:|--------:|--------:|----------:|
   | SendRequest | 214.0 ms | 4.14 ms | 6.07 ms |  69.25 KB |

Run 4. With TPL enabled


   | Method      | Mean     | Error   | StdDev  | Allocated |
   |------------ |---------:|--------:|--------:|----------:|
   | SendRequest | 107.1 ms | 1.97 ms | 1.54 ms |  43.87 KB |

   | Method      | Mean     | Error   | StdDev  | Allocated |
   |------------ |---------:|--------:|--------:|----------:|
   | SendRequest | 108.6 ms | 0.86 ms | 0.80 ms |   94.5 KB |

Run 5. ProtoBuf

   | Method      | Mean     | Error   | StdDev   | Median   | Allocated |
   |------------ |---------:|--------:|---------:|---------:|----------:|
   | SendRequest | 212.2 ms | 4.64 ms | 13.25 ms | 216.6 ms | 100.67 KB |

Run 6. TPL and ProtoBuf

   | Method      | Mean     | Error   | StdDev  | Allocated |
   |------------ |---------:|--------:|--------:|----------:|
   | SendRequest | 108.5 ms | 0.36 ms | 0.34 ms |   95.6 KB |

Run 7. TPL and System.Text.Json

   | Method      | Mean     | Error   | StdDev  | Allocated |
   |------------ |---------:|--------:|--------:|----------:|
   | SendRequest | 108.4 ms | 0.63 ms | 0.56 ms |  94.63 KB |

Run 8. TPL ans NewtonsoftJson

   | Method      | Mean     | Error   | StdDev  | Allocated |
   |------------ |---------:|--------:|--------:|----------:|
   | SendRequest | 108.1 ms | 0.53 ms | 0.50 ms |  103.6 KB |



Results ModuleOneRequest

Run 1. 

   | Method      | Mean     | Error    | StdDev   | Median   | Allocated |
   |------------ |---------:|---------:|---------:|---------:|----------:|
   | SendRequest | 415.7 ms | 13.12 ms | 38.49 ms | 433.8 ms | 233.05 KB |


*/