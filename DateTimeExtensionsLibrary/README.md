# DateTimeExtensions Library

A biblioteca DateTimeExtensions fornece uma variedade de métodos de extensão para a classe `DateTime` do .NET, facilitando a manipulação e formatação de datas e horários. Com DateTimeExtensions, você pode calcular dias úteis, obter representações amigáveis de datas, dividir intervalos de tempo em partes menores, e muito mais.

## Funcionalidades

- **Chunks**: Divide o intervalo de datas em partes menores com base em um número de dias especificado.
- **AddBusinessDays**: Adiciona um número específico de dias úteis a uma data.
- **IsWeekend**: Verifica se uma data é um fim de semana.
- **StartOfWeek**: Obtém a data de início da semana.
- **EndOfWeek**: Obtém a data de final da semana.
- **StartOfMonth**: Obtém a data de início do mês.
- **EndOfMonth**: Obtém a data de final do mês.
- **IsLeapYear**: Verifica se o ano é bissexto.
- **DaysInMonth**: Obtém o número de dias em um mês específico.
- **Age**: Calcula a idade com base em uma data de nascimento.
- **BusinessDaysBetween**: Calcula o número de dias úteis entre duas datas.
- **IsBusinessDay**: Verifica se uma data é um dia útil.
- **NextBusinessDay**: Obtém o próximo dia útil.
- **DaysUntil**: Calcula o número de dias até uma data futura.
- **BusinessDaysUntil**: Calcula o número de dias úteis até uma data futura.
- **ToShortDateString**: Converte a data em uma string de data curta.
- **ToLongDateString**: Converte a data em uma string de data longa.
- **ToShortTimeString**: Converte a hora em uma string de hora curta.
- **ToLongTimeString**: Converte a hora em uma string de hora longa.
- **ToCustomFormat**: Converte a data/hora em uma string de formato personalizado.
- **ToISO8601**: Converte a data/hora em uma string no formato ISO 8601.
- **ToDayOfWeekString**: Converte a data no dia da semana correspondente em string.
- **ToMonthName**: Converte a data no nome do mês correspondente.
- **ToFriendlyDateString**: Converte a data em uma string amigável (ex: "hoje", "amanhã").
- **ToOrdinalDateString**: Converte a data em uma string com sufixos ordinais (ex: "1st", "2nd").

## Instalação

Para instalar a biblioteca DateTimeExtensions via NuGet, use o seguinte comando:

```sh
dotnet add package TL.DateTimeExtensionsLibrary
```

## Exemplos de Uso


```sh
using DateTimeExtensionsLibrary;

class Program
{
    static void Main()
    {
        DateTime startDate = new DateTime(2023, 4, 1);
        DateTime endDate = new DateTime(2023, 4, 20);
        int days = 5;

        var chunks = startDate.Chunks(endDate, days);
        foreach (var chunk in chunks)
        {
            Console.WriteLine($"Chunk: {chunk.Item1} - {chunk.Item2}");
        }

        DateTime today = DateTime.Today;
        int daysToAdd = 5;

        DateTime futureDate = today.AddBusinessDays(daysToAdd);
        Console.WriteLine($"Future date after adding {daysToAdd} business days: {futureDate}");

    }
}

```