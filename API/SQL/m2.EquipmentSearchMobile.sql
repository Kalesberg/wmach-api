USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[EquipmentSearchMobile]    Script Date: 4/10/2017 10:32:57 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[EquipmentSearchMobile]
@Categories m2.CategoryFilters READONLY,
@Makes m2.MakeFilters READONLY,
@Models m2.ModelFilters READONLY,
@RentalStatus m2.RentalStatusFilters READONLY,
@Divisions m2.DivisionFilters READONLY,
@SearchTerms m2.SearchTerms READONLY,
@LocationStatus m2.LocationStatusFilters READONLY,
@Locations m2.LocationFilters READONLY,
@EquipmentOrAttachment NVARCHAR(10) = 'Machine',
@AttachmentType m2.AttachmentType READONLY,
@FitsOnCategories m2.AttachmentFitsOn READONLY,
@FitsOnMakes m2.AttachmentFitsOn READONLY,
@FitsOnModels m2.AttachmentFitsOn READONLY,
@MinPrice INT = 0,
@MaxPrice INT = 0,
@MinYear INT = 0,
@MaxYear INT = 0,
@OrderBy VARCHAR(100) = 'ModelNum',
@OrderDirection VARCHAR(5) = 'desc',
@PublicViewable BIT = 0,
@ForSale BIT = 0
AS

DECLARE @Results AS m2.SearchTermLookup;

DECLARE @CompareCategories TABLE (
value NVARCHAR(200)
)

DECLARE @CompareManufacturers TABLE (
value NVARCHAR(200)
)

DECLARE @CompareModels TABLE (
value NVARCHAR(200)
)

DECLARE @CompareSerialNums TABLE (
value NVARCHAR(200)
)

DECLARE @CompareRentalStatus TABLE (
value NVARCHAR(200)
)

DECLARE @CompareDivision TABLE (
value NVARCHAR(200)
)

DECLARE @CompareCity TABLE (
value NVARCHAR(200)
)

DECLARE @CompareState TABLE (
value NVARCHAR(200)
)

DECLARE @CompareCountry TABLE (
value NVARCHAR(200)
)

DECLARE @CityLookup AS m2.LocationFilters;
DECLARE @StateLookup AS m2.LocationFilters;
DECLARE @CountryLookup AS m2.LocationFilters;

INSERT INTO @CompareCategories
SELECT ProductCategoryName FROM ProductCategory

INSERT INTO @CompareManufacturers
SELECT ManufacturerName FROM Manufacturer

INSERT INTO @CompareModels
SELECT ModelNum FROM InventoryMaster

INSERT INTO @CompareSerialNums
SELECT SerialNum FROM Equipment

INSERT INTO @CompareRentalStatus
SELECT DISTINCT RentalStatus FROM Equipment

INSERT INTO @CompareDivision
SELECT DivisionShortName FROM Division


INSERT INTO @CompareCity
SELECT DISTINCT City 
FROM Address
WHERE (City IS NOT NULL AND City <> '')

INSERT INTO @CompareState
SELECT DISTINCT StateCode 
FROM Address
WHERE (StateCode IS NOT NULL AND StateCode <> '')

INSERT INTO @CompareCountry
SELECT DISTINCT CountryName
FROM Address
WHERE (CountryName IS NOT NULL AND CountryName <> '')

DECLARE @QueryItem NVARCHAR(50);

DECLARE Enumerator CURSOR FAST_FORWARD FOR
SELECT searchTerm FROM @SearchTerms

OPEN Enumerator
FETCH NEXT FROM Enumerator
INTO @QueryItem

