using MetroGrahf.Graph;
using MetroGrahf.Models;

namespace MetroGrahf.Algorithms;

/// <summary>
/// Маршрут с минимальным числом перегонов (BFS по числу рёбер).
/// Сложность: O(V + E) по времени, O(V) по памяти.
/// </summary>
public static class BfsMinTransfers
{
    public static RouteResult FindMinTransfers(MetroGraph graph, string startId, string finishId)
    {
        ArgumentNullException.ThrowIfNull(graph);

        if (!graph.ContainsStation(startId) || !graph.ContainsStation(finishId))
            return RouteResult.NotFound();

        var start = graph.GetStation(startId)!.Id;
        var finish = graph.GetStation(finishId)!.Id;

        if (string.Equals(start, finish, StringComparison.OrdinalIgnoreCase))
            return new RouteResult(true, 0, 0, new List<string> { start });

        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { start };
        var prev = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase)
        {
            [start] = null
        };
        var queue = new Queue<string>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (string.Equals(current, finish, StringComparison.OrdinalIgnoreCase)) break;

            foreach (var edge in graph.GetNeighbors(current))
            {
                var next = edge.ToId;
                if (visited.Contains(next)) continue;

                visited.Add(next);
                prev[next] = current;
                queue.Enqueue(next);
            }
        }

        if (!visited.Contains(finish))
            return RouteResult.NotFound();

        var path = BuildPath(prev, start, finish);
        var totalMinutes = ComputeMinutes(graph, path);
        return new RouteResult(true, totalMinutes, path.Count - 1, path);
    }

    private static List<string> BuildPath(
        Dictionary<string, string?> prev, string startId, string finishId)
    {
        var path = new LinkedList<string>();
        string? cur = finishId;
        while (cur != null)
        {
            path.AddFirst(cur);
            if (string.Equals(cur, startId, StringComparison.OrdinalIgnoreCase)) break;
            cur = prev[cur];
        }
        return path.ToList();
    }

    private static int ComputeMinutes(MetroGraph graph, List<string> path)
    {
        var total = 0;
        for (int i = 0; i < path.Count - 1; i++)
        {
            var from = path[i];
            var to = path[i + 1];
            var edge = graph.GetNeighbors(from).FirstOrDefault(e => e.ToId == to);
            if (edge != null) total += edge.TravelMinutes;
        }
        return total;
    }
}