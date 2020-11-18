USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[Opportunities_GetAggregatedModelsByDivision]    Script Date: 07/20/2016 15:31:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[Opportunities_GetAggregatedModelsByDivision]
@Division m2.DivisionFilters READONLY,
@Reason NVARCHAR(200),
@Duration INT
AS

WITH ModelTotals AS (
	SELECT inv.ModelNum, COUNT(1) as Total FROM Equipment e
	JOIN InventoryMaster inv ON inv.InventoryMasterID = e.InventoryMasterID
	WHERE e.RentalStatus NOT IN ('Sold', 'Returned')
	GROUP BY inv.ModelNum
)

SELECT @Duration as Duration, ISNULL(mt.Total, 0) as Total,
CASE WHEN oppItem.ModelNum IS NULL THEN 'No Model'
	 WHEN oppItem.ModelNum = '' THEN 'No Model'
	 ELSE oppItem.ModelNum
 END AS Aggregated, COUNT(1) as [Count] 
FROM m2.Opportunity opp
JOIN m2.OpportunityItem oppItem ON oppItem.OpportunityID = opp.OpportunityID
JOIN m2.OpportunityType oppType ON oppType.OpportunityTypeID = opp.OpportunityTypeID
JOIN m2.OpportunityLostReason oppReason ON oppReason.OpportunityLostReasonID = oppItem.OpportunityLostReasonID
LEFT JOIN ModelTotals mt ON mt.ModelNum = oppItem.ModelNum
LEFT JOIN Division div ON div.DivisionID = opp.DivisionID
WHERE div.DivisionShortName IN (SELECT Division FROM @Division) AND opp.EnterDateTime > DATEADD(dd, -@Duration, GETDATE()) AND oppReason.Reason = @Reason
GROUP BY oppItem.ModelNum, mt.Total


Go

USE [Mach1]
GO

/****** Object:  StoredProcedure [m2].[Opportunities_GetAggregatedReasonsByDivision]    Script Date: 07/20/2016 15:31:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[Opportunities_GetAggregatedReasonsByDivision]
@Division m2.DivisionFilters READONLY,
@Duration INT
AS

DECLARE @TotalReasonCount INT = (SELECT COUNT(1) total FROM m2.OpportunityItem)

SELECT @Duration as Duration, @TotalReasonCount as Total, oppReason.Reason as Aggregated, COUNT(1) as [Count] 
FROM m2.Opportunity opp
JOIN m2.OpportunityItem oppItem ON oppItem.OpportunityID = opp.OpportunityID
JOIN m2.OpportunityType oppType ON oppType.OpportunityTypeID = opp.OpportunityTypeID
JOIN m2.OpportunityLostReason oppReason ON oppReason.OpportunityLostReasonID = oppItem.OpportunityLostReasonID
LEFT JOIN Division div ON div.DivisionID = opp.DivisionID
WHERE div.DivisionShortName IN (SELECT Division FROM @Division) AND opp.EnterDateTime > DATEADD(dd, -@Duration, GETDATE())
GROUP BY oppReason.Reason
GO

