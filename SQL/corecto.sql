DBCC CHECKIDENT ('[PurchaseOrder]', NORESEED)
-- Current identity value = 9999999 (por ejemplo)

DECLARE @maxID INT;
SELECT @maxID = MAX(ID) FROM PurchaseOrder;
DBCC CHECKIDENT ('PurchaseOrder', RESEED, @maxID)

DECLARE @NextPONumber INT;

-- Tomamos el valor numérico más alto de PONumber
SELECT @NextPONumber = MAX(CAST(PONumber AS INT))
FROM PurchaseOrder
WHERE ISNUMERIC(PONumber) = 1; -- solo los que son números

-- Incrementamos 1
SET @NextPONumber = @NextPONumber + 1;

-- Insertamos la nueva orden con PONumber consecutivo
INSERT INTO PurchaseOrder (PONumber, Status, SupplierID, DateCreated, ConfirmingTo, Remarks)
VALUES (CAST(@NextPONumber AS VARCHAR(20)), 0, 0, GETDATE(), '', '');


SELECT *
FROM PurchaseOrder
WHERE PONumber = '9999999';

SELECT *
FROM PurchaseOrder
WHERE PONumber = '10000000';

SELECT MAX(CAST(PONumber AS BIGINT)) AS UltimoPONumber
FROM PurchaseOrder
WHERE ISNUMERIC(PONumber) = 1;

SELECT *
FROM PurchaseOrder
WHERE TRY_CAST(PONumber AS BIGINT) > 9999999;


DELETE FROM PurchaseOrder
WHERE TRY_CAST(PONumber AS BIGINT) > 9999999;




DECLARE @Start INT = CAST('9996540' AS INT);  -- último PONumber actual
DECLARE @End   INT = 9999999;               -- hasta dónde quieres llegar
DECLARE @Counter INT = @Start + 1;

WHILE @Counter <= @End
BEGIN
    INSERT INTO PurchaseOrder (PONumber, Status, SupplierID, DateCreated, ConfirmingTo, Remarks)
    VALUES (CAST(@Counter AS VARCHAR(20)), 0, 0, GETDATE(), '', '');

    SET @Counter = @Counter + 1;
END


select * from PurchaseOrderEntryDetail

CREATE TRIGGER trg_InsertPurchaseOrder
ON PurchaseOrder
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO PurchaseOrder (
        LastUpdated, POTitle, POType, StoreID, WorksheetID, PONumber,
        Status, DateCreated, [To], ShipTo, Requisitioner, ShipVia,
        FOBPoint, Terms, TaxRate, Shipping, Freight, ConfirmingTo,
        Remarks, SupplierID, OtherStoreID, CurrencyID, ExchangeRate,
        OtherPOID, InventoryLocation, IsPlaced, BatchNumber
    )
    SELECT
        ISNULL(i.LastUpdated, GETDATE()),
        ISNULL(i.POTitle, ''),
        ISNULL(i.POType, 0),
        ISNULL(i.StoreID, 0),
        ISNULL(i.WorksheetID, 0),
        CAST(ISNULL(MAX(TRY_CAST(PONumber AS BIGINT)), 0) + ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS NVARCHAR(20)),
        ISNULL(i.Status, 0),
        ISNULL(i.DateCreated, GETDATE()),
        ISNULL(i.[To], ''),
        ISNULL(i.ShipTo, ''),
        ISNULL(i.Requisitioner, ''),
        ISNULL(i.ShipVia, ''),
        ISNULL(i.FOBPoint, ''),
        ISNULL(i.Terms, ''),
        ISNULL(i.TaxRate, 0),
        ISNULL(i.Shipping, 0),
        ISNULL(i.Freight, ''),
        ISNULL(i.ConfirmingTo, ''),
        ISNULL(i.Remarks, ''),
        ISNULL(i.SupplierID, 0),
        ISNULL(i.OtherStoreID, 0),
        ISNULL(i.CurrencyID, 0),
        ISNULL(i.ExchangeRate, 1),
        ISNULL(i.OtherPOID, 0),
        ISNULL(i.InventoryLocation, 0),
        ISNULL(i.IsPlaced, 0),
        ISNULL(i.BatchNumber, 0)
    FROM inserted i
    CROSS JOIN (SELECT MAX(TRY_CAST(PONumber AS BIGINT)) AS MaxPO FROM PurchaseOrder) AS x;
END;
GO


CREATE TRIGGER trg_InsertPurchaseOrder
ON PurchaseOrder
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @maxPO BIGINT;

    -- Obtener el último PONumber como BIGINT
    SELECT @maxPO = MAX(TRY_CAST(PONumber AS BIGINT)) 
    FROM PurchaseOrder;

    -- Insertar los nuevos registros con PONumber consecutivo
    INSERT INTO PurchaseOrder (
        LastUpdated, POTitle, POType, StoreID, WorksheetID, PONumber,
        Status, DateCreated, [To], ShipTo, Requisitioner, ShipVia,
        FOBPoint, Terms, TaxRate, Shipping, Freight, ConfirmingTo,
        Remarks, SupplierID, OtherStoreID, CurrencyID, ExchangeRate,
        OtherPOID, InventoryLocation, IsPlaced, BatchNumber
    )
    SELECT
        ISNULL(i.LastUpdated, GETDATE()),
        ISNULL(i.POTitle, ''),
        ISNULL(i.POType, 0),
        ISNULL(i.StoreID, 0),
        ISNULL(i.WorksheetID, 0),
        CAST(@maxPO + ROW_NUMBER() OVER (ORDER BY i.ID) AS NVARCHAR(20)),
        ISNULL(i.Status, 0),
        ISNULL(i.DateCreated, GETDATE()),
        ISNULL(i.[To], ''),
        ISNULL(i.ShipTo, ''),
        ISNULL(i.Requisitioner, ''),
        ISNULL(i.ShipVia, ''),
        ISNULL(i.FOBPoint, ''),
        ISNULL(i.Terms, ''),
        ISNULL(i.TaxRate, 0),
        ISNULL(i.Shipping, 0),
        ISNULL(i.Freight, ''),
        ISNULL(i.ConfirmingTo, ''),
        ISNULL(i.Remarks, ''),
        ISNULL(i.SupplierID, 0),
        ISNULL(i.OtherStoreID, 0),
        ISNULL(i.CurrencyID, 0),
        ISNULL(i.ExchangeRate, 1),
        ISNULL(i.OtherPOID, 0),
        ISNULL(i.InventoryLocation, 0),
        ISNULL(i.IsPlaced, 0),
        ISNULL(i.BatchNumber, 0)
    FROM inserted i;
END;
GO
