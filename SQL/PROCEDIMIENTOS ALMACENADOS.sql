---obtener usuarios
CREATE OR ALTER PROCEDURE SP_OBTENER_USUARIO
AS
BEGIN
    SELECT 
        u.IdUsuario,
		u.Nombres,
		u.apellidos,
        u.NombreUsuario,
        u.Correo,
        u.Clave,
		u.Reestablecer,
        u.IdRol,
        r.Nombre AS Rol,
        u.IdEstado,
        e.Nombre AS Estado
    FROM Usuario u
    INNER JOIN Rol r ON u.IdRol = r.IdRol
    INNER JOIN Estado e ON u.IdEstado = e.IdEstado
END

---crear usuarios
CREATE OR ALTER PROCEDURE SP_CREAR_USUARIO
    @Nombres VARCHAR(100),
    @Apellidos VARCHAR(100),
    @NombreUsuario VARCHAR(100),
    @Correo VARCHAR(150),
    @Clave VARCHAR(256),
    @IdRol INT,
    @IdEstado INT
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Usuario WHERE NombreUsuario = @NombreUsuario OR Correo = @Correo)
    BEGIN
        RAISERROR('El usuario o correo ya existe.',16,1)
        RETURN
    END

    INSERT INTO Usuario
    (Nombres, Apellidos, NombreUsuario, Correo, Clave, Reestablecer, IdRol, IdEstado)
    VALUES
    (@Nombres, @Apellidos, @NombreUsuario, @Correo, @Clave, 1, @IdRol, @IdEstado)
END


----editar usuario
CREATE OR ALTER PROCEDURE SP_EDITAR_USUARIO
    @IdUsuario INT,
	@Nombres VARCHAR(100),
    @Apellidos VARCHAR(100),
    @NombreUsuario VARCHAR(100),
    @Correo VARCHAR(150),
    @Clave VARCHAR(256) = NULL,
    @IdRol INT,
    @IdEstado INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar si existe el usuario
    IF NOT EXISTS (SELECT 1 FROM USUARIO WHERE IdUsuario = @IdUsuario)
    BEGIN
        RAISERROR('El usuario no existe.', 16, 1);
        RETURN;
    END

    -- Validar que el nombre o correo no se repita en otro usuario
    IF EXISTS (SELECT 1 FROM USUARIO 
               WHERE (NombreUsuario = @NombreUsuario OR Correo = @Correo) 
                 AND IdUsuario <> @IdUsuario)
    BEGIN
        RAISERROR('El nombre de usuario o correo ya existe.', 16, 1);
        RETURN;
    END

    -- Actualizar datos del usuario
    UPDATE USUARIO
    SET 
	    Nombres = @Nombres,
		Apellidos = @Apellidos,
        NombreUsuario = @NombreUsuario,
        Correo = @Correo,
        IdRol = @IdRol,
        IdEstado = @IdEstado,
        Clave = CASE 
                    WHEN @Clave IS NOT NULL AND @Clave <> '' THEN @Clave 
                    ELSE Clave 
                END
    WHERE IdUsuario = @IdUsuario;
END
GO




---obtener Rol
CREATE PROCEDURE SP_OBTENER_ROL
AS
BEGIN
    SELECT r.IdRol,
           r.Nombre,
           r.Descripcion,
           r.IdEstado,
           e.Nombre AS Estado
    FROM Rol r
    INNER JOIN Estado e ON r.IdEstado = e.IdEstado
END
GO


---crear Rol
CREATE PROCEDURE SP_CREAR_ROL
    @IdEstado INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(150)
AS
BEGIN
    SET NOCOUNT ON;

    -- Verificar si ya existe departamento o descripcion
    IF EXISTS (SELECT 1 FROM Rol WHERE Nombre = @Nombre )
    BEGIN
        RAISERROR('El Rol ya existe.', 16, 1);
        RETURN;
    END

    INSERT INTO Rol(IdEstado, Nombre, Descripcion)
    VALUES (@IdEstado, @Nombre, @Descripcion);
