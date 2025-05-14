# API Endpoints

## Battle

### Start Battle

**Type**: `POST`
**Endpoint**: `/battle/start`
**Description**: Starts a new battle.
**Request Body**:

```json
{
  "player1": {
    "name": "string",
    "human": "bool",
    "pokemon": [
      {
        "name": "string",
        "level": "int",
        "moves": [
          "string"
        ]
      }
    ]
  },
  "player2": {
    "name": "string",
    "human": "bool",
    "pokemon": [
      {
        "name": "string",
        "level": "int",
        "moves": [
          "string"
        ]
      }
    ]
  }
}
```

**Response**:

```json
{
  "battle_id": "string",
  "websocket_url": "string"
}
```

### Action

**Type**: `Websocket connection`
**Endpoint**: `websocket`
**Description**: Takes an action (attack, item, switch) in the battle.
**Request Body**:

```json
{
  "action": "string (attack, item, switch)",
  "move": "string (only if attack action is taken)",
  "item": "string (only if item action is taken)",
  "switch_to": "string (only if switch action is taken)",
  "player_id": "int"
}
```

**Response**:

```json
{
  "messages": ["string"],
  "player1": {
    "pokemon": "string",
    "hp": "int",
    "effects": ["string"]
  },
  "player2": {
    "pokemon": "string",
    "hp": "int",
    "effects": ["string"]
  },
  "winner": "none | draw | 1 | 2"
}
```