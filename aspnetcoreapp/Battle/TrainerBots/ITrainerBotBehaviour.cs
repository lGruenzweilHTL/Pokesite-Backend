public interface ITrainerBotBehaviour {
    GameAction ChooseAction(Pokemon[] team, Pokemon[] opponentTeam, int teamCurrentActive, int opponentCurrentActive);
}