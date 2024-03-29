USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetMyClients]    Script Date: 11/20/2015 10:43:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [m2].[GetMyClients]
@userContactID nvarchar(20)
AS

SELECT DISTINCT 
      ctc.FirstName
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
FROM [Contract] ctr
Join Contact ctc on (ctr.CustomerContactID=ctc.ContactID)
   where SalesmanContactID=@userContactID

grant execute on m2.GetMyClients to public
