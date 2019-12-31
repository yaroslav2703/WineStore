

select FULLTEXTSERVICEPROPERTY('isfulltextinstalled');

USE WineStore;  
GO  
CREATE FULLTEXT CATALOG WineCatalog;  

GO
CREATE UNIQUE INDEX ui_ShopFaqs ON SHOP_FAQS(ID); 


GO  
CREATE FULLTEXT INDEX ON SHOP_FAQS  ( question    Language 2057 , answer   Language 2057 )                
KEY INDEX ui_ShopFaqs ON WineCatalog 
WITH CHANGE_TRACKING AUTO,   
STOPLIST = SYSTEM         
GO  
  

 
GO  
CREATE FULLTEXT CATALOG WineCatalog2;  

GO
CREATE UNIQUE INDEX ui_Wines ON Wine(ID); 

GO  
CREATE FULLTEXT INDEX ON WINE ( TITLE   Language 2057 )                
KEY INDEX ui_Wines ON WineCatalog2 
WITH CHANGE_TRACKING AUTO       
GO  
