SELECT * FROM m2.AttachmentFitsOn

select top 100 * From EquipmentAttachment

truncate table m2.AttachmentFitsOn

INSERT INTO m2.AttachmentFitsOn
SELECT ProductCategoryName, ManufacturerName, inv.ModelNum, ea.AttachmentEquipmentID
FROM InventoryMaster inv
JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
JOIN EquipmentAttachment ea ON ea.MachineInventoryMasterID = inv.InventoryMasterID