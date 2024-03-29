USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetManufacturerFilters]    Script Date: 12/3/2015 9:03:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[GetManufacturerFilters]
@Categories m2.CategoryFilters READONLY,
@AttachmentTypes m2.AttachmentType READONLY,
@EquipmentOrAttachment VARCHAR(100)
AS

DECLARE @CategoryCount INT = (SELECT COUNT(1) FROM @Categories);
DECLARE @AttachmentCount INT = (SELECT COUNT(1) FROM @AttachmentTypes);

IF(@EquipmentOrAttachment = 'Machine')
BEGIN
	IF (@CategoryCount > 0)
		BEGIN
			SELECT DISTINCT ManufacturerName
			FROM InventoryMaster inv
			INNER JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
			INNER JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
			WHERE ManufacturerName <> '' AND ManufacturerName IS NOT NULL AND pc.ProductType = @EquipmentOrAttachment
			AND pc.ProductCategoryName IN (
					SELECT Category FROM @Categories)
		END
ELSE
		SELECT DISTINCT ManufacturerName
		FROM InventoryMaster inv
		INNER JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
		INNER JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
		WHERE ManufacturerName <> '' AND ManufacturerName IS NOT NULL AND pc.ProductType = @EquipmentOrAttachment
END


IF(@EquipmentOrAttachment = 'Attachment')
BEGIN
	IF (@CategoryCount > 0 AND @AttachmentCount > 0)
		BEGIN
			SELECT DISTINCT ManufacturerName
			FROM InventoryMaster inv
			INNER JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
			INNER JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
			INNER JOIN ProductCatalog pcat ON pcat.ProductCatalogID = pc.ProductCatalogID
			WHERE ManufacturerName <> '' AND ManufacturerName IS NOT NULL AND pc.ProductType = @EquipmentOrAttachment
			AND pcat.ProductCatalogName IN (
					SELECT Category FROM @Categories)

			AND pc.ProductCategoryName IN (
					SELECT AttachmentType FROM @AttachmentTypes);
		END
	ELSE IF (@CategoryCount > 0 AND @AttachmentCount = 0)
		BEGIN
			SELECT DISTINCT ManufacturerName
			FROM InventoryMaster inv
			INNER JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
			INNER JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
			INNER JOIN ProductCatalog pcat ON pcat.ProductCatalogID = pc.ProductCatalogID
			WHERE ManufacturerName <> '' AND ManufacturerName IS NOT NULL AND pc.ProductType = @EquipmentOrAttachment
			AND pcat.ProductCatalogName IN (
					SELECT Category FROM @Categories)
		END
ELSE
		SELECT DISTINCT ManufacturerName
		FROM InventoryMaster inv
		INNER JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
		INNER JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
		WHERE ManufacturerName <> '' AND ManufacturerName IS NOT NULL AND pc.ProductType = @EquipmentOrAttachment
END
