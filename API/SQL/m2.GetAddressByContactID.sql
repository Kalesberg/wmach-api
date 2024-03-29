USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetAddressByContactID]    Script Date: 7/19/2016 11:09:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [m2].[GetAddressByContactID]
@ContactID INT
AS

--DECLARE @Email NVARCHAR(255)
--SET @Email = 'mrobinson@wrsrents.com'


SELECT adr.Address1 as 'street', 
adr.city, 
adr.StateCode as 'state', 
adr.PostalCode as 'zip'
FROM Contact c
INNER JOIN ContactAddressRelationship car ON car.ContactID = c.ContactID
INNER JOIN [Address] adr ON adr.AddressID = car.AddressID
WHERE c.ContactID = @ContactID;