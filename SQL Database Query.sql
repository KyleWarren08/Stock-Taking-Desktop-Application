USE MASTER
-------------------------------------------------------------------------------------------------------
IF EXISTS (SELECT * FROM sys.databases WHERE NAME = 'WISdatabase')
DROP DATABASE WISdatabase;
-------------------------------------------------------------------------------------------------------
CREATE DATABASE WISdatabase;
-------------------------------------------------------------------------------------------------------
USE WISdatabase;
-------------------------------------------------------------------------------------------------------



-------------------------------------------------------------------------------------------------------
--Main stock table--
CREATE TABLE Stock_Main (
[Stock_ID] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
[Brand] VARCHAR (100),
[Title] VARCHAR (150),
[Date] DATE,
[Unit] INT,
[Price_Each] DECIMAL (18,2),
[Type_ID] VARCHAR (30) NOT NULL,
[Catergory] VARCHAR (50) NOT NULL,
[Sub_Catergory] VARCHAR (50),
);
-------------------------------------------------------------------------------------------------------
DROP TABLE Stock_Main;

SET IDENTITY_INSERT dbo.Stock_Main ON;


DELETE FROM Stock_Main;

DBCC CHECKIDENT (Stock_Main, RESEED, 26);

SELECT * 
FROM Stock_Main;
-------------------------------------------------------------------------------------------------------
INSERT INTO Stock_Main ([Stock_ID], [Brand], [Title], [Date], [Unit], [Price_Each], [Type_ID], [Catergory], [Sub_Catergory])
VALUES ('1', NULL, 'Sealing Pliers', '2019-02-05', 1, 428.38, 'Scale', 'New Scales', NULL),
	   ('2', 'Axis', 'Axis BT 200', '2009-07-20', 1, 1781.33, 'Scale', 'New Scales', NULL),
	   ('3', 'DA', 'DA 630 30kg', '2017-04-19', 8, 1182.53, 'Scale', 'Outside Stockrooms And Garages', 'Outside Stockroom 1'),
	   ('4', 'GC-L', 'Weighing Indicator GC-L', '2017-05-29', 0, 489.40, 'Scale', 'Outside Stockrooms And Garages', 'Outside Stockroom 1'),
	   ('5', 'CA', 'CA 30kg', '2017-11-17', 5, 1027.98, 'Scale', 'Outside Stockrooms And Garages', 'Outside Stockroom 1'),
	   ('6', 'CA', 'CA 8kg', '2017-11-17', 10, 1027.98, 'Scale', 'Outside Stockrooms And Garages', 'Outside Stockroom 1'),
	   ('7', 'GC-L', 'GC-L Platform Scale 300kg', '2019-04-10', 19, 2011.04, 'Scale', 'Outside Stockrooms And Garages', 'Middle Garage'),
	   ('8', 'GC-L', 'GC-L Platform Scale 150kg', '2019-04-10', 3, 1907.38, 'Scale', 'Outside Stockrooms And Garages', 'Middle Garage'),
	   ('9', 'GC-L', 'GC-L Platform Scale 150kg', '2020-12-03', 45, 1650.94, 'Scale', 'Outside Stockrooms And Garages', 'Outside Garage'),
	   ('10', NULL, 'Platform Scale 0.8 x 0.8 M/S', '2020-12-03', 9, 2382.80, 'Scale', 'Outside Stockrooms And Garages', 'Outside Garage'),
	   ('11', NULL, 'Platform Scale 1.2 x 1.2 M/S', '2020-12-03', 14, 3114.66, 'Scale', 'Outside Stockrooms And Garages', 'Outside Garage'),
	   ('12', NULL, 'Crane Scale 10 000kg OCS-S-10', '2014-09-22', 1, 3473.18, 'Scale', 'Outside Stockrooms And Garages', 'Upstairs Stock'),
	   ('13', 'AGT-S', 'Waterproof Scale S/S 7.5kg AGT-S', '2019-02-20', 0, 979.85, 'Scale', 'Outside Stockrooms And Garages', 'Upstairs Stock'),
	   ('14', 'AGT-S', 'Waterproof Scale S/S 15kg AGT-S', '2019-02-20', 0, 979.85, 'Scale', 'Outside Stockrooms And Garages', 'Upstairs Stock'),
	   ('15', 'Dyna Link', 'Dyna Link 20t', '2016-09-02', 1, 12947.67, 'Scale', 'Outside Stockrooms And Garages', 'Upstairs Stock'),

	   ('16', 'Zemic', 'Zemic Loadcells L6D WA 30kg & CA 30kg', '2011-02-10', 3, 212.20, 'Loadcell', 'Loadcells', 'Common Loadcells'),
	   ('17', 'SQB', 'SQB 500kg', '2019-02-24', 26, 309.52, 'Loadcell', 'Loadcells', 'Common Loadcells'),
	   ('18', 'Shearbeam', '563YH 2000kg', '2008-03-05', 7, 1283.34, 'Loadcell', 'Loadcells', 'Shearbeam Loadcells'),
	   ('19', NULL, '8kg', NULL, 6, NULL, 'Loadcell', 'Loadcells', 'Slow Moving Loadcells'),

	   ('20', NULL, '2kg (Mild Steel)', NULL, 17, 31.60, 'Weight', 'Weights', 'Trade Weights'),
	   ('21', NULL, '500g', NULL, 392, 8.10, 'Weight', 'Weights', 'Trade Weights'),
	   ('22', NULL, '100g', NULL, 6, NULL, 'Weight', 'Weights', 'Nickel-Plated Brass'),
	   ('23', NULL, '200g', NULL, 0, NULL, 'Weight', 'Weights', 'Nickel-Plated Brass'),

	   ('24', NULL, '1g - 500g', NULL, 0, NULL, 'Weight_Set', 'Weight-Sets', 'Nickel-Plated Brass Sets'),
	   ('25', NULL, '1g - 1kg', NULL, 0, NULL, 'Weight_Set', 'Weight-Sets', 'Nickel-Plated Brass Sets'),

	   ('26', NULL, 'Wood for boxes & incomplete boxes Lrg, Med & Sml', NULL, 1, 1200, 'Misc', 'Work In Progress', NULL);
