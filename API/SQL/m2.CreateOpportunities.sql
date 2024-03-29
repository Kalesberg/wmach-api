USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[CreateOpportunity]    Script Date: 6/27/2016 1:24:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [m2].[CreateOpportunity]
@OpportunityType NVARCHAR(100),
@Equipment m2.OpportunityItem READONLY,
@Customer NVARCHAR(200),
@JobLocation NVARCHAR(200),
@Remarks NVARCHAR(MAX)
AS

DECLARE @Quantity INT
DECLARE @Category NVARCHAR(200)
DECLARE @Manufacturer NVARCHAR(200)
DECLARE @Model NVARCHAR(100)
DECLARE @Reason NVARCHAR(100)

DECLARE @CategoryID INT
DECLARE @ManufacturerID INT
DECLARE @InventoryMasterID INT
DECLARE @ReasonID INT
DECLARE @OpportunityTypeID INT = (SELECT CASE @OpportunityType WHEN 'Rental' THEN 1 WHEN 'Sale' THEN 2 END)

--POPULATE MAIN OPP TABLE
INSERT INTO m2.Opportunity VALUES (@OpportunityTypeID, @Customer,@JobLocation, @Remarks)

DECLARE @OpportunityID INT = SCOPE_IDENTITY()

--ENUMERATE OPP ITEMS AND INSERT
DECLARE Enumerator CURSOR FAST_FORWARD FOR
SELECT Quantity, Category, Manufacturer, Model, Reason FROM @Equipment

OPEN Enumerator
FETCH NEXT FROM Enumerator
INTO @Quantity, @Category, @Manufacturer, @Model, @Reason

	WHILE @@FETCH_STATUS = 0
		BEGIN

		SET @ReasonID = (SELECT OpportunityLostReasonID FROM m2.OpportunityLostReason WHERE Reason = @Reason AND OpportunityTypeID = @OpportunityTypeID)

		INSERT INTO m2.OpportunityItem VALUES (@OpportunityID, @Quantity, @Category, @Manufacturer, @Model, @ReasonID)

		FETCH NEXT FROM Enumerator
		INTO @Quantity, @Category, @Manufacturer, @Model, @Reason

		END

	CLOSE Enumerator
	DEALLOCATE Enumerator 