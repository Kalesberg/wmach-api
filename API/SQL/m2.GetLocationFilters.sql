USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[getLocationFilters]    Script Date: 11/20/2015 10:42:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[getLocationFilters]
@Categories m2.CategoryFilters READONLY,
@Makes m2.MakeFilters READONLY,
@Models m2.ModelFilters READONLY
AS

DECLARE @CategoryCount INT = (SELECT COUNT(1) FROM @Categories);
DECLARE @MakeCount INT = (SELECT COUNT(1) FROM @Makes);
DECLARE @ModelCount INT = (SELECT COUNT(1) FROM @Models);

IF (@CategoryCount > 0 AND @MakeCount > 0 AND @ModelCount > 0)
	BEGIN
		SELECT DISTINCT City + ', ' + StateCode FROM Equipment e
		JOIN Address adr ON adr.AddressID = e.CurrentLocationID
		JOIN InventoryMaster inv ON inv.InventoryMasterID = e.InventoryMasterID
		JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
		JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
		WHERE (City IS NOT NULL AND City <> '') AND (StateCode IS NOT NULL AND StateCode <> '')
		AND manf.ManufacturerName IN (
				SELECT Make FROM @Makes)

		AND pc.ProductCategoryName IN (
				SELECT Category FROM @Categories)

		AND inv.ModelNum IN (
				SELECT Model FROM @Models);
	END

IF (@CategoryCount > 0 AND @MakeCount > 0 AND @ModelCount = 0)
	BEGIN
		SELECT DISTINCT City + ', ' + StateCode FROM Equipment e
		JOIN Address adr ON adr.AddressID = e.CurrentLocationID
		JOIN InventoryMaster inv ON inv.InventoryMasterID = e.InventoryMasterID
		JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
		JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
		WHERE (City IS NOT NULL AND City <> '') AND (StateCode IS NOT NULL AND StateCode <> '')
		AND manf.ManufacturerName IN (
				SELECT Make FROM @Makes)

		AND pc.ProductCategoryName IN (
				SELECT Category FROM @Categories);

	END

IF (@CategoryCount > 0 AND @MakeCount = 0 AND @ModelCount = 0)
	BEGIN
		SELECT DISTINCT City + ', ' + StateCode FROM Equipment e
		JOIN Address adr ON adr.AddressID = e.CurrentLocationID
		JOIN InventoryMaster inv ON inv.InventoryMasterID = e.InventoryMasterID
		JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
		WHERE (City IS NOT NULL AND City <> '') AND (StateCode IS NOT NULL AND StateCode <> '')
		AND pc.ProductCategoryName IN (
				SELECT Category FROM @Categories);

	END

IF (@CategoryCount = 0 AND @MakeCount > 0 AND @ModelCount = 0)
	BEGIN
		SELECT DISTINCT City + ', ' + StateCode FROM Equipment e
		JOIN Address adr ON adr.AddressID = e.CurrentLocationID
		JOIN InventoryMaster inv ON inv.InventoryMasterID = e.InventoryMasterID
		JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
		WHERE (City IS NOT NULL AND City <> '') AND (StateCode IS NOT NULL AND StateCode <> '')
		AND manf.ManufacturerName IN (
				SELECT Make FROM @Makes);
	END

IF (@CategoryCount = 0 AND @MakeCount = 0 AND @ModelCount > 0)
	BEGIN
		SELECT DISTINCT City + ', ' + StateCode FROM Equipment e
		JOIN Address adr ON adr.AddressID = e.CurrentLocationID
		JOIN InventoryMaster inv ON inv.InventoryMasterID = e.InventoryMasterID
		JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
		WHERE (City IS NOT NULL AND City <> '') AND (StateCode IS NOT NULL AND StateCode <> '')
		AND manf.ManufacturerName IN (
				SELECT Make FROM @Makes);
	END

IF (@CategoryCount = 0 AND @MakeCount > 0 AND @ModelCount > 0)
	BEGIN
		SELECT DISTINCT City + ', ' + StateCode FROM Equipment e
		JOIN Address adr ON adr.AddressID = e.CurrentLocationID
		JOIN InventoryMaster inv ON inv.InventoryMasterID = e.InventoryMasterID
		JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
		WHERE (City IS NOT NULL AND City <> '') AND (StateCode IS NOT NULL AND StateCode <> '')
		AND manf.ManufacturerName IN (
				SELECT Make FROM @Makes)

		AND inv.ModelNum IN (
				SELECT Model FROM @Models);
	END

IF (@CategoryCount > 0 AND @MakeCount = 0 AND @ModelCount > 0)
	BEGIN
		SELECT DISTINCT City + ', ' + StateCode FROM Equipment e
		JOIN Address adr ON adr.AddressID = e.CurrentLocationID
		JOIN InventoryMaster inv ON inv.InventoryMasterID = e.InventoryMasterID
		JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
		WHERE (City IS NOT NULL AND City <> '') AND (StateCode IS NOT NULL AND StateCode <> '')
		AND inv.ModelNum IN (
				SELECT Model FROM @Models)

		AND pc.ProductCategoryName IN (
				SELECT Category FROM @Categories);
	END
