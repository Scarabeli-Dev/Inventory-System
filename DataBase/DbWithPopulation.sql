CREATE DATABASE  IF NOT EXISTS `inventory` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `inventory`;
-- MySQL dump 10.13  Distrib 8.0.33, for Win64 (x86_64)
--
-- Host: localhost    Database: inventory
-- ------------------------------------------------------
-- Server version	8.0.32

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20230621134722_FristMigration','6.0.16');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `addressing`
--

DROP TABLE IF EXISTS `addressing`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `addressing` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `WarehouseId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Addressing_WarehouseId` (`WarehouseId`),
  CONSTRAINT `FK_Addressing_Warehouse_WarehouseId` FOREIGN KEY (`WarehouseId`) REFERENCES `warehouse` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=121 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `addressing`
--

LOCK TABLES `addressing` WRITE;
/*!40000 ALTER TABLE `addressing` DISABLE KEYS */;
INSERT INTO `addressing` VALUES (1,'Temp',7),(2,'Bitwolf',3),(3,'Daltfresh',5),(4,'Redhold',1),(5,'Regrant',1),(6,'Mat Lam Tam',9),(7,'Sonsing',5),(8,'Andalax',7),(9,'Sonsing',5),(10,'Cardguard',5),(11,'Gembucket',6),(12,'Job',3),(13,'Voyatouch',4),(14,'Cookley',9),(15,'Toughjoyfax',4),(16,'Tin',8),(17,'Stronghold',8),(18,'Cardguard',7),(19,'Bitchip',2),(20,'It',8),(21,'Latlux',3),(22,'Solarbreeze',4),(23,'Temp',7),(24,'Kanlam',1),(25,'Stim',6),(26,'Biodex',9),(27,'Holdlamis',2),(28,'Overhold',4),(29,'Keylex',2),(30,'Zoolab',10),(31,'Mat Lam Tam',9),(32,'Temp',8),(33,'Sonair',2),(34,'Opela',2),(35,'Fix San',9),(36,'Tampflex',3),(37,'Prodder',9),(38,'Tin',8),(39,'Keylex',4),(40,'Kanlam',10),(41,'Bitchip',6),(42,'Rank',10),(43,'Alpha',6),(44,'Transcof',3),(45,'Ronstring',6),(46,'Rank',6),(47,'Treeflex',5),(48,'Temp',3),(49,'Home Ing',3),(50,'Duobam',1),(51,'Otcom',2),(52,'Flowdesk',10),(53,'Sonair',5),(54,'Asoka',9),(55,'Trippledex',2),(56,'Greenlam',4),(57,'Duobam',4),(58,'Redhold',6),(59,'Job',3),(60,'Opela',7),(61,'Lotstring',3),(62,'Rank',1),(63,'Alphazap',8),(64,'Regrant',9),(65,'Wrapsafe',5),(66,'Regrant',4),(67,'Aerified',4),(68,'Home Ing',7),(69,'Lotstring',5),(70,'Pannier',8),(71,'Zontrax',3),(72,'Redhold',2),(73,'Biodex',10),(74,'Alphazap',9),(75,'Bitwolf',5),(76,'Alphazap',6),(77,'Duobam',3),(78,'Tampflex',1),(79,'Veribet',5),(80,'Viva',1),(81,'Domainer',8),(82,'Bamity',5),(83,'Fintone',9),(84,'Cookley',5),(85,'Ronstring',2),(86,'Zaam-Dox',4),(87,'Veribet',3),(88,'Namfix',7),(89,'Ronstring',8),(90,'Tampflex',2),(91,'Prodder',7),(92,'Fintone',3),(93,'Bitwolf',10),(94,'Namfix',8),(95,'Tin',8),(96,'Alphazap',7),(97,'Fixflex',2),(98,'Transcof',2),(99,'Solarbreeze',8),(100,'Bitchip',1),(101,'Transcof',10),(102,'Bitchip',2),(103,'Bitwolf',1),(104,'Vagram',3),(105,'Quo Lux',3),(106,'Temp',7),(107,'Stim',4),(108,'Andalax',3),(109,'Solarbreeze',7),(110,'Home Ing',10),(111,'Domainer',8),(112,'Temp',1),(113,'Kanlam',6),(114,'Gembucket',4),(115,'Gembucket',2),(116,'Flowdesk',9),(117,'Namfix',10),(118,'Vagram',10),(119,'Rank',10),(120,'Ronstring',3);
/*!40000 ALTER TABLE `addressing` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `addressingsinventorystart`
--

DROP TABLE IF EXISTS `addressingsinventorystart`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `addressingsinventorystart` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AddressingId` int NOT NULL,
  `InventoryStartId` int NOT NULL,
  `AddressingCountRealized` tinyint(1) NOT NULL,
  `AddressingCountEnded` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AddressingsInventoryStart_AddressingId` (`AddressingId`),
  KEY `IX_AddressingsInventoryStart_InventoryStartId` (`InventoryStartId`),
  CONSTRAINT `FK_AddressingsInventoryStart_Addressing_AddressingId` FOREIGN KEY (`AddressingId`) REFERENCES `addressing` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AddressingsInventoryStart_InventoryStart_InventoryStartId` FOREIGN KEY (`InventoryStartId`) REFERENCES `inventorystart` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `addressingsinventorystart`
--

LOCK TABLES `addressingsinventorystart` WRITE;
/*!40000 ALTER TABLE `addressingsinventorystart` DISABLE KEYS */;
/*!40000 ALTER TABLE `addressingsinventorystart` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroleclaims`
--

DROP TABLE IF EXISTS `aspnetroleclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroleclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `RoleId` int NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetRoleClaims_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroleclaims`
--

LOCK TABLES `aspnetroleclaims` WRITE;
/*!40000 ALTER TABLE `aspnetroleclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroleclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetroles` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `RoleNameIndex` (`NormalizedName`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
INSERT INTO `aspnetroles` VALUES (1,'Member','MEMBER','6aa9199b-4d51-452f-906c-1c8090673bb6'),(2,'Admin','ADMIN','fd581946-ffb8-42a5-8230-a7c37bedecd7');
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserclaims` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `ClaimType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AspNetUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderKey` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderDisplayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` int NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`),
  KEY `IX_AspNetUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetuserroles` (
  `UserId` int NOT NULL,
  `RoleId` int NOT NULL,
  `Discriminator` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IX_AspNetUserRoles_RoleId` (`RoleId`),
  CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
INSERT INTO `aspnetuserroles` VALUES (1,1,'IdentityUserRole<int>'),(2,2,'IdentityUserRole<int>');
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusers` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Available` tinyint(1) NOT NULL,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Email` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NormalizedEmail` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SecurityStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ConcurrencyStamp` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES (1,'Teste Member',0,'usuario@localhost','USUARIO@LOCALHOST','usuario@localhost','USUARIO@LOCALHOST',1,'AQAAAAEAACcQAAAAEAwZhL0gJB1dA30jDyPB/+U2vZfAqdWj5th0XyWt6s/j2zSnSZA//XQp7WrvtguYEg==','W2I6NPQ6MPYAFVV733YITHLFEFHIBOOJ','3e1e9022-2f12-4819-a162-62558a6512a2',NULL,0,0,NULL,1,0),(2,'Elanã',0,'esesena','ESESENA','elana.scarabeli@rcproconsultoria.com.br','ELANA.SCARABELI@RCPROCONSULTORIA.COM.BR',1,'AQAAAAEAACcQAAAAEMZO9l5hhJ1Ih0qGf19ry0XrxpT6uhqId7akKpLi1sjkd1WrcY4oDACtskfU6yU/MQ==','3QMXSYIIA7K46V3NW2HOHZKUH5ZA4TCI','143ace27-7d92-4422-b77d-8746687b5bfe',NULL,0,0,NULL,1,0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `aspnetusertokens`
--

DROP TABLE IF EXISTS `aspnetusertokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusertokens` (
  `UserId` int NOT NULL,
  `LoginProvider` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`UserId`,`LoginProvider`,`Name`),
  CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusertokens`
--

LOCK TABLES `aspnetusertokens` WRITE;
/*!40000 ALTER TABLE `aspnetusertokens` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetusertokens` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventorymovement`
--

DROP TABLE IF EXISTS `inventorymovement`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inventorymovement` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ItemId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `MovementeType` int NOT NULL,
  `WarehouseId` int NOT NULL,
  `Amount` double NOT NULL,
  `MovementDate` datetime(6) NOT NULL,
  `ImportDate` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_InventoryMovement_ItemId` (`ItemId`),
  KEY `IX_InventoryMovement_WarehouseId` (`WarehouseId`),
  CONSTRAINT `FK_InventoryMovement_Item_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `item` (`Id`),
  CONSTRAINT `FK_InventoryMovement_Warehouse_WarehouseId` FOREIGN KEY (`WarehouseId`) REFERENCES `warehouse` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventorymovement`
--

LOCK TABLES `inventorymovement` WRITE;
/*!40000 ALTER TABLE `inventorymovement` DISABLE KEYS */;
/*!40000 ALTER TABLE `inventorymovement` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inventorystart`
--

DROP TABLE IF EXISTS `inventorystart`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inventorystart` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `InventoryStartDate` datetime(6) NOT NULL,
  `StockTakingFinishDate` datetime(6) NOT NULL,
  `IsCompleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inventorystart`
--

LOCK TABLES `inventorystart` WRITE;
/*!40000 ALTER TABLE `inventorystart` DISABLE KEYS */;
/*!40000 ALTER TABLE `inventorystart` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `item`
--

DROP TABLE IF EXISTS `item`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `item` (
  `Id` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `UnitOfMeasurement` int NOT NULL,
  `Quantity` decimal(65,30) NOT NULL,
  `Observation` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ImageUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `item`
--

LOCK TABLES `item` WRITE;
/*!40000 ALTER TABLE `item` DISABLE KEYS */;
INSERT INTO `item` VALUES ('1','Shrimp - Tiger 21/25',9,44.270000000000000000000000000000,'Vivamus vestibulum sagittis sapien.',NULL),('10','Food Colouring - Blue',7,16.580000000000000000000000000000,'Etiam justo.',NULL),('100','Cookie Dough - Oatmeal Rasin',9,60.080000000000000000000000000000,'In sagittis dui vel nisl.',NULL),('101','Soup - Campbells, Creamy',3,21.870000000000000000000000000000,'In blandit ultrices enim.',NULL),('102','Cheese - Mozzarella, Buffalo',3,18.210000000000000000000000000000,'Proin leo odio, porttitor id, consequat in, consequat ut, nulla.',NULL),('103','Island Oasis - Ice Cream Mix',4,29.860000000000000000000000000000,'Lorem ipsum dolor sit amet, consectetuer adipiscing elit.',NULL),('104','Nestea - Ice Tea, Diet',11,87.100000000000000000000000000000,'Maecenas tristique, est et tempus semper, est quam pharetra magna, ac consequat metus sapien ut nunc.',NULL),('105','Energy Drink - Franks Pineapple',10,84.820000000000000000000000000000,'Nam tristique tortor eu pede.',NULL),('106','Veal - Loin',8,42.900000000000000000000000000000,'Suspendisse ornare consequat lectus.',NULL),('107','Nantucket - Kiwi Berry Cktl.',5,16.610000000000000000000000000000,'Proin eu mi.',NULL),('108','Juice - Cranberry, 341 Ml',5,22.870000000000000000000000000000,'Vivamus in felis eu sapien cursus vestibulum.',NULL),('109','Puree - Raspberry',5,72.410000000000000000000000000000,'Sed accumsan felis.',NULL),('11','Crackers Cheez It',3,31.460000000000000000000000000000,'In eleifend quam a odio.',NULL),('110','Pail - 15l White, With Handle',10,15.230000000000000000000000000000,'Integer tincidunt ante vel ipsum.',NULL),('111','Beans - Butter Lrg Lima',8,45.090000000000000000000000000000,'Vivamus in felis eu sapien cursus vestibulum.',NULL),('112','Jam - Apricot',10,84.580000000000000000000000000000,'Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante.',NULL),('113','Wine - Harrow Estates, Vidal',9,26.420000000000000000000000000000,'In est risus, auctor sed, tristique in, tempus sit amet, sem.',NULL),('114','Pasta - Penne, Lisce, Dry',2,34.900000000000000000000000000000,'In hac habitasse platea dictumst.',NULL),('115','Wine - Conde De Valdemar',10,79.650000000000000000000000000000,'Aliquam sit amet diam in magna bibendum imperdiet.',NULL),('116','Cups 10oz Trans',8,6.860000000000000000000000000000,'Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci.',NULL),('117','Tilapia - Fillets',3,23.640000000000000000000000000000,'Nullam sit amet turpis elementum ligula vehicula consequat.',NULL),('118','Schnappes Peppermint - Walker',2,51.570000000000000000000000000000,'Integer pede justo, lacinia eget, tincidunt eget, tempus vel, pede.',NULL),('119','Mushroom - King Eryingii',2,28.270000000000000000000000000000,'Vestibulum sed magna at nunc commodo placerat.',NULL),('12','Muffin Chocolate Individual Wrap',0,3.370000000000000000000000000000,'Aenean fermentum.',NULL),('120','Baking Powder',8,15.520000000000000000000000000000,'Duis aliquam convallis nunc.',NULL),('121','Pears - Fiorelle',10,3.620000000000000000000000000000,'Pellentesque at nulla.',NULL),('122','Munchies Honey Sweet Trail Mix',1,83.760000000000000000000000000000,'Vestibulum sed magna at nunc commodo placerat.',NULL),('123','Wine - Sauvignon Blanc',6,19.600000000000000000000000000000,'Proin risus.',NULL),('124','Pork - Hock And Feet Attached',2,35.490000000000000000000000000000,'Nullam varius.',NULL),('125','Ham - Cooked Bayonne Tinned',11,57.360000000000000000000000000000,'Aliquam augue quam, sollicitudin vitae, consectetuer eget, rutrum at, lorem.',NULL),('126','Syrup - Kahlua Chocolate',2,44.050000000000000000000000000000,'Lorem ipsum dolor sit amet, consectetuer adipiscing elit.',NULL),('127','Wine - Fino Tio Pepe Gonzalez',8,8.400000000000000000000000000000,'Praesent blandit lacinia erat.',NULL),('128','Octopus',10,21.630000000000000000000000000000,'In sagittis dui vel nisl.',NULL),('129','Red Currant Jelly',0,82.500000000000000000000000000000,'Nam congue, risus semper porta volutpat, quam pede lobortis ligula, sit amet eleifend pede libero quis orci.',NULL),('13','Soup - Campbells',1,11.950000000000000000000000000000,'Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Mauris viverra diam vitae quam.',NULL),('130','Cattail Hearts',6,16.390000000000000000000000000000,'Pellentesque at nulla.',NULL),('131','Arizona - Green Tea',11,44.970000000000000000000000000000,'Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante.',NULL),('132','Taro Root',5,71.220000000000000000000000000000,'Pellentesque eget nunc.',NULL),('133','Daikon Radish',5,45.240000000000000000000000000000,'Praesent blandit lacinia erat.',NULL),('134','Blueberries - Frozen',6,72.120000000000000000000000000000,'Morbi porttitor lorem id ligula.',NULL),('135','Lobster - Live',9,47.310000000000000000000000000000,'Ut at dolor quis odio consequat varius.',NULL),('136','Oil - Olive Bertolli',0,13.750000000000000000000000000000,'In quis justo.',NULL),('137','Energy Drink',0,55.980000000000000000000000000000,'Aenean sit amet justo.',NULL),('138','Flour Dark Rye',7,36.340000000000000000000000000000,'Phasellus sit amet erat.',NULL),('139','Wine - Spumante Bambino White',0,1.980000000000000000000000000000,'Nullam sit amet turpis elementum ligula vehicula consequat.',NULL),('14','Smirnoff Green Apple Twist',5,83.160000000000000000000000000000,'In hac habitasse platea dictumst.',NULL),('140','Triple Sec - Mcguinness',6,62.290000000000000000000000000000,'Proin at turpis a pede posuere nonummy.',NULL),('141','Sauce - Demi Glace',9,52.680000000000000000000000000000,'Nam nulla.',NULL),('142','Roe - Flying Fish',3,29.340000000000000000000000000000,'Donec semper sapien a libero.',NULL),('143','Sauerkraut',6,64.030000000000000000000000000000,'Nam dui.',NULL),('144','Towels - Paper / Kraft',7,7.880000000000000000000000000000,'Aliquam sit amet diam in magna bibendum imperdiet.',NULL),('145','Tea - Lemon Scented',2,77.910000000000000000000000000000,'Nulla tempus.',NULL),('146','Edible Flower - Mixed',4,59.420000000000000000000000000000,'Morbi vel lectus in quam fringilla rhoncus.',NULL),('147','Truffle Paste',2,22.300000000000000000000000000000,'Morbi non lectus.',NULL),('148','Vodka - Moskovskaya',1,65.250000000000000000000000000000,'Cras in purus eu magna vulputate luctus.',NULL),('149','Tea - Orange Pekoe',2,93.060000000000000000000000000000,'Duis aliquam convallis nunc.',NULL),('15','Tomatoes - Orange',0,27.750000000000000000000000000000,'Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.',NULL),('150','Longos - Burritos',9,47.320000000000000000000000000000,'Nulla suscipit ligula in lacus.',NULL),('151','Sambuca - Ramazzotti',4,69.130000000000000000000000000000,'Donec diam neque, vestibulum eget, vulputate ut, ultrices vel, augue.',NULL),('152','Cheese - Gorgonzola',11,90.610000000000000000000000000000,'Nullam molestie nibh in lectus.',NULL),('153','Muffin Mix - Carrot',5,84.450000000000000000000000000000,'Quisque porta volutpat erat.',NULL),('154','Syrup - Monin - Granny Smith',5,38.550000000000000000000000000000,'Nulla tellus.',NULL),('155','Zucchini - Mini, Green',5,19.100000000000000000000000000000,'Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl.',NULL),('156','Salmon - Atlantic, Skin On',9,2.400000000000000000000000000000,'Proin at turpis a pede posuere nonummy.',NULL),('157','Dish Towel',6,69.820000000000000000000000000000,'Suspendisse ornare consequat lectus.',NULL),('158','Pail For Lid 1537',0,39.380000000000000000000000000000,'Morbi vestibulum, velit id pretium iaculis, diam erat fermentum justo, nec condimentum neque sapien placerat ante.',NULL),('159','Ham - Black Forest',5,85.620000000000000000000000000000,'Nullam molestie nibh in lectus.',NULL),('16','Wine - Duboeuf Beaujolais',9,85.040000000000000000000000000000,'Curabitur at ipsum ac tellus semper interdum.',NULL),('160','Water - Spring 1.5lit',7,77.950000000000000000000000000000,'Morbi porttitor lorem id ligula.',NULL),('161','Salmon - Whole, 4 - 6 Pounds',6,14.770000000000000000000000000000,'Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.',NULL),('162','Longos - Chicken Curried',10,91.860000000000000000000000000000,'Proin risus.',NULL),('163','Schnappes - Peach, Walkers',2,76.610000000000000000000000000000,'In sagittis dui vel nisl.',NULL),('164','Ginger - Fresh',9,57.370000000000000000000000000000,'Vivamus tortor.',NULL),('165','Spice - Paprika',0,84.660000000000000000000000000000,'In quis justo.',NULL),('166','Containter - 3oz Microwave Rect.',0,83.940000000000000000000000000000,'Vestibulum quam sapien, varius ut, blandit non, interdum in, ante.',NULL),('167','Compound - Rum',4,3.650000000000000000000000000000,'Etiam justo.',NULL),('168','Soup - Cream Of Broccoli, Dry',11,99.520000000000000000000000000000,'Aliquam quis turpis eget elit sodales scelerisque.',NULL),('169','Wine - Blue Nun Qualitatswein',0,81.710000000000000000000000000000,'Sed sagittis.',NULL),('17','Plums - Red',6,39.810000000000000000000000000000,'Maecenas rhoncus aliquam lacus.',NULL),('170','Jolt Cola',8,51.780000000000000000000000000000,'Nam congue, risus semper porta volutpat, quam pede lobortis ligula, sit amet eleifend pede libero quis orci.',NULL),('171','Butter Ripple - Phillips',11,38.700000000000000000000000000000,'Praesent lectus.',NULL),('172','Tea - Herbal I Love Lemon',6,99.640000000000000000000000000000,'Nullam varius.',NULL),('173','Nantucket - Kiwi Berry Cktl.',8,22.420000000000000000000000000000,'Integer non velit.',NULL),('174','Coffee Beans - Chocolate',9,8.270000000000000000000000000000,'Pellentesque at nulla.',NULL),('175','Truffle Cups - White Paper',5,12.520000000000000000000000000000,'Cras non velit nec nisi vulputate nonummy.',NULL),('176','Wood Chips - Regular',9,83.180000000000000000000000000000,'Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Duis faucibus accumsan odio.',NULL),('177','Juice - Orangina',7,95.070000000000000000000000000000,'Integer tincidunt ante vel ipsum.',NULL),('178','Barramundi',1,87.740000000000000000000000000000,'Nunc nisl.',NULL),('179','Beer - Camerons Auburn',6,45.920000000000000000000000000000,'Vestibulum sed magna at nunc commodo placerat.',NULL),('18','Wine - Niagara,vqa Reisling',9,85.580000000000000000000000000000,'Duis bibendum.',NULL),('180','Vaccum Bag - 14x20',9,7.270000000000000000000000000000,'Curabitur in libero ut massa volutpat convallis.',NULL),('181','Sea Bass - Whole',6,67.210000000000000000000000000000,'Duis mattis egestas metus.',NULL),('182','Coconut - Whole',9,9.250000000000000000000000000000,'In hac habitasse platea dictumst.',NULL),('183','Compound - Orange',9,81.430000000000000000000000000000,'Nulla ac enim.',NULL),('184','Iced Tea Concentrate',3,95.280000000000000000000000000000,'In tempor, turpis nec euismod scelerisque, quam turpis adipiscing lorem, vitae mattis nibh ligula nec sem.',NULL),('185','Wine - Tio Pepe Sherry Fino',11,30.820000000000000000000000000000,'Etiam vel augue.',NULL),('186','Pastry - Trippleberry Muffin - Mini',6,39.400000000000000000000000000000,'Cras in purus eu magna vulputate luctus.',NULL),('187','Couscous',6,24.600000000000000000000000000000,'Vestibulum quam sapien, varius ut, blandit non, interdum in, ante.',NULL),('188','Wine - White, Riesling, Semi - Dry',11,53.970000000000000000000000000000,'Phasellus id sapien in sapien iaculis congue.',NULL),('189','Tomato Puree',2,84.250000000000000000000000000000,'Curabitur convallis.',NULL),('19','Cake - French Pear Tart',8,76.090000000000000000000000000000,'Curabitur convallis.',NULL),('190','Wine - Peller Estates Late',11,43.340000000000000000000000000000,'Donec vitae nisi.',NULL),('191','Crush - Orange, 355ml',6,72.060000000000000000000000000000,'Nam ultrices, libero non mattis pulvinar, nulla pede ullamcorper augue, a suscipit nulla elit ac nulla.',NULL),('192','Shrimp - Prawn',6,24.510000000000000000000000000000,'Nam dui.',NULL),('193','Oranges - Navel, 72',9,17.610000000000000000000000000000,'Fusce lacus purus, aliquet at, feugiat non, pretium quis, lectus.',NULL),('194','Duck - Breast',6,22.710000000000000000000000000000,'Nam nulla.',NULL),('195','Beef - Tenderlion, Center Cut',5,16.900000000000000000000000000000,'Mauris lacinia sapien quis libero.',NULL),('196','Bouillion - Fish',5,97.390000000000000000000000000000,'Nunc nisl.',NULL),('197','Cabbage - Savoy',8,65.500000000000000000000000000000,'Nulla suscipit ligula in lacus.',NULL),('198','Bread - Frozen Basket Variety',4,5.510000000000000000000000000000,'In est risus, auctor sed, tristique in, tempus sit amet, sem.',NULL),('199','Lambcasing',11,99.560000000000000000000000000000,'Maecenas rhoncus aliquam lacus.',NULL),('2','Beef - Prime Rib Aaa',6,14.660000000000000000000000000000,'In sagittis dui vel nisl.',NULL),('20','Muffin Batt - Ban Dream Zero',1,47.480000000000000000000000000000,'Nulla mollis molestie lorem.',NULL),('200','Wine - Duboeuf Beaujolais',8,87.980000000000000000000000000000,'Nulla justo.',NULL),('201','Cheese - Cheddar, Medium',8,4.380000000000000000000000000000,'Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci.',NULL),('202','Sauce - Gravy, Au Jus, Mix',4,67.200000000000000000000000000000,'Nulla justo.',NULL),('203','Clams - Bay',0,97.380000000000000000000000000000,'Duis at velit eu est congue elementum.',NULL),('204','Beef - Ground Medium',5,5.320000000000000000000000000000,'Donec ut dolor.',NULL),('205','Pear - Packum',11,94.240000000000000000000000000000,'Curabitur convallis.',NULL),('206','Flour - Semolina',4,79.030000000000000000000000000000,'Nulla tellus.',NULL),('207','Extract - Almond',1,97.610000000000000000000000000000,'Aenean lectus.',NULL),('208','Buffalo - Striploin',10,24.730000000000000000000000000000,'In quis justo.',NULL),('209','Artichoke - Bottom, Canned',6,40.070000000000000000000000000000,'Curabitur convallis.',NULL),('21','Water - Tonic',2,6.970000000000000000000000000000,'Sed ante.',NULL),('210','Beans - Soya Bean',11,72.060000000000000000000000000000,'Lorem ipsum dolor sit amet, consectetuer adipiscing elit.',NULL),('211','Mushroom - Lg - Cello',6,9.960000000000000000000000000000,'Maecenas tincidunt lacus at velit.',NULL),('212','Silicone Paper 16.5x24',7,6.910000000000000000000000000000,'Duis consequat dui nec nisi volutpat eleifend.',NULL),('213','Pepper - Red Thai',8,17.450000000000000000000000000000,'Praesent lectus.',NULL),('214','Mop Head - Cotton, 24 Oz',3,37.530000000000000000000000000000,'Donec odio justo, sollicitudin ut, suscipit a, feugiat et, eros.',NULL),('215','Steel Wool S.o.s',0,50.600000000000000000000000000000,'Donec posuere metus vitae ipsum.',NULL),('216','Lamb Leg - Bone - In Nz',2,87.000000000000000000000000000000,'Morbi non quam nec dui luctus rutrum.',NULL),('217','Table Cloth 62x114 White',4,76.630000000000000000000000000000,'Cras in purus eu magna vulputate luctus.',NULL),('218','Amarula Cream',1,83.110000000000000000000000000000,'Nulla mollis molestie lorem.',NULL),('219','Beer - Blue',9,48.460000000000000000000000000000,'Nullam orci pede, venenatis non, sodales sed, tincidunt eu, felis.',NULL),('22','Pork - Sausage, Medium',1,97.110000000000000000000000000000,'Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.',NULL),('220','Corn Kernels - Frozen',10,37.540000000000000000000000000000,'Nulla ut erat id mauris vulputate elementum.',NULL),('221','Longos - Cheese Tortellini',8,89.020000000000000000000000000000,'Sed sagittis.',NULL),('222','Pasta - Bauletti, Chicken White',3,29.050000000000000000000000000000,'Aenean sit amet justo.',NULL),('223','Horseradish - Prepared',11,16.950000000000000000000000000000,'Duis at velit eu est congue elementum.',NULL),('224','Skirt - 24 Foot',1,13.220000000000000000000000000000,'Nullam orci pede, venenatis non, sodales sed, tincidunt eu, felis.',NULL),('225','Ginger - Crystalized',10,38.000000000000000000000000000000,'Nulla neque libero, convallis eget, eleifend luctus, ultricies eu, nibh.',NULL),('226','Baking Soda',4,91.230000000000000000000000000000,'Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Mauris viverra diam vitae quam.',NULL),('227','Tumeric',9,98.790000000000000000000000000000,'Suspendisse ornare consequat lectus.',NULL),('228','Chestnuts - Whole,canned',8,98.290000000000000000000000000000,'Vestibulum quam sapien, varius ut, blandit non, interdum in, ante.',NULL),('229','Cookie - Oreo 100x2',8,84.640000000000000000000000000000,'Phasellus sit amet erat.',NULL),('23','Syrup - Monin - Granny Smith',7,58.750000000000000000000000000000,'Mauris lacinia sapien quis libero.',NULL),('230','Marjoram - Dried, Rubbed',2,43.810000000000000000000000000000,'Aliquam augue quam, sollicitudin vitae, consectetuer eget, rutrum at, lorem.',NULL),('231','Island Oasis - Raspberry',9,10.190000000000000000000000000000,'Integer ac neque.',NULL),('232','Beer - Paulaner Hefeweisse',4,7.760000000000000000000000000000,'Etiam pretium iaculis justo.',NULL),('233','Table Cloth 72x144 White',11,18.020000000000000000000000000000,'Donec ut dolor.',NULL),('234','Black Currants',0,83.330000000000000000000000000000,'Lorem ipsum dolor sit amet, consectetuer adipiscing elit.',NULL),('235','Macaroons - Two Bite Choc',4,32.050000000000000000000000000000,'Vestibulum rutrum rutrum neque.',NULL),('236','The Pop Shoppe - Lime Rickey',5,41.560000000000000000000000000000,'Praesent lectus.',NULL),('237','Juice - Cranberry 284ml',5,48.440000000000000000000000000000,'Maecenas tincidunt lacus at velit.',NULL),('238','Sea Bass - Whole',2,70.600000000000000000000000000000,'Integer pede justo, lacinia eget, tincidunt eget, tempus vel, pede.',NULL),('239','Wine - Ruffino Chianti Classico',10,49.080000000000000000000000000000,'Integer non velit.',NULL),('24','Triple Sec - Mcguinness',10,69.830000000000000000000000000000,'Proin risus.',NULL),('240','Bread - Pain Au Liat X12',0,24.720000000000000000000000000000,'Proin eu mi.',NULL),('241','Salt - Rock, Course',0,1.870000000000000000000000000000,'Fusce lacus purus, aliquet at, feugiat non, pretium quis, lectus.',NULL),('242','Tea - English Breakfast',11,16.410000000000000000000000000000,'In congue.',NULL),('243','Lamb - Leg, Diced',8,79.990000000000000000000000000000,'Nulla suscipit ligula in lacus.',NULL),('244','Tilapia - Fillets',6,59.700000000000000000000000000000,'In est risus, auctor sed, tristique in, tempus sit amet, sem.',NULL),('245','Coffee - Cafe Moreno',11,53.110000000000000000000000000000,'Maecenas leo odio, condimentum id, luctus nec, molestie sed, justo.',NULL),('246','Chicken Breast Halal',5,58.750000000000000000000000000000,'Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Duis faucibus accumsan odio.',NULL),('247','Sobe - Green Tea',7,23.050000000000000000000000000000,'Integer tincidunt ante vel ipsum.',NULL),('248','Gherkin',11,59.140000000000000000000000000000,'Etiam vel augue.',NULL),('249','Cup - 6oz, Foam',10,69.740000000000000000000000000000,'In blandit ultrices enim.',NULL),('25','Asparagus - Mexican',6,50.430000000000000000000000000000,'Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci.',NULL),('250','Veal - Striploin',11,97.960000000000000000000000000000,'Phasellus sit amet erat.',NULL),('251','Wine - Riesling Alsace Ac 2001',5,39.430000000000000000000000000000,'Mauris enim leo, rhoncus sed, vestibulum sit amet, cursus id, turpis.',NULL),('252','Cheese - Brie, Triple Creme',4,69.470000000000000000000000000000,'Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Mauris viverra diam vitae quam.',NULL),('253','Pork - Smoked Kassler',1,29.840000000000000000000000000000,'Integer non velit.',NULL),('254','Beef - Eye Of Round',2,16.060000000000000000000000000000,'Nullam orci pede, venenatis non, sodales sed, tincidunt eu, felis.',NULL),('255','Hot Choc Vending',6,6.660000000000000000000000000000,'Nullam sit amet turpis elementum ligula vehicula consequat.',NULL),('256','Bag Stand',10,72.830000000000000000000000000000,'Etiam pretium iaculis justo.',NULL),('257','Skirt - 24 Foot',3,83.970000000000000000000000000000,'Morbi quis tortor id nulla ultrices aliquet.',NULL),('258','Jello - Assorted',4,97.280000000000000000000000000000,'Duis at velit eu est congue elementum.',NULL),('259','Pepper - Black, Crushed',7,89.310000000000000000000000000000,'Quisque porta volutpat erat.',NULL),('26','Oil - Olive, Extra Virgin',3,42.060000000000000000000000000000,'Nullam orci pede, venenatis non, sodales sed, tincidunt eu, felis.',NULL),('260','Crab - Dungeness, Whole',6,96.280000000000000000000000000000,'Vivamus vel nulla eget eros elementum pellentesque.',NULL),('261','Lettuce - Escarole',0,2.780000000000000000000000000000,'Nunc nisl.',NULL),('262','Pepsi, 355 Ml',8,74.280000000000000000000000000000,'Donec odio justo, sollicitudin ut, suscipit a, feugiat et, eros.',NULL),('263','Sloe Gin - Mcguinness',3,9.240000000000000000000000000000,'Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Mauris viverra diam vitae quam.',NULL),('264','Salsify, Organic',9,39.550000000000000000000000000000,'Suspendisse ornare consequat lectus.',NULL),('265','Lettuce - Romaine',7,98.930000000000000000000000000000,'Praesent blandit.',NULL),('266','Wine - Alicanca Vinho Verde',5,71.690000000000000000000000000000,'Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Mauris viverra diam vitae quam.',NULL),('267','Southern Comfort',10,44.120000000000000000000000000000,'Maecenas pulvinar lobortis est.',NULL),('268','Wine - Chardonnay Mondavi',10,6.500000000000000000000000000000,'Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci.',NULL),('269','Ham - Smoked, Bone - In',6,20.780000000000000000000000000000,'Mauris ullamcorper purus sit amet nulla.',NULL),('27','Basil - Dry, Rubbed',2,1.570000000000000000000000000000,'In hac habitasse platea dictumst.',NULL),('270','Onions - Cippolini',10,84.990000000000000000000000000000,'Nunc nisl.',NULL),('271','Longos - Lasagna Beef',5,30.000000000000000000000000000000,'Curabitur in libero ut massa volutpat convallis.',NULL),('272','Sauce - Hollandaise',9,69.530000000000000000000000000000,'Nunc purus.',NULL),('273','Chinese Foods - Pepper Beef',0,93.090000000000000000000000000000,'Vivamus tortor.',NULL),('274','Wine - Pinot Noir Latour',3,4.780000000000000000000000000000,'Donec ut dolor.',NULL),('275','Flour - Corn, Fine',11,79.720000000000000000000000000000,'Morbi odio odio, elementum eu, interdum eu, tincidunt in, leo.',NULL),('276','Wine - Pinot Grigio Collavini',7,23.970000000000000000000000000000,'Duis aliquam convallis nunc.',NULL),('277','Cheese - Blue',1,19.210000000000000000000000000000,'Aliquam erat volutpat.',NULL),('278','Orange Roughy 6/8 Oz',7,82.320000000000000000000000000000,'Maecenas tincidunt lacus at velit.',NULL),('279','Nutmeg - Ground',7,99.820000000000000000000000000000,'Curabitur gravida nisi at nibh.',NULL),('28','Water - Spring Water 500ml',0,88.120000000000000000000000000000,'Sed accumsan felis.',NULL),('280','Butter - Salted, Micro',2,41.720000000000000000000000000000,'Nullam porttitor lacus at turpis.',NULL),('281','Ecolab - Medallion',10,16.910000000000000000000000000000,'Fusce lacus purus, aliquet at, feugiat non, pretium quis, lectus.',NULL),('282','Sauce - Caesar Dressing',4,89.780000000000000000000000000000,'In eleifend quam a odio.',NULL),('283','Wine - Magnotta - Cab Sauv',9,60.310000000000000000000000000000,'Nam ultrices, libero non mattis pulvinar, nulla pede ullamcorper augue, a suscipit nulla elit ac nulla.',NULL),('284','Pepper - Sorrano',6,41.090000000000000000000000000000,'Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla dapibus dolor vel est.',NULL),('285','Pork - Suckling Pig',7,63.190000000000000000000000000000,'Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.',NULL),('286','Nut - Pecan, Pieces',0,71.120000000000000000000000000000,'Sed accumsan felis.',NULL),('287','Champagne - Brights, Dry',4,48.180000000000000000000000000000,'Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.',NULL),('288','Onions Granulated',7,30.970000000000000000000000000000,'Vestibulum sed magna at nunc commodo placerat.',NULL),('289','Radish',3,32.350000000000000000000000000000,'Quisque porta volutpat erat.',NULL),('29','Wine - Red, Colio Cabernet',4,36.790000000000000000000000000000,'Cras mi pede, malesuada in, imperdiet et, commodo vulputate, justo.',NULL),('290','Oil - Sunflower',5,84.250000000000000000000000000000,'Nulla mollis molestie lorem.',NULL),('291','Vanilla Beans',3,23.420000000000000000000000000000,'Praesent id massa id nisl venenatis lacinia.',NULL),('292','Stock - Chicken, White',4,43.860000000000000000000000000000,'Aliquam augue quam, sollicitudin vitae, consectetuer eget, rutrum at, lorem.',NULL),('293','Table Cloth 53x69 White',0,45.000000000000000000000000000000,'Donec diam neque, vestibulum eget, vulputate ut, ultrices vel, augue.',NULL),('294','Soup Campbells Beef With Veg',1,74.730000000000000000000000000000,'Integer aliquet, massa id lobortis convallis, tortor risus dapibus augue, vel accumsan tellus nisi eu orci.',NULL),('295','Puree - Passion Fruit',7,42.660000000000000000000000000000,'Vestibulum ac est lacinia nisi venenatis tristique.',NULL),('296','Lemonade - Island Tea, 591 Ml',4,46.250000000000000000000000000000,'Vivamus vel nulla eget eros elementum pellentesque.',NULL),('297','Rosemary - Dry',10,82.660000000000000000000000000000,'Nulla neque libero, convallis eget, eleifend luctus, ultricies eu, nibh.',NULL),('298','Tuna - Canned, Flaked, Light',2,90.170000000000000000000000000000,'Duis ac nibh.',NULL),('299','Garlic - Peeled',1,63.070000000000000000000000000000,'Nunc nisl.',NULL),('3','Grouper - Fresh',3,97.870000000000000000000000000000,'Nulla ac enim.',NULL),('30','Cheese Cloth No 60',2,92.340000000000000000000000000000,'In quis justo.',NULL),('300','Island Oasis - Ice Cream Mix',8,42.520000000000000000000000000000,'Nullam varius.',NULL),('31','Cream - 10%',3,12.810000000000000000000000000000,'Aliquam quis turpis eget elit sodales scelerisque.',NULL),('32','Mushroom - King Eryingii',10,25.670000000000000000000000000000,'Lorem ipsum dolor sit amet, consectetuer adipiscing elit.',NULL),('33','Crush - Orange, 355ml',5,5.630000000000000000000000000000,'Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.',NULL),('34','Salmon - Sockeye Raw',8,28.780000000000000000000000000000,'Morbi a ipsum.',NULL),('35','Ecolab Digiclean Mild Fm',4,92.150000000000000000000000000000,'Vivamus in felis eu sapien cursus vestibulum.',NULL),('36','Appetizer - Mango Chevre',2,47.720000000000000000000000000000,'Mauris ullamcorper purus sit amet nulla.',NULL),('37','Salmon - Sockeye Raw',10,53.350000000000000000000000000000,'In hac habitasse platea dictumst.',NULL),('38','Sprouts - Alfalfa',9,20.870000000000000000000000000000,'Nullam varius.',NULL),('39','The Pop Shoppe Pinapple',2,54.090000000000000000000000000000,'Nunc nisl.',NULL),('4','Soup - Knorr, Ministrone',10,20.730000000000000000000000000000,'Nullam sit amet turpis elementum ligula vehicula consequat.',NULL),('40','Wine - Cabernet Sauvignon',8,62.260000000000000000000000000000,'Nam dui.',NULL),('41','Chinese Foods - Chicken Wing',6,79.540000000000000000000000000000,'Suspendisse accumsan tortor quis turpis.',NULL),('42','Mix - Cocktail Strawberry Daiquiri',3,45.050000000000000000000000000000,'Nunc nisl.',NULL),('43','Quail - Jumbo Boneless',1,31.030000000000000000000000000000,'Nunc purus.',NULL),('44','Tart Shells - Savory, 4',2,54.570000000000000000000000000000,'Maecenas pulvinar lobortis est.',NULL),('45','Pasta - Elbows, Macaroni, Dry',7,78.620000000000000000000000000000,'Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl.',NULL),('46','Sultanas',6,74.130000000000000000000000000000,'Maecenas leo odio, condimentum id, luctus nec, molestie sed, justo.',NULL),('47','Bulgar',1,63.220000000000000000000000000000,'Morbi vel lectus in quam fringilla rhoncus.',NULL),('48','Cherries - Frozen',3,32.360000000000000000000000000000,'Mauris lacinia sapien quis libero.',NULL),('49','Pepper Squash',10,88.120000000000000000000000000000,'Donec diam neque, vestibulum eget, vulputate ut, ultrices vel, augue.',NULL),('5','Tomato - Green',11,13.590000000000000000000000000000,'Curabitur at ipsum ac tellus semper interdum.',NULL),('50','Pasta - Angel Hair',6,21.510000000000000000000000000000,'Curabitur in libero ut massa volutpat convallis.',NULL),('51','Lettuce - Escarole',4,67.530000000000000000000000000000,'Donec dapibus.',NULL),('52','Quinoa',1,97.930000000000000000000000000000,'Etiam faucibus cursus urna.',NULL),('53','Table Cloth 62x120 White',0,12.220000000000000000000000000000,'Morbi non lectus.',NULL),('54','Lettuce - Spring Mix',7,17.240000000000000000000000000000,'Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.',NULL),('55','Bread - Calabrese Baguette',8,97.990000000000000000000000000000,'Praesent lectus.',NULL),('56','Wine - Red, Gamay Noir',11,45.480000000000000000000000000000,'Phasellus id sapien in sapien iaculis congue.',NULL),('57','Soup - Campbells Beef Stew',2,25.170000000000000000000000000000,'Vivamus in felis eu sapien cursus vestibulum.',NULL),('58','Clam - Cherrystone',11,9.350000000000000000000000000000,'Aenean fermentum.',NULL),('59','Sauce Bbq Smokey',10,4.370000000000000000000000000000,'Phasellus id sapien in sapien iaculis congue.',NULL),('6','Cheese - Mozzarella',10,46.930000000000000000000000000000,'Praesent lectus.',NULL),('60','Greens Mustard',9,65.620000000000000000000000000000,'Donec dapibus.',NULL),('61','Juice - Pineapple, 48 Oz',1,10.110000000000000000000000000000,'Aliquam augue quam, sollicitudin vitae, consectetuer eget, rutrum at, lorem.',NULL),('62','Cheese - Brie, Cups 125g',3,85.940000000000000000000000000000,'Vivamus metus arcu, adipiscing molestie, hendrerit at, vulputate vitae, nisl.',NULL),('63','Fruit Salad Deluxe',11,39.390000000000000000000000000000,'Donec vitae nisi.',NULL),('64','Cocktail Napkin Blue',0,71.480000000000000000000000000000,'Nullam orci pede, venenatis non, sodales sed, tincidunt eu, felis.',NULL),('65','Green Scrubbie Pad H.duty',6,47.270000000000000000000000000000,'Quisque ut erat.',NULL),('66','Food Colouring - Blue',11,86.070000000000000000000000000000,'Morbi vel lectus in quam fringilla rhoncus.',NULL),('67','Soup - Campbells Chili',4,18.520000000000000000000000000000,'In congue.',NULL),('68','Cake Circle, Foil, Scallop',6,24.280000000000000000000000000000,'Nulla neque libero, convallis eget, eleifend luctus, ultricies eu, nibh.',NULL),('69','Ginger - Pickled',1,86.610000000000000000000000000000,'Fusce posuere felis sed lacus.',NULL),('7','Apricots Fresh',2,77.350000000000000000000000000000,'Quisque id justo sit amet sapien dignissim vestibulum.',NULL),('70','Sausage - Andouille',5,9.780000000000000000000000000000,'In hac habitasse platea dictumst.',NULL),('71','Pork - Sausage, Medium',10,24.060000000000000000000000000000,'Maecenas tincidunt lacus at velit.',NULL),('72','Olives - Black, Pitted',6,38.500000000000000000000000000000,'Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Duis faucibus accumsan odio.',NULL),('73','Beer - True North Lager',11,61.720000000000000000000000000000,'Vivamus in felis eu sapien cursus vestibulum.',NULL),('74','Lamb Rack - Ontario',3,89.320000000000000000000000000000,'Donec dapibus.',NULL),('75','Veal - Leg',10,68.900000000000000000000000000000,'Morbi sem mauris, laoreet ut, rhoncus aliquet, pulvinar sed, nisl.',NULL),('76','Cookie Double Choco',10,37.780000000000000000000000000000,'Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Duis faucibus accumsan odio.',NULL),('77','Pate - Peppercorn',0,91.100000000000000000000000000000,'Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus.',NULL),('78','Iced Tea - Lemon, 460 Ml',4,21.910000000000000000000000000000,'Curabitur in libero ut massa volutpat convallis.',NULL),('79','Vector Energy Bar',11,45.790000000000000000000000000000,'Sed vel enim sit amet nunc viverra dapibus.',NULL),('8','Wine - Pinot Noir Pond Haddock',11,98.420000000000000000000000000000,'Vivamus vel nulla eget eros elementum pellentesque.',NULL),('80','Yogurt - Plain',3,84.030000000000000000000000000000,'Morbi non quam nec dui luctus rutrum.',NULL),('81','Truffle - Whole Black Peeled',10,47.600000000000000000000000000000,'Aliquam quis turpis eget elit sodales scelerisque.',NULL),('82','Bay Leaf Fresh',9,86.050000000000000000000000000000,'Morbi vel lectus in quam fringilla rhoncus.',NULL),('83','Container - Clear 16 Oz',7,7.830000000000000000000000000000,'Quisque erat eros, viverra eget, congue eget, semper rutrum, nulla.',NULL),('84','Sultanas',4,28.690000000000000000000000000000,'Duis ac nibh.',NULL),('85','Wine - Sicilia Igt Nero Avola',10,56.540000000000000000000000000000,'Curabitur gravida nisi at nibh.',NULL),('86','Persimmons',6,56.050000000000000000000000000000,'Pellentesque ultrices mattis odio.',NULL),('87','Oil - Coconut',11,95.130000000000000000000000000000,'Nam ultrices, libero non mattis pulvinar, nulla pede ullamcorper augue, a suscipit nulla elit ac nulla.',NULL),('88','Mix - Cappucino Cocktail',5,67.390000000000000000000000000000,'Pellentesque ultrices mattis odio.',NULL),('89','Hipnotiq Liquor',2,3.560000000000000000000000000000,'Phasellus in felis.',NULL),('9','Tea - Lemon Scented',5,30.460000000000000000000000000000,'Cras non velit nec nisi vulputate nonummy.',NULL),('90','Roe - White Fish',5,90.690000000000000000000000000000,'Vestibulum ac est lacinia nisi venenatis tristique.',NULL),('91','Longos - Grilled Chicken With',3,38.000000000000000000000000000000,'Duis bibendum, felis sed interdum venenatis, turpis enim blandit mi, in porttitor pede justo eu massa.',NULL),('92','Beer - Steamwhistle',1,4.320000000000000000000000000000,'Proin at turpis a pede posuere nonummy.',NULL),('93','Kellogs Cereal In A Cup',7,9.270000000000000000000000000000,'Curabitur at ipsum ac tellus semper interdum.',NULL),('94','Sugar - Splenda Sweetener',6,35.510000000000000000000000000000,'Maecenas ut massa quis augue luctus tincidunt.',NULL),('95','Beans - Black Bean, Dry',3,2.410000000000000000000000000000,'Pellentesque at nulla.',NULL),('96','Vaccum Bag 10x13',10,76.670000000000000000000000000000,'Vestibulum ac est lacinia nisi venenatis tristique.',NULL),('97','Cut Wakame - Hanawakaba',1,99.940000000000000000000000000000,'Proin risus.',NULL),('98','Teriyaki Sauce',3,36.530000000000000000000000000000,'Maecenas pulvinar lobortis est.',NULL),('99','Sansho Powder',4,16.650000000000000000000000000000,'Maecenas leo odio, condimentum id, luctus nec, molestie sed, justo.',NULL);
/*!40000 ALTER TABLE `item` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `itemsaddressing`
--

DROP TABLE IF EXISTS `itemsaddressing`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `itemsaddressing` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `AddressingId` int NOT NULL,
  `ItemId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ItemsAddressing_AddressingId` (`AddressingId`),
  KEY `IX_ItemsAddressing_ItemId` (`ItemId`),
  CONSTRAINT `FK_ItemsAddressing_Addressing_AddressingId` FOREIGN KEY (`AddressingId`) REFERENCES `addressing` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ItemsAddressing_Item_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `item` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=351 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `itemsaddressing`
--

LOCK TABLES `itemsaddressing` WRITE;
/*!40000 ALTER TABLE `itemsaddressing` DISABLE KEYS */;
INSERT INTO `itemsaddressing` VALUES (1,13,'285'),(2,6,'16'),(3,12,'192'),(4,7,'289'),(5,10,'201'),(6,5,'195'),(7,9,'162'),(8,3,'289'),(9,5,'93'),(10,12,'27'),(11,11,'44'),(12,9,'230'),(13,9,'219'),(14,6,'120'),(15,1,'227'),(16,5,'67'),(17,4,'269'),(18,4,'224'),(19,13,'158'),(20,13,'202'),(21,9,'53'),(22,13,'63'),(23,8,'224'),(24,12,'172'),(25,9,'170'),(26,8,'140'),(27,9,'117'),(28,2,'62'),(29,8,'71'),(30,12,'183'),(31,9,'246'),(32,4,'300'),(33,2,'247'),(34,11,'232'),(35,1,'18'),(36,3,'184'),(37,1,'73'),(38,3,'171'),(39,6,'161'),(40,13,'203'),(41,3,'56'),(42,8,'227'),(43,6,'139'),(44,8,'236'),(45,6,'233'),(46,11,'131'),(47,8,'25'),(48,3,'73'),(49,11,'90'),(50,13,'180'),(51,5,'160'),(52,13,'269'),(53,9,'66'),(54,2,'266'),(55,9,'37'),(56,2,'103'),(57,10,'118'),(58,7,'113'),(59,5,'99'),(60,12,'92'),(61,3,'26'),(62,6,'151'),(63,8,'56'),(64,7,'165'),(65,6,'39'),(66,9,'112'),(67,10,'11'),(68,11,'292'),(69,10,'222'),(70,11,'118'),(71,1,'231'),(72,4,'277'),(73,3,'167'),(74,4,'44'),(75,8,'211'),(76,5,'284'),(77,7,'70'),(78,10,'218'),(79,5,'159'),(80,4,'212'),(81,1,'187'),(82,3,'165'),(83,2,'267'),(84,13,'292'),(85,12,'159'),(86,13,'218'),(87,13,'89'),(88,5,'71'),(89,11,'128'),(90,13,'18'),(91,10,'70'),(92,2,'90'),(93,3,'77'),(94,8,'283'),(95,2,'132'),(96,9,'145'),(97,2,'287'),(98,13,'132'),(99,10,'59'),(100,7,'151'),(101,2,'166'),(102,8,'166'),(103,10,'226'),(104,9,'116'),(105,11,'290'),(106,1,'55'),(107,11,'271'),(108,5,'16'),(109,12,'54'),(110,9,'69'),(111,3,'62'),(112,4,'22'),(113,12,'206'),(114,5,'5'),(115,10,'90'),(116,9,'250'),(117,12,'286'),(118,3,'7'),(119,4,'69'),(120,12,'91'),(121,10,'222'),(122,6,'158'),(123,7,'245'),(124,3,'255'),(125,5,'211'),(126,10,'212'),(127,2,'273'),(128,4,'208'),(129,8,'73'),(130,7,'58'),(131,4,'25'),(132,10,'136'),(133,2,'65'),(134,5,'5'),(135,8,'269'),(136,5,'227'),(137,1,'165'),(138,4,'229'),(139,2,'40'),(140,2,'106'),(141,12,'215'),(142,9,'187'),(143,12,'46'),(144,6,'159'),(145,8,'195'),(146,9,'94'),(147,8,'23'),(148,3,'188'),(149,10,'113'),(150,2,'136'),(151,3,'13'),(152,5,'110'),(153,8,'235'),(154,10,'172'),(155,1,'129'),(156,10,'29'),(157,6,'273'),(158,4,'286'),(159,10,'18'),(160,10,'252'),(161,10,'228'),(162,12,'132'),(163,1,'102'),(164,6,'29'),(165,7,'50'),(166,3,'180'),(167,1,'90'),(168,12,'284'),(169,2,'292'),(170,2,'255'),(171,7,'9'),(172,1,'204'),(173,11,'182'),(174,5,'156'),(175,2,'84'),(176,13,'234'),(177,7,'284'),(178,4,'30'),(179,9,'291'),(180,2,'227'),(181,9,'132'),(182,4,'227'),(183,6,'224'),(184,11,'210'),(185,9,'19'),(186,11,'271'),(187,11,'37'),(188,11,'231'),(189,2,'147'),(190,13,'82'),(191,6,'132'),(192,8,'196'),(193,8,'64'),(194,1,'257'),(195,8,'84'),(196,7,'110'),(197,2,'236'),(198,2,'174'),(199,2,'60'),(200,12,'110'),(201,12,'42'),(202,7,'22'),(203,8,'84'),(204,12,'282'),(205,11,'240'),(206,11,'100'),(207,13,'71'),(208,2,'236'),(209,1,'153'),(210,7,'188'),(211,12,'18'),(212,7,'142'),(213,8,'54'),(214,5,'203'),(215,2,'137'),(216,13,'253'),(217,5,'38'),(218,12,'139'),(219,9,'128'),(220,12,'192'),(221,13,'61'),(222,8,'251'),(223,6,'209'),(224,12,'52'),(225,1,'224'),(226,5,'237'),(227,10,'197'),(228,3,'76'),(229,5,'177'),(230,1,'267'),(231,2,'165'),(232,5,'151'),(233,6,'107'),(234,6,'53'),(235,12,'29'),(236,13,'94'),(237,9,'149'),(238,3,'127'),(239,7,'281'),(240,6,'9'),(241,3,'144'),(242,5,'189'),(243,5,'262'),(244,5,'100'),(245,3,'91'),(246,9,'3'),(247,12,'68'),(248,8,'199'),(249,6,'300'),(250,6,'23'),(251,13,'11'),(252,10,'32'),(253,13,'186'),(254,1,'150'),(255,7,'117'),(256,4,'214'),(257,12,'3'),(258,12,'66'),(259,2,'129'),(260,5,'277'),(261,6,'83'),(262,3,'40'),(263,13,'118'),(264,1,'197'),(265,8,'146'),(266,12,'212'),(267,3,'118'),(268,11,'47'),(269,11,'284'),(270,11,'220'),(271,11,'211'),(272,7,'42'),(273,13,'273'),(274,12,'239'),(275,13,'77'),(276,13,'250'),(277,9,'280'),(278,2,'80'),(279,4,'191'),(280,5,'97'),(281,6,'36'),(282,5,'89'),(283,9,'104'),(284,6,'84'),(285,13,'142'),(286,12,'17'),(287,9,'221'),(288,8,'125'),(289,13,'38'),(290,9,'222'),(291,8,'131'),(292,3,'81'),(293,9,'266'),(294,4,'280'),(295,5,'95'),(296,12,'174'),(297,1,'196'),(298,1,'6'),(299,1,'207'),(300,10,'150'),(301,3,'90'),(302,6,'5'),(303,2,'166'),(304,1,'42'),(305,4,'256'),(306,7,'49'),(307,1,'157'),(308,4,'206'),(309,12,'283'),(310,1,'67'),(311,9,'44'),(312,3,'56'),(313,11,'289'),(314,10,'32'),(315,2,'105'),(316,13,'31'),(317,3,'33'),(318,7,'192'),(319,12,'44'),(320,7,'33'),(321,7,'19'),(322,7,'70'),(323,5,'61'),(324,10,'75'),(325,9,'207'),(326,4,'68'),(327,6,'138'),(328,6,'150'),(329,10,'19'),(330,3,'256'),(331,7,'165'),(332,4,'207'),(333,1,'26'),(334,12,'45'),(335,6,'245'),(336,4,'264'),(337,2,'281'),(338,7,'191'),(339,2,'245'),(340,7,'238'),(341,12,'199'),(342,2,'228'),(343,13,'208'),(344,3,'76'),(345,1,'111'),(346,8,'284'),(347,4,'224'),(348,7,'180'),(349,10,'63'),(350,6,'83');
/*!40000 ALTER TABLE `itemsaddressing` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `itemsstocktaking`
--

DROP TABLE IF EXISTS `itemsstocktaking`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `itemsstocktaking` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `ItemId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `InventoryStartId` int NOT NULL,
  `ItemCountRealized` tinyint(1) NOT NULL,
  `ItemCountEnded` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ItemsStockTaking_InventoryStartId` (`InventoryStartId`),
  KEY `IX_ItemsStockTaking_ItemId` (`ItemId`),
  CONSTRAINT `FK_ItemsStockTaking_InventoryStart_InventoryStartId` FOREIGN KEY (`InventoryStartId`) REFERENCES `inventorystart` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ItemsStockTaking_Item_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `item` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `itemsstocktaking`
--

LOCK TABLES `itemsstocktaking` WRITE;
/*!40000 ALTER TABLE `itemsstocktaking` DISABLE KEYS */;
/*!40000 ALTER TABLE `itemsstocktaking` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `stocktaking`
--

DROP TABLE IF EXISTS `stocktaking`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `stocktaking` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StockTakingDate` datetime(6) DEFAULT NULL,
  `ItemId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `AddressingsInventoryStartId` int NOT NULL,
  `StockTakingQuantity` decimal(65,30) NOT NULL,
  `FabricationDate` datetime(6) DEFAULT NULL,
  `ExpirationDate` datetime(6) DEFAULT NULL,
  `ItemBatch` varchar(30) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `StockTakingObservation` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `NumberOfCount` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_StockTaking_AddressingsInventoryStartId` (`AddressingsInventoryStartId`),
  KEY `IX_StockTaking_ItemId` (`ItemId`),
  CONSTRAINT `FK_StockTaking_AddressingsInventoryStart_AddressingsInventorySt~` FOREIGN KEY (`AddressingsInventoryStartId`) REFERENCES `addressingsinventorystart` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_StockTaking_Item_ItemId` FOREIGN KEY (`ItemId`) REFERENCES `item` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `stocktaking`
--

LOCK TABLES `stocktaking` WRITE;
/*!40000 ALTER TABLE `stocktaking` DISABLE KEYS */;
/*!40000 ALTER TABLE `stocktaking` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `warehouse`
--

DROP TABLE IF EXISTS `warehouse`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `warehouse` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `warehouse`
--

LOCK TABLES `warehouse` WRITE;
/*!40000 ALTER TABLE `warehouse` DISABLE KEYS */;
INSERT INTO `warehouse` VALUES (1,'Ratke LLC'),(2,'Lubowitz, Wisoky and Heaney'),(3,'Walsh, Labadie and Emard'),(4,'Kautzer Inc'),(5,'Sanford, Doyle and Hudson'),(6,'Dickinson, Daniel and Kassulke'),(7,'King, Considine and Kertzmann'),(8,'McDermott and Sons'),(9,'Simonis-Prohaska'),(10,'Weimann-Stroman');
/*!40000 ALTER TABLE `warehouse` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-06-22 11:09:25
