# ObjectExtensions

## Resumo

Este projeto fornece várias extensões úteis para a classe `object` no .NET, facilitando a manipulação e transformação de objetos.

## Instalação

```bash
# Clone o repositório
git clone <URL_DO_REPOSITORIO>

# Navegue até o diretório do projeto
cd ObjectExtensions

# Restaure os pacotes NuGet
dotnet restore

# Compile o projeto
dotnet build

```

## Funcionalidades

```bash
Bytes
# Exemplo de uso:
var bytes = myObject.Bytes();

Dictionary
# Exemplo de uso:
var dictionary = myObject.Dictionary();

GetPropertiesWithAttribute<T>
# Exemplo de uso:
var propertiesWithAttribute = myObject.GetPropertiesWithAttribute<MyAttribute>();

Serialize
# Exemplo de uso:
var jsonString = myObject.Serialize();

SetProperty
# Exemplo de uso:
myObject.SetProperty("PropertyName", value);

Clone
# Exemplo de uso:
var clonedObject = myObject.Clone();

PropertiesEqual
# Exemplo de uso:
bool areEqual = myObject.PropertiesEqual(otherObject);

ToExpando
# Exemplo de uso:
var expandoObject = myObject.ToExpando();

TryGetProperty
# Exemplo de uso:
bool found = myObject.TryGetProperty("PropertyName", out var value);

IsDefault
# Exemplo de uso:
bool isDefault = myObject.IsDefault();

ToJsonIndented
# Exemplo de uso:
var indentedJson = myObject.ToJsonIndented();

GetPropertyNames
# Exemplo de uso:
var propertyNames = myObject.GetPropertyNames();

InvokeMethod
# Exemplo de uso:
var result = myObject.InvokeMethod("MethodName", arg1, arg2);

AddOrUpdateProperty
# Exemplo de uso:
expandoObject.AddOrUpdateProperty("PropertyName", value);

IsPropertyDefined
# Exemplo de uso:
bool isDefined = myObject.IsPropertyDefined("PropertyName");

```

## Contribuições
Sinta-se à vontade para contribuir com este projeto. Faça um fork, crie uma branch com suas melhorias e abra um pull request!

## Licença

Este projeto está licenciado sob a Licença MIT - consulte o arquivo [LICENSE.md](./LICENSE.md) para obter detalhes.
