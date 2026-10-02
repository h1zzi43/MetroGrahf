namespace MetroGrahf.Models;

/// <summary>
/// Результат поиска маршрута.
/// </summary>
public sealed class RouteResult
{
    public bool Found { get; }
    public int TotalMinutes { get; }
    public int Hops { get; }
    public IReadOnlyList<string> Path { get; }

    public RouteResult(bool found, int totalMinutes, int hops, IReadOnlyList<string> path)
    {
        Found = found;
        TotalMinutes = totalMinutes;
        Hops = hops;
        Path = path;
    }

    public static RouteResult NotFound() =>
        new(false, 0, 0, Array.Empty<string>());

    public override string ToString() =>
        Found
            ? $"Найден маршрут: {string.Join(" -> ", Path)} | время {TotalMinutes} мин, перегонов {Hops}"
            : "Маршрут не найден.";
}