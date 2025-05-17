SELECT
    p.id AS PokemonId,
    p.name AS PokemonName,
    p.description AS Description,
    p.typeFlags as TypeFlags,
    s.attack,
    s.defense,
    s.speed,
    s.spAttack,
    s.spDefense,
    s.hp
FROM
    pokemon p
        JOIN
    stats s ON p.id = s.pokemonId
GROUP BY
    p.id, p.name, p.description, s.attack, s.defense, s.speed, s.spAttack, s.spDefense, s.hp;