USE SistemaGestionAcademica;
GO

CREATE OR ALTER PROCEDURE spRegistrarCurso
    @id_profesor INT,
    @nombre_curso VARCHAR(150),
    @descripcion VARCHAR(500)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Profesores
        WHERE id_profesor = @id_profesor
          AND estado = 1
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    INSERT INTO Cursos
    (
        id_profesor,
        nombre_curso,
        descripcion,
        estado,
        fecha_registro
    )
    VALUES
    (
        @id_profesor,
        @nombre_curso,
        @descripcion,
        1,
        GETDATE()
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarCursos
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        C.id_curso AS IdCurso,
        C.id_profesor AS IdProfesor,
        C.nombre_curso AS NombreCurso,
        C.descripcion AS Descripcion,
        C.estado AS Estado,
        C.fecha_registro AS FechaRegistro
    FROM Cursos C
    ORDER BY C.id_curso DESC;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarCurso
    @id_curso INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        C.id_curso AS IdCurso,
        C.id_profesor AS IdProfesor,
        C.nombre_curso AS NombreCurso,
        C.descripcion AS Descripcion,
        C.estado AS Estado,
        C.fecha_registro AS FechaRegistro
    FROM Cursos C
    WHERE C.id_curso = @id_curso;
END;
GO


CREATE OR ALTER PROCEDURE spActualizarCurso
    @id_curso INT,
    @id_profesor INT,
    @nombre_curso VARCHAR(150),
    @descripcion VARCHAR(500),
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Cursos
        WHERE id_curso = @id_curso
    )
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Profesores
        WHERE id_profesor = @id_profesor
          AND estado = 1
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    UPDATE Cursos
    SET
        id_profesor = @id_profesor,
        nombre_curso = @nombre_curso,
        descripcion = @descripcion,
        estado = @estado
    WHERE id_curso = @id_curso;

    SELECT 1 AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spEliminarCurso
    @id_curso INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Cursos
        WHERE id_curso = @id_curso
    )
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    UPDATE Cursos
    SET estado = 0
    WHERE id_curso = @id_curso;

    SELECT 1 AS Resultado;
END;
GO