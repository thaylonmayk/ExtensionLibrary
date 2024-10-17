# StringExtensions

In English:

The StringExtensions library offers a robust set of extension methods for strings, surpassing the typical validation methods and enhancing the .NET System.String class. With StringExtensions, you can effortlessly manipulate strings, check their types, count occurrences of specific values within strings, and perform various other advanced operations, ensuring a more efficient and effective development process.

Em Português:

A biblioteca StringExtensions oferece um conjunto robusto de métodos de extensão para strings, que superam os métodos comuns de validação e aprimoram a classe System.String do .NET. Com o StringExtensions, você pode manipular strings sem esforço, verificar seus tipos, contar ocorrências de valores específicos dentro das strings e realizar várias outras operações avançadas, garantindo um processo de desenvolvimento mais eficiente e eficaz.

## Instalação

Você pode instalar a biblioteca via NuGet. No seu terminal, execute:

```bash
dotnet add package TL.StringExtensionsLibrary
```

## Usage

Once you have installed the String extension library within your project. String extensions functions will be available on all enum types 

```
            string BooleanEquivalent = "false";
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