-------------------------------------------------------------------------------------------------------



-------------------------------------------------------------------------------------------------------
CREATE TABLE Scales (
[S_Stock_ID] INT NOT NULL PRIMARY KEY,
[Limit] DECIMAL (18,2),
[Limit_Unit] VARCHAR (10),
[Dimension_Length] DECIMAL (18,2),
[Dimension_Opperator] VARCHAR (10),
[Dimension_Width] DECIMAL (18,2),
[Dimension_Unit] VARCHAR (10),
[Is_Water_Proof] BIT
);
-------------------------------------------------------------------------------------------------------
ALTER TABLE Scales
ADD CONSTRAINT S_Stock_ID
FOREIGN KEY (S_Stock_ID) REFERENCES Stock_Main(Stock_ID);

SET IDENTITY_INSERT dbo.Scales OFF;

DELETE FROM Scales;

DROP TABLE Scales;

SELECT * 
FROM Scales;
-------------------------------------------------------------------------------------------------------
INSERT INTO Scales ([S_Stock_ID], [Limit], [Limit_Unit], [Dimension_Length], [Dimension_Opperator], [Dimension_Width], [Dimension_Unit], [Is_Water_Proof])
VALUES ('1', NULL, NULL, NULL, NULL, NULL, NULL, 0),
	   ('2', NULL, NULL, NULL, NULL, NULL, NULL, 0),
	   ('3', 30, 'kg', NULL, NULL, NULL, NULL, 0),
	   ('4', NULL, NULL, NULL, NULL, NULL, NULL, 0),
	   ('5', 30, 'kg', NULL, NULL, NULL, NULL, 0),
	   ('6', 8, 'kg', NULL, NULL, NULL, NULL, 0),
	   ('7', 300, 'kg', NULL, NULL, NULL, NULL, 0),
	   ('8', 150, 'kg', NULL, NULL, NULL, NULL, 0),
	   ('9', 150, 'kg', NULL, NULL, NULL, NULL, 0),
	   ('10', NULL, NULL, 0.8, ' x ', 0.8, NULL, 0),
	   ('11', NULL, NULL, 1.2, ' x ', 1.2, NULL, 0),
	   ('12', 10000, 'kg', NULL, NULL, NULL, NULL, 0),
	   ('13', 7.5, 'kg', NULL, NULL, NULL, NULL, 1),
	   ('14', 15, 'kg', NULL, NULL, NULL, NULL, 1),
	   ('15', 20, 't', NULL, NULL, NULL, NULL, 0);
-------------------------------------------------------------------------------------------------------
CREATE TABLE Loadcells (
[L_Stock_ID] INT NOT NULL PRIMARY KEY,
[L_Mass] DECIMAL (18,2),
[L_Mass_Unit] VARCHAR (10)
);
-------------------------------------------------------------------------------------------------------
ALTER TABLE Loadcells
ADD CONSTRAINT L_Stock_ID
FOREIGN KEY (L_Stock_ID) REFERENCES Stock_Main(Stock_ID);

