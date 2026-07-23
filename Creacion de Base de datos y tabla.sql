CREATE DATABASE SistemaGestionAcademica;
GO

USE SistemaGestionAcademica;
GO

CREATE TABLE Roles
(
    id_rol INT IDENTITY(1,1) PRIMARY KEY,
    nombre_rol VARCHAR(50) NOT NULL,
    estado BIT NOT NULL DEFAULT 1
);
GO

CREATE TABLE Usuarios
(
    id_usuario INT IDENTITY(1,1) PRIMARY KEY,
    id_rol INT NOT NULL,
    correo VARCHAR(150) NOT NULL,
    contrasena VARCHAR(255) NOT NULL,
    estado BIT NOT NULL DEFAULT 1,
    fecha_registro DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Usuarios_Roles
        FOREIGN KEY (id_rol)
        REFERENCES Roles(id_rol),

    CONSTRAINT UQ_Usuarios_Correo
        UNIQUE (correo)
);
GO



CREATE TABLE Estudiantes
(
    id_estudiante INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    primer_apellido VARCHAR(100) NOT NULL,
    segundo_apellido VARCHAR(100) NOT NULL,
    identificacion VARCHAR(30) NOT NULL,
    correo VARCHAR(150) NOT NULL,
    telefono VARCHAR(20) NOT NULL,
    direccion VARCHAR(250) NOT NULL,
    estado BIT NOT NULL DEFAULT 1,
    fecha_registro DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Estudiantes_Usuarios
        FOREIGN KEY (id_usuario)
        REFERENCES Usuarios(id_usuario),

    CONSTRAINT UQ_Estudiantes_Usuario
        UNIQUE (id_usuario),

    CONSTRAINT UQ_Estudiantes_Identificacion
        UNIQUE (identificacion)
);
GO

CREATE TABLE Profesores
(
    id_profesor INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    primer_apellido VARCHAR(100) NOT NULL,
    segundo_apellido VARCHAR(100) NOT NULL,
    identificacion VARCHAR(30) NOT NULL,
    correo VARCHAR(150) NOT NULL,
    telefono VARCHAR(20) NOT NULL,
    especialidad VARCHAR(100) NOT NULL,
    estado BIT NOT NULL DEFAULT 1,
    fecha_registro DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Profesores_Usuarios
        FOREIGN KEY (id_usuario)
        REFERENCES Usuarios(id_usuario),

    CONSTRAINT UQ_Profesores_Usuario
        UNIQUE (id_usuario),

    CONSTRAINT UQ_Profesores_Identificacion
        UNIQUE (identificacion)
);
GO


CREATE TABLE Cursos
(
    id_curso INT IDENTITY(1,1) PRIMARY KEY,
    id_profesor INT NOT NULL,
    nombre_curso VARCHAR(150) NOT NULL,
    descripcion VARCHAR(500) NOT NULL,
    estado BIT NOT NULL DEFAULT 1,
    fecha_registro DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Cursos_Profesores
        FOREIGN KEY (id_profesor)
        REFERENCES Profesores(id_profesor)
);
GO

CREATE TABLE Matriculas
(
    id_matricula INT IDENTITY(1,1) PRIMARY KEY,
    id_estudiante INT NOT NULL,
    id_curso INT NOT NULL,
    fecha_matricula DATETIME NOT NULL DEFAULT GETDATE(),
    estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Matriculas_Estudiantes
        FOREIGN KEY (id_estudiante)
        REFERENCES Estudiantes(id_estudiante),

    CONSTRAINT FK_Matriculas_Cursos
        FOREIGN KEY (id_curso)
        REFERENCES Cursos(id_curso)
);
GO



CREATE TABLE Calificaciones
(
    id_calificacion INT IDENTITY(1,1) PRIMARY KEY,
    id_matricula INT NOT NULL,
    nota DECIMAL(5,2) NOT NULL,
    fecha_registro DATETIME NOT NULL DEFAULT GETDATE(),
    fecha_modificacion DATETIME NULL,

    CONSTRAINT FK_Calificaciones_Matriculas
        FOREIGN KEY (id_matricula)
        REFERENCES Matriculas(id_matricula),

    CONSTRAINT CK_Calificaciones_Nota
        CHECK (nota >= 0 AND nota <= 100)
);
GO

CREATE TABLE Asistencias
(
    id_asistencia INT IDENTITY(1,1) PRIMARY KEY,
    id_matricula INT NOT NULL,
    fecha DATE NOT NULL,
    estado VARCHAR(20) NOT NULL,
    observacion VARCHAR(250) NULL,

    CONSTRAINT FK_Asistencias_Matriculas
        FOREIGN KEY (id_matricula)
        REFERENCES Matriculas(id_matricula),

    CONSTRAINT CK_Asistencias_Estado
        CHECK (estado IN ('Presente', 'Ausente', 'Justificada'))
);
GO


CREATE TABLE Eventos
(
    id_evento INT IDENTITY(1,1) PRIMARY KEY,
    id_curso INT NOT NULL,
    titulo VARCHAR(150) NOT NULL,
    descripcion VARCHAR(500) NOT NULL,
    fecha_inicio DATETIME NOT NULL,
    fecha_fin DATETIME NOT NULL,
    estado BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Eventos_Cursos
        FOREIGN KEY (id_curso)
        REFERENCES Cursos(id_curso),

    CONSTRAINT CK_Eventos_Fechas
        CHECK (fecha_fin >= fecha_inicio)
);
GO

CREATE TABLE Notificaciones
(
    id_notificacion INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NOT NULL,
    asunto VARCHAR(150) NOT NULL,
    mensaje VARCHAR(1000) NOT NULL,
    fecha_envio DATETIME NOT NULL DEFAULT GETDATE(),
    estado_envio BIT NOT NULL DEFAULT 0,

    CONSTRAINT FK_Notificaciones_Usuarios
        FOREIGN KEY (id_usuario)
        REFERENCES Usuarios(id_usuario)
);
GO

CREATE TABLE Errores
(
    id_error INT IDENTITY(1,1) PRIMARY KEY,
    id_usuario INT NULL,
    fecha_error DATETIME NOT NULL DEFAULT GETDATE(),
    mensaje VARCHAR(500) NOT NULL,
    origen VARCHAR(250) NOT NULL,
    detalle VARCHAR(MAX) NOT NULL,

    CONSTRAINT FK_Errores_Usuarios
        FOREIGN KEY (id_usuario)
        REFERENCES Usuarios(id_usuario)
);
GO

--SP USARIOS
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

--SP ASISTENCIA

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

-- SP CALIFICACIONES

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

-- SP CURSOS

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

-- SP ESTUDIANTES

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

-- SP EVENTOS

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

-- SP MATRICULAS

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

-- SP NOTIFICACIONES

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

-- SP PROFESORES

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