--OBTENER PROVEEDORES = SUPLIERS DE FSQHDB
CREATE OR ALTER PROCEDURE SP_OBTENER_PROVEEDORES
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ID_PROVEEDOR,
        PAIS,
        HQID,
        FECHA_ACTUALIZACION,
        NOMBRE_PROVEEDOR,
        NOMBRE_CONTACTO,
        CIUDAD,
        DIRECCION,
        CORREO,
        SITIO_WEB,
        RUC,
        NUMERO_TELEFONO,
        FAX,
        TERMINOS
    FROM PROVEEDORES
    ORDER BY HQID;
END
GO

--SINCRONIZACION ENTRE HQ Y PROVEEDOR
CREATE OR ALTER PROCEDURE SP_SINCRONIZAR_PROVEEDORES
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @actualizados INT = 0,
        @insertados INT = 0,
        @mensaje NVARCHAR(4000);

    BEGIN TRY
        BEGIN TRAN;

        /* =========================
           ACTUALIZAR SOLO SI CAMBIA
           ========================= */
        UPDATE p
        SET 
            p.PAIS = s.Country,
            p.FECHA_ACTUALIZACION = s.LastUpdated,
            p.NOMBRE_CONTACTO = s.ContactName,
            p.CIUDAD = s.City,
            p.DIRECCION = LTRIM(RTRIM(s.Address1 + ' ' + s.Address2)),
            p.CORREO = s.EmailAddress,
            p.SITIO_WEB = s.WebPageAddress,
            p.NUMERO_TELEFONO = s.PhoneNumber,
            p.FAX = s.FaxNumber,
            p.TERMINOS = s.Terms
        FROM dbo.PROVEEDORES p
        INNER JOIN saba.FSHQDB.dbo.Supplier s
            ON p.RUC COLLATE DATABASE_DEFAULT = s.AccountNumber COLLATE DATABASE_DEFAULT
           AND p.NOMBRE_PROVEEDOR COLLATE DATABASE_DEFAULT = s.SupplierName COLLATE DATABASE_DEFAULT
        WHERE
            ISNULL(p.PAIS,'') <> ISNULL(s.Country COLLATE DATABASE_DEFAULT,'')
         OR ISNULL(p.NOMBRE_CONTACTO,'') <> ISNULL(s.ContactName COLLATE DATABASE_DEFAULT,'')
         OR ISNULL(p.CIUDAD,'') <> ISNULL(s.City COLLATE DATABASE_DEFAULT,'')
         OR ISNULL(p.DIRECCION,'') <> ISNULL(LTRIM(RTRIM(s.Address1 + ' ' + s.Address2)) COLLATE DATABASE_DEFAULT,'')
         OR ISNULL(p.CORREO,'') <> ISNULL(s.EmailAddress COLLATE DATABASE_DEFAULT,'')
         OR ISNULL(p.SITIO_WEB,'') <> ISNULL(s.WebPageAddress COLLATE DATABASE_DEFAULT,'')
         OR ISNULL(p.NUMERO_TELEFONO,'') <> ISNULL(s.PhoneNumber COLLATE DATABASE_DEFAULT,'')
         OR ISNULL(p.FAX,'') <> ISNULL(s.FaxNumber COLLATE DATABASE_DEFAULT,'')
         OR ISNULL(p.TERMINOS,'') <> ISNULL(s.Terms COLLATE DATABASE_DEFAULT,'');

        SET @actualizados = @@ROWCOUNT;

        /* =========================
           INSERTAR NUEVOS
           ========================= */
        INSERT INTO dbo.PROVEEDORES (
            PAIS,
            HQID,
            FECHA_ACTUALIZACION,
            NOMBRE_PROVEEDOR,
            NOMBRE_CONTACTO,
            CIUDAD,
            DIRECCION,
            CORREO,
            SITIO_WEB,
            RUC,
            NUMERO_TELEFONO,
            FAX,
            TERMINOS
        )
        SELECT
            s.Country,
            s.ID,
            s.LastUpdated,
            s.SupplierName,
            s.ContactName,
            s.City,
            LTRIM(RTRIM(s.Address1 + ' ' + s.Address2)),
            s.EmailAddress,
            s.WebPageAddress,
            s.AccountNumber,
            s.PhoneNumber,
            s.FaxNumber,
            s.Terms
        FROM saba.FSHQDB.dbo.Supplier s
        WHERE NOT EXISTS (
            SELECT 1
            FROM dbo.PROVEEDORES p
            WHERE p.NOMBRE_PROVEEDOR COLLATE DATABASE_DEFAULT = s.SupplierName COLLATE DATABASE_DEFAULT
              AND p.RUC COLLATE DATABASE_DEFAULT = s.AccountNumber COLLATE DATABASE_DEFAULT
        );

        SET @insertados = @@ROWCOUNT;

        COMMIT TRAN;

        SET @mensaje = 'Sincronización exitosa';

        SELECT 
            1 AS Exito,
            @mensaje AS Mensaje,
            @actualizados AS ProveedoresActualizados,
            @insertados AS ProveedoresInsertados;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        SELECT 
            0 AS Exito,
            ERROR_MESSAGE() AS Mensaje,
            ERROR_LINE() AS LineaError,
            ERROR_PROCEDURE() AS Procedimiento;
    END CATCH
