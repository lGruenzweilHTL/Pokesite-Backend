public static class RandomUtils {
    private static Random _rng = new();

    public static int FromRange(int min, int maxExclusive) {
        return _rng.Next(min, maxExclusive);
    }

    public static bool Chance(int percent) {
        return FromRange(0, 100) < percent;
    }

    public static T RandomElement<T>(this IEnumerable<T> collection, int minIndex = 0, int maxIndex = -1) {
        IEnumerable<T> enumerable = collection as T[] ?? collection.ToArray(); // enumerate only once for performance
        if (maxIndex == -1) maxIndex = enumerable.Count();
        return enumerable.ElementAt(FromRange(minIndex, maxIndex));
    }

    public static Move RandomMove(this Pokemon p) {
        return p.Moves.RandomElement();
    }

    public static string Guid() {
        return System.Guid.NewGuid().ToString();
    }
    
    // not really a 'random' util, but can't be bothered to make a new class
    public static T NextElement<T>(this IEnumerable<T> collection, int index) {
        IEnumerable<T> enumerable = collection as T[] ?? collection.ToArray(); // enumerate only once for performance
        return enumerable.ElementAt((index + 1) % enumerable.Count());
    }
    public static T PreviousElement<T>(this IEnumerable<T> collection, int index) {
        IEnumerable<T> enumerable = collection as T[] ?? collection.ToArray(); // enumerate only once for performance
        return enumerable.ElementAt((index + enumerable.Count() - 1) % enumerable.Count());
    }
}