using MetroGrahf.Graph;
using MetroGrahf.Models;

namespace MetroGrahf.Algorithms;

/// <summary>
/// Кратчайший по времени маршрут (алгоритм Дейкстры + приоритетная очередь).
/// Сложность: O((V + E) log V) по времени, O(V + E) по памяти.
/// </summary>
public static class Dijkstra
{
    public static RouteResult FindShortestRoute(MetroGraph graph, string startId, string finishId)
    {
        ArgumentNullException.ThrowIfNull(graph);

        if (!graph.ContainsStation(startId) || !graph.ContainsStation(finishId))
            return RouteResult.NotFound();

        // Приводим ввод к каноническому Id, хранящемуся в графе.
        var start = graph.GetStation(startId)!.Id;
        var finish = graph.GetStation(finishId)!.Id;

        if (string.Equals(start, finish, StringComparison.OrdinalIgnoreCase))
            return new RouteResult(true, 0, 0, new List<string> { start });

        var dist = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var prev = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        foreach (var s in graph.Stations)
        {
            dist[s.Id] = int.MaxValue;
            prev[s.Id] = null;
        }
        dist[start] = 0;

        var queue = new PriorityQueue<string, int>();
        queue.Enqueue(start, 0);

        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (!visited.Add(current)) continue;
            if (string.Equals(current, finish, StringComparison.OrdinalIgnoreCase)) break;

            foreach (var edge in graph.GetNeighbors(current))
            {
                var next = edge.ToId;
                var candidate = dist[current] + edge.TravelMinutes;

                if (candidate < dist[next])
                {
                    dist[next] = candidate;
                    prev[next] = current;
                    queue.Enqueue(next, candidate);
                }
            }
        }

        if (dist[finish] == int.MaxValue)
            return RouteResult.NotFound();

        var path = BuildPath(prev, start, finish);
        return new RouteResult(true, dist[finish], path.Count - 1, path);
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
}