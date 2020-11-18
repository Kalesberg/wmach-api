ALTER TABLE Division ADD Active BIT, ParentRank INT, DropDownParentID INT, DropDownName VARCHAR(30)
GO


UPDATE Division
SET Active = 1,
DropDownParentID = ParentDivisionID
GO

UPDATE Division
SET Active = 0
WHERE DivisionShortName IN ('WWMOC', 'WIPS', 'WMTD', 'WRS-STG')
GO

UPDATE Division
SET DropDownName = DivisionShortName
GO


UPDATE Division
SET DropDownParentID = 0,
DivisionType = 'Parent',
DropDownName = 'WWPERU'
WHERE DivisionShortName = 'WRS-PE'
GO

UPDATE Division
SET ParentRank = 1
WHERE DivisionShortName = 'WRS'
GO

UPDATE Division
SET ParentRank = 2
WHERE DivisionShortName = 'WMPD'
GO

UPDATE Division
SET ParentRank = 3
WHERE DivisionShortName = 'WF'
GO

UPDATE Division
SET ParentRank = 4
WHERE DivisionShortName = 'WWM'
GO

UPDATE Division
SET ParentRank = 5
WHERE DivisionShortName = 'WWINT'
GO

UPDATE Division
SET ParentRank = 6
WHERE DivisionShortName = 'WWPERU'
GO

UPDATE Division
SET ParentRank = 7
WHERE DivisionShortName = 'SUP'

UPDATE Division
SET DropDownParentID = 0
WHERE DivisionShortName = 'WWM'
GO

UPDATE Division
SET DropDownParentID = 22
WHERE DivisionShortName IN ('WWEU', 'WWPAC')
GO

UPDATE Division
SET DropDownParentID = 0
WHERE DivisionShortName = 'WMPD'

GO


USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetDivisionHierarchy]    Script Date: 10/26/2016 9:44:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[GetDivisionHierarchy]
AS

SELECT parent.DropDownName parentName, ISNULL(child.DropDownName, '') childName
FROM Division parent
LEFT JOIN Division child ON parent.DivisionID = child.DropDownParentID
WHERE parent.Active = 1 AND parent.DropDownParentID = 0 AND (child.Active = 1 OR child.Active IS NULL)
ORDER BY ISNULL(parent.ParentRank, 99), child.DivisionShortName


grant execute to public