END;
GO

--OBTENENER COMPRAS DE FSCDBD A MI TABLAS ORDEN DE COMPRA 
CREATE OR ALTER PROCEDURE SP_OBTENER_ORDENES_COMPRA
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        OC.IDORDEN,
        OC.IDORDENCOMPRA,
		OC.HQID, 
        P.NOMBRE_PROVEEDOR AS PROVEEDOR,  -- Nombre del proveedor desde PROVEEDORES
        OC.NUMERO_ORDEN,
        OC.STATUS_ORDEN,
        OC.FACTURA,
        OC.FECHA_CREACION,
        OC.TOTAL,
        OC.IDESTADO,
        OC.COMENTARIO
    FROM ORDENES_COMPRA OC
    INNER JOIN PROVEEDORES P
        ON OC.HQID = P.HQID
END;
GO

CREATE OR ALTER PROCEDURE SP_OBTENER_ORDENES_COMPRA
(
    @FECHA_INICIO DATE,
    @FECHA_FIN    DATE
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        OC.IDORDEN,
        OC.IDORDENCOMPRA,
        OC.HQID,
        P.NOMBRE_PROVEEDOR AS PROVEEDOR,
        OC.NUMERO_ORDEN,
        OC.STATUS_ORDEN,
        OC.FACTURA,
        OC.FECHA_CREACION,
        OC.TOTAL,
        OC.IDESTADO,
        E.NOMBRE AS NOMBRE_ESTADO,
        OC.COMENTARIO
    FROM ORDENES_COMPRA OC
    INNER JOIN PROVEEDORES P
        ON OC.HQID = P.HQID
    INNER JOIN ESTADO_ORDEN E
        ON OC.IDESTADO = E.IDESTADO
    WHERE OC.FECHA_CREACION >= @FECHA_INICIO
      AND OC.FECHA_CREACION < DATEADD(DAY,1,@FECHA_FIN)
      AND OC.IDESTADO = 1  -- solo pendientes
    ORDER BY OC.IDORDEN DESC; -- descendente por ID
END;
GO



--SP QUE INSERTA DE FSCDBD A TABLA ORDENES COMPRA
CREATE OR ALTER PROCEDURE SP_INSERTAR_ORDENES_COMPRA
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @FECHAINICIO DATETIME;
    SELECT @FECHAINICIO = ISNULL(MAX(FECHA_CREACION), DATEADD(MONTH, -3, GETDATE()))
    FROM ORDENES_COMPRA;

    DECLARE @FilasInsertadas INT = 0;

    -- Tabla temporal
    CREATE TABLE #OrdenesTemp (
        IDORDENCOMPRA INT,
        HQID INT,
        NUMERO_ORDEN NVARCHAR(50),
        STATUS_ORDEN NVARCHAR(50),
        FACTURA NVARCHAR(100),
        FECHA_CREACION DATETIME,
        TOTAL DECIMAL(18,2),
        IDESTADO INT,
        COMENTARIO NVARCHAR(MAX)
    );

    INSERT INTO #OrdenesTemp (IDORDENCOMPRA, HQID, NUMERO_ORDEN, STATUS_ORDEN, FACTURA, FECHA_CREACION, TOTAL, IDESTADO, COMENTARIO)
    SELECT
        PO.ID,
        P.HQID,
        PO.PONumber,
        'CERRADO',
        PO.ConfirmingTo,
        PO.DateCreated,
        SUM(ISNULL(POE.QuantityOrdered,0) * ISNULL(POE.Price,0)) AS TOTAL,
        1,
        PO.Remarks
    FROM saba.FSCDDB.dbo.PurchaseOrder PO
    INNER JOIN saba.FSCDDB.dbo.PurchaseOrderEntry POE ON PO.ID = POE.PurchaseOrderID
    INNER JOIN saba.FSCDDB.dbo.Supplier S ON PO.SupplierID = S.ID
    LEFT JOIN dbo.PROVEEDORES P ON P.HQID = S.ID
    WHERE PO.DateCreated > @FECHAINICIO
      AND PO.SupplierID NOT IN (0,1)
    GROUP BY PO.ID, P.HQID, PO.PONumber, PO.Status, PO.ConfirmingTo, PO.DateCreated, PO.Remarks;

    -- Insertar solo los que no existan
    INSERT INTO ORDENES_COMPRA (IDORDENCOMPRA, HQID, NUMERO_ORDEN, STATUS_ORDEN, FACTURA, FECHA_CREACION, TOTAL, IDESTADO, COMENTARIO)
    SELECT t.IDORDENCOMPRA, t.HQID, t.NUMERO_ORDEN, t.STATUS_ORDEN, t.FACTURA, t.FECHA_CREACION, t.TOTAL, t.IDESTADO, t.COMENTARIO
    FROM #OrdenesTemp t
    LEFT JOIN ORDENES_COMPRA oc ON oc.IDORDENCOMPRA = t.IDORDENCOMPRA
    WHERE oc.IDORDENCOMPRA IS NULL;

    SET @FilasInsertadas = @@ROWCOUNT; -- Aquí capturamos cuántas filas se insertaron

    DROP TABLE #OrdenesTemp;

    -- Devolver el resultado
    SELECT @FilasInsertadas AS FilasInsertadas;
