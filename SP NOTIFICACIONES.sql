USE SistemaGestionAcademica;
GO

CREATE OR ALTER PROCEDURE spRegistrarNotificacion
    @id_usuario INT,
    @asunto VARCHAR(150),
    @mensaje VARCHAR(1000)
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

    INSERT INTO Notificaciones
    (
        id_usuario,
        asunto,
        mensaje,
        fecha_envio,
        estado_envio
    )
    VALUES
    (
        @id_usuario,
        @asunto,
        @mensaje,
        GETDATE(),
        0
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarNotificaciones
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        N.id_notificacion AS IdNotificacion,
        N.id_usuario AS IdUsuario,
        N.asunto AS Asunto,
        N.mensaje AS Mensaje,
        N.fecha_envio AS FechaEnvio,
        N.estado_envio AS EstadoEnvio
    FROM Notificaciones N
    ORDER BY N.fecha_envio DESC;
END;
GO


CREATE OR ALTER PROCEDURE spConsultarNotificacion
    @id_notificacion INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        N.id_notificacion AS IdNotificacion,
        N.id_usuario AS IdUsuario,
        N.asunto AS Asunto,
        N.mensaje AS Mensaje,
        N.fecha_envio AS FechaEnvio,
        N.estado_envio AS EstadoEnvio
    FROM Notificaciones N
    WHERE N.id_notificacion = @id_notificacion;
END;
GO


CREATE OR ALTER PROCEDURE spActualizarNotificacion
    @id_notificacion INT,
    @id_usuario INT,
    @asunto VARCHAR(150),
    @mensaje VARCHAR(1000),
    @estado_envio BIT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Notificaciones
        WHERE id_notificacion = @id_notificacion
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

    UPDATE Notificaciones
    SET
        id_usuario = @id_usuario,
        asunto = @asunto,
        mensaje = @mensaje,
        estado_envio = @estado_envio
    WHERE id_notificacion = @id_notificacion;

    SELECT 1 AS Resultado;
END;
GO


CREATE OR ALTER PROCEDURE spEliminarNotificacion
    @id_notificacion INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM Notificaciones
        WHERE id_notificacion = @id_notificacion
    )
    BEGIN
        SELECT 0 AS Resultado;
        RETURN;
    END;

    DELETE FROM Notificaciones
    WHERE id_notificacion = @id_notificacion;

    SELECT 1 AS Resultado;
END;
GO