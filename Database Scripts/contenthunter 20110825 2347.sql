-- MySQL Administrator dump 1.4
--
-- ------------------------------------------------------
-- Server version	5.1.37-community-log


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
-- Definition of table `instructions`
--

DROP TABLE IF EXISTS `instructions`;
CREATE TABLE `instructions` (
  `Id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Url` varchar(200) CHARACTER SET latin1 NOT NULL,
  `Type` tinyint(3) unsigned NOT NULL,
  `Engine` varchar(45) CHARACTER SET latin1 NOT NULL,
  `IsRecursive` tinyint(3) unsigned NOT NULL,
  `StartedAt` datetime NOT NULL,
  `FinishedAt` datetime NOT NULL,
  `Category` varchar(50) NOT NULL,
  `IsRecurrent` tinyint(3) unsigned NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

--
-- Dumping data for table `instructions`
--

/*!40000 ALTER TABLE `instructions` DISABLE KEYS */;
INSERT INTO `instructions` (`Id`,`Url`,`Type`,`Engine`,`IsRecursive`,`StartedAt`,`FinishedAt`,`Category`,`IsRecurrent`) VALUES 
 (1,'http://leitoracompulsiva.wordpress.com/',1,'LeitoraCompulsiva',0,'0001-01-01 00:00:00','0001-01-01 00:00:00','Literatura',1);
/*!40000 ALTER TABLE `instructions` ENABLE KEYS */;




/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
