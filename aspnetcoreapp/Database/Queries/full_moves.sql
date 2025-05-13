SELECT
    m.id AS MoveId,
    m.name AS MoveName,
    m.description AS MoveDescription,
    m.type AS MoveType,
    m.power AS Power,
    m.accuracy AS Accuracy,
    m.special AS IsSpecial,
    m.priority AS HasPriority,
    m.status AS IsStatus,
    GROUP_CONCAT(CONCAT(e.effectName, ' (', e.effectType, ')')) AS Effects
FROM
    moves m
        LEFT JOIN
    move_effects me ON m.id = me.moveId
        LEFT JOIN
    effects e ON me.effectId = e.id
GROUP BY
    m.id, m.name, m.description, m.type, m.power, m.accuracy, m.special, m.priority, m.status;