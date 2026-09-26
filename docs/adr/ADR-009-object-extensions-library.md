# ADR 009: Decisões Arquiteturais do Pacote TL.ObjectExtensionsLibrary

---

## Contexto

O pacote `TL.ObjectExtensionsLibrary` oferece métodos utilitários genéricos para qualquer instância derivada de `System.Object`, abrangendo clonagem profunda (*deep cloning*), conversão em `ExpandoObject` e dicionários dinâmicos, conversão para byte arrays e invocação dinâmica via reflexão.

A proposta é acelerar o desenvolvimento de DTOs, testes automatizados e manipulação de estruturas flexíveis mantendo alocação controlada de memória.

---

## Decisões Arquiteturais

### 1. Clonagem Profunda com Proteção contra Ciclos
- A clonagem profunda de grafos de objetos utiliza serialização via `System.Text.Json` com `ReferenceHandler.IgnoreCycles`, assegurando a cópia de propriedades aninhadas e prevenindo estouros de pilha ou exceções de recursão infinita diante de referências circulares.

### 2. Conversão para Dicionário com Cache de Reflexão (`ToDictionary`)
- A conversão de objetos POCO em dicionários chave/valor disponibiliza a assinatura idiomática `ToDictionary()` (mantendo `Dictionary()` como compatibilidade).
- A inspeção de propriedades públicas legíveis é armazenada em cache thread-safe (`ConcurrentDictionary<Type, PropertyInfo[]>`), reduzindo o custo de Reflection em chamadas repetidas e permitindo pré-alocar a capacidade exata do dicionário gerado.

### 3. Preservação de Causa Raiz em Invocação Dinâmica (`InvokeMethod`)
- Em rotinas de invocação de métodos por reflexão, exceções disparadas dentro do método invocado são desembrulhadas de `TargetInvocationException` e relançadas via `ExceptionDispatchInfo.Capture(ex.InnerException).Throw()`, preservando integralmente o stack trace e a mensagem de erro original para depuração.

### 4. Encadeamento Funcional Fluente (`Pipe`, `As<T>`)
- Métodos de apoio como `obj.As<T>()` e `obj.Pipe(transform)` facilitam construções expressivas em estilo funcional sem poluir o código com variáveis temporárias descartáveis.

---

## Consequências e Trade-offs

- **Flexibilidade Máxima:** Capacidade de clonar e inspecionar objetos complexos com uma sintaxe simples e direta.
- **Rastreabilidade de Falhas:** Diagnóstico claro de erros em invocações dinâmicas sem mascaramento de exceções.
- **Trade-off de Tipagem:** Métodos de clonagem profunda baseados em dados focam em propriedades e campos serializáveis; delegates, manipuladores de eventos e ponteiros não são duplicados por design.
