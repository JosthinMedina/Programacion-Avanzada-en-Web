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