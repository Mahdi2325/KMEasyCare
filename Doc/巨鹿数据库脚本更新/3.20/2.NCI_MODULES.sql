/*
Navicat MySQL Data Transfer

Source Server         : sltc_dc01
Source Server Version : 50630
Source Host           : 192.168.1.106:3306
Source Database       : JLSLTC

Target Server Type    : MYSQL
Target Server Version : 50630
File Encoding         : 65001

Date: 2017-03-30 16:25:58
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `NCI_MODULES`
-- ----------------------------
DROP TABLE IF EXISTS `NCI_MODULES`;
CREATE TABLE `NCI_MODULES` (
  `MODULEID` varchar(20) COLLATE utf8_bin NOT NULL,
  `MODULENAME` varchar(50) COLLATE utf8_bin DEFAULT NULL,
  `URL` varchar(256) COLLATE utf8_bin DEFAULT NULL,
  `DESCRIPTION` varchar(256) COLLATE utf8_bin DEFAULT NULL,
  `SUPERMODULEID` varchar(40) COLLATE utf8_bin DEFAULT NULL,
  `TARGET` varchar(50) COLLATE utf8_bin DEFAULT NULL,
  `ICON` varchar(200) COLLATE utf8_bin DEFAULT NULL,
  `ROOTORDER` int(11) DEFAULT NULL,
  `ISSYS` tinyint(1) DEFAULT NULL,
  `CREATEDATE` datetime DEFAULT NULL,
  `CREATEBY` varchar(10) COLLATE utf8_bin DEFAULT NULL,
  `STATUS` tinyint(1) DEFAULT NULL,
  `SYSTYPE` varchar(10) COLLATE utf8_bin DEFAULT NULL,
  PRIMARY KEY (`MODULEID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

-- ----------------------------
-- Records of NCI_MODULES
-- ----------------------------
INSERT INTO `NCI_MODULES` VALUES ('01', '护理险资格申报', ' ', '', '00', '2', '/Images/menuIcons/applicant.png', '1', null, null, '', '1', 'N');
INSERT INTO `NCI_MODULES` VALUES ('01001', '申请人信息录入', '/NCIA/applicant', '', '01', '2', '', '1', null, null, '', '1', 'N');
INSERT INTO `NCI_MODULES` VALUES ('01002', '护理险资格申请', '/NCIA/appcertList', '', '01', '2', '', '2', null, null, '', '1', 'N');
INSERT INTO `NCI_MODULES` VALUES ('01003', '护理险入院申请', '/NCIA/appHosp', '', '01', '2', '', '4', null, null, '', '1', 'N');
INSERT INTO `NCI_MODULES` VALUES ('02', '护理险资格审核', ' ', '', '00', '2', '/Images/menuIcons/auditAppcertList.png', '1', null, null, '', '1', 'N');
INSERT INTO `NCI_MODULES` VALUES ('02001', '护理险资格审核', '/NCIA/auditAppcertList', '', '02', '2', '', '3', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('02002', '护理险入院审核', '/NCIA/auditAppHosp', '', '02', '2', '', '5', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('02003', '护理险资格年审', '/NCIA/AuditYearCert', null, '02', '2', '', '10', null, null, null, '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('03', '费用管理', '/NCIP/monFeeList', '', '00', '2', '/Images/menuIcons/monFeeList.png', '6', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('03001', '月度费用管理', '/NCIP/monFeeList', '', '03', '2', '', '6', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('03002', '拨款管理', '/NCIP/payGrantList', '', '03', '2', '', '7', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('03003', '巡检扣款管理', '/NCIP/DeducTion', null, '03', '2', null, null, null, null, null, '0', 'A');
INSERT INTO `NCI_MODULES` VALUES ('04', '保障金管理', '/NCIP/ServiceDepositList', '', '00', '2', '/Images/menuIcons/ServiceDepositList.png', '8', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('04001', '服务保障金发放', '/NCIP/ServiceDepositList', '', '04', '2', '/Images/menuIcons/ServiceDepositList.png', '8', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('04002', '服务保障金发放记录', '/NCIP/ServiceDepositGrantList', '', '04', '2', '/Images/menuIcons/ServiceDepositGrantList.png', '9', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('05', '统计分析查询', '/NCIA/ComStatistics', '', '00', '2', '/Images/menuIcons/ComStatistics.png', '13', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('05001', '综合统计', '/NCIA/ComStatistics', '', '05', '2', '', '13', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('05002', '各定点机构统计', '/NCIA/OrgStatistics', '', '05', '2', '', '14', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('05003', '费用申报统计', '/NCIA/MonthFeeStatistics', '', '05', '2', '', '1', null, null, '', '1', 'N');
INSERT INTO `NCI_MODULES` VALUES ('05004', '人员状态查询', '/NCIP/RegInHosStatusList', null, '05', '2', '', '16', null, null, null, '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('06', '报表管理', '/NCIP/ExportReport', '', '00', '2', '/Images/menuIcons/ExportReport.png', '15', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('06001', '综合统计报表', '/NCIP/reportTempManage', '', '06', '2', '', '15', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('06002', '人员状态统计报表', '/NCIP/reportPersonStatus', null, '06', '2', null, '6', null, null, null, '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('08', '系统管理', '/NCIA/UserList', '', '00', '2', '/Images/menuIcons/civilWork.png', '11', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('08001', '用户管理', '/NCIA/UserList', '', '08', '2', '', '11', null, null, '', '1', 'A');
INSERT INTO `NCI_MODULES` VALUES ('08002', '角色管理', '/NCIA/RoleList', '', '08', '2', '', '12', null, null, '', '1', 'A');
