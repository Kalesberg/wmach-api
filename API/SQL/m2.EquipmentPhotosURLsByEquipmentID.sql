USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[EquipmentPhotoURLsByEquipmentID]    Script Date: 6/7/2016 9:00:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[EquipmentPhotoURLsByEquipmentID]
@equipmentID INT,
@size VARCHAR(10)
AS

SELECT 'http://wwmach.com/images/inventory/' + @size +'/' + [fileName], PrimaryPicture
FROM InventoryPicture
WHERE EquipmentID = @equipmentID AND Active = 1
ORDER BY PrimaryPicture DESC
