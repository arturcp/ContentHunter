-- MySQL Administrator dump 1.4
--
-- ------------------------------------------------------
-- Server version	5.5.14


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


--
-- Create schema contenthunter
--

CREATE DATABASE IF NOT EXISTS contenthunter;
USE contenthunter;

--
-- Definition of table `crawlerresults`
--

DROP TABLE IF EXISTS `crawlerresults`;
CREATE TABLE `crawlerresults` (
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Title` varchar(300) NOT NULL,
  `Content` longtext NOT NULL,
  `Message` varchar(45) NOT NULL,
  `ErrorCode` tinyint(3) unsigned NOT NULL,
  `ErrorMessage` varchar(100) NOT NULL,
  `Url` varchar(200) NOT NULL,
  `Tags` varchar(500) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=291 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `crawlerresults`
--

/*!40000 ALTER TABLE `crawlerresults` DISABLE KEYS */;
/*!40000 ALTER TABLE `crawlerresults` ENABLE KEYS */;


--
-- Definition of table `instructions`
--

DROP TABLE IF EXISTS `instructions`;
CREATE TABLE `instructions` (
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Url` varchar(200) CHARACTER SET latin1 NOT NULL,
  `Type` tinyint(3) unsigned NOT NULL,
  `Engine` varchar(45) CHARACTER SET latin1 NOT NULL,
  `IsRecursive` bit(1) NOT NULL,
  `StartedAt` datetime NOT NULL,
  `FinishedAt` datetime NOT NULL,
  `Categories` varchar(500) NOT NULL,
  `IsRecurrent` bit(1) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `instructions`
--

/*!40000 ALTER TABLE `instructions` DISABLE KEYS */;
INSERT INTO `instructions` (`Id`,`Url`,`Type`,`Engine`,`IsRecursive`,`StartedAt`,`FinishedAt`,`Categories`,`IsRecurrent`) VALUES 
 (1,'http://leitoracompulsiva.wordpress.com/',1,'LeitoraCompulsiva',0x01,'2011-08-31 16:44:11','2011-08-31 16:47:36','Literatura, resenhas, livros',0x01);
/*!40000 ALTER TABLE `instructions` ENABLE KEYS */;




/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
