USE [Mach1]
GO

/****** Object:  UserDefinedTableType [m2].[SearchResults]    Script Date: 11/24/2015 12:55:10 PM ******/
CREATE TYPE [m2].[SearchResults] AS TABLE(
	[EquipmentID] [int] NOT NULL,
	[SerialNum] [nvarchar](50) NOT NULL,
	[ForRent] [bit] NOT NULL,
	[ForSale] [bit] NOT NULL,
	[PublicViewable] [bit] NOT NULL,
	[PublicPriceViewable] [bit] NOT NULL,
	[Price] [money] NULL,
	[MinPrice] [money] NULL,
	[MonthlyRentalRate] [money] NULL,
	[InsuranceValue] [money] NULL,
	[PropertyTag] [nvarchar](50) NOT NULL,
	[RentalStatus] [nvarchar](24) NOT NULL,
	[OwnerType] [nvarchar](15) NOT NULL,
	[LocationStatus] [nvarchar](50) NOT NULL,
	[ServiceStatus] [nvarchar](20) NOT NULL,
	[yearManufactured] [int] NULL,
	[hours] [int] NULL,
	[Miles] [int] NULL,
	[LastAppraisalDate] [datetime] NULL,
	[DateAcquired] [datetime] NOT NULL,
	[PurchasePrice] [money] NULL,
	[DateSold] [datetime] NULL,
	[SoldPrice] [money] NULL,
	[ModelNum] [nvarchar](50) NOT NULL,
	[Height] [nvarchar](50) NULL,
	[Length] [nvarchar](50) NULL,
	[Width] [nvarchar](50) NULL,
	[ManufacturerName] [nvarchar](120) NULL,
	[category] [nvarchar](50) NOT NULL,
	[division] [nvarchar](12) NOT NULL,
	[type] [nvarchar](12) NOT NULL,
	[City] [nvarchar](64) NOT NULL,
	[StateCode] [nvarchar](100) NOT NULL,
	[CountryName] [nvarchar](64) NOT NULL,
	[Address1] [nvarchar](60) NOT NULL,
	[PostalCode] [nvarchar](16) NOT NULL,
	[PositionLatitude] [decimal](8, 4) NULL,
	[PositionLongitude] [decimal](8, 4) NULL,
	[EquipmentSummaryDate] [datetime] NULL,
	[filename] [nvarchar](50) NULL,
	[YardName] [nvarchar](60) NULL,
	[RPOPRice] [money] NULL,
	[internationalRentalRate] [numeric](23, 6) NULL,
	[attachmentType] [nvarchar](50) NULL
)
GO

