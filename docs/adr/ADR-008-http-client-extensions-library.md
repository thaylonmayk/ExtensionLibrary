# ADR 008: Decisões Arquiteturais do Pacote TL.HttpClientExtensionsLibrary

---

## Contexto

O pacote `TL.HttpClientExtensionsLibrary` simplifica o consumo de APIs REST, autenticação com Bearer Token e estratégias de resiliência sobre `System.Net.Http.HttpClient`.

Em ambientes de microsserviços, chamadas HTTP remotas estão sujeitas a falhas intermitentes de rede, limites de taxa (rate limiting HTTP 429) e timeouts, exigindo mecanismos estruturados de retry e gestão adequada do ciclo de vida das requisições.

---

## Decisões Arquiteturais

### 1. Gestão Segura do Ciclo de Vida de `HttpRequestMessage`
- No runtime .NET, um `HttpRequestMessage` é consumido após o primeiro envio e não pode ser retransmitido diretamente sem reinicialização.
- Para garantir estabilidade em loops de retentativa, as políticas de retry aceitam uma fábrica de mensagens (`Func<HttpRequestMessage>`) ou realizam a recriação controlada do cabeçalho e corpo da requisição a cada tentativa, garantindo reexecução limpa e previsível.

### 2. Controle de Cancelamento Obrigatório (`CancellationToken`)
- Todos os métodos assíncronos de rede aceitam `CancellationToken cancellationToken = default` e o propagam para o cliente HTTP e pausas com backoff (`Task.Delay`), permitindo encerramento imediato das chamadas quando a requisição original for cancelada ou sofrer timeout.

### 3. Classificação Rigorosa de Erros Transitórios
- As políticas de retentativa concentram-se estritamente em falhas transitórias comprovadas: erros de servidor (HTTP 500, 502, 503, 504), requisições expiradas (HTTP 408) e interrupções de conexão de rede, evitando retentativas inúteis diante de erros permanentes de cliente (HTTP 400, 401, 403, 404).

### 4. Conformidade com Cabeçalho `Retry-After` (HTTP 429)
- Diante de respostas com status HTTP 429 (Too Many Requests), as extensões interpretam o valor do cabeçalho `Retry-After` (em segundos ou timestamp HTTP), respeitando a janela exigida pelo servidor de destino antes da próxima tentativa.

### 5. Foco em Retentativas e Remoção de Circuit Breakers Locais
- Métodos de extensão utilitários para `HttpClient` não devem reter estado compartilhado de falhas entre instâncias e threads. Por essa razão, implementações locais e simplificadas de Circuit Breaker foram removidas da biblioteca.
- O pacote foca estritamente em retentativas idempotentes leves, clonagem de requisições e respeito ao cabeçalho `Retry-After`. Para padrões complexos de tolerância a falhas com estado compartilhado, a recomendação é a adoção de `Polly v8` ou `Microsoft.Extensions.Http.Resilience` acoplados ao `IHttpClientFactory`.

---

## Consequências e Trade-offs

- **Resiliência Transparente:** Recuperação automática de instabilidades momentâneas de rede sem necessidade de código defensivo em cada chamada HTTP.
- **Proteção de Threads:** Prevenção contra exaustão de conexões e retenção de threads de I/O em cenários de cancelamento.
- **Simplicidade Operacional:** Ausência de efeitos colaterais por estados retidos em memória entre diferentes requisições HTTP.