WHILE @@FETCH_STATUS = 0
	BEGIN

		INSERT INTO @Results
		SELECT 'ProductCategory', value FROM @CompareCategories WHERE value LIKE '%' + @QueryItem + '%'

		INSERT INTO @Results
		SELECT 'Manufacturer', value FROM @CompareManufacturers WHERE value LIKE '%' + @QueryItem + '%'

		INSERT INTO @Results
		SELECT 'Model', value FROM @CompareModels WHERE value LIKE '%' + @QueryItem + '%'

		INSERT INTO @Results
		SELECT 'SerialNum', value FROM @CompareSerialNums WHERE value LIKE '%' + @QueryItem + '%'

		INSERT INTO @Results
		SELECT 'RentalStatus', value FROM @CompareRentalStatus WHERE value LIKE '%' + @QueryItem + '%'

		INSERT INTO @Results
		SELECT 'Division', value FROM @CompareDivision WHERE value LIKE '%' + @QueryItem + '%'

	FETCH NEXT FROM Enumerator
	INTO @QueryItem

	END

CLOSE Enumerator
DEALLOCATE Enumerator


--Location Lookup
DECLARE LocationCursor CURSOR FAST_FORWARD FOR
SELECT Location FROM @Locations

OPEN LocationCursor
FETCH NEXT FROM LocationCursor
INTO @QueryItem

WHILE @@FETCH_STATUS = 0
	BEGIN

		INSERT INTO @CityLookup
		SELECT value FROM @CompareCity WHERE value LIKE @QueryItem + '%'

		INSERT INTO @StateLookup
		SELECT value FROM @CompareState WHERE value LIKE @QueryItem + '%'

		INSERT INTO @CountryLookup
		SELECT value FROM @CompareCountry WHERE value LIKE @QueryItem + '%'

	FETCH NEXT FROM LocationCursor
	INTO @QueryItem

	END

CLOSE LocationCursor
DEALLOCATE LocationCursor

DECLARE @categoryCount INT = (SELECT COUNT(1) FROM @Categories);
DECLARE @makeCount INT = (SELECT COUNT(1) FROM @Makes);
DECLARE @modelCount INT = (SELECT COUNT(1) FROM @Models);
DECLARE @RentalStatusCount INT = (SELECT COUNT(1) FROM @RentalStatus);
DECLARE @divisionCount INT = (SELECT COUNT(1) FROM @Divisions);
DECLARE @locationCount INT = (SELECT COUNT(1) FROM @Locations);
DECLARE @locationStatusCount INT = (SELECT COUNT(1) FROM @LocationStatus);
DECLARE @searchTermCategoryCount INT = (SELECT COUNT(1) FROM @Results where fieldType = 'ProductCategory');
DECLARE @searchTermManufacturerCount INT = (SELECT COUNT(1) FROM @Results where fieldType = 'Manufacturer');
DECLARE @searchTermModelCount INT = (SELECT COUNT(1) FROM @Results where fieldType = 'Model');
DECLARE @searchTermSerialNumCount INT = (SELECT COUNT(1) FROM @Results where fieldType = 'SerialNum');
DECLARE @searchTermRentalStatusCount INT = (SELECT COUNT(1) FROM @Results where fieldType = 'RentalStatus');
DECLARE @searchTermDivisionCount INT = (SELECT COUNT(1) FROM @Results where fieldType = 'Division');
DECLARE @searchTermLocationCount INT = (SELECT COUNT(1) FROM @Results where fieldType = 'Location');
DECLARE @CityLookupCount INT = (SELECT COUNT(1) FROM @CityLookup);
DECLARE @StateLookupCount INT = (SELECT COUNT(1) FROM @StateLookup);
DECLARE @CountryLookupCount INT = (SELECT COUNT(1) FROM @CountryLookup);
DECLARE @FitsOnCategoryCount INT = (SELECT COUNT(1) FROM @FitsOnCategories);
DECLARE @FitsOnMakeCount INT = (SELECT COUNT(1) FROM @FitsOnMakes);
DECLARE @FitsOnModelCount INT = (SELECT COUNT(1) FROM @FitsOnModels);
DECLARE @AttachmentTypeCount INT = (SELECT COUNT(1) FROM @AttachmentType);