SET IDENTITY_INSERT dbo.Loadcells OFF;

DELETE FROM Loadcells;

DROP TABLE Loadcells;

SELECT *
FROM Loadcells;
-------------------------------------------------------------------------------------------------------
INSERT INTO Loadcells ([L_Stock_ID],[L_Mass],[L_Mass_Unit])
VALUES ('16',30,'kg'),
	   ('17',500,'kg'),
	   ('18',2000,'kg'),
	   ('19',8,'kg');

-------------------------------------------------------------------------------------------------------
CREATE TABLE Weights (
[W_Stock_ID] INT NOT NULL PRIMARY KEY,
[W_Mass] DECIMAL (18,2),
[W_Mass_Unit] VARCHAR (10)
);
-------------------------------------------------------------------------------------------------------
ALTER TABLE Weights
ADD CONSTRAINT W_Stock_ID
FOREIGN KEY (W_Stock_ID) REFERENCES Stock_Main(Stock_ID);

SET IDENTITY_INSERT dbo.Weights OFF;

DELETE FROM Weights;

DROP TABLE Weights;

SELECT * 
FROM Weights;
-------------------------------------------------------------------------------------------------------
INSERT INTO Weights ([W_Stock_ID],[W_Mass],[W_Mass_Unit])
VALUES ('20',2,'kg'),
	   ('21',500,'g'),
	   ('22',100,'g'),
	   ('23',200,'g');


-------------------------------------------------------------------------------------------------------
CREATE TABLE Weight_Sets (
[WS_Stock_ID] INT NOT NULL PRIMARY KEY,
[MinMass] DECIMAL (18,2) NOT NULL,
[WS_Opperator] VARCHAR (10),
[MaxMass] DECIMAL (18,2) NOT NULL,
[WS_Min_Mass_Unit] VARCHAR (10),
[WS_Max_Mass_Unit] VARCHAR (10)
);
-------------------------------------------------------------------------------------------------------
ALTER TABLE Weight_Sets
ADD CONSTRAINT WS_Stock_ID
FOREIGN KEY (WS_Stock_ID) REFERENCES Stock_Main(Stock_ID);

SET IDENTITY_INSERT dbo.Weight_Sets OFF;

DELETE FROM Weight_Sets;

DROP TABLE Weight_Sets;

SELECT *
FROM Weight_Sets;
-------------------------------------------------------------------------------------------------------
INSERT INTO Weight_Sets ([WS_Stock_ID],[MinMass], [WS_Opperator], [MaxMass],[WS_Min_Mass_Unit],[WS_Max_Mass_Unit])
VALUES ('24', 1, ' - ', 500, 'g', 'g'),
	   ('25', 1, ' - ', 1, 'g', 'kg');

-------------------------------------------------------------------------------------------------------
CREATE TABLE Misc (
[M_Stock_ID] INT NOT NULL PRIMARY KEY,
[Added_Info] VARCHAR (200) NOT NULL
);
-------------------------------------------------------------------------------------------------------
ALTER TABLE Misc
ADD CONSTRAINT M_Stock_ID
FOREIGN KEY (M_Stock_ID) REFERENCES Stock_Main(Stock_ID);

ALTER TABLE Misc
ALTER COLUMN Added_Info VARCHAR(200);

SET IDENTITY_INSERT dbo.Misc OFF;

DELETE FROM Misc;

DROP TABLE Misc;

SELECT *
FROM Misc;
-------------------------------------------------------------------------------------------------------
INSERT INTO Misc ([M_Stock_ID], [Added_Info])
VALUES ('26','Wood for boxes & incomplete boxes Lrg, Med & Sml');

-------------------------------------------------------------------------------------------------------
--Users Table--

CREATE TABLE Reg_Users (
[Username] VARCHAR (50) PRIMARY KEY,
[Password] VARCHAR (150) NOT NULL,
[Role] VARCHAR (20) NOT NULL,
);

DELETE FROM Reg_Users;

DROP TABLE Reg_Users;

SELECT *
FROM Reg_Users;
-------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------
GO
/****** Object:  Table [dbo].[Reg_Users1]    Script Date: 08/03/2021 00:48:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Reg_Users1](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NULL,
	[LastName] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Contact] [nvarchar](50) NULL,
	[UserName] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[ConfirmPwd] [nvarchar](50) NULL,
	[UserType] [nvarchar](50) NULL,
 CONSTRAINT [PK_Reg_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]