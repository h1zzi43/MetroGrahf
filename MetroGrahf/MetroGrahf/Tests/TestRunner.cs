using MetroGrahf.Algorithms;
using MetroGrahf.Demo;
using MetroGrahf.Graph;
using MetroGrahf.Models;

namespace MetroGrahf.Tests;

/// <summary>
/// Простой самописный набор тестов, покрывающий основной сценарий
/// и пограничные случаи. Запуск: dotnet run -- --test
/// </summary>
public static class TestRunner
{
    private static int _passed;
    private static int _failed;

    public static int RunAll()
    {
        _passed = 0;
        _failed = 0;

        Test_ShortestRoute_MainScenario();
        Test_MinTransfers_MainScenario();
        Test_CaseInsensitiveInput();
        Test_StartEqualsFinish();
        Test_UnknownStation();
        Test_EmptyGraph();
        Test_DisconnectedGraph_NoRoute();
        Test_SingleStationGraph();

        Console.WriteLine();
        Console.WriteLine($"Пройдено: {_passed}, Провалено: {_failed}");
        return _failed == 0 ? 0 : 1;
    }

    private static void Test_ShortestRoute_MainScenario()
    {
        var g = DemoData.BuildSampleMetro();
        var r = Dijkstra.FindShortestRoute(g, "R1", "B3");

        Check("Dijkstra: маршрут R1->B3 найден", r.Found);
        Check("Dijkstra: R1->B3 путь начинается с R1", r.Path[0] == "R1");
        Check("Dijkstra: R1->B3 путь заканчивается B3", r.Path[^1] == "B3");
        Check("Dijkstra: R1->B3 время = 14", r.TotalMinutes == 14);
    }

    private static void Test_MinTransfers_MainScenario()
    {
        var g = DemoData.BuildSampleMetro();
        var r = BfsMinTransfers.FindMinTransfers(g, "R1", "G3");

        Check("BFS: маршрут R1->G3 найден", r.Found);
        Check("BFS: R1->G3 путь начинается с R1", r.Path[0] == "R1");
        Check("BFS: R1->G3 путь заканчивается G3", r.Path[^1] == "G3");
    }

    private static void Test_CaseInsensitiveInput()
    {
        var g = DemoData.BuildSampleMetro();
        var r = Dijkstra.FindShortestRoute(g, "r1", "b3");

        Check("Регистр: 'r1'->'b3' найден", r.Found);
        Check("Регистр: 'r1'->'b3' время = 14", r.TotalMinutes == 14);
    }

    private static void Test_StartEqualsFinish()
    {
        var g = DemoData.BuildSampleMetro();
        var r = Dijkstra.FindShortestRoute(g, "R1", "R1");

        Check("Пограничный: старт == финиш найден", r.Found);
        Check("Пограничный: старт == финиш время 0", r.TotalMinutes == 0);
        Check("Пограничный: старт == финиш путь из 1 станции", r.Path.Count == 1);
    }

    private static void Test_UnknownStation()
    {
        var g = DemoData.BuildSampleMetro();
        var r = Dijkstra.FindShortestRoute(g, "R1", "NOPE");

        Check("Пограничный: неизвестная станция -> не найден", !r.Found);
    }

    private static void Test_EmptyGraph()
    {
        var g = new MetroGraph();
        var r = Dijkstra.FindShortestRoute(g, "A", "B");

        Check("Пограничный: пустой граф -> не найден", !r.Found);
    }

    private static void Test_DisconnectedGraph_NoRoute()
    {
        var g = new MetroGraph();
        g.AddStation("A", "A");
        g.AddStation("B", "B");
        var r = Dijkstra.FindShortestRoute(g, "A", "B");

        Check("Пограничный: несвязный граф -> не найден", !r.Found);
    }

    private static void Test_SingleStationGraph()
    {
        var g = new MetroGraph();
        g.AddStation("A", "A");
        var r = Dijkstra.FindShortestRoute(g, "A", "A");

        Check("Пограничный: граф из 1 станции, старт==финиш", r.Found && r.TotalMinutes == 0);
    }

    private static void Check(string name, bool condition)
    {
        if (condition)
        {
            _passed++;
            Console.WriteLine($"[ OK ]  {name}");
        }
        else
        {
            _failed++;
            Console.WriteLine($"[FAIL]  {name}");
        }
    }
}