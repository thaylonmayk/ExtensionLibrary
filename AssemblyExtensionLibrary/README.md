# AssemblyExtensions Library

A biblioteca AssemblyExtensions fornece uma variedade de métodos de extensão para a classe `Assembly` do .NET, facilitando a manipulação e extração de informações úteis sobre assemblies. Com AssemblyExtensions, você pode obter facilmente informações de versão, tipos definidos, assemblies referenciados, e muito mais.

## Funcionalidades

- **FileInfo**: Obtém o objeto `FileInfo` para a localização do assembly especificado.
- **GetVersion**: Obtém a versão do assembly.
- **GetReferencedAssemblies**: Obtém as assemblies referenciadas pelo assembly atual.
- **GetTypes**: Obtém todos os tipos definidos no assembly.
- **GetCustomAttribute**: Obtém um atributo personalizado específico do assembly.
- **GetEntryAssembly**: Obtém a assembly de entrada (o executável do processo) na aplicação atual.

## Instalação

Para instalar a biblioteca AssemblyExtensions via NuGet, use o seguinte comando:

```sh
dotnet add package TL.AssemblyExtensionsLibrary
```

## Exemplos de Uso

```sh
using System;
using System.Reflection;
using AssemblyExtensionsLibrary;

class Program
{
    static void Main()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        var fileInfo = assembly.FileInfo();
        Console.WriteLine($"Assembly location: {fileInfo.FullName}");
        var version = assembly.GetVersion();
        Console.WriteLine($"Assembly version: {version}"
    }
}

```
