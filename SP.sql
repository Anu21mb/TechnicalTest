-- =============================================     
-- Author  : Anu M  
-- Create date : 19-Jan-2024      
-- Description : To get customer recent orders.
--=================================================
-- exec CUST_GetCustomerOrders @user='bob@aol.com',@customerId='C34454'
-- =============================================  
CREATE OR ALTER PROC CUST_GetCustomerOrders(	
	@user VARCHAR(50)
	,@customerId VARCHAR(6)
)
AS
BEGIN
	
	IF EXISTS(SELECT 1 FROM CUSTOMERS WHERE EMAIL=@user AND CUSTOMERID=@customerId)
	BEGIN
		BEGIN TRY
			BEGIN TRAN

			select [FIRSTNAME] AS FirstName
					,[LASTNAME] AS LastName
			FROM CUSTOMERS 
			WHERE EMAIL LIKE @user AND CUSTOMERID LIKE @customerId;

			select TOP 1 
				ORDERID AS OrderNumber,
				ORDERDATE AS OrderDate,
				C.HOUSENO+' '+C.STREET +','+C.TOWN+','+C.POSTCODE AS DeliveryAddress,
				DELIVERYEXPECTED AS DeliveryExpected
			FROM ORDERS O
			LEFT JOIN CUSTOMERS C ON O.CUSTOMERID = C.CUSTOMERID
			WHERE O.CUSTOMERID LIKE @customerId
			ORDER BY ORDERDATE ASC			

			DECLARE @OrderId INT = NULL,@ContainsGift BIT = 0;
			select TOP 1 @OrderId = ORDERID,@ContainsGift= CONTAINSGIFT 
			FROM ORDERS O WHERE O.CUSTOMERID LIKE @customerId ORDER BY ORDERDATE ASC

			select 
				IIF(@ContainsGift=1,'Gift',PRODUCTNAME) AS product,
				QUANTITY AS quantity,
				PRICE AS priceEach
			FROM OrderItems OI
			LEFT JOIN PRODUCTS D ON OI.PRODUCTID = D.PRODUCTID
			WHERE OI.ORDERID = @OrderId

			COMMIT TRAN
		END TRY
		BEGIN CATCH
			ROLLBACK TRAN

			DECLARE @ERRMSG VARCHAR(500)=ERROR_MESSAGE()

			RAISERROR(@ERRMSG,16,1)
		END CATCH
	END
	ELSE
	BEGIN
		RAISERROR('User not found',16,1)
		RETURN;
	END
	
END
GO
