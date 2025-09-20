using System.Collections.Generic;

public static class UnitList
{

    public static List<Unit> enemies = new()
    {
        new Unit("Moskito", 1, 1, 1),
        new Unit("Moskito", 1, 1, 1),
        new Unit("Moskito", 1, 1, 1),
        new Unit("Moskito", 1, 1, 1),
        new Unit("Moskito", 1, 1, 1),
    };

    public static List<Unit> bosses = new()
    {
        new Unit("Day1Boss", 2, 2, 1),
        new Unit("Day2Boss", 10, 10, 1),
        new Unit("Day3Boss", 100, 100, 1),
        new Unit("Day4Boss", 1000, 1000, 1),
        new Unit("Earth", 9999, 9999, 1),
    };
}
