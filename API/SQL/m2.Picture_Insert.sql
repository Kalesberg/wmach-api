USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[Picture_Insert]    Script Date: 6/7/2016 10:10:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[Picture_Insert]
@SerialNum VARCHAR(100)
AS


DECLARE @EquipmentID INT = (SELECT EquipmentID FROM Equipment WHERE SerialNum = @SerialNum)
DECLARE @PictureID INT = (SELECT MAX(InventoryPictureID) FROM InventoryPicture) + 1
DECLARE @FileName VARCHAR(20) = CAST(@PictureID AS VARCHAR(10)) + '.jpg'
DECLARE @InventoryPictureSort INT = (SELECT MAX(InventoryPictureSort) FROM InventoryPicture WHERE EquipmentID = @EquipmentID)

IF @InventoryPictureSort <> 9999
	BEGIN
		SET @InventoryPictureSort = @InventoryPictureSort + 1
	END

INSERT INTO InventoryPicture (InventoryPictureID, EquipmentID, PictureAngle, FolderName, [Filename], PictureType, InventoryPictureDesc, PrimaryPicture, Deletable, Active, InventoryPictureSort, EnterUserStr, EnterDateTime, EditUserStr, EditDateTime)
VALUES (@PictureID, @EquipmentID, 'Unknown', '', @FileName, '.JPG', '', 0, 0, 1, @InventoryPictureSort, 'wwm\mobile_m1', GETDATE(), 'wwm\mobile_m1', GETDATE())

RETURN @PictureID;