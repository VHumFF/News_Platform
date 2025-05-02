/*
SQLyog Community v13.3.0 (64 bit)
MySQL - 8.0.40 : Database - NewsWebsiteDB
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`NewsWebsiteDB` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `NewsWebsiteDB`;

/*Table structure for table `ArticleViews` */

DROP TABLE IF EXISTS `ArticleViews`;

CREATE TABLE `ArticleViews` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ArticleId` bigint NOT NULL,
  `ViewedAt` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IDX_ArticleViews_ArticleId` (`ArticleId`),
  KEY `IDX_ArticleViews_ViewedAt` (`ViewedAt`),
  CONSTRAINT `FK_ArticleViews_Articles` FOREIGN KEY (`ArticleId`) REFERENCES `Articles` (`ArticleID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=850 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Table structure for table `Articles` */

DROP TABLE IF EXISTS `Articles`;

CREATE TABLE `Articles` (
  `ArticleID` bigint NOT NULL AUTO_INCREMENT,
  `Title` varchar(255) NOT NULL,
  `Slug` varchar(255) NOT NULL,
  `Description` varchar(255) DEFAULT NULL,
  `Content` text NOT NULL,
  `AuthorID` bigint NOT NULL,
  `CategoryID` bigint NOT NULL,
  `ImageURL` varchar(255) DEFAULT NULL,
  `Status` bigint DEFAULT '0',
  `PublishedAt` datetime DEFAULT NULL,
  `TotalViews` bigint DEFAULT '0',
  `LastViewedAt` datetime DEFAULT NULL,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`ArticleID`),
  KEY `AuthorID` (`AuthorID`),
  KEY `CategoryID` (`CategoryID`),
  FULLTEXT KEY `Title` (`Title`,`Slug`,`Content`),
  CONSTRAINT `Articles_ibfk_1` FOREIGN KEY (`AuthorID`) REFERENCES `Users` (`UserID`) ON DELETE CASCADE,
  CONSTRAINT `Articles_ibfk_2` FOREIGN KEY (`CategoryID`) REFERENCES `Categories` (`CategoryID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=71 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Table structure for table `Categories` */

DROP TABLE IF EXISTS `Categories`;

CREATE TABLE `Categories` (
  `CategoryID` bigint NOT NULL AUTO_INCREMENT,
  `CategoryName` varchar(100) NOT NULL,
  `Description` text,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`CategoryID`),
  UNIQUE KEY `CategoryName` (`CategoryName`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Table structure for table `Comments` */

DROP TABLE IF EXISTS `Comments`;

CREATE TABLE `Comments` (
  `CommentID` bigint NOT NULL AUTO_INCREMENT,
  `ArticleID` bigint NOT NULL,
  `UserID` bigint NOT NULL,
  `ParentCommentID` bigint DEFAULT NULL,
  `Content` text NOT NULL,
  `Status` bigint DEFAULT '0',
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`CommentID`),
  KEY `ArticleID` (`ArticleID`),
  KEY `UserID` (`UserID`),
  KEY `ParentCommentID` (`ParentCommentID`),
  CONSTRAINT `Comments_ibfk_1` FOREIGN KEY (`ArticleID`) REFERENCES `Articles` (`ArticleID`) ON DELETE CASCADE,
  CONSTRAINT `Comments_ibfk_2` FOREIGN KEY (`UserID`) REFERENCES `Users` (`UserID`) ON DELETE CASCADE,
  CONSTRAINT `Comments_ibfk_3` FOREIGN KEY (`ParentCommentID`) REFERENCES `Comments` (`CommentID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=61 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Table structure for table `EmailTemplates` */

DROP TABLE IF EXISTS `EmailTemplates`;

CREATE TABLE `EmailTemplates` (
  `TemplateID` int NOT NULL AUTO_INCREMENT,
  `TemplateName` varchar(255) NOT NULL,
  `Subject` varchar(100) DEFAULT NULL,
  `Template` text NOT NULL,
  `CreatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`TemplateID`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Table structure for table `Likes` */

DROP TABLE IF EXISTS `Likes`;

CREATE TABLE `Likes` (
  `LikeID` bigint NOT NULL AUTO_INCREMENT,
  `UserID` bigint NOT NULL,
  `ArticleID` bigint DEFAULT NULL,
  `CommentID` bigint DEFAULT NULL,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`LikeID`),
  KEY `UserID` (`UserID`),
  KEY `ArticleID` (`ArticleID`),
  KEY `CommentID` (`CommentID`),
  CONSTRAINT `Likes_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `Users` (`UserID`) ON DELETE CASCADE,
  CONSTRAINT `Likes_ibfk_2` FOREIGN KEY (`ArticleID`) REFERENCES `Articles` (`ArticleID`) ON DELETE CASCADE,
  CONSTRAINT `Likes_ibfk_3` FOREIGN KEY (`CommentID`) REFERENCES `Comments` (`CommentID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Table structure for table `UserTokens` */

DROP TABLE IF EXISTS `UserTokens`;

CREATE TABLE `UserTokens` (
  `TokenID` bigint NOT NULL AUTO_INCREMENT,
  `UserID` bigint NOT NULL,
  `Token` varchar(255) NOT NULL,
  `TokenType` bigint NOT NULL,
  `ExpiresAt` datetime NOT NULL,
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`TokenID`),
  KEY `UserID` (`UserID`),
  CONSTRAINT `UserTokens_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `Users` (`UserID`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*Table structure for table `Users` */

DROP TABLE IF EXISTS `Users`;

CREATE TABLE `Users` (
  `UserID` bigint NOT NULL AUTO_INCREMENT,
  `FirstName` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Role` bigint NOT NULL DEFAULT '1',
  `Status` bigint NOT NULL DEFAULT '0',
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `UpdatedAt` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserID`),
  UNIQUE KEY `Email` (`Email`),
  UNIQUE KEY `idx_users_email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
