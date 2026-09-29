# ADR 003: Decisões Arquiteturais do Pacote TL.EnumExtensionsLibrary

---

## Contexto

O pacote `TL.EnumExtensionsLibrary` é uma biblioteca pura da BCL do .NET que estende o tipo `System.Enum`. Ele fornece métodos para transformar enums em coleções estruturadas para preenchimento de select lists/combos em APIs e front-ends (`ToDictionary`, `ToList`) e extrair metadados descritivos declarados em atributos de anotação (`[Description]`, `[EnumDescription]`, `[Display]`).

O objetivo é eliminar código repetitivo de reflexão nas camadas de apresentação e serviços, mantendo alta eficiência computacional.

---

## Decisões Arquiteturais

### 1. Cache Estático Thread-Safe de Metadados (`EnumCache<T>`)
- A inspeção de atributos declarativos via Reflection em loops ou requisições HTTP de alta frequência é otimizada através de cache estático genérico thread-safe. As informações descritivas são lidas exatamente uma vez por tipo durante o ciclo de vida do processo, garantindo consultas subsequentes em tempo quase instantâneo ($O(1)$).

### 2. Aproveitamento de APIs Modernas do .NET 8+
- Em runtimes .NET 8+, a biblioteca adota `Enum.GetValues<T>()` genérico, eliminando operações de *boxing* e *unboxing* na iteração de valores enumerados.

### 3. Resolução Graciosa de Descrições
- Ao solicitar a descrição de um enum que não contenha atributo anotado, o método realiza fallback automático para o nome literal do identificador (`enumValue.ToString()`), assegurando que a chamada sempre retorne um texto utilizável sem quebras de execução.
- O mecanismo garante a resolução correta de descrições para membros enumerados com valor ordinal zero (ex.: `None = 0` ou `Initial = 0`), eliminando supressões indevidas causadas por checagens genéricas de igualdade contra `default`.

### 4. Suporte a Padrões de Atributos do Ecossistema
- Reconhecimento automático dos atributos padrão `System.ComponentModel.DescriptionAttribute`, `System.ComponentModel.DataAnnotations.DisplayAttribute` e o atributo embutido `EnumDescriptionAttribute`.

### 5. Blindagem Defensiva com Bounded Cache e Fallback Seguro 
- Para proteger ambientes sob restrição severa de memória (ex: containers AWS Fargate / Azure Container Apps com 256MB ou 512MB) contra enums dinâmicos ou volume massivo de consultas não mapeadas, o cache estático interno de descrições (`EnumDescriptionCache` / `EnumDescriptionCache<TEnum>`) foi blindado com limite finito de capacidade (Bounded Cache, padrão 1.024 entradas por partição).
- Ao atingir a capacidade máxima configurada, o mecanismo não lança exceções nem compromete a integridade do processo: adota fallback seguro de reflexão direta sob demanda para as novas entradas sem inseri-las no dicionário, estabilizando o consumo de memória em patamar rigorosamente previsível ($O(1)$ em espaço).
- Atomicidade e thread-safety absolutos sob alta concorrência são assegurados através de double-checked locking na inserção e leitura direta lock-free via `ConcurrentDictionary.TryGetValue`.
- Em runtimes modernos (.NET 8+), o cache genérico `EnumDescriptionCache<TEnum>` garante leitura ultrarrápida em tempo $O(1)$ com alocação estrita de `0 B` de heap para entradas já cacheadas.

### 6. Parsing Seguro com Fallback e Catálogo de Descrições Tipadas
- **Parsing com Fallback (`ToEnum` e `ToEnumOrNull`):** Permite converter cadeias de caracteres em membros enumerados com tolerância a maiúsculas/minúsculas e fallback explícito, eliminando o custo de `try/catch` de `ArgumentException` em dados corrompidos.
- **Catálogo Global de Descrições (`GetCachedDescriptions`):** Expõe um dicionário somente-leitura (`IReadOnlyDictionary<TEnum, string>`) alimentado pelo cache estático, viabilizando exposição fluente de catálogos descritivos para APIs e contratos de frontend.

---

## Consequências e Trade-offs

- **Performance Acelerada:** Redução expressiva do custo computacional de Reflection em endpoints que retornam catálogos de enums ($O(1)$ e `0 B` de alocação no caminho quente).
- **Produtividade:** Geração ágil de dicionários `(ID -> Descrição)` com uma única linha de código.
- **Blindagem de Memória (Bounded Memory Footprint):** O consumo de memória é estritamente limitado pelo teto do Bounded Cache (1.024 entradas), eliminando vulnerabilidade de vazamento de memória ou OutOfMemory por enums dinâmicos.
- **Fallback Gracioso sob Sobrecarga:** Entradas que ultrapassam a capacidade máxima são resolvidas via reflexão sob demanda sem falhas ou crashes de processo.