DECLARE @sqlString NVARCHAR(MAX) = 
'SELECT e.EquipmentID,
SerialNum,
ForRent,
ForSale,
PublicViewable,
PublicPriceViewable,
Price,
MinPrice,
MonthlyRentalRate,
InsuranceValue,
PropertyTag,
RentalStatus,
OwnerType,
LocationStatus,
ServiceStatus,
ManufacturedYear yearManufactured,
[hours],
Miles,
LastAppraisalDate,
DateAcquired,
PurchasePrice,
DateSold,
SoldPrice,
ModelNum ,
inv.Height,
inv.[Length],
inv.Width,
ManufacturerName,
pc.ProductCategoryName category,
DivisionShortName division,
pc.ProductType [equipmentType],
ad.City,
ad.StateCode State,
ad.CountryName Country,
ad.Address1 Address,
ad.PostalCode,
es.PositionLatitude,
es.PositionLongitude,
es.EquipmentSummaryDate,
picture.filename,
y.YardName,
e.RentalPurchaseOptionPrice RPOPRice,
MonthlyRentalRate * 1.05 internationalRentalRate,
attachmentType.ProductCategoryName attachmentType,
e.MarketingDescription,
currentContract.RentalRate ActualRentalRate,
pCat.ProductCatalogName attachmentCategory

FROM Equipment e
JOIN InventoryMaster inv ON inv.InventoryMasterID = e.InventoryMasterID
JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
LEFT JOIN ProductCatalog pCat ON pCat.ProductCatalogID = pc.ProductCatalogID
JOIN Division d on d.DivisionID = e.AssignedDivisionID
JOIN [Address] ad ON ad.AddressID = e.CurrentLocationID
LEFT JOIN (SELECT MAX(RentalRate) RentalRate, EquipmentID FROM
	(SELECT eq.EquipmentID, RentalRate FROM
	Contract con
	JOIN ContractDtl conDtl ON conDtl.ContractID = con.ContractID
	JOIN Equipment eq ON eq.EquipmentID = conDtl.EquipmentID
	WHERE con.ContractStatus = ''Open'')currentRate
	GROUP BY currentRate.EquipmentID)currentContract ON currentContract.EquipmentID = e.EquipmentID
LEFT JOIN Yard y on y.AddressID = e.CurrentLocationID
LEFT JOIN ProductCategory attachmentType ON attachmentType.ProductCategoryID = inv.ProductCategoryID
OUTER APPLY (SELECT Top 1 [QualCommID],[PositionLatitude],[PositionLongitude], [EquipmentSummaryDate] FROM [Mach1].[dbo].[EquipmentSummary] where QualCommID=e.SerialNum order by EquipmentSummaryDate desc) es
OUTER APPLY (SELECT Top 1 [fileName] FROM InventoryPicture WHERE EquipmentID = e.EquipmentID) picture
WHERE e.Active = 1 ';

IF (@EquipmentOrAttachment = 'Machine')
	BEGIN
	SET @sqlString = @sqlString + 'AND pc.ProductType = ''Machine'' ';
	END
ELSE IF(@EquipmentOrAttachment = 'Attachment')
	BEGIN
	SET @sqlString = @sqlString + 'AND pc.ProductType = ''Attachment'' ';
	END
ELSE IF(@EquipmentOrAttachment = 'Both')
	BEGIN
	SET @sqlString = @sqlString + 'AND (pc.ProductType = ''Machine'' OR pc.ProductType = ''Attachment'') ';
	END


IF EXISTS(SELECT RentalStatus FROM @RentalStatus WHERE RentalStatus = 'Returned')
	SET @sqlString = @sqlString + ' AND e.RentalStatus = ''Returned'' '
ELSE IF EXISTS(SELECT RentalStatus FROM @RentalStatus WHERE RentalStatus = 'Sold')
	SET @sqlString = @sqlString + ' AND e.RentalStatus = ''Sold'' '
ELSE SET @sqlString = @sqlString +  'AND e.RentalStatus NOT IN (''Sold'', ''Returned'') '


