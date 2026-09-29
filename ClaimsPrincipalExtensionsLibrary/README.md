# 🛡️ TL.ClaimsPrincipalExtensionsLibrary

[![NuGet](https://img.shields.io/nuget/v/TL.ClaimsPrincipalExtensionsLibrary.svg?style=flat-square&label=TL.ClaimsPrincipalExtensionsLibrary)](https://www.nuget.org/packages/TL.ClaimsPrincipalExtensionsLibrary/)
[![.NET](https://img.shields.io/badge/.NET-netstandard2.0%20%7C%20net8.0-blue.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](../LICENSE.txt)
> **Strongly-typed ClaimsPrincipal extensions for ASP.NET Core: clean, safe access to user IDs, roles, emails, and authentication claims.**  
> *Extensões fortemente tipadas para ClaimsPrincipal no ASP.NET Core: acesso limpo e seguro a IDs, roles, e-mails e claims de autenticação.*

O **`TL.ClaimsPrincipalExtensionsLibrary`** é uma biblioteca .NET BCL pura voltada para a simplificação e segurança na extração de dados de autenticação e autorização a partir de instâncias de `System.Security.Claims.ClaimsPrincipal` (`HttpContext.User`).

---

## 📦 Instalação

Adicione o pacote ao seu projeto através do .NET CLI:

```bash
dotnet add package TL.ClaimsPrincipalExtensionsLibrary
```

---

## 🚀 Funcionalidades Principais

| Método | Retorno | Descrição |
| :--- | :---: | :--- |
| `IsAuthenticated()` | `bool` | Verifica se o principal possui identidade ativa e autenticada com segurança. |
| `Id()` | `long` | Extrai o identificador numérico da claim `sub` (Subject ID). |
| `GetUserId<T>()` | `T` | Extrai e converte tipado o Subject ID (`Guid`, `int`, `long`, `string`) de `sub` ou `NameIdentifier`. |
| `GetUserIdOrDefault<T>(default)` | `T` | Extrai e converte o Subject ID tipado com fallback seguro sem lançar exceções. |
| `ClaimSub()` | `string?` | Retorna o valor original em texto da claim `sub`. |
| `Email()` | `string?` | Retorna o endereço de email autenticado (`email` ou `ClaimTypes.Email`). |
| `FullName()` | `string?` | Retorna o nome completo do usuário autenticado (`name` ou `ClaimTypes.Name`). |
| `HasRole(role)` | `bool` | Valida se o usuário possui determinado papel/permissão de acesso. |
| `ClaimRoles()` | `IEnumerable<string>` | Retorna todas as roles associadas ao usuário no token. |
| `Roles<T>()` | `IEnumerable<T>` | Faz o parse tipado das roles do usuário para um tipo `enum` de autorização. |
| `Claim(claimType)` | `Claim?` | Obtém a primeira ocorrência da claim especificada. |

---

## 💡 Exemplos de Uso

```csharp
using ClaimsPrincipalExtensionsLibrary;
using Microsoft.AspNetCore.Http;

public class PedidoController
{
    public void Processar(HttpContext context)
    {
        var user = context.User;

        if (!user.IsAuthenticated())
        {
            throw new UnauthorizedAccessException("Usuário não autenticado.");
        }

        long usuarioId = user.Id();
        string email = user.Email() ?? "desconhecido@empresa.com";
        string nome = user.FullName() ?? "Usuário";

        if (user.HasRole("Administrador"))
        {
            Console.WriteLine($"Acesso prioritário liberado para {nome} (ID: {usuarioId})");
        }
    }
}
```

---

## 🏛️ Decisões Arquiteturais e Segurança

Para detalhes sobre suporte a múltiplos esquemas de claims (JWT vs WS-Federation) e prevenção contra mascaramento de identidade, consulte o documento oficial:
- 📄 [ADR-004: Decisões Arquiteturais do TL.ClaimsPrincipalExtensionsLibrary](../docs/adr/ADR-004-claims-principal-extensions-library.md)

---

## 📄 Licença

Distribuído sob a licença [MIT](../LICENSE.txt).

