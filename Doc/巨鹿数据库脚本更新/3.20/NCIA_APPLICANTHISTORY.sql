/*
Navicat MySQL Data Transfer

Source Server         : sltc_dc01
Source Server Version : 50630
Source Host           : 192.168.1.106:3306
Source Database       : KMECare

Target Server Type    : MYSQL
Target Server Version : 50630
File Encoding         : 65001

Date: 2017-03-29 09:50:41
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `NCIA_APPLICANTHISTORY`
-- ----------------------------
DROP TABLE IF EXISTS `NCIA_APPLICANTHISTORY`;
CREATE TABLE `NCIA_APPLICANTHISTORY` (
  `APPLICANTID` varchar(32) COLLATE utf8_bin NOT NULL,
  `NSID` varchar(32) COLLATE utf8_bin NOT NULL,
  `IDNO` varchar(18) COLLATE utf8_bin NOT NULL COMMENT '身份证号',
  `NAME` varchar(32) COLLATE utf8_bin NOT NULL COMMENT '姓名',
  `GENDER` varchar(10) COLLATE utf8_bin NOT NULL COMMENT '性别',
  `BIRTHDATE` date NOT NULL COMMENT '出生日期',
  `SSNO` varchar(50) COLLATE utf8_bin NOT NULL COMMENT '社会保障号',
  `RESIDENCE` varchar(100) COLLATE utf8_bin DEFAULT NULL COMMENT '户籍或工作单位',
  `MCTYPE` int(11) DEFAULT NULL COMMENT '人员身份(医保类型)',
  `DISEASE` varchar(200) COLLATE utf8_bin DEFAULT NULL COMMENT '病种',
  `DISEASEDESC` varchar(500) COLLATE utf8_bin DEFAULT NULL,
  `ADDRESS` varchar(100) COLLATE utf8_bin NOT NULL COMMENT '现住址',
  `PHONE` varchar(50) COLLATE utf8_bin NOT NULL COMMENT '联系电话',
  `FAMILYMEMBERNAME` varchar(50) COLLATE utf8_bin NOT NULL COMMENT '患者家属',
  `FAMILYMEMBERPHONE` varchar(50) COLLATE utf8_bin NOT NULL COMMENT '家属联系电话',
  `OCCUPATION` varchar(32) COLLATE utf8_bin DEFAULT NULL COMMENT '曾从事过职业',
  `NATIVEPLACE` varchar(50) COLLATE utf8_bin DEFAULT NULL COMMENT '籍贯',
  `MARITALSTATUS` varchar(10) COLLATE utf8_bin DEFAULT NULL COMMENT '婚姻状况',
  `ECONOMYSTATUS` varchar(32) COLLATE utf8_bin DEFAULT NULL COMMENT '目前经济状况',
  `LIVECONDITION` varchar(50) COLLATE utf8_bin DEFAULT NULL COMMENT '居住情况',
  `HOUSINGPROPERTY` varchar(50) COLLATE utf8_bin DEFAULT NULL COMMENT '住房性质',
  `HABITHOS` varchar(100) COLLATE utf8_bin DEFAULT NULL COMMENT '习惯就诊医院',
  `ZIP` int(11) DEFAULT NULL COMMENT '邮编',
  `FAMILYMEMBERRELATIONSHIP` varchar(20) COLLATE utf8_bin DEFAULT NULL COMMENT '与申请人关系',
  `LASTCERTRESULT` int(11) DEFAULT NULL COMMENT '上次申请资格证书结果',
  `LASTCERTDATE` date DEFAULT NULL COMMENT '上次申请资格证书结果日期',
  `LASTHOSPRESULT` int(11) DEFAULT NULL COMMENT '上次申请住院审批结果',
  `LASTHOSPDATE` date DEFAULT NULL COMMENT '上次申请住院审批结果日期',
  `CREATEBY` varchar(32) COLLATE utf8_bin DEFAULT NULL,
  `CREATETIME` datetime DEFAULT NULL,
  `REMARK` varchar(100) COLLATE utf8_bin DEFAULT NULL COMMENT '修改原因备注',
  `ISDELETE` tinyint(1) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_bin COMMENT='申请人(参保人)基本信息表\r\n在定点服务机构、承办保险机构、经办服务机构作为共享资料备份表';

-- ----------------------------
-- Records of NCIA_APPLICANTHISTORY
-- ----------------------------
