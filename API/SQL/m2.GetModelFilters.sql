USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetModelFilters]    Script Date: 11/24/2015 2:48:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[GetModelFilters]
@Makes m2.MakeFilters READONLY,
@Categories m2.CategoryFilters READONLY,
@EquipmentOrAttachment VARCHAR(100)
AS

DECLARE @MakeCount INT = (SELECT COUNT(1) FROM @Makes);
DECLARE @CategoryCount INT = (SELECT COUNT(1) FROM @Categories);


IF(@EquipmentOrAttachment = 'Machine')
	BEGIN
	IF (@MakeCount > 0 AND @CategoryCount > 0)
		BEGIN
			SELECT DISTINCT ModelNum
			FROM InventoryMaster inv
			INNER JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
			INNER JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
			WHERE pc.ProductType = @EquipmentOrAttachment
		
			AND manf.ManufacturerName IN (
					SELECT Make FROM @Makes)

			AND pc.ProductCategoryName IN (
					SELECT Category FROM @Categories);
		END

	IF (@MakeCount > 0 AND @CategoryCount = 0)
		BEGIN
			SELECT DISTINCT ModelNum
			FROM InventoryMaster inv
			INNER JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
			INNER JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
			WHERE pc.ProductType = @EquipmentOrAttachment	
			AND manf.ManufacturerName IN (
					SELECT Make FROM @Makes)
		END

	IF (@MakeCount = 0 AND @CategoryCount > 0)
		BEGIN
			SELECT DISTINCT ModelNum
			FROM InventoryMaster inv
			INNER JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
			INNER JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
			WHERE pc.ProductType = @EquipmentOrAttachment
			AND pc.ProductCategoryName IN (
					SELECT Category FROM @Categories);
		END
	ELSE
		SELECT DISTINCT ModelNum
		FROM InventoryMaster inv
		INNER JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
		INNER JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
		WHERE pc.ProductType = @EquipmentOrAttachment AND ModelNum <> '' AND ModelNum IS NOT NULL
	END

IF(@EquipmentOrAttachment = 'Attachment')
	BEGIN
	IF (@MakeCount > 0 AND @CategoryCount > 0)
		BEGIN
			SELECT DISTINCT ModelNum
			FROM InventoryMaster inv
			INNER JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
			INNER JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
			INNER JOIN ProductCatalog pCat ON pCat.ProductCatalogID = pc.ProductCatalogID
			WHERE pc.ProductType = @EquipmentOrAttachment
		
			AND manf.ManufacturerName IN (
					SELECT Make FROM @Makes)

			AND pCat.ProductCatalogName IN (
					SELECT Category FROM @Categories);
		END

	IF (@MakeCount > 0 AND @CategoryCount = 0)
		BEGIN
			SELECT DISTINCT ModelNum
			FROM InventoryMaster inv
			INNER JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
			INNER JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
			WHERE pc.ProductType = @EquipmentOrAttachment	
			AND manf.ManufacturerName IN (
					SELECT Make FROM @Makes)
		END

	IF (@MakeCount = 0 AND @CategoryCount > 0)
		BEGIN
			SELECT DISTINCT ModelNum
			FROM InventoryMaster inv
			INNER JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
			INNER JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
			INNER JOIN ProductCatalog pCat ON pCat.ProductCatalogID = pc.ProductCatalogID
			WHERE pc.ProductType = @EquipmentOrAttachment
			AND pCat.ProductCatalogName IN (
					SELECT Category FROM @Categories);
		END
	ELSE
		SELECT DISTINCT ModelNum
		FROM InventoryMaster inv
		INNER JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
		INNER JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
		WHERE pc.ProductType = @EquipmentOrAttachment AND ModelNum <> '' AND ModelNum IS NOT NULL
	END