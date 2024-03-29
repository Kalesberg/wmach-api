USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetContactByName]    Script Date: 10/11/2016 1:53:25 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[GetContactByName]
@ContactID INT = NULL,
@BusinessPhone NVARCHAR(60) = NULL,
@FirstName NVARCHAR(50) = NULL,
@LastName NVARCHAR(50) = NULL,
@CompanyName NVARCHAR(120) = NULL
AS

--DECLARE @ContactID INT = NULL
--DECLARE @BusinessPhone NVARCHAR(60) = NULL
--DECLARE @FirstName NVARCHAR(50) = NULL
--DECLARE @LastName NVARCHAR(50) = NULL
--DECLARE @CompanyName NVARCHAR(120) = 'WF'

DECLARE @Temp TABLE (ContactID INT, BusinessPhone NVARCHAR(60), FirstName NVARCHAR(50), LastName NVARCHAR(50), CompanyName NVARCHAR(120), DivisionImageURI NVARCHAR(50))

IF @CompanyName IS NULL
BEGIN
	IF @FirstName IS NOT NULL AND LEN(@FirstName) > 0 AND @LastName IS NOT NULL AND LEN(@LastName) > 0
	BEGIN
		INSERT INTO @Temp SELECT con.ContactID, con.BusinessPhone, con.FirstName, con.LastName, con.CompanyName, NULL FROM Contact con
		WHERE con.FirstName LIKE CONCAT('%',@FirstName,'%') AND con.LastName LIKE CONCAT('%',@LastName,'%')
	END
	ELSE IF @FirstName IS NOT NULL AND LEN(@FirstName) > 0
	BEGIN
		INSERT INTO @Temp SELECT con.ContactID, con.BusinessPhone, con.FirstName, con.LastName, con.CompanyName, NULL FROM Contact con 
		WHERE con.FirstName LIKE CONCAT('%',@FirstName,'%')
	END
	ELSE IF @LastName IS NOT NULL AND LEN(@LastName) > 0
	BEGIN
		INSERT INTO @Temp SELECT con.ContactID, con.BusinessPhone, con.FirstName, con.LastName, con.CompanyName, NULL FROM Contact con 
		WHERE con.LastName LIKE CONCAT('%',@LastName,'%')
	END
END
ELSE
BEGIN
	INSERT INTO @Temp SELECT con.ContactID, con.BusinessPhone, con.FirstName, con.LastName, con.CompanyName, NULL FROM Contact con 
	WHERE con.CompanyName LIKE @CompanyName
END

--Now that we've gotten every relevant field except for the DivisionImageURI, now we have to pull that

DECLARE @ImageURI NVARCHAR(50)
DECLARE @UserContactID INT
SELECT TOP 1 @UserContactID = ContactID FROM @Temp
IF @UserContactID IS NOT NULL AND @UserContactID <> 0
	BEGIN
		exec @ImageURI = dbo.fnGetDivisionURIByContactID @ContactID = @UserContactID

		UPDATE @Temp SET DivisionImageURI = @ImageURI
	END

SELECT * FROM @Temp