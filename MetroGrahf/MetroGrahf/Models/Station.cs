namespace MetroGrahf.Models;

/// <summary>
/// Станция метро. Идентифицируется уникальным строковым Id.
/// </summary>
public sealed class Station
{
    public string Id { get; }
    public string Name { get; }

    public Station(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Id станции не может быть пустым.", nameof(id));

        Id = id;
        Name = string.IsNullOrWhiteSpace(name) ? id : name;
    }

    public override string ToString() => $"{Name} ({Id})";
}