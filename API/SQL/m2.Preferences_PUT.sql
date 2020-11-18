USE [Mach1]
GO

/****** Object:  StoredProcedure [m2].[Preferences_PUT]    Script Date: 5/19/2016 9:20:31 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[Preferences_PUT]
	@ID int,
	@Username NVarChar(20),
	@Group NVarChar(20),
	@Name NVarChar(50),
	@Data Xml = NULL,
	@Image Image = NULL,
	@IsActive Bit = NULL,
	@CreatedBy NVarChar(20),
	@CreatedDate DateTime = NULL,
	@EditedBy NVarChar(20),
	@EditedDate DateTime = NULL,
	@DisplayName NVarChar(100) = NULL
AS

SET NOCOUNT ON

DECLARE
	@Now DateTime = GETDATE()

UPDATE dbo.[Preferences]
SET
	UserID = ISNULL(@Username, UserID),
	PreferenceGroup = ISNULL(@Group, PreferenceGroup),
	PreferenceName = ISNULL(@Name, PreferenceName),
	TextData = ISNULL(@Data, TextData),
	BinaryData = ISNULL(@Image, BinaryData),
	Active = ISNULL(@IsActive, Active),
	EnterUserStr = ISNULL(@CreatedBy, EnterUserStr),
	EnterDateTime = ISNULL(@CreatedDate, EnterDateTime),
	EditUserStr = @EditedBy,
	EditDateTime = ISNULL(@EditedDate, @Now),
	DisplayName = ISNULL(@DisplayName, DisplayName)
WHERE 
	PreferenceID = @ID
	AND UserID = @Username
	AND PreferenceGroup = @Group
	AND PreferenceName = @Name
GO

