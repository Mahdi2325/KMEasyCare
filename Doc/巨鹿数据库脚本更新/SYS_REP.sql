/*
Navicat MySQL Data Transfer

Source Server         : sltc_dc01
Source Server Version : 50630
Source Host           : 192.168.1.106:3306
Source Database       : KMECare

Target Server Type    : MYSQL
Target Server Version : 50630
File Encoding         : 65001

Date: 2017-03-16 11:25:55
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `SYS_REP`
-- ----------------------------
DROP TABLE IF EXISTS `SYS_REP`;
CREATE TABLE `SYS_REP` (
  `REPID` int(11) NOT NULL AUTO_INCREMENT,
  `REPCODE` varchar(20) COLLATE utf8_bin DEFAULT NULL,
  `REPNAME` varchar(50) COLLATE utf8_bin DEFAULT NULL,
  `REPDESC` varchar(200) COLLATE utf8_bin DEFAULT NULL,
  `ORGID` varchar(10) COLLATE utf8_bin DEFAULT NULL,
  PRIMARY KEY (`REPID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Records of SYS_REP
-- ----------------------------
INSERT INTO `SYS_REP` VALUES ('1', 'H10', '住民清单', null, null);

-- ----------------------------
-- Table structure for `SYS_REPCOL`
-- ----------------------------
DROP TABLE IF EXISTS `SYS_REPCOL`;
CREATE TABLE `SYS_REPCOL` (
  `COLID` int(11) NOT NULL AUTO_INCREMENT,
  `REPID` int(11) DEFAULT NULL,
  `COLVALUE` varchar(40) COLLATE utf8_bin DEFAULT NULL,
  `COLNAME` varchar(50) COLLATE utf8_bin DEFAULT NULL,
  `ISENABLE` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`COLID`),
  KEY `sys_repcol_ibfk_1` (`REPID`),
  CONSTRAINT `sys_repcol_ibfk_1` FOREIGN KEY (`REPID`) REFERENCES `SYS_REP` (`REPID`)
) ENGINE=InnoDB AUTO_INCREMENT=22 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Records of SYS_REPCOL
-- ----------------------------
INSERT INTO `SYS_REPCOL` VALUES ('1', '1', 'Name', '個案姓名', '1');
INSERT INTO `SYS_REPCOL` VALUES ('2', '1', 'IdNo', '身分證字號', '1');
INSERT INTO `SYS_REPCOL` VALUES ('3', '1', 'BirthYear', '出生年', '1');
INSERT INTO `SYS_REPCOL` VALUES ('4', '1', 'BirthMonth', '出生月', '1');
INSERT INTO `SYS_REPCOL` VALUES ('5', '1', 'BirthDay', '出生日', '1');
INSERT INTO `SYS_REPCOL` VALUES ('6', '1', 'Age', '年齡', '1');
INSERT INTO `SYS_REPCOL` VALUES ('7', '1', 'FloorName', '居住區域', '1');
INSERT INTO `SYS_REPCOL` VALUES ('8', '1', 'RoomName', '居住寢室', '1');
INSERT INTO `SYS_REPCOL` VALUES ('9', '1', 'BedNo', '居住床號', '1');
INSERT INTO `SYS_REPCOL` VALUES ('10', '1', 'InDate', '住院日期', '1');
INSERT INTO `SYS_REPCOL` VALUES ('11', '1', 'IpdCause', '住院原因', '1');
INSERT INTO `SYS_REPCOL` VALUES ('12', '1', 'OutDate', '出院日期', '1');
INSERT INTO `SYS_REPCOL` VALUES ('13', '1', 'OutFlag', '已出院', '1');
INSERT INTO `SYS_REPCOL` VALUES ('14', '1', 'IpdDays', '住院天數', '1');
INSERT INTO `SYS_REPCOL` VALUES ('15', '1', 'MrSummary', '病歷摘要', '1');
INSERT INTO `SYS_REPCOL` VALUES ('16', '1', 'OutReason', '出院原因', '1');
INSERT INTO `SYS_REPCOL` VALUES ('17', '1', 'EscortPeople', '陪同住院者', '1');
INSERT INTO `SYS_REPCOL` VALUES ('18', '1', 'EscortRelation', '陪同住院者關係', '1');
INSERT INTO `SYS_REPCOL` VALUES ('19', '1', 'DepositDesc', '保證金狀況', '1');
INSERT INTO `SYS_REPCOL` VALUES ('20', '1', 'DepositAmt', '保證金金額', '1');
INSERT INTO `SYS_REPCOL` VALUES ('21', '1', 'HospName', '醫院名稱', '1');
