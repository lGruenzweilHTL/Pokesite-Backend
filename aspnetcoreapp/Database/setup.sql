CREATE DATABASE pokesite;
USE pokesite;

CREATE TABLE pokemon
(
    id INT UNIQUE, -- ID in the Pokedex
    name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(255),
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
    FOREIGN KEY (pokemonId) REFERENCES pokemon(id) ON DELETE CASCADE 
);

CREATE TABLE types
(
    id INT AUTO_INCREMENT,
    pokemonId INT,
    typeName VARCHAR(10) NOT NULL,
    PRIMARY KEY (id),
    FOREIGN KEY (pokemonId) REFERENCES pokemon(id) ON DELETE CASCADE 
);

CREATE TABLE effects
(
    id INT AUTO_INCREMENT,
    effectType VARCHAR(50) NOT NULL,
    effectName VARCHAR(50) NOT NULL,
    duration INT,
    PRIMARY KEY (id)
);
CREATE TABLE moves
(
    id INT AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(255),
    type VARCHAR(10) NOT NULL,
    power INT,
    accuracy INT,
    special BOOLEAN,
    priority BOOLEAN,
    status BOOLEAN,
    PRIMARY KEY (id)
);

-- Junction table for many-to-many relationship between moves and effects
CREATE TABLE move_effects
(
    moveId INT,
    effectId INT,
    PRIMARY KEY (moveId, effectId),
    FOREIGN KEY (moveId) REFERENCES moves(id) ON DELETE CASCADE,
    FOREIGN KEY (effectId) REFERENCES effects(id) ON DELETE CASCADE
);

-- TODO: more data, moves
INSERT INTO pokemon (id, name, description)
VALUES
    (1, 'Bulbasaur', 'A strange seed was planted on its back at birth.'),
    (2, 'Ivysaur', 'When the bulb on its back grows large, it appears to lose the ability to stand on its hind legs.'),
    (3, 'Venusaur', 'The plant blooms when it is absorbing solar energy. It stays on the move to seek sunlight.'),
    (4, 'Charmander', 'Obviously prefers hot places. When it rains, steam is said to spout from the tip of its tail.'),
    (5, 'Charmeleon', 'When it swings its burning tail, it elevates the temperature to unbearably high levels.'),
    (6, 'Charizard', 'Spits fire that is hot enough to melt boulders. Known to cause forest fires unintentionally.');

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


-- Insert data into effects table
INSERT INTO effects (effectType, effectName, duration)
VALUES
    ('Status', 'Burn', 5),
    ('Status', 'Paralyze', 0),
    ('Stat', 'Increase Attack', 3),
    ('Stat', 'Decrease Defense', 2);

-- Insert data into moves table
INSERT INTO moves (name, description, type, power, accuracy, special, priority, status)
VALUES
    ('Flamethrower', 'A powerful fire attack.', 'Fire', 90, 100, TRUE, FALSE, FALSE),
    ('Thunder Wave', 'Paralyzes the opponent.', 'Electric', 0, 90, FALSE, FALSE, TRUE),
    ('Swords Dance', 'Sharply raises attack.', 'Normal', 0, 0, FALSE, FALSE, FALSE),
    ('Tail Whip', 'Lowers the opponent defense.', 'Normal', 0, 100, FALSE, FALSE, FALSE),
    ('Fire Spin', 'Traps the opponent in a vortex of fire.', 'Fire', 35, 85, TRUE, FALSE, FALSE);

-- Insert data into move_effects table
-- Retrieve the auto-generated IDs for moves and effects
INSERT INTO move_effects (moveId, effectId)
VALUES
    (1, 1), -- Flamethrower causes Burn
    (2, 2), -- Thunder Wave causes Paralyze
    (3, 3), -- Swords Dance increases Attack
    (4, 4), -- Tail Whip decreases Defense
    (5, 1), -- Fire Spin causes Burn
    (5, 4); -- Fire Spin decreases Defense