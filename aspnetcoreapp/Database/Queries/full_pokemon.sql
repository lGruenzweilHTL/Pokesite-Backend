SELECT
    p.id AS PokemonId,
    p.name AS PokemonName,
    p.description AS Description,
    s.attack,
    s.defense,
    s.speed,
    s.spAttack,
    s.spDefense,
    s.hp,
    GROUP_CONCAT(t.typeName) AS Types
FROM
    pokemon p
        JOIN
    stats s ON p.id = s.pokemonId
        LEFT JOIN
    types t ON p.id = t.pokemonId
GROUP BY
    p.id, p.name, p.description, s.attack, s.defense, s.speed, s.spAttack, s.spDefense, s.hp;