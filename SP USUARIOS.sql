USE SistemaGestionAcademica;
GO

INSERT INTO Roles (nombre_rol, estado)
VALUES
('Administrador',1),
('Profesor',1),
('Estudiante',1);
GO


CREATE PROCEDURE spRegistrarUsuario
    @id_rol INT,
    @correo VARCHAR(150),
    @contrasena VARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (
        SELECT 1
        FROM Usuarios
        WHERE correo = @correo
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END

    INSERT INTO Usuarios
    (
        id_rol,
        correo,
        contrasena,
        estado,
        fecha_registro
    )
    VALUES
    (
        @id_rol,
        @correo,
        @contrasena,
        1,
        GETDATE()
    );

    SELECT SCOPE_IDENTITY() AS Resultado;
END;
GO

EXEC spRegistrarUsuario
    @id_rol = 1,
    @correo = 'admin@sistema.com',
    @contrasena = '123456';

    SELECT * FROM Usuarios;

    USE SistemaGestionAcademica;
GO

CREATE PROCEDURE spConsultarUsuarios
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.id_usuario AS IdUsuario,
        U.id_rol AS IdRol,
        R.nombre_rol AS NombreRol,
        U.correo AS Correo,
        U.estado AS Estado,
        U.fecha_registro AS FechaRegistro
    FROM Usuarios U
    INNER JOIN Roles R
        ON U.id_rol = R.id_rol
    ORDER BY U.id_usuario DESC;
END;
GO

EXEC spConsultarUsuarios;

USE SistemaGestionAcademica;
GO

CREATE PROCEDURE spConsultarUsuario
    @id_usuario INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        U.id_usuario AS IdUsuario,
        U.id_rol AS IdRol,
        R.nombre_rol AS NombreRol,
        U.correo AS Correo,
        U.estado AS Estado,
        U.fecha_registro AS FechaRegistro
    FROM Usuarios U
    INNER JOIN Roles R
        ON U.id_rol = R.id_rol
    WHERE U.id_usuario = @id_usuario;
END;
GO

EXEC spConsultarUsuario
    @id_usuario = 1;


    USE SistemaGestionAcademica;
GO

CREATE PROCEDURE spActualizarUsuario
    @id_usuario INT,
    @id_rol INT,
    @correo VARCHAR(150),
    @contrasena VARCHAR(255),
    @estado BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS
    (
        SELECT 1
        FROM Usuarios
        WHERE correo = @correo
        AND id_usuario <> @id_usuario
    )
    BEGIN
        SELECT -1 AS Resultado;
        RETURN;
    END

    UPDATE Usuarios
    SET
        id_rol = @id_rol,
        correo = @correo,
        contrasena = @contrasena,
        estado = @estado
    WHERE id_usuario = @id_usuario;

    SELECT 1 AS Resultado;
END;
GO


EXEC spActualizarUsuario
    @id_usuario = 1,
    @id_rol = 1,
    @correo = 'admin@universidad.com',
    @contrasena = '123456',
    @estado = 1;

    SELECT * FROM Usuarios;


    USE SistemaGestionAcademica;
GO

CREATE PROCEDURE spEliminarUsuario
    @id_usuario INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Usuarios
    SET estado = 0
    WHERE id_usuario = @id_usuario;

    SELECT 1 AS Resultado;
END;
GO

EXEC spEliminarUsuario
    @id_usuario = 1;

    SELECT * FROM Usuarios;


    USE SistemaGestionAcademica;
GO

UPDATE Usuarios
SET estado = 1
WHERE id_usuario = 1;
GO

SELECT * FROM Usuarios;