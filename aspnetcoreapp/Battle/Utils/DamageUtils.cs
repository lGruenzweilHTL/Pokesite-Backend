public static class DamageUtils
{
    public static int CalculateDamage(Pokemon attacker, Pokemon defender, Move move)
    {
        int level = attacker.Level;
        int critical = IsCriticalHit(attacker) ? 2 : 1;
        int power = move.Power;
        int a = move.Special ? attacker.Stats.SpecialAttack : attacker.Stats.Attack;
        int d = move.Special ? defender.Stats.SpecialDefense : defender.Stats.Defense;
        double stab = (move.Type == attacker.Types[0] || (attacker.Types.Count > 1 && move.Type == attacker.Types[1]))
            ? 1.5
            : 1.0;
        double type = TypeUtils.MoveEffectiveness(move, defender);

        // The formula for damage calculation is as follows:
        var calc1 = (2 * level * critical / 5) + 2;
        var calc2 = calc1 * power * a / d;
        var calc3 = calc2 / 50 + 2;
        var random = Math.Floor(new Random().NextDouble() * (255 - 217 + 1) + 217) / 255;
        var calc4 = calc3 * stab * type * random;

        return (int)calc4;
    }

    /*
    Whether a move scores a critical hit is determined by comparing a 1-byte random number (0 to 255) against a 1-byte threshold value (also 0 to 255).
    If the random number is strictly less than the threshold, the Pokémon scores a critical hit.

    If the threshold would exceed 255, it instead becomes 255. Consequently, the maximum possible chance of landing a critical hit is 255/256.
    (If the generated random number is 255, that number can never be less than the threshold, regardless of the value of the threshold.)

    In the Generation I core series games, the threshold is normally equal to half the user's base Speed.
    */
    private static bool IsCriticalHit(Pokemon attacker)
    {
        var threshold = Math.Min(attacker.Stats.Speed / 2, 255);
        var random = new Random().Next(0, 256);
        return random < threshold;
    }
}