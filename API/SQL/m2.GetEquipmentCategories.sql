USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[getEquipmentCategories]    Script Date: 12/29/2015 9:23:12 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[getEquipmentCategories]
@EquipmentOrAttachment VARCHAR(100) = 'Machine'
AS

IF @EquipmentOrAttachment = 'Machine'
	BEGIN
		SELECT ProductCategoryName
		FROM ProductCategory 
		WHERE ProductType = 'Machine' AND ProductCategoryName <> '' AND ProductCategoryName IS NOT NULL
		ORDER BY ProductCategoryName
	END
ELSE
	SELECT ProductCatalogName
	FROM ProductCatalog 
	WHERE ProductType = 'Attachment' AND ProductCatalogName <> '' AND ProductCatalogName IS NOT NULL
	ORDER BY ProductCatalogName