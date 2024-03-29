USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetEquipmentBySerialNum]    Script Date: 9/30/2016 9:28:10 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [m2].[GetMachineBySerialNum]
@SerialNum NVARCHAR(100)
AS

WITH CurrentRentalRate AS (
	SELECT e.EquipmentID, RentalRate FROM
	Contract con
	JOIN ContractDtl conDtl ON conDtl.ContractID = con.ContractID
	JOIN Equipment e ON e.EquipmentID = conDtl.EquipmentID
	WHERE e.SerialNum = @SerialNum AND con.ContractStatus = 'Open'
)

SELECT e.EquipmentID,
SerialNum,
ForRent,
ForSale,
PublicViewable,
PublicPriceViewable,
price,
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
miles,
LastAppraisalDate,
DateAcquired,
PurchasePrice,
dateSold,
SoldPrice,
inv.ModelNum,
inv.Height,
inv.[Length],
inv.Width,
manf.ManufacturerName,
pc.ProductCategoryName Category,
DivisionShortName Division,
pc.ProductType [EquipmentType],
ad.City,
ad.StateCode,
ad.CountryName,
ad.Address1,
ad.PostalCode,
es.PositionLatitude,
es.PositionLongitude,
es.EquipmentSummaryDate,
y.YardName,
e.RentalPurchaseOptionPrice,
MonthlyRentalRate * 1.05 InternationalRentalRate,
actualRate.RentalRate ActualRentalRate,
e.MarketingDescription,
pCat.ProductCatalogName attachmentCategory


FROM Equipment e
JOIN InventoryMaster inv ON inv.InventoryMasterID = e.InventoryMasterID
JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
JOIN Division d on d.DivisionID = e.AssignedDivisionID
JOIN [Address] ad ON ad.AddressID = e.CurrentLocationID 
LEFT JOIN Yard y on y.AddressID = e.CurrentLocationID
LEFT JOIN CurrentRentalRate actualRate ON actualRate.EquipmentID = e.EquipmentID
LEFT JOIN ProductCatalog pCat ON pCat.ProductCatalogID = pc.ProductCatalogID
OUTER APPLY (SELECT Top 1 [QualCommID],[PositionLatitude],[PositionLongitude], [EquipmentSummaryDate] FROM [Mach1].[dbo].[EquipmentSummary] where QualCommID=e.SerialNum order by EquipmentSummaryDate desc) es
OUTER APPLY (SELECT Top 1 [fileName] FROM InventoryPicture WHERE EquipmentID = e.EquipmentID) picture
WHERE e.SerialNum = @SerialNum
