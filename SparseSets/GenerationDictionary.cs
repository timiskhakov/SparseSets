using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SparseSets;

internal class GenItem<T>
{
    public int Generation;
    public T? Value;
}

public class GenerationDictionary<T>
{
    private int _generation = 1;
    private int _count = 0;
    private readonly GenItem<T>[] _items;

    public GenerationDictionary(int size)
    {
        _items = new GenItem<T>[size];
        for (var i = 0; i < size; i++)
        {
            _items[i] = new GenItem<T>();
        }
    }

    public T this[int key]
    {
        get
        {
            if (key < 0 || key >= _items.Length) throw new IndexOutOfRangeException($"Key {key} is out of range 0..{_items.Length}");
            var item = _items[key];
            if (item.Generation != _generation) throw new KeyNotFoundException($"Key {key} is not found");
            return item.Value!;
        }
        set
        {
            if (key < 0 || key >= _items.Length) throw new IndexOutOfRangeException($"Key {key} is out of range 0..{_items.Length}");
            _items[key].Generation = _generation;
            _items[key].Value = value;
            _count++;
        }
    }

    public int Capacity => _items.Length;

    public int Count => _count;

    public void Clear()
    {
        _generation++;
        _count = 0;
    }

    public bool ContainsKey(int key)
    {
        if (key < 0 || key >= _items.Length) return false;
        return _items[key].Generation == _generation;
    }

    public bool TryGetValue(int key, [NotNullWhen(true)] out T? value)
    {
        if (key < 0 || key >= _items.Length)
        {
            value = default;
            return false;
        }

        var item = _items[key];
        if (item.Generation != _generation)
        {
            value = default;
            return false;
        }

        value = item.Value!;
        return true;
    }

    public void Remove(int key)
    {
        if (key < 0 || key >= _items.Length) return;
        var item = _items[key];
        if (item.Generation != _generation) return;
        _items[key].Generation = _generation - 1;
        _count--;
    }
}
