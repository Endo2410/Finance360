---INSERTA PARA METER DATOS DE FSCDBD A LA TABLAS OREDENES COMPRA 
DECLARE @FECHAINICIO DATETIME = DATEADD(MONTH, -3, GETDATE());

INSERT INTO ORDENES_COMPRA
    (IDORDENCOMPRA, HQID, NUMERO_ORDEN, STATUS_ORDEN, FACTURA, FECHA_CREACION, TOTAL, IDESTADO, COMENTARIO)
SELECT
    PO.ID,
    P.HQID,
    PO.PONumber,
    'CERRADO',
    PO.ConfirmingTo,       -- FACTURA
    PO.DateCreated,
    SUM(POE.QuantityOrdered * POE.Price) AS TOTAL,
    1,                     -- PENDIENTE
    PO.Remarks              -- Comentario
FROM saba.FSCDDB.dbo.PurchaseOrder PO
INNER JOIN saba.FSCDDB.dbo.Supplier S
    ON PO.SupplierID = S.ID
INNER JOIN saba.FSCDDB.dbo.PurchaseOrderEntry POE
    ON PO.ID = POE.PurchaseOrderID
INNER JOIN dbo.PROVEEDORES P
    ON P.HQID = S.ID        -- Solo si existe en nuestra tabla
WHERE PO.DateCreated >= @FECHAINICIO
  AND PO.SupplierID NOT IN (0,1)  -- Ignorar SupplierID 0 y 1
GROUP BY
    PO.ID,
    P.HQID,
    PO.PONumber,
    PO.Status,
    PO.ConfirmingTo,
    PO.DateCreated,
    PO.Remarks
ORDER BY PO.DateCreated ASC;       -- Inserción por fecha ascendente
