USE [Mach1]
GO

/****** Object:  StoredProcedure [m2].[Preferences_POST]    Script Date: 5/19/2016 9:58:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [m2].[Preferences_POST]
	@Username NVarChar(20),
	@Group NVarChar(20) = 'General',
	@Name NVarChar(50),
	@Data Xml = NULL,
	@Image Image = 0x,
	@IsActive Bit = 1,
	@CreatedBy NVarChar(20),
	@CreatedDate DateTime = NULL,
	@EditedBy NVarChar(20) = NULL,
	@EditedDate DateTime = NULL,
	@DisplayName NVarChar(100) = ''
AS
SET NOCOUNT ON

DECLARE 
	@ID int = (SELECT MAX(PreferenceID) FROM Preferences) + 1,
	@Now DateTime = GETDATE()

INSERT INTO dbo.[Preferences]
(
	PreferenceID,
	UserID,
	PreferenceGroup,
	PreferenceName,
	TextData,
	BinaryData,
	Active,
	EnterUserStr,
	EnterDateTime,
	EditUserStr,
	EditDateTime,
	DisplayName
)
VALUES
(
	@ID,
	@Username,
	@Group,
	@Name,
	@Data,
	@Image,
	@IsActive,
	@CreatedBy,
	ISNULL(@CreatedDate, @Now),
	ISNULL(@EditedBy, @CreatedBy),
	ISNULL(@EditedDate, @Now),
	@DisplayName
)
RETURN @ID
GO

