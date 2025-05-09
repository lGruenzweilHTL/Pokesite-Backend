namespace aspnetcoreapp.Battle.Utils;

public static class TypeUtils
{
    public static double MoveEffectiveness(Move move, Pokemon defender)
    {
        return MoveEffectiveness(move.Type, defender.Types[0])
               * defender.Types.Count > 1
                   ? MoveEffectiveness(move.Type, defender.Types[1])
                   : 1;
    }
    public static double MoveEffectiveness(string moveType, string defenderType)
    {
        throw new NotImplementedException();
    }
}