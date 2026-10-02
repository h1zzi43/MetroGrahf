using MetroGrahf.Models;

namespace MetroGrahf.Graph;

/// <summary>
/// Граф метро в виде списка смежности.
/// Перегоны двунаправленные, поэтому добавляем оба направления.
/// Словари созданы с StringComparer.OrdinalIgnoreCase, чтобы Id станций
/// можно было вводить в любом регистре ("r1" == "R1").
/// </summary>
public sealed class MetroGraph
{
    private readonly Dictionary<string, Station> _stations =
        new(StringComparer.OrdinalIgnoreCase);

    private readonly Dictionary<string, List<Edge>> _adjacency =
        new(StringComparer.OrdinalIgnoreCase);

    public int StationCount => _stations.Count;
    public IReadOnlyCollection<Station> Stations => _stations.Values;

    public void AddStation(string id, string name)
    {
        if (_stations.ContainsKey(id)) return;
        _stations[id] = new Station(id, name);
        _adjacency[id] = new List<Edge>();
    }

    public void AddBidirectionalEdge(string aId, string bId, int travelMinutes)
    {
        EnsureStationExists(aId);
        EnsureStationExists(bId);

        _adjacency[aId].Add(new Edge(aId, bId, travelMinutes));
        _adjacency[bId].Add(new Edge(bId, aId, travelMinutes));
    }

    public IReadOnlyList<Edge> GetNeighbors(string stationId)
    {
        EnsureStationExists(stationId);
        return _adjacency[stationId];
    }

    public bool ContainsStation(string id) => _stations.ContainsKey(id);

    public Station? GetStation(string id) =>
        _stations.TryGetValue(id, out var s) ? s : null;

    private void EnsureStationExists(string id)
    {
        if (!_stations.ContainsKey(id))
            throw new KeyNotFoundException($"Станция '{id}' отсутствует в графе.");
    }
}