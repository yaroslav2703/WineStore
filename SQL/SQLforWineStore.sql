
CREATE DATABASE WineStore ON
(name = 'WineStore', filename = 'D:\3 йспя\йо\WineStore\WineStore.mdf', size = 10 mb)
log on(name = 'WineStore_log', filename = 'D:\3 йспя\йо\WineStore\WineStore_log.ldf', size = 10 mb) ;
GO

use WineStore;

DROP TABLE "CARD";
DROP TABLE ABOUT_USER;
DROP TABLE "USER";
DROP TABLE WINE;
DROP TABLE BASKET;
DROP TABLE "SESSION";
DROP TABLE ORDER_HISTORY;
DROP TABLE "ORDER";
DROP TABLE SHOP;
DROP TABLE SHOP_FAQS;
DROP TABLE ORDER_INFORMATION;


CREATE TABLE "CARD"(
ID int primary key identity(1,1),
CARD_NUMBER INT,
DATE  nvarchar(5),
CVC INT 
);

CREATE TABLE ABOUT_USER(
ID int primary key identity(1,1),
FIRST_NAME nvarchar(50) not null,
LAST_NAME nvarchar(50) not null,
STREET_ADDRESS nvarchar(50) not null,
CITY nvarchar(50) not null,
PHONE_NUMBER nvarchar(50) not null,
CARD_INFO int references "CARD"(ID)
);

CREATE TABLE "USER"(
ID int primary key identity(1000000,1),
LOGIN nvarchar(30) not null unique,
PASSWORD nvarchar(30) not null,
ABOUT int references ABOUT_USER(ID),
IS_ACTIVE int references "SESSION"(ID)
);

CREATE TABLE "SESSION"(
ID int primary key identity(1,1),
LOGIN nvarchar(30) not null unique,
ACTIVE bit default(0)
);

CREATE TABLE WINE(
ID int primary key identity(1,1),
PRODUCT_CODE nvarchar(15) not null unique,
TITLE nvarchar(200) not null,
VOLUME int not null,
PRICE decimal(10,2) not null,
VINTAGE int not null,
AVAILABLE int not null,
TYPE nvarchar(20) not null,
COUNTRY nvarchar(30) not null,
COTEGORY nvarchar(30) not null,
FOTO nvarchar(100) not null
);



CREATE TABLE BASKET(
ID int primary key identity(1,1),
"USER" int references "USER"(ID),
WINE int references WINE(ID)
);


CREATE TABLE ORDER_HISTORY(
NUMBER int primary key identity(1,1),
"USER" int references "USER"(ID),
DATE datetime not null,
ProductCount int DEFAULT(1),
Price int NOT NULL,
Info int references ORDER_INFORMATION(ID)
);

CREATE TABLE "ORDER"(
NUMBER int primary key identity(1,1),
ORDER_NUMBER int references ORDER_HISTORY(NUMBER),
WINE int references WINE(ID)
);

CREATE TABLE ORDER_INFORMATION(
ID int primary key identity(1,1),
FIRST_NAME nvarchar(50) not null,
LAST_NAME nvarchar(50) not null,
STREET_ADDRESS nvarchar(50) not null,
CITY nvarchar(50) not null,
PHONE_NUMBER nvarchar(50) not null,
CARD_NUMBER INT not null,
DATE  nvarchar(5) not null,
CVC INT not null
);

CREATE TABLE SHOP(
ID int primary key identity(1,1),
ADDRESS nvarchar(100) not null,
TELEPHON_NUMBER nvarchar(20) not null,
Email nvarchar(20) not null,
DESCRIPTION nvarchar(1000),
ALL_WINE int default(0)
);


CREATE TABLE SHOP_FAQS(
ID int primary key identity(1,1),
question nvarchar(400) NOT NULL,
answer nvarchar(400) NOT NULL,
SHOP_NAME INT references SHOP(ID)
);
