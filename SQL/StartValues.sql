

use WineStore;  
GO

INSERT SHOP(ADDRESS,TELEPHON_NUMBER,Email,DESCRIPTION)
VALUES
('Davies St. Mayfair, London', '+4402072907870', 'drink@hedonism.co.uk', 'Multiple Award Winner');


INSERT WINE(PRODUCT_CODE, TITLE, VOLUME, PRICE, VINTAGE, AVAILABLE, TYPE, COUNTRY, COTEGORY, FOTO)
VALUES
('HED70879','Chateau Chalon Bourdy 1959', 62,488.60,1959,1,'White','France','Still Wines','/Images/default_drink_wine_white_teaser.png'),
('HED86977', 'Douro Superior Quinta do Crasto 1200cl 2012', 1200, 871.00, 2012, 2, 'Red', 'Portugal', 'Still Wines','/Images/default_drink_wine_red_teaser.png'),
('HED87094', 'Chateauneuf du Pape Vieux Telegraphe 600cl 2013', 600, 588.00, 2013, 1, 'Red', 'France', 'Still Wines','/Images/default_drink_wine_red_teaser.png'),
('HED87097', 'Gigondas Les Pallieres Terrasse de Diable 600cl 2013', 600, 312.00, 2013, 2, 'Red', 'France', 'Still Wines','/Images/default_drink_wine_red_teaser.png');




INSERT WINE(PRODUCT_CODE, TITLE, VOLUME, PRICE, VINTAGE, AVAILABLE, TYPE, COUNTRY, COTEGORY, FOTO)
VALUES
('HED89044', 'Yquem 1913', 75, 4610.00, 1913, 1, 'White', 'France', 'Sweet Wines', '/Images/HED89044.jpg'),
('HED52605', 'Climens 1937', 75, 888.00, 1937, 1, 'White', 'France', 'Sweet Wines', '/Images/HED52605.jpg'),
('HED1740', 'Dom Perignon 1982', 75, 525.00, 1982, 1, 'White', 'France', 'Sweet Wines', '/Images/HED1740.jpg')


INSERT SHOP_FAQS
VALUES
('What are your opening hours?',
'From Monday – Saturday, Hedonism Wines opens at 10am & closes at 9pm. On Sundays, the store opens at 12pm & closes at 6pm.',
2),
('Do you sell gift cards?',
'Hedonism Wines gift cards can be issued at any value & are able to be redeemed against wines, spirits, accessories or used at our Enomatic tasting machines.',
2),
('Do you organise events?',
'We offer a bespoke events service which includes the use of our downstairs tasting area, optional catering and sommelier service. If you are interested in 
holding an event at Hedonism Wines, please call the store or speak to a member of staff for more information or visit the private tastings page to make an enquirey.',
2),
('Do you have any chilled wine at the store?',
'We always have a selection of chilled white wines, sake, and champagnes available at our Davies Street store.',
2),
('What should I do if the wine I purchase is corked or out of condition?',
'If you have a bottle that is not quite tasting as it should please contact the shop management team by calling +44 (0) 207 290 7871 or getting in touch via the contact us page. 
For faulty wines returned within 30 days we will offer you an exchange, a store credit or a refund. Please note that we won’t be able to proceed without the wine being returned.',
2),
('Do you offer a wine storage service?',
'While our Davies Street store is temperature and humidity controlled, we are currently unable to offer a storage service for customers.',
2),
('Do you offer En Primeur services?',
'We do not currently offer En Primeur services for customers.',
2),
('Are there any wines available to taste at the store?',
'Our Davies Street store is currently equipped with state-of-the-art Enomatic tasting machines. These allow us to offer a constantly 
changing range of around 50 wines for customers to sample. Check out our wine tasting page for more information',
2),
('You stock many fine and rare wines, are you able to provide provenance?',
'We are able to provide full provenance for our wines. If you have a question about a particular wine, please feel free to speak to
 a member of the Hedonism Wines team either over the phone or in person at our Davies Street store. Alternatively, please visit the 
 contact us page and get in touch via email.',
2),
('Do have any foreign language speakers among your staff?',
'Our staff members speak over half a dozen languages between them including English, German, Spanish, Russian, Mandarin, French, 
Portuguese & Italian. Check out the meet the team page for more information.',
2),
('Do you buy wine from private individuals?',
'If you are a private individual with a collection of rare wines or spirits that you are looking to sell, please feel free to get 
in touch with a member of the Hedonism Wines team by either calling or visiting our Davies Street store. Alternatively,
 please visit the contact us page and get in touch with the buying team via email.',
2),
('Who is responsible for the wonderful images on your website?',
'All of the wonderful photographs of bottles, glassware & accessories are taken by product photographer Ilya Krylov (http://www.ikrylov.com/).',
2)




CREATE PROCEDURE sp_filling_out
AS
BEGIN

DECLARE @number INT, @LOGIN INT, @PASSWORD INT
SET @LOGIN = 1;
SET @PASSWORD =1;
SET @number = 0;
 
WHILE @number < 100000
    BEGIN
       
EXEC sp_InsertUser @LOGIN, @PASSWORD

SET @LOGIN = @LOGIN +1;
SET @PASSWORD = @PASSWORD +1;
SET @number = @number +1;
    END;

END

EXEC sp_filling_out;


SELECT ID FROM "USER"

SELECT ID FROM "SESSION"

