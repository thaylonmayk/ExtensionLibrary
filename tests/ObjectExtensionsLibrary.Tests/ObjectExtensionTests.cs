using System;
using System.Diagnostics.CodeAnalysis;
using ObjectExtensionsLibrary;
using Xunit;

namespace ObjectExtensionsLibrary.Tests;

[SuppressMessage("Major Code Smell", "S2325:Methods should be static", Justification = "Métodos de instância intencionais para validar chamadas de reflexão via InvokeMethod.")]
public class SampleService
{
    public string Echo(string message) => message;
    public void ThrowError() => throw new ArgumentException("Erro de teste disparado.");
}

public class ObjectExtensionTests
{
    [Fact]
    public void InvokeMethod_ShouldExecuteExistingMethod()
    {
        var service = new SampleService();
        var result = service.InvokeMethod("Echo", "teste");

        Assert.Equal("teste", result);
    }

    [Fact]
    public void InvokeMethod_ShouldPreserveOriginalException()
    {
        var service = new SampleService();

        var ex = Assert.Throws<ArgumentException>(() => service.InvokeMethod("ThrowError"));
        Assert.Equal("Erro de teste disparado.", ex.Message);
    }

    [Fact]
    public void Clone_ShouldCreateIndependentDeepCopy()
    {
        var original = new Person { Name = "Alice", Age = 30 };
        var cloned = original.Clone();

        Assert.NotNull(cloned);
        Assert.NotSame(original, cloned);
        Assert.Equal(original.Name, cloned.Name);
        Assert.Equal(original.Age, cloned.Age);

        cloned.Name = "Bob";
        Assert.Equal("Alice", original.Name);
    }

    [Fact]
    public void Dictionary_ShouldConvertProperties()
    {
        var person = new Person { Name = "Alice", Age = 30 };
        var dict = person.Dictionary();

        Assert.NotNull(dict);
        Assert.Equal("Alice", dict["Name"]);
        Assert.Equal(30, dict["Age"]);
    }

    [Fact]
    public void Dictionary_WhenNull_ShouldReturnNull()
    {
        Person? nullPerson = null;
        var dict = nullPerson!.Dictionary();
        Assert.Null(dict);
    }

    [Fact]
    public void SetProperty_ShouldUpdateValue()
    {
        var person = new Person { Name = "Alice", Age = 30 };
        person.SetProperty("Name", "Charlie");

        Assert.Equal("Charlie", person.Name);
    }

    [Fact]
    public void ToDictionary_ShouldMapPropertiesIdiomatically()
    {
        var person = new Person { Name = "Alice", Age = 30 };
        var dict = person.ToDictionary();

        Assert.NotNull(dict);
        Assert.Equal("Alice", dict["Name"]);
        Assert.Equal(30, dict["Age"]);
    }

    [Fact]
    public void Clone_WithCircularReference_ShouldNotThrowException()
    {
        var nodeA = new NodeWithCycle { Name = "Node A" };
        var nodeB = new NodeWithCycle { Name = "Node B", Next = nodeA };
        nodeA.Next = nodeB;

        var cloned = nodeA.Clone();
        Assert.NotNull(cloned);
        Assert.Equal("Node A", cloned.Name);
        Assert.NotNull(cloned.Next);
        Assert.Equal("Node B", cloned.Next.Name);
    }
}

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
}

public class NodeWithCycle
{
    public string Name { get; set; } = string.Empty;
    public NodeWithCycle? Next { get; set; }
}