# 🏷️ TL.EnumExtensionsLibrary

[![NuGet](https://img.shields.io/nuget/v/TL.EnumExtensionsLibrary.svg?style=flat-square&label=TL.EnumExtensionsLibrary)](https://www.nuget.org/packages/TL.EnumExtensionsLibrary/)
[![.NET](https://img.shields.io/badge/.NET-netstandard2.0%20%7C%20net8.0-blue.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](../LICENSE.txt)
> **Cached and safe enum extensions for .NET: fast attribute description lookup, key-value mappings, and reliable string parsing.**  
> *Extensões seguras e com cache para Enums no .NET: busca rápida de descrições por atributos, mapeamentos chave-valor e conversões confiáveis.*

O **`TL.EnumExtensionsLibrary`** é uma biblioteca .NET BCL pura voltada para a manipulação produtiva e de alta performance de tipos enumerados (`System.Enum`), fornecendo cache thread-safe de metadados, suporte a descrições multicenário/chave (`[EnumDescription]`), conversão para dicionários e busca reversa.

---

## 📦 Instalação

Adicione o pacote ao seu projeto através do .NET CLI:

```bash
dotnet add package TL.EnumExtensionsLibrary
```

---

## 🚀 Funcionalidades Principais

| Método | Retorno | Descrição |
| :--- | :---: | :--- |
| `ToEnum<TEnum>(defaultValue, ignoreCase)` | `TEnum` | Converte string para enum de forma segura com fallback sem disparar exceções. |
| `ToEnumOrNull<TEnum>(ignoreCase)` | `TEnum?` | Converte string para enum anulável retornando `null` caso inválido. |
| `GetCachedDescriptions<TEnum>()` | `IReadOnlyDictionary<TEnum, string>` | Retorna catálogo de descrições tipadas com cache estático thread-safe $O(1)$. |
| `GetDescription()` | `string` | Retorna a descrição do atributo `[Description]` com cache estático thread-safe $O(1)$ ou o nome do enum. |
| `GetDescription(key)` | `string` | Retorna a descrição associada a uma chave específica mapeada via `[EnumDescription(key, description)]`. |
| `GetDescription(key, default)` | `string` | Retorna a descrição para a chave especificada ou o valor padrão caso a chave não exista. |
| `ToDictionary<T>()` | `IDictionary<int, string>` | Converte o enum em um dicionário `(Id -> Nome)` para preenchimento de combos e selects. |
| `GetAllDescriptions<T>()` | `Dictionary<T, string>` | Retorna um dicionário mapeando cada valor enumerado para sua respectiva descrição. |
| `GetValues<T>()` | `IList<T>` | Retorna uma lista tipada contendo todos os valores definidos no enum. |
| `GetNames<T>()` | `IList<string>` | Retorna uma lista contendo todos os nomes literais dos identificadores do enum. |
| `GetEnumByDescription<T>(desc)` | `T` | Realiza a busca reversa, localizando o valor do enum a partir de sua descrição textual. |
| `TryParse<T>(str, out result)` | `bool` | Realiza o parse seguro de texto para o valor enumerado correspondente. |
| `HasFlag<T>(flag)` | `bool` | Verifica se um determinado bit/flag está ativo no enum decorado com `[Flags]`. |

---

## 💡 Exemplos de Uso

```csharp
using System;
using System.ComponentModel;
using EnumExtensionsLibrary;

public enum StatusPedido
{
    [Description("Aguardando Pagamento")]
    [EnumDescription(1, "Aguardando Confirmação Bancária")]
    [EnumDescription(2, "Em Processamento no Gateway")]
    Pendente,

    [Description("Pedido Faturado")]
    Faturado,

    [Description("Cancelado pelo Cliente")]
    Cancelado
}

public class Exemplo
{
    public void Executar()
    {
        var status = StatusPedido.Pendente;

        var resolvido = "faturado".ToEnum(StatusPedido.Pendente);
        var opcional = "inexistente".ToEnumOrNull<StatusPedido>();

        var catalogo = EnumExtension.GetCachedDescriptions<StatusPedido>();

        string descPadrao = status.GetDescription();
        string descGateway = status.GetDescription(2);

        var opcoes = EnumExtension.ToDictionary<StatusPedido>();
        var encontrado = EnumExtension.GetEnumByDescription<StatusPedido>("Pedido Faturado");
    }
}
```

---

## 🏛️ Decisões Arquiteturais

Para detalhes sobre o cache thread-safe de metadados, suporte a múltiplos atributos e resolução graciosa de descrições, consulte:
- 📄 [ADR-003: Decisões Arquiteturais do TL.EnumExtensionsLibrary](../docs/adr/ADR-003-enum-extensions-library.md)

---

## 📄 Licença

Distribuído sob a licença [MIT](../LICENSE.txt).