END;
GO





------VIENE DE VENCIDO 
CREATE OR ALTER PROCEDURE SP_OBTENER_ORDENES_VENCIDAS
(
    @FECHA_INICIO DATE,
    @FECHA_FIN    DATE
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        V.IDVENCIDO,
        V.IDORDENVENCIDO,
        V.HQID,
        P.NOMBRE_PROVEEDOR AS PROVEEDOR,
        V.NUMERO_ORDEN,
        V.STATUS_ORDEN,
        V.CONCEPTO,
        V.FECHA_CREACION,
        V.USUARIO,
        V.TOTAL,
        V.IDESTADO,
        E.NOMBRE AS NOMBRE_ESTADO,
        V.FECHA_PAGO
    FROM VENCIDOS V
    INNER JOIN PROVEEDORES P
        ON V.HQID = P.HQID
    INNER JOIN ESTADO E
        ON V.IDESTADO = E.IDESTADO
    WHERE V.FECHA_CREACION >= @FECHA_INICIO
      AND V.FECHA_CREACION < DATEADD(DAY, 1, @FECHA_FIN)
      AND V.IDESTADO = 5 -- pendientes
    ORDER BY V.IDVENCIDO DESC;
END;
GO

----INSERT
CREATE OR ALTER PROCEDURE SP_INSERTAR_ORDENES_VENCIDAS
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @FilasInsertadas INT = 0;

    -- Tabla temporal
    CREATE TABLE #VencidosTemp
    (
        IDORDENVENCIDO INT,
        HQID INT,
        NUMERO_ORDEN NVARCHAR(50),
        STATUS_ORDEN NVARCHAR(50),
        CONCEPTO NVARCHAR(150),
        FECHA_CREACION DATETIME,
        USUARIO NVARCHAR(50),
        TOTAL DECIMAL(18,2),
        IDESTADO INT
    );

    INSERT INTO #VencidosTemp
    (
        IDORDENVENCIDO,
        HQID,
        NUMERO_ORDEN,
        STATUS_ORDEN,
        CONCEPTO,
        FECHA_CREACION,
        USUARIO,
        TOTAL,
        IDESTADO
    )
    SELECT
        PO.ID,
        P.HQID,
        PO.PONumber,
        PO.Status,
        PO.Remarks,
        PO.DateCreated,
        PO.Requisitioner,              -- USUARIO desde FSCDBD
        SUM(ISNULL(POE.QuantityOrdered,0) * ISNULL(POE.Price,0)) AS TOTAL,
        5                               -- Pendiente
    FROM saba.FSVENDB.dbo.PurchaseOrder PO
    INNER JOIN saba.FSVENDB.dbo.PurchaseOrderEntry POE
        ON PO.ID = POE.PurchaseOrderID
    INNER JOIN saba.FSVENDB.dbo.Supplier S
        ON PO.SupplierID = S.ID
    LEFT JOIN dbo.PROVEEDORES P
        ON P.HQID = S.ID
    WHERE PO.SupplierID <> 0
      AND PO.DateCreated >= '2025-12-01'   -- SOLO DICIEMBRE 2025+
    GROUP BY
        PO.ID,
        P.HQID,
        PO.PONumber,
        PO.Status,
        PO.Remarks,
        PO.DateCreated,
        PO.Requisitioner;

    -- Insertar solo los que no existan
    INSERT INTO VENCIDOS
    (
        IDORDENVENCIDO,
        HQID,
        NUMERO_ORDEN,
        STATUS_ORDEN,
        CONCEPTO,
        FECHA_CREACION,
        USUARIO,
        TOTAL,
        IDESTADO
    )
    SELECT
        t.IDORDENVENCIDO,
        t.HQID,
        t.NUMERO_ORDEN,
        t.STATUS_ORDEN,
        t.CONCEPTO,
        t.FECHA_CREACION,
        t.USUARIO,
        t.TOTAL,
        t.IDESTADO
    FROM #VencidosTemp t
    LEFT JOIN VENCIDOS v
        ON v.IDORDENVENCIDO = t.IDORDENVENCIDO
    WHERE v.IDORDENVENCIDO IS NULL;

    SET @FilasInsertadas = @@ROWCOUNT;

    DROP TABLE #VencidosTemp;

    SELECT @FilasInsertadas AS FilasInsertadas;
END;
GO

select * from ESTADO
DECLARE @PONumber NVARCHAR(25) = '0007466';

SELECT
    po.ID,
    s.SupplierName,
    po.PONumber,
    po.Status,
    po.DateCreated,
	[Remarks],
	Requisitioner,
    (SELECT SUM(poe.QuantityOrdered * poe.Price)
     FROM dbo.PurchaseOrderEntry poe
     WHERE poe.PurchaseOrderID = po.ID) AS TotalPrice
FROM dbo.PurchaseOrder po
INNER JOIN dbo.Supplier s
    ON po.SupplierID = s.ID
WHERE po.SupplierID <> 0
  AND po.PONumber = @PONumber;
