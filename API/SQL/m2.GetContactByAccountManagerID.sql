USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetMyClients]    Script Date: 3/6/2017 9:57:29 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[GetContactsByAccountManager]
@accountManagerID int
AS

IF @accountManagerID != 0
BEGIN
	SELECT DISTINCT
		  ctc.ContactID 
		  ,ctc.FirstName
		 ,ctc.LastName
		 ,ctc.JobTitle
		 ,ctc.BusinessPhone
		 ,ctc.BusinessFax
		 ,ctc.Email
		 ,ctc.Email2
		 ,ctc.Pager
		 ,ctc.Email
		 ,ctc.OtherPhone
		 ,ctc.OtherFax
	FROM Contact ctc
	where AccountManagerID=@accountManagerID
END


