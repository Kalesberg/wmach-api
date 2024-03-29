SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [m2].[GetEquipmentAudit] 
	@EquipmentID INT
AS
BEGIN
	SET NOCOUNT ON;

	WITH CTE AS (
		SELECT
		rownum = ROW_NUMBER() OVER (ORDER BY ea.OwnedByID),
		ea.DateSold, ea.SaleDateAvailable,
		(coalesce(ea.PurchasePrice, 0) + coalesce(ea.AdditionalCapCost, 0) + coalesce(ea.AcqAttachmentPrice, 0) + coalesce(ea.AcqShippingCost, 0)) AS TotalAcquisitionCost,
			eq.ConsolidatedNBV, ea.SoldPrice,
			CASE ct1.ContactType WHEN 'Person' THEN CONCAT(ct1.LastName,', ',ct1.FirstName)
			ELSE ct1.CompanyName
			END
			AS 'OwnedByName',
			ea.[Hours], ea.SerialNum, ea.ManufacturedYear,
		ea.EquipmentID
			 ,ea.OwnedByID
			  ,MIN(ea.EditDateTime) as SetAsOwnerOn

		FROM [Mach1].[dbo].[EquipmentAudit] ea
		INNER JOIN Equipment eq ON eq.EquipmentID = @EquipmentID
		INNER JOIN Contact ct1 ON ct1.ContactID = ea.OwnedByID

		Where ea.EquipmentID = @EquipmentID

		Group By ea.OwnedByID, ea.EquipmentID, ea.DateSold, ea.SaleDateAvailable, eq.ConsolidatedNBV, ea.SoldPrice, ea.[Hours], ea.SerialNum, ea.ManufacturedYear,
		ea.PurchasePrice, ea.AdditionalCapCost, ea.AcqAttachmentPrice, ea.AcqShippingCost, ct1.ContactType, ct1.LastName, ct1.FirstName, ct1.CompanyName

		)
		SELECT
		CTE.DateSold, CTE.SaleDateAvailable, CTE.TotalAcquisitionCost,
		CTE.ConsolidatedNBV, CTE.SoldPrice, CTE.OwnedByName, prev.OwnedByName PreviousOwnedByName,
		CTE.[Hours], CTE.SerialNum, CTE.ManufacturedYear
		FROM CTE
		LEFT JOIN CTE prev ON prev.rownum = CTE.rownum - 1

		WHERE CTE.OwnedByName <> prev.OwnedByName

		Order By SetAsOwnerOn Asc
		

END
GO
