using System;
using System.Collections.Generic;
using Xunit;

namespace SparseSets.Tests;

public class SparseDictionaryTests
{
    [Fact]
    public void Indexer_Get_KeyOutOfRange()
    {
        var dictionary = new SparseDictionary<string>(10);

        var exception = Assert.Throws<IndexOutOfRangeException>(() => dictionary[15]);
        Assert.Equal("Key 15 is out of range 0..10", exception.Message);
    }

    [Fact]
    public void Indexer_Get_KeyNotFound()
    {
        var dictionary = new SparseDictionary<string>(10);

        var exception = Assert.Throws<KeyNotFoundException>(() => dictionary[5]);
        Assert.Equal("Key 5 is not found", exception.Message);
    }

    [Fact]
    public void Indexer_Set_KeyOutOfRanges()
    {
        var dictionary = new SparseDictionary<string>(10);

        var exception = Assert.Throws<IndexOutOfRangeException>(() => dictionary[20] = "test");
        Assert.Equal("Key 20 is out of range 0..10", exception.Message);        
    }

    [Theory]
    [InlineData(0, "zero")]
    [InlineData(5, "five")]
    [InlineData(9, "zero")]
    public void Indexer(int key, string value)
    {
        var dictionary = new SparseDictionary<string>(10);

        dictionary[key] = value;
        
        Assert.Equal(value, dictionary[key]);
    }

    [Theory]
    [InlineData(5, true)]
    [InlineData(15, false)]
    [InlineData(9, false)]
    public void ContainsKey(int key, bool expected)
    {
        var dictionary = new SparseDictionary<string>(10);
        dictionary[5] = "five";

        var actual = dictionary.ContainsKey(key);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Remove()
    {
        var dictionary = new SparseDictionary<string>(10);
        dictionary[6] = "six";
        dictionary[7] = "seven";
        dictionary[8] = "eight";
        
        dictionary.Remove(7);

        Assert.False(dictionary.ContainsKey(7));
        Assert.Equal(2, dictionary.Count);
    }

    [Theory]
    [InlineData(5, "five", true)]
    [InlineData(15, null, false)]
    [InlineData(9, null, false)]
    public void TryGetValue(int key, string? expectedValue, bool expectedResult)
    {
        var dictionary = new SparseDictionary<string>(10);
        dictionary[5] = "five";

        var actualResult = dictionary.TryGetValue(key, out var actualValue);

        Assert.Equal(expectedResult, actualResult);
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void Clear()
    {
        var dictionary = new SparseDictionary<string>(10);
        
        dictionary[5] = "five";
        dictionary[9] = "nine";
        Assert.Equal(2, dictionary.Count);

        dictionary.Clear();
        Assert.Equal(0, dictionary.Count);

        dictionary[4] = "four";
        dictionary[5] = "five";
        Assert.Equal(2, dictionary.Count);

        var result = dictionary.TryGetValue(9, out var value);
        Assert.False(result);
        Assert.Null(value);
    }
}
