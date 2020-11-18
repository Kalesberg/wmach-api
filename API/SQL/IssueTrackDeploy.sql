USE [IssueTrack]
GO

/****** Object:  Table [dbo].[IssueComments]    Script Date: 10/18/2016 10:46:03 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IssueComments](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IssueID] [int] NOT NULL,
	[CommentText] [nvarchar](max) NOT NULL,
	[EnterUserStr] [nvarchar](50) NOT NULL,
	[EnterDateTime] [datetime] NOT NULL,
	[EditUserStr] [nvarchar](50) NOT NULL,
	[EditDateTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

USE [IssueTrack]
GO

/****** Object:  Table [dbo].[IssueImages]    Script Date: 10/18/2016 10:46:11 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[IssueImages](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[IssueID] [int] NOT NULL,
	[Path] [nvarchar](max) NOT NULL,
	[EnterUserStr] [nvarchar](50) NOT NULL,
	[EnterDateTime] [datetime] NOT NULL,
	[EditUserStr] [nvarchar](50) NOT NULL,
	[EditDateTime] [datetime] NOT NULL,
	[Active] [bit] NULL,
	[fileExt] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

USE [IssueTrack]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 10/18/2016 10:46:23 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[M1ContactID] [int] NULL,
	[UserName] [varchar](30) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[IsDeveloper] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


INSERT INTO Users VALUES (0, 'eandersen', 'Eric', 'Andersen', 1),
(0, 'kromero', 'Kyle', 'Romero', 1),
(0, 'mphelps', 'Matt', 'Phelps', 1),
(0, 'dbarth', 'David', 'Barth', 1),
(0, 'ssamborski', 'Sean', 'Samborski', 1)

GO

SET ANSI_PADDING OFF
GO

INSERT INTO enumList (enumID, ModuleGroupID, enumTypeID, enumName, Active, EnterUserStr, EnterDateTime, EditUSerStr, EditDateTime) 
VALUES (1304, NULL, 3, 'Dashboard', 1, 'wwm\eandersen', GETDATE(), 'wwm\eandersen', GETDATE())

INSERT INTO enumList (enumID, ModuleGroupID, enumTypeID, enumName, Active, EnterUserStr, EnterDateTime, EditUSerStr, EditDateTime) 
VALUES (1305, NULL, 3, 'Inventory Search', 1, 'wwm\eandersen', GETDATE(), 'wwm\eandersen', GETDATE())

INSERT INTO enumList (enumID, ModuleGroupID, enumTypeID, enumName, Active, EnterUserStr, EnterDateTime, EditUSerStr, EditDateTime) 
VALUES (1306, NULL, 3, 'Lost Rentals and Sales', 1, 'wwm\eandersen', GETDATE(), 'wwm\eandersen', GETDATE())

INSERT INTO enumList (enumID, ModuleGroupID, enumTypeID, enumName, Active, EnterUserStr, EnterDateTime, EditUSerStr, EditDateTime) 
VALUES (1307, NULL, 3, 'Rental Rates', 1, 'wwm\eandersen', GETDATE(), 'wwm\eandersen', GETDATE())

INSERT INTO ModuleGroup VALUES ('Inventory Search'), ('Lost Rentals and Sales'), ('Rental Rates'), ('Dashboard')

GO


USE [IssueTrack]
GO

/****** Object:  StoredProcedure [m2].[CreateIssue]    Script Date: 10/18/2016 10:47:11 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[CreateIssue]
@Form NVARCHAR(250),
@IssueType NVARCHAR(20),
@Priority NVARCHAR(100),
@Description NVARCHAR(MAX),
@Module NVARCHAR(100),
@User NVARCHAR(50),
@PhotoIDs PhotoIDs READONLY
AS

DECLARE @IssueID INT = (SELECT MAX(IssueID) FROM Issue) + 1;
DECLARE @ModuleID INT = (SELECT enumID FROM enumList WHERE enumName = @module)

INSERT INTO Issue (IssueID, Environment, SystemName, SystemID, ModuleName, ModuleID, FormName, IssueType, Priority, IssueStatus, AssignedTo, AssignedToID, AssignedDate, EstReleaseDate, ReleaseVersion, ReleasePriority, Description, StepsToRecreate, Resolution, CompletedDate, EstimatedHours, EnterUserStr, EnterDateTime, EditUserStr, EditDateTime, Historical)
VALUES (@IssueID, 'Production', '', 1019, '', @ModuleID, @form, @issueType, @priority, 'Open', 1000, NULL, NULL, NULL, 'NotAsg', 0, @description, '', '', NULL, 0, @user, GETDATE(), @user, GETDATE(), 0)


INSERT INTO IssueImages
SELECT @IssueID, ID, @User, GETDATE(), @User, GETDATE(), 1, '.jpg'
FROM @PhotoIDs

RETURN @IssueID
GO

USE [IssueTrack]
GO

/****** Object:  StoredProcedure [m2].[CreateIssueComment]    Script Date: 10/18/2016 10:47:19 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[CreateIssueComment]
@IssueID INT,
@Comment NVARCHAR(MAX),
@User NVARCHAR(50)
AS


INSERT INTO IssueComments VALUES (@IssueID, @Comment, @User, GETDATE(), @User, GETDATE())

DECLARE @ID INT = (SELECT MAX(ID) FROM IssueComments)
RETURN @ID
GO

USE [IssueTrack]
GO

/****** Object:  StoredProcedure [m2].[CreateIssueImages]    Script Date: 10/18/2016 10:47:27 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[CreateIssueImages]
@IssueID INT,
@PhotoIDs PhotoIDs READONLY,
@User NVARCHAR(50)
AS

INSERT INTO IssueImages
SELECT @IssueID, ID, @User, GETDATE(), @User, GETDATE(), 1, 'jpg'
FROM @PhotoIDs
GO

USE [IssueTrack]
GO

/****** Object:  StoredProcedure [m2].[CreateIssueTrackUser]    Script Date: 10/18/2016 10:47:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[CreateIssueTrackUser]
@UserName NVARCHAR(50),
@FirstName NVARCHAR(50),
@LastName NVARCHAR(50)
AS

INSERT INTO Users VALUES (0, @UserName, @FirstName, @LastName, 0)
GO

USE [IssueTrack]
GO

/****** Object:  StoredProcedure [m2].[DeactivateIssueImage]    Script Date: 10/18/2016 10:47:44 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[DeactivateIssueImage]
@issueID INT,
@fileName NVARCHAR(40)
AS

UPDATE IssueImages
SET Active = 0
WHERE IssueID = @issueID AND [Path] = @fileName
GO

USE [IssueTrack]
GO

/****** Object:  StoredProcedure [m2].[GetIssueByID]    Script Date: 10/18/2016 10:47:51 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[GetIssueByID]
@IssueID INT
AS

SELECT IssueID ID, module.enumName Module, FormName, IssueType, IssueStatus [Status], [Priority], [Description], issue.EnterDateTime Created
FROM Issue issue
JOIN enumList module ON module.enumID = issue.ModuleID
WHERE IssueID = @IssueID
GO

USE [IssueTrack]
GO

/****** Object:  StoredProcedure [m2].[GetIssueCommentsbyIssueID]    Script Date: 10/18/2016 10:47:57 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[GetIssueCommentsbyIssueID]
@IssueID INT
AS

SELECT ID, IssueID, CommentText [text], EnterUserStr UserName, EnterDateTime Created
FROM IssueComments
WHERE IssueID = @IssueID

GO

USE [IssueTrack]
GO

/****** Object:  StoredProcedure [m2].[GetIssueCommentsByLookupIDs]    Script Date: 10/18/2016 10:48:03 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [m2].[GetIssueCommentsByLookupIDs]
@LookupIDs LookupIDs READONLY
AS

SELECT ID, IssueID, CommentText, EnterUserStr CreatedBy, EnterDateTime Created
FROM IssueComments
WHERE IssueID IN (SELECT ID FROM @LookupIDs)


GO


USE [IssueTrack]
GO

/****** Object:  StoredProcedure [m2].[GetIssueImagesByIssueID]    Script Date: 10/18/2016 10:48:18 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[GetIssueImagesByIssueID]
@IssueID INT
AS

SELECT ID, IssueID, [Path], EnterUserStr Created
FROM IssueImages
WHERE IssueID = @IssueID AND Active = 1
GO

USE [IssueTrack]
GO

/****** Object:  StoredProcedure [m2].[GetIssueImagesByLookupIDs]    Script Date: 10/18/2016 10:48:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [m2].[GetIssueImagesByLookupIDs]
@LookupIDs LookupIDs READONLY
AS

SELECT ID, IssueID, [Path], EnterUserStr Created
FROM IssueImages
WHERE IssueID IN (SELECT ID FROM @LookupIDs) AND Active = 1


GO


USE [IssueTrack]
GO

/****** Object:  StoredProcedure [m2].[GetIssues]    Script Date: 10/18/2016 10:48:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[GetIssues]
@IssueStatus VARCHAR(20) = NULL
AS

SELECT IssueID ID, module.enumName Module, FormName Form, IssueType, IssueStatus [Status], [Priority], [Description], issue.EnterDateTime Created
FROM Issue issue
JOIN enumList module ON module.enumID = issue.ModuleID
WHERE IssueStatus = COALESCE(@IssueStatus, IssueStatus) AND Historical <> 0

GO

USE [IssueTrack]
GO

/****** Object:  StoredProcedure [m2].[GetUserByUserName]    Script Date: 10/18/2016 10:48:48 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [m2].[GetUserByUserName]
@UserName NVARCHAR(50)
AS

SELECT ID, M1ContactID, UserName, FirstName, LastName, IsDeveloper
FROM Users
WHERE UserName = @UserName
GO

USE [IssueTrack]
GO

/****** Object:  UserDefinedFunction [m2].[IsIssueTrackUser]    Script Date: 10/18/2016 10:49:04 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [m2].[IsIssueTrackUser] (@UserName NVARCHAR(50))

RETURNS BIT AS  

BEGIN 

SET @UserName = (SELECT UserName FROM Users WHERE UserName = @UserName);

DECLARE @Exists BIT;
IF(@UserName IS NULL)
	BEGIN
		SET @Exists = 0
	END
ELSE
	BEGIN
		SET @Exists = 1
	END

RETURN @Exists

END
 
GO

USE [IssueTrack]
GO

/****** Object:  UserDefinedFunction [m2].[IsUserDeveloper]    Script Date: 10/18/2016 10:49:13 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [m2].[IsUserDeveloper] (@UserName NVARCHAR(50))

RETURNS BIT AS  

BEGIN 

DECLARE @IsDev BIT = (SELECT IsDeveloper FROM Users WHERE UserName = @UserName);

RETURN @IsDev

END

GO


USE [IssueTrack]
GO

/****** Object:  UserDefinedTableType [dbo].[LookupIDs]    Script Date: 10/18/2016 10:50:08 AM ******/
CREATE TYPE [dbo].[LookupIDs] AS TABLE(
	[ID] [int] NOT NULL
)
GO

USE [IssueTrack]
GO

/****** Object:  UserDefinedTableType [dbo].[PhotoIDs]    Script Date: 10/18/2016 10:50:18 AM ******/
CREATE TYPE [dbo].[PhotoIDs] AS TABLE(
	[ID] [nvarchar](40) NOT NULL
)
GO





