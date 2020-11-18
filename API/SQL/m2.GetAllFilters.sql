USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetAllFilters]    Script Date: 11/20/2015 10:40:59 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[GetAllFilters] AS

SELECT DISTINCT 'Model', ModelNum, pc.ProductType 
FROM InventoryMaster inv
JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID 
WHERE ModelNum <> '' AND ModelNum IS NOT NULL

UNION

SELECT DISTINCT 'Make', ManufacturerName, pc.ProductType 
FROM InventoryMaster inv
JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
JOIN ProductCategory pc ON  pc.ProductCategoryID = inv.ProductCategoryID
WHERE ManufacturerName <> '' AND ManufacturerName IS NOT NULL

UNION

--Attachment Categories
SELECT 'Categories', ProductCatalogName, 'Attachment' 
FROM ProductCatalog 
WHERE ProductType = 'Attachment' AND ProductCatalogName <> ''

UNION

--Machine Categories
SELECT 'Categories', ProductCategoryName, 'Machine'
FROM ProductCategory 
WHERE ProductType = 'Machine'

UNION

SELECT DISTINCT 'Division', DivisionShortName, 'NA'
FROM Division 
WHERE DivisionShortName <> '' AND DivisionShortName IS NOT NULL

UNION

SELECT DISTINCT 'RentalStatus', RentalStatus, 'NA'
FROM Equipment 
WHERE RentalStatus <> '' AND RentalStatus IS NOT NULL

UNION 

SELECT DISTINCT 'LocationStatus', LocationStatus, 'NA'
FROM Equipment 
WHERE LocationStatus <> '' AND LocationStatus IS NOT NULL

UNION

SELECT DISTINCT 'AttachmentType', productcategoryname, 'NA'
FROM ProductCategory WHERE ProductCatalogID = 6 