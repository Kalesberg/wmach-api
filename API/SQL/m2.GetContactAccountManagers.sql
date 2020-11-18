USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetContactByName]    Script Date: 10/13/2016 2:13:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[GetContactAccountManagers]
AS

--DECLARE @ContactID INT = NULL
--DECLARE @BusinessPhone NVARCHAR(60) = NULL
--DECLARE @FirstName NVARCHAR(50) = NULL
--DECLARE @LastName NVARCHAR(50) = NULL
--DECLARE @CompanyName NVARCHAR(120) = 'WF'

DECLARE @Temp TABLE (ContactID INT, BusinessPhone NVARCHAR(60), FirstName NVARCHAR(50), LastName NVARCHAR(50), CompanyName NVARCHAR(120), DivisionImageURI NVARCHAR(50))
	

INSERT INTO @Temp
SELECT con.ContactID, con.BusinessPhone, con.FirstName, con.LastName, con.CompanyName, NULL FROM ContactCategory cCat
INNER JOIN Contact con ON cCat.ContactID = con.ContactID
WHERE cCat.ContactCategoryType LIKE 'Salesperson' AND cCat.Active = 1
ORDER BY con.LastName + ', ' + con.FirstName

DECLARE @ImageURI NVARCHAR(50)
DECLARE @UserContactID INT
SELECT TOP 1 @UserContactID = ContactID FROM @Temp WHERE DivisionImageURI is null

WHILE EXISTS (SELECT TOP 1 ContactID FROM @Temp WHERE DivisionImageURI IS NULL)
BEGIN
	SELECT TOP 1 @UserContactID = ContactID FROM @Temp WHERE DivisionImageURI IS NULL
	IF @UserContactID IS NOT NULL AND @UserContactID <> 0
		BEGIN
			exec @ImageURI = dbo.fnGetDivisionURIByContactID @ContactID = @UserContactID

			UPDATE @Temp SET DivisionImageURI = @ImageURI
		END
END



SELECT * FROM @Temp
