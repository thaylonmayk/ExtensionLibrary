# 📦 TL.CollectionExtensionsLibrary

[![NuGet](https://img.shields.io/nuget/v/TL.CollectionExtensionsLibrary.svg?style=flat-square&label=TL.CollectionExtensionsLibrary)](https://www.nuget.org/packages/TL.CollectionExtensionsLibrary/)
[![.NET](https://img.shields.io/badge/.NET-netstandard2.0%20%7C%20net8.0-blue.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](../LICENSE.txt)
> **Practical collection extensions for .NET: efficient batching (ChunkBy), in-memory keyset pagination, safe filtering, and uniform shuffling.**  
> *Extensões práticas para coleções no .NET: particionamento em lotes (ChunkBy), paginação keyset em memória, filtros seguros e embaralhamento uniforme.*

O **`TL.CollectionExtensionsLibrary`** oferece um conjunto robusto de extensões de alto desempenho para coleções no .NET (`IEnumerable<T>`, `IList<T>`, `Dictionary<TKey, TValue>`, `HashSet<T>`, `Queue<T>` e `Stack<T>`), simplificando particionamento (batching), buscas, ordenações e manipulações comuns com complexidade assintótica previsível.

---

## 📦 Instalação

Adicione o pacote ao seu projeto através do .NET CLI:

```bash
dotnet add package TL.CollectionExtensionsLibrary
```

---

## 🚀 Funcionalidades Principais

### `IEnumerable<T>`
| Método | Retorno | Descrição |
| :--- | :---: | :--- |
| `ChunkBy(chunkSize)` | `IEnumerable<List<T>>` | Divide a sequência em lotes com passagem linear única $O(N)$ em streaming. |
| `Shuffle()` | `IEnumerable<T>` | Embaralha os elementos com algoritmo Fisher-Yates e distribuição estatisticamente uniforme. |
| `WhereIf(condition, predicate)` | `IEnumerable<T>` | Aplica o filtro de forma condicional apenas se `condition` for verdadeira. |
| `DistinctBy(keySelector)` | `IEnumerable<T>` | Retorna elementos com base na unicidade da chave selecionada. |
| `ToKeysetPagedList(keySelector, pageSize)` | `KeysetPagedList<T, TKey>` | Paginação de alta performance Keyset/Seek $O(1)$ para a primeira página. |
| `ToKeysetPagedList(keySelector, cursor, pageSize, direction)` | `KeysetPagedList<T, TKey>` | Navegação Keyset/Seek $O(1)$ em streaming baseada em cursor e direção (`Forward` / `Backward`). |
| `IsNullOrEmpty()` | `bool` | Valida se a coleção é nula ou se não possui nenhum elemento. |

### `IList<T>`
| Método | Retorno | Descrição |
| :--- | :---: | :--- |
| `ChunkBy(chunkSize)` | `List<IList<T>>` | Divide a lista em lotes com indexação direta e complexidade linear estrita $O(N)$. |
| `AddRangeIfNotExists(items)` | `void` | Adiciona em lote apenas os itens inexistentes com verificação $O(1)$ por elemento. |
| `Replace(oldItem, newItem)` | `bool` | Substitui a primeira ocorrência do elemento com comparação segura contra nulos. |
| `SortBy(keySelector)` | `void` | Ordena a lista *in-place* a partir de um seletor de chave. |
| `Move(oldIndex, newIndex)` | `void` | Reposiciona um elemento dentro da lista a partir dos índices informados. |

### `Dictionary<TKey, TValue>` & Estruturas Especializadas
| Estrutura | Método | Descrição |
| :--- | :--- | :--- |
| `Dictionary` | `GetOrAdd(key, valueFactory)` | Retorna o valor existente ou calcula e armazena um novo valor caso ausente. |
| `Dictionary` | `AddRangeIfNotExists(pairs)` | Adiciona múltiplos pares chave/valor ignorando chaves já presentes. |
| `Queue<T>` | `EnqueueRange(items)` | Enfileira múltiplos itens sequencialmente em lote. |
| `Stack<T>` | `PushRange(items)` | Empilha múltiplos itens sequencialmente em lote. |

---

## 💡 Exemplos de Uso

```csharp
using System;
using System.Collections.Generic;
using CollectionExtensionsLibrary;

public class LoteProcessador
{
    public void ProcessarEmLotes(IEnumerable<string> itens)
    {
        // 1. Particionamento linear de alta performance em lotes de 100
        foreach (var lote in itens.ChunkBy(100))
        {
            EnviarParaFila(lote);
        }

        // 2. Filtro condicional fluente sem quebrar a cadeia LINQ
        bool apenasAtivos = true;
        var filtrados = itens.WhereIf(apenasAtivos, item => item.StartsWith("ATIVO_"));
    }

    private void EnviarParaFila(List<string> lote) => Console.WriteLine($"Lote de {lote.Count} enviado.");
}
```

---

## 🏛️ Decisões Arquiteturais

Para entender as garantias de complexidade assintótica $O(N)$ em particionamentos e otimização de alocação de memória, consulte:
- 📄 [ADR-007: Decisões Arquiteturais do TL.CollectionExtensionsLibrary](../docs/adr/ADR-007-collection-extensions-library.md)

---

## 📄 Licença

Distribuído sob a licença [MIT](../LICENSE.txt).
