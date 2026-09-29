using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using AssemblyExtensionLibrary;
using ClaimsPrincipalExtensionsLibrary;
using CollectionExtensionsLibrary;
using DateTimeExtensionsLibrary;
using EnumExtensionsLibrary;
using NumericExtensionLibrary;
using ObjectExtensionsLibrary;
using StringExtensionLibrary;

namespace ExtensionLibrary.Showcase;

public enum OrderPriority
{
    [Description("Baixa Prioridade")]
    [EnumDescription(1, "Prioridade Nível 1 - SLA 72h")]
    Low = 1,

    [Description("Prioridade Padrão")]
    [EnumDescription(2, "Prioridade Nível 2 - SLA 24h")]
    Standard = 2,

    [Description("Alta Prioridade")]
    [EnumDescription(3, "Prioridade Nível 3 - SLA 4h")]
    High = 3
}

public class OrderModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        DemonstrateKeysetPagination();
        DemonstrateClaimsPrincipal();
        DemonstrateDateTimeExtensions();
        DemonstrateStringExtensions();
        DemonstrateNumericExtensions();
        await DemonstrateCollectionExtensionsAsync();
        DemonstrateEnumExtensions();
        DemonstrateObjectExtensions();
    }

    private static void DemonstrateKeysetPagination()
    {
        var orders = Enumerable.Range(1, 25).Select(i => new OrderModel
        {
            Id = i,
            Title = $"Pedido #{i}",
            Amount = i * 15.50m
        }).ToList();

        var firstPage = orders.ToKeysetPagedList(o => o.Id, pageSize: 5);
        Console.WriteLine($"[Keyset] Pagina 1: {firstPage.Items.Count} itens | HasNext: {firstPage.HasNextPage} | NextCursor: {firstPage.NextCursor}");

        var secondPage = orders.ToKeysetPagedList(o => o.Id, cursor: firstPage.NextCursor, pageSize: 5, SeekDirection.Forward);
        Console.WriteLine($"[Keyset] Pagina 2: {secondPage.Items.Count} itens | Primeiro ID: {secondPage.Items.First().Id} | Ultimo ID: {secondPage.Items.Last().Id}");
    }

    private static void DemonstrateClaimsPrincipal()
    {
        var userId = Guid.NewGuid();
        var identity = new ClaimsIdentity(new[]
        {
            new Claim("sub", userId.ToString()),
            new Claim("email", "dev@empresa.com"),
            new Claim("role", "Administrator"),
            new Claim("role", "Developer")
        });
        var principal = new ClaimsPrincipal(identity);

        var parsedGuid = principal.GetUserId<Guid>();
        var parsedLong = principal.GetUserIdOrDefault<long>(defaultValue: -1L);
        Console.WriteLine($"[Claims] Sub: {principal.ClaimSub()} | Email: {principal.Email()} | Guid: {parsedGuid} | LongFallback: {parsedLong}");
    }

    private static void DemonstrateDateTimeExtensions()
    {
        var startDate = new DateTime(2026, 9, 1);
        var targetDate = startDate.AddBusinessDays(5);
        int businessDays = startDate.BusinessDaysBetween(targetDate);
        var utcDate = startDate.EnsureUtc();
        long epoch = utcDate.ToUnixTimeMilliseconds();
        var dayStart = startDate.StartOfDay();
        var dayEnd = startDate.EndOfDay();
        var dateOnly = startDate.ToDateOnly();

        Console.WriteLine($"[DateTime] Inicio: {startDate:yyyy-MM-dd} | +5 dias uteis: {targetDate:yyyy-MM-dd} | Total dias uteis: {businessDays} | Epoch ms: {epoch}");
        Console.WriteLine($"[DateTime] StartOfDay: {dayStart:yyyy-MM-dd HH:mm:ss.fff} | EndOfDay: {dayEnd:yyyy-MM-dd HH:mm:ss.fff} | DateOnly: {dateOnly}");
    }

    private static void DemonstrateStringExtensions()
    {
        string text = "ExtensionLibrary fornece utilitarios para .NET";
        string truncated = text.Truncate(20);
        string sanitized = "log\r\ninjection".SanitizeForLog();
        string maskedEmail = "usuario.corporativo@empresa.com".MaskEmail();
        var jsonDict = "{\"servico\":\"api\",\"timeout\":30}".JsonToDictionary();
        string snakeCase = "CustomerBillingAddress".ToSnakeCase();
        string kebabCase = "OrderItemDetail".ToKebabCase();
        string slug = "C# 12 e .NET 8 em Producao!".ToSlug();

        Console.WriteLine($"[String] Truncate: '{truncated}' | Sanitize: '{sanitized}' | MaskEmail: '{maskedEmail}' | JsonDict: servico={jsonDict["servico"]}, timeout={jsonDict["timeout"]}");
        Console.WriteLine($"[String] SnakeCase: '{snakeCase}' | KebabCase: '{kebabCase}' | Slug: '{slug}'");
    }

    private static void DemonstrateNumericExtensions()
    {
        decimal a = 100m;
        decimal b = 0m;
        decimal safeDivision = a.SafeDivide(b, fallback: -1m);
        decimal rounded = 12.345m.RoundFinancial(2);
        decimal percentage = 25m.CalculatePercentageOf(200m);
        bool between = 15.IsBetween(10, 20);
        int digitSum = 9876.DigitSum();

        Console.WriteLine($"[Numeric] SafeDivide(100/0): {safeDivision} | RoundFinancial(12.345): {rounded} | 25 em 200: {percentage}% | 15 entre [10,20]: {between} | DigitSum(9876): {digitSum}");
    }

    private static async Task DemonstrateCollectionExtensionsAsync()
    {
        var numbers = new List<int> { 1, 2, 2, 3, 4, 4, 5, 6, 7, 8 };
        bool isEmpty = numbers.IsNullOrEmpty();
        var distinct = numbers.DistinctBy(x => x).ToList();
        var shuffled = distinct.Shuffle().ToList();
        var (evens, odds) = numbers.Partition(x => x % 2 == 0);

        int processedCount = 0;
        await numbers.ForEachAsync((item, ct) =>
        {
            Interlocked.Increment(ref processedCount);
            return Task.CompletedTask;
        }, maxDegreeOfParallelism: 2);

        Console.WriteLine($"[Collection] IsNullOrEmpty: {isEmpty} | DistinctCount: {distinct.Count} | Shuffled: [{string.Join(", ", shuffled)}]");
        Console.WriteLine($"[Collection] Partition -> Pares: [{string.Join(", ", evens)}] | Impares: [{string.Join(", ", odds)}] | ForEachAsync: {processedCount} itens");
    }

    private static void DemonstrateEnumExtensions()
    {
        var priority = OrderPriority.High;
        string defaultDescription = priority.GetDescription();
        string contextualDescription = priority.GetDescription(3);
        var parsed = "Standard".ToEnum(OrderPriority.Low);
        var dictionary = EnumExtension.ToDictionary<OrderPriority>();

        Console.WriteLine($"[Enum] Descricao: '{defaultDescription}' | Contextual: '{contextualDescription}'");
        Console.WriteLine($"[Enum] ParsedToEnum: '{parsed}' | DictionaryCount: {dictionary.Count}");
    }

    private static void DemonstrateObjectExtensions()
    {
        OrderModel? nullOrder = null;
        var clonedNull = nullOrder.Clone();

        var original = new OrderModel { Id = 42, Title = "Original", Amount = 199.90m };
        var clone = original.Clone();
        clone.Title = "Clonado";

        Console.WriteLine($"[Object] NullClone: {clonedNull is null} | Original: '{original.Title}' | Clone: '{clone.Title}'");
    }
}

