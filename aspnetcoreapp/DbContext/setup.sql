CREATE DATABASE pokesite;
USE pokesite;

CREATE TABLE pokemon
(
    id INT,
    name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(255),
    level INT NOT NULL,
    PRIMARY KEY (id)
);

CREATE TABLE stats
(
    pokemonId INT,
    attack INT NOT NULL,
    defense INT NOT NULL,
    speed INT NOT NULL,
    spAttack INT NOT NULL,
    spDefense INT NOT NULL,
    hp INT NOT NULL,
    PRIMARY KEY (pokemonId),
    FOREIGN KEY (pokemonId) REFERENCES pokemon(id)
);

CREATE TABLE types
(
    id INT AUTO_INCREMENT,
    pokemonId INT,
    typeName VARCHAR(10) NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY (pokemonId) REFERENCES pokemon(id)
);

CREATE TABLE effects
(
    id INT AUTO_INCREMENT,
    pokemonId INT,
    effectType VARCHAR(50) NOT NULL,
    effectName VARCHAR(50) NOT NULL,
    duration INT,
    PRIMARY KEY (id),
    FOREIGN KEY (pokemonId) REFERENCES pokemon(id)
);

-- TODO: more data, moves
INSERT INTO pokemon (id, name, description, level)
VALUES
    (1, 'Bulbasaur', 'A strange seed was planted on its back at birth.', 5),
    (2, 'Ivysaur', 'When the bulb on its back grows large, it appears to lose the ability to stand on its hind legs.', 16),
    (3, 'Venusaur', 'The plant blooms when it is absorbing solar energy. It stays on the move to seek sunlight.', 32),
    (4, 'Charmander', 'Obviously prefers hot places. When it rains, steam is said to spout from the tip of its tail.', 5),
    (5, 'Charmeleon', 'When it swings its burning tail, it elevates the temperature to unbearably high levels.', 16),
    (6, 'Charizard', 'Spits fire that is hot enough to melt boulders. Known to cause forest fires unintentionally.', 36);

INSERT INTO stats (pokemonId, attack, defense, speed, spAttack, spDefense, hp)
VALUES
    (1, 49, 49, 45, 65, 65, 45),
    (2, 62, 63, 60, 80, 80, 60),
    (3, 82, 83, 80, 100, 100, 80),
    (4, 52, 43, 65, 60, 50, 39),
    (5, 64, 58, 80, 80, 65, 58),
    (6, 84, 78, 100, 109, 85, 78);

INSERT INTO types (pokemonId, typeName)
VALUES
    (1, 'Grass'), (1, 'Poison'),
    (2, 'Grass'), (2, 'Poison'),
    (3, 'Grass'), (3, 'Poison'),
    (4, 'Fire'),
    (5, 'Fire'),
    (6, 'Fire'), (6, 'Flying');