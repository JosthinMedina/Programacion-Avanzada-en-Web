USE SistemaGestionAcademica;
GO

CREATE OR ALTER PROCEDURE spRegistrarEvento
    @id_curso INT,
    @titulo VARCHAR(150),
    @descripcion VARCHAR(500),
    @fecha_inicio DATETIME,
    @fecha_fin DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Cursos
        WHERE id_curso = @id_curso
          AND estado = 1
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    IF @fecha_fin < @fecha_inicio
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    INSERT INTO Eventos
    (
        id_curso,
        titulo,
        descripcion,
        fecha_inicio,
        fecha_fin,
        estado
    )
    VALUES
    (
        @id_curso,
        @titulo,
        @descripcion,
        @fecha_inicio,
        @fecha_fin,
        1
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarEventos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_evento AS IdEvento,
        id_curso AS IdCurso,
        titulo AS Titulo,
        descripcion AS Descripcion,
        fecha_inicio AS FechaInicio,
        fecha_fin AS FechaFin,
        estado AS Estado
    FROM Eventos
    ORDER BY fecha_inicio DESC;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarEvento
    @id_evento INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        id_evento AS IdEvento,
        id_curso AS IdCurso,
        titulo AS Titulo,
        descripcion AS Descripcion,
        fecha_inicio AS FechaInicio,
        fecha_fin AS FechaFin,
        estado AS Estado
    FROM Eventos
    WHERE id_evento = @id_evento;
END;
GO


CREATE OR ALTER PROCEDURE spActualizarEvento
    @id_evento INT,
    @id_curso INT,
    @titulo VARCHAR(150),
    @descripcion VARCHAR(500),
    @fecha_inicio DATETIME,
    @fecha_fin DATETIME,
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Eventos
        WHERE id_evento = @id_evento
    )
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Cursos
        WHERE id_curso = @id_curso
          AND estado = 1
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    IF @fecha_fin < @fecha_inicio
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    UPDATE Eventos
    SET
        id_curso = @id_curso,
        titulo = @titulo,
        descripcion = @descripcion,
        fecha_inicio = @fecha_inicio,
        fecha_fin = @fecha_fin,
        estado = @estado
    WHERE id_evento = @id_evento;

    SELECT 1 AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spEliminarEvento
    @id_evento INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Eventos
        WHERE id_evento = @id_evento
    )
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    UPDATE Eventos
    SET estado = 0
    WHERE id_evento = @id_evento;

    SELECT 1 AS Resultado;
END;
GO