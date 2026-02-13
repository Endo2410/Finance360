
CREATE DATABASE DB_RRHH;
GO

USE DB_RRHH;
GO


CREATE TABLE Estado (
    IdEstado INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL UNIQUE, 
    Descripcion VARCHAR(200) NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE Departamento (
    IdDepartamento INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL UNIQUE,
    Descripcion VARCHAR(250) NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);
GO


CREATE TABLE Puesto (
    IdPuesto INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Nivel VARCHAR(50) NULL,
    IdDepartamento INT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Puesto_Departamento FOREIGN KEY (IdDepartamento) REFERENCES Departamento(IdDepartamento)
);
GO


CREATE TABLE Rol (
    IdRol INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL UNIQUE, 
    Descripcion VARCHAR(200) NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);
GO


CREATE TABLE Permiso (
    IdPermiso INT IDENTITY(1,1) PRIMARY KEY,
    IdRol INT NOT NULL,
    Nombre VARCHAR(150) NOT NULL, -- Ej: 'CrearUsuario', 'AprobarSolicitud'
    FechaAsignacion DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Permiso_Rol FOREIGN KEY (IdRol) REFERENCES Rol(IdRol)
);
GO


CREATE TABLE Usuario (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    IdRol INT NOT NULL,
    NombreUsuario VARCHAR(100) NOT NULL UNIQUE,
    Correo VARCHAR(150) NOT NULL UNIQUE,
    Clave VARCHAR(256) NOT NULL, -- Se almacena un hash de la clave, no el texto plano
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    Activo BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Usuario_Rol FOREIGN KEY (IdRol) REFERENCES Rol(IdRol)
);
GO


CREATE TABLE Empleado (
    IdEmpleado INT IDENTITY(1,1) PRIMARY KEY,
    CodigoEmpleado VARCHAR(30) NOT NULL UNIQUE,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    TipoDocumento VARCHAR(20) NULL,
    NumeroDocumento VARCHAR(50) NULL UNIQUE,
    FechaNacimiento DATE NULL,
    Correo VARCHAR(150) NULL UNIQUE,
    Telefono VARCHAR(50) NULL,
    Direccion VARCHAR(250) NULL,
    FechaIngreso DATE NOT NULL,
    FechaSalida DATE NULL,
    IdPuesto INT NULL,
    IdDepartamento INT NULL,
    IdEstado INT NOT NULL DEFAULT 1,
    IdUsuario INT NULL UNIQUE, -- Un empleado puede tener un usuario asociado
    SalarioBase DECIMAL(18,2) NULL,
    Observaciones VARCHAR(500) NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Empleado_Puesto FOREIGN KEY (IdPuesto) REFERENCES Puesto(IdPuesto),
    CONSTRAINT FK_Empleado_Departamento FOREIGN KEY (IdDepartamento) REFERENCES Departamento(IdDepartamento),
    CONSTRAINT FK_Empleado_Estado FOREIGN KEY (IdEstado) REFERENCES Estado(IdEstado),
    CONSTRAINT FK_Empleado_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuario(IdUsuario)
);
GO


CREATE TABLE TipoSolicitud (
    IdTipoSolicitud INT IDENTITY(1,1) PRIMARY KEY,
    Codigo VARCHAR(50) NOT NULL UNIQUE, 
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(250) NULL
);
GO


CREATE TABLE TipoPermiso (
    IdTipoPermiso INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Descripcion VARCHAR(250) NULL,
    ConGoceSalarial BIT NOT NULL DEFAULT 0,  -- Política por defecto
    DescuentaVacaciones BIT NOT NULL DEFAULT 0 -- Política por defecto
);
GO


CREATE TABLE Solicitud (
    IdSolicitud INT IDENTITY(1,1) PRIMARY KEY,
    IdEmpleado INT NOT NULL,
    IdTipoSolicitud INT NOT NULL,
    IdTipoPermiso INT NULL, -- Se llena solo si es un permiso de ausencia
    FechaSolicitud DATETIME NOT NULL DEFAULT GETDATE(),
    FechaInicio DATE NULL,
    FechaFin DATE NULL,
    DiasSolicitados DECIMAL(5,2) NULL,
    Motivo VARCHAR(500) NULL,
    Estado VARCHAR(50) NOT NULL DEFAULT 'PENDIENTE', -- PENDIENTE, APROBADA, RECHAZADA
    IdAprobador INT NULL,
    FechaAprobacion DATETIME NULL,
    ObservacionAprobacion VARCHAR(500) NULL,
    
    -- Campos para almacenar la DECISIÓN FINAL de RRHH
    ConGoceSalarial_Aprobado BIT NULL,
    DescuentaVacaciones_Aprobado BIT NULL,

    CONSTRAINT FK_Solicitud_Empleado FOREIGN KEY (IdEmpleado) REFERENCES Empleado(IdEmpleado),
    CONSTRAINT FK_Solicitud_Tipo FOREIGN KEY (IdTipoSolicitud) REFERENCES TipoSolicitud(IdTipoSolicitud),
    CONSTRAINT FK_Solicitud_TipoPermiso FOREIGN KEY (IdTipoPermiso) REFERENCES TipoPermiso(IdTipoPermiso),
    CONSTRAINT FK_Solicitud_Aprobador FOREIGN KEY (IdAprobador) REFERENCES Usuario(IdUsuario)
);
GO


CREATE TABLE AprobacionSolicitud (
    IdAprobacion INT IDENTITY(1,1) PRIMARY KEY,
    IdSolicitud INT NOT NULL,
    IdUsuarioAprobador INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Resultado VARCHAR(50) NOT NULL, -- APROBADO, RECHAZADO
    Comentario VARCHAR(500) NULL,
    CONSTRAINT FK_AprobacionSolicitud_Solicitud FOREIGN KEY (IdSolicitud) REFERENCES Solicitud(IdSolicitud),
    CONSTRAINT FK_AprobacionSolicitud_Usuario FOREIGN KEY (IdUsuarioAprobador) REFERENCES Usuario(IdUsuario)
);
GO



CREATE TABLE Asistencia (
    IdAsistencia INT IDENTITY(1,1) PRIMARY KEY,
    IdEmpleado INT NOT NULL,
    Fecha DATE NOT NULL,
    HoraEntrada TIME NULL,
    HoraSalida TIME NULL,
    HorasTrabajadas AS DATEDIFF(HOUR, HoraEntrada, HoraSalida),
    Tipo VARCHAR(50) NOT NULL DEFAULT 'PRESENCIAL', -- PRESENCIAL, REMOTO, AUSENCIA_JUSTIFICADA
    Observaciones VARCHAR(300) NULL,
    CONSTRAINT FK_Asistencia_Empleado FOREIGN KEY (IdEmpleado) REFERENCES Empleado(IdEmpleado),
    CONSTRAINT UQ_Asistencia_Empleado_Fecha UNIQUE (IdEmpleado, Fecha) -- Un registro por empleado por día
);
GO



CREATE TABLE Vacaciones (
    IdVacaciones INT IDENTITY(1,1) PRIMARY KEY,
    IdEmpleado INT NOT NULL,
    Anio INT NOT NULL,
    DiasOtorgados DECIMAL(5,2) NOT NULL DEFAULT 0,
    DiasTomados DECIMAL(5,2) NOT NULL DEFAULT 0,
    DiasRestantes AS (DiasOtorgados - DiasTomados) PERSISTED,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Vacaciones_Empleado FOREIGN KEY (IdEmpleado) REFERENCES Empleado(IdEmpleado),
    CONSTRAINT UQ_Vacaciones_Empleado_Anio UNIQUE (IdEmpleado, Anio)
);
GO

/* ESTA NO SE SI DEFINIRLA, CREO QUE NO ES NECESARIO */
-- Define los períodos de pago (quincenal, mensual)
CREATE TABLE PeriodoNomina (
    IdPeriodo INT IDENTITY(1,1) PRIMARY KEY,
    FechaInicio DATE NOT NULL,
    FechaFin DATE NOT NULL,
    Descripcion VARCHAR(200) NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);
GO

CREATE TABLE Nomina (
    IdNomina INT IDENTITY(1,1) PRIMARY KEY,
    IdPeriodo INT NOT NULL,
    IdEmpleado INT NOT NULL,
    SalarioBase DECIMAL(18,2) NOT NULL,
    Bonificaciones DECIMAL(18,2) NOT NULL DEFAULT 0,
    Deducciones DECIMAL(18,2) NOT NULL DEFAULT 0,
    NetoPagar AS (SalarioBase + Bonificaciones - Deducciones) PERSISTED,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Nomina_Periodo FOREIGN KEY (IdPeriodo) REFERENCES PeriodoNomina(IdPeriodo),
    CONSTRAINT FK_Nomina_Empleado FOREIGN KEY (IdEmpleado) REFERENCES Empleado(IdEmpleado)
);
GO


CREATE TABLE Auditoria (
    IdAuditoria INT IDENTITY(1,1) PRIMARY KEY,
    TablaNombre VARCHAR(100) NULL,
    TipoOperacion VARCHAR(50) NULL, -- INSERT, UPDATE, DELETE
    IdRegistro INT NULL,
    Usuario VARCHAR(150) NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Detalle VARCHAR(MAX) NULL -- Puede almacenar valores antiguos y nuevos en formato JSON
);
GO

CREATE TABLE Notificacion (
    IdNotificacion INT IDENTITY(1,1) PRIMARY KEY,
    IdUsuarioDestinatario INT NOT NULL,
    Titulo VARCHAR(200) NOT NULL,
    Mensaje VARCHAR(2000) NOT NULL,
    Leida BIT NOT NULL DEFAULT 0,
    FechaEnvio DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Notificacion_Usuario FOREIGN KEY (IdUsuarioDestinatario) REFERENCES Usuario(IdUsuario)
);
GO