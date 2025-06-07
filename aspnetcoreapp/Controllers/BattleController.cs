using System.Text.Json;
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
        bool success = gameManager.TryJoinGame(guid, player, out Guid playerGuid);

        if (!success) {
            return BadRequest($"The game with guid: {guid} does not exist.");
        }
        
        JoinBattleResponse response = new() {
            WebsocketUrl = "http://localhost:8080/ws?guid=" + playerGuid
        };
        return Ok(response);
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
        bool success = gameManager.StartGame(guid, out _);

        if (!success) {
            return BadRequest($"The game with guid: {guid} does not exist.");
        }
        
        return Ok();
    }
    
    [HttpPost("start/bot")]
    public async Task<IActionResult> StartBotMatch([FromBody] JsonElement request) {
        string battleGuid = gameManager.NewGame();

        Player player = await CreateHumanPlayerAsync(request.GetProperty("player"));
        (Player bot, string? behaviourName) = await CreateBotPlayerAsync(request.GetProperty("bot"));

        bool success = gameManager.TryJoinGame(battleGuid, player, out Guid playerGuid)
                       && gameManager.TryJoinAsBot(battleGuid, bot, behaviourName)
                       && gameManager.StartGame(battleGuid, out _);
            
        if (!success) {
            return BadRequest($"Something went wrong while creating the game with guid: {battleGuid}.");
        }
        
        StartBotMatchResponse response = new() {
            WebsocketUrl = "http://localhost:8080/ws?guid=" + playerGuid,
            BattleGuid = battleGuid,
        };
        return Ok(response.ToJson());
    }
    
    [HttpGet("active")]
    public IActionResult GetAllActiveBattles() {
        var battles = gameManager.ActiveGames
            .Select(g => new {
                battle_guid = g.Key,
                players = g.Value.ConnectedPlayers
                    .Select(pair => $"{pair.p.Name} ({pair.guid})")
                    .ToArray(),
                bots = g.Value.ConnectedBots
                    .Select(p => $"{p.p.Name} ({p.behaviour})")
                    .ToArray(),
                state = g.Value.GameState.ToString()
            });
        return Ok(JsonSerializer.Serialize(battles));
    }
    
    private async Task<Player> CreateHumanPlayerAsync(JsonElement json) {
        Pokemon[] team = await Task.WhenAll(
            json.GetProperty("pokemon")!
                .EnumerateArray()!
                .Select(async p => {
                    var pokemon = await pokemonService.GetPokemonWithMovesByNameAsync(
                        p.GetProperty("name").GetString()!,
                        p.GetProperty("moves").EnumerateArray().Select(m => m.GetString()!).ToArray()!
                    );
                    if (p.TryGetProperty("level", out var levelProp))
                        pokemon.Level = levelProp.GetInt32();
                    return pokemon;
                })
        );

        Player player = new(
            json.GetProperty("name").GetString()!,
            false,
            team,
            Player.GetDefaultItems());

        return player;
    }

    private async Task<(Player p, string? behaviourName)> CreateBotPlayerAsync(JsonElement json) {
        Player p = await CreateHumanPlayerAsync(json);
        p.IsBot = true;
        bool behaviourSpecified = json.TryGetProperty("behaviour", out JsonElement value);
        return (p, behaviourSpecified ? value.GetString() : null);
    }
}