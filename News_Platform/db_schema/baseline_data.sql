/*
SQLyog Community v13.3.0 (64 bit)
MySQL - 9.2.0 : Database - NewsWebsiteDB
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

/*Data for the table `ArticleTags` */

/*Data for the table `ArticleViews` */

insert  into `ArticleViews`(`Id`,`ArticleId`,`ViewedAt`) values 
(1,6,'2025-03-30 04:03:31'),
(2,6,'2025-03-30 04:03:38'),
(3,6,'2025-03-30 04:03:40'),
(4,6,'2025-03-30 05:02:05'),
(5,6,'2025-03-30 05:02:05'),
(6,6,'2025-03-30 05:02:05'),
(7,6,'2025-03-30 05:02:05'),
(8,6,'2025-03-30 05:02:05'),
(9,6,'2025-03-30 05:02:05'),
(10,6,'2025-03-30 05:02:06'),
(11,6,'2025-03-30 05:02:06'),
(12,6,'2025-03-30 05:02:06'),
(13,6,'2025-03-30 05:02:06'),
(14,6,'2025-03-30 05:02:06'),
(15,6,'2025-03-30 05:02:06'),
(16,6,'2025-03-30 05:02:07'),
(17,6,'2025-03-30 05:02:07'),
(18,6,'2025-03-30 05:03:13'),
(19,6,'2025-03-30 05:04:27');

/*Data for the table `Articles` */

insert  into `Articles`(`ArticleID`,`Title`,`Slug`,`Content`,`AuthorID`,`CategoryID`,`ImageURL`,`Status`,`PublishedAt`,`TotalViews`,`LastViewedAt`,`CreatedAt`,`UpdatedAt`) values 
(6,'Breaking News: AI Takes Over','breaking-news-ai','Content of AI news...',1,3,'/',1,'2025-03-27 15:32:49',19,'2025-03-27 15:32:49','2025-03-27 15:32:49','2025-03-28 00:47:56'),
(7,'Latest Tech Innovations','latest-tech-innovations','Content about technology...',2,3,'/',1,'2025-03-27 15:32:49',0,'2025-03-27 15:32:49','2025-03-27 15:32:49','2025-03-28 00:47:56'),
(8,'Business Growth in 2025','business-growth-2025','Content about business...',1,2,'/',1,'2025-03-27 15:32:49',0,'2025-03-27 15:32:49','2025-03-27 15:32:49','2025-03-28 00:47:56'),
(9,'Sports Update: Champions League','sports-update-champions-league','Content about sports...',1,4,'/',1,'2025-03-27 15:32:49',190,'2025-03-27 15:32:49','2025-03-27 15:32:49','2025-03-30 05:01:36'),
(10,'The Future of Renewable Energy','future-of-renewable-energy','Content about energy...',1,7,'/',1,'2025-03-27 15:32:49',1,'2025-03-27 15:32:49','2025-03-27 15:32:49','2025-03-28 00:47:56'),
(11,'string','string','string',2,1,'string',0,NULL,0,NULL,'2025-03-28 01:45:02','2025-03-28 01:45:02');

/*Data for the table `Categories` */

insert  into `Categories`(`CategoryID`,`CategoryName`,`Description`,`CreatedAt`,`UpdatedAt`) values 
(1,'Politics','News and updates about government and policies','2025-03-27 04:57:54','2025-03-27 04:57:54'),
(2,'Business','Market trends, economy, and financial news','2025-03-27 04:57:54','2025-03-27 04:57:54'),
(3,'Technology','Latest tech news, gadgets, and innovations','2025-03-27 04:57:54','2025-03-27 04:57:54'),
(4,'Sports','Sports events, teams, and athlete updates','2025-03-27 04:57:54','2025-03-27 04:57:54'),
(5,'Entertainment','Movies, music, and celebrity news','2025-03-27 04:57:54','2025-03-27 04:57:54'),
(6,'Health','Medical news, wellness, and fitness tips','2025-03-27 04:57:54','2025-03-27 04:57:54'),
(7,'Science','Scientific discoveries and research','2025-03-27 04:57:54','2025-03-27 04:57:54'),
(8,'World News','Global events and international news','2025-03-27 04:57:54','2025-03-27 04:57:54'),
(9,'Lifestyle','Fashion, food, and travel stories','2025-03-27 04:57:54','2025-03-27 04:57:54'),
(10,'Opinion','Editorial pieces and opinion articles','2025-03-27 04:57:54','2025-03-27 04:57:54');

