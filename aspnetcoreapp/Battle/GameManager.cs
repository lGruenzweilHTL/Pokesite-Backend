/*
 * GameManager is a singleton service that is injected into the battle controller
 * A WebSocketHandler is injected into the GameManager to handle the client connections
 *
 * The GameManager stores all games with GUIDs
 * Each GameLoop stores all connected clients with GUIDs
 *
 * A client can choose to create a new battle, or join an existing one
 * When a client creates a new battle, it receives the GUID of the battle, as well of the client
 * When a client join a battle, it only receives the GUID of the client. Joining a battle requires its GUID.
 *
 * The client must include its GUID in each websocket request
 * When a battle ends, the websocket connection is closed and the GUID becomes useless
 */

public class GameManager(WebSocketHandler socketHandler) {
    public const int MAX_PLAYERS = 2;
    
    public Dictionary<string, GameLoop> ActiveGames = new();

    // Returns the GUID of the new game, to be returned to clients
    public string NewGame() {
        string guid = RandomUtils.Guid();
        GameLoop game = new(guid, MAX_PLAYERS, socketHandler);
        ActiveGames.Add(guid, game);
        
        return guid;
    }

    public bool TryJoinGame(string guid, Player p, out string playerGuid) {
        if (!ActiveGames.TryGetValue(guid, out GameLoop? value)) {
            playerGuid = "";
            return false;
        }
        
        playerGuid = RandomUtils.Guid();
        return value.ConnectPlayer(playerGuid, p);
    }

    public bool TryJoinAsBot(string guid, Player p, string? preferredBehaviour = null) {
        return ActiveGames.TryGetValue(guid, out GameLoop? value) 
               && value.ConnectBot(p, preferredBehaviour);
    }

    public bool StartGame(string guid, out GameLoop? game) {
        if (!ActiveGames.TryGetValue(guid, out game)) return false;

        game.StartWithWebSocket();
        return true;
    }
}