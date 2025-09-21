using System.Collections.Generic;

public static class UnitList
{

    public static List<Unit> enemies = new()
    {
        new Unit("Moskito", 1, 1, 0),
        new Unit("Moskito", 1, 1, 0),
        new Unit("Moskito", 1, 1, 0),
        new Unit("Moskito", 1, 1, 0),
        new Unit("Moskito", 1, 1, 0),
    };

    public static List<Unit> bosses = new()
    {
        new Unit("Day1Boss", 2, 2, 0),
        new Unit("Day2Boss", 10, 10, 0),
        new Unit("Day3Boss", 100, 100, 0),
        new Unit("Day4Boss", 1000, 1000, 0),
        new Unit("Earth", 9999, 9999, 0),
    };
}
