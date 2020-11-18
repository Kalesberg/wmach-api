
--CREATE TABLE m2.ReleaseNotes (
--ID INT IDENTITY (1,1) NOT NULL,
--ReleaseID INT NOT NULL,
--ChangeType VARCHAR(20) NOT NULL,
--ReleaseNote VARCHAR(MAX) NOT NULL
--)

--CREATE TABLE m2.Releases (
--ID INT IDENTITY (1,1) NOT NULL,
--ReleaseSystemID INT NOT NULL,
--ReleaseVersion VARCHAR(20),
--ReleaseDate DATETIME NOT NULL
--)

--CREATE TABLE m2.ReleaseSystem (
--ID INT IDENTITY (1,1) NOT NULL,
--ReleaseSystem VARCHAR(100)
--)

--CREATE TABLE m2.ReleaseNoteCheck (
--ID INT IDENTITY (1,1) NOT NULL,
--UserName VARCHAR(200) NOT NULL,
--ReleaseID INT NOT NULL
--)



ALTER PROCEDURE m2.GetMostRecentReleaseNotesBySystem
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

INSERT INTO m2.ReleaseNotes VALUES (2, 'Feature', 'Super Awesome Feature also done') --(2, 'Bug', 'Awesome bug fixed'), (2, 'Feature', 'Awesome Feature Implemented')