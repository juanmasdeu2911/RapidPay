DECLARE @i INT = 1;
DECLARE @prefix VARCHAR(4);
DECLARE @randomPart VARCHAR(11);

WHILE @i <= 1000
BEGIN
    -- Elegir aleatoriamente entre los prefijos 2552 y 5666
    SET @prefix = CASE WHEN RAND() < 0.5 THEN '2552' ELSE '5666' END;

    -- Generar la parte restante del número
    SET @randomPart = RIGHT('00000000000' + CAST(ABS(CHECKSUM(NEWID())) % 10000000000 AS VARCHAR(11)), 11);

    -- Insertar el número con el balance aleatorio
    INSERT INTO Cards (Number, Balance)
    VALUES (
        @prefix + @randomPart, -- Número completo
        ROUND(RAND() * (1000000 - 10000) + 10000, 2) -- Balance entre 10,000 y 1,000,000
    );

    SET @i = @i + 1;
END;

