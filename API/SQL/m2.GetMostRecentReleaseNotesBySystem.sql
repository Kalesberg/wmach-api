USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetMostRecentReleaseNotesBySystem]    Script Date: 1/6/2016 3:45:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[GetMostRecentReleaseNotesBySystem]
@ReleaseSystem VARCHAR(100),
@UserName VARCHAR(200) = NULL
AS

SET NOCOUNT ON

DECLARE @ReleaseData TABLE (
ReleaseSystem VARCHAR(100),
ReleaseID INT,
ReleaseNote VARCHAR(MAX),
ChangeType VARCHAR(20),
ReleaseVersion VARCHAR(20),
ReleaseDate DATETIME,
Ranking INT
)

INSERT INTO @ReleaseData
SELECT ReleaseSystem, ReleaseID, ReleaseNote, ChangeType, ReleaseVersion, ReleaseDate, RANK() OVER (PARTITION BY ReleaseSystem ORDER BY ReleaseID DESC) AS 'Ranking'
FROM m2.Releases rel
JOIN m2.ReleaseSystem relSys ON relSys.ID = rel.ReleaseSystemID
JOIN m2.ReleaseNotes relNotes ON relNotes.ReleaseID = rel.ID
WHERE relSys.ReleaseSystem = @ReleaseSystem

DECLARE @CurrentRelease INT = (SELECT DISTINCT ReleaseID FROM @ReleaseData WHERE Ranking = 1);

IF(@UserName IS NOT NULL)
	BEGIN
		IF (EXISTS(SELECT UserName FROM m2.ReleaseNoteCheck WHERE ReleaseID = @CurrentRelease AND UserName = COALESCE(@UserName, '')))
			BEGIN
				RETURN NULL
			END
		ELSE
			BEGIN	
				INSERT INTO m2.ReleaseNoteCheck VALUES (@UserName, @CurrentRelease)
			END
	END

SELECT ChangeType, ReleaseNote 
FROM @ReleaseData 
WHERE Ranking = 1