/*Data for the table `Comments` */

insert  into `Comments`(`CommentID`,`ArticleID`,`UserID`,`ParentCommentID`,`Content`,`Status`,`CreatedAt`,`UpdatedAt`) values 
(13,6,2,NULL,'This is an amazing article!',0,'2025-03-28 09:55:16','2025-03-28 09:55:16'),
(14,6,3,NULL,'I learned so much from this.',0,'2025-03-28 09:55:16','2025-03-28 09:55:16'),
(21,6,3,NULL,'Yes! I totally agree with you.',0,'2025-03-28 09:58:23','2025-03-28 09:58:23'),
(23,6,3,14,'Could you share more details?',0,'2025-03-28 09:58:50','2025-03-28 09:58:50'),
(24,6,3,14,'Could you share more details?',0,'2025-03-28 10:01:01','2025-03-28 10:01:01'),
(26,6,3,14,'Could you share more details?',0,'2025-03-28 10:01:34','2025-03-28 10:01:34');

/*Data for the table `EmailTemplates` */

insert  into `EmailTemplates`(`TemplateID`,`TemplateName`,`Subject`,`Template`,`CreatedAt`,`UpdatedAt`) values 
(1,'ACCOUNT_ACTIVATION_1','Welcome! Activate Your Account Now','<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <title>Activate Your Account</title>\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f4f4f4;\r\n            margin: 0;\r\n            padding: 0;\r\n        }\r\n        .container {\r\n            width: 100%;\r\n            max-width: 600px;\r\n            margin: 20px auto;\r\n            background-color: #ffffff;\r\n            padding: 20px;\r\n            border-radius: 8px;\r\n            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);\r\n        }\r\n        .header {\r\n            text-align: center;\r\n            font-size: 24px;\r\n            font-weight: bold;\r\n            color: #333;\r\n        }\r\n        .content {\r\n            font-size: 16px;\r\n            color: #555;\r\n            margin-top: 10px;\r\n        }\r\n        .activation-button {\r\n            display: block;\r\n            width: 200px;\r\n            margin: 20px auto;\r\n            text-align: center;\r\n            background-color: #007bff;\r\n            color: #ffffff;\r\n            padding: 10px;\r\n            text-decoration: none;\r\n            border-radius: 5px;\r\n            font-weight: bold;\r\n        }\r\n        .footer {\r\n            font-size: 14px;\r\n            text-align: center;\r\n            color: #777;\r\n            margin-top: 20px;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <div class=\"header\">Activate Your Account</div>\r\n        <div class=\"content\">\r\n            <p>Dear [NAME],</p>\r\n            <p>Thank you for signing up! Please click the button below to activate your account:</p>\r\n            <a href=\"[ACTIVATION_LINK]\" class=\"activation-button\">Activate Account</a>\r\n            <p>If the button above doesn’t work, you can also activate your account by clicking on the link below:</p>\r\n            <p><a href=\"[ACTIVATION_LINK]\">[ACTIVATION_LINK]</a></p>\r\n            <p>If you didn’t create this account, please ignore this email.</p>\r\n        </div>\r\n        <div class=\"footer\">\r\n            <p>Best regards,<br>News Platform Team</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n','2025-03-28 18:54:17','2025-03-29 07:17:58'),
(2,'ACCOUNT_ACTIVATION_2','Your Journalist Account is Ready – Complete Your Activation','<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <title>Activate Your Journalist Account</title>\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            background-color: #f4f4f4;\r\n            margin: 0;\r\n            padding: 0;\r\n        }\r\n        .container {\r\n            width: 100%;\r\n            max-width: 600px;\r\n            margin: 20px auto;\r\n            background-color: #ffffff;\r\n            padding: 20px;\r\n            border-radius: 8px;\r\n            box-shadow: 0px 0px 10px rgba(0, 0, 0, 0.1);\r\n        }\r\n        .header {\r\n            text-align: center;\r\n            font-size: 24px;\r\n            font-weight: bold;\r\n            color: #333;\r\n        }\r\n        .content {\r\n            font-size: 16px;\r\n            color: #555;\r\n            margin-top: 10px;\r\n        }\r\n        .activation-button {\r\n            display: block;\r\n            width: 250px;\r\n            margin: 20px auto;\r\n            text-align: center;\r\n            background-color: #007bff;\r\n            color: #ffffff;\r\n            padding: 10px;\r\n            text-decoration: none;\r\n            border-radius: 5px;\r\n            font-weight: bold;\r\n        }\r\n        .footer {\r\n            font-size: 14px;\r\n            text-align: center;\r\n            color: #777;\r\n            margin-top: 20px;\r\n        }\r\n        .temp-password {\r\n            font-weight: bold;\r\n            color: #d9534f;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <div class=\"header\">Welcome to the Team, [NAME]!</div>\r\n        <div class=\"content\">\r\n            <p>Dear [NAME],</p>\r\n            <p>Your journalist account has been created by the admin. To activate your account and set up your password, please follow these steps:</p>\r\n            \r\n            <p><strong>Step 1:</strong> Click the button below to activate your account:</p>\r\n            <a href=\"[ACTIVATION_LINK]\" class=\"activation-button\">Activate Your Account</a>\r\n\r\n            <p>If the button above doesn’t work, you can also activate your account by clicking on this link:</p>\r\n            <p><a href=\"[ACTIVATION_LINK]\">[ACTIVATION_LINK]</a></p>\r\n\r\n            <p><strong>Step 2:</strong> On the activation page, enter the temporary password provided below:</p>\r\n            <p class=\"temp-password\">[TEMPORARY_PASSWORD]</p>\r\n\r\n            <p><strong>Step 3:</strong> Choose a new password and complete the activation process.</p>\r\n\r\n            <p>If you didn’t request this account, please ignore this email.</p>\r\n        </div>\r\n        <div class=\"footer\">\r\n            <p>Best regards,<br>News Platform Team</p>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>\r\n','2025-03-29 07:16:49','2025-03-29 07:17:10');

