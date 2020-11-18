USE [Mach1]
GO
/****** Object:  StoredProcedure [m2].[GetCustomers]    Script Date: 9/29/2016 3:05:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [m2].[GetCustomers] 
	@ContactID INT = -1,
	@ContactNTLogin NVARCHAR(MAX) = NULL,
	@All BIT = 0,
	@SearchString NVARCHAR(MAX) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TempContractStore TABLE (ContractIDStore INT,CompanyContactID INT,ContactRelationshipID INT, ContactID INT, FirstName NVARCHAR(50), LastName NVARCHAR(50), CompanyName NVARCHAR(120), NumOpenContracts INT, NumMachinesOnRent INT)
	DECLARE @DistinctCustomers TABLE (ContactID INT, FirstName NVARCHAR(50), LastName NVARCHAR(50), CompanyName NVARCHAR(120), NumOpenContracts INT, NumMachinesOnRent INT)
	
	;WITH CTE AS (
		SELECT DISTINCT
		rownum = ROW_NUMBER() OVER (ORDER BY con.ContractID ASC),
		ctact.FirstName, ctact.LastName, ctRel.ParentContactID, ctRel.ContactRelationshipID,
		con.ContractID, ctact.ContactID
		FROM [Mach1].[dbo].[Contract] con
		INNER JOIN ContactRelationship ctRel ON ctRel.ContactRelationshipID = con.CustomerContactID
		INNER JOIN Contact ctact ON ctact.ContactID = ctRel.ChildContactID
		INNER JOIN ContractDtl cdtl ON cdtl.ContractID = con.ContractID
		WHERE con.ContractStatus LIKE CASE @All
			WHEN 1 THEN con.ContractStatus
			WHEN 0 THEN 'Open'
			END
		AND ctact.ContactID = CASE
			WHEN @ContactID >= 0 THEN @ContactID
			WHEN @ContactID < 0 THEN ctact.ContactID
			END
		AND ctRel.Active = 1 AND ctact.Active = 1 AND con.Active = 1
		AND ctact.FirstName NOT LIKE '{Default}' AND ctact.LastName NOT LIKE '{Individual}'
		AND con.SalesmanContactID = dbo.getContactIDByNTLogin(@ContactNTLogin, con.SalesmanContactID)


		Group By con.ContractID, ctact.ContactID, ctact.FirstName, ctact.LastName, ctRel.ParentContactID, ctRel.ContactRelationshipID
		)
		--We need to first get the sum of the contracts that are open belonging to a given "Contact"


		INSERT INTO @TempContractStore
		SELECT commonTE.ContractID, commonTE.ParentContactID, commonTE.ContactRelationshipID, commonTE.ContactID, commonTE.FirstName, commonTE.LastName, NULL, NULL, NULL
		FROM CTE commonTE

		--Now we need to remove all duplicate company names and users
		--If first and last name, and company name are equal.  We need to assume there's a duplicate contact and include them.  We don't want 2 records.

		UPDATE @TempContractStore SET NumOpenContracts = (SELECT COUNT(ContractID) FROM [Contract] WHERE CustomerContactID = ContactRelationshipID AND ContractStatus LIKE 'Open');

		UPDATE @TempContractStore SET NumMachinesOnRent = CASE WHEN NumOpenContracts > 0 THEN (SELECT COUNT(ContractDtlId) FROM ContractDtl WHERE ContractID = ContractIDStore AND ActualEndDate IS NULL)
				WHEN NumOpenContracts <= 0 THEN 0
				END,
		CompanyName = (SELECT CompanyName FROM Contact WHERE ContactID = CompanyContactID)

		INSERT INTO @DistinctCustomers
		SELECT DISTINCT ContactID, NULL, NULL, NULL, NULL, NULL FROM @TempContractStore

		UPDATE dc
		SET dc.FirstName = (SELECT TOP 1 tcs.FirstName FROM @TempContractStore tcs WHERE tcs.ContactID = dc.ContactID),
		dc.LastName = (SELECT TOP 1 tcs.LastName FROM @TempContractStore tcs WHERE tcs.ContactID = dc.ContactID),
		dc.CompanyName = (SELECT TOP 1 tcs.CompanyName FROM @TempContractStore tcs WHERE tcs.ContactID = dc.ContactID),
		dc.NumOpenContracts = (SELECT TOP 1 tcs.NumOpenContracts FROM @TempContractStore tcs WHERE tcs.ContactID = dc.ContactID),
		dc.NumMachinesOnRent = (SELECT SUM(tcs.NumMachinesOnRent) FROM @TempContractStore tcs WHERE tcs.ContactID = dc.ContactID)
		FROM @DistinctCustomers dc

		SELECT FirstName as 'firstName', LastName as 'lastName', CompanyName as 'companyName',
		 NumOpenContracts as 'numOpenContracts', NumMachinesOnRent as 'numMachinesOnRent' from @DistinctCustomers ORDER BY LastName, FirstName

		

END