IF(@PublicViewable != 0)
		BEGIN
		SET @sqlString = @sqlString + ' AND e.PublicViewable = 1 ';
		END
		
IF(@ForSale != 0)
		BEGIN
		SET @sqlString = @sqlString + ' AND e.PublicViewable = 1 ';
		END


DECLARE @PredicateBase NVARCHAR(MAX) = 'AND (';
DECLARE @AndClause NVARCHAR(3) = 'AND '
DECLARE @OrClause NVARCHAR(3) = 'OR '
DECLARE @SelectOptionString NVARCHAR(MAX);
DECLARE @FilterOptionString NVARCHAR(MAX);
DECLARE @ProductCategoryPredicate NVARCHAR(250) = '(pc.ProductCategoryName IN (SELECT searchTerm FROM @Results)) ';
DECLARE @ManufacturerPredicate NVARCHAR(250) = '(manf.ManufacturerName IN (SELECT searchTerm FROM @Results)) ';
DECLARE @ModelPredicate NVARCHAR(250) = '(inv.ModelNum IN (SELECT searchTerm FROM @Results)) ';
DECLARE @SerialNumPredicate NVARCHAR(250) = '(e.SerialNum IN (SELECT searchTerm FROM @Results)) ';
DECLARE @RentalStatusPredicate NVARCHAR(250) = '(e.RentalStatus IN (SELECT searchTerm FROM @Results)) ';
DECLARE @DivisionPredicate NVARCHAR(250) = '(d.DivisionShortName IN (SELECT searchTerm FROM @Results)) ';
DECLARE @LocationPredicate NVARCHAR(250) = '(ad.city IN (SELECT searchTerm FROM @Results) OR ad.stateCode IN (SELECT searchTerm FROM @Results)) ';


--SELECT OPTIONS SEARCH TERMS
IF (@searchTermCategoryCount > 0 OR @searchTermManufacturerCount > 0 OR @searchTermModelCount > 0 OR @searchTermSerialNumCount > 0)
	BEGIN
		SET @SelectOptionString = @PredicateBase;
	END


IF (@searchTermCategoryCount > 0)
	BEGIN
		SET @SelectOptionString = @SelectOptionString + @ProductCategoryPredicate;
	END


IF (@searchTermManufacturerCount > 0)
	BEGIN
		IF(@SelectOptionString = @PredicateBase + @ProductCategoryPredicate)
			BEGIN
				SET @SelectOptionString = @SelectOptionString + @OrClause + @ManufacturerPredicate;
			END
		ELSE SET @SelectOptionString = @PredicateBase + @ManufacturerPredicate;
	END


IF (@searchTermModelCount > 0)
	BEGIN
		IF(@searchTermCategoryCount > 0 OR @searchTermManufacturerCount > 0)
			BEGIN
				SET @SelectOptionString = @SelectOptionString + @OrClause + @ModelPredicate;
			END
		ELSE IF (@searchTermCategoryCount < 1 AND @searchTermManufacturerCount < 1) SET @SelectOptionString = @PredicateBase + @ModelPredicate;
	END


IF (@searchTermSerialNumCount > 0)
	BEGIN
		IF(@searchTermCategoryCount > 0 OR @searchTermManufacturerCount > 0 OR @searchTermModelCount > 0)
			BEGIN
				SET @SelectOptionString = @SelectOptionString + @OrClause + @SerialNumPredicate;
			END
		ELSE IF (@searchTermCategoryCount < 1 AND @searchTermManufacturerCount < 1 AND @searchTermModelCount < 1) SET @SelectOptionString = @PredicateBase + @SerialNumPredicate;
	END


--FILTER OPTIONS SEARCH TERMS
IF (@searchTermRentalStatusCount > 0 OR @searchTermDivisionCount > 0 OR @searchTermLocationCount > 0)
	BEGIN
		SET @FilterOptionString = @PredicateBase;
	END

