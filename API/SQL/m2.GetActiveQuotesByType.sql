USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetActiveQuotes]    Script Date: 10/26/2016 1:04:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[GetActiveQuotesByType]
@QuoteType NVARCHAR(20) = NULL
AS

SELECT q.quoteID, q.QuoteType, c.CompanyName, q.QuoteNumber, div.divisionshortname division, q.JobSite, q.EstimatedStartDate, qd.Description, qd.Quantity, qd.ManufacturerName make, qd.Model, qd.MonthlyRate, qd.WeeklyRate, qd.OvertimeHourlyRate, qd.DailyRate, q.EnterDateTime
FROM quote q
JOIN quoteDetail qd ON qd.QuoteID = q.QuoteID
JOIN ContactRelationship cr ON cr.ContactRelationshipID = q.ContactRelationshipID
JOIN Contact c on c.ContactID = cr.ParentContactID
JOIN Division div ON div.DivisionID = q.DivisionID
WHERE q.EnterDateTime > DATEADD(dd, -90, GETDATE()) AND q.QuoteType = COALESCE(@QuoteType, q.QuoteType)
