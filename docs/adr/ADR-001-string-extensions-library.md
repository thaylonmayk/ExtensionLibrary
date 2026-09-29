# ADR 001: Decisões Arquiteturais do Pacote TL.StringExtensionsLibrary

---

## Contexto

O pacote `TL.StringExtensionsLibrary` provê métodos utilitários de extensão para o tipo fundamental `System.String`. Por lidar diretamente com dados de entrada de usuários e integração com sistemas externos (APIs, mensageria, arquivos CSV), o pacote desempenha um papel central na garantia de integridade, facilidade de uso e alta performance para microsserviços e bibliotecas em C# / .NET.

A biblioteca é compatível com múltiplos runtimes (`netstandard2.0`, `net5.0`, `net6.0` e `net8.0`).

---

## Decisões Arquiteturais

### 1. Pureza Funcional e Imutabilidade
- Operações de extensão sobre strings produzem sempre novas instâncias ou estruturas imutáveis, garantindo ausência de efeitos colaterais e segurança para concorrência multithread.
- Validação defensiva de nulidade no primeiro parâmetro (`this string source`), exceto em métodos cujo objetivo expresso seja checar estados nulos ou em branco (como `IsNullOrEmpty()` ou `IsNullOrWhiteSpace()`).

### 2. Conversões e Parsing com Padrão Try/Default
- Para fluxos de dados com formato imprevisível, o pacote prioriza sobrecargas seguras:
  - `TryToInt(out int result)`: Permite controle de fluxo idiomático em checagens booleanas.
  - `ToIntOrDefault(int defaultValue = 0)`: Permite extração rápida com valor de fallback sem interrupção por exceções de formato.

### 3. Otimização de Memória e Zero-Allocation via `ReadOnlySpan<char>`
- Métodos de fatiamento e truncamento (`Left`, `Right`, `Trim`, `TruncateWithEllipsis`) são estruturados para priorizar o uso de `ReadOnlySpan<char>`, minimizando alocações intermediárias na Heap e melhorando o throughput da aplicação consumidora.

### 4. Resiliência e Prevenção de DoS em Expressões Regulares
- Operações que utilizam expressões regulares (`Regex`) definem `TimeSpan matchTimeout` explícito, protegendo aplicações contra travamentos de CPU decorrentes de padrões de texto patológicos.

### 5. Proteção de Dados Sensíveis e Sanitização de Logs
- **Prevenção contra Injeção de Logs e Quebras de Linha:** O método `SanitizeForLog()` neutraliza quebras de linha (`\r`, `\n`) substituindo-as por underscores (`_`), impedindo falsificação de registros em auditorias e logs estruturados.
- **Mascaramento de Dados Pessoais Sensíveis:** O método `MaskEmail()` ofusca dados pessoais identificáveis preservando unicamente os caracteres limítrofes do identificador e o domínio corporativo (ex: `t*****n@empresa.com`). O método `Mask()` viabiliza ofuscação flexível de cartões, documentos e tokens com salvaguarda estrita contra exceções de limites de array (`IndexOutOfRangeException`).
- **Truncamento e Decodificação Defensiva:** `TruncateWithEllipsis()` garante que o comprimento resultante nunca ultrapasse a cota máxima estipulada, enquanto `TryFromBase64()` elimina exceções de formato (`FormatException`) em processamento de fluxos externos.

### 6. Conversões de Nomenclatura e Normalização de Slugs
- **Transformação de Nomenclatura:** Os métodos `ToSnakeCase()` e `ToKebabCase()` convertem identificadores PascalCase e camelCase com varredura linear em passagem única, tratando adequadamente sequências de maiúsculas contíguas (ex.: `APIResponse` -> `api_response`) sem dependência de expressões regulares.
- **Normalização de Slugs para Rotas e Identificadores:** O método `ToSlug()` sanitiza textos para URLs amigáveis através de decomposição Unicode (`NormalizationForm.FormD`) para remoção de acentos e diacríticos, substituição de pontuações por hífens e remoção de hífens duplicados.

---

## Consequências e Trade-offs

- **Robustez:** Alta tolerância a variações de payloads e formatos de entrada sem risco de falhas não tratadas.
- **Conformidade de Segurança:** Blindagem contra injeção de logs e vazamento de dados sensíveis em logs operacionais.
- **Performance:** Eficiência de memória ao processar grandes volumes de texto através de spans e algoritmos lineares sem Regex no hot-path.
- **Compatibilidade:** O suporte a `netstandard2.0` em conjunto com APIs modernas do .NET 8+ é viabilizado de forma transparente por diretivas de compilação condicional, preservando a interoperabilidade da biblioteca.

