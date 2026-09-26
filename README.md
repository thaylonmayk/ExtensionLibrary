<p align="center">
  <img src="assets/icon.svg" alt="TL Extensions Icon" width="128" height="128" />
</p>

# 🚀 TL.ExtensionLibrary

[![.NET](https://img.shields.io/badge/.NET-netstandard2.0%20%7C%20net8.0-blue.svg)](https://dotnet.microsoft.com/)
[![NuGet Profile](https://img.shields.io/badge/NuGet-ThaylonMALopes-004880.svg?logo=nuget)](https://www.nuget.org/profiles/ThaylonMALopes)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

Bem-vindo ao ecossistema **TL.ExtensionLibrary**! Esta suíte de bibliotecas em C# / .NET disponibiliza métodos de extensão utilitários de alta performance, projetados para simplificar o desenvolvimento diário, eliminar boilerplate, assegurar pureza funcional e manter seu código limpo e idiomático.

A biblioteca oferece duas formas de consumo:
1. **Pacotes Granulares:** Instale estritamente os módulos específicos sob o prefixo `TL.*ExtensionsLibrary`.
2. **Metapacotes da Clean Architecture:** Instale agregadores por camada com **zero overhead binário** e total garantia de isolamento arquitetural ([ADR-011](./docs/adr/ADR-011-metapacotes-agregadores-clean-architecture.md)).

---

## 🏛️ Metapacotes por Camadas da Clean Architecture

Para projetos corporativos que seguem **Clean Architecture** ou **Domain-Driven Design (DDD)**, utilize os metapacotes dedicados para acelerar o desenvolvimento e impedir vazamento de dependências técnicas:

```mermaid
graph TD
    subgraph DomainLayer ["Camada de Domínio (Pureza Total)"]
        Domain["📦 TL.ExtensionLibrary.Domain"]
        S["String, Numeric, DateTime, Enum, Collection"]
        Domain --> S
    end

    subgraph AppLayer ["Camada de Aplicação (Orquestração & DTOs)"]
        App["📦 TL.ExtensionLibrary.Application"]
        O["Object, ClaimsPrincipal"]
        App --> O
        App --> Domain
    end

    subgraph InfraLayer ["Camada de Infraestrutura (Tecnologias Externas)"]
        Infra["📦 TL.ExtensionLibrary.Infrastructure"]
        T["Queryable, HttpClient, Assembly"]
        Infra --> T
        Infra --> App
    end
```

| Metapacote NuGet | Camada Alvo | Módulos Incluídos | Instalação CLI |
| :--- | :--- | :--- | :--- |
| **`TL.ExtensionLibrary.Domain`** | Domínio DDD / Entidades | `String`, `Numeric`, `DateTime`, `Enum`, `Collection` | `dotnet add package TL.ExtensionLibrary.Domain` |
| **`TL.ExtensionLibrary.Application`** | Casos de Uso / DTOs / CQRS | `Object`, `ClaimsPrincipal` + Transitivo `Domain` | `dotnet add package TL.ExtensionLibrary.Application` |
| **`TL.ExtensionLibrary.Infrastructure`** | Acesso a Dados / APIs / Repositórios | `Queryable`, `HttpClient`, `Assembly` + Transitivo `Application` e `Domain` (Todos os 10 módulos) | `dotnet add package TL.ExtensionLibrary.Infrastructure` |

> 💡 **Nota Técnica:** Os metapacotes utilizam `<IncludeBuildOutput>false</IncludeBuildOutput>`, não incluindo arquivos `.dll` próprios. O download possui apenas ~18 KB e propaga as dependências transitivas nativamente via NuGet.

---

## 📦 Catálogo de Módulos NuGet & Documentação

| Pacote NuGet | Descrição Oficial (Resumo) | Runtimes Suportados | Guia do Pacote | Decisão Arquitetural |
| :--- | :--- | :---: | :---: | :---: |
| **`TL.StringExtensionsLibrary`** | Essential and ergonomic C# string utilities: safe truncation, span-friendly trimming, case-insensitive helpers, and clean parsing. | `netstandard2.0`<br/>`net8.0` | [README](./StringExtensionLibrary/README.md) | [ADR-001](./docs/adr/ADR-001-string-extensions-library.md) |
| **`TL.NumericExtensionsLibrary`** | Lightweight numeric helpers for C#: value clamping, percentage calculations, and fluent math utilities for domain models and business logic. | `netstandard2.0`<br/>`net8.0` | [README](./NumericExtensionLibrary/README.md) | [ADR-002](./docs/adr/ADR-002-numeric-extensions-library.md) |
| **`TL.EnumExtensionsLibrary`** | Cached and safe enum extensions for .NET: fast attribute description lookup, key-value mappings, and reliable string parsing. | `netstandard2.0`<br/>`net8.0` | [README](./EnumExtensionsLibrary/README.md) | [ADR-003](./docs/adr/ADR-003-enum-extensions-library.md) |
| **`TL.ClaimsPrincipalExtensionsLibrary`** | Strongly-typed ClaimsPrincipal extensions for ASP.NET Core: clean, safe access to user IDs, roles, emails, and authentication claims. | `netstandard2.0`<br/>`net8.0` | [README](./ClaimsPrincipalExtensionsLibrary/README.md) | [ADR-004](./docs/adr/ADR-004-claims-principal-extensions-library.md) |
| **`TL.AssemblyExtensionLibrary`** | Lightweight assembly scanning extensions for .NET: type discovery, attribute filtering, and metadata helpers for clean dependency injection. | `netstandard2.0`<br/>`net8.0` | [README](./AssemblyExtensionLibrary/README.md) | [ADR-005](./docs/adr/ADR-005-assembly-extension-library.md) |
| **`TL.DateTimeExtensionsLibrary`** | Business-ready DateTime extensions for .NET: business day calculations, holiday evaluation, deadline handling, and fluent date arithmetic. | `netstandard2.0`<br/>`net8.0` | [README](./DateTimeExtensionsLibrary/README.md) | [ADR-006](./docs/adr/ADR-006-date-time-extensions-library.md) |
| **`TL.CollectionExtensionsLibrary`** | Practical collection extensions for .NET: efficient batching (ChunkBy), in-memory keyset pagination, safe filtering, and uniform shuffling. | `netstandard2.0`<br/>`net8.0` | [README](./CollectionExtensionsLibrary/README.md) | [ADR-007](./docs/adr/ADR-007-collection-extensions-library.md) |
| **`TL.HttpClientExtensionsLibrary`** | Resilient HttpClient extensions for .NET: safe request handling, built-in retry policies, JSON helpers, and robust API communication. | `netstandard2.0`<br/>`net8.0` | [README](./HttpClientExtensionsLibrary/README.md) | [ADR-008](./docs/adr/ADR-008-http-client-extensions-library.md) |
| **`TL.ObjectExtensionsLibrary`** | Handy C# object extensions: pre-configured JSON serialization, dictionary/Expando conversion, deep cloning, and cached property access. | `netstandard2.0`<br/>`net8.0` | [README](./ObjectExtensionsLibrary/README.md) | [ADR-009](./docs/adr/ADR-009-object-extensions-library.md) |
| **`TL.QueryableExtensionsLibrary`** | Dynamic IQueryable extensions for EF Core: keyset pagination, string-based filtering, safe sorting, and conditional queries for clean APIs. | `netstandard2.0`<br/>`net8.0` | [README](./QueryableExtensionsLibrary/README.md) | [ADR-010](./docs/adr/ADR-010-queryable-extensions-library.md) |

---

## ⚡ Instalação Rápida via CLI

Escolha os módulos desejados e instale via .NET CLI:

```bash
# Manipulação de Strings
dotnet add package TL.StringExtensionsLibrary

# Cálculos Numéricos e Matemáticos
dotnet add package TL.NumericExtensionsLibrary

# Utilitários de Enum e Atributos
dotnet add package TL.EnumExtensionsLibrary

# Autenticação e ClaimsPrincipal
dotnet add package TL.ClaimsPrincipalExtensionsLibrary

# Diagnóstico de Assemblies e Reflexão
dotnet add package TL.AssemblyExtensionLibrary

# Operações de Data e Calendário
dotnet add package TL.DateTimeExtensionsLibrary

# Coleções, Particionamento e LINQ
dotnet add package TL.CollectionExtensionsLibrary

# Cliente HTTP Resiliente e Tokens
dotnet add package TL.HttpClientExtensionsLibrary

# Clonagem e Utilitários Genéricos de Objetos
dotnet add package TL.ObjectExtensionsLibrary

# Filtros e Ordenação Dinâmica em IQueryable
dotnet add package TL.QueryableExtensionsLibrary
```

---

## 💻 Exemplos Práticos de Uso

### 1. `NumericExtensionsLibrary`
```csharp
using NumericExtensionLibrary;

int numero = 17;
Console.WriteLine($"É primo? {numero.IsPrime()}"); // True
Console.WriteLine($"É par? {numero.IsEven()}");    // False
Console.WriteLine($"Fatorial de 5: {5.Factorial()}"); // 120

decimal valor = 250.00m;
decimal taxa = valor.Percentage(15.0m); // Calcula 15% de 250 -> 37.50
```

### 2. `StringExtensionLibrary`
```csharp
using StringExtensionLibrary;

string entrada = "true";
bool ativo = entrada.ToBoolean(); // true

string textoLongo = "Este é um texto corporativo de auditoria de segurança.";
string resumo = textoLongo.TruncateWithEllipsis(25); // "Este é um texto corpor..."

string dataString = "2026-09-03";
bool ehDataValida = dataString.IsDateTime("yyyy-MM-dd"); // true
```

### 3. `EnumExtensionsLibrary`
```csharp
using EnumExtensionsLibrary;

public enum Prioridade
{
    [System.ComponentModel.Description("Alta Urgência")]
    Urgente = 1,
    [System.ComponentModel.Description("Normal")]
    Padrao = 2
}

var descricao = Prioridade.Urgente.GetDescription(); // "Alta Urgência"
Dictionary<int, string> catalogo = Prioridade.Urgente.ToDictionary();
```

### 4. `CollectionExtensionsLibrary`
```csharp
using CollectionExtensionsLibrary;

var lista = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

// Particionamento em lotes (batching)
var lotes = lista.ChunkBy(3); // [1,2,3], [4,5,6], [7,8,9], [10]

// Embaralhamento seguro
var embaralhado = lista.Shuffle();
```

### 5. `QueryableExtensionsLibrary`
```csharp
using QueryableExtensionsLibrary;

IQueryable<Produto> produtos = dbContext.Produtos.AsQueryable();

// Filtro e ordenação dinâmicos a partir de strings da requisição
var resultado = produtos
    .Filter("Nome", "contains", "Notebook")
    .Order("Preco", ascending: false)
    .Page(index: 1, size: 20);
```

---

## 🔬 Suíte Oficial de Micro-benchmarks Científicos

A solução conta com um projeto executável oficial ([`ExtensionLibrary.Benchmarks`](./benchmarks/ExtensionLibrary.Benchmarks/README.md)) baseado em **BenchmarkDotNet v0.14.0**, medindo cientificamente 26 cenários em 8 suítes com diagnósticos de memória (`[MemoryDiagnoser]`):

```bash
# Executar todos os micro-benchmarks em modo interativo
dotnet run -c Release --project benchmarks/ExtensionLibrary.Benchmarks

# Validação rápida de compilação e estabilidade (Job Dry)
dotnet run -c Release --project benchmarks/ExtensionLibrary.Benchmarks -- --job dry --filter *
```

Consulte a [ADR 012: Suíte de Micro-benchmarks Científicos e Engenharia Zero-Allocation](./docs/adr/ADR-012-benchmarks-e-performance-zero-allocation.md) para detalhes dos trade-offs de engenharia.

---

## 🏛️ Arquitetura e Decisões de Engenharia

Para entender os padrões de design, trade-offs, análise de segurança e matriz de compatibilidade do ecossistema, consulte nossa documentação técnica:
- [Visão Geral da Arquitetura & Diagramas C4](./docs/arquitetura/visao-geral.md)
- [ADR 000: Arquitetura e Convenções](./docs/adr/ADR-000-arquitetura-e-convencoes.md)
- [Catálogo Completo de 13 ADRs](./docs/adr/)

---

## 🤝 Contribuição

Contribuições são muito bem-vindas! Siga estas diretrizes:
1. Abra uma issue para discutir novas funcionalidades ou relatar bugs.
2. Certifique-se de que os métodos de extensão respeitem **Guard Clauses** no primeiro argumento (`this T source`).
3. Mantenha conformidade com todos os runtimes suportados (`dotnet build`).
4. Acompanhe novas implementações com testes unitários no padrão Triple A.

---

## 📄 Licença

Este projeto é distribuído sob a licença [MIT](LICENSE).
