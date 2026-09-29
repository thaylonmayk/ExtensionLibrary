# 🧵 TL.StringExtensionsLibrary

[![NuGet](https://img.shields.io/nuget/v/TL.StringExtensionsLibrary.svg?style=flat-square&label=TL.StringExtensionsLibrary)](https://www.nuget.org/packages/TL.StringExtensionsLibrary/)
[![.NET](https://img.shields.io/badge/.NET-netstandard2.0%20%7C%20net8.0-blue.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](../LICENSE.txt)
> **Essential and ergonomic C# string utilities: safe truncation, span-friendly trimming, case-insensitive helpers, and clean parsing.**  
> *Utilitários essenciais e ergonômicos de string em C#: truncamento seguro, trim otimizado, comparações case-insensitive e conversões limpas.*

O **`TL.StringExtensionsLibrary`** fornece um conjunto abrangente de métodos de extensão para manipulação, formatação, validação, sanitização e conversão segura de cadeias de caracteres (`string`) em aplicações C# / .NET.

---

## 📦 Instalação

Adicione o pacote ao seu projeto através do .NET CLI:

```bash
dotnet add package TL.StringExtensionsLibrary
```

---

## 🚀 Funcionalidades Principais

| Categoria | Métodos Disponíveis | Descrição |
| :--- | :--- | :--- |
| **Parsing & Conversões** | `ToBoolean()`, `ToInt32()`, `ToDecimal()`, `ToDateTime()` | Conversão tipada com validação defensiva e suporte a diferentes formatos. |
| **Nomenclatura & Slugs** | `ToSnakeCase()`, `ToKebabCase()`, `ToSlug()` | Conversão padronizada de identificadores para banco, APIs e URLs amigáveis. |
| **Validação & Estado** | `IsNullOrEmpty()`, `IsNullOrWhiteSpace()`, `IsDateTime()`, `IsNumeric()` | Checagens rápidas de nulidade e formatos de data/numérico. |
| **Truncamento & Fatiamento** | `Truncate()`, `TruncateWithEllipsis()`, `Left()`, `Right()`, `TrimSafe()` | Corte seguro de strings sem disparar `ArgumentOutOfRangeException`. |
| **Formatação & Delimitadores** | `SplitBy()`, `ToCsv()`, `FromCsv()`, `ToDictionary()` | Transformação de textos estruturados em coleções e dicionários. |
| **Expressões Regulares** | `IsMatch()`, `RegexReplace()` | Operações com regex com prevenção de ReDoS. |

---

## 💡 Exemplos de Uso

```csharp
using StringExtensionLibrary;

string ativoStr = "yes";
bool ativo = ativoStr.ToBoolean();

string coluna = "UsuarioCadastroId".ToSnakeCase();
string header = "ContentSecurityPolicy".ToKebabCase();
string urlAmigavel = "Artigo Especial: C# 12 e .NET 8!".ToSlug();

string texto = "Relatório mensal consolidado de faturamento corporativo";
string preview = texto.TruncateWithEllipsis(20);

string dataInput = "03/09/2026";
bool ehDataValida = dataInput.IsDateTime("dd/MM/yyyy");

string codigo = "INV-2026-998877";
string prefixo = codigo.Left(3);
string sufixo = codigo.Right(6);
```

---

## 🏛️ Decisões Arquiteturais e Segurança

Para detalhes sobre diretrizes de segurança (proteção contra ReDoS, invariância de cultura e uso de `ReadOnlySpan<char>`), consulte o documento oficial:
- 📄 [ADR-001: Decisões Arquiteturais do TL.StringExtensionsLibrary](../docs/adr/ADR-001-string-extensions-library.md)

---

## 📄 Licença

Distribuído sob a licença [MIT](../LICENSE.txt).