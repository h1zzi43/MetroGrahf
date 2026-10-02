using MetroGrahf.Graph;

namespace MetroGrahf.Demo;

/// <summary>
/// Тестовые данные: условная схема метро из 3 линий.
/// </summary>
public static class DemoData
{
    public static MetroGraph BuildSampleMetro()
    {
        var g = new MetroGraph();

        // Красная линия
        g.AddStation("R1", "Северная");
        g.AddStation("R2", "Центральная");
        g.AddStation("R3", "Южная");

        // Синяя линия
        g.AddStation("B1", "Западная");
        g.AddStation("B2", "Центральная-2");
        g.AddStation("B3", "Восточная");

        // Зелёная линия
        g.AddStation("G1", "Парковая");
        g.AddStation("G2", "Университет");
        g.AddStation("G3", "Аэропорт");

        // Перегоны красной линии
        g.AddBidirectionalEdge("R1", "R2", 5);
        g.AddBidirectionalEdge("R2", "R3", 6);

        // Перегоны синей линии
        g.AddBidirectionalEdge("B1", "B2", 4);
        g.AddBidirectionalEdge("B2", "B3", 7);

        // Перегоны зелёной линии
        g.AddBidirectionalEdge("G1", "G2", 3);
        g.AddBidirectionalEdge("G2", "G3", 8);

        // Пересадки
        g.AddBidirectionalEdge("R2", "B2", 2);
        g.AddBidirectionalEdge("R3", "G1", 4);
        g.AddBidirectionalEdge("B3", "G3", 5);

        return g;
    }
}