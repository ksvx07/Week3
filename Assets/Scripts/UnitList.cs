using System.Collections.Generic;

public static class UnitList
{

    public static List<Unit> enemies = new()
    {
        new Unit("Moskito", 1, 1, 1),
    };

    public static List<Unit> bosses = new()
    {
        new Unit("Day1Boss", 2, 2, 1),
    };
}
