USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[UpdateEquipmentListRates]    Script Date: 7/26/2016 11:31:31 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [m2].[UpdateEquipment]
(
	@EquipmentID Int,
	@MachineAttachedToID Int=null,
	@AttachmentPosition NVarChar(12)=null,
	@RentalStatus NVarChar(24)=null,
	@AssignedDivisionID Int=null,
	@CurrentLocationID Int=null,
	@SerialNum NVarChar(50)=null,
	@SerialNumStripped NVarChar(50)=null,
	@PropertyTag NVarChar(50)=null,
	@PropertyTagStripped NVarChar(50)=null,
	@SupplierID Int=null,
	@InventoryMasterID Int=null,
	@PictureCount Int=null,
	@BarCode NVarChar(20)=null,
	@AdditionalCapCost Money=null,
	@MarketingDescription NVarChar(1024)=null,
	@ConditionID Int=null,
	@AppraiserID Int=null,
	@ForRent Bit=null,
	@ForSale Bit=null,
	@Rerented Bit=null,
	@PublicViewable Bit=null,
	@PublicPriceViewable Bit=null,
	@Price Money=null,
	@MinPrice Money=null,
	@MonthlyRentalRate Money=null,
	@OrderlyLiquidationValue Money=null,
	@InsuranceValue Money=null,
	@RentalPurchaseOptionPrice Money=null,
	@PurchasePrice Money=null,
	@Hours Int=null,
	@ManufacturedYear Int=null,
	@DateAcquired DateTime=null,
	@DateSold DateTime=null,
	@IsTemplate Bit=null,
	@Active Bit=null,
	@Deletable Bit=null,
	@EnterUserStr NVarChar(20)=null,
	@EnterDateTime DateTime=null,
	@EditUserStr NVarChar(20)=null,
	@EditDateTime DateTime=null,
	@TimeStamp Timestamp=null,
	@OwnerType NVarChar(15)=null,
	@ProjectNumber NVarChar(20)=null,
	@OriginalHour Int=null,
	@RentDateAvailable DateTime=null,
	@SaleDateAvailable DateTime=null,
	@LocationStatus NVarChar(50)=null,
	@PackageNumber NVarChar(20)=null,
	@EnterpriseBuyerID Int=null,
	@AcqAttachmentPrice Money=null,
	@AcqShippingCost Money=null,
	@AddToFixedAsset Bit=null,
	@DateSentFixedAsset DateTime=null,
	@OwnedByID Int=null,
	@LastAppraisalDate DateTime=null,
	@MinMonthlyRentalRate Money=null,
	@WarrantyExpDate DateTime=null,
	@WarrantyExpClick Int=null,
	@WarrantyServicer NVarChar(80)=null,
	@PurchasedNew Bit=null,
	@OriginalPONumber NVarChar(20)=null,
	@DepreciateAsset Bit=null,
	@SoldPrice Money=null,
	@SoldToID Int=null,
	@SoldInvoiceNumber NVarChar(20)=null,
	@BrokerPrice Money=null,
	@PublicViewSerialNum Bit=null,
	@EquipmentSize Decimal(10,2)=null,
	@EquipmentSizeUnit NVarChar(30)=null,
	@AttachmentDefaultPosition NVarChar(12)=null,
	@Weight Decimal(8,2)=null,
	@Height Decimal(8,2)=null,
	@Length Decimal(8,2)=null,
	@Width Decimal(8,2)=null,
	@LocationContactRelationshipID Int=null,
	@EquipmentDescriptor NVarChar(max)=null,
	@ServiceStatus NVarChar(20)=null,
	@UnitNumber NVarChar(20)=null,
	@SMMPlate NVarChar(20)=null,
	@SMMTag NVarChar(20)=null,
	@SalesmanContactID Int=null,
	@SmmTabExpiresOn DateTime=null,
	@SmmPlateExpiresOn DateTime=null,
	@LicensingNotes NVarChar(25)=null,
	@SmmTabLastOne NVarChar(20)=null,
	@SmmPlateLastOne NVarChar(20)=null,
	@EquipmentStatusNotes NVarChar(max)=null,
	@EquipmentSize2 Decimal(10,2)=null,
	@SizeRangeType NVarChar(50)=null,
	@CustomerName NVarChar(50)=null,
	@CustomerPhone NVarChar(30)=null,
	@EngArrNum NVarChar(50)=null,
	@TranArrNum NVarChar(50)=null,
	@MachArrNum NVarChar(50)=null,
	@EngSerialNum NVarChar(50)=null,
	@TranSerialNum NVarChar(50)=null,
	@HasHours Bit=null,
	@Generation Int=null,
	@AssetOwner Int=null,
	@AssetLife Int=null,
	@HasMiles Bit=null,
	@Miles Int=null,
	@ExcludeFromForecast Bit=null,
	@AuctionValue Money=null
	)
