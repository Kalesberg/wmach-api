
CREATE TABLE m2.RateEquipmentType (
ID INT IDENTITY(1,1) NOT NULL,
EquipmentType VARCHAR(100) NOT NULL
)

GO

INSERT INTO m2.RateEquipmentType
SELECT DISTINCT Category FROM m2.EquipmentRatesTest

GO

CREATE TABLE m2.RateEquipmentCategory (
ID INT IDENTITY(1,1) NOT NULL,
EquipmentTypeID INT NOT NULL,
EquipmentCategory VARCHAR(100) NOT NULL
)

GO

INSERT INTO m2.RateEquipmentCategory
SELECT er.ID, s.equipment FROM
(SELECT DISTINCT category, equipment FROM m2.EquipmentRatesTest)s
JOIN m2.RateEquipmentType er ON er.EquipmentType = s.category
ORDER BY s.equipment

GO

CREATE TABLE m2.RateEquipmentSubCategory (
ID INT IDENTITY(1,1) NOT NULL,
EquipmentCategoryID INT NOT NULL,
EquipmentSubCategory VARCHAR(250) NOT NULL
)

GO

INSERT INTO m2.RateEquipmentSubCategory
SELECT rc.ID, r.model FROM
(SELECT category, Equipment, model, monthly, weekly
FROM m2.EquipmentRatesTest WHERE monthly = '' and weekly = '')r
JOIN m2.RateEquipmentCategory rc ON rc.EquipmentCategory = r.Equipment
 select * from m2.RateEquipmentSubCategory

 GO

CREATE TABLE m2.RateEquipmentModel (
ID INT IDENTITY(1,1) NOT NULL,
Equipment VARCHAR(250) NOT NULL,
EquipmentCategoryID INT NOT NULL,
EquipmentSubCategoryID INT NULL
)

GO

EXEC [m2].[PopulateRateEquipmentModel]

GO

CREATE TABLE m2.RateEquipmentWeekly (
ID INT IDENTITY(1,1) NOT NULL,
EquipmentModelID INT NOT NULL,
WeeklyRate DECIMAL NULL,
WeeklyRateLowerRange DECIMAL NULL,
WeeklyRateUpperRange DECIMAL NULL,
IsPOR BIT NOT NULL,
IsRange BIT NOT NULL
)

GO

CREATE TABLE m2.RateEquipmentMonthly (
ID INT IDENTITY(1,1) NOT NULL,
EquipmentModelID INT NOT NULL,
MonthlyRate DECIMAL NULL,
MontlyRateLowerRange DECIMAL NULL,
MontlyRateUpperRange DECIMAL NULL,
IsPOR BIT NOT NULL,
IsRange BIT NOT NULL
)

GO

INSERT INTO m2.RateEquipmentWeekly
SELECT q.ID, 
CASE WHEN CHARINDEX('-', weekly) <> 0 OR CHARINDEX('/', weekly) <> 0 OR weekly = 'CALL' THEN NULL ELSE weekly END AS weekly,
REPLACE(q.RateLower, ',', '') 'RateLower', 
REPLACE(q.RateUpper, ',', '') 'RateUpper', q.isPOR, q.isRange
FROM 
(SELECT ID, 
CASE WHEN weekly = 'P.O.R.' OR weekly = '-' or weekly = 'N/A' THEN NULL ELSE weekly END as 'weekly',
RateLower,
CASE WHEN RateUpper = weekly THEN NULL ELSE RateUpper END as RateUpper,
 CASE WHEN weekly = 'P.O.R.' THEN 1 ELSE 0 End as 'isPOR', 
 CASE WHEN RateLower IS NOT NULL THEN 1 ELSE 0 END AS 'isRange'
FROM
(SELECT r.Category, r.Equipment, r.Model, r.weekly,
CASE WHEN CHARINDEX('/', weekly) <> 0 AND weekly <>'N/A'  THEN
	SUBSTRING(weekly, 2, CHARINDEX('/', weekly) - 2)
	ELSE NULL
	END AS RateLower,
CASE WHEN CHARINDEX('/', weekly) <> 0 AND weekly <> 'N/A' THEN
	 SUBSTRING(weekly, CHARINDEX('/', weekly) + 2, LEN(weekly) - CHARINDEX('/', weekly) - 1) 
	 ELSE NULL 
	 END AS RateUpper
FROM m2.EquipmentRatesTest r)l
JOIN (SELECT rm.ID, rc.EquipmentCategory, rm.Equipment, rm.EquipmentCategoryID FROM m2.RateEquipmentModel rm
	  JOIN m2.RateEquipmentCategory rc ON rc.ID = rm.EquipmentCategoryID)rat ON rat.Equipment = l.model AND rat.EquipmentCategory = l.equipment

UNION

SELECT ID, 
CASE WHEN weekly = 'P.O.R.' OR weekly = '-' or weekly = 'N/A' THEN NULL ELSE weekly END as 'weekly',
RateLower,
CASE WHEN RateUpper = weekly THEN NULL ELSE RateUpper END as RateUpper,
 CASE WHEN weekly = 'P.O.R.' THEN 1 ELSE 0 End as 'isPOR', 
 CASE WHEN RateLower IS NOT NULL THEN 1 ELSE 0 END AS 'isRange' 
FROM
(SELECT r.Category, r.Equipment, r.Model, r.weekly,
CASE WHEN CHARINDEX('-', weekly) <> 0 AND CHARINDEX('-', weekly) <> 1 THEN
	SUBSTRING(weekly, 2, CHARINDEX('-', weekly) - 2)
	ELSE NULL
	END AS RateLower,
CASE WHEN CHARINDEX('-', weekly) <> 0 THEN
	 SUBSTRING(weekly, CHARINDEX('-', weekly) + 3, LEN(weekly) - CHARINDEX('-', weekly) - 2) 
	 ELSE NULL 
	 END AS RateUpper
FROM m2.EquipmentRatesTest r)l
JOIN (SELECT rm.ID, rc.EquipmentCategory, rm.Equipment, rm.EquipmentCategoryID FROM m2.RateEquipmentModel rm
	  JOIN m2.RateEquipmentCategory rc ON rc.ID = rm.EquipmentCategoryID)rat ON rat.Equipment = l.model AND rat.EquipmentCategory = l.equipment
	  WHERE CHARINDEX('-', weekly) <> 0 AND CHARINDEX('-', weekly) <> 1)q



