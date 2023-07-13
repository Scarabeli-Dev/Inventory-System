Create VIEW StockTakingReportView AS
SELECT
  i.Id AS ItemId,
  i.Name AS ItemName,
  i.UnitOfMeasurement AS UnitOfMeasurement,
  COALESCE(ia.Quantity, 0) AS InitialQuantity,
  COALESCE(st.StockTakingQuantity, 0) AS QuantityStockTaking,
  COALESCE(im.Amount, 0) AS QuantityMovement,
  COALESCE(im.MovementeType, 0) AS MovementeType,
CASE
    WHEN im.MovementDate < st.StockTakingDate THEN
        CASE
            WHEN im.MovementeType = 1 THEN COALESCE(st.StockTakingQuantity, 0) + COALESCE(im.Amount, 0)
            WHEN im.MovementeType = 2 THEN COALESCE(st.StockTakingQuantity, 0) - COALESCE(im.Amount, 0)
            ELSE COALESCE(st.StockTakingQuantity, 0)
        END
    ELSE COALESCE(st.StockTakingQuantity, 0)
END AS QuantityClosed
FROM item AS i
LEFT JOIN itemsaddressing AS ia ON i.Id = ia.ItemId
LEFT JOIN stocktaking AS st ON i.Id = st.ItemId
LEFT JOIN inventorymovement AS im ON i.Id = im.ItemId
ORDER BY i.Id;
