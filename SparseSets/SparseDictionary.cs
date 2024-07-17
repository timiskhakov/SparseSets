using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SparseSets;

public class Item<T>
{
    public int Key;
    public T? Value;
}

public class SparseDictionary<T>
{
    private readonly Item<T>[] _dense;
    private readonly int[] _sparse;
    private int _position;

    public SparseDictionary(int size)
    {
        _dense = new Item<T>[size];
        _sparse = new int[size];
        for (var i = 0; i < size; i++)
        {
            _dense[i] = new Item<T>();
        }
    }

    public T this[int key]
    {
        get
        {
            if (key < 0 || key >= _sparse.Length) throw new IndexOutOfRangeException($"Key {key} is out of range 0..{_sparse.Length}");
            if (!TryGetIndex(key, out var index)) throw new KeyNotFoundException($"Key {key} is not found");
            return _dense[index].Value!;
        }
        set
        {
            if (key < 0 || key >= _sparse.Length) throw new IndexOutOfRangeException($"Key {key} is out of range 0..{_sparse.Length}");
            if (TryGetIndex(key, out var index))
            {
                _dense[index].Value = value;
                return;
            }
            _dense[_position].Key = key;
            _dense[_position].Value = value;
            _sparse[key] = _position;
            _position++;
        }
    }

    public int Capacity => _sparse.Length;

    public int Count => _position;

    public void Clear()
    {
        _position = 0;
    }

    public bool ContainsKey(int key)
    {
        return key > 0 && key < _sparse.Length && TryGetIndex(key, out _);
    }

    public bool TryGetValue(int key, [NotNullWhen(true)] out T? value)
    {
        if (key < 0 || key >= _sparse.Length || !TryGetIndex(key, out var index))
        {
            value = default;
            return false;
        }

        value = _dense[index].Value!;
        return true;
    }

    public void Remove(int key)
    {
        if (!TryGetIndex(key, out var index)) return;
        var last = _dense[_position - 1];
        _dense[index] = last;
        _sparse[last.Key] = _sparse[key];
        _position--;
    }

    private bool TryGetIndex(int key, out int index)
    {
        index = _sparse[key];
        return index < _position && _dense[index].Key == key;
    }
}
