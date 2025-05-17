# API Endpoints

## Battle

### Create Battle

**Type**: `GET`
**Endpoint**: `/battle/create`
**Description**: Starts a new battle.
**Request Body**: n. a.
**Response**:

```json
{
  "battle_guid": "string"
}
```

### Join Battle

**Type**: `POST`
**Endpoint**: `/battle/join/{guid}`
**Description**: Joins the battle with the specified GUID.
**Request Body**: 

```json
{
  "name": "string",
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
```

### Join Battle as Bot

**Type**: `POST`
**Endpoint**: `/battle/join-bot/{guid}`
**Description**: Joins the battle with the specified GUID as a bot.
**Request Body**:

```json
{
  "name": "string",
  "behaviour": "string (optional)",
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
```

**Response**: n. a.

### Start Battle

**Type**: `GET`
**Endpoint**: `battle/start/{guid}`
**Description**: Start the battle with the specified GUID.
**Request Body**: n. a.
**Response**:

```json
{
  "websocket_url": "string",
  "player1": {
    "bot": "bool",
    "guid": "string (only if bot is false)",
    "pokemon": "string",
    "hp": "int"
  },
  "player2": {
    "bot": "bool",
    "guid": "string (only if bot is false)",
    "pokemon": "string",
    "hp": "int"
  }
}
```

### Create and Start Battle

**Type**: `POST`
**Endpoint**: `battle/start/new`
**Description**: Creates a new battle with the specified players and start it.
**Request Body**:

```json
{
  "players": [
    {
      "name": "string",
      "human": "bool",
      "behaviour": "string (optional, only used if human = false)",
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
  ]
}
```

**Response**:

```json
{
  "battle_guid": "string",
  "websocket_url": "string",
  "players": [
    {
      "bot": "bool",
      "guid": "string (guid if bot = false; 'n. a.' otherwise)",
      "pokemon": "string",
      "hp": "int"
    }
  ]
}
```

### Get Active Battles

**Type**: `GET`
**Endpoint**: `battle/active`
**Description**: Lists all active battles with connected players
**Request Body**: n. a.
**Response**:

```json
[
  {
    "battle_guid": "string",
    "players": [
      "string"
    ],
    "bots": [
      "string (behaviourName)"
    ],
    "state": "string"
  }
]
```

### Action

**Type**: `Websocket connection`
**Endpoint**: `ws://127.0.0.1:8080`
**Description**: Takes an action (attack, item, switch) in the battle.
**Request Body**:

```json
{
  "type": "attack | item | switch",
  "object": "string (move | item | switch target)",
  "battle_guid": "string",
  "player_guid": "string"
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