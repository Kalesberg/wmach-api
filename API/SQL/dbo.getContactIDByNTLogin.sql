USE [Mach1]
GO
/****** Object:  UserDefinedFunction [dbo].[fnGetContactName]    Script Date: 9/29/2016 3:35:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[getContactIDByNTLogin]
	(
	@ContactNTLogin NVARCHAR(MAX),
	@ContactID INT = NULL
	)
	
	/*
		Returns the Contact Name assocuiated with a Contact ID
		if the Contact is not Found the Missing Value is returned.
		To test for ID not Found - specify NULL for the Missing Value and test for isNull upon return
	*/
RETURNS INT	
AS
	BEGIN
	RETURN ( 
				isnull(
				(Select Top 1 ContactID
				FROM Contact
				WHERE NTLogin LIKE @ContactNTLogin), @ContactID)
			)
	END

