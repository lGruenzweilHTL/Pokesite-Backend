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
        Player player = await CreatePlayerAsync(request);
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
        Player player = await CreatePlayerAsync(request);
        player.IsBot = true;

        string? behaviour = null;
        if (request.TryGetProperty("behaviour", out JsonElement value)) behaviour = value.GetString();
        
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
                Guid = p.Key
            }).ToArray()
        };
        return Ok(response.ToJson());
    }

    [HttpPost("start/new")]
    public async Task<IActionResult> StartNewBattle([FromBody] JsonElement request) {
        string battleGuid = gameManager.NewGame();

        Player player1 = await CreatePlayerAsync(request.GetProperty("player1"));
        Player player2 = await CreatePlayerAsync(request.GetProperty("player2"));

        GameLoop? game = null;
        bool success = gameManager.TryJoinGame(battleGuid, player1, out string p1Guid)
                       && gameManager.TryJoinGame(battleGuid, player2, out string p2Guid)
                       && gameManager.StartGame(battleGuid, out game);

        if (!success || game == null) {
            return BadRequest("Something went wrong while creating battle with guid: " + battleGuid);
        }

        player1.IsBot = !request.GetProperty("player1").GetProperty("human").GetBoolean();
        player2.IsBot = !request.GetProperty("player2").GetProperty("human").GetBoolean();

        StartNewBattleResponse response = new() {
            BattleGuid = battleGuid,
            WebsocketUrl = "http://localhost:8080/ws/",
            Player1 = new ResponsePlayerData {
                Pokemon = game.Player1.CurrentPokemon.Name,
                Hp = game.Player1.CurrentPokemon.CurrentHp,
                Guid = game.ConnectedPlayers.Keys.ElementAt(0)
            },
            Player2 = new ResponsePlayerData {
                Pokemon = game.Player2.CurrentPokemon.Name,
                Hp = game.Player2.CurrentPokemon.CurrentHp,
                Guid = game.ConnectedPlayers.Keys.ElementAt(1)
            }
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

    private async Task<Player> CreatePlayerAsync(JsonElement json) {
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
            false, // don't extract from json because only /battle/start/new requests have this
            team);

        return player;
    }
}