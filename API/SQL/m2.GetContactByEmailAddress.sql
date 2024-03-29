USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetContactByEmailAddress]    Script Date: 7/19/2016 11:08:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [m2].[GetContactByEmailAddress]
@Email NVARCHAR(100)
AS

--DECLARE @Email NVARCHAR(255)
--SET @Email = 'mrobinson@wrsrents.com'

DECLARE @Temp TABLE (contactID INT, BusinessPhone NVARCHAR(MAX))


INSERT INTO @Temp SELECT c.contactID,
c.BusinessPhone
FROM Contact c
INNER JOIN ContactAddressRelationship car ON car.ContactID = c.ContactID
INNER JOIN [Address] adr ON adr.AddressID = car.AddressID
WHERE c.Email = @Email;

IF NOT EXISTS (SELECT 1 FROM @Temp)
BEGIN
	INSERT INTO @Temp SELECT TOP 1 c.contactID, c.BusinessPhone
	FROM Contact c
	INNER JOIN ContactRelationship cr ON c.ContactID = cr.ChildContactID
	INNER JOIN Contact c2 ON cr.ParentContactID = c2.ContactID
	INNER JOIN ContactAddressRelationship car ON car.ContactID = c2.ContactID
	INNER JOIN [Address] adr on adr.AddressID = car.AddressID
	WHERE c.Email = @Email AND adr.Address1 NOT LIKE 'Billing' AND c2.Active = 1;
END

SELECT * from @Temp