IF (@searchTermRentalStatusCount > 0)
	BEGIN
		SET @FilterOptionString = @FilterOptionString + @RentalStatusPredicate;
	END

IF (@searchTermDivisionCount > 0)
	BEGIN
		IF(@FilterOptionString = @PredicateBase + @RentalStatusPredicate)
			BEGIN
				SET @FilterOptionString = @FilterOptionString + @AndClause + @DivisionPredicate;
			END
		ELSE SET @FilterOptionString = @PredicateBase + @DivisionPredicate;
	END


--STANDARD FILTERS
IF (@categoryCount > 0 AND @EquipmentOrAttachment = 'Machine')
	BEGIN
	SET @sqlString = @sqlString + 'AND pc.ProductCategoryName IN (SELECT category FROM @Categories) ';
	END
ELSE IF(@categoryCount > 0 AND @EquipmentOrAttachment = 'Attachment')
	BEGIN
	SET @sqlString = @sqlString + 'AND pCat.ProductCatalogName IN (SELECT category FROM @Categories) ';
	END
ELSE IF(@categoryCount > 0 AND @EquipmentOrAttachment = 'Both')
	BEGIN
	SET @sqlString = @sqlString + 'AND (pc.ProductCategoryName IN (SELECT category FROM @Categories) OR pCat.ProductCatalogName IN (SELECT category FROM @Categories)) ';
	END

IF (@makeCount > 0)
	BEGIN
	SET @sqlString = @sqlString + 'AND manf.ManufacturerName IN (SELECT Make FROM @Makes) ';
	END

IF (@modelCount > 0)
	BEGIN
	SET @sqlString = @sqlString + 'AND inv.ModelNum IN (SELECT Model FROM @Models) ';
	END

IF (@RentalStatusCount > 0)
	BEGIN
	SET @sqlString = @sqlString + 'AND e.RentalStatus IN (SELECT CASE WHEN RentalStatus = ''On Rent'' THEN ''OnRent'' ELSE RentalStatus END FROM @RentalStatus) ';
	END

IF (@divisionCount > 0)
	BEGIN
	SET @sqlString = @sqlString + 'AND d.DivisionShortName IN (SELECT Division FROM @Divisions) ';
	END

IF (@AttachmentTypeCount > 0)
	BEGIN
	SET @sqlString = @sqlString + 'AND attachmentType.ProductCategoryName IN (SELECT AttachmentType FROM @AttachmentType) ';
	END

IF (@CityLookupCount > 0 OR @StateLookupCount > 0 OR @CountryLookupCount > 0)
	BEGIN
	SET @sqlString = @sqlString + 'AND (ad.City IN (SELECT Location FROM @CityLookup) OR ad.StateCode IN (SELECT Location FROM @StateLookup) OR ad.CountryName IN (SELECT Location FROM @CountryLookup)) ';
	END

IF (@locationStatusCount > 0)
	BEGIN
	SET @sqlString = @sqlString + 'AND e.LocationStatus IN (SELECT LocationStatus FROM @LocationStatus) ';
	END

IF (@MinYear <> 0 AND @MaxYear <> 0)
	BEGIN
	SET @sqlString = @sqlString + 'AND e.ManufacturedYear BETWEEN @MinYear AND @MaxYear ';
	END
ELSE IF (@MinYear = 0 AND @MaxYear <> 0)
	BEGIN
	SET @sqlString = @sqlString + 'AND e.ManufacturedYear < @MaxYear ';
	END
ELSE IF (@MinYear <> 0 AND @MaxYear = 0)
	BEGIN
	SET @sqlString = @sqlString + 'AND e.ManufacturedYear > @MinYear ';
	END

IF (@MinPrice <> 0 AND @MaxPrice <> 0)
	BEGIN
	SET @sqlString = @sqlString + 'AND e.price BETWEEN @MinPrice AND @MaxPrice ';
	END
