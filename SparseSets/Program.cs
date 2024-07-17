using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;


namespace SparseSets;

public class Result
{
    public required List<Item> Items { get; set; }
}

public class Item
{
    public int Id { get; set; }
    public double Value { get; set; }
}

[MemoryDiagnoser]
public class Program
{
    private const int Capacity = 1_000;
    private List<Result> _results = null!;

    [Params(1_000, 10_000, 100_000)]
    public int Results { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(42);

        _results = new List<Result>(Results);
        for (var i = 0; i < Results; i++)
        {
            var items = new List<Item>(Capacity);
            for (var j = 0; j < Capacity; j++)
            {
                items.Add(new Item
                {
                    Id = random.Next(0, Capacity),
                    Value = random.NextDouble(),
                });
            }
            _results.Add(new Result { Items = items });
        }
    }

    [Benchmark]
    public void InnerDictionary()
    {
        foreach (var result in _results)
        {
            var cache = new Dictionary<int, double>(Capacity);
            foreach (var item in result.Items)
            {
                if (cache.ContainsKey(item.Id)) continue;
                cache[item.Id] = item.Value;
            }
        }
    }

    [Benchmark]
    public void OuterDictionary()
    {
        var cache = new Dictionary<int, double>(Capacity);
        foreach (var result in _results)
        {
            foreach (var item in result.Items)
            {
                if (cache.ContainsKey(item.Id)) continue;
                cache[item.Id] = item.Value;
            }
            cache.Clear();
        }
    }

    [Benchmark]
    public void SparseDictionary()
    {
        var cache = new SparseDictionary<double>(Capacity);
        foreach (var result in _results)
        {
            foreach (var item in result.Items)
            {
                if (cache.ContainsKey(item.Id)) continue;
                cache[item.Id] = item.Value;
            }
            cache.Clear();
        }
    }

    [Benchmark]
    public void GenerationDictionary()
    {
        var cache = new GenerationDictionary<double>(Capacity);
        foreach (var result in _results)
        {
            foreach (var item in result.Items)
            {
                if (cache.ContainsKey(item.Id)) continue;
                cache[item.Id] = item.Value;
            }
            cache.Clear();
        }
    }

    private static void Main()
    {
        BenchmarkRunner.Run<Program>();
    }
}