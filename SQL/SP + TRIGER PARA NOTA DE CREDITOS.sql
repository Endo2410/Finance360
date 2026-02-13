CREATE OR ALTER PROCEDURE SP_LISTAR_NOTAS_CREDITO
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        NC.ID_NC,
        NC.NUMERO_NC,
        NC.FECHA_EMISION,
        NC.MONTO,
        NC.TIPO_ORIGEN,
        NC.ID_ORIGEN,
        NC.NUMERO_DOCUMENTO_ORIGEN,  -- <--- agregado
        NC.DOCUMENTO_ADJUNTO,        -- <--- agregado
        NC.FECHA_REGISTRO,

        P.ID_PROVEEDOR,
        P.NOMBRE_PROVEEDOR,

        E.IDESTADO,
        E.NOMBRE AS ESTADO

    FROM NOTA_CREDITO NC
    INNER JOIN PROVEEDORES P 
        ON P.ID_PROVEEDOR = NC.ID_PROVEEDOR
    INNER JOIN ESTADO E 
        ON E.IDESTADO = NC.IDESTADO
    ORDER BY NC.FECHA_REGISTRO DESC;
END
GO

-- Eliminar trigger si existía
IF OBJECT_ID('TR_GENERAR_NC_PAGO_PUBLICIDAD', 'TR') IS NOT NULL
    DROP TRIGGER TR_GENERAR_NC_PAGO_PUBLICIDAD;
GO


-- Crear secuencia global para NUMERO_NC
IF OBJECT_ID('SEQ_NUMERO_NC') IS NULL
BEGIN
    CREATE SEQUENCE SEQ_NUMERO_NC
        AS BIGINT           -- Tipo BIGINT, máximo ~9 trillones
        START WITH 1        -- Comenzamos desde 1
        INCREMENT BY 1      -- Incremento de 1 en 1
        MINVALUE 1          -- Mínimo 1
        NO MAXVALUE         -- Sin máximo, seguirá creciendo
        CACHE 10;           -- Opcional, mejora performance en inserciones masivas
END;
GO

CREATE OR ALTER TRIGGER TR_GENERAR_NC_PAGO_PUBLICIDAD
ON DETALLE_PAGO_PUBLICIDAD
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TIPO_DOC_NC INT = 3;  -- Ajusta según tu catálogo para "Nota de Crédito"

    INSERT INTO NOTA_CREDITO
    (
        ID_PROVEEDOR,
        NUMERO_NC,
        FECHA_EMISION,
        MONTO,
        IDESTADO,
        DOCUMENTO_ADJUNTO,
        TIPO_ORIGEN,
        ID_ORIGEN,
        NUMERO_DOCUMENTO_ORIGEN,
        FECHA_REGISTRO
    )
    SELECT 
        C.ID_PROVEEDOR,
        -- Número NC global con ceros a la izquierda (5 dígitos, crece ilimitado)
        RIGHT(REPLICATE('0', 5) + CAST(NEXT VALUE FOR SEQ_NUMERO_NC AS VARCHAR), 5),
        GETDATE(),
        D.MONTO_PAGADO,
        5,  -- ACTIVA
        D.COMPROBANTE,
        'PUBLICIDAD',
        P.ID_CAMPANIA,
        C.NUMERO_CAMPANIA,  -- Ahora tomamos el número de documento de la campaña
        GETDATE()
    FROM INSERTED D
    INNER JOIN PAGO_PUBLICIDAD P ON D.ID_PAGO = P.ID_PAGO
    INNER JOIN CAMPANIA_PUBLICITARIA C ON P.ID_CAMPANIA = C.ID_CAMPANIA
    WHERE D.ID_TIPO_DOC = @TIPO_DOC_NC;
END;
GO


-- Eliminar trigger si existía
IF OBJECT_ID('TR_GENERAR_NC_PAGO_VENCIDO', 'TR') IS NOT NULL
    DROP TRIGGER TR_GENERAR_NC_PAGO_VENCIDO;
GO

---vencidos notas de creditos 
CREATE OR ALTER TRIGGER TR_GENERAR_NC_PAGO_VENCIDO
ON DETALLE_PAGO_VENCIDO
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TIPO_DOC_NC INT = 3;  -- Ajusta según tu catálogo para "Nota de Crédito"

    INSERT INTO NOTA_CREDITO
    (
        ID_PROVEEDOR,
        NUMERO_NC,
        FECHA_EMISION,
        MONTO,
        IDESTADO,
        DOCUMENTO_ADJUNTO,
        TIPO_ORIGEN,
        ID_ORIGEN,
        NUMERO_DOCUMENTO_ORIGEN,
        FECHA_REGISTRO
    )
    SELECT 
       PR.ID_PROVEEDOR,  -- ahora sí el proveedor correcto
        -- Número NC global con ceros a la izquierda (5 dígitos)
        RIGHT(REPLICATE('0', 5) + CAST(NEXT VALUE FOR SEQ_NUMERO_NC AS VARCHAR), 5),
        GETDATE(),
        D.MONTO_PAGADO,
        5,  -- ACTIVA
        D.COMPROBANTE,
        'VENCIDO',
        V.IDVENCIDO,         -- id origen
        V.NUMERO_ORDEN,  -- numero documento de pago vencido
        GETDATE()
    FROM INSERTED D
    INNER JOIN PAGO_VENCIDO P ON D.ID_PAGO = P.ID_PAGO
    INNER JOIN VENCIDOS V ON P.ID_VENCIDO = V.IDVENCIDO
      INNER JOIN PROVEEDORES PR ON V.HQID = PR.HQID  -- join correcto
    WHERE D.ID_TIPO_DOC = @TIPO_DOC_NC;
END;
GO




--REBATE

-- Eliminar trigger si existía
IF OBJECT_ID('TR_GENERAR_NC_PAGO_REBATE', 'TR') IS NOT NULL
    DROP TRIGGER TR_GENERAR_NC_PAGO_REBATE;
GO

CREATE OR ALTER TRIGGER TR_GENERAR_NC_PAGO_REBATE
ON DETALLE_PAGO_REBATE
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TIPO_DOC_NC INT = 3;  -- Nota de Crédito

    INSERT INTO NOTA_CREDITO
    (
        ID_PROVEEDOR,
        NUMERO_NC,
        FECHA_EMISION,
        MONTO,
        IDESTADO,
        DOCUMENTO_ADJUNTO,
        TIPO_ORIGEN,
        ID_ORIGEN,
        NUMERO_DOCUMENTO_ORIGEN,
        FECHA_REGISTRO
    )
    SELECT
        A.ID_PROVEEDOR,  -- proveedor del acuerdo
        RIGHT(REPLICATE('0', 5) + CAST(NEXT VALUE FOR SEQ_NUMERO_NC AS VARCHAR), 5), -- NC global
        GETDATE(),
        D.MONTO_PAGADO,
        5,  -- ACTIVA
        D.COMPROBANTE,
        'REBATE',
        D.ID_EJECUCION,          -- id origen = ejecución rebate
                ER.NUMERO_DOCUMENTO_SOPORTE,
        GETDATE()
    FROM INSERTED D
    INNER JOIN PAGO_REBATE P ON D.ID_PAGO_REBATE = P.ID_PAGO_REBATE
    INNER JOIN EJECUCION_REBATE ER ON D.ID_EJECUCION = ER.ID_EJECUCION
    INNER JOIN ACUERDO_REBATE A ON ER.ID_ACUERDO = A.ID_ACUERDO
    WHERE D.ID_TIPO_DOC = @TIPO_DOC_NC;
END;
GO

