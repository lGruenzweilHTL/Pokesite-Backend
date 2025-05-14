-- After setup.sql

USE pokesite;

-- TODO: add items to trainers, item use strategy for trainers
CREATE TABLE trainers
(
    id INT AUTO_INCREMENT,
    name VARCHAR(25) NOT NULL,
    description VARCHAR(255),
    PRIMARY KEY (id)
);
CREATE TABLE trainer_pokemon
(
    trainerId INT,
    slot INT,
    pokemonId INT,
    level INT,
    PRIMARY KEY (trainerId, slot),
    FOREIGN KEY (trainerId) REFERENCES trainers (id) ON DELETE CASCADE,
    FOREIGN KEY (pokemonId) REFERENCES pokemon (id) ON DELETE CASCADE
);

CREATE TABLE trainer_pokemon_moves
(
    trainerId INT,
    slot INT,
    moveId INT,
    PRIMARY KEY (trainerId, slot, moveId),
    FOREIGN KEY (trainerId, slot) REFERENCES trainer_pokemon (trainerId, slot) ON DELETE CASCADE,
    FOREIGN KEY (moveId) REFERENCES moves (id) ON DELETE CASCADE
);

INSERT INTO trainers (name, description)
VALUES
    ('Lance', 'Lance is a master of Dragon-type Pokémon and a member of the Elite Four in the Indigo League. Known for his powerful team and strategic skills, he is a formidable opponent who values strength and honor in battle.');

INSERT INTO trainer_pokemon (trainerId, slot, pokemonId, level)
VALUES 
    -- Lance
    (1, 1, 130, 58),
    (1, 2, 148, 56),
    (1, 3, 148, 56),
    (1, 4, 142, 60),
    (1, 5, 149, 62);

INSERT INTO trainer_pokemon_moves (trainerId, slot, moveId) 
VALUES 
    -- Lance - Gyarados
    (1, 1, -1), -- TODO: hyper beam
    (1, 1, -1), -- TODO: hydro pump
    (1, 1, -1), -- TODO: dragon rage
    (1, 1, -1), -- TODO: leer
    
    -- Lance - Dragonair 1
    (1, 2, -1), -- TODO: hyper beam
    (1, 2, -1), -- TODO: dragon rage
    (1, 2, -1), -- TODO: agility
    (1, 2, -1), -- TODO: slam
    
    -- Lance - Dragonair 2
    (1, 3, -1), -- TODO: hyper beam
    (1, 3, -1), -- TODO: agility
    (1, 3, -1), -- TODO: slam
    (1, 3, -1), -- TODO: dragon rage
    
    -- Lance - Aerodactyl
    (1, 4, -1), -- TODO: hyper beam
    (1, 4, -1), -- TODO: supersonic
    (1, 4, -1), -- TODO: take down
    (1, 4, -1), -- TODO: bite
    
    -- Lance - Dragonite
    (1, 5, -1), -- TODO: hyper beam
    (1, 5, -1), -- TODO: slam
    (1, 5, -1), -- TODO: barrier
    (1, 5, -1); -- TODO: agility
    
