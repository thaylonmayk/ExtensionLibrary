# ADR 010: Decisões Arquiteturais do Pacote TL.QueryableExtensionsLibrary

---

## Contexto

O pacote `TL.QueryableExtensionsLibrary` estende `System.Linq.IQueryable<T>`, viabilizando a construção de consultas dinâmicas diretamente a partir de parâmetros textuais enviados por clientes de APIs REST (como filtros por coluna, ordenação parametrizada e paginação).

Por interagir diretamente com provedores LINQ (como Entity Framework Core e bancos de dados relacionais), o pacote é projetado para produzir Expression Trees limpas, tipadas e traduzíveis em consultas SQL eficientes.

---

## Decisões Arquiteturais

### 1. Construção Defensiva e Conversão Tipada de Filtros Dinâmicos
- Métodos de filtragem como `Filter(property, comparison, value)` validam previamente a existência da propriedade pública na entidade através de reflexão (`BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase`).
- A conversão de valores textuais para o tipo de destino suporta explicitamente `Guid`, `DateTime`, `DateTimeOffset`, `TimeSpan`, `Enum` e tipos anuláveis (`Nullable<T>`), tratando strings vazias ou em branco de forma defensiva para evitar falhas de runtime na geração de Expression Trees.

### 2. Ordenação Dinâmica Estática (Sem Uso de DLR)
- A ordenação dinâmica por nome de propriedade (`Order(property, ascending)`) constrói e invoca a chamada genérica `Queryable.OrderBy` / `OrderByDescending` através de Expression Trees compiladas estaticamente, dispensando o uso do Dynamic Language Runtime (`dynamic`). Isso elimina sobrecarga em tempo de execução e garante compatibilidade com compilação Native AOT.

### 3. Paginação com Proteção de Limites
- A extensão `Page(index, size)` valida limites mínimos (`index >= 1`, `size >= 1`) e protege contra estouros de cálculo em `(index - 1) * size`, delegando diretamente para os operadores `Skip` e `Take` do provedor LINQ subjacente para execução no próprio servidor de banco de dados.

### 4. Composição Fluente Condicional (`WhereIf`, `OrderByIf`)
- Disponibilização de predicados condicionais que aplicam regras de filtragem e ordenação somente quando uma condição booleana prévia for satisfeita, eliminando blocos encadeados de `if` na montagem de consultas em repositórios.

---

## Consequências e Trade-offs

- **Tradução Nativa para SQL:** Consultas dinâmicas traduzidas com total fidelidade para instruções `WHERE`, `ORDER BY` e `OFFSET/FETCH` pelo Entity Framework Core.
- **Segurança de Execução:** Validação restrita a propriedades públicas da entidade, prevenindo acesso a membros internos ou restritos.
- **Ampla Cobertura de Tipos:** Suporte transparente aos tipos mais comuns de domínio e banco de dados (`Guid`, `DateTime`, `Enum`, `int?`, `decimal?`), simplificando a integração direta com query parameters de APIs HTTP.
