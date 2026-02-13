CREATE OR ALTER TRIGGER trg_InsertarEstadoCuenta
ON dbo.CAMPANIA_PUBLICITARIA
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ID INT, @Inicio DATE, @Fin DATE, @Monto DECIMAL(18,2), @Intervalo INT;
    DECLARE @TipoIntervalo VARCHAR(10), @Cantidad INT, @i INT;

    DECLARE cur CURSOR FOR
        SELECT i.ID_CAMPANIA, i.FECHA_INICIO, i.FECHA_FIN, i.MONTO_INVERSION,
               m.VALOR_INTERVALO, m.TIPO_INTERVALO
        FROM INSERTED i
        INNER JOIN MODALIDAD m ON i.ID_MODALIDAD = m.ID_MODALIDAD;

    OPEN cur;
    FETCH NEXT FROM cur INTO @ID, @Inicio, @Fin, @Monto, @Intervalo, @TipoIntervalo;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Calcular cantidad de cuotas
        IF @TipoIntervalo = 'DIA'
            SET @Cantidad = DATEDIFF(DAY, @Inicio, @Fin) / @Intervalo;
        ELSE IF @TipoIntervalo = 'MES'
            SET @Cantidad = DATEDIFF(MONTH, @Inicio, @Fin) / @Intervalo;
        ELSE IF @TipoIntervalo = 'ANIO'
            SET @Cantidad = DATEDIFF(YEAR, @Inicio, @Fin) / @Intervalo;

        IF @Cantidad = 0 SET @Cantidad = 1;

        SET @i = 1;
        WHILE @i <= @Cantidad
        BEGIN
            INSERT INTO ESTADO_CUENTA_PUBLICIDAD (ID_CAMPANIA, NUMERO_CUOTA, FECHA_PAGO_PROGRAMADA, MONTO_CUOTA, IDESTADO)
            VALUES (
                @ID,
                @i,
                CASE 
                    WHEN @TipoIntervalo = 'DIA' THEN DATEADD(DAY, @Intervalo * @i, @Inicio)
                    WHEN @TipoIntervalo = 'MES' THEN DATEADD(MONTH, @Intervalo * @i, @Inicio)
                    WHEN @TipoIntervalo = 'ANIO' THEN DATEADD(YEAR, @Intervalo * @i, @Inicio)
                END,
                @Monto / @Cantidad,
                5 -- Pendiente
            );

            SET @i = @i + 1;
        END

        FETCH NEXT FROM cur INTO @ID, @Inicio, @Fin, @Monto, @Intervalo, @TipoIntervalo;
    END

    CLOSE cur;
    DEALLOCATE cur;
END
GO


CREATE OR ALTER TRIGGER TR_ESTADO_PAGADO
ON DETALLE_PAGO_PUBLICIDAD
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE ECP
    SET IDESTADO = 6 -- PAGADO
    FROM ESTADO_CUENTA_PUBLICIDAD ECP
    INNER JOIN inserted I
        ON ECP.ID_ESTADO_CUENTA = I.ID_ESTADO_CUENTA;
END;
GO


CREATE OR ALTER TRIGGER TR_ACTUALIZAR_ESTADO_PAGO_CAMPANIA
ON DETALLE_PAGO_PUBLICIDAD
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Actualiza el estado de pago de la campaña según los pagos realizados
    UPDATE C
    SET IDESTADO_PAGO = CASE
                            WHEN C.MONTO_INVERSION <= ISNULL(PAGOS_TOTALES.MontoPagado, 0)
                                THEN 6  -- PAGADO
                            WHEN ISNULL(PAGOS_TOTALES.MontoPagado, 0) > 0
                                THEN 7  -- PARCIAL
                            ELSE 5     -- PENDIENTE
                        END
    FROM CAMPANIA_PUBLICITARIA C
    INNER JOIN (
        -- Suma todos los pagos realizados por campaña
        SELECT P.ID_CAMPANIA, SUM(DPP.MONTO_PAGADO) AS MontoPagado
        FROM PAGO_PUBLICIDAD P
        INNER JOIN DETALLE_PAGO_PUBLICIDAD DPP
            ON P.ID_PAGO = DPP.ID_PAGO
        GROUP BY P.ID_CAMPANIA
    ) AS PAGOS_TOTALES
        ON PAGOS_TOTALES.ID_CAMPANIA = C.ID_CAMPANIA;
END;
GO
