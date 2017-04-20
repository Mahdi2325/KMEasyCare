/*
Navicat MySQL Data Transfer

Source Server         : sltc_dc01
Source Server Version : 50630
Source Host           : 192.168.1.106:3306
Source Database       : KMECare

Target Server Type    : MYSQL
Target Server Version : 50630
File Encoding         : 65001

Date: 2017-03-28 16:40:46
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `NCIA_DEDUCTION`
-- ----------------------------
DROP TABLE IF EXISTS `NCIA_DEDUCTION`;
CREATE TABLE `NCIA_DEDUCTION` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `NSMONFEEID` bigint(20) DEFAULT NULL COMMENT '定点机构月费用ID',
  `NSNO` varchar(50) COLLATE utf8_bin NOT NULL COMMENT '定点服务机构编号',
  `RESIDENTSSID` varchar(18) COLLATE utf8_bin DEFAULT NULL COMMENT '参保人身份证',
  `DEDUCTIONTYPE` int(2) NOT NULL COMMENT '扣款类型 0:请假 1:经办机构操作',
  `DEBITMONTH` varchar(7) COLLATE utf8_bin NOT NULL COMMENT '扣款月份',
  `DEBITDAYS` int(4) DEFAULT NULL COMMENT '扣款天数',
  `AMOUNT` double(18,2) NOT NULL COMMENT '扣款金额',
  `DEDUCTIONREASON` varchar(200) COLLATE utf8_bin DEFAULT NULL COMMENT '扣款原因',
  `STATUS` int(11) DEFAULT NULL COMMENT '状态同residentMonthFee',
  `CREATEBY` varchar(10) COLLATE utf8_bin NOT NULL,
  `CREATTIME` datetime NOT NULL,
  `UPDATEBY` varchar(10) COLLATE utf8_bin DEFAULT NULL,
  `UPDATETIME` datetime DEFAULT NULL,
  `ISDELETE` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`ID`),
  KEY `PK_NCI_DEDUCTION` (`NSMONFEEID`),
  CONSTRAINT `PK_NCI_DEDUCTION` FOREIGN KEY (`NSMONFEEID`) REFERENCES `NCIP_NSMONFEE` (`NSMONFEEID`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;


