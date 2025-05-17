using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class BattleController(IPokemonService pokemonService, GameManager gameManager) : ControllerBase
{
    [HttpGet("create")]
    public IActionResult CreateBattle() {
        string battleGuid = gameManager.NewGame();

        CreateBattleResponse response = new() {
            BattleGuid = battleGuid
        };
        return Ok(response.ToJson());
    }

    [HttpPost("join/{guid}")]
    public async Task<IActionResult> JoinBattle(string guid, [FromBody] JsonElement request) {
        Player player = await CreateHumanPlayerAsync(request);
        bool success = gameManager.TryJoinGame(guid, player, out string playerGuid);

        if (!success) {
            return BadRequest($"The game with guid: {guid} does not exist.");
        }

        JoinBattleResponse response = new() {
            PlayerGuid = playerGuid
        };
        return Ok(response.ToJson());
    }

    [HttpPost("join-bot/{guid}")]
    public async Task<IActionResult> JoinBot(string guid, [FromBody] JsonElement request) {
        (Player player, string? behaviour) = await CreateBotPlayerAsync(request);
        bool success = gameManager.TryJoinAsBot(guid, player, behaviour);

        if (!success) {
            return BadRequest($"The game with guid: {guid} does not exist.");
        }

        return Ok();
    }

    [HttpGet("start/{guid}")]
    public IActionResult StartBattle(string guid) {
        bool success = gameManager.StartGame(guid, out GameLoop? game);

        if (!success) {
            return BadRequest($"The game with guid: {guid} does not exist.");
        }

        StartBattleResponse response = new() {
            WebsocketUrl = "http://localhost:8080/ws/",
            Players = game!.ConnectedPlayers.Select(p => new ResponsePlayerData {
                Hp = p.Value.CurrentPokemon.CurrentHp,
                Pokemon = p.Value.CurrentPokemon.Name,
                Guid = p.Key,
                IsBot = p.Value.IsBot
            }).ToArray()
        };
        return Ok(response.ToJson());
    }
    
    [HttpPost("start/new")]
    public async Task<IActionResult> StartNewBattle([FromBody] JsonElement request) {
        string battleGuid = gameManager.NewGame();

        var players = new List<(Player p, string? behaviour)>();
        foreach (var json in request.GetProperty("players").EnumerateArray()) {
            players.Add(await CreatePlayerAsync(json));
        }

        bool success = true;
        string[] guids = Enumerable.Repeat("n. a.", players.Count).ToArray();

        for (int i = 0; i < players.Count; i++) {
            (Player? p, string? behaviour) = players[i];
            
            if (p.IsBot) success &= gameManager.TryJoinAsBot(battleGuid, p, behaviour);
            else success &= gameManager.TryJoinGame(battleGuid, p, out guids[i]);
        }

        success &= gameManager.StartGame(battleGuid, out GameLoop? game);
        
        if (!success || game == null) {
            return BadRequest("Something went wrong while creating battle with guid: " + battleGuid);
        }

        StartNewBattleResponse response = new() {
            BattleGuid = battleGuid,
            WebsocketUrl = "http://localhost:8080/ws/",
            Players = players.Select((p, i) => new ResponsePlayerData {
                Guid = guids[i],
                Hp = p.p.CurrentPokemon.CurrentHp,
                Pokemon = p.p.CurrentPokemon.Name,
                IsBot = p.p.IsBot
            }).ToArray()
        };
        return Ok(response.ToJson());
    }
    
    [HttpGet("active")]
    public IActionResult GetAllActiveBattles() {
        var battles = gameManager.ActiveGames
            .Select(g => new {
                battle_guid = g.Key,
                players = g.Value.ConnectedPlayers.Values.Select(p => p.Name).ToArray(),
                bots = g.Value.ConnectedBots.Select(p => $"{p.p.Name} ({p.behaviour})").ToArray(),
                state = g.Value.GameState.ToString()
            });
        return Ok(JsonSerializer.Serialize(battles));
    }

    // Requires the json to have the 'human' property
    private async Task<(Player p, string? behaviour)> CreatePlayerAsync(JsonElement json) {
        bool human = json.GetProperty("human").GetBoolean();
        if (!human) return await CreateBotPlayerAsync(json);
        
        return (await CreateHumanPlayerAsync(json), null);
    }
    private async Task<Player> CreateHumanPlayerAsync(JsonElement json) {
        Pokemon[] team = await Task.WhenAll(
            json.GetProperty("pokemon")!
                .EnumerateArray()!
                .Select(async p => await pokemonService.GetPokemonWithMovesByNameAsync(
                    p.GetProperty("name").GetString()!,
                    p.GetProperty("moves").EnumerateArray().Select(m => m.GetString()!).ToArray()!
                ))
                .Select(Task<Pokemon> (p) => p!)
        );
        
        
        Player player = new(
            json.GetProperty("name").GetString()!,
            false,
            team);

        return player;
    }

    private async Task<(Player p, string? behaviourName)> CreateBotPlayerAsync(JsonElement json) {
        Player p = await CreateHumanPlayerAsync(json);
        p.IsBot = true;
        bool behaviourSpecified = json.TryGetProperty("behaviour", out JsonElement value);
        return (p, behaviourSpecified ? value.GetString() : null);
    }
}