# 🔬 Matriz Científica de Micro-benchmarks: TL.ExtensionLibrary

[![BenchmarkDotNet](https://img.shields.io/badge/BenchmarkDotNet-v0.14.0-blue.svg)](https://benchmarkdotnet.org/)
[![.NET](https://img.shields.io/badge/.NET-net8.0-purple.svg)](https://dotnet.microsoft.com/)
[![Configuration](https://img.shields.io/badge/Configuration-Release-green.svg)](https://learn.microsoft.com/dotnet/core/tools/dotnet-run)

Este projeto reúne a suíte oficial e exaustiva de micro-benchmarks científicos do ecossistema **TL.ExtensionLibrary**, confrontando seus módulos otimizados com as abordagens convencionais da Base Class Library (BCL) do .NET e bibliotecas consolidadas de mercado (`Humanizer.Core` e `morelinq`).

---

## ⚖️ Maturidade Técnica & Honestidade Intelectual (Trade-offs de Engenharia)

Nem todo método da biblioteca será estritamente mais rápido em todas as situações volumétricas. **Evidenciar isso com clareza reflete maturidade técnica e credibilidade arquitetural**:

1. **Zero Allocation vs CPU em Payloads Reduzidos:**  
   Em textos curtos (ex: strings de 10 caracteres), rotinas convencionais como `Substring` podem demandar 1 a 2 nanossegundos a menos de CPU que o overhead de setup de `string.Create`. No entanto, nossa implementação prioriza a **eliminação da fragmentação de Heap e coletas do Garbage Collector (0 Bytes alocados)**, prevenindo pausas de coleta (`GC pauses`) sob alta concorrência.
2. **Pureza de Dependências no Core (Zero External Dependencies):**  
   Bibliotecas de mercado como `Humanizer.Core` e `morelinq` trazem árvores de dependência e binários adicionais. A `TL.ExtensionLibrary` entrega paridade funcional e desempenho equivalente ou superior mantendo pacotes com **15 KB e 100% de pureza BCL** em .NET 8.
3. **Complexidade Algorítmica Real ($O(1)$ vs $O(N)$):**  
   Em coleções pequenas, o ganho de Keyset Pagination sobre Offset Pagination é imperceptível (empate técnico). A superioridade algorítmica ($O(1)$ constante) se manifesta em lotes profundos (ex: página 9.500 em bases de 100.000 itens), onde o Keyset Seek evita escanear milhares de nós descartáveis.

---

## 🚀 Como Executar

### 1. Execução Completa (Menu Interativo do BenchmarkDotNet)
```bash
dotnet run -c Release --project benchmarks/ExtensionLibrary.Benchmarks
```

### 2. Execução Rápida para Validação de Compilação (Job Dry)
```bash
dotnet run -c Release --project benchmarks/ExtensionLibrary.Benchmarks -- --job dry --filter *
```

### 3. Execução Filtrada por Módulo Específico
Para avaliar apenas um pacote isoladamente:

```bash
# Apenas Manipulação de Strings (Truncate, Sanitize, ToInt)
dotnet run -c Release --project benchmarks/ExtensionLibrary.Benchmarks -- --filter *String*

# Apenas Coleções e LINQ (IsNullOrEmpty, DistinctBy, Shuffle, Chunk)
dotnet run -c Release --project benchmarks/ExtensionLibrary.Benchmarks -- --filter *Collection*

# Apenas Consultas IQueryable e Paginação Keyset (Seek vs Offset, Order)
dotnet run -c Release --project benchmarks/ExtensionLibrary.Benchmarks -- --filter *Queryable*

# Apenas Metadados e Descrições de Enums (Cache Genérico vs Reflexão)
dotnet run -c Release --project benchmarks/ExtensionLibrary.Benchmarks -- --filter *Enum*

# Apenas Cálculos Numéricos e Financeiros (SafeDivide, RoundFinancial, Percentage)
dotnet run -c Release --project benchmarks/ExtensionLibrary.Benchmarks -- --filter *Numeric*

# Apenas Manipulação Temporal e Dias Úteis (BusinessDays, EnsureUtc, StartOfMonth)
dotnet run -c Release --project benchmarks/ExtensionLibrary.Benchmarks -- --filter *DateTime*

# Apenas Inspeção e Clonagem de Objetos (Clone, PropertiesEqual, IsDefault)
dotnet run -c Release --project benchmarks/ExtensionLibrary.Benchmarks -- --filter *Object*

# Apenas Segurança e ClaimsPrincipal (GetUserId tipado vs FindFirst manual)
dotnet run -c Release --project benchmarks/ExtensionLibrary.Benchmarks -- --filter *ClaimsPrincipal*
```

---

## 🏛️ Matriz Exaustiva dos 24 Cenários de Benchmark
## 🏛️ Matriz Exaustiva dos 26 Cenários de Benchmark

Abaixo, a decomposição técnica dos cenários medidos em cada um dos 8 módulos centrais:

### 1. `StringExtensionLibrary` (`StringBenchmarks.cs`)
Focado na redução de alocações efêmeras de strings no Heap e higienização segura de logs.

1. **Truncamento de Texto (`Truncate`)**: Compara concatenação manual com `Substring` contra `StringExtensions.Truncate` (baseado em `string.Create`) e `Humanizer.Truncate()`.  
   *Trade-off:* Zero string intermediária instanciada no Heap em .NET 8.
2. **Higienização de Log anti-CRLF (`SanitizeForLog`)**: Compara expressão regular compilada `Regex.Replace` contra substituição direta em buffer de caracteres (`Replace('\r', '_').Replace('\n', '_')`).  
   *Ganho:* Throughput ordens de grandeza superior sem overhead de máquina de estados de regex.
3. **Conversão Segura de Inteiro (`ToIntOrDefault`)**: Compara parsing defensivo com `try/catch` de `FormatException` contra `int.TryParse` com cultura invariante.  
   *Ganho:* Elimina stack unwinding completo do CLR em dados malformados.

---

### 2. `CollectionExtensionsLibrary` (`CollectionBenchmarks.cs`)
Focado na complexidade algorítmica e eliminação de alocações de enumeradores em coleções em memória.

1. **Verificação de Vazio (`IsNullOrEmpty`)**: Compara `!source.Any()` (que aloca enumerador no Heap) contra a checagem $O(1)$ de interfaces de contagem (`ICollection<T>`, `IReadOnlyCollection<T>`).  
   *Ganho:* Resolução instantânea sem alocação para listas e arrays.
2. **Remoção de Duplicatas (`DistinctBy`)**: Compara o agrupamento tradicional `GroupBy().Select(First)` contra o método nativo .NET 8 da biblioteca e `MoreLinq.MoreEnumerable.DistinctBy`.  
   *Ganho:* Operação em passagem única com tabela hash sem alocar grupos intermediários.
3. **Embaralhamento (`Shuffle`)**: Compara a abordagem ingênua `OrderBy(_ => Guid.NewGuid())` ($O(N \log N)$ com alocação massiva de GUIDs) contra o algoritmo Fisher-Yates ($O(N)$ linear) utilizando `Random.Shared`.  
   *Ganho:* Complexidade linear e zero contenção de threads.
4. **Particionamento em Lotes (`Chunk`)**: Compara rotinas iterativas aninhadas de `Skip/Take` ($O(N^2)$) contra o fatiador linear de chunks.  
   *Ganho:* Throughput proporcional ao tamanho da fonte de dados sem varreduras repetidas.

---

### 3. `QueryableExtensionsLibrary` (`QueryableBenchmarks.cs`)
Focado na eficiência de consultas a dados indexados e paginação profunda.

1. **Paginação Profunda (Keyset Seek vs Offset)**: Compara o método convencional `.Skip(9500).Take(20)` ($O(N)$) contra o Keyset Seek cursor indexado (`WHERE Id > @cursor`) ($O(1)$).  
   *Ganho:* Tempo constante de busca independente da profundidade da página navegada.
2. **Ordenação Dinâmica (`Order`)**: Compara reflexão sem cache com chamada a `PropertyInfo.GetValue` em tempo de execução contra árvores de expressão pré-compiladas com cache tipado de `MethodInfo`.  
   *Ganho:* Execução na velocidade de LINQ fortemente tipado sem invocações dinâmicas do DLR.

---

### 4. `EnumExtensionsLibrary` (`EnumBenchmarks.cs`)
Focado na eliminação do gargalo de Reflection para leitura de atributos de descrição em enums.

1. **Leitura de Descrição (`GetDescription`)**: Compara a consulta direta por reflexão `typeof(T).GetField().GetCustomAttribute<DescriptionAttribute>()` contra o cache genérico tipado `EnumMetadataCache<T>` (zero boxing no Heap) e o cache untyped com capacidade defensiva.  
   *Ganho:* Busca em dicionário concorrente em sub-microssegundos sem alocação após primeiro warm-up.
2. **Contenção Concorrente Multi-Thread**: Avalia 16 leituras paralelas simultâneas (`Parallel.For`) sobre o cache genérico e não-tipado (`Bounded LRU Cache`).  
   *Ganho:* Operação sem lock com `ConcurrentDictionary` garantindo baixa contenção sob concorrência intensa.

---

### 5. `NumericExtensionLibrary` (`NumericBenchmarks.cs`)
Focado na resiliência aritmética e precisão contábil.

1. **Divisão Segura (`SafeDivide`)**: Compara proteção por captura de `DivideByZeroException` contra verificação antecipada de divisor zero.  
   *Ganho:* Custo computacional de fração de nanossegundo e zero alocação de exceção.
2. **Arredondamento Bancário (`RoundFinancial`)**: Avalia o arredondamento simétrico financeiro (`MidpointRounding.ToEven`) com validação defensiva de casas decimais.  
   *Ganho:* Prevenção de viés estatístico cumulativo em operações financeiras e contábeis.
3. **Cálculo Percentual Seguro (`CalculatePercentageOf`)**: Avalia o cálculo proporcional com proteção nativa de divisão por zero.  
   *Ganho:* Garantia de estabilidade numérica sem bifurcações de código na camada de aplicação.

---

### 6. `DateTimeExtensionsLibrary` (`DateTimeBenchmarks.cs`)
Focado em cálculos de calendário corporativo e normalização temporal.

1. **Dias Úteis entre Datas (`BusinessDaysBetween`)**: Compara um loop diário iterativo (`while (d <= end)`) varrendo 5 anos (1.826 iterações) contra a fórmula vetorial direta da biblioteca (`fullWeeks * 5 + resto`).  
   *Ganho:* Redução de 1.800+ iterações para no máximo 6 verificações de dias residuais.
2. **Garantia de Fuso UTC (`EnsureUtc`)**: Compara chamadas incondicionais a `ToUniversalTime()` contra verificação defensiva de `DateTimeKind`.  
   *Ganho:* Evita recálculos de offset quando a data já se encontra em UTC.
3. **Início do Mês (`StartOfMonth`)**: Avalia a construção da data normalizada no primeiro instante (00:00:00.000) preservando o `DateTimeKind`.  
   *Ganho:* Código expressivo com alocação nula em pilha de execução.

---

### 7. `ObjectExtensionsLibrary` (`ObjectBenchmarks.cs`)
Focado na clonagem profunda e verificações de valor padrão.

1. **Clonagem Profunda (`Clone`)**: Compara cópia manual via reflexão propriedade-a-propriedade contra serialização/deserialização `System.Text.Json` com cláusula de guarda de nulidade.  
   *Trade-off:* O clone JSON aloca memória temporária de buffer, porém suporta árvores de objetos complexos e grafos aninhados sem acoplamento manual.
2. **Igualdade Estrutural (`PropertiesEqual`)**: Compara comparações explícitas contra varredura reflexiva de propriedades.  
3. **Verificação de Default (`IsDefault<T>`)**: Compara a chamada com boxing `object.Equals(obj, default)` contra a extensão tipada via `EqualityComparer<T>.Default`.  
   *Ganho:* Zero boxing allocation para value types e enums.

---

### 8. `ClaimsPrincipalExtensionsLibrary` (`ClaimsPrincipalBenchmarks.cs`)
Focado na resolução rápida de identificadores em pipelines de segurança de APIs.

1. **Extração Tipada de Identificador (`GetUserId<Guid>`)**: Compara a extração manual com `principal.FindFirst("sub")?.Value` + `Guid.TryParse()` contra o resolvedor tipado direto `GetUserId<Guid>()`.  
   *Ganho:* Código de segurança conciso e padronizado em todo o pipeline HTTP.
2. **Extração Numérica de Identificador (`GetUserId<int>`)**: Compara o fluxo manual de busca e conversão com cultura invariante contra `GetUserId<int>()`.  
   *Ganho:* Resolução segura sem exceções sob tokens JWT malformados ou incompletos.

---

## 📊 Glossário das Colunas do BenchmarkDotNet

| Coluna | Descrição Técnica |
| :--- | :--- |
| **`Method`** | Nome da rotina sob medição. |
| **`Mean`** | Tempo médio aritmético de execução por operação. |
| **`Error`** | Margem de erro estatístico dentro do intervalo de confiança de 99.9%. |
| **`StdDev`** | Desvio padrão amostral das iterações controladas. |
| **`Ratio`** | Razão relativa calculada contra o método de referência (`Baseline = 1.00`). Razões menores que 1.0 indicam maior velocidade. |
| **`Allocated`** | Quantidade de bytes alocados no Heap gerenciado por invocação. `0 B` indica Zero-Allocation. |
| **`Gen 0 / 1 / 2`** | Frequência de coletas efetuadas pelo Garbage Collector a cada 1.000 invocações. |
