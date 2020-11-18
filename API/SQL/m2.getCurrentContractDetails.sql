USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[getCurrentContractDetails]    Script Date: 9/30/2016 8:42:55 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[getCurrentContractDetails]
@EquipmentID INT,
@EquipmentType VARCHAR(30) = 'Machine'
AS

DECLARE @AttachmentID INT;
IF(@EquipmentType = 'Attachment')
	BEGIN
		SET @AttachmentID = @EquipmentID; --Copy EquipmentID into AttachmentID. If The Attachment is not on a piece of machinery, then we will use the original ID passed in.
		SET @EquipmentID = (SELECT MachineAttachedToId FROM Equipment WHERE EquipmentID = @EquipmentID);

		IF(@EquipmentID = 0) --Attachment is not on machine, do contract lookup on attachment ID instead
			BEGIN
				SET @EquipmentID = @AttachmentID;
			END
	END

SELECT
accountManager.FirstName AccountManagerFirstName,
accountManager.LastName AccountManagerLastName,
rentCoord.FirstName RentalCoordinatorFirstName,
rentCoord.LastName RentalCoordinatorLastName,
owningDivision.DivisionShortName ContractOwner,
con.ReferenceNumberCustomer CustomerReferenceNumber,
business.CompanyName RentingCompany,
con.JobSiteContact CustomerName,
con.JobSiteEmail customerEmail,
con.JobSitePhone customerBusinessPhone,
CASE WHEN con.JobSiteMobile = '' THEN con.JobSitePhone
							     ELSE con.JobSiteMobile
							     END AS customerMobilePhone,
customerAddress.StateCode CustomerStateCode,
customerAddress.City CustomerCity,
customerAddress.Address1 CustomerStreet,
details.StartHours,
details.EstimatedStartDate ContractEstimatedStartDate,
details.EstimatedEndDate ContractEstimatedEndDate,
details.ActualStartDate ContractActualStartDate,
CAST(details.RentalPeriod AS VARCHAR(10)) + ' ' +  details.RentalPeriodTimeSpan as ContractMinRentalTerm,
details.RentalRate ContractRentalRate,
details.RentalPurchaseOptionPrice ContractRPOPrice

FROM contract con
LEFT JOIN ContractDtl details ON details.ContractID = con.ContractID
LEFT JOIN ContactRelationship cr ON cr.ContactRelationshipID = con.CustomerContactID
LEFT JOIN contact customer ON customer.ContactID = cr.ChildContactID
LEFT JOIN contact business ON business.ContactID = cr.ParentContactID
LEFT JOIN contact rentCoord ON rentCoord.ContactID = con.RentalCoordinatorID
LEFT JOIN contact accountManager ON accountManager.ContactID = business.AccountManagerID
LEFT JOIN division owningDivision ON owningDivision.DivisionID = con.WWMDivisionID
LEFT JOIN Address customerAddress ON customerAddress.AddressID = con.JobSiteAddressID
WHERE details.EquipmentID = @EquipmentID AND con.ContractStatus = 'Open'
