ALTER VIEW viewstocktakingfinalreport AS
SELECT
    i.Id AS ItemId,
    i.Name AS ItemName,
    i.UnitOfMeasurement AS UnitOfMeasurement,
    COALESCE(ia.Quantity, 0) AS SystemQuantity,
    COALESCE(st.StockTakingQuantity, 0) AS QuantityStockTaking,
    COALESCE(im.Amount, 0) AS QuantityMovement,
    im.MovementDate AS MovementDate,
    st.StockTakingDate AS StockTakingDate,
    ia.Id AS ItemsAddressingId,
    COALESCE(ad.WarehouseId, 0) AS WarehouseAddressingId,
    COALESCE(st.AddressingsInventoryStartId, ais.AddressingId, 0) AS WarehouseStocktakingId,
    CASE
		WHEN COALESCE(st.AddressingsInventoryStartId, ais.AddressingId, 0) = 0 THEN 1
        WHEN (COALESCE(ia.Quantity, 0) + COALESCE(im.Amount, 0) - COALESCE(st.StockTakingQuantity, 0)) = 0 AND COALESCE(st.StockTakingQuantity, 0) = 0 THEN 1
        WHEN (COALESCE(ia.Quantity, 0) + COALESCE(im.Amount, 0) - COALESCE(st.StockTakingQuantity, 0)) = 0 THEN 0
        WHEN (COALESCE(ia.Quantity, 0) + COALESCE(im.Amount, 0) - COALESCE(st.StockTakingQuantity, 0)) > 0 THEN 2
        WHEN (COALESCE(ia.Quantity, 0) + COALESCE(im.Amount, 0) - COALESCE(st.StockTakingQuantity, 0)) < 0 THEN 3
    END AS StockSituation,
    CASE
        WHEN ia.Id IS NULL THEN 3
        WHEN COUNT(DISTINCT ia.AddressingId) > 1 THEN 2
        WHEN COUNT(DISTINCT ia.AddressingId) = 0 THEN 
            CASE
                WHEN COALESCE(ia.Quantity, 0) = 0 AND COALESCE(st.StockTakingQuantity, 0) <> 0 THEN 3
                ELSE 1
            END
        WHEN COALESCE(st.AddressingsInventoryStartId, ais.AddressingId, 0) = 0 THEN 1
        ELSE 0
    END AS AddressingSituation,
    COALESCE(ia.Quantity, 0) + COALESCE(im.Amount, 0) - COALESCE(st.StockTakingQuantity, 0) AS Divergence
FROM
    inventory.item i
    LEFT JOIN inventory.itemsaddressing ia ON i.Id = ia.ItemId
    LEFT JOIN inventory.stocktaking st ON i.Id = st.ItemId
    LEFT JOIN inventory.inventorymovement im ON i.Id = im.ItemId
    LEFT JOIN inventory.addressing ad ON ia.AddressingId = ad.Id
    LEFT JOIN inventory.addressingsinventorystart ais ON st.AddressingsInventoryStartId = ais.Id
GROUP BY
    i.Id, i.Name, i.UnitOfMeasurement, im.MovementDate, st.StockTakingDate, ia.Id, st.Id, im.Id
ORDER BY
    i.Id;