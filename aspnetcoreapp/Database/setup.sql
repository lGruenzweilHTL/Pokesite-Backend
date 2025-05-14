DROP DATABASE IF EXISTS pokesite; -- Fresh reset to rebuild without conflicts
CREATE DATABASE pokesite;
USE pokesite;

CREATE TABLE pokemon
(
    id INT UNIQUE, -- ID in the Pokedex
    name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(300),
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

-- TODO: items
INSERT INTO pokemon (id, name, description)
VALUES
    (6, 'Charizard', 'Charizard evolves from Charmeleon and gains the ability to fly. It’s a majestic yet intimidating Pokémon that breathes intense flames, capable of melting almost anything. Charizard has a fiery spirit and loves soaring high above the clouds, searching for worthy foes to battle.'),
    (94, 'Gengar', 'Gengar is a mischievous and playful Pokémon known for its cunning tricks and ghostly nature. It thrives in the shadows, using its abilities to surprise and outsmart its opponents.'),
    (149, 'Dragonite', 'Dragonite is a strong and gentle Pokémon known for its impressive flying abilities and kind nature. It is capable of circling the globe in just 16 hours and is often seen helping those in need.'),
    (9, 'Blastoise', 'Blastoise is the final evolution of Squirtle, a massive Pokémon with cannons on its shell. These cannons can fire powerful jets of water with incredible force, capable of breaking through solid steel. Blastoise is both a guardian and a powerful battler.'),
    (3, 'Venusaur', 'The final evolution of Bulbasaur, Venusaur is a massive Pokémon with a fully bloomed flower on its back. This flower releases a soothing fragrance to calm others. Venusaur is deeply connected to nature and thrives in sunny weather, using its flower to harness sunlight for powerful moves.'),
    (11, 'Metapod', 'Metapod evolves from Caterpie, encasing itself in a hard shell to protect its body as it prepares for its final stage. It stays still, conserving energy for its transformation into a Butterfree.'),
    (68, 'Machamp', 'Machamp is a strong and determined Pokémon, famous for its muscular build and fighting skills. It is a trusted companion in battles and admired for its bravery. Its unmatched strength and enduring spirit make it a formidable opponent and a valued ally.'),
    (103, 'Exeggutor', 'Exeggutor, known as the Coconut Pokémon, has multiple heads that think independently, making it truly unique. It grows stronger in sunny environments and is believed to come from a tropical paradise where it naturally thrives, adding to its mysterious charm.'),
    (100, 'Voltorb', 'Voltorb is a round and mysterious Pokémon that resembles a Poké Ball. It is known for its unpredictable nature and habit of suddenly exploding when approached, disturbed, or feeling threatened.'),
    (18, 'Pidgeot', 'Pidgeot is the final evolution of Pidgey, a majestic bird Pokémon with powerful wings that allow it to fly at high speeds. Its feathers are sleek and glossy, often admired for their beauty.'),
    (130, 'Gyarados', 'Gyarados is a fierce and majestic Pokémon known for its incredible power and intimidating appearance. Often found near water, it possesses both beauty and strength, making it a fascinating creature in the Pokémon world.'),
    (95, 'Onix', 'Onix is a giant serpent-like Pokémon with a body made up of large boulders, renowned for its incredible strength and ability to drill through solid rock seamlessly. It is often found in caves and mountain tunnels.');

INSERT INTO stats (pokemonId, attack, defense, speed, spAttack, spDefense, hp)
VALUES
    (6, 84, 78, 100, 109, 85, 78),
    (94, 65, 60, 110, 130, 75, 60),
    (149, 134, 95, 80, 100, 100, 91),
    (9, 83, 100, 78, 85, 105, 79),
    (3, 82, 83, 80, 100, 100, 80),
    (11, 20, 55, 30, 25, 25, 50),
    (68, 130, 80, 55, 65, 85, 90),
    (103, 95, 85, 55, 125, 65, 95),
    (100, 30, 50, 100, 55, 55, 40),
    (18, 80, 75, 101, 70, 70, 83),
    (130, 125, 79, 81, 60, 100, 95),
    (95, 45, 160, 70, 30, 45, 35);

INSERT INTO types (pokemonId, typeName)
VALUES
    (6, 'fire'), (6, 'flying'),
    (94, 'ghost'), (94, 'poison'),
    (149, 'dragon'), (149, 'flying'),
    (9, 'water'),
    (3, 'grass'), (3, 'poison'),
    (11, 'bug'),
    (68, 'fighting'),
    (103, 'grass'), (103, 'psychic'),
    (100, 'electric'),
    (18, 'normal'), (18, 'flying'),
    (130, 'water'), (130, 'flying'),
    (95, 'rock'), (95, 'ground');


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