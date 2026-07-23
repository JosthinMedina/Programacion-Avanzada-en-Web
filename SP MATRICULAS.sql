USE SistemaGestionAcademica;
GO

CREATE OR ALTER PROCEDURE spRegistrarMatricula
    @id_estudiante INT,
    @id_curso INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Estudiantes
        WHERE id_estudiante = @id_estudiante
          AND estado = 1
    )
    BEGIN
        SELECT -1 AS Resultado;
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

    IF EXISTS
    (
        SELECT 1
        FROM Matriculas
        WHERE id_estudiante = @id_estudiante
          AND id_curso = @id_curso
          AND estado = 1
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    INSERT INTO Matriculas
    (
        id_estudiante,
        id_curso,
        fecha_matricula,
        estado
    )
    VALUES
    (
        @id_estudiante,
        @id_curso,
        GETDATE(),
        1
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarMatriculas
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        M.id_matricula AS IdMatricula,
        M.id_estudiante AS IdEstudiante,
        M.id_curso AS IdCurso,
        M.fecha_matricula AS FechaMatricula,
        M.estado AS Estado
    FROM Matriculas M
    ORDER BY M.id_matricula DESC;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarMatricula
    @id_matricula INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        M.id_matricula AS IdMatricula,
        M.id_estudiante AS IdEstudiante,
        M.id_curso AS IdCurso,
        M.fecha_matricula AS FechaMatricula,
        M.estado AS Estado
    FROM Matriculas M
    WHERE M.id_matricula = @id_matricula;
END;
GO


CREATE OR ALTER PROCEDURE spActualizarMatricula
    @id_matricula INT,
    @id_estudiante INT,
    @id_curso INT,
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Matriculas
        WHERE id_matricula = @id_matricula
    )
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Estudiantes
        WHERE id_estudiante = @id_estudiante
          AND estado = 1
    )
    BEGIN
        SELECT -1 AS Resultado;
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

    IF EXISTS
    (
        SELECT 1
        FROM Matriculas
        WHERE id_estudiante = @id_estudiante
          AND id_curso = @id_curso
          AND id_matricula <> @id_matricula
          AND estado = 1
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    UPDATE Matriculas
    SET
        id_estudiante = @id_estudiante,
        id_curso = @id_curso,
        estado = @estado
    WHERE id_matricula = @id_matricula;

    SELECT 1 AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spEliminarMatricula
    @id_matricula INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Matriculas
        WHERE id_matricula = @id_matricula
    )
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    UPDATE Matriculas
    SET estado = 0
    WHERE id_matricula = @id_matricula;

    SELECT 1 AS Resultado;
END;
GO