AS
BEGIN
	-- ===================================

	-- ===================================
	SET NOCOUNT ON;

	DECLARE @EquipmentIDParam Int
	DECLARE @MachineAttachedToIDParam Int
	DECLARE @AttachmentPositionParam NVarChar(12)
	DECLARE @RentalStatusParam NVarChar(24)
	DECLARE @AssignedDivisionIDParam Int
	DECLARE @CurrentLocationIDParam Int
	DECLARE @SerialNumParam NVarChar(50)
	DECLARE @SerialNumStrippedParam NVarChar(50)
	DECLARE @PropertyTagParam NVarChar(50)
	DECLARE @PropertyTagStrippedParam NVarChar(50)
	DECLARE @SupplierIDParam Int
	DECLARE @InventoryMasterIDParam Int
	DECLARE @PictureCountParam Int
	DECLARE @BarCodeParam NVarChar(20)
	DECLARE @AdditionalCapCostParam Money
	DECLARE @MarketingDescriptionParam NVarChar(1024)
	DECLARE @ConditionIDParam Int
	DECLARE @AppraiserIDParam Int
	DECLARE @ForRentParam Bit
	DECLARE @ForSaleParam Bit
	DECLARE @RerentedParam Bit
	DECLARE @PublicViewableParam Bit
	DECLARE @PublicPriceViewableParam Bit
	DECLARE @PriceParam Money
	DECLARE @MinPriceParam Money
	DECLARE @MonthlyRentalRateParam Money
	DECLARE @OrderlyLiquidationValueParam Money
	DECLARE @InsuranceValueParam Money
	DECLARE @RentalPurchaseOptionPriceParam Money
	DECLARE @PurchasePriceParam Money
	DECLARE @HoursParam Int
	DECLARE @ManufacturedYearParam Int
	DECLARE @DateAcquiredParam DateTime
	DECLARE @DateSoldParam DateTime
	DECLARE @IsTemplateParam Bit
	DECLARE @ActiveParam Bit
	DECLARE @DeletableParam Bit
	DECLARE @EnterUserStrParam NVarChar(20)
	DECLARE @EnterDateTimeParam DateTime
	DECLARE @EditUserStrParam NVarChar(20)
	DECLARE @EditDateTimeParam DateTime
	DECLARE @TimeStampParam Timestamp
	DECLARE @OwnerTypeParam NVarChar(15)
	DECLARE @ProjectNumberParam NVarChar(20)
	DECLARE @OriginalHourParam Int
	DECLARE @RentDateAvailableParam DateTime
	DECLARE @SaleDateAvailableParam DateTime
	DECLARE @LocationStatusParam NVarChar(50)
	DECLARE @PackageNumberParam NVarChar(20)
	DECLARE @EnterpriseBuyerIDParam Int
	DECLARE @AcqAttachmentPriceParam Money
	DECLARE @AcqShippingCostParam Money
	DECLARE @AddToFixedAssetParam Bit
	DECLARE @DateSentFixedAssetParam DateTime
	DECLARE @OwnedByIDParam Int
	DECLARE @LastAppraisalDateParam DateTime
	DECLARE @MinMonthlyRentalRateParam Money
	DECLARE @WarrantyExpDateParam DateTime
	DECLARE @WarrantyExpClickParam Int
	DECLARE @WarrantyServicerParam NVarChar(80)
	DECLARE @PurchasedNewParam Bit
	DECLARE @OriginalPONumberParam NVarChar(20)
	DECLARE @DepreciateAssetParam Bit
	DECLARE @SoldPriceParam Money
	DECLARE @SoldToIDParam Int
	DECLARE @SoldInvoiceNumberParam NVarChar(20)
	DECLARE @BrokerPriceParam Money
	DECLARE @PublicViewSerialNumParam Bit
	DECLARE @EquipmentSizeParam Decimal(10,2)
	DECLARE @EquipmentSizeUnitParam NVarChar(30)
	DECLARE @AttachmentDefaultPositionParam NVarChar(12)
	DECLARE @WeightParam Decimal(8,2)
	DECLARE @HeightParam Decimal(8,2)
	DECLARE @LengthParam Decimal(8,2)
	DECLARE @WidthParam Decimal(8,2)
	DECLARE @LocationContactRelationshipIDParam Int
	DECLARE @EquipmentDescriptorParam NVarChar(max)
	DECLARE @ServiceStatusParam NVarChar(20)
	DECLARE @UnitNumberParam NVarChar(20)
	DECLARE @SMMPlateParam NVarChar(20)
	DECLARE @SMMTagParam NVarChar(20)
	DECLARE @SalesmanContactIDParam Int
	DECLARE @SmmTabExpiresOnParam DateTime
	DECLARE @SmmPlateExpiresOnParam DateTime
	DECLARE @LicensingNotesParam NVarChar(25)
	DECLARE @SmmTabLastOneParam NVarChar(20)
	DECLARE @SmmPlateLastOneParam NVarChar(20)
	DECLARE @EquipmentStatusNotesParam NVarChar(max)
	DECLARE @EquipmentSize2Param Decimal(10,2)
	DECLARE @SizeRangeTypeParam NVarChar(50)
	DECLARE @CustomerNameParam NVarChar(50)
	DECLARE @CustomerPhoneParam NVarChar(30)
	DECLARE @EngArrNumParam NVarChar(50)
	DECLARE @TranArrNumParam NVarChar(50)
	DECLARE @MachArrNumParam NVarChar(50)
	DECLARE @EngSerialNumParam NVarChar(50)
	DECLARE @TranSerialNumParam NVarChar(50)
	DECLARE @HasHoursParam Bit
	DECLARE @GenerationParam Int
	DECLARE @AssetOwnerParam Int
	DECLARE @AssetLifeParam Int
	DECLARE @HasMilesParam Bit
	DECLARE @MilesParam Int
	DECLARE @ExcludeFromForecastParam Bit
	DECLARE @AuctionValueParam Money

	SELECT
	@EquipmentIDParam=EquipmentID,
	@MachineAttachedToIDParam=MachineAttachedToID,
	@AttachmentPositionParam=AttachmentPosition,
	@RentalStatusParam=RentalStatus,
	@AssignedDivisionIDParam=AssignedDivisionID,
	@CurrentLocationIDParam=CurrentLocationID,
	@SerialNumParam=SerialNum,
	@SerialNumStrippedParam=SerialNumStripped,
	@PropertyTagParam=PropertyTag,
	@PropertyTagStrippedParam=PropertyTagStripped,
	@SupplierIDParam=SupplierID,
	@InventoryMasterIDParam=InventoryMasterID,
	@PictureCountParam=PictureCount,
	@BarCodeParam=BarCode,
	@AdditionalCapCostParam=AdditionalCapCost,
	@MarketingDescriptionParam=MarketingDescription,
	@ConditionIDParam=ConditionID,
	@AppraiserIDParam=AppraiserID,
	@ForRentParam=ForRent,
	@ForSaleParam=ForSale,
	@RerentedParam=Rerented,
	@PublicViewableParam=PublicViewable,
	@PublicPriceViewableParam=PublicPriceViewable,
	@PriceParam=Price,
	@MinPriceParam=MinPrice,
	@MonthlyRentalRateParam=MonthlyRentalRate,
	@OrderlyLiquidationValueParam=OrderlyLiquidationValue,
	@InsuranceValueParam=InsuranceValue,
	@RentalPurchaseOptionPriceParam=RentalPurchaseOptionPrice,
	@PurchasePriceParam=PurchasePrice,
	@HoursParam=[Hours],
	@ManufacturedYearParam=ManufacturedYear,
	@DateAcquiredParam=DateAcquired,
	@DateSoldParam=DateSold,
	@IsTemplateParam=IsTemplate,
	@ActiveParam=Active,
	@DeletableParam=Deletable,
	@EnterUserStrParam=EnterUserStr,
	@EnterDateTimeParam=EnterDateTime,
	@EditUserStrParam=EditUserStr,
	@EditDateTimeParam=EditDateTime,
	@TimeStampParam=[TimeStamp],
	@OwnerTypeParam=OwnerType,
	@ProjectNumberParam=ProjectNumber,
	@OriginalHourParam=OriginalHour,
	@RentDateAvailableParam=RentDateAvailable,
	@SaleDateAvailableParam=SaleDateAvailable,
	@LocationStatusParam=LocationStatus,
	@PackageNumberParam=PackageNumber,
	@EnterpriseBuyerIDParam=EnterpriseBuyerID,
	@AcqAttachmentPriceParam=AcqAttachmentPrice,
	@AcqShippingCostParam=AcqShippingCost,
	@AddToFixedAssetParam=AddToFixedAsset,
	@DateSentFixedAssetParam=DateSentFixedAsset,
	@OwnedByIDParam=OwnedByID,
	@LastAppraisalDateParam=LastAppraisalDate,
	@MinMonthlyRentalRateParam=MinMonthlyRentalRate,
	@WarrantyExpDateParam=WarrantyExpDate,
	@WarrantyExpClickParam=WarrantyExpClick,
	@WarrantyServicerParam=WarrantyServicer,
	@PurchasedNewParam=PurchasedNew,
	@OriginalPONumberParam=OriginalPONumber,
	@DepreciateAssetParam=DepreciateAsset,
	@SoldPriceParam=SoldPrice,
	@SoldToIDParam=SoldToID,
	@SoldInvoiceNumberParam=SoldInvoiceNumber,
	@BrokerPriceParam=BrokerPrice,
	@PublicViewSerialNumParam=PublicViewSerialNum,
	@EquipmentSizeParam=EquipmentSize,
	@EquipmentSizeUnitParam=EquipmentSizeUnit,
	@AttachmentDefaultPositionParam=AttachmentDefaultPosition,
	@WeightParam=[Weight],
	@HeightParam=Height,
	@LengthParam=[Length],
	@WidthParam=[Width],
	@LocationContactRelationshipIDParam=LocationContactRelationshipID,
	@EquipmentDescriptorParam=EquipmentDescriptor,
	@ServiceStatusParam=ServiceStatus,
	@UnitNumberParam=UnitNumber,
	@SMMPlateParam=SMMPlate,
	@SMMTagParam=SMMTag,
	@SalesmanContactIDParam=SalesmanContactID,
	@SmmTabExpiresOnParam=SmmTabExpiresOn,
	@SmmPlateExpiresOnParam=SmmPlateExpiresOn,
	@LicensingNotesParam=LicensingNotes,
	@SmmTabLastOneParam=SmmTabLastOne,
	@SmmPlateLastOneParam=SmmPlateLastOne,
	@EquipmentStatusNotesParam=EquipmentStatusNotes,
	@EquipmentSize2Param=EquipmentSize2,
	@SizeRangeTypeParam=SizeRangeType,
	@CustomerNameParam=CustomerName,
	@CustomerPhoneParam=CustomerPhone,
	@EngArrNumParam=EngArrNum,
	@TranArrNumParam=TranArrNum,
	@MachArrNumParam=MachArrNum,
	@EngSerialNumParam=EngSerialNum,
	@TranSerialNumParam=TranSerialNum,
	@HasHoursParam=HasHours,
	@GenerationParam=Generation,
	@AssetOwnerParam=AssetOwner,
	@AssetLifeParam=AssetLife,
	@HasMilesParam=HasMiles,
	@MilesParam=Miles,
	@ExcludeFromForecastParam=ExcludeFromForecast,
	@AuctionValueParam=AuctionValue
	FROM Equipment WHERE EquipmentID = @EquipmentID

	DECLARE @Updated BIT = 0

	IF @EquipmentID Is Not Null
		BEGIN
		------------------------------------------Iterating each parameter and updating only pertinent records if they have changed -------------------------------


		--------------------------------------------Transaction Wrapper------------------------------------------------

			BEGIN TRY
				BEGIN TRANSACTION

					--First and foremost we need to update the equipment history


					IF @EquipmentDescriptor IS NOT NULL AND @EquipmentDescriptor <> @EquipmentDescriptorParam
					BEGIN
						UPDATE Equipment
						SET EquipmentDescriptor = @EquipmentDescriptor
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @MachineAttachedToID IS NOT NULL AND @MachineAttachedToID <> @MachineAttachedToIDParam
					BEGIN
						UPDATE Equipment
						SET MachineAttachedToID = @MachineAttachedToID
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @RentalStatus IS NOT NULL AND @RentalStatus <> @RentalStatusParam
					BEGIN
						UPDATE Equipment
						SET RentalStatus = @RentalStatus
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @AssignedDivisionID IS NOT NULL AND @AssignedDivisionID <> @AssignedDivisionIDParam
					BEGIN
						UPDATE Equipment
						SET AssignedDivisionID = @AssignedDivisionID
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @CurrentLocationID IS NOT NULL AND @CurrentLocationID <> @CurrentLocationIDParam
					BEGIN
						UPDATE Equipment
						SET CurrentLocationID = @CurrentLocationID
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SerialNum IS NOT NULL AND @SerialNum <> @SerialNumParam
					BEGIN
						UPDATE Equipment
						SET SerialNum = @SerialNum
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SerialNumStripped IS NOT NULL AND @SerialNumStripped <> @SerialNumStrippedParam
					BEGIN
						UPDATE Equipment
						SET SerialNumStripped = @SerialNumStripped
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @PropertyTag IS NOT NULL AND @PropertyTag <> @PropertyTagParam
					BEGIN
						UPDATE Equipment
						SET PropertyTag = @PropertyTag
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @PropertyTagStripped IS NOT NULL AND @PropertyTagStripped <> @PropertyTagStrippedParam
					BEGIN
						UPDATE Equipment
						SET PropertyTagStripped = @PropertyTagStripped
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SupplierID IS NOT NULL AND @SupplierID <> @SupplierIDParam
					BEGIN
						UPDATE Equipment
						SET SupplierID = @SupplierID
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @InventoryMasterID IS NOT NULL AND @InventoryMasterID <> @InventoryMasterIDParam
					BEGIN
						UPDATE Equipment
						SET InventoryMasterID = @InventoryMasterID
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @PictureCount IS NOT NULL AND @PictureCount <> @PictureCountParam
					BEGIN
						UPDATE Equipment
						SET PictureCount = @PictureCount
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @BarCode IS NOT NULL AND @BarCode <> @BarCodeParam
					BEGIN
						UPDATE Equipment
						SET BarCode = @BarCode
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @AdditionalCapCost IS NOT NULL AND @AdditionalCapCost <> @AdditionalCapCostParam
					BEGIN
						UPDATE Equipment
						SET AdditionalCapCost = @AdditionalCapCost
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @MarketingDescription IS NOT NULL AND @MarketingDescription <> @MarketingDescriptionParam
					BEGIN
						UPDATE Equipment
						SET MarketingDescription = @MarketingDescription
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @ConditionID IS NOT NULL AND @ConditionID <> @ConditionIDParam
					BEGIN
						UPDATE Equipment
						SET ConditionID = @ConditionID
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @AppraiserID IS NOT NULL AND @AppraiserID <> @AppraiserIDParam
					BEGIN
						UPDATE Equipment
						SET AppraiserID = @AppraiserID
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @ForRent IS NOT NULL AND @ForRent <> @ForRentParam
					BEGIN
						UPDATE Equipment
						SET ForRent = @ForRent
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @ForSale IS NOT NULL AND @ForSale <> @ForSaleParam
					BEGIN
						UPDATE Equipment
						SET ForSale = @ForSale
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @Rerented IS NOT NULL AND @Rerented <> @RerentedParam
					BEGIN
						UPDATE Equipment
						SET Rerented = @Rerented
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @PublicViewable IS NOT NULL AND @PublicViewable <> @PublicViewableParam
					BEGIN
						UPDATE Equipment
						SET PublicViewable = @PublicViewable
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @PublicPriceViewable IS NOT NULL AND @PublicPriceViewable <> @PublicPriceViewableParam
					BEGIN
						UPDATE Equipment
						SET PublicPriceViewable = @PublicPriceViewable
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @Price IS NOT NULL AND @Price <> @PriceParam
					BEGIN
						UPDATE Equipment
						SET Price = @Price
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @MinPrice IS NOT NULL AND @MinPrice <> @MinPriceParam
					BEGIN
						UPDATE Equipment
						SET MinPrice = @MinPrice
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @MonthlyRentalRate IS NOT NULL AND @MonthlyRentalRate <> @MonthlyRentalRateParam
					BEGIN
						UPDATE Equipment
						SET MonthlyRentalRate = @MonthlyRentalRate
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @OrderlyLiquidationValue IS NOT NULL AND @OrderlyLiquidationValue <> @OrderlyLiquidationValueParam
					BEGIN
						UPDATE Equipment
						SET OrderlyLiquidationValue = @OrderlyLiquidationValue
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @InsuranceValue IS NOT NULL AND @InsuranceValue <> @InsuranceValueParam
					BEGIN
						UPDATE Equipment
						SET InsuranceValue = @InsuranceValue
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @RentalPurchaseOptionPrice IS NOT NULL AND @RentalPurchaseOptionPrice <> @RentalPurchaseOptionPriceParam
					BEGIN
						UPDATE Equipment
						SET RentalPurchaseOptionPrice = @RentalPurchaseOptionPrice
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @PurchasePrice IS NOT NULL AND @PurchasePrice <> @PurchasePriceParam
					BEGIN
						UPDATE Equipment
						SET PurchasePrice = @PurchasePrice
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @Hours IS NOT NULL AND @Hours <> @HoursParam
					BEGIN
						UPDATE Equipment
						SET Hours = @Hours
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @ManufacturedYear IS NOT NULL AND @ManufacturedYear <> @ManufacturedYearParam
					BEGIN
						UPDATE Equipment
						SET ManufacturedYear = @ManufacturedYear
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @DateAcquired IS NOT NULL AND @DateAcquired <> @DateAcquiredParam
					BEGIN
						UPDATE Equipment
						SET DateAcquired = @DateAcquired
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @DateSold IS NOT NULL AND @DateSold <> @DateSoldParam
					BEGIN
						UPDATE Equipment
						SET DateSold = @DateSold
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @IsTemplate IS NOT NULL AND @IsTemplate <> @IsTemplateParam
					BEGIN
						UPDATE Equipment
						SET IsTemplate = @IsTemplate
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @Active IS NOT NULL AND @Active <> @ActiveParam
					BEGIN
						UPDATE Equipment
						SET Active = @Active
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @Deletable IS NOT NULL AND @Deletable <> @DeletableParam
					BEGIN
						UPDATE Equipment
						SET Deletable = @Deletable
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @EnterUserStr IS NOT NULL AND @EnterUserStr <> @EnterUserStrParam
					BEGIN
						UPDATE Equipment
						SET EnterUserStr = @EnterUserStr
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @EnterDateTime IS NOT NULL AND @EnterDateTime <> @EnterDateTimeParam
					BEGIN
						UPDATE Equipment
						SET EnterDateTime = @EnterDateTime
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @EditUserStr IS NOT NULL AND @EditUserStr <> @EditUserStrParam
					BEGIN
						UPDATE Equipment
						SET EditUserStr = @EditUserStr
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @EditDateTime IS NOT NULL AND @EditDateTime <> @EditDateTimeParam
					BEGIN
						UPDATE Equipment
						SET EditDateTime = @EditDateTime
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					--IF @TimeStamp IS NOT NULL AND @TimeStamp <> @TimeStampParam
					--BEGIN
					--	UPDATE Equipment
					--	SET [TimeStamp] = @TimeStamp
					--	WHERE EquipmentID = @EquipmentID
					--
					--	SET @Updated = 1
					--END

					IF @OwnerType IS NOT NULL AND @OwnerType <> @OwnerTypeParam
					BEGIN
						UPDATE Equipment
						SET OwnerType = @OwnerType
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @ProjectNumber IS NOT NULL AND @ProjectNumber <> @ProjectNumberParam
					BEGIN
						UPDATE Equipment
						SET ProjectNumber = @ProjectNumber
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @OriginalHour IS NOT NULL AND @OriginalHour <> @OriginalHourParam
					BEGIN
						UPDATE Equipment
						SET OriginalHour = @OriginalHour
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @RentDateAvailable IS NOT NULL AND @RentDateAvailable <> @RentDateAvailableParam
					BEGIN
						UPDATE Equipment
						SET RentDateAvailable = @RentDateAvailable
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SaleDateAvailable IS NOT NULL AND @SaleDateAvailable <> @SaleDateAvailableParam
					BEGIN
						UPDATE Equipment
						SET SaleDateAvailable = @SaleDateAvailable
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @LocationStatus IS NOT NULL AND @LocationStatus <> @LocationStatusParam
					BEGIN
						UPDATE Equipment
						SET LocationStatus = @LocationStatus
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @PackageNumber IS NOT NULL AND @PackageNumber <> @PackageNumberParam
					BEGIN
						UPDATE Equipment
						SET PackageNumber = @PackageNumber
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @EnterpriseBuyerID IS NOT NULL AND @EnterpriseBuyerID <> @EnterpriseBuyerIDParam
					BEGIN
						UPDATE Equipment
						SET EnterpriseBuyerID = @EnterpriseBuyerID
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @AcqAttachmentPrice IS NOT NULL AND @AcqAttachmentPrice <> @AcqAttachmentPriceParam
					BEGIN
						UPDATE Equipment
						SET AcqAttachmentPrice = @AcqAttachmentPrice
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @AcqShippingCost IS NOT NULL AND @AcqShippingCost <> @AcqShippingCostParam
					BEGIN
						UPDATE Equipment
						SET AcqShippingCost = @AcqShippingCost
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @AddToFixedAsset IS NOT NULL AND @AddToFixedAsset <> @AddToFixedAssetParam
					BEGIN
						UPDATE Equipment
						SET AddToFixedAsset = @AddToFixedAsset
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @DateSentFixedAsset IS NOT NULL AND @DateSentFixedAsset <> @DateSentFixedAssetParam
					BEGIN
						UPDATE Equipment
						SET DateSentFixedAsset = @DateSentFixedAsset
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @OwnedByID IS NOT NULL AND @OwnedByID <> @OwnedByIDParam
					BEGIN
						UPDATE Equipment
						SET OwnedByID = @OwnedByID
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @LastAppraisalDate IS NOT NULL AND @LastAppraisalDate <> @LastAppraisalDateParam
					BEGIN
						UPDATE Equipment
						SET LastAppraisalDate = @LastAppraisalDate
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @MinMonthlyRentalRate IS NOT NULL AND @MinMonthlyRentalRate <> @MinMonthlyRentalRateParam
					BEGIN
						UPDATE Equipment
						SET MinMonthlyRentalRate = @MinMonthlyRentalRate
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @WarrantyExpDate IS NOT NULL AND @WarrantyExpDate <> @WarrantyExpDateParam
					BEGIN
						UPDATE Equipment
						SET WarrantyExpDate = @WarrantyExpDate
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @WarrantyExpClick IS NOT NULL AND @WarrantyExpClick <> @WarrantyExpClickParam
					BEGIN
						UPDATE Equipment
						SET WarrantyExpClick = @WarrantyExpClick
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @WarrantyServicer IS NOT NULL AND @WarrantyServicer <> @WarrantyServicerParam
					BEGIN
						UPDATE Equipment
						SET WarrantyServicer = @WarrantyServicer
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @PurchasedNew IS NOT NULL AND @PurchasedNew <> @PurchasedNewParam
					BEGIN
						UPDATE Equipment
						SET PurchasedNew = @PurchasedNew
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @OriginalPONumber IS NOT NULL AND @OriginalPONumber <> @OriginalPONumberParam
					BEGIN
						UPDATE Equipment
						SET OriginalPONumber = @OriginalPONumber
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @DepreciateAsset IS NOT NULL AND @DepreciateAsset <> @DepreciateAssetParam
					BEGIN
						UPDATE Equipment
						SET DepreciateAsset = @DepreciateAsset
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SoldPrice IS NOT NULL AND @SoldPrice <> @SoldPriceParam
					BEGIN
						UPDATE Equipment
						SET SoldPrice = @SoldPrice
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SoldToID IS NOT NULL AND @SoldToID <> @SoldToIDParam
					BEGIN
						UPDATE Equipment
						SET SoldToID = @SoldToID
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SoldInvoiceNumber IS NOT NULL AND @SoldInvoiceNumber <> @SoldInvoiceNumberParam
					BEGIN
						UPDATE Equipment
						SET SoldInvoiceNumber = @SoldInvoiceNumber
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @BrokerPrice IS NOT NULL AND @BrokerPrice <> @BrokerPriceParam
					BEGIN
						UPDATE Equipment
						SET BrokerPrice = @BrokerPrice
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @PublicViewSerialNum IS NOT NULL AND @PublicViewSerialNum <> @PublicViewSerialNumParam
					BEGIN
						UPDATE Equipment
						SET PublicViewSerialNum = @PublicViewSerialNum
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @EquipmentSize IS NOT NULL AND @EquipmentSize <> @EquipmentSizeParam
					BEGIN
						UPDATE Equipment
						SET EquipmentSize = @EquipmentSize
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @EquipmentSizeUnit IS NOT NULL AND @EquipmentSizeUnit <> @EquipmentSizeUnitParam
					BEGIN
						UPDATE Equipment
						SET EquipmentSizeUnit = @EquipmentSizeUnit
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @AttachmentDefaultPosition IS NOT NULL AND @AttachmentDefaultPosition <> @AttachmentDefaultPositionParam
					BEGIN
						UPDATE Equipment
						SET AttachmentDefaultPosition = @AttachmentDefaultPosition
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @Weight IS NOT NULL AND @Weight <> @WeightParam
					BEGIN
						UPDATE Equipment
						SET [Weight] = @Weight
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @Height IS NOT NULL AND @Height <> @HeightParam
					BEGIN
						UPDATE Equipment
						SET Height = @Height
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @Length IS NOT NULL AND @Length <> @LengthParam
					BEGIN
						UPDATE Equipment
						SET [Length] = @Length
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @Width IS NOT NULL AND @Width <> @WidthParam
					BEGIN
						UPDATE Equipment
						SET Width = @Width
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @LocationContactRelationshipID IS NOT NULL AND @LocationContactRelationshipID <> @LocationContactRelationshipIDParam
					BEGIN
						UPDATE Equipment
						SET LocationContactRelationshipID = @LocationContactRelationshipID
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @ServiceStatus IS NOT NULL AND @ServiceStatus <> @ServiceStatusParam
					BEGIN
						UPDATE Equipment
						SET ServiceStatus = @ServiceStatus
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @UnitNumber IS NOT NULL AND @UnitNumber <> @UnitNumberParam
					BEGIN
						UPDATE Equipment
						SET UnitNumber = @UnitNumber
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SMMPlate IS NOT NULL AND @SMMPlate <> @SMMPlateParam
					BEGIN
						UPDATE Equipment
						SET SMMPlate = @SMMPlate
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SMMTag IS NOT NULL AND @SMMTag <> @SMMTagParam
					BEGIN
						UPDATE Equipment
						SET SMMTag = @SMMTag
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SalesmanContactID IS NOT NULL AND @SalesmanContactID <> @SalesmanContactIDParam
					BEGIN
						UPDATE Equipment
						SET SalesmanContactID = @SalesmanContactID
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SmmTabExpiresOn IS NOT NULL AND @SmmTabExpiresOn <> @SmmTabExpiresOnParam
					BEGIN
						UPDATE Equipment
						SET SmmTabExpiresOn = @SmmTabExpiresOn
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SmmPlateExpiresOn IS NOT NULL AND @SmmPlateExpiresOn <> @SmmPlateExpiresOnParam
					BEGIN
						UPDATE Equipment
						SET SmmPlateExpiresOn = @SmmPlateExpiresOn
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @LicensingNotes IS NOT NULL AND @LicensingNotes <> @LicensingNotesParam
					BEGIN
						UPDATE Equipment
						SET LicensingNotes = @LicensingNotes
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SmmTabLastOne IS NOT NULL AND @SmmTabLastOne <> @SmmTabLastOneParam
					BEGIN
						UPDATE Equipment
						SET SmmTabLastOne = @SmmTabLastOne
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SmmPlateLastOne IS NOT NULL AND @SmmPlateLastOne <> @SmmPlateLastOneParam
					BEGIN
						UPDATE Equipment
						SET SmmPlateLastOne = @SmmPlateLastOne
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @EquipmentStatusNotes IS NOT NULL AND @EquipmentStatusNotes <> @EquipmentStatusNotesParam
					BEGIN
						UPDATE Equipment
						SET EquipmentStatusNotes = @EquipmentStatusNotes
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @EquipmentSize2 IS NOT NULL AND @EquipmentSize2 <> @EquipmentSize2Param
					BEGIN
						UPDATE Equipment
						SET EquipmentSize2 = @EquipmentSize2
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @SizeRangeType IS NOT NULL AND @SizeRangeType <> @SizeRangeTypeParam
					BEGIN
						UPDATE Equipment
						SET SizeRangeType = @SizeRangeType
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @CustomerName IS NOT NULL AND @CustomerName <> @CustomerNameParam
					BEGIN
						UPDATE Equipment
						SET CustomerName = @CustomerName
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @CustomerPhone IS NOT NULL AND @CustomerPhone <> @CustomerPhoneParam
					BEGIN
						UPDATE Equipment
						SET CustomerPhone = @CustomerPhone
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @EngArrNum IS NOT NULL AND @EngArrNum <> @EngArrNumParam
					BEGIN
						UPDATE Equipment
						SET EngArrNum = @EngArrNum
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @TranArrNum IS NOT NULL AND @TranArrNum <> @TranArrNumParam
					BEGIN
						UPDATE Equipment
						SET TranArrNum = @TranArrNum
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @MachArrNum IS NOT NULL AND @MachArrNum <> @MachArrNumParam
					BEGIN
						UPDATE Equipment
						SET MachArrNum = @MachArrNum
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @EngSerialNum IS NOT NULL AND @EngSerialNum <> @EngSerialNumParam
					BEGIN
						UPDATE Equipment
						SET EngSerialNum = @EngSerialNum
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @TranSerialNum IS NOT NULL AND @TranSerialNum <> @TranSerialNumParam
					BEGIN
						UPDATE Equipment
						SET TranSerialNum = @TranSerialNum
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @HasHours IS NOT NULL AND @HasHours <> @HasHoursParam
					BEGIN
						UPDATE Equipment
						SET HasHours = @HasHours
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @Generation IS NOT NULL AND @Generation <> @GenerationParam
					BEGIN
						UPDATE Equipment
						SET Generation = @Generation
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @AssetOwner IS NOT NULL AND @AssetOwner <> @AssetOwnerParam
					BEGIN
						UPDATE Equipment
						SET AssetOwner = @AssetOwner
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @AssetLife IS NOT NULL AND @AssetLife <> @AssetLifeParam
					BEGIN
						UPDATE Equipment
						SET AssetLife = @AssetLife
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @HasMiles IS NOT NULL AND @HasMiles <> @HasMilesParam
					BEGIN
						UPDATE Equipment
						SET HasMiles = @HasMiles
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @ExcludeFromForecast IS NOT NULL AND @ExcludeFromForecast <> @ExcludeFromForecastParam
					BEGIN
						UPDATE Equipment
						SET ExcludeFromForecast = @ExcludeFromForecast
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END

					IF @AuctionValue IS NOT NULL AND @AuctionValue <> @AuctionValueParam
					BEGIN
						UPDATE Equipment
						SET AuctionValue = @AuctionValue
						WHERE EquipmentID = @EquipmentID
					
						SET @Updated = 1
					END


				COMMIT TRANSACTION

				--Now we're auditing the equipment we just saved an update for.
				BEGIN TRANSACTION

				INSERT INTO [dbo].[EquipmentAudit]
           ([Deleted]
           ,[EquipmentID]
           ,[SerialNum]
           ,[PropertyTag]
           ,[MachineAttachedToID]
           ,[AttachmentPosition]
           ,[AttachmentDefaultPosition]
           ,[RentalStatus]
           ,[LocationStatus]
           ,[ServiceStatus]
           ,[ManufacturedYear]
           ,[InventoryMasterID]
           ,[AssignedDivisionID]
           ,[OwnedByID]
           ,[OwnerType]
           ,[Rerented]
           ,[CurrentLocationID]
           ,[LocationContactRelationshipID]
           ,[HasHours]
           ,[Hours]
           ,[HasMiles]
           ,[Miles]
           ,[ForRent]
           ,[ForSale]
           ,[PublicViewable]
           ,[PublicPriceViewable]
           ,[PublicViewSerialNum]
           ,[EquipmentDescriptor]
           ,[MarketingDescription]
           ,[Price]
           ,[MinPrice]
           ,[BrokerPrice]
           ,[MonthlyRentalRate]
           ,[MinMonthlyRentalRate]
           ,[InsuranceValue]
           ,[RentalPurchaseOptionPrice]
           ,[AuctionValue]
           ,[AppraiserID]
           ,[LastAppraisalDate]
           ,[OrderlyLiquidationValue]
           ,[DateAcquired]
           ,[SupplierID]
           ,[EnterpriseBuyerID]
           ,[OriginalHour]
           ,[PurchasedNew]
           ,[OriginalPONumber]
           ,[PackageNumber]
           ,[ProjectNumber]
           ,[RentDateAvailable]
           ,[SaleDateAvailable]
           ,[PurchasePrice]
           ,[AdditionalCapCost]
           ,[AcqAttachmentPrice]
           ,[AcqShippingCost]
           ,[DepreciateAsset]
           ,[Generation]
           ,[AssetOwner]
           ,[AssetLife]
           ,[AddToFixedAsset]
           ,[DateSentFixedAsset]
           ,[DateSold]
           ,[SoldPrice]
           ,[SoldToID]
           ,[CustomerName]
           ,[CustomerPhone]
           ,[SalesmanContactID]
           ,[SoldInvoiceNumber]
           ,[EquipmentStatusNotes]
           ,[ConditionID]
           ,[WarrantyExpDate]
           ,[WarrantyExpClick]
           ,[WarrantyServicer]
           ,[EquipmentSize]
           ,[EquipmentSizeUnit]
           ,[EquipmentSize2]
           ,[SizeRangeType]
           ,[Weight]
           ,[Height]
           ,[Length]
           ,[Width]
           ,[BarCode]
           ,[UnitNumber]
           ,[SMMPlate]
           ,[SMMTag]
           ,[SmmTabExpiresOn]
           ,[SmmPlateExpiresOn]
           ,[SmmTabLastOne]
           ,[SmmPlateLastOne]
           ,[LicensingNotes]
           ,[EngArrNum]
           ,[TranArrNum]
           ,[MachArrNum]
           ,[EngSerialNum]
           ,[TranSerialNum]
           ,[ExcludeFromForecast]
           ,[IsTemplate]
           ,[PictureCount]
           ,[SerialNumStripped]
           ,[PropertyTagStripped]
           ,[Active]
           ,[Deletable]
           ,[EnterUserStr]
           ,[EnterDateTime]
           ,[EditUserStr]
           ,[EditDateTime])
		   VALUES
		   (0
           ,@EquipmentID
           ,@SerialNum
           ,@PropertyTag
           ,@MachineAttachedToID
           ,@AttachmentPosition
           ,@AttachmentDefaultPosition
           ,@RentalStatus
           ,@LocationStatus
           ,@ServiceStatus
           ,@ManufacturedYear
           ,@InventoryMasterID
           ,@AssignedDivisionID
           ,@OwnedByID
           ,@OwnerType
           ,@Rerented
           ,@CurrentLocationID
           ,@LocationContactRelationshipID
           ,@HasHours
           ,@Hours
           ,@HasMiles
           ,@Miles
           ,@ForRent
           ,@ForSale
           ,@PublicViewable
           ,@PublicPriceViewable
           ,@PublicViewSerialNum
           ,@EquipmentDescriptor
           ,@MarketingDescription
           ,@Price
           ,@MinPrice
           ,@BrokerPrice
           ,@MonthlyRentalRate
           ,@MinMonthlyRentalRate
           ,@InsuranceValue
           ,@RentalPurchaseOptionPrice
           ,@AuctionValue
           ,@AppraiserID
           ,@LastAppraisalDate
           ,@OrderlyLiquidationValue
           ,@DateAcquired
           ,@SupplierID
           ,@EnterpriseBuyerID
           ,@OriginalHour
           ,@PurchasedNew
           ,@OriginalPONumber
           ,@PackageNumber
           ,@ProjectNumber
           ,@RentDateAvailable
           ,@SaleDateAvailable
           ,@PurchasePrice
           ,@AdditionalCapCost
           ,@AcqAttachmentPrice
           ,@AcqShippingCost
           ,@DepreciateAsset
           ,@Generation
           ,@AssetOwner
           ,@AssetLife
           ,@AddToFixedAsset
           ,@DateSentFixedAsset
           ,@DateSold
           ,@SoldPrice
           ,@SoldToID
           ,@CustomerName
           ,@CustomerPhone
           ,@SalesmanContactID
           ,@SoldInvoiceNumber
           ,@EquipmentStatusNotes
           ,@ConditionID
           ,@WarrantyExpDate
           ,@WarrantyExpClick
           ,@WarrantyServicer
           ,@EquipmentSize
           ,@EquipmentSizeUnit
           ,@EquipmentSize2
           ,@SizeRangeType
           ,@Weight
           ,@Height
           ,@Length
           ,@Width
           ,@BarCode
           ,@UnitNumber
           ,@SMMPlate
           ,@SMMTag
           ,@SmmTabExpiresOn
           ,@SmmPlateExpiresOn
           ,@SmmTabLastOne
           ,@SmmPlateLastOne
           ,@LicensingNotes
           ,@EngArrNum
           ,@TranArrNum
           ,@MachArrNum
           ,@EngSerialNum
           ,@TranSerialNum
           ,@ExcludeFromForecast
           ,@IsTemplate
           ,@PictureCount
           ,@SerialNumStripped
           ,@PropertyTagStripped
           ,@Active
           ,@Deletable
           ,@EnterUserStr
           ,@EnterDateTime
           ,@EditUserStr
           ,@EditDateTime)

			COMMIT TRANSACTION

			END TRY
			BEGIN CATCH
			---------------------Rollback if Error--------------------------
				IF @@TRANCOUNT > 0
				BEGIN
					ROLLBACK TRANSACTION
					SET @Updated = 0
				END
			END CATCH
			------------------------------------------End Transactional Block-------------------------------------------

		END

	RETURN @Updated;

END
