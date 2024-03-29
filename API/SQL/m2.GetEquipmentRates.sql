USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetEquipmentRatesTest]    Script Date: 1/13/2016 10:18:58 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[GetEquipmentRates]
AS

SELECT equipmentType.EquipmentType,
category.EquipmentCategory, 
model.Equipment, 
CASE WHEN monthly.IsRange = 1 THEN '$' + CONVERT(VARCHAR(100), monthly.MontlyRateLowerRange) + '/' + '$' + CONVERT(VARCHAR(100), monthly.MontlyRateUpperRange)
	 WHEN monthly.IsPOR = 1 THEN 'P.O.R.'
	 WHEN monthly.IsPOR = 0 AND monthly.MonthlyRate IS NULL THEN 'CALL'
	 ELSE '$' + CONVERT(VARCHAR(100), monthly.MonthlyRate)
END AS 'MonthlyRate',

CASE WHEN weekly.IsRange = 1 THEN '$' + CONVERT(VARCHAR(100), weekly.WeeklyRateLowerRange) + '/' + '$' + CONVERT(VARCHAR(100), weekly.WeeklyRateUpperRange)
	 WHEN weekly.IsPOR = 1 THEN 'P.O.R.'
	 WHEN weekly.IsPOR = 0 AND weekly.WeeklyRate IS NULL THEN 'CALL'
	 ELSE '$' + CONVERT(VARCHAR(100), weekly.WeeklyRate)
END AS 'WeeklyRate',
subCategory.EquipmentSubCategory
FROM
m2.RateEquipmentModel model
JOIN m2.RateEquipmentCategory category ON category.ID = model.EquipmentCategoryID
LEFT JOIN m2.RateEquipmentSubCategory subCategory ON subCategory.ID = model.EquipmentSubCategoryID
JOIN m2.RateEquipmentType equipmentType ON equipmentType.ID = category.EquipmentTypeID
LEFT JOIN m2.RateEquipmentMonthly monthly ON monthly.EquipmentModelID = model.ID
LEFT JOIN m2.RateEquipmentWeekly weekly ON weekly.EquipmentModelID = model.ID


