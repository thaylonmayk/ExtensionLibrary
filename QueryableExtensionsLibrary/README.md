# 🔍 TL.QueryableExtensionsLibrary

[![NuGet](https://img.shields.io/nuget/v/TL.QueryableExtensionsLibrary.svg?style=flat-square&label=TL.QueryableExtensionsLibrary)](https://www.nuget.org/packages/TL.QueryableExtensionsLibrary/)
[![.NET](https://img.shields.io/badge/.NET-netstandard2.0%20%7C%20net8.0-blue.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](../LICENSE.txt)
> **Dynamic IQueryable extensions for EF Core: keyset pagination, string-based filtering, safe sorting, and conditional queries for clean APIs.**  
> *Extensões dinâmicas para IQueryable no EF Core: paginação keyset, filtros por string, ordenação segura e consultas condicionais para APIs limpas.*

O **`TL.QueryableExtensionsLibrary`** fornece extensões para `IQueryable<T>` voltadas para construção dinâmica de consultas LINQ através de Expression Trees, viabilizando filtros, ordenações, paginação e agregações por nome de propriedade em tempo de execução.

---

## 📦 Instalação

Adicione o pacote ao seu projeto através do .NET CLI:

```bash
dotnet add package TL.QueryableExtensionsLibrary
```

---

## 🚀 Funcionalidades Principais

| Método | Retorno | Descrição |
| :--- | :---: | :--- |
| `Filter(property, value)` | `IQueryable<T>` | Filtra dinamicamente por igualdade, com conversão automática para tipos primitivos, `Guid`, `DateTime`, `Enum` e tipos anuláveis (`Nullable<T>`). |
| `Filter(property, comparison, value)` | `IQueryable<T>` | Filtra dinamicamente com operadores relacionais e textuais (`=`, `!=`, `>`, `<`, `>=`, `<=`, `Contains`, `StartsWith`, `EndsWith`), suportando `Guid`, datas e nullables. |
| `Order(property, ascending)` | `IQueryable<T>` | Ordena dinamicamente (`OrderBy` ou `OrderByDescending`) pelo nome da propriedade informada. |
| `Page(index, size)` | `IQueryable<T>` | Realiza paginação baseada em índice inicial 1 (`Skip((index-1)*size).Take(size)`). |
| `ToKeysetPagedList(keySelector, pageSize)` | `KeysetPagedList<T, TKey>` | Paginação Keyset/Seek $O(1)$ otimizada para bancos de dados sem cláusulas `OFFSET/SKIP`. |
| `ToKeysetPagedList(keySelector, cursor, pageSize, direction)` | `KeysetPagedList<T, TKey>` | Navegação Keyset $O(1)$ dinâmica com cursor (`> cursor` ou `< cursor`) e direção (`Forward` / `Backward`). |
| `GroupBy<T, TKey>(property)` | `IQueryable<IGrouping<TKey, T>>` | Agrupa os elementos dinamicamente pela propriedade especificada. |
| `DistinctBy<T, TKey>(property)` | `IQueryable<T>` | Remove duplicatas mantendo o primeiro elemento para cada chave agrupada. |
| `Sum(property)` | `decimal` | Calcula o somatório dinâmico da propriedade numérica especificada. |
| `Min(property)` | `decimal` | Obtém o valor mínimo da propriedade numérica especificada. |
| `Max(property)` | `decimal` | Obtém o valor máximo da propriedade numérica especificada. |
| `Average(property)` | `decimal` | Calcula a média dos valores da propriedade numérica especificada. |
| `Count(predicate)` | `int` | Retorna a quantidade de elementos que satisfazem o predicado informado. |
| `Any(predicate)` | `bool` | Determina se qualquer elemento satisfaz a condição especificada. |

---

## 💡 Exemplos de Uso

```csharp
using System.Linq;
using QueryableExtensionsLibrary;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Categoria { get; set; } = string.Empty;
}

public class ConsultaService
{
    public IQueryable<Produto> FiltrarEOrdenar(IQueryable<Produto> produtos, string campoOrdenacao, bool ascendente, int pagina, int tamanho)
    {
        return produtos
            .Filter("Categoria", "Eletronicos")
            .Order(campoOrdenacao, ascendente)
            .Page(pagina, tamanho);
    }
}
```

---

## 🏛️ Decisões Arquiteturais

Para detalhes sobre a construção das árvores de expressão e considerações de desempenho, consulte:
- 📄 [ADR-010: Decisões Arquiteturais do TL.QueryableExtensionsLibrary](../docs/adr/ADR-010-queryable-extensions-library.md)

---

## 📄 Licença

Distribuído sob a licença [MIT](../LICENSE.txt).