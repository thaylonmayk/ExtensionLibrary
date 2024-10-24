# CollectionExtensions

## Descrição

A biblioteca `CollectionExtensions` fornece uma variedade de métodos de extensão para coleções em .NET, facilitando a manipulação e a realização de operações comuns em `IList`, `Dictionary`, `IEnumerable` e `HashSet`. Com `CollectionExtensions`, você pode adicionar, remover, pesquisar e ordenar elementos de forma mais eficiente.

## Instalação

Para instalar a biblioteca `CollectionExtensions` via NuGet, use o seguinte comando:

```bash
dotnet add package TL.CollectionExtensions
```

## Funcionalidades

### Para `IList`

- **AddRangeIfNotExists**
  - **Descrição**: Adiciona uma gama de itens à lista se eles ainda não existirem na lista.
  - **Exemplo de Uso**:
    ```csharp
    myList.AddRangeIfNotExists(newItems);
    ```

- **RemoveAll**
  - **Descrição**: Remove todos os itens da lista que correspondem ao predicado especificado.
  - **Exemplo de Uso**:
    ```csharp
    myList.RemoveAll(item => item.Condition);
    ```

- **FindAll**
  - **Descrição**: Encontra todos os elementos na lista que correspondem ao predicado especificado.
  - **Exemplo de Uso**:
    ```csharp
    var matches = myList.FindAll(item => item.Condition);
    ```

- **SortBy**
  - **Descrição**: Ordena a lista pelo seletor de chave especificado.
  - **Exemplo de Uso**:
    ```csharp
    myList.SortBy(item => item.Key);
    ```

### Para `Dictionary<TKey, TValue>`

- **AddRangeIfNotExists**
  - **Descrição**: Adiciona uma gama de pares chave/valor ao dicionário se as chaves não existirem no dicionário.
  - **Exemplo de Uso**:
    ```csharp
    myDictionary.AddRangeIfNotExists(newKeyValuePairs);
    ```

- **RemoveAll**
  - **Descrição**: Remove todos os pares chave/valor do dicionário que correspondem ao predicado especificado.
  - **Exemplo de Uso**:
    ```csharp
    myDictionary.RemoveAll(kvp => kvp.Value.Condition);
    ```

- **FindAll**
  - **Descrição**: Encontra todos os pares chave/valor no dicionário que correspondem ao predicado especificado.
  - **Exemplo de Uso**:
    ```csharp
    var matches = myDictionary.FindAll(kvp => kvp.Value.Condition);
    ```

- **SortBy**
  - **Descrição**: Ordena o dicionário pelo seletor de chave especificado.
  - **Exemplo de Uso**:
    ```csharp
    var sortedDictionary = myDictionary.SortBy(kvp => kvp.Key);
    ```

### Para `IEnumerable<T>`

- **DistinctBy**
  - **Descrição**: Retorna elementos distintos de uma sequência usando um seletor de chave especificado.
  - **Exemplo de Uso**:
    ```csharp
    var distinctItems = myEnumerable.DistinctBy(item => item.Key);
    ```

- **WhereIf**
  - **Descrição**: Filtra uma sequência de valores com base em um predicado se uma condição for verdadeira.
  - **Exemplo de Uso**:
    ```csharp
    var filteredItems = myEnumerable.WhereIf(condition, item => item.Condition);
    ```

    
## Contribuições
Sinta-se à vontade para contribuir com este projeto. Faça um fork, crie uma branch com suas melhorias e abra um pull request!

## Licença
Este projeto está licenciado sob a Licença MIT - consulte o arquivo LICENSE para obter detalhes.

