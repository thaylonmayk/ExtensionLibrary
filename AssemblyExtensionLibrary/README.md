# AssemblyExtensionsLibrary

A biblioteca AssemblyExtensions fornece uma variedade de métodos de extensão para a classe `Assembly` do .NET, facilitando a manipulação e extração de informações úteis sobre assemblies. Com AssemblyExtensions, você pode obter facilmente informações de versão, tipos definidos, assemblies referenciados, e muito mais.

## Instalação

Para instalar a biblioteca AssemblyExtensions via NuGet, use o seguinte comando:

```sh
dotnet add package TL.AssemblyExtensionsLibrary
```

## Funcionalidades

- **FileInfo**: Obtém o objeto `FileInfo` para a localização do assembly especificado.
```bash
  var fileInfo = assembly.FileInfo()
```
- **GetVersion**: Obtém a versão do assembly.
```bash
var version = assembly.GetVersion();
```
- **GetReferencedAssemblies**: Obtém as assemblies referenciadas pelo assembly atual.
```bash
 
var referencedAssemblies = assembly.GetReferencedAssemblies();

```
- **GetTypes**: Obtém todos os tipos definidos no assembly.
```bash
 
var types = assembly.GetTypes();

```
- **GetCustomAttribute**: Obtém um atributo personalizado específico do assembly.
```bash
 
var customAttribute = assembly.GetCustomAttribute<YourAttribute>();

```
- **GetEntryAssembly**: Obtém a assembly de entrada (o executável do processo) na aplicação atual.
```bash
 
var entryAssembly = AssemblyExtensions.GetEntryAssembly();

```
- **GetName**: Obtém o nome do assembly.
```bash
 
var name = assembly.GetName();

```
- **GetLocation**: Obtém a localização do assembly.
```bash
 
var location = assembly.GetLocation();

```
- **GetManifestResourceNames**: Obtém os nomes dos recursos incorporados no assembly.
```bash
 
var resourceNames = assembly.GetManifestResourceNames();

```
- **GetDefinedNamespaces**: Obtém os namespaces definidos no assembly.
```bash
 
var namespaces = assembly.GetDefinedNamespaces();

```
- **IsDebugBuild**: Verifica se o assembly foi compilado em modo debug.
```bash
 
var isDebug = assembly.IsDebugBuild();

```

## Contribuições
Sinta-se à vontade para contribuir com este projeto. Faça um fork, crie uma branch com suas melhorias e abra um pull request!

## Licença
Este projeto está licenciado sob a Licença MIT - consulte o arquivo LICENSE para obter detalhes.

