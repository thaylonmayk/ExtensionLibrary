# ADR 012: Suíte de Micro-benchmarks Científicos e Engenharia Zero-Allocation

---

## Contexto

A suíte `TL.ExtensionLibrary` foi concebida com premissas rigorosas de alta performance, tempo de execução constante ($O(1)$) e alocação mínima ou nula de memória no Heap gerenciado (Zero-Allocation em rotinas críticas via `ReadOnlySpan<char>`, `string.Create`, `Random.Shared` e caches thread-safe).

Para comprovar cientificamente essas características de engenharia, prevenir regressões silenciosas de performance em evoluções futuras e fornecer transparência comparativa em relação às abordagens convencionais do .NET e bibliotecas consolidadas do ecossistema (`Humanizer.Core` e `morelinq`), estabeleceu-se a necessidade de incorporar uma infraestrutura oficial, reproduzível e auditável de micro-benchmarks.

---

## Decisões Arquiteturais

### 1. Adoção Oficial do BenchmarkDotNet v0.14.0 em Projeto Dedicado

Optou-se por criar o projeto executável `benchmarks/ExtensionLibrary.Benchmarks` utilizando **BenchmarkDotNet v0.14.0**:
- O projeto é configurado como executável console (`net8.0`, `OutputType = Exe`) com `<IsPackable>false</IsPackable>`, garantindo que binários de teste jamais sejam publicados como pacotes NuGet da biblioteca.
- Utilização de `BenchmarkSwitcher` no ponto de entrada (`Program.cs`), viabilizando tanto navegação interativa no terminal quanto automação em pipelines via filtros (`--filter *String*`) e validações rápidas (`--job dry`).

### 2. Isolamento Rígido de Dependências Comparativas via CPM

Para confrontar nossos métodos com alternativas de mercado:
- Pacotes como `Humanizer.Core` e `morelinq` foram centralizados no [`Directory.Packages.props`](file:///c:/src/Library/ExtensionLibrary/Directory.Packages.props) e referenciados exclusivamente pelo projeto de benchmark.
- Os pacotes nucleares da biblioteca (`TL.ExtensionLibrary.Domain`, `Application`, `Infrastructure` e módulos base) permanecem com **zero dependências externas e pureza estrita BCL em .NET 8**.

### 3. Diagnóstico de Memória Mandatório (`[MemoryDiagnoser]`)

Todas as suítes de benchmark herdam de `BenchmarkBase` decorado com `[MemoryDiagnoser]`:
- O critério de avaliação não se restringe à latência aritmética (`Mean`), mas audita com rigor a coluna `Allocated` (bytes no Heap) e as coletas por geração (`Gen 0 / Gen 1 / Gen 2`).
- Assegura-se que rotinas de hot-path demonstrem explicitamente sua capacidade de aliviar a pressão sobre o Garbage Collector.

### 4. Transparência Técnica e Honestidade Intelectual (Trade-offs Explícitos)

A governança do projeto estabelece que a biblioteca deve relatar seus limites com honestidade técnica:
- **Zero Allocation vs Latência em Payloads Minúsculos:** Reconhece-se que em textos de 5 a 10 caracteres, o custo de alocação de `Substring` pode apresentar 1-2 ns a menos de CPU que o setup de buffers, mas ao custo de poluição de Heap. A biblioteca prioriza a estabilidade do GC em produção.
- **Pureza Binária:** A paridade funcional é entregue sem inflar a aplicação consumidora com árvores pesadas de dependência de terceiros.
- **Escala Algorítmica:** Demonstra-se que vantagens algorítmicas ($O(1)$ Keyset vs $O(N)$ Offset) são comprovadamente determinantes em paginações profundas e grandes volumes.

### 5. Cobertura de Micro-benchmarks das Extensões Utilitárias

A matriz foi expandida para 30 cenários comparativos, incorporando cenários dedicados para as novas extensões:
- `ToSnakeCase`: comparando varredura em passagem única via `ReadOnlySpan<char>` contra rotinas convencionais baseadas em `Regex`.
- `Partition`: confrontando o particionamento em passagem única $O(N)$ contra a dupla filtragem com `.Where()`.
- `ToEnum`: avaliando o parsing resiliente com fallback contra a captura de `ArgumentException` em `Enum.Parse`.
- `StartOfDay`: aferindo a construção direta de limites temporais com preservação de `DateTimeKind`.

---

## Consequências e Trade-offs

- **Evidência Empírica e Reproduzível:** Métricas documentadas e verificáveis por qualquer engenheiro de software executando o comando `dotnet run -c Release`.
- **Prevenção de Regressões:** O job dry permite testar a estabilidade de todos os benchmarks rapidamente durante o ciclo de integração.
- **Zero Contaminação de Produção:** Nenhuma dependência comparativa vaza para as bibliotecas finais.
- **Custo de Manutenção:** A inclusão de novas extensões de alta performance demandará a criação concomitante de cenários correspondentes na matriz de benchmarks.
