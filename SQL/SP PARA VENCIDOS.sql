-- Primero definimos un tipo de tabla para los detalles del pago de VENCIDOS
CREATE TYPE T_DETALLE_PAGO_VENCIDO AS TABLE
(
    MONTO_PAGADO DECIMAL(18,2),
    ID_TIPO_DOCUMENTO INT,
    COMPROBANTE VARCHAR(MAX)  -- solo ruta/nombre del archivo
);
GO

-- Procedimiento para registrar pago de un vencido
CREATE OR ALTER PROCEDURE SP_REGISTRAR_PAGO_VENCIDO
(
    @ID_VENCIDO INT,
    @FECHA_DOCUMENTO DATE,
    @MONTO_TOTAL DECIMAL(18,2),
    @OBSERVACION VARCHAR(300),
    @DETALLE T_DETALLE_PAGO_VENCIDO READONLY,
    @ID_PAGO INT OUTPUT,
    @NUMERO_DOCUMENTO_OUT VARCHAR(20) OUTPUT
)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE 
        @CORRELATIVO INT,
        @LARGO INT;

    BEGIN TRY
        BEGIN TRAN;

        -- Obtener el siguiente número de documento
        SELECT @CORRELATIVO = ISNULL(MAX(CAST(NUMERO_DOCUMENTO AS INT)), 0) + 1
        FROM PAGO_VENCIDO;  -- asumimos que tendrás tabla principal de pagos de VENCIDOS

        -- Mínimo 4 dígitos
        SET @LARGO = CASE WHEN LEN(@CORRELATIVO) < 4 THEN 4 ELSE LEN(@CORRELATIVO) END;

        -- Formatear con ceros a la izquierda
        SET @NUMERO_DOCUMENTO_OUT = RIGHT(REPLICATE('0', @LARGO) + CAST(@CORRELATIVO AS VARCHAR), @LARGO);

        -- Insertar pago principal
        INSERT INTO PAGO_VENCIDO
        (
            ID_VENCIDO,
            NUMERO_DOCUMENTO,
            FECHA_DOCUMENTO,
            MONTO_TOTAL,
            OBSERVACION,
            IDESTADO,
            FECHA_REGISTRO
        )
        VALUES
        (
            @ID_VENCIDO,
            @NUMERO_DOCUMENTO_OUT,
            @FECHA_DOCUMENTO,
            @MONTO_TOTAL,
            @OBSERVACION,
            6, -- pagado
            GETDATE()
        );

        SET @ID_PAGO = SCOPE_IDENTITY();

        -- Insertar los detalles del pago
        INSERT INTO DETALLE_PAGO_VENCIDO
        (
            ID_PAGO,
            MONTO_PAGADO,
            ID_TIPO_DOC,
            COMPROBANTE
        )
        SELECT
            @ID_PAGO,
            MONTO_PAGADO,
            ID_TIPO_DOCUMENTO,
            COMPROBANTE
        FROM @DETALLE;

        -- Actualizar el estado de la orden vencida a "pagado"
        UPDATE VENCIDOS
        SET 
            IDESTADO = 6,          -- 6 = PAGADO
            FECHA_PAGO = GETDATE()
        WHERE IDVENCIDO = @ID_VENCIDO;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END;
GO



CREATE OR ALTER PROCEDURE SP_LISTA_VENCIDOS
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        V.IDVENCIDO,
        V.IDORDENVENCIDO,
        V.NUMERO_ORDEN,
        V.STATUS_ORDEN,
        V.CONCEPTO,
        V.FECHA_CREACION,
        V.USUARIO,
        V.TOTAL AS MONTO_TOTAL,

        ISNULL(SUM(PV.MONTO_TOTAL), 0) AS MONTO_PAGADO,
        V.TOTAL - ISNULL(SUM(PV.MONTO_TOTAL), 0) AS SALDO_PENDIENTE,

        V.IDESTADO AS ID_ESTADO_PAGO,
        E.NOMBRE AS ESTADO_PAGO
    FROM VENCIDOS V
        LEFT JOIN PAGO_VENCIDO PV ON PV.ID_VENCIDO = V.IDVENCIDO
        LEFT JOIN ESTADO E ON E.IDESTADO = V.IDESTADO
    GROUP BY
        V.IDVENCIDO,
        V.IDORDENVENCIDO,
        V.NUMERO_ORDEN,
        V.STATUS_ORDEN,
        V.CONCEPTO,
        V.FECHA_CREACION,
        V.USUARIO,
        V.TOTAL,
        V.IDESTADO,
        E.NOMBRE
    ORDER BY V.FECHA_CREACION DESC;
END
GO

CREATE OR ALTER PROCEDURE SP_DETALLE_PAGO_VENCIDO
    @ID_VENCIDO INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        DPV.ID_DETALLE_PAGO AS IdDetallePago,
        PV.NUMERO_DOCUMENTO AS DocumentoPago,
        DPV.MONTO_PAGADO AS MontoPagado,
        TD.NOMBRE AS TipoDocumento,
        PV.FECHA_DOCUMENTO AS FechaDocumento,
        DPV.FECHA_REGISTRO AS FechaRegistro,
        DPV.COMPROBANTE AS Comprobante
    FROM DETALLE_PAGO_VENCIDO DPV
    INNER JOIN PAGO_VENCIDO PV ON PV.ID_PAGO = DPV.ID_PAGO
    LEFT JOIN TIPO_DOCUMENTO_PAGO TD ON TD.ID_TIPO_DOC = DPV.ID_TIPO_DOC
    WHERE PV.ID_VENCIDO = @ID_VENCIDO
    ORDER BY DPV.FECHA_REGISTRO ASC;
END
GO
