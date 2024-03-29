USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetActiveQuotes]    Script Date: 10/25/2016 7:20:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [m2].[GetActiveQuotes]
AS

SELECT q.quoteID, q.QuoteType, c.CompanyName, q.QuoteNumber, div.divisionshortname division, q.JobSite, q.EstimatedStartDate, qd.Description, qd.Quantity, qd.ManufacturerName make, qd.Model, qd.MonthlyRate, qd.WeeklyRate, qd.OvertimeHourlyRate, qd.DailyRate, q.EnterDateTime
FROM quote q
JOIN quoteDetail qd ON qd.QuoteID = q.QuoteID
JOIN ContactRelationship cr ON cr.ContactRelationshipID = q.ContactRelationshipID
JOIN Contact c on c.ContactID = cr.ParentContactID
JOIN Division div ON div.DivisionID = q.DivisionID
WHERE q.EnterDateTime > DATEADD(dd, -90, GETDATE())


