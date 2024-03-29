USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetQuotes]    Script Date: 4/21/2016 9:37:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[GetQuotes] 
@QuoteID INT = 0
AS

SELECT c.CompanyName, q.QuoteNumber, q.EstimatedStartDate, qd.Description, qd.MonthlyRate, qd.WeeklyRate, qd.OvertimeHourlyRate, qd.DailyRate, q.EnterDateTime
FROM quote q
JOIN quoteDetail qd ON qd.QuoteID = q.QuoteID
JOIN ContactRelationship cr ON cr.ContactRelationshipID = q.ContactRelationshipID
JOIN Contact c on c.ContactID = cr.ParentContactID
WHERE q.QuoteNumber = @QuoteID