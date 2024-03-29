USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetAttachmentsMobile]    Script Date: 11/20/2015 10:41:11 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [m2].[GetAttachmentsMobile]
@equipmentID INT = NULL
AS

SELECT [EquipmentID]
      ,[SerialNum]
      ,[AttachmentPosition]
      ,[AttachmentDefaultPosition]
	  ,p.ProductCategoryName
	  ,p.ProductCategoryDesc


  FROM [Mach1].[dbo].[Equipment] e
  Join InventoryMaster i on (i.InventoryMasterID = e.InventoryMasterID)
  Join ProductCategory p on (p.ProductCategoryID=i.ProductCategoryID)
  where MachineAttachedToID=@equipmentID

