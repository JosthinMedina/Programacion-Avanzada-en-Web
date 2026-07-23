USE SistemaGestionAcademica;
GO

CREATE OR ALTER PROCEDURE spRegistrarAsistencia
    @id_matricula INT,
    @fecha DATE,
    @estado VARCHAR(20),
    @observacion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Matriculas
        WHERE id_matricula = @id_matricula
          AND estado = 1
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    IF @estado NOT IN ('Presente', 'Ausente', 'Justificada')
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM Asistencias
        WHERE id_matricula = @id_matricula
          AND fecha = @fecha
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    INSERT INTO Asistencias
    (
        id_matricula,
        fecha,
        estado,
        observacion
    )
    VALUES
    (
        @id_matricula,
        @fecha,
        @estado,
        @observacion
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarAsistencias
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        A.id_asistencia AS IdAsistencia,
        A.id_matricula AS IdMatricula,
        A.fecha AS Fecha,
        A.estado AS Estado,
        A.observacion AS Observacion
    FROM Asistencias A
    ORDER BY A.fecha DESC, A.id_asistencia DESC;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarAsistencia
    @id_asistencia INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        A.id_asistencia AS IdAsistencia,
        A.id_matricula AS IdMatricula,
        A.fecha AS Fecha,
        A.estado AS Estado,
        A.observacion AS Observacion
    FROM Asistencias A
    WHERE A.id_asistencia = @id_asistencia;
END;
GO


CREATE OR ALTER PROCEDURE spActualizarAsistencia
    @id_asistencia INT,
    @id_matricula INT,
    @fecha DATE,
    @estado VARCHAR(20),
    @observacion VARCHAR(250) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Asistencias
        WHERE id_asistencia = @id_asistencia
    )
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Matriculas
        WHERE id_matricula = @id_matricula
          AND estado = 1
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    IF @estado NOT IN ('Presente', 'Ausente', 'Justificada')
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM Asistencias
        WHERE id_matricula = @id_matricula
          AND fecha = @fecha
          AND id_asistencia <> @id_asistencia
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    UPDATE Asistencias
    SET
        id_matricula = @id_matricula,
        fecha = @fecha,
        estado = @estado,
        observacion = @observacion
    WHERE id_asistencia = @id_asistencia;

    SELECT 1 AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spEliminarAsistencia
    @id_asistencia INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Asistencias
        WHERE id_asistencia = @id_asistencia
    )
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    DELETE FROM Asistencias
    WHERE id_asistencia = @id_asistencia;

    SELECT 1 AS Resultado;
END;
GO