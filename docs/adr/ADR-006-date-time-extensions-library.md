# ADR 006: Decisões Arquiteturais do Pacote TL.DateTimeExtensionsLibrary

---

## Contexto

O pacote `TL.DateTimeExtensionsLibrary` disponibiliza utilitários para cálculos temporais, manipulação de calendários, operações de fuso horário e contagem de dias úteis sobre `System.DateTime`.

Em serviços de faturamento, agendamento de tarefas e relatórios analíticos, cálculos de prazos e agregações temporais exigem precisão de timezone, imutabilidade e alta performance algorítmica.

---

## Decisões Arquiteturais

### 1. Preservação de Integridade de `DateTimeKind`
- Todas as operações de transformação de datas (como início de mês, adições de períodos e cálculo de idade) preservam o `DateTimeKind` original (`Utc`, `Local` ou `Unspecified`), evitando conversões acidentais de fuso horário em pipelines de dados.

### 2. Eficiência Algorítmica em Dias Úteis ($O(1)$)
- O cálculo de dias úteis em intervalos longos é otimizado matematicamente através do cálculo das semanas completas ($5 \times \text{semanas}$) somado aos dias remanescentes, reduzindo a complexidade de $O(N)$ iterativo para tempo constante $O(1)$ e eliminando loops custosos.

### 3. Fatiamento Temporal para Processamento em Lote (`Chunks`)
- O método `Chunks(endDate, days)` facilita o particionamento de consultas a bancos de dados e chamadas externas em janelas temporais menores e gerenciáveis, prevenindo travamentos por consultas massivas.

### 4. Testabilidade com `TimeProvider` (.NET 8+)
- Para garantir suporte a testes unitários determinísticos em pipelines de CI/CD, operações que consultam o relógio do sistema oferecem suporte opcional à abstração `TimeProvider`, permitindo simular datas futuras e passadas sem alterar o relógio da máquina servidora.

### 5. Invariância UTC, Interoperabilidade Unix Epoch e Períodos Mensais
- **Normalização Segura para UTC (`EnsureUtc`):** Elimina anomalias de fuso horário em ambientes distribuídos convertendo instâncias com `DateTimeKind.Local` via `ToUniversalTime()`, atribuindo `DateTimeKind.Utc` a instâncias `Unspecified` e preservando instâncias já em UTC.
- **Interoperabilidade Unix Epoch (`ToUnixTimeMilliseconds` e `FromUnixTimeMilliseconds`):** Padroniza a conversão bidirecional de e para carimbos de data/hora no padrão POSIX/Unix Epoch (`1970-01-01T00:00:00Z`), essencial para contratos de telemetria, eventos e mensageria distribuída.
- **Cálculo Determinístico de Limites Mensais (`StartOfMonth` e `EndOfMonth`):** Retorna com precisão o primeiro (`00:00:00.000`) e último instante (`23:59:59.999`) de cada mês, calculando dinamicamente dias do mês com suporte a anos bissextos e preservando estritamente o `DateTimeKind`.

### 6. Suporte Extensível a Feriados Corporativos (`IHolidayProvider`)
- Para atender a regras de negócio corporativas que exigem dedução de feriados bancários, municipais e nacionais móveis, a suíte introduz o contrato `IHolidayProvider` e sobrecargas dedicadas para `BusinessDaysBetween` e `BusinessDaysUntil`:
  - **Null Object Pattern (`NullHolidayProvider`):** Implementação singleton padrão que trata todas as datas como dias regulares caso nenhum calendário específico seja configurado.
  - **Sobrecargas Flexíveis:** Suporte direto a coleções enumeradas (`IEnumerable<DateTime>`), conjuntos (`HashSet<DateTime>`) ou predicados funcionais (`Func<DateTime, bool>`).
  - **Tratamento UTC em Timestamps:** Normalização consistente de `DateTimeKind.Unspecified` para UTC em métodos de conversão de época Unix (`ToUnixTimestamp`), assegurando paridade entre servidores em fusos distintos.

### 7. Limites Diários, Anuais e Conversores DateOnly / TimeOnly
- **Normalização de Limites Temporais:** `StartOfDay()` e `EndOfDay()` oferecem truncamento e expansão determinísticos para o primeiro (`00:00:00.000`) e último instante (`23:59:59.999`) do dia, com preservação estrita de `DateTimeKind`.
- **Enquadramento Anual e Cálculo de Idade:** `StartOfYear()` e `EndOfYear()` agilizam queries fiscais anuais. `CalculateAge()` calcula a idade exata com tratamento correto para anos bissextos.
- **Interoperabilidade com Tipos Modernos (.NET 8+):** `ToDateOnly()` e `ToTimeOnly()` eliminam conversões manuais e reduzem boilerplate na persistência de entidades e contratos de API.

---

## Consequências e Trade-offs

- **Performance Escalável:** Resolução instantânea de prazos úteis para contratos e faturas de longa duração.
- **Confiabilidade Temporal:** Ausência de inconsistências em ambientes de nuvem e contêineres que operam estritamente em UTC.
- **Interoperabilidade Universal:** Integração simplificada com bancos de dados time-series, brokers de mensageria e protocolos REST/gRPC que trafegam Unix Epoch timestamps ou DateOnly.
- **Flexibilidade de Feriados:** A contagem padrão opera em $O(1)$ sobre fins de semana; quando calendários com feriados corporativos são necessários, as sobrecargas com `IHolidayProvider` realizam a dedução exata dos dias não-trabalhados sem acoplamento a provedores externos de dados.
