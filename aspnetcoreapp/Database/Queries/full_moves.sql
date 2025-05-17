SELECT
    m.id AS MoveId,
    m.name AS MoveName,
    m.description AS MoveDescription,
    m.typeFlags AS TypeFlags,
    m.power AS Power,
    m.accuracy AS Accuracy,
    m.special AS IsSpecial,
    m.priority AS Priority,
    m.status AS IsStatus,
    GROUP_CONCAT(e.effectName) AS Effects
FROM
    moves m
        LEFT JOIN
    move_effects me ON m.id = me.moveId
        LEFT JOIN
    effects e ON me.effectCode = e.effectCode
GROUP BY
    m.id, m.name, m.description, m.typeFlags, m.power, m.accuracy, m.special, m.priority, m.status;