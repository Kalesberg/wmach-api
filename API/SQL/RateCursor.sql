USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[PopulateRateEquipmentModel]    Script Date: 1/7/2016 8:49:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [m2].[PopulateRateEquipmentModel]
AS

WITH HeaderRankings AS (
	SELECT * FROM
	(SELECT ID, category, model, monthly, weekly
	FROM m2.EquipmentRatesTest)s WHERE monthly = '' and weekly = ''
),

EquipmentRatesRanked AS (
	SELECT ID, category, equipment, model, monthly, weekly
	FROM m2.EquipmentRatesTest
),

Transform AS (
SELECT  ID,
		category,
		equipment,
		CASE WHEN Header IS NOT NULL THEN NULL ELSE model END AS Model,
		monthly,
		weekly,
		Header
FROM (
SELECT ER.ID, ER.category, ER.equipment, ER.model, ER.monthly, ER.weekly, HR.model as 'Header' FROM
EquipmentRatesRanked ER
LEFT JOIN HeaderRankings HR ON HR.ID = ER.ID)r
)

SELECT ID, category as 'type', equipment as 'category', model as 'equipment', monthly, weekly, header as 'subCategory' 
INTO #RateTemp 
FROM Transform
ORDER BY ID

DECLARE @CurrentSub VARCHAR(200) = NULL
DECLARE @CurrentCategory VARCHAR(200) = ''
DECLARE @EquipmentType VARCHAR(100)
DECLARE @EquipmentCategory VARCHAR(200)
DECLARE @Equipment VARCHAR(250)
DECLARE @Monthly VARCHAR(100)
DECLARE @Weekly VARCHAR(100)
DECLARE @SubCategory VARCHAR(100)
DECLARE @CategoryChanged BIT

DECLARE @Rates TABLE (
EquipmentType VARCHAR(100),
EquipmentCategory VARCHAR(200),
Equipment VARCHAR(250),
Monthly VARCHAR(100),
Weekly VARCHAR(100),
SubCategory VARCHAR(100) NULL

)

DECLARE RateCursor CURSOR FAST_FORWARD FOR
SELECT type, category, equipment, monthly, weekly, subCategory  FROM #RateTemp

OPEN RateCursor
FETCH NEXT FROM RateCursor
INTO @EquipmentType, @EquipmentCategory, @Equipment, @Monthly, @Weekly, @SubCategory

WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @CategoryChanged = 0;

		IF(@CurrentCategory <> @EquipmentCategory)
		BEGIN 
			SET @CategoryChanged = 1;
			SET @CurrentCategory = @EquipmentCategory;
		END

		IF (@CategoryChanged = 1 AND @SubCategory IS NULL)
		BEGIN 
			SET @CurrentSub = NULL;
		END
		ELSE IF (@SubCategory IS NOT NULL)
		BEGIN 
			SET @CurrentSub = @SubCategory;
		END

		INSERT INTO @Rates VALUES (	@EquipmentType, @EquipmentCategory, @Equipment, @Monthly, @Weekly, @CurrentSub )

	FETCH NEXT FROM RateCursor
	INTO @EquipmentType, @EquipmentCategory, @Equipment, @Monthly, @Weekly, @SubCategory

	END

CLOSE RateCursor
DEALLOCATE RateCursor

DROP TABLE #RateTemp

INSERT INTO m2.RateEquipmentModel
SELECT r.Equipment, c.ID, sc.ID FROM 
@Rates r
JOIN m2.RateEquipmentCategory c ON c.EquipmentCategory = r.EquipmentCategory
LEFT JOIN m2.RateEquipmentSubCategory sc ON sc.EquipmentSubCategory = r.SubCategory
WHERE Equipment IS NOT NULL