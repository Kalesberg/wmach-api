USE [Mach1]
GO

/****** Object:  StoredProcedure [m2].[Preferences_GET]    Script Date: 5/19/2016 9:20:09 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [m2].[Preferences_GET]
	@ID int = NULL,
	@Username nvarchar(20) = NULL,
	@Group nvarchar(20) = NULL,
	@Name nvarchar(50) = NULL
AS

SELECT
	P.PreferenceID AS 'ID',
	P.UserID AS 'Username',
	P.PreferenceGroup AS 'Group',
	P.PreferenceName AS 'Name',
	P.TextData AS 'Data',
	P.BinaryData AS 'Image',
	P.Active AS 'IsActive',
	P.DisplayName,
	P.EnterUserStr AS 'CreatedBy',
	P.EnterDateTime AS 'CreatedDate',
	P.EditUserStr AS 'EditedBy',
	P.EditDateTime AS 'EditedDate'
FROM Preferences AS P
WHERE
	PreferenceID = COALESCE(@ID, PreferenceID)
	AND	UserID = COALESCE(@Username, UserID)
	AND PreferenceGroup = COALESCE(@Group, PreferenceGroup)
	AND PreferenceName = COALESCE(@Name, PreferenceName)
GO

