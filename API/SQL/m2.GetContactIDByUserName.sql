create procedure m2.GetContactIdByUsername
@Username VARCHAR(30)
AS

SELECT ContactID FROM Contact
WHERE NTLogin = @Username
