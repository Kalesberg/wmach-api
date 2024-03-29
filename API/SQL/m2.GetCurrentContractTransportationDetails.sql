USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[getCurrentContractTransportationDetails]    Script Date: 9/30/2016 8:51:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[getCurrentContractTransportationDetails]
@EquipmentID INT,
@EquipmentType VARCHAR(30) = 'Machine'
AS

DECLARE @AttachmentID INT;
IF(@EquipmentType = 'Attachment')
	BEGIN
		SET @AttachmentID = @EquipmentID; --Copy EquipmentID into AttachmentID. If The Attachment is not on a piece of machinery, then we will use the original id passed in
		SET @EquipmentID = (SELECT MachineAttachedToId FROM Equipment WHERE EquipmentID = @EquipmentID);

		IF(@EquipmentID = 0) --Attachment is not on machine, do contract lookup on attachment ID instead
			BEGIN
				SET @EquipmentID = @AttachmentID;
			END
	END

SELECT Pickup.City + ', ' + Pickup.StateCode as FromLocation,
	   DropOff.City + ', ' + DropOff.StateCode as ToLocation,
	   carrier.CompanyName ResponsibleParty
FROM ShipmentInventory shipInv
JOIN Shipment ship ON ship.ShipmentID = shipInv.ShipmentID
JOIN address Pickup ON Pickup.AddressID = ship.PickupLocationAddressID
JOIN address DropOff ON DropOff.AddressID = ship.DropOffLocationAddressID
JOIN ContractDtl conDtl ON conDtl.ContractDtlID = shipInv.ContractDtlID
JOIN Contract con ON con.ContractID = conDtl.ContractID
JOIN ContactRelationship conRel ON conRel.ContactRelationshipID = ship.CarrierContactRelationshipID
JOIN contact carrier ON carrier.ContactID = conrel.ParentContactID
WHERE con.ContractStatus = 'Open' AND conDtl.EquipmentID = @EquipmentID
