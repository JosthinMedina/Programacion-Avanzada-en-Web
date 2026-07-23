USE SistemaGestionAcademica;
GO

CREATE OR ALTER PROCEDURE spRegistrarEstudiante
    @id_usuario INT,
    @nombre VARCHAR(100),
    @primer_apellido VARCHAR(100),
    @segundo_apellido VARCHAR(100),
    @identificacion VARCHAR(30),
    @correo VARCHAR(150),
    @telefono VARCHAR(20),
    @direccion VARCHAR(250)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM Estudiantes
        WHERE identificacion = @identificacion
           OR id_usuario = @id_usuario
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    INSERT INTO Estudiantes
    (
        id_usuario,
        nombre,
        primer_apellido,
        segundo_apellido,
        identificacion,
        correo,
        telefono,
        direccion,
        estado,
        fecha_registro
    )
    VALUES
    (
        @id_usuario,
        @nombre,
        @primer_apellido,
        @segundo_apellido,
        @identificacion,
        @correo,
        @telefono,
        @direccion,
        1,
        GETDATE()
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarEstudiantes
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        E.id_estudiante AS IdEstudiante,
        E.id_usuario AS IdUsuario,
        E.nombre AS Nombre,
        E.primer_apellido AS PrimerApellido,
        E.segundo_apellido AS SegundoApellido,
        E.identificacion AS Identificacion,
        E.correo AS Correo,
        E.telefono AS Telefono,
        E.direccion AS Direccion,
        E.estado AS Estado,
        E.fecha_registro AS FechaRegistro
    FROM Estudiantes E
    ORDER BY E.id_estudiante DESC;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarEstudiante
    @id_estudiante INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        E.id_estudiante AS IdEstudiante,
        E.id_usuario AS IdUsuario,
        E.nombre AS Nombre,
        E.primer_apellido AS PrimerApellido,
        E.segundo_apellido AS SegundoApellido,
        E.identificacion AS Identificacion,
        E.correo AS Correo,
        E.telefono AS Telefono,
        E.direccion AS Direccion,
        E.estado AS Estado,
        E.fecha_registro AS FechaRegistro
    FROM Estudiantes E
    WHERE E.id_estudiante = @id_estudiante;
END;
GO


CREATE OR ALTER PROCEDURE spActualizarEstudiante
    @id_estudiante INT,
    @id_usuario INT,
    @nombre VARCHAR(100),
    @primer_apellido VARCHAR(100),
    @segundo_apellido VARCHAR(100),
    @identificacion VARCHAR(30),
    @correo VARCHAR(150),
    @telefono VARCHAR(20),
    @direccion VARCHAR(250),
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM Estudiantes
        WHERE identificacion = @identificacion
          AND id_estudiante <> @id_estudiante
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM Estudiantes
        WHERE id_usuario = @id_usuario
          AND id_estudiante <> @id_estudiante
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    UPDATE Estudiantes
    SET
        id_usuario = @id_usuario,
        nombre = @nombre,
        primer_apellido = @primer_apellido,
        segundo_apellido = @segundo_apellido,
        identificacion = @identificacion,
        correo = @correo,
        telefono = @telefono,
        direccion = @direccion,
        estado = @estado
    WHERE id_estudiante = @id_estudiante;

    IF @@ROWCOUNT = 0
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    SELECT 1 AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spEliminarEstudiante
    @id_estudiante INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Estudiantes
    SET estado = 0
    WHERE id_estudiante = @id_estudiante;

    IF @@ROWCOUNT = 0
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    SELECT 1 AS Resultado;
END;
GO