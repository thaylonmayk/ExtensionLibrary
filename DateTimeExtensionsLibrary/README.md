# 📅 TL.DateTimeExtensionsLibrary

[![NuGet](https://img.shields.io/nuget/v/TL.DateTimeExtensionsLibrary.svg?style=flat-square&label=TL.DateTimeExtensionsLibrary)](https://www.nuget.org/packages/TL.DateTimeExtensionsLibrary/)
[![.NET](https://img.shields.io/badge/.NET-netstandard2.0%20%7C%20net8.0-blue.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](../LICENSE.txt)
> **Business-ready DateTime extensions for .NET: business day calculations, holiday evaluation, deadline handling, and fluent date arithmetic.**  
> *Extensões práticas de DateTime para .NET: cálculo de dias úteis, verificação de feriados, gestão de prazos e operações fluentes com datas.*

O **`TL.DateTimeExtensionsLibrary`** oferece uma suíte de utilitários de alta performance para `System.DateTime`, simplificando cálculos de dias úteis em tempo constante $O(1)$, fatiamento temporal de intervalos (`Chunks`), limites de calendário e formatações especializadas com total preservação do `DateTimeKind`.

---

## 📦 Instalação

Adicione o pacote ao seu projeto através do .NET CLI:

```bash
dotnet add package TL.DateTimeExtensionsLibrary
```

---

## 🚀 Funcionalidades Principais

### Dias Úteis, Feriados e Prazos
| Método | Retorno | Descrição |
| :--- | :---: | :--- |
| `AddBusinessDays(days)` | `DateTime` | Adiciona dias úteis ignorando finais de semana com integridade de timezone. |
| `BusinessDaysBetween(endDate)` | `int` | Calcula o total de dias úteis em tempo constante $O(1)$ sem loops iterativos. |
| `BusinessDaysBetween(endDate, holidayProvider)` | `int` | Calcula dias úteis desconsiderando fins de semana e feriados providos via `IHolidayProvider`. |
| `BusinessDaysBetween(endDate, holidays)` | `int` | Calcula dias úteis desconsiderando fins de semana e a coleção de datas de feriados informada. |
| `BusinessDaysBetween(endDate, isHoliday)` | `int` | Calcula dias úteis desconsiderando fins de semana e datas que atendem ao predicado informado. |
| `BusinessDaysUntil(futureDate, ...)` | `int` | Sobrecargas para cálculo de dias úteis até a data futura considerando feriados ou provedor customizado. |
| `IsBusinessDay()` | `bool` | Retorna se a data atual corresponde a um dia útil (segunda a sexta). |
| `IsWeekend()` | `bool` | Verifica se a data corresponde a sábado ou domingo. |
| `NextBusinessDay()` | `DateTime` | Obtém o próximo dia útil subsequente. |

### Calendário e Particionamento de Intervalos
| Método | Retorno | Descrição |
| :--- | :---: | :--- |
| `Chunks(endDate, days)` | `IEnumerable<Tuple<DateTime, DateTime>>` | Divide intervalos longos em lotes menores para consultas paginadas e jobs em lote. |
| `StartOfMonth()` / `EndOfMonth()` | `DateTime` | Retorna o início (`00:00:00.000`) ou fim (`23:59:59.999`) do mês. |
| `StartOfWeek()` / `EndOfWeek()` | `DateTime` | Retorna o primeiro ou último momento da semana. |
| `Age()` | `int` | Calcula a idade exata com base na data atual ou especificada. |
| `DaysUntil(target)` | `int` | Retorna a contagem de dias corridos até a data alvo. |

### Formatações Especializadas
| Método | Retorno | Descrição |
| :--- | :---: | :--- |
| `ToFriendlyDateString()` | `string` | Retorna texto humanizado (ex: "hoje", "ontem", "há 3 dias"). |
| `ToISO8601()` | `string` | Converte para a representação universal padrão ISO 8601. |
| `ToOrdinalDateString()` | `string` | Converte para texto ordinal (ex: "1st", "2nd", "3rd"). |

---

## 💡 Exemplos de Uso

```csharp
using System;
using DateTimeExtensionsLibrary;

public class ServicoPrazos
{
    public void ExecutarExemplos()
    {
        DateTime hoje = DateTime.UtcNow;

        // 1. Cálculo de Prazos e Vencimentos Úteis
        DateTime vencimento = hoje.AddBusinessDays(10);
        int diasUteisRestantes = hoje.BusinessDaysBetween(vencimento);

        // 2. Fatiamento de Consultas Massivas por Lotes de 7 dias
        DateTime inicioAno = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime fimAno = new DateTime(2026, 12, 31, 0, 0, 0, DateTimeKind.Utc);

        foreach (var (inicio, fim) in inicioAno.Chunks(fimAno, 7))
        {
            Console.WriteLine($"Consultando período: {inicio:yyyy-MM-dd} até {fim:yyyy-MM-dd}");
        }
    }
}
```

---

## 🏛️ Decisões Arquiteturais

Para detalhes sobre a complexidade algorítmica $O(1)$ de dias úteis e preservação de fuso horário, consulte:
- 📄 [ADR-006: Decisões Arquiteturais do TL.DateTimeExtensionsLibrary](../docs/adr/ADR-006-date-time-extensions-library.md)

---

## 📄 Licença

Distribuído sob a licença [MIT](../LICENSE.txt).