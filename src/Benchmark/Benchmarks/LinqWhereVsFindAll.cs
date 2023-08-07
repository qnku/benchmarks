using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Benchmark.Benchmarks;

[SimpleJob(RuntimeMoniker.Net70, baseline: true)]
[MinColumn]
[MaxColumn]
public class LinqWhereVsFindAll
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
    public IReadOnlyList<int> FindAll()
    {
        return _numbers.FindAll(x => x % 2 == 42);
    }

    [Benchmark]
    public IReadOnlyList<int> LinqWhere()
    {
        return _numbers.Where(x => x % 2 == 42).ToArray();
    }
}