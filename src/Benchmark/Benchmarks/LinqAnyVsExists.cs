using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmark.Benchmarks;

// See S6605

[SimpleJob(RuntimeMoniker.Net70, baseline: true)]
[MinColumn]
[MaxColumn]
public class LinqAnyVsExists
{
    private List<int> _numbers = null!;

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);
        _numbers = new List<int>();
        for (var i = 0; i < 50000; i++)
        {
            _numbers.Add(random.Next());
        }
    }

    [Benchmark(Baseline = true)]
    public bool Exists()
    {
        return _numbers.Exists(x => x % 2 == 42);
    }

    [Benchmark]
    public bool LinqAny()
    {
        return _numbers.Any(x => x % 2 == 42);
    }

    [Benchmark]
    public bool InlinedForeach()
    {
        // ReSharper disable once ForeachCanBeConvertedToQueryUsingAnotherGetEnumerator
        foreach (var num in _numbers)
        {
            if (num % 2 == 42)
            {
                return true;
            }
        }

        return false;
    }

    [Benchmark]
    public bool InlinedFor()
    {
        // ReSharper disable once ForCanBeConvertedToForeach
        // ReSharper disable once LoopCanBeConvertedToQuery
        for (var i = 0; i < _numbers.Count; i++)
        {
            if (_numbers[i] % 2 == 42)
            {
                return true;
            }
        }

        return false;
    }
}