/*Data for the table `Likes` */

insert  into `Likes`(`LikeID`,`UserID`,`ArticleID`,`CommentID`,`CreatedAt`) values 
(1,1,6,NULL,'2025-03-28 13:19:33'),
(2,2,6,NULL,'2025-03-28 13:19:33'),
(3,3,6,NULL,'2025-03-28 13:19:33'),
(4,1,NULL,13,'2025-03-28 16:12:24'),
(5,2,NULL,13,'2025-03-28 16:12:24'),
(6,3,NULL,13,'2025-03-28 16:12:24'),
(7,1,NULL,14,'2025-03-28 16:12:59');

/*Data for the table `Tags` */

/*Data for the table `UserTokens` */

insert  into `UserTokens`(`TokenID`,`UserID`,`Token`,`TokenType`,`ExpiresAt`,`CreatedAt`) values 
(8,10,'jAA/WEHdIcy9517lCUq2V/e+uCPywcQqekbAkGL1e7Y=',1,'2025-04-05 08:18:46','2025-03-29 08:18:46');

/*Data for the table `Users` */

insert  into `Users`(`UserID`,`FirstName`,`LastName`,`Email`,`PasswordHash`,`Role`,`Status`,`CreatedAt`,`UpdatedAt`) values 
(1,'Admin','1','admin@mail.com','$2a$11$PsQZuolc8wLVTZAmAmgJ/unKeZONOTittfNFcBBpiAWTE6KCdR/uq',2,1,'2025-03-27 06:11:17','2025-03-29 10:07:30'),
(2,'John','test','user@example.com','$2a$11$PsQZuolc8wLVTZAmAmgJ/unKeZONOTittfNFcBBpiAWTE6KCdR/uq',1,1,'2025-03-27 06:58:27','2025-03-29 04:31:53'),
(3,'string','string','user1@example.com','$2a$11$n3EiZBkdTIFwoFYX/rLC2u7ldEWWQHkSfqklrUh9O0Z5Wwl8eZzp2',1,1,'2025-03-27 13:41:55','2025-03-29 04:31:53'),
(10,'vernon','ng','vernonng2000@gmail.com','$2a$11$KztJmnOs1d1sElB12G1nmeMzu2XM1YuqgUEWsuU1Glkl5f5MpeZUq',1,0,'2025-03-29 08:18:45','2025-03-29 08:18:45');

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
