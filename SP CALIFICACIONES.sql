USE SistemaGestionAcademica;
GO

CREATE OR ALTER PROCEDURE spRegistrarCalificacion
    @id_matricula INT,
    @nota DECIMAL(5,2)
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

    IF @nota < 0 OR @nota > 100
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM Calificaciones
        WHERE id_matricula = @id_matricula
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    INSERT INTO Calificaciones
    (
        id_matricula,
        nota,
        fecha_registro,
        fecha_modificacion
    )
    VALUES
    (
        @id_matricula,
        @nota,
        GETDATE(),
        NULL
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarCalificaciones
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        C.id_calificacion AS IdCalificacion,
        C.id_matricula AS IdMatricula,
        C.nota AS Nota,
        C.fecha_registro AS FechaRegistro,
        C.fecha_modificacion AS FechaModificacion
    FROM Calificaciones C
    ORDER BY C.id_calificacion DESC;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarCalificacion
    @id_calificacion INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        C.id_calificacion AS IdCalificacion,
        C.id_matricula AS IdMatricula,
        C.nota AS Nota,
        C.fecha_registro AS FechaRegistro,
        C.fecha_modificacion AS FechaModificacion
    FROM Calificaciones C
    WHERE C.id_calificacion = @id_calificacion;
END;
GO


CREATE OR ALTER PROCEDURE spActualizarCalificacion
    @id_calificacion INT,
    @id_matricula INT,
    @nota DECIMAL(5,2)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Calificaciones
        WHERE id_calificacion = @id_calificacion
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

    IF @nota < 0 OR @nota > 100
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM Calificaciones
        WHERE id_matricula = @id_matricula
          AND id_calificacion <> @id_calificacion
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    UPDATE Calificaciones
    SET
        id_matricula = @id_matricula,
        nota = @nota,
        fecha_modificacion = GETDATE()
    WHERE id_calificacion = @id_calificacion;

    SELECT 1 AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spEliminarCalificacion
    @id_calificacion INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Calificaciones
        WHERE id_calificacion = @id_calificacion
    )
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    DELETE FROM Calificaciones
    WHERE id_calificacion = @id_calificacion;

    SELECT 1 AS Resultado;
END;
GO