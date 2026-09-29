# ADR 007: Decisões Arquiteturais do Pacote TL.CollectionExtensionsLibrary

---

## Contexto

O pacote `TL.CollectionExtensionsLibrary` provê extensões para coleções do .NET (`IEnumerable<T>`, `IList<T>`, `Dictionary<TKey, TValue>`, `HashSet<T>`, `Queue<T>`, `Stack<T>`). É uma das bibliotecas mais consumidas para operações de particionamento de lotes (batching), paginação em memória, filtros condicionais e manipulação de listas.

Por atuar no núcleo de pipelines de dados, a previsibilidade da complexidade assintótica e o consumo eficiente de memória são fatores mandatórios.

---

## Decisões Arquiteturais

### 1. Particionamento Linear $O(N)$ em `ChunkBy`
- O particionamento de `IEnumerable<T>` opera em passagem única (*single-pass streaming*) via `yield return`, processando sequências sem armazenar toda a fonte em memória.
- Para coleções indexadas (`IList<T>`), o particionamento utiliza indexação direta (`source[i + j]`) com pré-alocação da capacidade das listas de lotes (`(totalCount + chunkSize - 1) / chunkSize`), eliminando o custo quadrático $O(N^2)$ decorrente de chamadas encadeadas a `Skip(i).Take(size)`.
- Remoção de métodos utilitários redundantes (`RemoveAll` e `FindAll` em `List<T>`) para manter a API enxuta e sem duplicação de membros já presentes na BCL.

### 2. Embaralhamento Estatisticamente Uniforme (`Shuffle`)
- A operação de embaralhamento adota o algoritmo de Fisher-Yates (Knuth Shuffle), garantindo distribuição uniforme com complexidade $O(N)$ e aproveitando instâncias thread-safe (`Random.Shared` no .NET 6+) para máxima eficiência em cenários concorrentes.

### 3. Prevenção de Duplicatas com Tempo Constante $O(1)$
- O método `AddRangeIfNotExists` utiliza tabelas hash (`HashSet<T>`) para indexação intermediária, garantindo verificações de existência em tempo $O(1)$ por elemento adicionado, otimizando inserções em lote em listas grandes.

### 4. Tratamento Seguro de Nulidade em Comparações
- Operações de substituição e busca (`Replace`, `DistinctBy`) utilizam `EqualityComparer<T>.Default.Equals`, suportando coleções que contenham valores nulos com total estabilidade.

### 5. Otimizações de Coleções e Segregação de APIs
- **Verificação Segura em Tempo Constante (`IsNullOrEmpty`):** Inspeciona propriedades de contagem (`Count`) de interfaces especializadas (`ICollection<T>`, `IReadOnlyCollection<T>`) antes de recorrer ao enumerador, evitando alocações no heap e iterações desnecessárias para checagens de nulidade e vazio.
- **Segregação Física de `DistinctBy`:** Para evitar colisões de sobrecarga no .NET 8 (onde `Enumerable.DistinctBy` já existe nativamente na BCL), a extensão foi segregada fisicamente:
  - No .NET 8, delega diretamente para a implementação nativa da Microsoft com zero alocação intermediária.
  - No .NET Standard 2.0, utiliza tabela hash interna (`HashSet<TKey>`) com tratamento seguro para chaves nulas.
- **Inserção Tolerante a Nulos (`AddRangeIfNotNull`):** Facilita a agregação em coleções de destino ignorando fontes nulas sem interrupção de fluxo nem exigência de verificações condicionais redundantes no chamador.
- **Blindagem Defensiva em `ChunkBy` de Listas:** Validação estrita de limites (`chunkSize > 0`) e de fonte em coleções indexadas, prevenindo loops infinitos em chamadas com parâmetros inválidos.

### 6. Particionamento em Passagem Única e Concorrência Assíncrona com Semáforo
- **Particionamento $O(N)$ em Passagem Única (`Partition`):** Divide a sequência em uma tupla de listas `(Match, NonMatch)` avaliando o predicado exatamente uma vez por elemento, reduzindo pela metade as iterações em relação ao duplo `.Where()`.
- **Controle de Concorrência em Loops Assíncronos (`ForEachAsync`):** Utiliza `SemaphoreSlim` para delimitar o grau máximo de paralelismo (`maxDegreeOfParallelism`) em processamento I/O assíncrono, protegendo serviços dependentes contra exaustão de sockets ou threads.
- **Checagem Rápida $O(1)$ de Existência (`HasItems`):** Inspeciona interfaces especializadas de contagem (`ICollection<T>`, `IReadOnlyCollection<T>`) antes de acionar o enumerador, proporcionando verificação de existência com zero alocação intermediária.

---

## Consequências e Trade-offs

- **Alto Throughput:** Capacidade de particionar e processar milhões de registros em streams sem degradação exponencial de CPU.
- **Previsibilidade:** Eliminação de travamentos por consumo descontrolado de memória em pipelines de dados.
- **Trade-off Funcional:** Os métodos de `IEnumerable<T>` operam de forma puramente funcional retornando novas sequências (sem mutação), enquanto utilitários sobre `IList<T>` (como `Move` e `SortBy`) oferecem mutações *in-place* de alto desempenho quando expressamente desejado.
