USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetDivisionHierarchy]    Script Date: 10/25/2016 12:51:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[GetDivisionHierarchy]
AS

SELECT parent.DivisionShortName parentName, ISNULL(child.DivisionShortName, '') childName
FROM Division parent
LEFT JOIN Division child ON parent.DivisionID = child.DropDownParentID
WHERE parent.Active = 1 AND parent.DropDownParentID = 0 AND (child.Active = 1 OR child.Active IS NULL)
ORDER BY ISNULL(parent.ParentRank, 99), child.DivisionShortName


grant execute to public
