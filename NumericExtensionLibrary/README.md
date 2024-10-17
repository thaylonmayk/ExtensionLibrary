# NumericExtensionsLibrary

Uma biblioteca .NET para fornecer métodos de extensão úteis para `int`, `double` e `decimal`.

## Funcionalidades

### Extensões para `int`

- `IsPrime()`: Verifica se um número é primo.
- `IsEven()`: Verifica se um número é par.
- `IsOdd()`: Verifica se um número é ímpar.
- `Factorial()`: Calcula o fatorial de um número.
- `IsPerfectSquare()`: Verifica se um número é um quadrado perfeito.
- `ToBinaryString()`: Converte um número para uma string binária.
- `ToHexString()`: Converte um número para uma string hexadecimal.
- `IsMultipleOf(int divisor)`: Verifica se um número é múltiplo de outro número.
- `DigitSum()`: Calcula a soma dos dígitos de um número.
- `ReverseDigits()`: Inverte os dígitos de um número.
- `GreatestCommonDivisor(int b)`: Calcula o maior divisor comum entre dois números.
- `GreatestCommonMultiple(int b)`: Calcula o mínimo múltiplo comum entre dois números.
- `Subtract(int subtrahend)`: Subtrai um número de outro.

### Extensões para `double`

- `ToRadians()`: Converte graus para radianos.
- `ToDegrees()`: Converte radianos para graus.
- `IsEven()`: Verifica se um número é par.
- `IsOdd()`: Verifica se um número é ímpar.

### Extensões para `decimal`

- `Percentage(decimal percentage)`: Calcula a porcentagem de um número.
- `WeightedAverage(List<decimal> numbers, List<decimal> weights)`: Calcula a média ponderada de uma lista de números com seus pesos correspondentes.
- `ToRadians()`: Converte graus para radianos.
- `ToDegrees()`: Converte radianos para graus.
- `DigitSum()`: Calcula a soma dos dígitos de um número.
- `ReverseDigits()`: Inverte os dígitos de um número.
- `Subtract(decimal subtrahend)`: Subtrai um número de outro.

## Instalação

Você pode instalar a biblioteca via NuGet. No seu terminal, execute:

```bash
dotnet add package NumericExtensionsLibrary
