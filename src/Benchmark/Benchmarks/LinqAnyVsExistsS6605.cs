using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmark.Benchmarks;

// See S6605

[SimpleJob(RuntimeMoniker.Net70, baseline: true)]
[MinColumn]
[MaxColumn]
public class LinqAnyVsExistsS6605
{
    private List<int> data = null!;
    private readonly Random random = new();

    [Params(1_000)] public int N { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        data = Enumerable.Range(0, N).Select(x => 43).ToList();
    }

    [Benchmark(Baseline = true)]
    public void Any()
    {
        for (var i = 0; i < N; i++)
        {
            _ = data.Any(x => x % 2 == 0); // Enumerable.Any
        }
    }

    [Benchmark]
    public void Exists()
    {
        for (var i = 0; i < N; i++)
        {
            _ = data.Exists(x => x % 2 == 0); // List<T>.Exists
        }
    }
}