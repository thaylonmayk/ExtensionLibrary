# UtilExtensions

Bem-vindo ao UtilExtensions! Esta biblioteca fornece uma variedade de métodos de extensão para `Int32`, `double`, `decimal`, `strings` e `enum`, facilitando a manipulação e cálculos comuns em suas aplicações .NET. Com UtilExtensions, você pode manipular strings, realizar cálculos numéricos e trabalhar com enums de maneira eficiente e eficaz.

- **[StringExtensionsLibrary](./StringExtensionsLibrary/README.md)**: Métodos de extensão para strings.
- **[EnumExtensionsLibrary](./EnumExtensionsLibrary/README.md)**: Métodos de extensão para enums.
- **[NumericExtensionsLibrary](./NumericExtensionsLibrary/README.md)**: Métodos de extensão para tipos numéricos.
- **[AssemblyExtensionsLibrary](./AssemblyExtensionsLibrary/README.md)**: Métodos de extensão para a classe `Assembly`.
- **[ClaimsPrincipalExtensions](./ClaimsPrincipalExtensions/README.md)**: Métodos de extensão para `ClaimsPrincipal`.
- **[DateTimeExtensionsLibrary](./DateTimeExtensionsLibrary/README.md)**: Métodos de extensão para `DateTime`.
- **[ObjectExtensionsLibrary](./ObjectExtensionsLibrary/README.md)**: Métodos de extensão para objetos.
- **[QueryableExtensionsLibrary](./QueryableExtensionsLibrary/README.md)**: Métodos de extensão para `IQueryable`
- 
## Instalação

Para instalar a biblioteca UtilExtensions via NuGet, use o seguinte comando:

```sh
dotnet add package TL.StringExtensionsLibrary
dotnet add package TL.EnumExtensionsLibrary
dotnet add package TL.NumericExtensionsLibrary
dotnet add package TL.AssemblyExtensionsLibrary
dotnet add package TL.ClaimsPrincipalExtensions
dotnet add package TL.DateTimeExtensionsLibrary
dotnet add package TL.ObjectExtensionsLibrary
dotnet add package TL.QueryableExtensionsLibrary
```

## Exemplos de Uso

**NumericExtensionsLibrary** 
```
using NumericExtensionsLibrary;

class Program
{
    static void Main()
    {
        //Int32 Extensions
        int number = 150;
        Console.WriteLine($"IsPrime: {number.IsPrime()}");
        Console.WriteLine($"IsEven: {number.IsEven()}");
        Console.WriteLine($"Factorial: {number.Factorial()}");
        //Double Extensions
        double baseNumber = 150.75;
        double percent = 20.0;
        Console.WriteLine($"{percent}% de {baseNumber} é {baseNumber.Percentage(percent)}"

        //Decimal Extensions
        decimal numberDecimal = 150.75M;
        decimal percentDecimal= 20.0M;
        Console.WriteLine($"{percentDecimal}% de {numberDecimal} é {numberDecimal.Percentage(percent)}"
    }
}

```

**StringExtensionLibrary**
```
using StringExtensionLibrary;

class Program
{
    static void Main()
    {

      public string? Summary { get; set; }
      public string? BooleanEquivalent { get; set; }

       BooleanEquivalent = "false";
       var teste1 = BooleanEquivalent.ToBoolean();
       Console.WriteLine(teste1);
       BooleanEquivalent = "true";
       teste1 = BooleanEquivalent.ToBoolean();
       Console.WriteLine(teste1);
       BooleanEquivalent = "yes";
       teste1 = BooleanEquivalent.ToBoolean();
       Console.WriteLine(teste1);
       BooleanEquivalent = "no";
       teste1 = BooleanEquivalent.ToBoolean();
       Console.WriteLine(teste1);
       BooleanEquivalent = "y";
       teste1 = BooleanEquivalent.ToBoolean(); 
       Console.WriteLine(teste1);
       BooleanEquivalent = "n";
       teste1 = BooleanEquivalent.ToBoolean(); 
       Console.WriteLine(teste1);



        BooleanEquivalent = DateTime.UtcNow.Date.ToString();
        teste1 = BooleanEquivalent.IsDateTime("dd/MM/yyyy");
        Console.WriteLine(teste1);


        teste1 = Summary.IsNullOrEmpty();
        Console.WriteLine(teste1);
    }
}

```

**EnumExtensionsLibrary**
```
using EnumExtensionsLibrary;
public enum Priority
{
    Urgent,
    High,
    Medium,
    Low
}
class Program
{
    static void Main()
    {
       var dictionary = Priority.Urgent.ToDictionary();
        Console.WriteLine("Dicionário do enum Priority:");
        foreach (var item in dictionary)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
        }
    }
}

```

## Contribuição
Sinta-se à vontade para contribuir com este projeto! Por favor, abra um pull request ou envie issues para sugestões e melhorias.

## Licença
Este projeto está licenciado sob a MIT License - veja o arquivo LICENSE para detalhes.