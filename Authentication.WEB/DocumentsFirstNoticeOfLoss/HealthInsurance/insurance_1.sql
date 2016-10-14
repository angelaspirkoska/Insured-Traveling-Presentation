-- phpMyAdmin SQL Dump
-- version 4.5.2
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Generation Time: Jul 22, 2016 at 01:58 PM
-- Server version: 5.7.9
-- PHP Version: 5.6.16

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `insurance_1`
--

-- --------------------------------------------------------

--
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
CREATE TABLE IF NOT EXISTS `client` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `FirstLastName` text COLLATE utf8_bin,
  `StreetNumber` text COLLATE utf8_bin,
  `City` text COLLATE utf8_bin,
  `PostalCode` int(20) DEFAULT NULL,
  `PhoneNumber` text COLLATE utf8_bin,
  `MailAdress` text COLLATE utf8_bin,
  `PassportNumber` text COLLATE utf8_bin,
  `EMBG` text COLLATE utf8_bin,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM AUTO_INCREMENT=24 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `client`
--

INSERT INTO `client` (`id`, `FirstLastName`, `StreetNumber`, `City`, `PostalCode`, `PhoneNumber`, `MailAdress`, `PassportNumber`, `EMBG`) VALUES
(1, 'Altaj said', 'Ruger boskovic', 'Pehcevo', 1231, '22366532', 'saltaj@gmail.com', NULL, '1234567123451'),
(2, 'asd', 'Volgogradska', 'Skopje', 1000, '123123', 'atanasovski46@gmail.com', NULL, '1234567123451'),
(3, 'Kosta Atanasovski', 'Volgogradska', 'Skopje', 1000, '+38972530184', 'atanasovski46@gmail.com', '06056056', '1234567123451'),
(4, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
(5, NULL, NULL, NULL, 0, NULL, NULL, NULL, NULL),
(6, 'A', NULL, NULL, 0, NULL, NULL, NULL, NULL),
(7, 'A', NULL, NULL, 0, NULL, NULL, NULL, NULL),
(8, 'A', NULL, NULL, 0, NULL, NULL, NULL, NULL),
(9, 'A', NULL, NULL, 0, NULL, NULL, NULL, NULL),
(10, 'A', NULL, NULL, 0, NULL, NULL, NULL, NULL),
(11, 'A', NULL, NULL, 0, NULL, NULL, NULL, NULL),
(12, 'A', NULL, NULL, 0, NULL, NULL, NULL, NULL),
(13, 'A', NULL, NULL, 0, NULL, NULL, NULL, NULL),
(14, 'Kosta Atanasovski', 'Volgogradska', 'Skopje', 1000, '+38972530184', 'atanasovski46@gmail.com', '06056056', '1234567123451'),
(15, 'Kosta Atanasovsk', 'Volgogradska', 'Skopje', 1000, '+38972530184', 'atanasovski46@gmail.com', '06056056', '1234567123451'),
(16, NULL, 'Volgogradska', 'Skopje', 1000, '+38972530184', 'atanasovski46@gmail.com', '06056056', '1234567123451'),
(17, NULL, 'Volgogradska', 'Skopje', 1000, '+38972530184', 'atanasovski46@gmail.com', '06056056', '1234567891234'),
(18, 'Kosta Atanasovski', 'Volgogradska', 'Skopje', 1000, '+38972530184', 'atanasovski46@gmail.com', '06056056', '1234567123451'),
(19, NULL, 'Volgogradska', 'Skopje', 1000, '+38972530184', 'atanasovski46@gmail.com', '06056056', '1234567123451'),
(20, 'Dance', 'Volgogradska', 'Skopje City', 324, '+38972530184', 'face.knock@yahoo.com', '32353232', '0803994450040'),
(21, 'Dance 123', 'Bardovci Road', 'Skopje City', 100, '+38972530184', 'DanceWithMe@yahoo.com', 'A0738854', '0803994450040'),
(22, 'Dance 12334', 'Bardovci Road', 'Skopje City', 1234, '+38972530184', 'face.knock@yahoo.com', '123', '0803994450040'),
(23, 'Kosta Atanasovski', 'Volgogradska', 'Skopje City', 123, '+38972530184', 'face.knock@yahoo.com', '32353232', '0803994450040');

-- --------------------------------------------------------

--
-- Table structure for table `eurolinkusers`
--

DROP TABLE IF EXISTS `eurolinkusers`;
CREATE TABLE IF NOT EXISTS `eurolinkusers` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` text COLLATE utf8_bin NOT NULL,
  `Password` text COLLATE utf8_bin NOT NULL,
  `Roles` text COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `eurolinkusers`
--

INSERT INTO `eurolinkusers` (`ID`, `Username`, `Password`, `Roles`) VALUES
(3, 'admin', 'admin', 'Admin');

-- --------------------------------------------------------

--
-- Table structure for table `patnicko`
--

DROP TABLE IF EXISTS `patnicko`;
CREATE TABLE IF NOT EXISTS `patnicko` (
  `Polisa_Broj` bigint(20) NOT NULL,
  `Kurs` double DEFAULT NULL,
  `Zemja_Na_Patuvanje` text COLLATE utf8_bin,
  `Procent_Zemja_Na_Patuvanje` double DEFAULT NULL,
  `Vid_Polisa` text COLLATE utf8_bin,
  `Fransiza` text COLLATE utf8_bin,
  `Dedactible` text COLLATE utf8_bin,
  `Popust_Fransiza` double DEFAULT NULL,
  `Ime_I_Prezime` text COLLATE utf8_bin,
  `Adresa` text COLLATE utf8_bin,
  `EMBG` text COLLATE utf8_bin,
  `P_Vozrast` double DEFAULT NULL,
  `Fransiza_Vozrast` text COLLATE utf8_bin,
  `Dedactible_Age` text COLLATE utf8_bin,
  `Broj_Pasos` text COLLATE utf8_bin,
  `Zapocnuva_Na` date DEFAULT NULL,
  `Zavrsuva_Na` date DEFAULT NULL,
  `Vazi_Denovi` bigint(20) DEFAULT NULL,
  `Popust_Denovi` double DEFAULT NULL,
  `Fransiza_Denovi` text COLLATE utf8_bin,
  `Dedactible_Period` text COLLATE utf8_bin,
  `Pat_Edno` tinyint(1) DEFAULT NULL,
  `Pat_Poveke` tinyint(1) DEFAULT NULL,
  `Vid_Poedinecno` tinyint(1) DEFAULT NULL,
  `Vid_Grupno` tinyint(1) DEFAULT NULL,
  `Vid_Familijarno` tinyint(1) DEFAULT NULL,
  `Vid_Biznis` tinyint(1) DEFAULT NULL,
  `Popust_Business` double DEFAULT NULL,
  `Popust_Familija` double DEFAULT NULL,
  `Osigurenik1_Ime_I_Prezime` text COLLATE utf8_bin,
  `Osigurenik1_EMBG` text COLLATE utf8_bin,
  `P_Vozrast1` double DEFAULT NULL,
  `Osigurenik2_Ime_I_Prezime` text COLLATE utf8_bin,
  `Osigurenik2_EMBG` text COLLATE utf8_bin,
  `P_Vozrast2` double DEFAULT NULL,
  `Osigurenik3_Ime_I_Prezime` text COLLATE utf8_bin,
  `Osigurenik3_EMBG` text COLLATE utf8_bin,
  `P_Vozrast3` double DEFAULT NULL,
  `Osigurenik4_Ime_I_Prezime` text COLLATE utf8_bin,
  `Osigurenik4_EMBG` text COLLATE utf8_bin,
  `P_Vozrast4` double DEFAULT NULL,
  `Osigurenik5_Ime_I_Prezime` text COLLATE utf8_bin,
  `Osigurenik5_EMBG` text COLLATE utf8_bin,
  `P_Vozrast5` double DEFAULT NULL,
  `Osigurenik6_Ime_I_Prezime` text COLLATE utf8_bin,
  `Osigurenik6_EMBG` text COLLATE utf8_bin,
  `P_Vozrast6` double DEFAULT NULL,
  `Grupa` int(11) DEFAULT NULL,
  `Popust_Grupa` double DEFAULT NULL,
  `Grupna_Vkupna_Premija` double DEFAULT NULL,
  `Osnovna_Premija` double DEFAULT NULL,
  `Doplatok_1` text COLLATE utf8_bin,
  `P_Doplatok_1` double DEFAULT NULL,
  `Doplatok_2` text COLLATE utf8_bin,
  `P_Doplatok_2` double DEFAULT NULL,
  `Vkupna_Premija` double DEFAULT NULL,
  `Ovlastena_Agencija` text COLLATE utf8_bin,
  `Sifra` text COLLATE utf8_bin,
  `Referent` text COLLATE utf8_bin,
  `Datum_Na_Izdavanje` date DEFAULT NULL,
  `Status` int(11) DEFAULT NULL,
  `Datum_Na_Storniranje` date DEFAULT NULL,
  PRIMARY KEY (`Polisa_Broj`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `patnicko`
--

INSERT INTO `patnicko` (`Polisa_Broj`, `Kurs`, `Zemja_Na_Patuvanje`, `Procent_Zemja_Na_Patuvanje`, `Vid_Polisa`, `Fransiza`, `Dedactible`, `Popust_Fransiza`, `Ime_I_Prezime`, `Adresa`, `EMBG`, `P_Vozrast`, `Fransiza_Vozrast`, `Dedactible_Age`, `Broj_Pasos`, `Zapocnuva_Na`, `Zavrsuva_Na`, `Vazi_Denovi`, `Popust_Denovi`, `Fransiza_Denovi`, `Dedactible_Period`, `Pat_Edno`, `Pat_Poveke`, `Vid_Poedinecno`, `Vid_Grupno`, `Vid_Familijarno`, `Vid_Biznis`, `Popust_Business`, `Popust_Familija`, `Osigurenik1_Ime_I_Prezime`, `Osigurenik1_EMBG`, `P_Vozrast1`, `Osigurenik2_Ime_I_Prezime`, `Osigurenik2_EMBG`, `P_Vozrast2`, `Osigurenik3_Ime_I_Prezime`, `Osigurenik3_EMBG`, `P_Vozrast3`, `Osigurenik4_Ime_I_Prezime`, `Osigurenik4_EMBG`, `P_Vozrast4`, `Osigurenik5_Ime_I_Prezime`, `Osigurenik5_EMBG`, `P_Vozrast5`, `Osigurenik6_Ime_I_Prezime`, `Osigurenik6_EMBG`, `P_Vozrast6`, `Grupa`, `Popust_Grupa`, `Grupna_Vkupna_Premija`, `Osnovna_Premija`, `Doplatok_1`, `P_Doplatok_1`, `Doplatok_2`, `P_Doplatok_2`, `Vkupna_Premija`, `Ovlastena_Agencija`, `Sifra`, `Referent`, `Datum_Na_Izdavanje`, `Status`, `Datum_Na_Storniranje`) VALUES
(2, 61.5, 'Europe', 0.25, 'Comfort', 'asdsafd', 'Deductible', 0.25, 'Ivan', 'Skopje', '1212993121212', 0.5, 'Fransiza', 'Deductbler', 'pasos', '2016-04-07', '2016-08-27', 15, 0.4, 'asd', 'asd', 0, 1, 1, 0, 0, 0, NULL, NULL, 'Pere klukajdrcevto', '12345678912345', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 12, 32, 123, 123, 'Za dzeparac', 1233, 'Za asdfa', 1312, 123123, 'Sava osiguruvanje', '313231', 'Stole Od Prvi', '2016-04-13', NULL, NULL),
(1223, NULL, 'Nemacka', NULL, 'Zasekogash', 'asdf', 'ASdf', NULL, NULL, NULL, '0607993450040', NULL, '23', '23', 'asd12321331', '2016-02-10', '2016-07-30', 23, NULL, NULL, NULL, 1, 0, 1, 0, 0, NULL, NULL, NULL, 'Pere Toshev', '01256546289', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Sava tabak', NULL, NULL, NULL, NULL, NULL),
(3, NULL, 'Nemacka', NULL, 'Zasekogash', 'asdf', 'ASdf', NULL, NULL, NULL, '0607993450040', NULL, '23', '23', 'asd12321331', '2016-02-10', '2016-07-30', 23, NULL, NULL, NULL, 1, 0, 1, 0, 0, NULL, NULL, NULL, 'Pere Toshev', '01256546289', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'Sava tabak', NULL, NULL, NULL, NULL, NULL),
(4, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `patnicko_vid`
--

DROP TABLE IF EXISTS `patnicko_vid`;
CREATE TABLE IF NOT EXISTS `patnicko_vid` (
  `Vid_Polisa` text COLLATE utf8_bin NOT NULL,
  `Vid` bigint(20) DEFAULT NULL,
  PRIMARY KEY (`Vid_Polisa`(15))
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `patnicko_vid`
--

INSERT INTO `patnicko_vid` (`Vid_Polisa`, `Vid`) VALUES
('Comfort', 0),
('Normal', 1),
('Visa', 2),
('VisaGr', 3),
('VisaSosedi', 4);

-- --------------------------------------------------------

--
-- Table structure for table `p_den`
--

DROP TABLE IF EXISTS `p_den`;
CREATE TABLE IF NOT EXISTS `p_den` (
  `Patuva_Denovi` text COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`Patuva_Denovi`(20))
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `p_den`
--

INSERT INTO `p_den` (`Patuva_Denovi`) VALUES
('Do 14 dena'),
('Nad 15 dena'),
('Nad 30 dena');

-- --------------------------------------------------------

--
-- Table structure for table `p_denovi`
--

DROP TABLE IF EXISTS `p_denovi`;
CREATE TABLE IF NOT EXISTS `p_denovi` (
  `I_Denovi` bigint(20) NOT NULL AUTO_INCREMENT,
  `Vid_Polisa` text COLLATE utf8_bin NOT NULL,
  `Patuva_Denovi` bigint(20) DEFAULT NULL,
  `Popust_Denovi` double DEFAULT NULL,
  PRIMARY KEY (`I_Denovi`)
) ENGINE=MyISAM AUTO_INCREMENT=16 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `p_denovi`
--

INSERT INTO `p_denovi` (`I_Denovi`, `Vid_Polisa`, `Patuva_Denovi`, `Popust_Denovi`) VALUES
(1, 'Comfort', 1, 0),
(2, 'Comfort', 15, 0.0667),
(3, 'Comfort', 30, 0.0667),
(4, 'Normal', 1, 0),
(5, 'Normal', 15, 0.0833),
(6, 'Normal', 30, 0.0833),
(7, 'Visa', 1, 0),
(8, 'Visa', 15, 0.0833),
(9, 'Visa', 30, 0.0833),
(10, 'VisaGr', 1, 0),
(11, 'VisaGr', 15, 0),
(12, 'VisaGr', 30, 0.145),
(13, 'VisaSosedi', 1, 0),
(14, 'VisaSosedi', 15, 0),
(15, 'VisaSosedi', 30, 0.145);

-- --------------------------------------------------------

--
-- Table structure for table `p_doplatoci`
--

DROP TABLE IF EXISTS `p_doplatoci`;
CREATE TABLE IF NOT EXISTS `p_doplatoci` (
  `Doplatok` text COLLATE utf8_bin NOT NULL,
  `P_Doplatok` double DEFAULT NULL,
  `Surcharge` text COLLATE utf8_bin,
  PRIMARY KEY (`Doplatok`(20))
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `p_doplatoci`
--

INSERT INTO `p_doplatoci` (`Doplatok`, `P_Doplatok`, `Surcharge`) VALUES
('Bez doplatok', 1, 'No surcharge'),
('Fizicka rabota', 1.20000004768371, 'Physical work'),
('Sportska nezgoda', 1.60000002384185, 'Sport accident');

-- --------------------------------------------------------

--
-- Table structure for table `p_familija`
--

DROP TABLE IF EXISTS `p_familija`;
CREATE TABLE IF NOT EXISTS `p_familija` (
  `I_Familija` bigint(20) NOT NULL AUTO_INCREMENT,
  `Vid_Polisa` text COLLATE utf8_bin,
  `Popust_Familija` double DEFAULT NULL,
  PRIMARY KEY (`I_Familija`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `p_familija`
--

INSERT INTO `p_familija` (`I_Familija`, `Vid_Polisa`, `Popust_Familija`) VALUES
(1, 'Comfort', 0.07),
(2, 'Normal', 0.07),
(3, 'Visa', 0.07),
(4, 'VisaGr', 0.2364),
(5, 'VisaSosedi', 0.2364);

-- --------------------------------------------------------

--
-- Table structure for table `p_fran`
--

DROP TABLE IF EXISTS `p_fran`;
CREATE TABLE IF NOT EXISTS `p_fran` (
  `Fransiza` text COLLATE utf8_bin NOT NULL,
  `Dedactible` text COLLATE utf8_bin,
  `I_Fran` int(11) DEFAULT NULL,
  PRIMARY KEY (`Fransiza`(92))
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `p_fran`
--

INSERT INTO `p_fran` (`Fransiza`, `Dedactible`, `I_Fran`) VALUES
('150 EUR', '150 EUR', 1),
('Bez fransiza', NULL, 2),
('Dopolnitelna fransiza za vremetraewe na prestoj 150 EUR', 'Additional deduction for duration of trip 150 EUR', 3),
('Vozrasna fransiza 500 EUR medicinska pomos i 5.000EUR vrakjanje posmtrni ostatoci', 'Age deductible: 500 EUR for medicine help and 5.000EUR for returning of mortal remainds', 4);

-- --------------------------------------------------------

--
-- Table structure for table `p_grupa`
--

DROP TABLE IF EXISTS `p_grupa`;
CREATE TABLE IF NOT EXISTS `p_grupa` (
  `I_Grupa` bigint(20) NOT NULL AUTO_INCREMENT,
  `Vid_Polisa` text COLLATE utf8_bin,
  `Grupa` bigint(20) DEFAULT NULL,
  `Popust_Grupa` double DEFAULT NULL,
  PRIMARY KEY (`I_Grupa`)
) ENGINE=MyISAM AUTO_INCREMENT=11 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `p_grupa`
--

INSERT INTO `p_grupa` (`I_Grupa`, `Vid_Polisa`, `Grupa`, `Popust_Grupa`) VALUES
(1, 'Comfort', 7, 0.15),
(2, 'Comfort', 40, 0.235),
(3, 'Normal', 7, 0.15),
(4, 'Normal', 40, 0.235),
(5, 'Visa', 7, 0.13),
(6, 'Visa', 40, 0.217),
(7, 'VisaGr', 7, 0.11),
(8, 'VisaGr', 40, 0.11),
(9, 'VisaSosedi', 7, 0.11),
(10, 'VisaSosedi', 40, 0.11);

-- --------------------------------------------------------

--
-- Table structure for table `p_kurs`
--

DROP TABLE IF EXISTS `p_kurs`;
CREATE TABLE IF NOT EXISTS `p_kurs` (
  `Kurs` double NOT NULL,
  PRIMARY KEY (`Kurs`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `p_kurs`
--

INSERT INTO `p_kurs` (`Kurs`) VALUES
(61.5);

-- --------------------------------------------------------

--
-- Table structure for table `p_min_premija`
--

DROP TABLE IF EXISTS `p_min_premija`;
CREATE TABLE IF NOT EXISTS `p_min_premija` (
  `I_Min` bigint(20) NOT NULL AUTO_INCREMENT,
  `Vid_Polisa` text COLLATE utf8_bin,
  `Minimalna_Premija` double DEFAULT NULL,
  PRIMARY KEY (`I_Min`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `p_min_premija`
--

INSERT INTO `p_min_premija` (`I_Min`, `Vid_Polisa`, `Minimalna_Premija`) VALUES
(1, 'Comfort', 250),
(2, 'Normal', 250),
(3, 'Visa', 200),
(4, 'VisaGr', 200),
(5, 'VisaSosedi', 200);

-- --------------------------------------------------------

--
-- Table structure for table `p_referent`
--

DROP TABLE IF EXISTS `p_referent`;
CREATE TABLE IF NOT EXISTS `p_referent` (
  `Sifra` text COLLATE utf8_bin NOT NULL,
  `Referent` text COLLATE utf8_bin,
  `Ovlastena_Agencija` text COLLATE utf8_bin,
  `Sifra_Deloven_Partner` text COLLATE utf8_bin,
  PRIMARY KEY (`Sifra`(10))
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `p_referent`
--

INSERT INTO `p_referent` (`Sifra`, `Referent`, `Ovlastena_Agencija`, `Sifra_Deloven_Partner`) VALUES
('25032', 'G.Atanasova', 'Evroins Osiguruvanje', '0140265');

-- --------------------------------------------------------

--
-- Table structure for table `p_stapki`
--

DROP TABLE IF EXISTS `p_stapki`;
CREATE TABLE IF NOT EXISTS `p_stapki` (
  `ID` bigint(20) NOT NULL AUTO_INCREMENT,
  `Vid_Polisa` text COLLATE utf8_bin,
  `Osnovna_Premija` double DEFAULT NULL,
  `Zemja_Na_Patuvanje` text COLLATE utf8_bin,
  `Procent_Zemja_Na_Patuvanje` double DEFAULT NULL,
  `Fransiza` text COLLATE utf8_bin,
  `Popust_Fransiza` double DEFAULT NULL,
  `Patuva_Denovi` text COLLATE utf8_bin,
  `Popust_Denovi` double DEFAULT NULL,
  `Vozrast` text COLLATE utf8_bin,
  `P_Vozrast` double DEFAULT NULL,
  `Grupa` text COLLATE utf8_bin,
  `Popust_Grupa` double DEFAULT NULL,
  `Familija` text COLLATE utf8_bin,
  `Popust_Familija` double DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `p_stapki`
--

INSERT INTO `p_stapki` (`ID`, `Vid_Polisa`, `Osnovna_Premija`, `Zemja_Na_Patuvanje`, `Procent_Zemja_Na_Patuvanje`, `Fransiza`, `Popust_Fransiza`, `Patuva_Denovi`, `Popust_Denovi`, `Vozrast`, `P_Vozrast`, `Grupa`, `Popust_Grupa`, `Familija`, `Popust_Familija`) VALUES
(1, 'Comfort', 1, 'Europe', 1, '0.00', 0, 'Do 14 dena', 0, 'Lica od 18 do 65 god', 0, 'Poedinecno', 0, 'Poedinecno', 0),
(2, 'Comfort', 1, 'All countries', 1.2, '0.00', 0, 'Do 14 dena', 0, 'Lica od 18 do 65 god', 0, 'Poedinecno', 0, 'Poedinecno', 0),
(3, 'Comfort', 1, 'Greece only', 0.5, '0.00', 0, 'Do 14 dena', 0, 'Lica od 18 do 65 god', 0, 'Poedinecno', 0, 'Poedinecno', 0),
(4, 'Comfort', 1, 'Greece,SR,CG,KS,CR, AL,BiH and BG only', 0.5, '0.00', 0, 'Do 14 dena', 0, 'Lica od 18 do 65 god', 0, 'Poedinecno', 0, 'Poedinecno', 0),
(5, 'Comfort', 1, 'Europe', 1, '150 EUR', 0.5, 'Do 14 dena', 0, 'Lica od 18 do 65 god', 0, 'Poedinecno', 0, 'Poedinecno', 0);

-- --------------------------------------------------------

--
-- Table structure for table `p_vozrast`
--

DROP TABLE IF EXISTS `p_vozrast`;
CREATE TABLE IF NOT EXISTS `p_vozrast` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Vozrast` double DEFAULT NULL,
  `P_Vozrast` double DEFAULT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `p_vozrast`
--

INSERT INTO `p_vozrast` (`ID`, `Vozrast`, `P_Vozrast`) VALUES
(1, 17, 0),
(2, 64, -1),
(3, 0.001, 0.4);

-- --------------------------------------------------------

--
-- Table structure for table `p_zem`
--

DROP TABLE IF EXISTS `p_zem`;
CREATE TABLE IF NOT EXISTS `p_zem` (
  `Zemja_Na_Patuvanje` text COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`Zemja_Na_Patuvanje`(40))
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `p_zem`
--

INSERT INTO `p_zem` (`Zemja_Na_Patuvanje`) VALUES
('All countries'),
('Europe'),
('Greece only'),
('Greece,SR,CG,KS,CR, AL,BiH and BG only');

-- --------------------------------------------------------

--
-- Table structure for table `p_zemja_na_patuvanje`
--

DROP TABLE IF EXISTS `p_zemja_na_patuvanje`;
CREATE TABLE IF NOT EXISTS `p_zemja_na_patuvanje` (
  `I_Zemja_Na_Patuvanje` bigint(20) NOT NULL AUTO_INCREMENT,
  `Zemja_Na_Patuvanje` text COLLATE utf8_bin,
  `Vid_Polisa` text COLLATE utf8_bin,
  `Procent_Zemja_Na_Patuvanje` double DEFAULT NULL,
  `Fransiza` text COLLATE utf8_bin,
  `Popust_Fransiza` double DEFAULT NULL,
  PRIMARY KEY (`I_Zemja_Na_Patuvanje`)
) ENGINE=MyISAM AUTO_INCREMENT=32 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `p_zemja_na_patuvanje`
--

INSERT INTO `p_zemja_na_patuvanje` (`I_Zemja_Na_Patuvanje`, `Zemja_Na_Patuvanje`, `Vid_Polisa`, `Procent_Zemja_Na_Patuvanje`, `Fransiza`, `Popust_Fransiza`) VALUES
(29, 'All countries', 'Comfort', 0.48, '150 EUR', 0),
(30, 'All countries', 'Normal', 0.42, '150 EUR', 0),
(31, 'All countries', 'Visa', 0.384, '150 EUR', 0),
(1, 'Europe', 'Comfort', 0.8, 'Bez fransiza', 0),
(2, 'Europe', 'Comfort', 0.8, '150 EUR', 0.5),
(3, 'Europe', 'Normal', 0.7, 'Bez fransiza', 0),
(4, 'Europe', 'Normal', 0.7, '150 EUR', 0.5),
(5, 'Europe', 'Visa', 0.64, 'Bez fransiza', 0),
(6, 'Europe', 'Visa', 0.64, '150 EUR', 0.5),
(7, 'All countries', 'Comfort', 0.96, 'Bez fransiza', 0),
(9, 'All countries', 'Normal', 0.84, 'Bez fransiza', 0),
(11, 'All countries', 'Visa', 0.768, 'Bez fransiza', 0),
(13, 'Greece only', 'Comfort', 0.8, 'Bez fransiza', 0),
(14, 'Greece only', 'Comfort', 0.8, '150 EUR', 0.5),
(15, 'Greece only', 'Normal', 0.7, 'Bez fransiza', 0),
(16, 'Greece only', 'Normal', 0.7, '150 EUR', 0.5),
(17, 'Greece only', 'Visa', 0.64, 'Bez fransiza', 0),
(18, 'Greece only', 'Visa', 0.64, '150 EUR', 0.5),
(19, 'Greece only', 'VisaGr', 0.4, 'Bez fransiza', 0),
(20, 'Greece only', 'VisaGr', 0.4, '150 EUR', 0.5),
(21, 'Greece,SR,CG,KS,CR, AL,BiH and BG only', 'Comfort', 0.8, 'Bez fransiza', 0),
(22, 'Greece,SR,CG,KS,CR, AL,BiH and BG only', 'Comfort', 0.8, '150 EUR', 0.5),
(23, 'Greece,SR,CG,KS,CR, AL,BiH and BG only', 'Normal', 0.7, 'Bez fransiza', 0),
(24, 'Greece,SR,CG,KS,CR, AL,BiH and BG only', 'Normal', 0.7, '150 EUR', 0.5),
(25, 'Greece,SR,CG,KS,CR, AL,BiH and BG only', 'Visa', 0.64, 'Bez fransiza', 0),
(26, 'Greece,SR,CG,KS,CR, AL,BiH and BG only', 'Visa', 0.64, '150 EUR', 0.5),
(27, 'Greece,SR,CG,KS,CR, AL,BiH and BG only', 'VisaSosedi', 0.4, 'Bez fransiza', 0),
(28, 'Greece,SR,CG,KS,CR, AL,BiH and BG only', 'VisaSosedi', 0.4, '150 EUR', 0.5);

-- --------------------------------------------------------

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` text COLLATE utf8_bin NOT NULL,
  `Password` text COLLATE utf8_bin NOT NULL,
  `Roles` text COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=MyISAM AUTO_INCREMENT=5 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

--
-- Dumping data for table `user`
--

INSERT INTO `user` (`ID`, `Username`, `Password`, `Roles`) VALUES
(1, 'admin', 'admin', 'admin'),
(2, 'user@mail.com', 'Password1!', 'client');

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
