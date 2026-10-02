using MetroGrahf.Algorithms;
using MetroGrahf.Demo;
using MetroGrahf.Models;
using MetroGrahf.Tests;

namespace MetroGrahf;

public static class Program
{
    public static int Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "--test")
            return TestRunner.RunAll();

        return RunInteractiveDemo();
    }

    private static int RunInteractiveDemo()
    {
        var graph = DemoData.BuildSampleMetro();

        Console.WriteLine("=== Планировщик маршрутов в метро ===");
        Console.WriteLine($"Станций в графе: {graph.StationCount}");
        Console.WriteLine();
        Console.WriteLine("Доступные станции:");
        foreach (var s in graph.Stations)
            Console.WriteLine($"  {s.Id} — {s.Name}");
        Console.WriteLine();

        Console.Write("Введите Id станции отправления: ");
        var start = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Введите Id станции назначения: ");
        var finish = Console.ReadLine()?.Trim() ?? "";

        Console.WriteLine();
        Console.WriteLine("--- Кратчайший по времени маршрут (Дейкстра) ---");
        PrintResult(Dijkstra.FindShortestRoute(graph, start, finish));

        Console.WriteLine();
        Console.WriteLine("--- Маршрут с минимумом перегонов (BFS) ---");
        PrintResult(BfsMinTransfers.FindMinTransfers(graph, start, finish));

        return 0;
    }

    private static void PrintResult(RouteResult result)
    {
        if (!result.Found)
        {
            Console.WriteLine("Маршрут не найден.");
            return;
        }

        Console.WriteLine($"Путь: {string.Join(" -> ", result.Path)}");
        Console.WriteLine($"Время: {result.TotalMinutes} мин");
        Console.WriteLine($"Перегонов: {result.Hops}");
    }
}