CREATE TABLE m2.Opportunity (
OpportunityID INT PRIMARY KEY IDENTITY,
OpportunityTypeID INT NOT NULL,
Customer VARCHAR(250),
JobLocation VARCHAR(250),
Remarks VARCHAR(MAX)
)

CREATE TABLE m2.OpportunityType (
OpportunityTypeID INT PRIMARY KEY IDENTITY,
OpportunityType VARCHAR(200)
) 

DROP TABLE m2.OpportunityItem
CREATE TABLE m2.OpportunityItem (
OpportunityItemID INT PRIMARY KEY IDENTITY,
OpportunityID INT NOT NULL,
Quantity INT NOT NULL,
Category NVARCHAR(100) NULL,
Manufacturer NVARCHAR(100) NULL,
ModelNum NVARCHAR(100) NULL,
OpportunityLostReasonID INT NOT NULL 
)

CREATE TABLE m2.OpportunityLostReason (
OpportunityLostReasonID INT PRIMARY KEY IDENTITY,
OpportunityTypeID INT NOT NULL,
Reason VARCHAR(250) NOT NULL
)

INSERT INTO m2.OpportunityType VALUES ('Rental'), ('Sale')


INSERT INTO m2.OpportunityLostReason VALUES 
(1, 'None Available'), (1, 'Not Spec''d Properly'), (1, 'Rate Too High'), (1, 'Too Far Away'), (1, 'Equipment Too Old'), (1, 'Other'),
(2, 'Price Too High'), (2, 'Financial Terms'), (2, 'Machine Too Old'), (2, 'Machine Too New'), (2, 'Competitor'), (2, 'Other')

USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[CreateOpportunity]    Script Date: 7/1/2016 8:00:37 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[CreateOpportunity]
@OpportunityType NVARCHAR(100),
@Equipment m2.OpportunityItem READONLY,
@Customer NVARCHAR(200) = null,
@JobLocation NVARCHAR(200) = null,
@Remarks NVARCHAR(MAX) = null
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
INSERT INTO m2.Opportunity VALUES (@OpportunityTypeID, @Customer, @JobLocation, @Remarks)

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
		IF (@ReasonID IS NULL)
			BEGIN
				IF (@OpportunityType = 'Sales')
					BEGIN
						SET @ReasonID = 6;
					END
				ELSE
					BEGIN
						SET @ReasonID = 12;
					END
			END

		INSERT INTO m2.OpportunityItem VALUES (@OpportunityID, @Quantity, @Category, @Manufacturer, @Model, @ReasonID)

		FETCH NEXT FROM Enumerator
		INTO @Quantity, @Category, @Manufacturer, @Model, @Reason

		END

	CLOSE Enumerator
	DEALLOCATE Enumerator 

ALTER PROCEDURE m2.GetOpportunity
AS

SELECT oppType.OpportunityType, opp.Remarks, opp.OpportunityID, oppItem.Quantity, oppItem.Category, oppItem.Manufacturer, oppItem.ModelNum, oppReason.Reason, opp.JobLocation
FROM m2.Opportunity opp
JOIN m2.OpportunityType oppType ON oppType.OpportunityTypeID = opp.OpportunityTypeID
LEFT JOIN m2.OpportunityItem oppItem ON oppItem.OpportunityID = opp.OpportunityID
JOIN m2.OpportunityLostReason oppReason ON oppReason.OpportunityLostReasonID = oppItem.OpportunityLostReasonID


CREATE PROCEDURE m2.GetOpportunityLostReasons
AS

SELECT CASE OpportunityTypeID
	   WHEN 1 THEN 'Rental'
	   WHEN 2 THEN 'Sale'
	   END AS 'oppportunityType',
	   Reason
FROM m2.OpportunityLostReason
