# 🌐 TL.HttpClientExtensionsLibrary

[![NuGet](https://img.shields.io/nuget/v/TL.HttpClientExtensionsLibrary.svg?style=flat-square&label=TL.HttpClientExtensionsLibrary)](https://www.nuget.org/packages/TL.HttpClientExtensionsLibrary/)
[![.NET](https://img.shields.io/badge/.NET-netstandard2.0%20%7C%20net8.0-blue.svg)](https://dotnet.microsoft.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](../LICENSE.txt)
> **Resilient HttpClient extensions for .NET: safe request handling, built-in retry policies, JSON helpers, and robust API communication.**  
> *Extensões resilientes para HttpClient no .NET: manipulação segura de requisições, políticas de retry integradas, helpers de JSON e comunicação robusta com APIs.*

O **`TL.HttpClientExtensionsLibrary`** é uma biblioteca .NET voltada para simplificar chamadas REST sobre `System.Net.Http.HttpClient`, adicionando suporte a políticas de retentativa resilientes, tratamento de rate limiting (HTTP 429), upload/download de arquivos e manipulação de autenticação com Bearer Token e JWT.

---

## 📦 Instalação

Adicione o pacote ao seu projeto através do .NET CLI:

```bash
dotnet add package TL.HttpClientExtensionsLibrary
```

---

## 🚀 Funcionalidades Principais

| Categoria | Métodos Disponíveis | Descrição |
| :--- | :--- | :--- |
| **Envio & Respostas JSON** | `SendJsonAsync<T>()`, `ReadAsJsonAsync<T>()`, `PostJsonAsync<T>()` | Serialização e deserialização automática de payloads HTTP. |
| **Resiliência & Retry** | `ExponentialBackoffRetryAsync()`, `RateLimitRetryAsync()`, `TimeoutRetryAsync()` | Retentativas com backoff exponencial e respeito a cabeçalhos `Retry-After`. |
| **Autenticação & Tokens** | `AddBearerToken(token)`, `ExtractClaims(token)` | Injeção de credenciais Bearer e inspeção de claims em tokens JWT. |
| **Transferência de Arquivos** | `UploadFileAsync()`, `DownloadFileAsync()`, `GetFileSizeAsync()` | Upload via `multipart/form-data` e download via streams otimizados. |
| **Diagnóstico & Logs** | `LogRequest()`, `LogResponse()`, `LogError()` | Rastreabilidade de chamadas HTTP externas sem vazamento de credenciais. |

> 💡 **Nota de Arquitetura Corporativa:** As extensões de retry deste pacote são leves e voltadas para chamadas HTTP pontuais, ferramentas CLI e scripts utilitários. Para microsserviços de alta concorrência em produção corporativa que demandam Circuit Breaker distribuído com estado compartilhado entre requisições, recomenda-se a adoção de `Polly v8` ou `Microsoft.Extensions.Http.Resilience` integrado ao `IHttpClientFactory`.

---

## 💡 Exemplos de Uso

```csharp
using System.Net.Http;
using HttpClientExtensionsLibrary;

class Program
{
    static async Task Main()
    {
        using var httpClient = new HttpClient();

        // 1. Configuração de Bearer Token
        httpClient.AddBearerToken("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...");

        // 2. Envio de Payload com Resposta Tipada
        var payload = new { Nome = "Serviço de Cobrança", Ativo = true };
        var resposta = await httpClient.SendJsonAsync<ResultadoDto>(
            "https://api.empresa.com/servicos", 
            HttpMethod.Post, 
            payload
        );

        // 3. Download Direto de Arquivo
        await httpClient.DownloadFileAsync(
            "https://api.empresa.com/relatorios/2026-09.pdf", 
            @"C:\temp\relatorio.pdf"
        );
    }
}
```

---

## 🏛️ Decisões Arquiteturais e Segurança

Para detalhes sobre a prevenção de reenvio inválido de instâncias `HttpRequestMessage`, suporte a `CancellationToken` e proteção contra vazamento de tokens em logs, consulte o documento oficial:
- 📄 [ADR-008: Decisões Arquiteturais do TL.HttpClientExtensionsLibrary](../docs/adr/ADR-008-http-client-extensions-library.md)

---

## 📄 Licença

Distribuído sob a licença [MIT](../LICENSE.txt).
