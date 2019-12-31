
use WineStore;  

GO
drop procedure sp_UserVerification;
drop procedure sp_UserLogOut;
drop procedure sp_InsertUser;
drop procedure sp_SelectALLWines;
drop procedure sp_SelectAllWinesSortedPrice;
drop procedure sp_SelectAllWinesSortedName;
drop procedure sp_SelectAllWinesSortedVintage;
drop procedure sp_SelectWines;
drop procedure sp_AddWines;
drop procedure sp_AlterWines;
drop procedure sp_AddWinesToBasket;
drop procedure sp_SelectALLWinesToBasket;
drop procedure sp_RemoveWineFromBasket;
drop procedure sp_CreateOrder;
drop procedure sp_InsertOrderInformation;
drop procedure sp_SelectALLOrders;
drop procedure sp_ImportWineFromXML;
drop procedure sp_ExportWineToXML;
drop procedure sp_SelectShippingInformation;
drop procedure sp_InsertToAboutUser;
drop procedure sp_UpdateShippingInformation;
drop procedure sp_SelectPaymentInformation;
drop procedure sp_InsertToCard;
drop procedure sp_UpdatePaymentInformation;
drop procedure sp_SelectAllFaqs;
drop procedure sp_SelectFromFaqs;
drop procedure sp_SelectShopInfo;


GO
CREATE PROCEDURE sp_UserVerification
@login nvarchar(50),
@password nvarchar(50),
@result int out
AS
BEGIN 
update "SESSION"
set ACTIVE =0


SELECT @result = COUNT(*) FROM "USER" WHERE LOGIN = @login and PASSWORD = @password;

if @result = 1
begin
update "SESSION"
set ACTIVE =1
WHERE "SESSION".LOGIN = @login;
end

END





GO
CREATE PROCEDURE sp_UserLogOut
@result int out
AS
BEGIN 
declare @login nvarchar(50)
SELECT @result = COUNT(*) FROM "SESSION" WHERE ACTIVE=1;
select @login = "LOGIN" FROM "SESSION" WHERE ACTIVE=1;


if @result = 1
begin
update "SESSION"
set ACTIVE =0
WHERE "LOGIN"=@login;

end

END




GO
CREATE PROCEDURE sp_InsertUser
@login nvarchar(30),
@password nvarchar(30)
AS
BEGIN
update "SESSION"
set ACTIVE =0

INSERT "SESSION"
VALUES
(@login,0)

INSERT "USER"(LOGIN,PASSWORD,IS_ACTIVE)
VALUES(@login,@password,(SELECT "SESSION".ID FROM "SESSION" WHERE "SESSION"."LOGIN" = @login))
END



GO
CREATE PROCEDURE sp_SelectALLWines
AS
BEGIN
SELECT * FROM WINE;
END





GO
CREATE PROCEDURE sp_SelectAllWinesSortedPrice 
@PriceSort int
AS
BEGIN
IF @PriceSort <> 0 
SELECT * FROM WINE
order by price;
else
SELECT * FROM WINE
order by price DESC;
END



GO
CREATE PROCEDURE sp_SelectAllWinesSortedName
@NameSort int
AS
BEGIN
IF @NameSort <> 0 
SELECT * FROM WINE
order by title;
else
SELECT * FROM WINE
order by title DESC;
END



GO
CREATE PROCEDURE sp_SelectAllWinesSortedVintage
@VintageSort int
AS
BEGIN
IF @VintageSort <> 0 
SELECT * FROM WINE
order by vintage;
else
SELECT * FROM WINE
order by vintage DESC;
END




GO
CREATE PROCEDURE sp_SelectWines
@Title nvarchar(30),
@Category nvarchar(30), 
@Type nvarchar(30),
@Country nvarchar(30),
@Vintage int,
@PriceMin int,
@PriceMax int,
@Volume int
AS
BEGIN
SELECT * FROM WINE
 WHERE CONTAINS(TITLE, @Title) and COTEGORY =@Category and TYPE = @Type and COUNTRY = @Country and VINTAGE = Vintage and PRICE <= @PriceMax and PRICE >= @PriceMin and VOLUME = @Volume and AVAILABLE>0;
END




