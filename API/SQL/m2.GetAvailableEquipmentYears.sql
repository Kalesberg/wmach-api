USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[getAvailableEquipmentYears]    Script Date: 11/24/2015 2:49:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[getAvailableEquipmentYears]
@Categories m2.CategoryFilters READONLY,
@Makes m2.MakeFilters READONLY,
@Models m2.ModelFilters READONLY,
@RentalStatus m2.RentalStatusFilters READONLY,
@Divisions m2.DivisionFilters READONLY,
@SearchTerms m2.SearchTerms READONLY,
@LocationStatus m2.LocationStatusFilters READONLY,
@Locations m2.LocationFilters READONLY,
@EquipmentOrAttachment NVARCHAR (10) = 'Machine',
@AttachmentType m2.AttachmentType READONLY,
@FitsOnCategories m2.AttachmentFitsOn READONLY,
@FitsOnMakes m2.AttachmentFitsOn READONLY,
@FitsOnModels m2.AttachmentFitsOn READONLY,
@MinPrice INT = 0,
@MaxPrice INT = 0,
@MinYear INT = 0,
@MaxYear INT = 0
AS

DECLARE @Results AS m2.SearchResults;

INSERT INTO @Results
EXEC m2.EquipmentSearchMobile @Categories, @Makes, @Models, @RentalStatus, @Divisions, @SearchTerms, @LocationStatus, @Locations, @EquipmentOrAttachment, @AttachmentType, @FitsOnCategories, @FitsOnMakes, @FitsOnModels, @MinPrice, @MaxPrice, @MinYear, @MaxYear

SELECT DISTINCT CAST(yearManufactured AS VARCHAR(10)) FROM @Results
WHERE yearManufactured IS NOT NULL