END
GO


----editar Rol
CREATE PROCEDURE SP_EDITAR_ROL
    @IdRol INT,
    @Nombre VARCHAR(100),
    @Descripcion VARCHAR(100),
    @IdEstado INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar si existe el departamento
    IF NOT EXISTS (SELECT 1 FROM Rol WHERE IdRol = @IdRol)
    BEGIN
        RAISERROR('El Rol no existe.', 16, 1);
        RETURN;
    END

    -- Actualizar datos
    UPDATE Rol
    SET 
        Nombre = @Nombre,
        Descripcion = @Descripcion,
        IdEstado = @IdEstado
    WHERE IdRol = @IdRol;
END
GO



CREATE OR ALTER PROCEDURE SP_OBTENER_REPORTE_ORDENES_COMPRA
(
    @FechaInicio DATE,
    @FechaFin DATE
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.SupplierName  AS Proveedor,
        po.PONumber     AS NumeroOrden,
        'Cerrado'       AS Estado,       -- Status = 2 siempre lo ponemos como Cerrado
        po.ConfirmingTo AS Confirmacion,
        po.DateCreated  AS FechaCreacion,
		Remarks AS Observaciones
    FROM FSCDDB.dbo.PurchaseOrder po
    INNER JOIN FSCDDB.dbo.Supplier s
        ON po.SupplierID = s.ID
    WHERE po.SupplierID <> 0
      AND po.Status = 2                  -- Solo Status = 2
      AND po.DateCreated >= @FechaInicio
      AND po.DateCreated < DATEADD(DAY, 1, @FechaFin)
    ORDER BY po.DateCreated DESC;
END
GO

CREATE OR ALTER PROCEDURE SP_OBTENER_REPORTE_ORDENES_COMPRA
(
    @FechaInicio DATE,
    @FechaFin DATE
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
    s.SupplierName  AS Proveedor,
    po.PONumber     AS NumeroOrden,
    'Cerrado'       AS Estado,
    po.ConfirmingTo AS Confirmacion,
    po.DateCreated  AS FechaCreacion,
    po.Remarks      AS Observaciones
	FROM [saba].[FSVENDB].[dbo].[PurchaseOrder] po
	INNER JOIN saba.[FSCDDB].[dbo].[Supplier] s
		ON po.SupplierID = s.ID
	WHERE po.SupplierID <> 0
	  AND po.Status = 2
	  AND po.DateCreated >= @FechaInicio
	  AND po.DateCreated < DATEADD(DAY, 1, @FechaFin)
	ORDER BY po.DateCreated DESC;
END
GO
	

-- Índice compuesto para Status + DateCreated
-- Permite filtrar rápidamente por Status y rango de fechas
CREATE NONCLUSTERED INDEX IX_PurchaseOrder_Status_DateCreated
ON FSCDDB.dbo.PurchaseOrder(Status, DateCreated);
GO

-- Índice para SupplierID
--  Mejora el rendimiento del JOIN con la tabla Supplier
CREATE NONCLUSTERED INDEX IX_PurchaseOrder_SupplierID
ON FSCDDB.dbo.PurchaseOrder(SupplierID);
GO

-- c) Índice filtrado opcional solo para Status = 2
--    Ideal si la mayoría de consultas son solo órdenes cerradas
CREATE NONCLUSTERED INDEX IX_PurchaseOrder_Status2_DateCreated
ON FSCDDB.dbo.PurchaseOrder(DateCreated)
WHERE Status = 2;
GO

-- ================================================
-- ACTUALIZAR ESTADÍSTICAS
-- ================================================
-- Mantiene los índices actualizados para que el optimizador genere planes de ejecución eficientes
UPDATE STATISTICS FSCDDB.dbo.PurchaseOrder;
GO