GO
CREATE PROCEDURE sp_AddWines
@Code nvarchar(15), 
@Name nvarchar(200), 
@Category nvarchar(30), 
@Type nvarchar(30),
@Country nvarchar(30),
@Vintage int,
@Price decimal(10,2),
@Available int,
@Volume int,
@Foto nvarchar(100)
AS
BEGIN
INSERT WINE(PRODUCT_CODE, TITLE, VOLUME, PRICE, VINTAGE, AVAILABLE, TYPE, COUNTRY, COTEGORY, FOTO)
VALUES
(@Code,@Name, @Volume, @Price, @Vintage, @Available, @Type, @Country, @Category, @Foto);
END




GO
CREATE PROCEDURE sp_AlterWines 
@Code nvarchar(15), 
@Price decimal(10,2),
@Available int
AS
BEGIN
if @Price !=0
begin
update WINE
set PRICE = @Price
where PRODUCT_CODE=@Code;
end

if @Available !=0
begin
update WINE
set AVAILABLE = @Available
where PRODUCT_CODE=@Code;
end
END



GO
CREATE PROCEDURE sp_AddWinesToBasket
@ID int
AS
BEGIN
INSERT BASKET
VALUES
(
(SELECT ID FROM "USER" WHERE "USER".IS_ACTIVE=(SELECT "SESSION".ID FROM "SESSION" WHERE "SESSION".ACTIVE=1 AND "SESSION"."LOGIN" = "USER"."LOGIN")),
(SELECT ID FROM WINE WHERE WINE.ID=@ID)
)

END



GO
CREATE PROCEDURE sp_SelectALLWinesToBasket
AS
BEGIN
SELECT WINE.PRODUCT_CODE, WINE.TITLE, WINE.VOLUME, WINE.PRICE, WINE.FOTO 
FROM BASKET
JOIN WINE ON WINE.ID = BASKET.WINE
JOIN "USER" ON "USER".ID = BASKET."USER" AND "USER".IS_ACTIVE = (SELECT "SESSION".ID FROM "SESSION" WHERE "SESSION".ACTIVE=1 AND "SESSION"."LOGIN" = "USER"."LOGIN")
END

GO
CREATE PROCEDURE sp_RemoveWineFromBasket
@Code nvarchar(15)
AS
BEGIN
DELETE BASKET
WHERE "USER" = (SELECT "USER".ID FROM "USER" WHERE "USER".IS_ACTIVE = (SELECT "SESSION".ID FROM "SESSION" WHERE "SESSION".ACTIVE=1 AND "SESSION"."LOGIN" = "USER"."LOGIN"))
AND WINE = (SELECT WINE.ID FROM WINE WHERE PRODUCT_CODE = @Code)
END


GO
CREATE PROCEDURE sp_CreateOrder
@price int,
@count int,
@FirstName nvarchar(50),
@LastName nvarchar(50),
@StreetAddress nvarchar(50),
@City nvarchar(50),
@PhoneNumber nvarchar(50),
@CardNumber int,
@ExpirationDate nvarchar(5),
@CVC int
AS
BEGIN

DECLARE @date DATETIME = GETDATE();
DECLARE @CURSOR CURSOR;
DECLARE @ID int;

INSERT ORDER_HISTORY("USER",DATE,ProductCount,Price)
VALUES
(
(SELECT "USER".ID
FROM "USER"
WHERE "LOGIN" = (SELECT "SESSION"."LOGIN" FROM "SESSION" WHERE "SESSION".ACTIVE=1 )
),
@date,
@count,
@price

)

SET @CURSOR = CURSOR SCROLL
FOR
SELECT WINE.ID
FROM BASKET
JOIN WINE ON WINE.ID = BASKET.WINE
JOIN "USER" ON "USER".ID = BASKET."USER" AND "USER".IS_ACTIVE = (SELECT "SESSION".ID FROM "SESSION" WHERE "SESSION".ACTIVE=1 AND "SESSION"."LOGIN" = "USER"."LOGIN")

 OPEN @CURSOR

 FETCH NEXT FROM @CURSOR INTO @ID

 WHILE @@FETCH_STATUS = 0
 BEGIN

 INSERT "ORDER"
VALUES
(
(
SELECT ORDER_HISTORY.NUMBER
FROM ORDER_HISTORY
JOIN "USER" ON "USER".ID = ORDER_HISTORY."USER" 
WHERE ORDER_HISTORY.DATE = @date
),
(
@ID
)
)