INSERT INTO m2.RateEquipmentMonthly
SELECT q.ID, 
CASE WHEN CHARINDEX('-', monthly) <> 0 OR CHARINDEX('/', monthly) <> 0 OR monthly = 'CALL' THEN NULL ELSE monthly END AS monthly,
REPLACE(q.RateLower, ',', '') AS 'RateLower', 
REPLACE(q.RateUpper, ',', '') AS 'RateUpper', q.isPOR, q.isRange INTO #RateTemp
FROM 
(SELECT ID, 
CASE WHEN monthly = 'P.O.R.' OR monthly = '-' THEN NULL ELSE monthly END as 'monthly',
RateLower,
CASE WHEN RateUpper = monthly THEN NULL ELSE RateUpper END as RateUpper,
 CASE WHEN monthly = 'P.O.R.' THEN 1 ELSE 0 End as 'isPOR', 
 CASE WHEN RateLower IS NOT NULL THEN 1 ELSE 0 END AS 'isRange'
FROM
(SELECT r.Category, r.Equipment, r.Model, r.monthly,
CASE WHEN CHARINDEX('/', monthly) <> 0 THEN
	SUBSTRING(monthly, 2, CHARINDEX('/', monthly) - 2)
	ELSE NULL
	END AS RateLower,
CASE WHEN CHARINDEX('/', monthly) <> 0 THEN
	 SUBSTRING(monthly, CHARINDEX('/', monthly) + 2, LEN(monthly) - CHARINDEX('/', monthly) - 1) 
	 ELSE NULL 
	 END AS RateUpper
FROM m2.EquipmentRatesTest r)l
JOIN (SELECT rm.ID, rc.EquipmentCategory, rm.Equipment, rm.EquipmentCategoryID FROM m2.RateEquipmentModel rm
	  JOIN m2.RateEquipmentCategory rc ON rc.ID = rm.EquipmentCategoryID)rat ON rat.Equipment = l.model AND rat.EquipmentCategory = l.equipment
	  WHERE CHARINDEX('-', monthly) = 0 OR CHARINDEX('-', monthly) = 1

UNION 

SELECT ID, 
CASE WHEN monthly = 'P.O.R.' OR monthly = '-' THEN NULL ELSE monthly END as 'monthly',
RateLower,
CASE WHEN RateUpper = monthly THEN NULL ELSE RateUpper END as RateUpper,
 CASE WHEN monthly = 'P.O.R.' THEN 1 ELSE 0 End as 'isPOR', 
 CASE WHEN RateLower IS NOT NULL THEN 1 ELSE 0 END AS 'isRange' 
FROM
(SELECT r.Category, r.Equipment, r.Model, r.monthly,
CASE WHEN CHARINDEX('-', monthly) <> 0 AND CHARINDEX('-', monthly) <> 1 THEN
	SUBSTRING(monthly, 2, CHARINDEX('-', monthly) - 2)
	ELSE NULL
	END AS RateLower,
CASE WHEN CHARINDEX('-', monthly) <> 0 THEN
	 SUBSTRING(monthly, CHARINDEX('-', monthly) + 3, LEN(monthly) - CHARINDEX('-', monthly) - 2) 
	 ELSE NULL 
	 END AS RateUpper
FROM m2.EquipmentRatesTest r)l
JOIN (SELECT rm.ID, rc.EquipmentCategory, rm.Equipment, rm.EquipmentCategoryID FROM m2.RateEquipmentModel rm
	  JOIN m2.RateEquipmentCategory rc ON rc.ID = rm.EquipmentCategoryID)rat ON rat.Equipment = l.model AND rat.EquipmentCategory = l.equipment
	  WHERE CHARINDEX('-', monthly) <> 0 AND CHARINDEX('-', monthly) <> 1)q
