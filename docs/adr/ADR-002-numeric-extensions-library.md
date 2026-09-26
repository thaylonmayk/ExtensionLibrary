# ADR 002: Decisões Arquiteturais do Pacote TL.NumericExtensionsLibrary

---

## Contexto

O pacote `TL.NumericExtensionsLibrary` fornece métodos de extensão matemáticos, estatísticos e predicados numéricos para os tipos `int`, `double` e `decimal`. A biblioteca foi concebida com zero dependências externas (BCL pura), suportando `netstandard2.0`, `net5.0`, `net6.0` e `net8.0`.

O objetivo é fornecer uma camada utilitária confiável para regras de negócio, validações de integridade, cálculos de porcentagem, médias e teoria dos números (primos, divisores, fatoriais).

---

## Decisões Arquiteturais

### 1. Independência de Dependências e Zero Alocação
- Manter o pacote operando exclusivamente sobre tipos primitivos da Base Class Library (.NET BCL). As operações ocorrem diretamente na stack, garantindo tempo de execução determinístico e sem sobrecarga no Garbage Collector.

### 2. Rigor Matemático e Validações de Domínio
- **Soma de Algarismos (`DigitSum`):** O método opera sobre a decomposição individual dos dígitos do número em valor absoluto, garantindo precisão matemática para cálculos de checksum e regras de validação.
- **MDC e MMC (`GreatestCommonDivisor` e `GreatestCommonMultiple`):** As operações de máximo divisor comum e mínimo múltiplo comum incluem tratamento defensivo para denominadores e inputs nulos ou iguais a zero, evitando divisões por zero em tempo de execução.
- **Múltiplos com Divisor Seguro (`IsMultipleOf`):** Retorna `false` de forma defensiva quando o divisor for zero, evitando exceções não tratadas de divisão por zero.
- **Fatorial com Proteção contra Overflow:** A operação de fatorial calcula valores até 64 bits (`long`) com validação estrita de limites ($0 \le n \le 20$). Valores negativos ou acima de 20 disparam `ArgumentOutOfRangeException`, eliminando estouros aritméticos em tempo de execução.

### 3. Precisão Financeira em Cálculos Decimais
- Cálculos percentuais e médias ponderadas sobre `decimal` preservam estritamente o tipo de ponto fixo de alta precisão, evitando conversões intermediárias para ponto flutuante binário (`double`) que possam causar pequenas discrepâncias de arredondamento.

### 4. Evolução com Generic Math (`INumber<T>`)
- Para runtimes modernos (.NET 7 e .NET 8+), a suíte evolui para abstrações genéricas baseadas em `System.Numerics.INumber<T>`, reduzindo redundância de código e usufruindo de inlining nativo do JIT compiler.

### 5. Matemática Defensiva e Precisão Financeira
- **Divisão Defensiva (`SafeDivide`):** Oferece divisão segura com fallback configurável (padrão 0), eliminando exceções de divisão por zero em tempo de execução durante rotinas de fechamento, relatórios agregados e cálculos proporcionais.
- **Arredondamento Bancário (`RoundFinancial`):** Adota a estratégia de arredondamento para o par mais próximo (`MidpointRounding.ToEven`), alinhada às normas bancárias e contábeis para prevenir divergências acumulativas de centavos em operações monetárias.
- **Cálculo de Proporção Percentual (`CalculatePercentageOf`):** Provê utilitário expressivo para determinar a fração percentual que uma parte representa do todo, com tratamento defensivo quando a base total for zero ou inválida.
- **Avaliação de Faixas (`IsBetween`):** Implementação genérica inclusiva para tipos comparáveis com inversão automática de limites para evitar falhas silenciosas quando parâmetros forem invertidos.

---

## Consequências e Trade-offs

- **Previsibilidade:** Resultados matemáticos determinísticos com proteção contra exceções de domínio.
- **Eficiência:** Velocidade máxima de execução em processamento numérico de alta frequência.
- **Trade-off de Tipagem:** Métodos em `decimal` priorizam precisão exata para cenários financeiros, enquanto sobrecargas em `double` priorizam performance máxima para cálculos científicos e trigonométricos.