DELETE BASKET
WHERE WINE = @ID

FETCH NEXT FROM @CURSOR INTO @ID
 END

 CLOSE @CURSOR

   EXEC sp_InsertOrderInformation @FirstName, @LastName, @StreetAddress, @City, @PhoneNumber,@CardNumber, @ExpirationDate, @CVC

 update ORDER_HISTORY
set Info = (SELECT TOP 1 ORDER_INFORMATION.ID FROM ORDER_INFORMATION ORDER BY ID desc)
WHERE ORDER_HISTORY."USER" = (SELECT "USER".ID FROM "USER" WHERE "USER".IS_ACTIVE=(SELECT "SESSION".ID FROM "SESSION" WHERE "SESSION".ACTIVE=1 )) AND
ORDER_HISTORY.NUMBER = (SELECT TOP 1 ORDER_HISTORY.NUMBER FROM ORDER_HISTORY ORDER BY NUMBER desc)

END



GO
CREATE PROCEDURE sp_InsertOrderInformation
@FirstName nvarchar(50),
@LastName nvarchar(50),
@StreetAddress nvarchar(50),
@City nvarchar(50),
@PhoneNumber nvarchar(50),
@CardNumber int,
@ExpirationDate nvarchar(5),
@CVC int
AS
BEGIN
INSERT INTO ORDER_INFORMATION
VALUES
(@FirstName,@LastName,@StreetAddress,@City,@PhoneNumber,@CardNumber,@ExpirationDate,@CVC);
END



GO
CREATE PROCEDURE sp_SelectALLOrders
AS
BEGIN
SELECT DATE, ProductCount,Price 
FROM ORDER_HISTORY
JOIN "USER" ON "USER".ID = ORDER_HISTORY."USER" AND "USER".IS_ACTIVE=(SELECT "SESSION".ID FROM "SESSION" WHERE "SESSION".ACTIVE=1 AND "SESSION"."LOGIN" = "USER"."LOGIN")
END




GO
CREATE PROCEDURE sp_ImportWineFromXML	
	AS BEGIN
Declare @xml XML

Select  @xml  = 
CONVERT(XML,bulkcolumn,2)  FROM OPENROWSET(BULK 'D:\3 йспя\йо\WineStore\XML\Wine.xml', SINGLE_BLOB) AS X

SET ARITHABORT ON

Insert into Wine
        (
           PRODUCT_CODE,TITLE,VOLUME,PRICE,VINTAGE,AVAILABLE,TYPE,COUNTRY,COTEGORY,FOTO
        )

    Select 
        P.value('PRODUCT_CODE[1]','nvarchar(15)') AS PRODUCT_CODE,
        P.value('TITLE[1]','nvarchar(200)') AS TITLE,
	    P.value('VOLUME[1]','int') AS VOLUME,
        P.value('PRICE[1]','decimal(10,2)') AS PRICE,
        P.value('VINTAGE[1]','int') AS VINTAGE,
        P.value('AVAILABLE[1]','int') AS AVAILABLE,
        P.value('TYPE[1]','nvarchar(20)') AS TYPE,
        P.value('COUNTRY[1]','nvarchar(30)') AS COUNTRY,
		 P.value('COTEGORY[1]','nvarchar(30)') AS COTEGORY,
        P.value('FOTO[1]','nvarchar(100)') AS FOTO
    From @xml.nodes('/WineData') PropertyFeed(P)
end;



GO
CREATE PROCEDURE sp_ExportWineToXML	
 @xmlm  XML out
as
begin
SET @xmlm = ( Select * FROM Wine 
for xml path('Wine'), root('WineData'))
end



GO
CREATE PROCEDURE sp_SelectShippingInformation
AS
BEGIN
select FIRST_NAME, LAST_NAME, STREET_ADDRESS, CITY, PHONE_NUMBER
from ABOUT_USER
join "USER" ON "USER".ABOUT = ABOUT_USER.ID AND "USER".IS_ACTIVE=(SELECT "SESSION".ID FROM "SESSION" WHERE "SESSION".ACTIVE=1 AND "SESSION"."LOGIN" = "USER"."LOGIN")
END


GO
CREATE PROCEDURE sp_UpdateShippingInformation
@Flag int,
@FirstName nvarchar(50),
@LastName nvarchar(50),
@StreetAddress nvarchar(50),
@City nvarchar(50),
@PhoneNumber nvarchar(50)
AS
BEGIN

