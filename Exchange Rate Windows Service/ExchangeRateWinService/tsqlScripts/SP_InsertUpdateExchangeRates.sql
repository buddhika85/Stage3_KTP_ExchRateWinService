USE [BCMY_Stock];

GO
SET QUOTED_IDENTIFIER ON
GO
-- ====================================================================================================================================================================================
-- Author:		Buddhika
-- Create date: 28 August 2015
-- Description:	<Inserts or Updates with exchange rates>
-- ====================================================================================================================================================================================
ALTER PROCEDURE SP_InserUpdateExchangeRates 
	-- input/output params
	@usd DECIMAL(10,4),
	@euro DECIMAL(10,4),	
	@insertEditStatus VARCHAR(1000) OUTPUT
AS
BEGIN

	--SET @insertEditStatus = '';
	
	-- checking whther we have a record for the current date
	DECLARE @todaysRecord DATE = (SELECT [dateER] AS todaysRecord FROM [dbo].[TblExchangeRate] WHERE [dateER] = CONVERT(DATE, GETDATE()));	
	IF @todaysRecord IS NOT NULL
	BEGIN
		-- update
		UPDATE [dbo].[TblExchangeRate] SET 
			[usdValue] = @usd, 
			[euroValue] = @euro, 
			[timeER] = CONVERT(TIME, GETDATE()) WHERE
			[dateER] = CONVERT(DATE, GETDATE(), 101);
		SET @insertEditStatus = 'UPDATED Exchange rates for ' + CONVERT(VARCHAR(500), GETDATE());
	END
	ELSE
	BEGIN
		-- insert
		INSERT INTO [dbo].[TblExchangeRate] VALUES (CONVERT(DATE, GETDATE(), 101), CONVERT(TIME, GETDATE()), @usd, @euro);
		SET @insertEditStatus = 'INSERTED Exchange rates for ' + CONVERT(VARCHAR(500), GETDATE());
	END


	
	--INSERT INTO [dbo].[TblExchangeRate] VALUES (CONVERT(DATE, GETDATE()), CONVERT(TIME, GETDATE()), 2.511, 1.511);
	--SELECT * FROM [TblExchangeRate]
	--TRUNCATE TABLE [TblExchangeRate]

END 
GO