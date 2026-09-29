# 🧩 TL.ObjectExtensionsLibrary

[![NuGet](https://img.shields.io/nuget/v/TL.ObjectExtensionsLibrary.svg?style=flat-square&label=TL.ObjectExtensionsLibrary)](https://www.nuget.org/packages/TL.ObjectExtensionsLibrary/)
[![.NET](https://img.shields.io/badge/.NET-netstandard2.0%20%7C%20net8.0-blue.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](../LICENSE.txt)
> **Handy C# object extensions: pre-configured JSON serialization, dictionary/Expando conversion, deep cloning, and cached property access.**  
> *Utilitários práticos para objetos em C#: serialização JSON pré-configurada, conversões para dicionário/Expando, clonagem e reflexão com cache.*

O **`TL.ObjectExtensionsLibrary`** disponibiliza métodos utilitários genéricos para qualquer instância derivada de `System.Object`, facilitando clonagem profunda (*deep cloning*), conversões para dicionários e `ExpandoObject`, serialização de dados e invocação reflexiva com preservação do stack trace original.

---

## 📦 Instalação

Adicione o pacote ao seu projeto através do .NET CLI:

```bash
dotnet add package TL.ObjectExtensionsLibrary
```

---

## 🚀 Funcionalidades Principais

| Método | Retorno | Descrição |
| :--- | :---: | :--- |
| `Clone()` | `T` | Realiza clonagem profunda (*deep copy*) em memória com proteção nativa contra referências circulares. |
| `ToExpando()` | `ExpandoObject` | Converte o objeto POCO em uma estrutura dinâmica expansível em tempo de execução. |
| `ToDictionary()` / `Dictionary()` | `Dictionary<string, object?>` | Converte as propriedades públicas legíveis do objeto em dicionário, com cache de reflexão em memória. |
| `Bytes()` | `byte[]` | Converte o objeto serializado para representação em array de bytes. |
| `InvokeMethod(method, ...args)` | `object?` | Invoca dinamicamente um método por nome preservando a causa raiz original de exceções. |
| `TryGetProperty(prop, out val)` | `bool` | Tenta ler o valor de uma propriedade pública sem disparar exceções caso ausente. |
| `SetProperty(prop, value)` | `void` | Atribui valor à propriedade pelo nome informado via reflexão. |
| `PropertiesEqual(other)` | `bool` | Compara se os valores de todas as propriedades públicas são idênticos entre dois objetos. |
| `IsDefault()` | `bool` | Verifica se o valor da instância corresponde ao `default` do seu tipo. |
| `Serialize()` | `string` | Serializa o objeto diretamente para formato JSON compacto. |
| `ToJsonIndented()` | `string` | Serializa o objeto para JSON formatado e indentado (pretty-print). |

---

## 💡 Exemplos de Uso

```csharp
using System;
using ObjectExtensionsLibrary;

public class Configuracao
{
    public string Servidor { get; set; } = "localhost";
    public int Porta { get; set; } = 8080;
    public bool Habilitado { get; set; } = true;
}

public class Exemplo
{
    public void Executar()
    {
        var config = new Configuracao();

        var copia = config.Clone();
        copia.Porta = 9000;

        bool saoIguais = config.PropertiesEqual(copia);
        var mapa = config.Dictionary();
        dynamic expando = config.ToExpando();
    }
}
```

---

## 🏛️ Decisões Arquiteturais

Para detalhes sobre clonagem profunda, desembrulho de `TargetInvocationException` e convenções de reflexão, consulte:
- 📄 [ADR-009: Decisões Arquiteturais do TL.ObjectExtensionsLibrary](../docs/adr/ADR-009-object-extensions-library.md)

---

## 📄 Licença

Distribuído sob a licença [MIT](../LICENSE.txt).
