USE [Mach1]
GO

/****** Object:  StoredProcedure [m2].[Preferences_DELETE]    Script Date: 5/19/2016 9:19:52 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [m2].[Preferences_DELETE]
	@ID int,
	@Username nvarchar(20),
	@Group nvarchar(20),
	@Name nvarchar(50)
AS

SET NOCOUNT ON

DELETE FROM dbo.[Preferences] 
WHERE 
	PreferenceID = @ID
	AND UserID = @Username
	AND PreferenceGroup = @Group
	AND PreferenceName = @Name

RETURN
GO

