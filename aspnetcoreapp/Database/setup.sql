DROP DATABASE IF EXISTS pokesite; -- Fresh reset to rebuild without conflicts
CREATE DATABASE pokesite;
USE pokesite;

CREATE TABLE pokemon
(
    id INT, -- ID in the Pokedex
    name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(300),
    typeFlags INT,
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

CREATE TABLE items
(
    id INT AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(255),
    type TINYINT,
    amount INT,
    PRIMARY KEY (id)
);

CREATE TABLE effects
(
    effectCode VARCHAR(50) NOT NULL UNIQUE,
    effectName VARCHAR(50) NOT NULL,
    PRIMARY KEY (effectCode)
);

CREATE TABLE moves
(
    id INT AUTO_INCREMENT,
    name VARCHAR(50) NOT NULL UNIQUE,
    description VARCHAR(255),
    typeFlags INT,
    power INT,
    accuracy INT,
    special BOOLEAN,
    priority TINYINT, -- -1 - 7
    status BOOLEAN,
    PRIMARY KEY (id)
);

-- Junction table for many-to-many relationship between moves and effects
-- Now includes duration and chance, which are specific to each move-effect combination
CREATE TABLE move_effects
(
    moveId INT,
    effectCode VARCHAR(50),
    duration INT,
    chance INT,
    targetsSelf BOOLEAN,
    PRIMARY KEY (moveId, effectCode),
    FOREIGN KEY (moveId) REFERENCES moves(id) ON DELETE CASCADE,
    FOREIGN KEY (effectCode) REFERENCES effects(effectCode) ON DELETE CASCADE
);

INSERT INTO pokemon (id, name, typeFlags, description)
VALUES
    (6, 'Charizard', 514, 'Charizard evolves from Charmeleon and gains the ability to fly. It’s a majestic yet intimidating Pokémon that breathes intense flames, capable of melting almost anything. Charizard has a fiery spirit and loves soaring high above the clouds, searching for worthy foes to battle.'),
    (94, 'Gengar', 8320, 'Gengar is a mischievous and playful Pokémon known for its cunning tricks and ghostly nature. It thrives in the shadows, using its abilities to surprise and outsmart its opponents.'),
    (149, 'Dragonite', 16896, 'Dragonite is a strong and gentle Pokémon known for its impressive flying abilities and kind nature. It is capable of circling the globe in just 16 hours and is often seen helping those in need.'),
    (9, 'Blastoise', 4, 'Blastoise is the final evolution of Squirtle, a massive Pokémon with cannons on its shell. These cannons can fire powerful jets of water with incredible force, capable of breaking through solid steel. Blastoise is both a guardian and a powerful battler.'),
    (3, 'Venusaur', 144, 'The final evolution of Bulbasaur, Venusaur is a massive Pokémon with a fully bloomed flower on its back. This flower releases a soothing fragrance to calm others. Venusaur is deeply connected to nature and thrives in sunny weather, using its flower to harness sunlight for powerful moves.'),
    (11, 'Metapod', 2048, 'Metapod evolves from Caterpie, encasing itself in a hard shell to protect its body as it prepares for its final stage. It stays still, conserving energy for its transformation into a Butterfree.'),
    (68, 'Machamp', 64, 'Machamp is a strong and determined Pokémon, famous for its muscular build and fighting skills. It is a trusted companion in battles and admired for its bravery. Its unmatched strength and enduring spirit make it a formidable opponent and a valued ally.'),
    (103, 'Exeggutor', 1040, 'Exeggutor, known as the Coconut Pokémon, has multiple heads that think independently, making it truly unique. It grows stronger in sunny environments and is believed to come from a tropical paradise where it naturally thrives, adding to its mysterious charm.'),
    (100, 'Voltorb', 8, 'Voltorb is a round and mysterious Pokémon that resembles a Poké Ball. It is known for its unpredictable nature and habit of suddenly exploding when approached, disturbed, or feeling threatened.'),
    (18, 'Pidgeot', 513, 'Pidgeot is the final evolution of Pidgey, a majestic bird Pokémon with powerful wings that allow it to fly at high speeds. Its feathers are sleek and glossy, often admired for their beauty.'),
    (130, 'Gyarados', 516, 'Gyarados is a fierce and majestic Pokémon known for its incredible power and intimidating appearance. Often found near water, it possesses both beauty and strength, making it a fascinating creature in the Pokémon world.'),
    (95, 'Onix', 4352, 'Onix is a giant serpent-like Pokémon with a body made up of large boulders, renowned for its incredible strength and ability to drill through solid rock seamlessly. It is often found in caves and mountain tunnels.');

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

INSERT INTO items (name, description, type, amount)
VALUES
    ('Potion', 'Heals 20 HP.', 1, 20),
    ('Super Potion', 'Heals 50 HP.', 1, 50),
    ('Hyper Potion', 'Heals 200 HP.', 1, 200),
    ('Max Potion', 'Fully heals HP.', 1, 999),
    ('Revive', 'Revives a fainted Pokémon with half HP.', 2, 0),
    ('Full Restore', 'Fully heals HP and cures status conditions.', 3, 999),
    ('Antidote', 'Cures poison status condition.', 4, 0),
    ('Burn Heal', 'Cures burn status condition.', 5, 0);

INSERT INTO effects (effectCode, effectName)
VALUES
    -- Status effects
    ('burn', 'Burn'),
    ('paralyze', 'Paralyze'),
    ('poison', 'Poison'),
    ('confusion', 'Confusion'),
    ('freeze', 'Freeze'),
    ('sleep', 'Sleep'),
    
    -- Stat effects
    ('atk_up', 'Increase Attack'),
    ('def_down', 'Decrease Defense'),
    ('def_up', 'Increase Defense'),
    ('sp_atk_up', 'Increase Special Attack'),
    ('sp_atk_down', 'Decrease Special Attack'),
    ('sp_def_down', 'Decrease Special Defense'),
    ('speed_up', 'Increase Speed'),
    
    -- Miscellaneous effects
    ('flinch', 'Flinch'),
    ('dest_bond', 'Destiny Bond'), -- Destiny bond fainting effect
    ('faint', 'Faint'),
    ('heal', 'Heal'),
    ('entry_hazard', 'Entry Hazard'),
    ('recoil', 'Recoil');

INSERT INTO moves (name, description, typeFlags, power, accuracy, special, priority, status)
VALUES
    ('Flamethrower', 'A powerful fire attack.', 2, 90, 100, TRUE, 0, FALSE),
    ('Dragon Claw', '', 16384, 80, 100, FALSE, 0, FALSE),
    ('Air Slash', '', 512, 75, 95, FALSE, 0, FALSE),
    ('Fire Blast', '', 2, 110, 85, TRUE, 0, FALSE),
    ('Shadow Ball', '', 8192, 80, 100, TRUE, 0, FALSE),
    ('Dark Pulse', '', 32768, 80, 100, TRUE, 0, FALSE),
    ('Sludge Bomb', '', 128, 90, 100, TRUE, 0, FALSE),
    ('Destiny Bond', '', 8192, 0, 100, FALSE, 0, TRUE),
    ('Dragon Dance', '', 16384, 0, 100, FALSE, 0, TRUE),
    ('Outrage', '', 16384, 120, 100, FALSE, 0, FALSE),
    ('Earthquake', '', 256, 100, 100, FALSE, 0, FALSE),
    ('Fire Punch', '', 2, 75, 100, FALSE, 0, FALSE),
    ('Hydro Pump', '', 4, 110, 80, TRUE, 0, FALSE),
    ('Ice Beam', '', 32, 90, 100, TRUE, 0, FALSE),
    ('Focus Blast', '', 64, 120, 70, TRUE, 0, FALSE),
    ('Solar Beam', '', 16, 120, 100, TRUE, 0, FALSE),
    ('Synthesis', '', 16, 0, 100, FALSE, 0, TRUE),
    ('Harden', '', 1, 0, 100, FALSE, 0, TRUE),
    ('Cross Chop', '', 64, 100, 80, FALSE, 0, FALSE),
    ('Stone Edge', '', 4096, 100, 80, FALSE, 0, FALSE),
    ('Ice Punch', '', 32, 75, 100, FALSE, 0, FALSE),
    ('Psychic', '', 1024, 90, 100, TRUE, 0, FALSE),
    ('Giga Drain', '', 16, 75, 100, TRUE, 0, FALSE),
    ('Sleep Powder', '', 16, 0, 75, FALSE, 0, TRUE),
    ('Thunderbolt', '', 8, 90, 100, TRUE, 0, FALSE),
    ('Thunder Wave', '', 8, 0, 90, FALSE, 0, TRUE),
    ('Explosion', '', 1, 250, 100, FALSE, 0, FALSE),
    ('Charge Beam', '', 8, 50, 90, TRUE, 0, FALSE),
    ('Brave Bird', '', 512, 120, 100, FALSE, 0, FALSE),
    ('Return', '', 1, 102, 100, FALSE, 0, FALSE),
    ('Roost', '', 512, 0, 100, FALSE, 0, TRUE),
    ('Heat Wave', '', 2, 95, 90, TRUE, 0, FALSE),
    ('Waterfall', '', 4, 80, 100, FALSE, 0, FALSE),
    ('Ice Fang', '', 32, 65, 95, FALSE, 0, FALSE),
    ('Stealth Rock', '', 4096, 0, 100, FALSE, 0, TRUE),
    ('Toxic', '', 128, 0, 90, FALSE, 0, TRUE);

-- move_effects now includes duration and chance for each move-effect pair
INSERT INTO move_effects (moveId, effectCode, duration, chance, targetsSelf)
VALUES
    (1, 'burn', 2, 10, false),              -- Flamethrower: Burn for 2 turns, 10% chance
    (3, 'flinch', 1, 30, false),            -- Air Slash: Flinch for 1 turn, 30% chance
    (4, 'burn', 2, 10, false),              -- Fire Blast: Burn for 2 turns, 10% chance
    (5, 'sp_def_down', 0, 20, false),       -- Shadow Ball: Decrease Opponent's Special Defense, 20% chance
    (6, 'flinch', 1, 20, false),            -- Dark Pulse: Flinch for 1 turn, 20% chance
    (7, 'poison', 2, 30, false),            -- Sludge Bomb: Poison for 2 turns, 30% chance
    (8, 'dest_bond', 1, 100, false),        -- Destiny Bond: Destiny Bond effect, 100% chance
    (9, 'atk_up', 0, 100, true),            -- Dragon Dance: Increase Attack, 100% chance
    (9, 'speed_up', 0, 100, true),          -- Dragon Dance: Increase Speed, 100% chance
    (10, 'confusion', 2, 100, true),        -- Outrage: Confusion for 2 turns, 100% chance
    (12, 'burn', 2, 10, false),             -- Fire Punch: Burn for 2 turns, 10% chance
    (14, 'freeze', 2, 10, false),           -- Ice Beam: Freeze for 2 turns, 10% chance
    (15, 'sp_def_down', 0, 10, false),      -- Focus Blast: Decrease Opponent's Special Defense, 10% chance
    (17, 'heal', 0, 100, true),             -- Synthesis: Heal, 100% chance
    (18, 'def_up', 0, 100, true),           -- Harden: Increase Defense, 100% chance
    (21, 'freeze', 2, 10, false),           -- Ice Punch: Freeze for 2 turns, 10% chance
    (22, 'sp_def_down', 0, 10, false),      -- Psychic: Decrease Opponent's Special Defense, 10% chance
    (23, 'heal', 0, 100, true),             -- Giga Drain: Heal, 100% chance
    (24, 'sleep', 2, 100, false),           -- Sleep Powder: Sleep for 2 turns, 100% chance
    (25, 'paralyze', 2, 10, false),         -- Thunderbolt: Paralyze for 2 turns, 10% chance
    (26, 'paralyze', 2, 100, false),        -- Thunder Wave: Paralyze for 2 turns, 100% chance
    (27, 'faint', 0, 100, true),            -- Explosion: Faint, 100% chance
    (28, 'sp_atk_up', 0, 70, true),         -- Charge Beam: Increase Special Attack, 70% chance
    (29, 'recoil', 0, 100, true),           -- Brave Bird: Recoil, 100% chance
    (31, 'heal', 0, 100, true),             -- Roost: Heal, 100% chance
    (32, 'burn', 2, 10, false),             -- Heat Wave: Burn for 2 turns, 10% chance
    (33, 'flinch', 1, 20, false),           -- Waterfall: Flinch for 1 turn, 20% chance
    (34, 'freeze', 2, 10, false),           -- Ice Fang: Freeze for 2 turns, 10% chance
    (35, 'entry_hazard', 0, 100, false),    -- Stealth Rock: Entry Hazard, 100% chance
    (36, 'poison', 2, 100, false);          -- Toxic: Poison for 2 turns, 100% chance
