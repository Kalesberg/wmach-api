USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetAttachmentBySerialNum]    Script Date: 9/30/2016 9:27:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[GetAttachmentBySerialNum]
@SerialNum NVARCHAR(100)
AS


SELECT e.EquipmentID,
e.SerialNum,
e.ForRent,
e.ForSale,
e.PublicViewable,
e.PublicPriceViewable,
e.price,
e.MinPrice,
e.MonthlyRentalRate,
e.InsuranceValue,
e.PropertyTag,
e.RentalStatus,
e.OwnerType,
e.LocationStatus,
e.ServiceStatus,
e.ManufacturedYear yearManufactured,
e.[hours],
e.miles,
e.LastAppraisalDate,
e.DateAcquired,
e.PurchasePrice,
e.dateSold,
e.SoldPrice,
inv.ModelNum,
inv.Height,
inv.[Length],
inv.Width,
manf.ManufacturerName,
pc.ProductCategoryName Category,
DivisionShortName Division,
pc.ProductType [EquipmentType],
ad.City,
ad.StateCode [State],
ad.CountryName [Country],
ad.Address1,
ad.PostalCode,
es.PositionLatitude,
es.PositionLongitude,
es.EquipmentSummaryDate,
y.YardName,
e.RentalPurchaseOptionPrice,
e.MonthlyRentalRate * 1.05 InternationalRentalRate,
e.MarketingDescription,
pCat.ProductCatalogName attachmentCategory,
ISNULL(attachedToSerial.SerialNum, 'Not Attached') AttachedToSerialNum,
ISNULL(attachedToModel.ModelNum, 'Not Attached') AttachedToModelNum,
dbo.fnFitsOnModelList(e.equipmentID) AttachmentFitsOn,
CASE e.EquipmentSizeUnit
		WHEN 'Inches' THEN CAST(ISNULL(e.EquipmentSize, '') as VARCHAR) + ' "'
		WHEN 'Feet' THEN CAST(ISNULL(e.EquipmentSize, '') as VARCHAR) + ' '''
		WHEN 'N/A' THEN CAST(ISNULL(e.EquipmentSize, '') as VARCHAR) + ''
		ELSE CAST(e.EquipmentSize as VARCHAR) + ' ' + e.EquipmentSizeUnit
END AS Size

FROM Equipment e
JOIN InventoryMaster inv ON inv.InventoryMasterID = e.InventoryMasterID
JOIN Manufacturer manf ON manf.ManufacturerID = inv.ManufacturerId
JOIN ProductCategory pc ON pc.ProductCategoryID = inv.ProductCategoryID
JOIN Division d on d.DivisionID = e.AssignedDivisionID
JOIN [Address] ad ON ad.AddressID = e.CurrentLocationID 
LEFT JOIN Yard y on y.AddressID = e.CurrentLocationID
LEFT JOIN ProductCatalog pCat ON pCat.ProductCatalogID = pc.ProductCatalogID
LEFT JOIN Equipment attachedToSerial ON attachedToSerial.EquipmentID = e.MachineAttachedToID AND e.MachineAttachedToID <> 0
LEFT JOIN InventoryMaster attachedToModel ON attachedToModel.InventoryMasterID = attachedToSerial.InventoryMasterID
OUTER APPLY (SELECT Top 1 [QualCommID],[PositionLatitude],[PositionLongitude], [EquipmentSummaryDate] FROM [Mach1].[dbo].[EquipmentSummary] where QualCommID=e.SerialNum order by EquipmentSummaryDate desc) es
OUTER APPLY (SELECT Top 1 [fileName] FROM InventoryPicture WHERE EquipmentID = e.EquipmentID) picture
WHERE e.SerialNum = @SerialNum