ELSE IF (@MinPrice = 0 AND @MaxPrice <> 0)
	BEGIN
	SET @sqlString = @sqlString + 'AND e.price < @MaxPrice ';
	END
ELSE IF (@MinPrice <> 0 AND @MaxPrice = 0)
	BEGIN
	SET @sqlString = @sqlString + 'AND e.price > @MinPrice ';
	END


--FITS ON FOR ATTACHMENTS
IF @EquipmentOrAttachment = 'attachment'
	BEGIN

	IF(@FitsOnCategoryCount) > 0
		BEGIN 
		SET @sqlString = @sqlString + ' AND e.EquipmentID IN (SELECT AttachmentID FROM m2.AttachmentFitsOn WHERE ProductCategoryName IN (SELECT Value FROM @FitsOnCategories)) '
		END

	IF(@FitsOnMakeCount) > 0
		BEGIN 
		SET @sqlString = @sqlString + ' AND e.EquipmentID IN (SELECT AttachmentID FROM m2.AttachmentFitsOn WHERE ManufacturerName IN (SELECT Value FROM @FitsOnMakes)) '
		END

	IF(@FitsOnModelCount) > 0
		BEGIN 
		SET @sqlString = @sqlString + ' AND e.EquipmentID IN (SELECT AttachmentID FROM m2.AttachmentFitsOn WHERE ModelNum IN (SELECT Value FROM @FitsOnModels)) '
		END

	END

IF (@SelectOptionString IS NOT NULL) 
	BEGIN
		SET @SelectOptionString = @SelectOptionString + ') ';
	END

IF (@FilterOptionString IS NOT NULL) 
	BEGIN
		SET @FilterOptionString = @FilterOptionString + ') ';
	END

--PREVENT SQL INJECTION
IF @OrderDirection <> 'asc' AND @OrderDirection <> 'desc'
	BEGIN
		SET @OrderDirection = 'desc'
	END

--PREVENT SQL INJECTION
IF @OrderBy <> 'ModelNum' AND @OrderBy <> 'MonthlyRentalRate' AND @OrderBy <> 'Price' AND @OrderBy <> 'RentalStatus'
	BEGIN
		SET @OrderBy = 'ModelNum'
	END

SET @sqlString = @sqlString + ISNULL(@SelectOptionString, '') + ISNULL(@FilterOptionString, '');
SET @sqlString = @sqlString + ' ORDER BY ' + @OrderBy + ' ' + @OrderDirection

DECLARE @params NVARCHAR(MAX) = 
	   '@Categories m2.CategoryFilters READONLY,
		@Makes m2.MakeFilters READONLY,
		@Models m2.ModelFilters READONLY,
		@RentalStatus m2.RentalStatusFilters READONLY,
		@Divisions m2.DivisionFilters READONLY,
		@Results m2.SearchTermLookup READONLY,
		@FitsOnCategories m2.AttachmentFitsOn READONLY,
		@FitsOnMakes m2.AttachmentFitsOn READONLY,
		@FitsOnModels m2.AttachmentFitsOn READONLY,
		@AttachmentType m2.AttachmentType READONLY,
		@LocationStatus m2.LocationStatusFilters READONLY,
		@Locations m2.LocationFilters READONLY,
		@CityLookup m2.LocationFilters READONLY,
		@StateLookup m2.LocationFilters READONLY,
		@CountryLookup m2.LocationFilters READONLY,
		@MinPrice INT = 0,
		@MaxPrice INT = 0,
		@MinYear INT = 0,
		@MaxYear INT = 0'

--SELECT @sqlString
--SELECT * FROM @RentalStatus
--SELECT * FROM @Results

EXEC sp_executesql @sqlString, @params, @Categories, @Makes, @Models, @RentalStatus, @Divisions, @Results, @FitsOnCategories, @FitsOnMakes, @FitsOnModels, @AttachmentType, @LocationStatus, @Locations, @CityLookup, @StateLookup, @CountryLookup, @MinPrice, @MaxPrice, @MinYear, @MaxYear