if @Flag != 0
begin

update About_user
set
 FIRST_NAME= @FirstName ,
LAST_NAME= @LastName,
STREET_ADDRESS = @StreetAddress,
CITY= @City ,
PHONE_NUMBER= @PhoneNumber
WHERE ABOUT_USER.ID=(SELECT "USER".ABOUT FROM "USER" WHERE "USER".IS_ACTIVE=(SELECT "SESSION".ID FROM "SESSION" WHERE "SESSION".ACTIVE=1 ))

end
ELSE
BEGIN

EXEC sp_InsertToAboutUser @FirstName, @LastName, @StreetAddress, @City, @PhoneNumber;

update "USER"
set ABOUT = (SELECT TOP 1 ABOUT_USER.ID FROM ABOUT_USER ORDER BY ID desc)
WHERE "USER".IS_ACTIVE=(SELECT "SESSION".ID FROM "SESSION" WHERE "SESSION".ACTIVE=1 )

END

END



GO
CREATE PROCEDURE sp_InsertToAboutUser
@FirstName nvarchar(50),
@LastName nvarchar(50),
@StreetAddress nvarchar(50),
@City nvarchar(50),
@PhoneNumber nvarchar(50)
AS
BEGIN
INSERT INTO About_user(FIRST_NAME, LAST_NAME, STREET_ADDRESS, CITY, PHONE_NUMBER)
VALUES(@FirstName, @LastName, @StreetAddress, @City, @PhoneNumber);
END



GO
CREATE PROCEDURE sp_SelectPaymentInformation
AS
BEGIN
select CARD_NUMBER, DATE, CVC 
from "CARD"
JOIN ABOUT_USER ON ABOUT_USER.CARD_INFO = "CARD".ID
join "USER" ON "USER".ABOUT = ABOUT_USER.ID AND "USER".IS_ACTIVE=(SELECT "SESSION".ID FROM "SESSION" WHERE "SESSION".ACTIVE=1 AND "SESSION"."LOGIN" = "USER"."LOGIN")
END




GO
CREATE PROCEDURE sp_UpdatePaymentInformation
@Flag int,
@CardNumber int,
@ExpirationDate nvarchar(5),
@CVC int
AS
BEGIN

if @Flag != 0
begin

update "CARD"
set
 CARD_NUMBER = @CardNumber,
"CARD".DATE = @ExpirationDate,
CVC = @CVC
WHERE "CARD".ID =( SELECT ABOUT_USER.CARD_INFO FROM ABOUT_USER WHERE ABOUT_USER.ID=(SELECT "USER".ABOUT FROM "USER" WHERE "USER".IS_ACTIVE=(SELECT "SESSION".ID FROM "SESSION" WHERE "SESSION".ACTIVE=1 )))

end
ELSE
BEGIN

EXEC sp_InsertToCard  @CardNumber, @ExpirationDate, @CVC;

update "ABOUT_USER"
set CARD_INFO = (SELECT TOP 1 "CARD".ID FROM "CARD" ORDER BY ID desc)
WHERE ABOUT_USER.ID=(SELECT "USER".ABOUT FROM "USER" WHERE "USER".IS_ACTIVE=(SELECT "SESSION".ID FROM "SESSION" WHERE "SESSION".ACTIVE=1 ))

END

END




GO
CREATE PROCEDURE sp_InsertToCard
@CardNumber int,
@ExpirationDate nvarchar(5),
@CVC int
AS
BEGIN
INSERT INTO "CARD"(CARD_NUMBER, DATE, CVC)
VALUES(@CardNumber, @ExpirationDate, @CVC);
END



GO
CREATE PROCEDURE sp_SelectAllFaqs
AS
BEGIN
SELECT question, answer
FROM SHOP_FAQS 
END

GO
CREATE PROCEDURE sp_SelectShopInfo
AS
BEGIN
SELECT ADDRESS, TELEPHON_NUMBER, Email
FROM SHOP
END




GO
CREATE PROCEDURE sp_SelectFromFaqs
@Text nvarchar(400)
AS
BEGIN
SELECT question, answer
FROM SHOP_FAQS 
WHERE  FREETEXT((question, answer), @Text) 
END



















