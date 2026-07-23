USE SistemaGestionAcademica;
GO

CREATE OR ALTER PROCEDURE spRegistrarProfesor
    @id_usuario INT,
    @nombre VARCHAR(100),
    @primer_apellido VARCHAR(100),
    @segundo_apellido VARCHAR(100),
    @identificacion VARCHAR(30),
    @correo VARCHAR(150),
    @telefono VARCHAR(20),
    @especialidad VARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Usuarios
        WHERE id_usuario = @id_usuario
          AND estado = 1
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM Profesores
        WHERE id_usuario = @id_usuario
           OR identificacion = @identificacion
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    INSERT INTO Profesores
    (
        id_usuario,
        nombre,
        primer_apellido,
        segundo_apellido,
        identificacion,
        correo,
        telefono,
        especialidad,
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
        @especialidad,
        1,
        GETDATE()
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarProfesores
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.id_profesor AS IdProfesor,
        P.id_usuario AS IdUsuario,
        P.nombre AS Nombre,
        P.primer_apellido AS PrimerApellido,
        P.segundo_apellido AS SegundoApellido,
        P.identificacion AS Identificacion,
        P.correo AS Correo,
        P.telefono AS Telefono,
        P.especialidad AS Especialidad,
        P.estado AS Estado,
        P.fecha_registro AS FechaRegistro
    FROM Profesores P
    ORDER BY P.id_profesor DESC;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarProfesor
    @id_profesor INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        P.id_profesor AS IdProfesor,
        P.id_usuario AS IdUsuario,
        P.nombre AS Nombre,
        P.primer_apellido AS PrimerApellido,
        P.segundo_apellido AS SegundoApellido,
        P.identificacion AS Identificacion,
        P.correo AS Correo,
        P.telefono AS Telefono,
        P.especialidad AS Especialidad,
        P.estado AS Estado,
        P.fecha_registro AS FechaRegistro
    FROM Profesores P
    WHERE P.id_profesor = @id_profesor;
END;
GO


CREATE OR ALTER PROCEDURE spActualizarProfesor
    @id_profesor INT,
    @id_usuario INT,
    @nombre VARCHAR(100),
    @primer_apellido VARCHAR(100),
    @segundo_apellido VARCHAR(100),
    @identificacion VARCHAR(30),
    @correo VARCHAR(150),
    @telefono VARCHAR(20),
    @especialidad VARCHAR(100),
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Profesores
        WHERE id_profesor = @id_profesor
    )
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Usuarios
        WHERE id_usuario = @id_usuario
          AND estado = 1
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM Profesores
        WHERE id_usuario = @id_usuario
          AND id_profesor <> @id_profesor
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    IF EXISTS
    (
        SELECT 1
        FROM Profesores
        WHERE identificacion = @identificacion
          AND id_profesor <> @id_profesor
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END;

    UPDATE Profesores
    SET
        id_usuario = @id_usuario,
        nombre = @nombre,
        primer_apellido = @primer_apellido,
        segundo_apellido = @segundo_apellido,
        identificacion = @identificacion,
        correo = @correo,
        telefono = @telefono,
        especialidad = @especialidad,
        estado = @estado
    WHERE id_profesor = @id_profesor;

    SELECT 1 AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spEliminarProfesor
    @id_profesor INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Profesores
        WHERE id_profesor = @id_profesor
    )
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    UPDATE Profesores
    SET estado = 0
    WHERE id_profesor = @id_profesor;

    SELECT 1 AS Resultado;
END;
GO