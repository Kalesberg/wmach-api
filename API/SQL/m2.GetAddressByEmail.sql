CREATE PROCEDURE m2.GetAddressByEmailAddress
@Email NVARCHAR(100)
AS

SELECT c.contactID, 
adr.Address1 as 'street', 
adr.city, 
adr.StateCode as 'state', 
adr.PostalCode as 'zip',
c.BusinessPhone
FROM Contact c
JOIN ContactAddressRelationship car ON car.ContactID = c.ContactID
JOIN [Address] adr ON adr.AddressID = car.AddressID
WHERE c.Email = @Email
