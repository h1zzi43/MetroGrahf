namespace MetroGrahf.Models;

/// <summary>
/// Ребро графа: перегон между двумя станциями с временем в пути (в минутах).
/// </summary>
public sealed class Edge
{
    public string FromId { get; }
    public string ToId { get; }
    public int TravelMinutes { get; }

    public Edge(string fromId, string toId, int travelMinutes)
    {
        if (string.IsNullOrWhiteSpace(fromId))
            throw new ArgumentException("FromId не может быть пустым.", nameof(fromId));
        if (string.IsNullOrWhiteSpace(toId))
            throw new ArgumentException("ToId не может быть пустым.", nameof(toId));
        if (travelMinutes <= 0)
            throw new ArgumentOutOfRangeException(nameof(travelMinutes),
                "Время перегона должно быть положительным.");

        FromId = fromId;
        ToId = toId;
        TravelMinutes = travelMinutes;
    }

    public override string ToString() => $"{FromId} -> {ToId} ({TravelMinutes} мин)";
}