# QueryableExtensions

Este projeto fornece várias extensões úteis para a interface `IQueryable` no .NET, facilitando a manipulação e filtragem de dados.


## Instalação

Você pode instalar a biblioteca via NuGet. No seu terminal, execute:

```bash
dotnet add package TL.QueryableExtensionsLibrary
```

## Funcionalidades

```bash
ClaimRoles
# Exemplo de uso:
var roles = user.ClaimsPrincipal.ClaimRoles();

Claims
# Exemplo de uso:
var emails = user.ClaimsPrincipal.Claims("email");

Roles<T>
# Exemplo de uso:
var roleEnums = user.ClaimsPrincipal.Roles<MyEnum>();

ClaimSub
# Exemplo de uso:
var sub = user.ClaimsPrincipal.ClaimSub();

Claim
# Exemplo de uso:
var claim = user.ClaimsPrincipal.Claim("email");

Id
# Exemplo de uso:
var userId = user.ClaimsPrincipal.Id();

RolesFlag<T>
# Exemplo de uso:
var roleFlag = user.ClaimsPrincipal.RolesFlag<MyEnum>();

HasRole
# Exemplo de uso:
var hasAdminRole = user.ClaimsPrincipal.HasRole("Admin");

Email
# Exemplo de uso:
var email = user.ClaimsPrincipal.Email();

FullName
# Exemplo de uso:
var fullName = user.ClaimsPrincipal.FullName();

IsAuthenticated
# Exemplo de uso:
var isAuthenticated = user.ClaimsPrincipal.IsAuthenticated();

Birthdate
# Exemplo de uso:
var birthdate = user.ClaimsPrincipal.Birthdate();

AllClaims
# Exemplo de uso:
var allClaims = user.ClaimsPrincipal.AllClaims();

Filter
# Exemplo de uso:
var filtered = data.Filter("Name", "John");

Order
# Exemplo de uso:
var ordered = data.Order("Name", true);

Page
# Exemplo de uso:
var paged = data.Page(1, 10);

GroupBy
# Exemplo de uso:
var grouped = data.GroupBy<string, string>("Category");

Count
# Exemplo de uso:
var count = data.Count(x => x.Age > 30);

Sum
# Exemplo de uso:
var total = data.Sum("Price");

Min
# Exemplo de uso:
var minimum = data.Min("Price");

Max
# Exemplo de uso:
var maximum = data.Max("Price");

DistinctBy
# Exemplo de uso:
var distinctItems = data.DistinctBy<string, string>("Category");

Any
# Exemplo de uso:
var anyAdults = data.Any(x => x.Age >= 18);

Average
# Exemplo de uso:
var averagePrice = data.Average("Price");

ToDictionary
# Exemplo de uso:
var dictionary = data.ToDictionary<string, string, string>("Id", "Name");

FirstOrDefault
# Exemplo de uso:
var first = data.FirstOrDefault(x => x.Name == "John");

```

### Filter

```csharp
public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, string property, object value)
public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, string property, string comparison, object value)

## Contribuições
Sinta-se à vontade para contribuir com este projeto. Faça um fork, crie uma branch com suas melhorias e abra um pull request!

## Licença
Este projeto está licenciado sob a Licença MIT - consulte o arquivo LICENSE.md para obter detalhes.