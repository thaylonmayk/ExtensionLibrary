# HttpClientExtensionsLibrary

A biblioteca `HttpClientExtensionsLibrary` fornece uma variedade de métodos de extensão para a classe `HttpClient` do .NET, facilitando a realização de requisições HTTP, a manipulação de respostas, a autenticação e a implementação de políticas de retry.

## Instalação

Para instalar a biblioteca `HttpClientExtensionsLibrary` via NuGet, use o seguinte comando:

```bash
dotnet add package TL.HttpClientExtensionsLibrary
```

## Funcionalidades

- **SendJsonAsync<T>**:

Descrição: Envia uma requisição HTTP com payload JSON e recebe a resposta como um objeto do tipo T.

Exemplo de Uso:
```
var response = await httpClient.SendJsonAsync<MyResponseType>("https://api.example.com/data", HttpMethod.Post, myPayload);
```
- **AddBearerToken**:

Descrição: Adiciona um token de autenticação Bearer aos cabeçalhos das requisições.

Exemplo de Uso:
```
httpClient.AddBearerToken("myBearerToken");
```
- **ExponentialBackoffRetryAsync**:

Descrição: Implementa uma política de retry com backoff exponencial.

Exemplo de Uso:
```
var response = await httpClient.ExponentialBackoffRetryAsync(request, retryCount: 3, baseDelayMilliseconds: 200);
```
- **ReadAsJsonAsync<T>**:

Descrição: Lê o conteúdo da resposta como um objeto JSON.

Exemplo de Uso:
```
var responseObject = await httpResponseMessage.ReadAsJsonAsync<MyResponseType>();
```
- **LogError**:

Descrição: Loga erros de requisição.

Exemplo de Uso:
```
try
{
    var response = await httpClient.LogError(request);
}
catch (Exception ex)
{
    Console.WriteLine($"Erro na requisição: {ex.Message}");
}
```
- **UploadFileAsync**:

Descrição: Envia um arquivo para o servidor via multipart/form-data.

Exemplo de Uso:
```
var response = await httpClient.UploadFileAsync("https://api.example.com/upload", "path/to/file.txt", "file");
```
- **DownloadFileAsync**:

Descrição: Faz o download de arquivos de uma URL e os salva no caminho especificado.

Exemplo de Uso:
```
await httpClient.DownloadFileAsync("https://api.example.com/file", "path/to/save/file.txt");
```
- **GetFileMetadataAsync**:

Descrição: Obtém os metadados de um arquivo a partir de uma URL.

Exemplo de Uso:
```
var metadata = await httpClient.GetFileMetadataAsync("https://api.example.com/file");
```
- **GetFileSizeAsync**:

Descrição: Obtém o tamanho de um arquivo a partir de uma URL.

Exemplo de Uso:
```
var fileSize = await httpClient.GetFileSizeAsync("https://api.example.com/file");
```
- **RetryPolicyAsync**:

Descrição: Configura uma política de retry básica.

Exemplo de Uso:
```
var response = await httpClient.RetryPolicyAsync(request, retryCount: 3);
```
- **HandleTransientErrorsAsync**:

Descrição: Lida com erros transitórios e re-tenta a requisição.

Exemplo de Uso:
```
var response = await httpClient.HandleTransientErrorsAsync(request, 
```



## Contribuições
Sinta-se à vontade para contribuir com este projeto. Faça um fork, crie uma branch com suas melhorias e abra um pull request!

## Licença
Este projeto está licenciado sob a Licença MIT - consulte o arquivo LICENSE para obter detalhes.

