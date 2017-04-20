
/*==============================================================*/
/* Table: CARE_PLANACTIVITY                                     */
/*==============================================================*/
create table CARE_PLANACTIVITY
(
   CA_NO                int(11) not null,
   CP_NO                int(11),
   DIAPR                varchar(200),
   PR                   varchar(254),
   ACTIVITY             varchar(254),
   F_DAY                int(11),
   F_TIMES              int(11),
   SYSACTION            varchar(60),
   primary key (CA_NO)
);

/*==============================================================*/
/* Table: CARE_PLANCHECKPOINT                                   */
/*==============================================================*/
create table CARE_PLANCHECKPOINT
(
   CC_NO                int(11) not null,
   CP_NO                int(11),
   DIAPR                varchar(200),
   TABLE_NAME           varchar(254),
   FIELD_NAME           varchar(254),
   FIELD_VALUE          varchar(254),
   COMPARE_TYPE         int(11),
   VALUE_WEIGHT         int(11),
   primary key (CC_NO)
);

/*==============================================================*/
/* Table: CARE_PLANDATA                                         */
/*==============================================================*/
create table CARE_PLANDATA
(
   CD_NO                int(11) not null,
   CP_NO                int(11),
   DIAPR                varchar(200),
   PR                   varchar(254),
   PRDATA               varchar(254),
   primary key (CD_NO)
);

/*==============================================================*/
/* Table: CARE_PLANEVAL                                         */
/*==============================================================*/
create table CARE_PLANEVAL
(
   CE_NO                int(11) not null,
   CP_NO                int(11),
   DIAPR                varchar(200),
   ASSESSVALUER         varchar(200),
   primary key (CE_NO)
);

/*==============================================================*/
/* Table: CARE_PLANFOCUS                                        */
/*==============================================================*/
create table CARE_PLANFOCUS
(
   CF_NO                int(11) not null,
   CP_NO                int(11),
   DIAPR                varchar(200),
   FOCUSPR              varchar(254),
   primary key (CF_NO)
);

/*==============================================================*/
/* Table: CARE_PLANGOAL                                         */
/*==============================================================*/
create table CARE_PLANGOAL
(
   CG_NO                int(11) not null,
   CP_NO                int(11),
   DIAPR                varchar(200),
   GOALP                varchar(300),
   primary key (CG_NO)
);

/*==============================================================*/
/* Table: CARE_PLANPROBLEM                                      */
/*==============================================================*/
create table CARE_PLANPROBLEM
(
   CP_NO                int(11) not null,
   LEVELPR              varchar(20),
   CATEGORY             varchar(20),
   DIAPR                varchar(200),
   primary key (CP_NO)
);

/*==============================================================*/
/* Table: CARE_PLANREASON                                       */
/*==============================================================*/
create table CARE_PLANREASON
(
   CR_NO                int(11) not null,
   CP_NO                int(11),
   DIAPR                varchar(200),
   CAUSEP               varchar(300),
   primary key (CR_NO)
);

/*==============================================================*/
/* Table: CARE_PLANTRAIN                                        */
/*==============================================================*/
create table CARE_PLANTRAIN
(
   CT_NO                int(11) not null,
   CP_NO                int(11),
   DIAPR                varchar(200),
   TRAINR               varchar(300),
   primary key (CT_NO)
);

/*==============================================================*/
/* Table: CHECKITEM                                             */
/*==============================================================*/
create table CHECKITEM
(
   ORGID                varchar(10) not null,
   CHECKITEMCODE        varchar(20) not null,
   CHECKITEMNAME        varchar(200),
   CHECKITEMNAMEEN      varchar(200),
   SHOWNUMBER           int(11),
   NORMALVALUE          varchar(20),
   LOWBOUND             varchar(20),
   UPBOUND              varchar(20),
   UNITNAME             varchar(20),
   LASTUPDATETIME       datetime,
   ISDELETE             tinyint(1),
   primary key (CHECKITEMCODE, ORGID)
);

/*==============================================================*/
/* Table: CHECKTEMPLATE                                         */
/*==============================================================*/
create table CHECKTEMPLATE
(
   CHECKTEMPLATECODE    varchar(20) not null,
   CHECKTEMPLATENAME    varchar(200),
   SHOWNUMBER           int(11),
   ISDELETE             tinyint(1),
   ORGID                varchar(10) not null,
   primary key (CHECKTEMPLATECODE, ORGID)
);

/*==============================================================*/
/* Table: CHECKTEMPLATEITEM                                     */
/*==============================================================*/
create table CHECKTEMPLATEITEM
(
   ID                   bigint(20) not null auto_increment,
   CHECKTEMPLATECODE    varchar(20),
   CHECKITEMCODE        varchar(20),
   ISDELETE             tinyint(1),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_ABNORMALEMOTIONREC                                 */
/*==============================================================*/
create table DC_ABNORMALEMOTIONREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNAME              varchar(20),
   RESIDENTNO           varchar(20),
   SEX                  varchar(10),
   NURSEAIDES           varchar(10),
   YEAR                 int(11),
   MONTH                int(11),
   RECORDDATE           datetime,
   DELUSION             int(11),
   VISUALILLUSION       int(11),
   MISDEEM              int(11),
   REPEATASKING         int(11),
   REPEATLANGUAGE       int(11),
   REPEATBEHAVIOR       int(11),
   VERBALATTACK         int(11),
   BODYATTACK           int(11),
   GETLOST              int(11),
   ROAM                 int(11),
   SLEEPDISORDER        int(11),
   REGNO                varchar(10),
   ORGID                varchar(10) not null,
   DELFLAG              tinyint(1),
   DELDATE              datetime,
   FORGETEAT            int(11),
   REFUSALTOEAT         int(11),
   EXPOSEDBODYPARTS     int(11),
   NOTWEARCLOTHES       int(11),
   INAPPROPRIATETOUCH   int(11),
   COLLECTION           int(11),
   IRRITABILITY         int(11),
   COMPLAIN             int(11),
   NOTCOOPERATE         int(11),
   REFUSEHYGIENE        int(11),
   NOINTEREST           int(11),
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_ASSESSVALUE                                        */
/*==============================================================*/
create table DC_ASSESSVALUE
(
   ID                   bigint(20) not null auto_increment,
   SEQNO                bigint(20),
   RECDATE              datetime,
   VALUEDESC            text,
   RECORDBY             varchar(10),
   EXECUTEBY            varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_CAREPLANACTIVITY                                   */
/*==============================================================*/
create table DC_CAREPLANACTIVITY
(
   CANO                 int(11) not null,
   CPNO                 int(11),
   DIAPR                varchar(200),
   PR                   varchar(20),
   ACTIVITY             text,
   SYSACTION            varchar(200),
   primary key (CANO)
);

/*==============================================================*/
/* Table: DC_CAREPLANDATA                                       */
/*==============================================================*/
create table DC_CAREPLANDATA
(
   CDNO                 int(11) not null,
   CPNO                 int(11),
   DIAPR                varchar(200),
   PR                   varchar(200),
   PRDATA               varchar(500),
   primary key (CDNO)
);

/*==============================================================*/
/* Table: DC_CAREPLANEVAL                                       */
/*==============================================================*/
create table DC_CAREPLANEVAL
(
   CENO                 int(11) not null,
   CPNO                 int(11),
   DIAPR                varchar(200),
   ASSESSVALUE          varchar(200),
   primary key (CENO)
);

/*==============================================================*/
/* Table: DC_CAREPLANGOAL                                       */
/*==============================================================*/
create table DC_CAREPLANGOAL
(
   CGNO                 int(11) not null,
   CPNO                 int(11),
   DIAPR                varchar(200),
   GOALP                varchar(200),
   primary key (CGNO)
);

/*==============================================================*/
/* Table: DC_CAREPLANPROBLEM                                    */
/*==============================================================*/
create table DC_CAREPLANPROBLEM
(
   CPNO                 int(11) not null,
   LEVELPR              varchar(20),
   CATEGORYTYPE         varchar(50),
   DIAPR                varchar(200),
   ORGID                varchar(10) not null,
   primary key (CPNO)
);

/*==============================================================*/
/* Table: DC_CAREPLANREASON                                     */
/*==============================================================*/
create table DC_CAREPLANREASON
(
   CRNO                 int(11) not null,
   CPNO                 int(11),
   DIAPR                varchar(200),
   CAUSEP               varchar(500),
   primary key (CRNO)
);

/*==============================================================*/
/* Table: DC_COMMDTL                                            */
/*==============================================================*/
create table DC_COMMDTL
(
   ITEMCODE             varchar(20) not null,
   ITEMTYPE             varchar(20) not null,
   ITEMNAME             varchar(200),
   DESCRIPTION          varchar(256),
   ORDERSEQ             int(11),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   primary key (ITEMCODE, ITEMTYPE)
);

/*==============================================================*/
/* Table: DC_COMMFILE                                           */
/*==============================================================*/
create table DC_COMMFILE
(
   ITEMTYPE             varchar(20) not null,
   TYPENAME             varchar(50),
   DESCRIPTION          varchar(200),
   MODIFYFLAG           char(1),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (ITEMTYPE)
);

/*==============================================================*/
/* Table: DC_DAYLIFECAREDTL                                     */
/*==============================================================*/
create table DC_DAYLIFECAREDTL
(
   SEQNO                bigint(20) not null auto_increment,
   ID                   bigint(20),
   TEA9                 varchar(200),
   SNACKTEA9            varchar(200),
   LUNCH                varchar(200),
   SOUPAMOUNT           varchar(200),
   TEA14                varchar(200),
   SNACKTEA1530         varchar(200),
   NOONBREAK            varchar(200),
   BRUSHINGTEETH        varchar(200),
   PERINEALWASHING      varchar(200),
   OTHERCLEAN           varchar(200),
   SHITAMOUNT           varchar(200),
   SHITCOLOR            varchar(50),
   SHITNATURE           varchar(50),
   URINECOLOR           varchar(50),
   TOILET               varchar(50),
   TOILETTIME           varchar(100),
   EQUIPMENT            varchar(200),
   RECORDDATE           datetime,
   DAYOFWEEK            varchar(20),
   HOLIDAYFLAG          varchar(10),
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: DC_DAYLIFECAREREC                                     */
/*==============================================================*/
create table DC_DAYLIFECAREREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                varchar(10),
   RESIDENTNO           varchar(20),
   REGNAME              varchar(20),
   SEX                  varchar(10),
   NURSEAIDES           varchar(10),
   WEEKNUMBER           int(11),
   WEEKSTARTDATE        datetime,
   CONTACTMATTERS       text,
   FAMILYMESSAGE        text,
   SOCIALWORKER         varchar(10),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   DELFLAG              tinyint(1),
   DELDATE              datetime,
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_EVALQUESTION                                       */
/*==============================================================*/
create table DC_EVALQUESTION
(
   RECORDID             bigint(20) not null auto_increment,
   ID                   bigint(20),
   FEENO                bigint(20),
   REGNO                varchar(10),
   QUESTIONID           int(11),
   QUESTIONCODE         varchar(10),
   EVALNUMBER           int(11),
   SCORE                decimal(8,2),
   EVALDATE             datetime,
   NEXTEVALDATE         datetime,
   EVALRESULT           varchar(200),
   DESCRIPTION          varchar(200),
   ORGID                varchar(10) not null,
   primary key (RECORDID)
);

/*==============================================================*/
/* Table: DC_EVALQUESTIONRESULT                                 */
/*==============================================================*/
create table DC_EVALQUESTIONRESULT
(
   ID                   bigint(20) not null auto_increment,
   RECORDID             bigint(20),
   QUESTIONID           int(11),
   MAKERID              int(11),
   MAKERVALUE           decimal(18,2),
   LIMITEDVALUEID       int(11),
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_IPDREG                                             */
/*==============================================================*/
create table DC_IPDREG
(
   FEENO                bigint(20) not null auto_increment,
   REGNO                varchar(10),
   INDATE               datetime,
   STATIONCODE          varchar(10),
   IPDFLAG              char(1),
   RESIDENTNO           varchar(20),
   SOCIALWORKER         varchar(10),
   NURSEAIDES           varchar(10),
   NURSENO              varchar(10),
   CLOSEFLAG            tinyint(1),
   CLOSEREASON          varchar(254),
   OUTDATE              datetime,
   PROVIDESERVICE       varchar(100),
   SVRCONTENT           varchar(3000),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   CHECKDATE            datetime,
   CHECKEDBY            varchar(10),
   ORGID                varchar(10) not null,
   primary key (FEENO)
);

/*==============================================================*/
/* Table: DC_LIFEHISTORY                                        */
/*==============================================================*/
create table DC_LIFEHISTORY
(
   Id                   int(11) not null auto_increment,
   FEENO                bigint(20) not null,
   NAME                 varchar(20),
   NICKNAME             varchar(20),
   RESIDENTNO           varchar(20),
   BIRTHPLACE           varchar(50),
   FAMILYENVIRONMENT    varchar(100),
   CHILDHOODEXPERIENCE  varchar(100),
   SCHOOL               varchar(100),
   PROUDDEEDS           varchar(100),
   ROMANCE              varchar(200),
   MERRYINFO            varchar(200),
   MPORTANTPEOPLE       varchar(100),
   WORKHISTORY          varchar(200),
   SERVICEHISTORY       varchar(200),
   RELIGIOUS            varchar(50),
   LIVING               varchar(100),
   POSITIVEPERSONALITY  varchar(50),
   NEGATIVEPERSONALITY  varchar(50),
   FAMILYTROUBLED       varchar(200),
   SOOTHINGEMOTION      varchar(200),
   SKILL                varchar(200),
   FAVORITEDRESS        varchar(200),
   FOODLIKE             varchar(200),
   ANIMALLIKE           varchar(200),
   HOLIDAYACTIVITY      varchar(200),
   NOTLIKETHINGS        varchar(200),
   INTERESTEDTHINGS     varchar(200),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (Id)
);

/*==============================================================*/
/* Table: DC_MULTITEAMCAREPLAN                                  */
/*==============================================================*/
create table DC_MULTITEAMCAREPLAN
(
   ID                   bigint(20) not null auto_increment,
   MAJORTYPE            varchar(20),
   QUESTIONTYPE         text,
   ACTIVITY             text,
   TRACEDESC            text,
   SEQNO                bigint(20),
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_MULTITEAMCAREPLANEVAL                              */
/*==============================================================*/
create table DC_MULTITEAMCAREPLANEVAL
(
   SEQNO                bigint(20) not null,
   FEENO                bigint(20),
   REGNO                varchar(10),
   NURSEAIDES           varchar(10),
   ECOLOGICALMAP        varchar(50),
   PERSONALHISTORY      text,
   PHYSIOLOGY           text,
   FAMILYSUPPORT        text,
   SOCIALRESOURCES      text,
   DISEASEINFO          text,
   MMSESCORE            varchar(20),
   IADLSCORE            varchar(20),
   ADLSCORE             varchar(20),
   GODSSCORE            varchar(20),
   MOOD                 varchar(500),
   PROBLEMBEHAVIOR      text,
   PSYCHOLOGY           text,
   ECONOMICCAPACITY     text,
   INTERPERSONAL        varchar(500),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   CHECKDATE            datetime,
   CHECKEDBY            varchar(10),
   EVALDATE             datetime,
   EVALNUMBER           int(11),
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: DC_MULTITEAMCAREPLANREC                               */
/*==============================================================*/
create table DC_MULTITEAMCAREPLANREC
(
   FEENO                bigint(20) not null,
   REGNO                varchar(10),
   NURSEAIDES           varchar(10),
   EVALDATE             datetime,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   CHECKDATE            datetime,
   CHECKEDBY            varchar(10),
   SEQNO                bigint(20) not null auto_increment,
   EVALNUMBER           int(11),
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: DC_NOTE                                               */
/*==============================================================*/
create table DC_NOTE
(
   NOTEID               int(11) not null auto_increment,
   ACTIONUSERCODE       varchar(10),
   NOTENAME             varchar(200),
   NOTECONTENT          varchar(500),
   SHOWNUMBER           int(11),
   ISSHOW               tinyint(4),
   ORGID                varchar(10) not null,
   primary key (NOTEID)
);

/*==============================================================*/
/* Table: DC_NSCPLACTIVITY                                      */
/*==============================================================*/
create table DC_NSCPLACTIVITY
(
   ID                   bigint(20) not null auto_increment,
   SEQNO                bigint(20),
   RECDATE              datetime,
   CPLACTIVITY          text,
   FINISHFLAG           bigint(20),
   FINISHDATE           datetime,
   UNFINISHREASON       text,
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_NSCPLGOAL                                          */
/*==============================================================*/
create table DC_NSCPLGOAL
(
   ID                   bigint(20) not null auto_increment,
   SEQNO                bigint(20),
   RECDATE              datetime,
   CPLGOAL              varchar(500),
   FINISHDATE           datetime,
   FINISHFLAG           bigint(20),
   UNFINISHREASON       varchar(200),
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_NURSEINGLIFECAREDTL                                */
/*==============================================================*/
create table DC_NURSEINGLIFECAREDTL
(
   SEQNO                bigint(20) not null auto_increment,
   ID                   bigint(20),
   ACTIVITY9            varchar(200),
   ACTIVITY11           varchar(200),
   ACTIVITY14           varchar(200),
   ACTIVITY15           varchar(200),
   ACTIVITY16           varchar(200),
   BODYTEMPERATURE      decimal(5,2),
   PULSE                int(11),
   BREATH               int(11),
   SBP                  int(11),
   DBP                  int(11),
   MEDICINE             varchar(200),
   RECORDDATE           datetime,
   DAYOFWEEK            varchar(20),
   HOLIDAYFLAG          varchar(10),
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: DC_NURSEINGLIFECAREREC                                */
/*==============================================================*/
create table DC_NURSEINGLIFECAREREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                varchar(10),
   RESIDENTNO           varchar(20),
   REGNAME              varchar(20),
   SEX                  varchar(10),
   NURSEAIDES           varchar(10),
   WEEKNUMBER           int(11),
   WEEKSTARTDATE        datetime,
   SECURITYMEASURES     text,
   ARTICLESCARRIED      varchar(100),
   MEDICATIONINSTRUCTIONS varchar(100),
   ACTIVITYSUMMARY      varchar(100),
   QUESTIONBEHAVIOR     varchar(100),
   REMARKS              text,
   NURSENO              varchar(10),
   SUPERVISOR           varchar(10),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   DELFLAG              tinyint(1),
   DELDATE              datetime,
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_NURSEINGPLANEVAL                                   */
/*==============================================================*/
create table DC_NURSEINGPLANEVAL
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                varchar(10),
   REGNAME              varchar(20),
   RESIDENTNO           varchar(20),
   SEX                  varchar(10),
   BIRTHDATE            datetime,
   FIRSTEVALDATE        datetime,
   EVALNUMBER           int(11),
   EVALDATE             datetime,
   INDATE               datetime,
   HOSPITALNAME         varchar(50),
   FAMILYSIGNATURE      varchar(20),
   DISEASEINFO          text,
   OPERATIONINFO        varchar(100),
   CHINESEDRUGFLAG      tinyint(1),
   CHINESEDRUG          varchar(200),
   WESTERNDRUGFLAG      tinyint(1),
   WESTERNDRUG          varchar(200),
   VISITHOSPITALNAME    varchar(20),
   VISITTYPE            varchar(20),
   VISITNUMBER          int(11),
   HEARTRATE            varchar(20),
   BREATHTYPE           varchar(50),
   HEIGHT               int(11),
   WEIGHT               decimal(5,2),
   WAISTLINE            decimal(5,2),
   BMI                  decimal(5,2),
   IDEALWEIGHT          decimal(5,2),
   APPETITE             varchar(20),
   FOODHABIT            varchar(200),
   FOODTYPE             varchar(200),
   EATTYPE              varchar(20),
   TEETHSTATE           varchar(50),
   GUMSSTATE            varchar(20),
   ORALMUCOSA           varchar(50),
   SWALLOWINGABILITY    varchar(50),
   MASTICATORYABILITY   varchar(50),
   STAPLEFOOD           varchar(50),
   MEAT                 varchar(50),
   VEGETABLES           varchar(50),
   SNACK                varchar(50),
   SOUP                 varchar(50),
   SKINSTATE            varchar(50),
   SKINCOLOR            varchar(50),
   EDEMA                varchar(200),
   SKININTEGRITY        varchar(50),
   SKINPART             varchar(20),
   SKINSIZE             varchar(20),
   SKINLEVEL            int(11),
   DEFECATION           varchar(50),
   SHITNUMBER           int(11),
   SHITNATURE           varchar(20),
   SHITAMOUT            varchar(20),
   ASSISTEDDEFECATION   varchar(50),
   INTESTINALPERISTALSIS varchar(100),
   MICTURITION          varchar(20),
   URINATIONNATURE      varchar(20),
   ACONURESISFLAG       tinyint(1),
   ACONURESISINFO       varchar(20),
   URINATIONDISPOSAL    varchar(50),
   RIGHTMUSCLE1         varchar(20),
   RIGHTMUSCLE2         varchar(20),
   LEFTMUSCLE1          varchar(20),
   LEFTMUSCLE2          varchar(20),
   RIGHTJOINT1          varchar(20),
   RIGHTJOINT2          char(18),
   LEFTJOINT1           varchar(20),
   LEFTJOINT2           varchar(20),
   GAIT                 varchar(20),
   ASSISTANTTOOL        varchar(50),
   ASSISTANTSECURITY    varchar(20),
   ASSISTANTSUITABILITY varchar(20),
   ACTIVITYNAME         varchar(50),
   FALLINFO             varchar(20),
   FALL1YEAR            tinyint(1),
   INJUREDFLAG          tinyint(1),
   INJUREDPART          varchar(200),
   PASTFALLINFO         varchar(200),
   INJURYDISPOSALINFO   varchar(50),
   PAINFREQ             varchar(200),
   PAINLEVEL            varchar(20),
   PAINPART             varchar(20),
   PAINNATURE           varchar(20),
   DURATIONTIME         varchar(20),
   EASEPAINMETHOD       varchar(200),
   VISUALACUITY         varchar(200),
   ASSISTANTTOOLS       varchar(200),
   LISTENINGSTATE       varchar(200),
   DELUSION             varchar(200),
   PERSONIMAGE          varchar(50),
   ATTITUDE             varchar(50),
   EMOTIONSTATE         varchar(200),
   DISTURBINGENV        varchar(50),
   SOOTHEMOTION         varchar(50),
   BEHAVIOR             varchar(200),
   COMMUNICATIONTYPE    varchar(200),
   COMMUNICATIONSKILL   varchar(50),
   PROBLEMBEHAVIOR      varchar(200),
   NEXTEVALDATE         datetime,
   INTERVALDAY          int(11),
   DELDATE              datetime,
   DELFLAG              tinyint(1),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_QUESTION                                           */
/*==============================================================*/
create table DC_QUESTION
(
   QUESTIONID           int(11) not null,
   QUESTIONNAME         varchar(200),
   SHOWNUMBER           int(11),
   QUESTIONCODE         varchar(10),
   ISSHOW               tinyint(1),
   QUESTIONDESC         varchar(200),
   ORGID                varchar(10),
   primary key (QUESTIONID)
);

/*==============================================================*/
/* Table: DC_QUESTIONITEM                                       */
/*==============================================================*/
create table DC_QUESTIONITEM
(
   ITEMID               int(11) not null,
   QUESTIONID           int(11),
   ITEMNAME             varchar(200),
   SHOWNUMBER           int(11),
   DESCRIPTION          varchar(200),
   primary key (ITEMID)
);

/*==============================================================*/
/* Table: DC_QUESTIONVALUE                                      */
/*==============================================================*/
create table DC_QUESTIONVALUE
(
   VALUEID              int(11) not null,
   QUESTIONID           int(11),
   VALUE                int(11),
   primary key (VALUEID)
);

/*==============================================================*/
/* Table: DC_REFERRALLISTS                                      */
/*==============================================================*/
create table DC_REFERRALLISTS
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   SIOLOGICALSTATE      text,
   PROBLEMSTATEMENT     text,
   REFERRALPURPOSE      varchar(50),
   DOCUMENTINFO         varchar(20),
   REFERRALDATE         datetime,
   SOCIALWORKER         varchar(10),
   SUPERVISOR           varchar(10),
   REFERRALUNIT         varchar(50),
   REFERRALRESULT       varchar(300),
   REPLYDATE            datetime,
   UNITCONTACTOR        varchar(20),
   UNITPHONE            varchar(20),
   UNITFAX              varchar(20),
   UNITNAME             varchar(200),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_REGACTIVITYREQUESTEVAL                             */
/*==============================================================*/
create table DC_REGACTIVITYREQUESTEVAL
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                varchar(10),
   INDATE               datetime,
   EVALDATE             datetime,
   INTERVALDAY          int(11),
   NEXTEVALDATE         datetime,
   REGNAME              varchar(20),
   SEX                  varchar(10),
   BIRTHDATE            datetime,
   EDUCATION            varchar(20),
   MERRYSTATE           varchar(20),
   PROFESSION           varchar(50),
   LANGUAGE             varchar(20),
   SKILL                varchar(50),
   INTERESTING          varchar(50),
   VISUALACUITY         varchar(200),
   LISTENINGSTATE       varchar(200),
   DELUSION             varchar(200),
   VISUALILLUSION       varchar(200),
   MMSESCORE            varchar(20),
   MMSEITEMRESULT       varchar(200),
   MMSERESULT           varchar(20),
   ADLSCORE             varchar(20),
   ADLITEMRESULT        varchar(200),
   ADLRESULT            varchar(20),
   IADLSCORE            varchar(20),
   IADLITEMRESULT       varchar(200),
   IADLRESULT           varchar(20),
   GDSSCORE             varchar(20),
   GDSITEMRESULT        varchar(200),
   GDSRESULT            varchar(20),
   EMOTIONSTATE         varchar(200),
   PROBLEMBEHAVIOR      varchar(200),
   INTERPERSONAL        varchar(200),
   SELF                 varchar(200),
   PROMOTION            varchar(200),
   PROMOTIONSTRATEGY    text,
   PRESERVE             varchar(200),
   PRESERVESTRATEGY     text,
   EASE                 varchar(200),
   EASESTRATEGY         text,
   DELFLAG              tinyint(1),
   DELDATE              datetime,
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_REGBASEINFOLIST                                    */
/*==============================================================*/
create table DC_REGBASEINFOLIST
(
   ID                   bigint(20) not null auto_increment,
   RECORDDATE           datetime,
   CNT                  int(11),
   FEENO                bigint(20),
   REGNAME              varchar(20),
   RESIDENTNO           varchar(20),
   CONTACTNAME          varchar(20),
   CONTACTPHONE         varchar(20),
   ADDRESS              varchar(100),
   BIRTHDATE            datetime,
   LANGUAGE             varchar(20),
   VS                   varchar(20),
   JOB                  varchar(20),
   RELIGION             varchar(20),
   MERRYSTATE           varchar(20),
   EDUCATION            varchar(20),
   HEIGHT               varchar(20),
   WEIGHT               varchar(20),
   BMI                  varchar(20),
   WAISTLINE            varchar(20),
   DISEASEHISTORY       varchar(200),
   ADLSCORE             varchar(20),
   IADLSCORE            varchar(20),
   MMSESCORE            varchar(20),
   GDSSCORE             varchar(20),
   UPPERDISORDER        varchar(20),
   LOWERDISORDER        varchar(20),
   APHASIA              varchar(20),
   VISUALLYIMPAIRED     varchar(20),
   HEARINGIMPAIRED      varchar(20),
   FALSETEETHU          varchar(20),
   FALSETEETHL          varchar(20),
   NOTEATFOOD           varchar(50),
   LIKEFOOD             varchar(50),
   QUESTIONBEHAVIOR     varchar(50),
   CHECKDATE            datetime,
   XRAY                 varchar(50),
   SYPHILIS             varchar(50),
   AIDS                 varchar(50),
   HBSAG                varchar(50),
   AMIBADYSENTERY       varchar(50),
   INSECTEGG            varchar(50),
   BACILLUSDYSENTERY    varchar(50),
   NEXTCHECKDATE        datetime,
   MEDICINE             varchar(50),
   NURSENO              varchar(10),
   SUPERVISOR           varchar(10),
   DIRECTOR             varchar(10),
   REGNO                varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_REGCASECAREPLAN                                    */
/*==============================================================*/
create table DC_REGCASECAREPLAN
(
   ID                   bigint(20) not null auto_increment,
   QUESTIONTYPE         text,
   GOAL                 text,
   ACTIVITY             text,
   MAJORTYPE            varchar(20),
   TRACEDESC            text,
   SEQNO                bigint(20),
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_REGCASECAREPLANREC                                 */
/*==============================================================*/
create table DC_REGCASECAREPLANREC
(
   SEQNO                bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                varchar(10),
   EVALDATE             datetime,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   CHECKDATE            datetime,
   CHECKEDBY            varchar(10),
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: DC_REGCHECKRECORD                                     */
/*==============================================================*/
create table DC_REGCHECKRECORD
(
   RECORDID             bigint(20) not null auto_increment,
   REGNO                varchar(10),
   CHECKDATE            datetime,
   CHECKTEMPLATECODE    varchar(20),
   ACTIONUSERCODE       varchar(20),
   ISABNORMAL           tinyint(1),
   TRACESTATUS          tinyint(1),
   TRACEDATE            datetime,
   LASTUPDATETIME       datetime,
   ISDELETE             tinyint(1),
   ORGID                varchar(10) not null,
   primary key (RECORDID)
);

/*==============================================================*/
/* Table: DC_REGCHECKRECORDDATA                                 */
/*==============================================================*/
create table DC_REGCHECKRECORDDATA
(
   DATAID               bigint(20) not null auto_increment,
   RECORDID             bigint(20),
   CHECKITEMCODE        varchar(20),
   CHECKITEMVALUE       varchar(200),
   LOWBOUND             varchar(20),
   UPBOUND              varchar(20),
   SEVERITYNAME         varchar(20),
   primary key (DATAID)
);

/*==============================================================*/
/* Table: DC_REGCPL                                             */
/*==============================================================*/
create table DC_REGCPL
(
   SEQNO                bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                varchar(10),
   EMPNO                varchar(10),
   STARTDATE            datetime,
   NEEDDAYS             int(11),
   TARGETDATE           datetime,
   MAJORTYPE            varchar(20),
   CPLEVEL              varchar(200),
   CPDIA                text,
   ID                   bigint(20),
   NSDESC               text,
   CPREASON             text,
   FINISHFLAG           tinyint(1),
   FINISHDATE           datetime,
   TOTALDAYS            int(11),
   CPRESULT             text,
   UNFINISHREASON       varchar(20),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: DC_REGDAYLIFE                                         */
/*==============================================================*/
create table DC_REGDAYLIFE
(
   Id                   int(11) not null auto_increment,
   FEENO                bigint(20) not null,
   PAST0                varchar(100),
   PAST2                varchar(100),
   PAST4                varchar(100),
   PAST6                varchar(100),
   PAST7                varchar(100),
   PAST8                varchar(100),
   PAST9                varchar(100),
   PAST10               varchar(100),
   PAST11               varchar(100),
   PAST12               varchar(100),
   PAST14               varchar(100),
   PAST15               varchar(100),
   PAST16               varchar(100),
   PAST17               varchar(100),
   PAST18               varchar(100),
   PAST19               varchar(100),
   PAST20               varchar(100),
   PAST21               varchar(100),
   PAST22               varchar(100),
   PAST24               varchar(100),
   NOW0                 varchar(100),
   NOW2                 varchar(100),
   NOW4                 varchar(100),
   NOW6                 varchar(100),
   NOW7                 varchar(100),
   NOW8                 varchar(100),
   NOW9                 varchar(100),
   NOW10                varchar(100),
   NOW11                varchar(100),
   NOW12                varchar(100),
   NOW14                varchar(100),
   NOW15                varchar(100),
   NOW16                varchar(100),
   NOW17                varchar(100),
   NOW18                varchar(100),
   NOW19                varchar(100),
   NOW20                varchar(100),
   NOW21                varchar(100),
   NOW22                varchar(100),
   NOW24                varchar(100),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   NAME                 varchar(20),
   NICKNAME             varchar(20),
   RESIDENTNO           varchar(20),
   ORGID                varchar(10) not null,
   primary key (Id)
);

/*==============================================================*/
/* Table: DC_REGEVALQUESTION                                    */
/*==============================================================*/
create table DC_REGEVALQUESTION
(
   SEQ                  int(11) not null auto_increment,
   QUESTIONID           int(11),
   EVALRECID            int(11),
   primary key (SEQ)
);

/*==============================================================*/
/* Table: DC_REGFILE                                            */
/*==============================================================*/
create table DC_REGFILE
(
   REGNO                varchar(10) not null,
   REGNAME              varchar(20),
   NICKNAME             varchar(20),
   SEX                  varchar(10),
   BIRTHPLACE           varchar(50),
   ORIGINPLACE          varchar(50),
   BIRTHDATE            datetime,
   IDNO                 varchar(20),
   PTYPE                varchar(10),
   PHONE                varchar(20),
   LIVINGADDRESS        varchar(50),
   PERMANENTADDRESS     varchar(50),
   LANGUAGE             varchar(100),
   EDUCATION            varchar(20),
   PROFESSION           varchar(50),
   MERRYSTATE           varchar(20),
   RELIGION             varchar(20),
   ECONOMICSOURCES      varchar(50),
   LIVCONDITION         varchar(50),
   SOURCETYPE           varchar(200),
   OBSTACLEMANUAL       varchar(100),
   SURETYNAME           varchar(20),
   SURETYAGE            int(11),
   SURETYUNIT           varchar(50),
   SURETYTITLE          varchar(20),
   SURETYADDRESS        varchar(50),
   SURETYEMAIL          varchar(20),
   SURETYPHONE          varchar(20),
   SURETYMOBILE         varchar(20),
   CONTACTNAME1         varchar(20),
   CONTACTAGE1          int(11),
   CONTACTUNIT1         varchar(50),
   CONTACTTITLE1        varchar(20),
   CONTACTADDRESS1      varchar(50),
   CONTACTEMAIL1        varchar(20),
   CONTACTPHONE1        varchar(20),
   CONTACTMOBILE1       varchar(20),
   CONTACTNAME2         varchar(20),
   CONTACTAGE2          int(11),
   CONTACTUNIT2         varchar(50),
   CONTACTTITLE2        varchar(20),
   CONTACTADDRESS2      varchar(50),
   CONTACTEMAIL2        varchar(20),
   CONTACTPHONE2        varchar(20),
   CONTACTMOBILE2       varchar(20),
   DISEASEINFO          varchar(300),
   ECOLOGICALMAP        varchar(50),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   CHECKDATE            char(18),
   CHECKEDBY            varchar(10),
   ORGID                varchar(10) not null,
   primary key (REGNO)
);

/*==============================================================*/
/* Table: DC_REGLIFEQUALITYEVAL                                 */
/*==============================================================*/
create table DC_REGLIFEQUALITYEVAL
(
   ID                   int(11) not null auto_increment,
   FEENO                bigint(20),
   HEALTH               varchar(20),
   ENERGY               varchar(20),
   MOOD                 varchar(20),
   LIVINGCONDITION      varchar(20),
   MEMORY               varchar(20),
   FAMILY               varchar(20),
   MERRY                varchar(20),
   FRIENDS              varchar(20),
   SELF                 varchar(20),
   FAMILYABILITY        varchar(20),
   ENTERTAINMENT        varchar(20),
   MONEY                varchar(20),
   WHOLELIFE            varchar(20),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   REGNO                varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_REGMEDICINE                                        */
/*==============================================================*/
create table DC_REGMEDICINE
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                varchar(10),
   REGNAME              varchar(20),
   SEX                  varchar(10),
   BIRTHDATE            datetime,
   RESIDENTNO           varchar(20),
   MEDICINENAME         varchar(50),
   HOSPITALNAME         varchar(50),
   DEPTNAME             varchar(50),
   TAKEQTY              varchar(50),
   TAKEPROC             varchar(50),
   TAKEDATETIME         varchar(20),
   STARTDATE            datetime,
   ENDDATE              datetime,
   BREAKDATE            datetime,
   BREAKREASON          varchar(50),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_REGNOTERECORD                                      */
/*==============================================================*/
create table DC_REGNOTERECORD
(
   RECORDID             bigint(20) not null auto_increment,
   ACTIONUSERCODE       varchar(10),
   NOTEDATE             date,
   REGNO                varchar(10),
   NOTEID               int(11),
   NOTENAME             varchar(200),
   NOTECONTENT          varchar(500),
   VIEWSTATUS           tinyint(4),
   VIEWDATE             date,
   ORGID                varchar(10),
   primary key (RECORDID)
);

/*==============================================================*/
/* Table: DC_REGNURSINGDIAG                                     */
/*==============================================================*/
create table DC_REGNURSINGDIAG
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                varchar(10),
   REGNAME              varchar(20),
   NURSEDIAG            varchar(200),
   STARTDATE            datetime,
   ENDDATE              datetime,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_REGQUESTIONDATA                                    */
/*==============================================================*/
create table DC_REGQUESTIONDATA
(
   ID                   int(11) not null auto_increment,
   ITEMID               int(11),
   DESCRIPTION          varchar(200),
   ITEMVALUE            int(11),
   SEQ                  int(11),
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_REGQUESTIONEVALREC                                 */
/*==============================================================*/
create table DC_REGQUESTIONEVALREC
(
   EVALRECID            int(11) not null auto_increment,
   SCORE                float(11,2),
   FEENO                bigint(20),
   REGNO                varchar(10),
   EVALDATE             datetime,
   EVALRESULT           varchar(100),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   CHECKDATE            datetime,
   CHECKEDBY            varchar(10),
   ORGID                varchar(10) not null,
   primary key (EVALRECID)
);

/*==============================================================*/
/* Table: DC_REGVISITRECORD                                     */
/*==============================================================*/
create table DC_REGVISITRECORD
(
   RECORDID             bigint(20) not null auto_increment,
   ACTIONUSERCODE       varchar(10),
   VISITDATE            date,
   REGNO                varchar(10),
   VISITNAME            varchar(200),
   VISITCONTENT         varchar(500),
   ORGID                varchar(10),
   primary key (RECORDID)
);

/*==============================================================*/
/* Table: DC_SWREGEVALPLAN                                      */
/*==============================================================*/
create table DC_SWREGEVALPLAN
(
   EVALPLANID           bigint(20) not null auto_increment,
   FEENO                bigint(20),
   RESIDENTNO           varchar(20),
   EVALDATE             datetime,
   EVALNUMBER           int(11),
   NEXTEVALDATE         datetime,
   INDATE               datetime,
   REGNAME              varchar(20),
   SEX                  varchar(10),
   BIRTHDATE            datetime,
   IDNO                 varchar(20),
   CONTACTNAME          varchar(20),
   CONTACTPHONE         varchar(20),
   CONTACTMOBILE        varchar(20),
   APPELLATION          varchar(20),
   LIVINGADDRESS        varchar(50),
   PTYPE                varchar(10),
   OBSTACLEMANUAL       varchar(100),
   SOURCETYPE           varchar(200),
   TAKECAREREASON       varchar(200),
   TAKECARETYPE         varchar(200),
   SERVICETYPE          varchar(50),
   DISEASEINFO          varchar(300),
   ECOLOGICALMAP        varchar(50),
   PERSONALHISTORY      text,
   PHYSIOLOGY           text,
   PSYCHOLOGY           text,
   FAMILYSUPPORT        text,
   ECONOMICCAPACITY     text,
   SOCIALRESOURCES      text,
   SOCIALRESOURCE       varchar(200),
   CURRENTSUBSIDY       varchar(200),
   ASSISTAPPLICATION    varchar(200),
   MMSE                 varchar(20),
   ADL                  varchar(20),
   IADL                 varchar(20),
   GDS                  varchar(20),
   EMOTIONSTATE         varchar(200),
   BEHAVIOR             varchar(200),
   ATTITUDE             varchar(200),
   PAYATTENTION         varchar(200),
   THOUGHT              varchar(200),
   UNDERSTANDABILITY    varchar(200),
   SOCIALABILITY        varchar(200),
   EYESIGHT             varchar(50),
   HEARING              varchar(50),
   EXPRESSION           varchar(50),
   UNDERSTANDING        varchar(50),
   FAMILYINTERACTION    varchar(50),
   RELATIVEINTERACTION  varchar(50),
   FRIENDINTERACTION    varchar(50),
   ELDERINTERACTION     varchar(50),
   ADAPTIVESTATE        varchar(50),
   LIVINGCONDITION      varchar(50),
   JOBINFO              varchar(50),
   DAYTAKECAREHOUR      varchar(50),
   RELATIVESNEEDCARE    tinyint(1),
   REPLACEMENT          tinyint(1),
   EASEPRESSURE         tinyint(1),
   LIFEQUALITY          varchar(10),
   FAMILYEXPECT         text,
   ORGID                varchar(10) not null,
   REGNO                varchar(10),
   DELFLAG              tinyint(1),
   DELDATE              datetime,
   primary key (EVALPLANID)
);

/*==============================================================*/
/* Table: DC_TASKGOALSSTRATEGY                                  */
/*==============================================================*/
create table DC_TASKGOALSSTRATEGY
(
   EVALPLANID           bigint(20) not null,
   QUESTIONTYPE         varchar(100),
   CPDIA                varchar(500),
   QUESTIONDESC         text,
   TREATMENTGOAL        text,
   QUESTIONANALYSIS     text,
   PLANACTIVITY         text,
   RECDATE              datetime,
   RECORDBY             varchar(10),
   CHECKDATE            varchar(20),
   CHECKEDBY            varchar(10),
   EVALUATIONVALUE      varchar(200),
   UNFINISHREASON       varchar(200),
   ID                   bigint(20) not null auto_increment,
   MAJORTYPE            int(11),
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_TASKREMIND                                         */
/*==============================================================*/
create table DC_TASKREMIND
(
   ID                   bigint(20) not null auto_increment,
   ASSIGNDATE           datetime,
   ASSIGNEDBY           varchar(10),
   ASSIGNEDNAME         varchar(50),
   ASSIGNEE             varchar(10),
   ASSIGNNAME           varchar(50),
   CONTENT              varchar(500),
   RECSTATUS            tinyint(1),
   FINISHDATE           datetime,
   UNFINISHREASON       varchar(500),
   PERFORMDATE          datetime,
   URL                  varchar(200),
   NEWRECFLAG           tinyint(1),
   FEENO                bigint(20),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: DC_TEAMACTIVITY                                       */
/*==============================================================*/
create table DC_TEAMACTIVITY
(
   SEQNO                int(11) not null auto_increment,
   ACTIVITYCODE         varchar(10),
   ACTIVITYNAME         varchar(200),
   ORGID                varchar(10),
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: DC_TEAMACTIVITYDTL                                    */
/*==============================================================*/
create table DC_TEAMACTIVITYDTL
(
   ID                   int(11) not null auto_increment,
   SEQNO                int(11),
   TITLENAME            varchar(100),
   ITEMNAME             varchar(200),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_ACTIVEPERIOD                                      */
/*==============================================================*/
create table LTC_ACTIVEPERIOD
(
   ORGID                varchar(10) not null,
   PERIODLENGTH         int(11),
   STARTDATE            datetime,
   ENDDATE              datetime,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   STATUS               tinyint(1),
   FEE                  decimal(14,2),
   ACTUALENDDATE        datetime,
   primary key (ORGID)
);

/*==============================================================*/
/* Table: LTC_ACTIVEPERIOD_HIST                                 */
/*==============================================================*/
create table LTC_ACTIVEPERIOD_HIST
(
   ORGID                varchar(10) not null,
   PERIODLENGTH         int(11),
   STARTDATE            datetime,
   ENDDATE              datetime,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   STATUS               tinyint(1),
   RECORDDATE           datetime,
   RECORDBY             varchar(10),
   FEE                  decimal(14,2),
   ACTUALENDDATE        datetime,
   primary key (ORGID)
);

/*==============================================================*/
/* Table: LTC_AFFAIRSHANDOVER                                   */
/*==============================================================*/
create table LTC_AFFAIRSHANDOVER
(
   ID                   int(11) not null auto_increment,
   CLASSTYPE            varchar(10),
   RECORDDATE           datetime,
   RECORDBY             varchar(10),
   CONTENT              varchar(500),
   EXECUTEDATE          datetime,
   EXECUTEBY            varchar(10),
   FINISHFLAG           tinyint(1),
   FINISHDATE           datetime,
   UNFINISHREASON       varchar(500),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   RECORDERNAME         varchar(50),
   EXECUTIVENAME        varchar(50),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_ASSESSVALUE                                       */
/*==============================================================*/
create table LTC_ASSESSVALUE
(
   ID                   bigint(20) not null auto_increment,
   VALUEDESC            text,
   RECORDBY             varchar(10),
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   EXECUTEBY            varchar(10),
   SEQNO                bigint(20),
   FEENO                bigint(20),
   REGNO                int(11),
   RECDATE              datetime,
   CREATEDATE           datetime,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_ASSIGNTASK                                        */
/*==============================================================*/
create table LTC_ASSIGNTASK
(
   ID                   int(11) not null auto_increment,
   ASSIGNDATE           datetime,
   ASSIGNEDBY           varchar(10),
   ASSIGNEDNAME         varchar(50),
   ASSIGNEE             varchar(10),
   ASSIGNNAME           varchar(50),
   CONTENT              varchar(500),
   RECSTATUS            tinyint(1),
   FINISHDATE           datetime,
   UNFINISHREASON       varchar(500),
   PERFORMDATE          datetime,
   CLASSTYPE            varchar(10),
   FEENO                bigint(20),
   URL                  varchar(200),
   AUTOFLAG             tinyint(1),
   NEWRECFLAG           tinyint(1),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_BEDBASIC                                          */
/*==============================================================*/
create table LTC_BEDBASIC
(
   BEDNO                varchar(10) not null,
   BEDDESC              varchar(200),
   ROOMNO               varchar(10),
   BEDCLASS             varchar(10),
   FLOOR                varchar(10),
   DEPTNO               varchar(10),
   BEDTYPE              varchar(10),
   BEDKIND              varchar(10),
   BEDSTATUS            varchar(10),
   SEXTYPE              varchar(10),
   PRESTATUS            varchar(10),
   INSBEDFLAG           varchar(10),
   STATUS               tinyint(1),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   ORGID                varchar(10) not null,
   FEENO                bigint(20),
   primary key (BEDNO)
);

/*==============================================================*/
/* Table: LTC_BEDSORECHGREC                                     */
/*==============================================================*/
create table LTC_BEDSORECHGREC
(
   ID                   bigint(20) not null auto_increment,
   SEQ                  bigint(20),
   ECALDATE             datetime,
   WOUNDPART            varchar(100),
   DEGREE               varchar(50),
   SIZE_L               varchar(10),
   SIZE_W               varchar(10),
   SIZE_D               varchar(10),
   WOUNDDIRECTION       varchar(10),
   WOUNDDEPTH           varchar(10),
   WOUNDCOLOR           varchar(20),
   SKINDESC             varchar(20),
   SECRETIONCOLOR       varchar(20),
   SECRETIONNATURE      varchar(20),
   SECRETIONAMT         varchar(20),
   NURSE                varchar(20),
   DRESSING             varchar(200),
   TREATEMENT           varchar(200),
   PICTURE              varchar(100),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_BEDSOREREC                                        */
/*==============================================================*/
create table LTC_BEDSOREREC
(
   SEQ                  bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   OCCURCHANCE          varchar(20),
   OCCURDATE            datetime,
   DEGREE               varchar(40),
   EVALUTEBY            varchar(10),
   INSPECTIONRESULT     varchar(50),
   REVOCERYDATE         datetime,
   EVENTREVIEW          text,
   REVOCERYFLAG         tinyint(1),
   REVOCERYDESC         varchar(40),
   PICT1                varchar(1000),
   PICT2                varchar(1000),
   PICT3                varchar(1000),
   PICT4                varchar(100),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (SEQ)
);

/*==============================================================*/
/* Table: LTC_BEDTRANSFER                                       */
/*==============================================================*/
create table LTC_BEDTRANSFER
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20) not null,
   EVENTTYPE            varchar(2),
   DEPT_O               varchar(10),
   FLOOR_O              varchar(10),
   ROOM_O               varchar(10),
   BEDNO_O              varchar(10),
   DEPT_D               varchar(10),
   FLOOR_D              varchar(10),
   ROOMNO_D             varchar(10),
   BEDNO_D              varchar(10),
   TRANDATE             datetime,
   TRANDESC             varchar(200),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_BILL                                              */
/*==============================================================*/
create table LTC_BILL
(
   ID                   bigint(20) not null auto_increment,
   BILLNO               varchar(20) not null,
   BILLTYPE             varchar(10),
   BILLDATE             datetime,
   BILLENDDATE          datetime,
   BILLSTATE            varchar(10),
   COST                 decimal(18,4),
   DESCRIPTION          varchar(200),
   CREATEDATE           datetime,
   REGNO                int(11) not null,
   FEENO                bigint(20) not null,
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_CAREDEMANDEVAL                                    */
/*==============================================================*/
create table LTC_CAREDEMANDEVAL
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   EVALDATE             datetime,
   NEXTEVALDATE         datetime,
   NEXTEVALUATEBY       varchar(10),
   COUNT                int(11),
   EVALUATEBY           varchar(10),
   HEALTHFLAG           tinyint(1),
   HEALTHDESC           text,
   INFECTIONFLAG        tinyint(1),
   INFECTIONDESC        text,
   VITALSIGNFLAG        tinyint(1),
   BODYTEMP             decimal(5,2),
   PULSE                int(11),
   BREATHE              int(11),
   SBP                  int(11),
   DBP                  int(11),
   HEIGHT               decimal(8,2),
   WEIGHT               decimal(8,2),
   CONSCIOUSNESS        varchar(50),
   CONSCIOUSNESS_E      varchar(50),
   CONSCIOUSNESS_V      varchar(50),
   CONSCIOUSNESS_M      varchar(50),
   APPEARANCEFLAG       tinyint(1),
   APPEARANCEDESC       varchar(254),
   ATTITUDE             varchar(20),
   BREATHPROBLEMFLAG    tinyint(1),
   BREATEDESC           varchar(50),
   SECRETIONDESC        varchar(50),
   SECRETIONNATURE      varchar(50),
   SECRETIONAMT         varchar(50),
   COUGHDESC            varchar(50),
   BREATHAIDTOOLS       varchar(50),
   SMOKINGHISTORY       varchar(50),
   NUTRITIONFLAG        tinyint(1),
   EATTYPE              varchar(50),
   DIETTYPEDESC         varchar(50),
   FOODINTAKEAMT        varchar(50),
   WATERAMT             varchar(50),
   WATERENOUGHAMT       varchar(50),
   ORALMUCOSA           varchar(50),
   DENTUREFIXED         varchar(50),
   DENTUREMOVABLE       varchar(50),
   DENTURESAFETY        varchar(50),
   DENTUREFIT           varchar(50),
   DENTUREHEALTH        varchar(50),
   EXCRETIONFLAG        tinyint(1),
   MICTURITIONDESC      varchar(50),
   MICTURITIONAIDTYPE   varchar(50),
   DEFECATIONDESC       varchar(50),
   DEFECATIONFREQ       varchar(50),
   DEFECATIONAIDDESC    varchar(50),
   OTHERDEFECATION      varchar(50),
   SLEEPFLAG            tinyint(1),
   SLEEPHOURS_N         decimal(5,2),
   SLEEPHOURS_D         decimal(5,2),
   SLEEPDESC            varchar(50),
   SLEEPPILLS           varchar(50),
   ACTIVEFUNFLAG        tinyint(1),
   GAIT                 varchar(50),
   PHYSICALIMPAIRMENT   varchar(50),
   MUSCLE_LH            varchar(50),
   MUSCLE_RH            varchar(50),
   MUSCLE_LF            varchar(50),
   MUSCLE_RF            varchar(50),
   AIDTOOLS             varchar(50),
   SKINFLAG             tinyint(1),
   SKINDESC             varchar(60),
   SKINPARTDESC         varchar(60),
   WOUNDDESC            varchar(60),
   BEDSOREPART          varchar(60),
   BEDSORESIZE          varchar(60),
   BEDSOREDEGREE        varchar(60),
   FEELINGFLAG          tinyint(1),
   SIGHTDESC            varchar(60),
   SIGHT_L              varchar(60),
   SIGHT_R              varchar(60),
   LISTENDESC           varchar(60),
   LISTEN_L             varchar(60),
   LISTEN_R             varchar(60),
   SENSATIONDESC        varchar(60),
   PAINDESC             varchar(60),
   PAINPART             varchar(60),
   PAIN_FREQ            varchar(60),
   PAIN_NUTURE          varchar(60),
   PAIN_SHOW            varchar(60),
   PAINDRUGDESC         varchar(60),
   ILLUSIONDESC         varchar(60),
   ADLSCORE             int(11),
   MMSESCORE            int(11),
   MMSEDESC             varchar(200),
   INTERACTIONFLAG      tinyint(1),
   COMMUNICATESKILL     varchar(60),
   COMMUNICATETYPE      varchar(60),
   COMMUNICATEDESC      varchar(60),
   APPELLATION          varchar(60),
   EMOTION              varchar(60),
   ATTITUDEREMARK       varchar(60),
   ALLERGYFLAG          tinyint(1),
   ALLERGY_DRUG         varchar(254),
   ALLERGY_FOOD         varchar(254),
   ALLERGY_OTHERS       varchar(254),
   GOAL_S               varchar(254),
   GOAL_L               varchar(254),
   DESCRIPTION          text,
   PIC1                 varchar(254),
   PIC2                 varchar(254),
   ADLRESULTS           varchar(60),
   iADLRESULTS          varchar(60),
   KS_RESULTS           varchar(80),
   YY_RESULTS           varchar(80),
   FALLRESULTS          varchar(80),
   PRESSURESORE         varchar(80),
   MUSCLETYPE_LH        varchar(80),
   MUSCLETYPE_RH        varchar(80),
   MUSCLETYPE_LF        varchar(80),
   MUSCLETYPE_RF        varchar(80),
   JOINTTYPE_LH         varchar(80),
   JOINTTYPE_RH         varchar(80),
   JOINTTYPE_LF         varchar(80),
   JOINTTYPE_RF         varchar(80),
   GCSRESULTS           varchar(80),
   PAINDEGREEDESC_W     text,
   ACCIDENT_W           varchar(80),
   ABNORMAL_W           varchar(80),
   HEARINGAID           varchar(80),
   SIGHTCORRECTED       varchar(80),
   BALANCE_SIT          varchar(80),
   BALANCE_STAND        varchar(80),
   BALANCE_WALK         varchar(80),
   URINESHAPE           varchar(80),
   GUT_SOUND            varchar(80),
   GUT_FLATULENCE       varchar(60),
   GUT_LUMP             varchar(60),
   STOOLSHAPE           varchar(80),
   MOUTHPAIN            varchar(80),
   TOOTHDESC            varchar(80),
   SWALLOWDESC          varchar(80),
   LIGHTREFLECTION      varchar(80),
   HEARTBEAT            varchar(80),
   LIMBEDEMA_LH         varchar(80),
   LIMBEDEMA_RH         varchar(80),
   LIMBEDEMA_LF         varchar(80),
   LIMBEDEMA_RF         varchar(80),
   CARENEEDS            varchar(80),
   CAREQUESTION         text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_CARERSVRREC                                       */
/*==============================================================*/
create table LTC_CARERSVRREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   CARER                varchar(10),
   RECDATE              datetime,
   SVRADDRESS           varchar(50),
   SVRTYPE              varchar(50),
   SVRPEOPLE            varchar(50),
   RELATIONTYPE         varchar(50),
   QUESTIONLEVEL        varchar(500),
   QUESTIONFOCUS        varchar(500),
   PROCESSACTIVITY      varchar(500),
   SVRFOCUS             varchar(50),
   QUESTIONDESC         text,
   TREATDESC            text,
   EVALSTATUS           varchar(50),
   EVALMINUTES          int(11),
   EVALDESC             text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_CHECKGROUP                                        */
/*==============================================================*/
create table LTC_CHECKGROUP
(
   GROUPCODE            varchar(10) not null,
   GROUPNAME            varchar(50),
   primary key (GROUPCODE)
);

/*==============================================================*/
/* Table: LTC_CHECKITEM                                         */
/*==============================================================*/
create table LTC_CHECKITEM
(
   ITEMCODE             varchar(10) not null,
   ITEMNAME             varchar(50),
   NORMALVALUE          varchar(200),
   LOWBOUND             varchar(20),
   UPBOUND              varchar(20),
   DESCRIPTION          varchar(200),
   TYPECODE             varchar(50),
   GROUPCODE            varchar(10),
   primary key (ITEMCODE)
);

/*==============================================================*/
/* Table: LTC_CHECKREC                                          */
/*==============================================================*/
create table LTC_CHECKREC
(
   RECORDID             bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   CHECKDATE            datetime,
   RECORDBY             varchar(10),
   HOSPNAME             varchar(50),
   CHECKRESULTS         varchar(50),
   NEXTCHECKDATE        datetime,
   NEXTCHECKBY          varchar(10),
   TRACESTATE           varchar(20),
   DISEASEDESC          text,
   XRAYFLAG             tinyint(1),
   NORMALFLAG           tinyint(1),
   DESCRIPTION          text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (RECORDID)
);

/*==============================================================*/
/* Table: LTC_CHECKRECDTL                                       */
/*==============================================================*/
create table LTC_CHECKRECDTL
(
   ID                   bigint(20) not null auto_increment,
   CHECKTYPE            varchar(50),
   CHECKITEM            varchar(200),
   CHECKRESULTS         varchar(100),
   DESCRIPTION          varchar(200),
   EVALCOMBO            varchar(200),
   RECORDID             bigint(20),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_CHECKTYPE                                         */
/*==============================================================*/
create table LTC_CHECKTYPE
(
   TYPECODE             varchar(50) not null,
   TYPENAME             varchar(50),
   primary key (TYPECODE)
);

/*==============================================================*/
/* Table: LTC_CODEDTL_REF                                       */
/*==============================================================*/
create table LTC_CODEDTL_REF
(
   ITEMCODE             varchar(20) not null,
   ITEMTYPE             varchar(10) not null,
   ITEMNAME             varchar(100),
   DESCRIPTION          varchar(256),
   ORDERSEQ             int(11),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   primary key (ITEMTYPE, ITEMCODE)
);

/*==============================================================*/
/* Table: LTC_CODEFILE_REF                                      */
/*==============================================================*/
create table LTC_CODEFILE_REF
(
   ITEMTYPE             varchar(10) not null,
   TYPENAME             varchar(40),
   DESCRIPTION          varchar(200),
   MODIFYFLAG           char(1),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   primary key (ITEMTYPE)
);

/*==============================================================*/
/* Table: LTC_COMMFILE                                          */
/*==============================================================*/
create table LTC_COMMFILE
(
   ID                   int(11) not null auto_increment,
   TYPECODE             varchar(50),
   TYPENAME             varchar(50),
   ITEMNAME             varchar(100),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_COMMFILE_copy                                     */
/*==============================================================*/
create table LTC_COMMFILE_copy
(
   ID                   int(11) not null auto_increment,
   TYPECODE             varchar(50),
   TYPENAME             varchar(50),
   ITEMNAME             varchar(100),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_COMPLAINREC                                       */
/*==============================================================*/
create table LTC_COMPLAINREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                int(11),
   REGNO                int(11),
   RECDATE              datetime,
   PROCESSBY            varchar(10),
   QUESTIONDESC         text,
   QUESTIONTYPE         varchar(10),
   EMPNO                varchar(10),
   STATUS               varchar(10),
   RESULTS              text,
   DESCRIPTION          text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_CONSTRAINSBEVAL                                   */
/*==============================================================*/
create table LTC_CONSTRAINSBEVAL
(
   ID                   bigint(20) not null auto_increment,
   SEQNO                bigint(20),
   EVALDATE             datetime,
   REASON               varchar(50),
   CONSTRAINTWAY        varchar(50),
   BODYPART             varchar(50),
   DURATION             varchar(50),
   TEMPCANCELDESC       varchar(50),
   SKINDESC             varchar(50),
   BLOODCIRCLEDESC      varchar(50),
   EVALUATEBY           varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_CONSTRAINTREC                                     */
/*==============================================================*/
create table LTC_CONSTRAINTREC
(
   SEQNO                bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   RECORDBY             varchar(10),
   STARTDATE            datetime,
   CLASSTYPE            varchar(20),
   DAYS                 int(11),
   REASON               varchar(50),
   EXECREASON           varchar(50),
   CONSTRAINTWAY        varchar(50),
   BODYPART             varchar(50),
   CONSTRAINTWAYCNT     varchar(10),
   CANCELFLAG           tinyint(1),
   CANCELDATE           datetime,
   DURATION             varchar(50),
   CANCELREASON         varchar(50),
   CANCELEXECBY         varchar(10),
   CANCEL24FLAG         tinyint(1),
   DESCRIPTION          text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: LTC_COSTDTL                                           */
/*==============================================================*/
create table LTC_COSTDTL
(
   ID                   bigint(20) not null auto_increment,
   COSTNO               varchar(20),
   COSTITEMNO           varchar(10),
   COSTNAME             varchar(50),
   COSTSOURCE           int(2),
   ITEMTYPE             varchar(10),
   OCCURTIME            datetime,
   QUANTITY             int(11),
   PRICE                decimal(18,2),
   TOTALPRICE           decimal(18,2),
   SELFFLAG             tinyint(1),
   DESCRIPTION          varchar(200),
   REGNO                int(11) not null,
   FEENO                bigint(20) not null,
   ORGID                varchar(10) not null,
   BILLID               bigint(20),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_COSTGROUP                                         */
/*==============================================================*/
create table LTC_COSTGROUP
(
   ID                   int(11) not null auto_increment,
   GROUPNO              varchar(10) not null,
   GROUPNAME            varchar(50),
   GROUPTYPE            varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_COSTGROUPDTL                                      */
/*==============================================================*/
create table LTC_COSTGROUPDTL
(
   ID                   int(11) not null auto_increment,
   GROUPID              int(11) not null,
   COSTITEMNO           varchar(10),
   COSTNAME             varchar(50),
   PRICE                decimal(18,2),
   PERIOD               varchar(10),
   REPEATCOUNT          int(11),
   COSTITEMID           int(11),
   ITEMUNIT             varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_COSTITEM                                          */
/*==============================================================*/
create table LTC_COSTITEM
(
   ID                   int(11) not null auto_increment,
   COSTITEMNO           varchar(10) not null,
   COSTNAME             varchar(50),
   ITEMTYPE             varchar(10),
   ITEMUNIT             varchar(10),
   PRICE                decimal(18,2),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_DEPTFILE                                          */
/*==============================================================*/
create table LTC_DEPTFILE
(
   DEPTNO               varchar(10) not null,
   DEPTNAME             varchar(50),
   REMARK               text,
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   STATUS               tinyint(1),
   ORGID                varchar(10) not null,
   primary key (ORGID, DEPTNO)
);

/*==============================================================*/
/* Table: LTC_DOCTORCHECKREC                                    */
/*==============================================================*/
create table LTC_DOCTORCHECKREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   CHECKDATE            datetime,
   DEPTNO               varchar(10),
   DOCNO                varchar(10),
   CONSCIOUSNESS        varchar(20),
   PHYSIOLOGY           varchar(20),
   BODYTEMP             decimal(5,2),
   PULSE                int(11),
   BP                   int(11),
   BPH                  int(11) comment '舒张压',
   OXYGEN               decimal(5,2),
   BS                   decimal(5,2),
   DISPOSITIONDESC      varchar(254),
   OTHERDESC            varchar(254),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_DOCTOREVALREC                                     */
/*==============================================================*/
create table LTC_DOCTOREVALREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   EVALDATE             datetime,
   DOCNAME              varchar(10),
   SMOKINGHISTORY       varchar(30),
   DRINKWINE            varchar(30),
   CHEWINGBETELNUT      varchar(30),
   DRUGFLAG             varchar(10),
   REGULAREXERCISE      varchar(10),
   OTHERDESC            varchar(30),
   DISEASEDESC          varchar(100),
   MEDICALHISTORY       varchar(30),
   HEIGHT               decimal(5,1),
   WEIGHT               decimal(5,1),
   WAIST                decimal(5,1),
   HIPLINE              decimal(5,1),
   ARMSAROUND           decimal(5,1),
   VITALSIGNS           int(11),
   CONSCIOUSNESS        varchar(50),
   EMOTION              varchar(50),
   COMMUNICATION        varchar(50),
   EYESIGHT             varchar(20),
   HEARING              varchar(20),
   TACTILESENSATION     varchar(20),
   PAINSTATE            varchar(20),
   PAINTYPE             varchar(20),
   PAINNATURE           varchar(20),
   DIETSTATE            varchar(20),
   DIETWAY              varchar(20),
   DIETPATTERN          varchar(20),
   DRUGUSESTATE         varchar(20),
   MOVEMENT             varchar(20),
   SLEEP                varchar(20),
   SLEEPPILLS           tinyint(1),
   WOUNDFLAG            tinyint(1),
   WOUNDHEAL            varchar(20),
   WOUNDPART            varchar(20),
   WOUNDSIZE            varchar(20),
   SKINDESC             varchar(20),
   FALSETEETH           varchar(20),
   LYMPHNODE            varchar(20),
   BREAST               varchar(20),
   PLEURAL              varchar(20),
   SYMMETRICALCHEST     varchar(10),
   HEART                varchar(20),
   HEARTNOISE           varchar(20),
   HEARTARRHYTHMIA      varchar(20),
   ABDOMEN              varchar(20),
   LIMBS                varchar(20),
   NERVOUSSYSTEM        varchar(50),
   CHECKDATA            varchar(100),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_DRGFREQREF                                        */
/*==============================================================*/
create table LTC_DRGFREQREF
(
   FREQNO               varchar(20) not null,
   FREQNAME             varchar(40),
   FREQHOUR             int(11),
   FREQDAY              int(11),
   FREQQTY              int(11),
   FREQMOR              char(1),
   FREQNOON             char(1),
   FREQEVE              char(1),
   FREQSLEEP            char(1),
   FREQMEAL             char(1),
   FREQTIME             char(24),
   FREQPRN              char(1),
   FREQFLG              char(1),
   FREQWEEK             char(7),
   STATEFLAG            char(1),
   CREATEID             varchar(20),
   CREATETIME           datetime,
   UPDATEID             varchar(20),
   UPDATETIME           datetime,
   primary key (FREQNO)
);

/*==============================================================*/
/* Table: LTC_EMPFILE                                           */
/*==============================================================*/
create table LTC_EMPFILE
(
   EMPNO                varchar(10) not null,
   EMPNAME              varchar(50),
   EMPGROUP             varchar(10),
   IDNO                 varchar(18),
   BRITHDATE            datetime,
   BRITHPLACE           varchar(20),
   SEX                  varchar(10),
   BLOODTYPE            varchar(10),
   RTHTYPE              varchar(10),
   MERRYFLAG            varchar(10),
   HOMETELNO            varchar(20),
   ZIP1                 varchar(10),
   ADDRESS1             varchar(200),
   ZIP2                 varchar(10),
   ADDRESS2             varchar(200),
   CONTNAME             varchar(20),
   CONTRELATION         varchar(10),
   CONTTELPHONE         varchar(20),
   CONTADDRESS          varchar(200),
   JOBTITLE             varchar(10),
   EDUCATION            varchar(50),
   STATUS               tinyint(1),
   DEPTNO               varchar(10) not null,
   HIREDTYPE            varchar(20),
   JOBTYPE              varchar(20),
   NATIVESFLAG          tinyint(1),
   LIGIOUSFAITH         varchar(10),
   DISABILITYFLAG       tinyint(1),
   NATIONALITY          varchar(10),
   ORGID                varchar(10) not null,
   STARTDATE            datetime,
   ENDDATE              datetime,
   primary key (EMPNO)
);

/*==============================================================*/
/* Table: LTC_FALLINCIDENTEVENT                                 */
/*==============================================================*/
create table LTC_FALLINCIDENTEVENT
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   RECORDBY             varchar(10),
   EVENTADDRESS         varchar(20),
   EVENTDATE            datetime,
   EVENTTYPE            varchar(200),
   EVENTDEGREE          varchar(50),
   EVENTCAUSE           varchar(200),
   CONSCIOUSSTATE       varchar(50),
   EMOTIONALSTATE       varchar(50),
   SIGNS                varchar(50),
   EVENTDESC            text,
   SETTLEBY             varchar(10),
   VISITDOCDATE         datetime,
   HOSPNAME             varchar(50),
   NOTIFYPEOPLE         varchar(100),
   NOTIFYGOVFLAG        tinyint(1),
   NOTIFYDATE           datetime,
   CONSCIOUSSTATE_A     varchar(50),
   EMOTIONSTATE_A       varchar(50),
   SETTLERESULT         varchar(50),
   FOLLOWUPINSTRUCTIONS text,
   MEDICALDISPUTE       varchar(20),
   AFFECTS              varchar(50),
   AFFECTSDESC          varchar(200),
   IMPROVEMENT          varchar(200),
   DESCRIPTION          text,
   ORGID                varchar(10),
   PICT1                varchar(60),
   PICT2                varchar(60),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_FAMILYDISCUSSREC                                  */
/*==============================================================*/
create table LTC_FAMILYDISCUSSREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   RECORDBY             varchar(10),
   STARTDATE            datetime,
   ENDDATE              datetime,
   VISITTYPE            varchar(50),
   VISITORNAME          varchar(20),
   ISGOABROAD           tinyint(1),
   GOABROADPLACE        varchar(200),
   APPELLATION          varchar(20),
   BLOODRELATIONSHIP    varchar(20),
   BODYTEMP             decimal(5,2),
   DESCRIPTION          text,
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_FIXEDCOST                                         */
/*==============================================================*/
create table LTC_FIXEDCOST
(
   ID                   int(11) not null auto_increment,
   STARTDATE            datetime not null comment '项目收费开始时间',
   ENDDATE              datetime comment '项目收费结束时间',
   ISENDCHARGES         tinyint(1) unsigned zerofill default 0,
   COSTITEMID           int(11) not null,
   COSTITEMNO           varchar(10),
   COSTNAME             varchar(50),
   ITEMUNIT             varchar(10),
   PRICE                decimal(18,2),
   PERIOD               varchar(10),
   REPEATCOUNT          int(11),
   GENERATEDATE         datetime,
   REGNO                int(11) not null,
   FEENO                bigint(20) not null,
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_GOODS                                             */
/*==============================================================*/
create table LTC_GOODS
(
   Id                   int(11) not null auto_increment,
   Name                 varchar(50) not null,
   No                   varchar(50),
   BarNo                varchar(50),
   Type                 varchar(50),
   Unit                 varchar(50),
   LoanPrice            decimal(18,2),
   SellingPrice         decimal(18,2),
   InventoryQuantity    int(11),
   InventoryBaseline    int(11),
   TotalLoanAmount      int(11),
   TotalSaleAmount      int(11),
   NotLess              tinyint(1),
   IsPayItem            tinyint(1),
   IsUsed               tinyint(1),
   OrgId                varchar(10),
   primary key (Id)
);

/*==============================================================*/
/* Table: LTC_GOODSLOAN                                         */
/*==============================================================*/
create table LTC_GOODSLOAN
(
   Id                   int(11) not null auto_increment,
   GoodsId              int(11) not null,
   No                   varchar(50),
   ManufactureId        int(11),
   LoanDate             datetime,
   Price                decimal(18,2),
   Amount               int(11),
   Sum                  decimal(18,2),
   IntervalDay          int(11),
   NextDate             datetime,
   Purchaser            varchar(10),
   Remark               varchar(150),
   OrgId                varchar(10),
   primary key (Id)
);

/*==============================================================*/
/* Table: LTC_GOODSSALE                                         */
/*==============================================================*/
create table LTC_GOODSSALE
(
   Id                   int(11) not null auto_increment,
   GoodsId              int(11) not null,
   GoodsName            varchar(50),
   No                   varchar(50),
   Amount               int(11),
   Sum                  decimal(18,2),
   Price                decimal(18,2),
   SaleTime             datetime,
   Remark               varchar(150),
   OrgId                varchar(10),
   primary key (Id)
);

/*==============================================================*/
/* Table: LTC_GROUP                                             */
/*==============================================================*/
create table LTC_GROUP
(
   GROUPID              varchar(10) not null,
   GROUPNAME            varchar(200),
   ADDRESS              varchar(256),
   primary key (GROUPID)
);

/*==============================================================*/
/* Table: LTC_GROUPACTIVITYREC                                  */
/*==============================================================*/
create table LTC_GROUPACTIVITYREC
(
   ID                   int(11) not null auto_increment,
   ACTIVITYNAME         varchar(60),
   ACTIVITYTYPE         varchar(50),
   RECORDDATE           datetime,
   EVENTDATE            datetime,
   EVENTHOURS           int(11),
   EVENTPLACE           varchar(200),
   LEADERNAME           varchar(50),
   LEADERNAME1          varchar(50),
   LEADERNAME2          varchar(50),
   ACTIVITYRESULTS      varchar(50),
   EVALRESULTS          int(11),
   ATTENDNUMBER         int(11),
   GROUPCOMPETENCE      text,
   INDIVIDUALSUMMARY    text,
   SUGGESTION           text,
   DESCRIPTION          text,
   PICTURE1             varchar(100),
   PICTURE2             varchar(100),
   ATTENDNO             varchar(300),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_HOMECARESUPERVISE                                 */
/*==============================================================*/
create table LTC_HOMECARESUPERVISE
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   SUPERVISEDATE        datetime,
   OPERATOR             varchar(10),
   SUPERVISOR           varchar(10),
   CONTACTTYPE          varchar(20),
   MINUTES              int(11),
   SUPERVISEDESC        text,
   ASSESSMENT           text,
   PROCESSDESC          text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   CONTACTDATE          datetime,
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_HOMECARESVRREC                                    */
/*==============================================================*/
create table LTC_HOMECARESVRREC
(
   ID                   bigint(20) not null,
   FEENO                bigint(20),
   REGNO                int(11),
   RECRDATE             datetime,
   RECORDBY             varchar(10),
   ATTENDTYPE           varchar(50),
   LEAVEREASON          varchar(50),
   BODYTEMP             decimal(5,2),
   SBP                  int(11),
   DBP                  int(11),
   BREATHE              int(11),
   PULSE                int(11),
   MOUTHCARE            tinyint(1),
   TEETHCARE            tinyint(1),
   DENTURECARE          tinyint(1),
   GARGLE               tinyint(1),
   MOUTHCHECK           tinyint(1),
   WOUNDCARE            tinyint(1),
   WOUNDCAREREASON      varchar(50),
   BODYCARE             tinyint(1),
   CUTNAILS             tinyint(1),
   SHAVE                tinyint(1),
   CHANGECLOTH          tinyint(1),
   CUTHAIR              tinyint(1),
   WASHHAIR             tinyint(1),
   BATH                 tinyint(1),
   LUNCH                varchar(20),
   LUNCHMEAT            varchar(20),
   GENERALDIET          tinyint(1),
   SOFTDIET             tinyint(1),
   BROKENDIET           tinyint(1),
   LOWSUGARDIET         tinyint(1),
   LOWSODIUMDIET        tinyint(1),
   LOWOILDIET           tinyint(1),
   SNACK                varchar(20),
   WATERAMT             int(11),
   EATASSIST            varchar(20),
   MEDICATION_BB        tinyint(1),
   MEDICATION           tinyint(1),
   MEDICATION_AB        tinyint(1),
   MEDICATION_BL        tinyint(1),
   MEDICATION_AL        tinyint(1),
   MEDICATION_AS        tinyint(1),
   EYEDROPS             tinyint(1),
   EYEDROPS_TIMES       int(11),
   GETDRUGS             tinyint(1),
   VISITDOCTOR          tinyint(1),
   VISITHOSPITAL        varchar(200),
   URINETIMES           int(11),
   DIAPERCHANGETIMES    int(11),
   DEFECTIONTIMES       int(11),
   DISCHARGESTATE       varchar(200),
   MOODSTABLE           tinyint(1),
   MOODANXIOUS          tinyint(1),
   MOODDYSPHORIA        tinyint(1),
   MOODANGER            tinyint(1),
   MOODAPATHY           tinyint(1),
   SUNDOWNSYNDROME      tinyint(1),
   MOODOTHERS           tinyint(1),
   MOODOTHERSDESC       varchar(200),
   NOONBREAK            varchar(20),
   NOONSHUTTLE          varchar(20),
   AFTERNOONSHUTTLE     varchar(20),
   RELATIVESVISIT       tinyint(1),
   RELATIVESTALK        tinyint(1),
   RELATIVESTEL         tinyint(1),
   CONTACTBOOK          tinyint(1),
   AM_ACTIVITY          varchar(200),
   AM_A_RARTICIPATE     varchar(20),
   AM_ATTENTION         varchar(20),
   PM_ACTIVITY          varchar(200),
   PM_A_PARTICIPATE     varchar(20),
   PM_ATTENTION         varchar(20),
   PERSONALACTIVITY     varchar(200),
   EQUIPMENTUSE         varchar(100),
   SPECIALCASE          text,
   PASTYDIET            tinyint(1),
   OTHERDIET            tinyint(1),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_ICD9_DISEASE                                      */
/*==============================================================*/
create table LTC_ICD9_DISEASE
(
   ICDCODE              varchar(20) not null,
   STATUS               tinyint(1),
   ENGNAME              varchar(500),
   primary key (ICDCODE)
);

/*==============================================================*/
/* Table: LTC_INFECTIONIND                                      */
/*==============================================================*/
create table LTC_INFECTIONIND
(
   SEQNO                bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   IFCTYPE              varchar(200),
   IFCDATE              datetime,
   RECORDBY             varchar(10),
   RECDATE              datetime,
   CATEGORY             varchar(50),
   H72FLAG              tinyint(1),
   IFCFLAG              tinyint(1),
   CATHETERFLAG         tinyint(1),
   ITEMSCORE            decimal(5,2),
   SECTYPE              varchar(200),
   SECSTARTDATE         datetime,
   SECENDDATE           datetime,
   SECDAYS              int(11),
   CLINICALSYMPTOM      text,
   DOCTORDIAG           text,
   B3ANTIFLAG           tinyint(1),
   ANTITREATFLAG        tinyint(1),
   ANTITREATTYPE        varchar(200),
   IMPROVEMENT          text,
   DESCRIPTION          text,
   ORGID                varchar(10),
   ITEMNO               varchar(20),
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: LTC_INFECTIONITEM                                     */
/*==============================================================*/
create table LTC_INFECTIONITEM
(
   ITEMCODE             varchar(20) not null,
   ITEMNAME             varchar(200),
   SCORE                int(11),
   primary key (ITEMCODE)
);

/*==============================================================*/
/* Table: LTC_INFECTIONSYMPOTM                                  */
/*==============================================================*/
create table LTC_INFECTIONSYMPOTM
(
   ID                   bigint(20) not null auto_increment,
   SEQNO                bigint(20),
   ITEMNAME             varchar(100),
   SYMPOTM              varchar(200),
   OCCURDATE            datetime,
   CREATEDATE           datetime,
   CREATEBY             varchar(20),
   ITEMNO               varchar(50),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_INSULININJECT                                     */
/*==============================================================*/
create table LTC_INSULININJECT
(
   ID                   bigint(20) not null,
   FEENO                bigint(20),
   REGNO                int(11),
   INJECTDATE           datetime,
   BSVALUE              decimal(6,1),
   DOSEVALUE            decimal(6,1),
   BODYPART             varchar(10),
   OPERATOR             varchar(10),
   DESCRIPTION          text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_INVALUE                                           */
/*==============================================================*/
create table LTC_INVALUE
(
   INNO                 bigint(20) not null auto_increment,
   FEENO                bigint(20),
   RECDATE              datetime,
   CLASSTYPE            varchar(10),
   INTYPE               varchar(10),
   INVALUE              int(11),
   COMMDESC             varchar(200),
   RECORDBY             varchar(10),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (INNO)
);

/*==============================================================*/
/* Table: LTC_IPDCLOSECASE                                      */
/*==============================================================*/
create table LTC_IPDCLOSECASE
(
   FEENO                bigint(20) not null,
   REGNO                int(11),
   PALLIATIVECARE       varchar(50),
   PALLIATIVECAREFILE   varchar(100),
   CLOSEFLAG            tinyint(1),
   CLOSEDATE            datetime,
   CLOSEREASON          varchar(10),
   TRAININGDATA         tinyint(1),
   WILLSFLAG            tinyint(1),
   WILLSDESC            varchar(200),
   BODYPROCESS          varchar(50),
   BODYPROCESSDESC      varchar(200),
   BODYKEEPPLACE        varchar(20),
   ESTATEEXECUTOR       varchar(50),
   ESTATEPROCESS        varchar(50),
   FUNERALPROCESS       varchar(200),
   BODYPROCESS_EXE      varchar(50),
   BODYPROCESSDESC_EXE  varchar(50),
   BODYKEEPPLACE_EXE    varchar(50),
   ESTATEEXECUTOR_EXE   varchar(50),
   ESTATEPROCESS_EXE    varchar(50),
   DEATHFLAG            tinyint(1),
   DEATHDATE            datetime,
   DEATHREASON          varchar(200),
   DEATHPLACE           varchar(200),
   MEETINGDATE          datetime,
   PARTICIPANTS         varchar(200),
   MEETINGNOTES         varchar(500),
   ORGID                varchar(10),
   primary key (FEENO)
);

/*==============================================================*/
/* Table: LTC_IPDREG                                            */
/*==============================================================*/
create table LTC_IPDREG
(
   FEENO                bigint(20) not null auto_increment,
   SERVICETYPE          varchar(10),
   PTYPE                varchar(10),
   INDATE               datetime,
   DEPTNO               varchar(10),
   NURSENO              varchar(10),
   CARER                varchar(10),
   BEDNO                varchar(10),
   ROOMNO               varchar(10),
   BEDCLASS             varchar(10),
   BEDTYPE              varchar(10),
   FLOOR                varchar(10),
   BEDKIND              varchar(10),
   SICKFLAG             tinyint(1),
   ROOMFLAG             tinyint(1),
   PROTFLAF             tinyint(1),
   IPDFLAG              char(2),
   OUTDATE              datetime,
   DANGERFLAG           tinyint(1),
   DEPOSITAMT           decimal(14,2),
   PREPAYAMT            decimal(14,2),
   CTRLFLAG             tinyint(1),
   CTRLREASON           varchar(200),
   NURSINGTIPS          varchar(10),
   CARERTIPS            varchar(10),
   STATEFLAG            varchar(10),
   STATEREASON          varchar(200),
   ORGID                varchar(10) not null,
   REGNO                int(11),
   NUTRITIONIST         varchar(10),
   PHYSIOTHERAPIST      varchar(10),
   DOCTOR               varchar(10),
   primary key (FEENO)
);

/*==============================================================*/
/* Table: LTC_IPDREGOUT                                         */
/*==============================================================*/
create table LTC_IPDREGOUT
(
   FEENO                bigint(20) not null,
   INDATE               datetime,
   CLOSEFLAG            tinyint(1),
   CLOSEREASON          varchar(10),
   DEADFLAG             tinyint(1),
   DEADREASON           varchar(10),
   DEADDATE             datetime,
   CLOSEDATE            datetime,
   TOTALDAY             int(11),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (FEENO)
);

/*==============================================================*/
/* Table: LTC_IPDVERIFY                                         */
/*==============================================================*/
create table LTC_IPDVERIFY
(
   ID                   int(11) not null auto_increment,
   FEENO                bigint(20) not null,
   REGNO                int(11),
   SUBSIDYAMT           decimal(18,2),
   SUBSIDYWAY           varchar(50),
   SUBSIDYUNIT          varchar(50),
   FEERATE              decimal(4,2),
   APPLYDOCNO           varchar(50),
   APPLYDATE            datetime,
   EXPIREDATE           datetime,
   APPROVEDOCNO         varchar(50),
   APPROVEDATE          datetime,
   INSTYPE              varchar(50),
   BANKNAME             varchar(60),
   BANKACCOUNTNO        varchar(60),
   OTHERACCOUNTNO       varchar(60),
   DEPOSITBALANCE       decimal(18,2),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_LABEXAMREC                                        */
/*==============================================================*/
create table LTC_LABEXAMREC
(
   ID                   bigint(20) not null auto_increment,
   SEQNO                bigint(20),
   UNIT                 varchar(50),
   EXAMDATE             datetime,
   EXAMTYPE             varchar(100),
   RPTDATE              datetime,
   FUNGUS               varchar(100),
   EXAMRESULTS          varchar(10),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_LEAVEHOSP                                         */
/*==============================================================*/
create table LTC_LEAVEHOSP
(
   ID                   int(11) not null auto_increment,
   SHOWNUMBER           int(11),
   FEENO                bigint(20),
   STARTDATE            datetime,
   ENDDATE              datetime,
   LEHOUR               decimal(6,2),
   LENOTE               varchar(500),
   RETURNDATE           datetime,
   ADDRESS              varchar(200),
   CONTNAME             varchar(20),
   CONTREL              varchar(10),
   CONTTEL              varchar(20),
   LETYPE               varchar(10),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_LIFERECORDS                                       */
/*==============================================================*/
create table LTC_LIFERECORDS
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   RECORDDATE           datetime,
   AMACTIVITY           varchar(200),
   PMACTIVITY           varchar(200),
   COMMENTS             text,
   BODYTEMP             decimal(4,2),
   RECORDBY             varchar(10),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_LOG                                               */
/*==============================================================*/
create table LTC_LOG
(
   ID                   bigint(18) not null auto_increment,
   USERNAME             varchar(50),
   IP                   varchar(50),
   HOSTNAME             varchar(50),
   LOGINTIME            datetime,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_MAKERITEM                                         */
/*==============================================================*/
create table LTC_MAKERITEM
(
   MAKERID              int(11) not null auto_increment,
   MAKENAME             varchar(200),
   SHOWNUMBER           int(11),
   ISSHOW               tinyint(1),
   QUESTIONID           int(11),
   DATATYPE             char(1),
   LIMITEDID            int(11),
   CATEGORY             varchar(50),
   primary key (MAKERID)
);

/*==============================================================*/
/* Table: LTC_MAKERITEMLIMITED                                  */
/*==============================================================*/
create table LTC_MAKERITEMLIMITED
(
   LIMITEDID            int(11) not null,
   LIMITEDNAME          varchar(200),
   REPEATCOLUMNS        int(11),
   SHOWNUMBER           int(11),
   ISSHOW               tinyint(1),
   QUESTIONID           int(11),
   primary key (LIMITEDID)
);

/*==============================================================*/
/* Index: R_21                                                  */
/*==============================================================*/
create index R_21 on LTC_MAKERITEMLIMITED
(
   QUESTIONID
);

/*==============================================================*/
/* Table: LTC_MAKERITEMLIMITEDVALUE                             */
/*==============================================================*/
create table LTC_MAKERITEMLIMITEDVALUE
(
   LIMITEDVALUEID       int(11) not null auto_increment,
   LIMITEDVALUE         decimal(18,2),
   LIMITEDVALUENAME     varchar(300),
   SHOWNUMBER           int(11),
   ISDEFAULT            tinyint(1),
   LIMITEDID            int(11) not null,
   primary key (LIMITEDVALUEID)
);

/*==============================================================*/
/* Index: Q3                                                    */
/*==============================================================*/
create index Q3 on LTC_MAKERITEMLIMITEDVALUE
(
   LIMITEDID
);

/*==============================================================*/
/* Table: LTC_MANUFACTURE                                       */
/*==============================================================*/
create table LTC_MANUFACTURE
(
   Id                   int(11) not null auto_increment,
   No                   varchar(50),
   Name                 varchar(50) not null,
   Description          varchar(150),
   OrgId                varchar(10),
   primary key (Id)
);

/*==============================================================*/
/* Table: LTC_MEDICINE                                          */
/*==============================================================*/
create table LTC_MEDICINE
(
   MEDID                int(11) not null auto_increment,
   MEDCODE              varchar(20),
   CHNNAME              varchar(100),
   ENGNAME              varchar(100),
   SIDEEFFECT           varchar(100),
   MEDKIND              varchar(10),
   MEDICOLOR            varchar(20),
   MEDIFACADE           varchar(20),
   SPECDESC             varchar(20),
   USEDESC              varchar(100),
   COMMCODE             varchar(200),
   MEDTYPE              varchar(20),
   MEDPICT              varchar(60),
   INSNO                varchar(50),
   HOSPNO               varchar(10),
   ORGID                varchar(10) not null,
   primary key (MEDID)
);

/*==============================================================*/
/* Table: LTC_MODULES                                           */
/*==============================================================*/
create table LTC_MODULES
(
   MODULEID             varchar(40) not null,
   MODULENAME           varchar(200),
   URL                  varchar(256),
   DESCRIPTION          varchar(256),
   SUPERMODULEID        varchar(40),
   TARGET               varchar(50),
   ICON                 varchar(200),
   ROOTORDER            int(11),
   ISSYS                tinyint(1),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   STATUS               tinyint(1),
   SYSTYPE              varchar(10),
   primary key (MODULEID)
);

/*==============================================================*/
/* Table: LTC_NEWREGENVADAPTATION                               */
/*==============================================================*/
create table LTC_NEWREGENVADAPTATION
(
   ID                   bigint(20) not null auto_increment,
   INDATE               datetime,
   W1EVALDATE           datetime,
   INFORMFLAG           bigint(20),
   COMMFLAG             bigint(20),
   INTERPERSONAL        varchar(200),
   PARTICIPATION        varchar(200),
   COORDINATION         varchar(20),
   EMOTION              text,
   RESISTANCE           text,
   HELP                 text,
   WEEK                 int(4),
   PROCESSACTIVITY      text,
   TRACEREC             varchar(50),
   EVALDATE             datetime,
   EVALUATION           text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   FEENO                bigint(20),
   REGNO                int(11),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_NEWRESIDENTENVREC                                 */
/*==============================================================*/
create table LTC_NEWRESIDENTENVREC
(
   ID                   bigint(20) not null auto_increment,
   RESIDENGNO           varchar(10),
   BEDNO                varchar(10),
   SEX                  varchar(10),
   INDATE               datetime,
   RECORDDATE           datetime,
   BIRTHDATE            datetime,
   FAMILYPARTICIPATION  tinyint(1),
   CONTRACTFLAG         tinyint(1),
   LIFEFLAG             tinyint(1),
   STAFF1               varchar(10),
   REGULARACTIVITY      tinyint(1),
   NOTREGULARACTIVITY   tinyint(1),
   STAFF2               varchar(10),
   BELLFLAG             tinyint(1),
   LAMPFLAG             tinyint(1),
   TVFLAG               tinyint(1),
   LIGHTSWITCH          tinyint(1),
   ESCAPEDEVICE         tinyint(1),
   ENVIRONMENT          tinyint(1),
   COMMUNITYFACILITIES  tinyint(1),
   POSTOFFICE           tinyint(1),
   SCHOOL               tinyint(1),
   BANK                 tinyint(1),
   STATION              tinyint(1),
   PARK                 tinyint(1),
   TEMPLE               tinyint(1),
   HOSPITAL             tinyint(1),
   OTHERFACILITIES      tinyint(1),
   CLEANLINESS          tinyint(1),
   MEDICALCARE          tinyint(1),
   MEALSERVICE          tinyint(1),
   WORKACTIVITIES       tinyint(1),
   STAFF3               varchar(10),
   STAFF4               varchar(10),
   PERSONINCHARGE       tinyint(1),
   DIRECTOR             tinyint(1),
   NURSE                tinyint(1),
   NURSEAIDES           tinyint(1),
   RESIDENT             tinyint(1),
   DOCTOR               tinyint(1),
   SOCIALWORKER         tinyint(1),
   DIETITIAN            tinyint(1),
   OTHERPEOPLE          tinyint(1),
   STAFF5               varchar(10),
   RECORDBY             varchar(10),
   FEENO                bigint(20),
   REGNO                int(11),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_NOTIFICATION                                      */
/*==============================================================*/
create table LTC_NOTIFICATION
(
   ID                   int(11) not null auto_increment,
   SUBJECTS             varchar(200),
   CONTENTS             text,
   DOCTYPE              varchar(20),
   DOCUMENTNO           varchar(30),
   RECORDBY             varchar(50),
   RECORDDATE           datetime,
   EXPRIEDATE           datetime,
   REMARKS              text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_NSCPL                                             */
/*==============================================================*/
create table LTC_NSCPL
(
   SEQNO                bigint(20) not null auto_increment,
   EMPNO                varchar(10),
   CPSOURCE             varchar(50),
   CPTYPE               varchar(50),
   CPLEVEL              varchar(20),
   CPDIAG               varchar(100),
   NSDESC               text,
   CPCAUSE              varchar(20),
   CPREASON             text,
   FINISHFLAG           varchar(6) comment '照顧計劃評值結果',
   CPRESULT             varchar(200),
   DESCRIPTION          text,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   FEENO                bigint(20),
   REGNO                int(11),
   NEEDDAYS             int(11),
   TOTALDAYS            int(11),
   STARTDATE            datetime,
   TARGETDATE           datetime,
   FINISHDATE           datetime,
   CREATEDATE           datetime,
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: LTC_NSCPLACTIVITY                                     */
/*==============================================================*/
create table LTC_NSCPLACTIVITY
(
   ID                   bigint(20) not null auto_increment,
   CPLACTIVITY          varchar(500),
   FINISHFLAG           tinyint(1),
   UNFINISHREASON       varchar(500),
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   SEQNO                bigint(20),
   FEENO                bigint(20),
   REGNO                int(11),
   RECDATE              datetime,
   FINISHDATE           datetime,
   CREATEDATE           datetime,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_NSCPLGOAL                                         */
/*==============================================================*/
create table LTC_NSCPLGOAL
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   RECDATE              datetime,
   CPLGOAL              varchar(500),
   FINISHFLAG           tinyint(1),
   FINISHDATE           datetime,
   UNFINISHREASON       varchar(500),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   SEQNO                bigint(20),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_NURSERPTTPR                                       */
/*==============================================================*/
create table LTC_NURSERPTTPR
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   RECDATE              datetime,
   CLASSTYPE            varchar(10),
   RECORDBY             varchar(10),
   BODYTEMP             decimal(5,2),
   SBP                  int(11),
   DBP                  int(11),
   PULSE                int(11),
   BREATH               int(11),
   OXYGEN               decimal(5,2),
   INVALUE              int(11),
   OUTVALUE             int(11),
   EDEMA                varchar(20),
   OTHERDESC            varchar(500),
   NOISEI1              int(11),
   NOISEI2              int(11),
   NOISEI3              int(11),
   FPG                  int(11),
   PPBS                 int(11),
   NASOGASTRIC          tinyint(1),
   CATHETER             tinyint(1),
   TRACHEOSTOMY         tinyint(1),
   STOMAFISTULA         tinyint(1),
   WOUNDSKINCARE        tinyint(1),
   SPRAYINHALATION      tinyint(1),
   OXYGENUSE            varchar(20),
   APPETITE             varchar(20),
   VOMITINGTIMES        int(11),
   SLEEPHOURS           int(11),
   SLEEPSTATE           varchar(20),
   MENSTRUALCYCLE       tinyint(1),
   INTESTINALPERISTALSIS varchar(20),
   STOOLNATURE          varchar(20),
   STOOLTIMES           int(11),
   REHABILITATION       varchar(50),
   OUTBEDNUMBER         int(11),
   CONSTRAINTSEVAL      varchar(50),
   SKININTEGRITY        varchar(50),
   DOCDIAGFLAG          tinyint(1),
   PAINFLAG             tinyint(1),
   PAINPART             varchar(50),
   PAINLEVEL            varchar(50),
   PAINPRESCRIPTION     varchar(200),
   SIDEEFFECT           varchar(50),
   PAINCARE             varchar(200),
   THERAPYRESPONSE      varchar(50),
   PULMONARYMURMUR      varchar(50),
   SPUTUMCOLOR          varchar(50),
   REGTALKFLAG          tinyint(1),
   FAMILYTALKFLAG       tinyint(1),
   CARERECFLAG          tinyint(1),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   DEFECATIONWAY        varchar(20),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_NURSINGHANDOVER                                   */
/*==============================================================*/
create table LTC_NURSINGHANDOVER
(
   ID                   int(11) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   CONTENT_D            text,
   CONTENT_E            text,
   CONTENT_N            text,
   RECDATE_D            datetime,
   RECDATE_E            datetime,
   RECDATE_N            datetime,
   RECORDBY_D           varchar(10),
   RECORDBY_E           varchar(10),
   RECORDBY_N           varchar(10),
   NURSE_D              varchar(20),
   NURSE_E              varchar(20),
   NURSE_N              varchar(20),
   PRINTFLAG            tinyint(1),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_NURSINGREC                                        */
/*==============================================================*/
create table LTC_NURSINGREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   RECORDDATE           datetime,
   CLASSTYPE            varchar(10),
   CONTENT              text,
   RECORDBY             varchar(10),
   PRINTFLAG            tinyint(1),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_NUTRTION72EVAL                                    */
/*==============================================================*/
create table LTC_NUTRTION72EVAL
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   RECORDDATE           datetime,
   RECORDBY             varchar(10),
   CURRENTWEIGHT        decimal(5,2),
   IDEALWEIGHT          decimal(5,2),
   HEIGHT               int(11),
   BMI                  decimal(5,2),
   DIETARY              varchar(100),
   FEEDING              varchar(100),
   CHEW                 varchar(100),
   SWALLOW              varchar(100),
   BREAKFAST            varchar(100),
   LUNCH                varchar(100),
   DINNER               varchar(100),
   SNACK                varchar(100),
   LIKEFOOD             varchar(200),
   NOTLIKEFOOD          varchar(200),
   ALLERGICFOOD         varchar(200),
   GASTROINTESTINAL     varchar(200),
   FUNCTIONALEVAL       varchar(200),
   FATREDUCTION         varchar(200),
   MUSCLEWEAK           varchar(200),
   EDEMA                varchar(200),
   ASCITES              varchar(200),
   BEDSORE              varchar(200),
   BEDSORELEVEL         varchar(10),
   EVALRESULT           varchar(200),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_NUTRTIONCAREREC                                   */
/*==============================================================*/
create table LTC_NUTRTIONCAREREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   RECORDDATE           datetime,
   DIETPATTERN          varchar(100),
   NUTRTIONPATHWAY      varchar(10),
   DIETWAY              varchar(10),
   DINNERFREQ           varchar(50),
   ACTIVITYABILITY      varchar(50),
   WEIGHT               decimal(6,2),
   OTHERDISEASE         varchar(200),
   BMI                  decimal(5,2),
   WEIGHTEVAL           varchar(50),
   DIETSTATE            varchar(50),
   WATERUPTAKE          decimal(10,2),
   KCAL                 int(11),
   KCALTYPE             varchar(50) comment '能量需求类型',
   KCALFOOD             int(11),
   KCALFISH             int(11),
   KCALVEGETABLES       int(11),
   KCALFRUIT            int(11),
   KCALGREASE           int(11),
   PROTEIN              int(11),
   SALINITY             varchar(50),
   PIPEKCAL             int(11),
   PIPEPROTEIN          int(11),
   PIPLEWATER           varchar(10),
   PIPLEVIT             varchar(10),
   PIPLEOTHERWATER      varchar(10),
   NUTRTIONDIAG         varchar(200),
   SPECIALDIET          varchar(200),
   SUGGESTION           varchar(200),
   DIETITIAN            varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_NUTRTIONEVAL                                      */
/*==============================================================*/
create table LTC_NUTRTIONEVAL
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   NAME                 varchar(50),
   EVALDATE             datetime,
   BIRTHDATE            datetime,
   SEX                  varchar(10),
   RESIDENGNO           varchar(10),
   BEDNO                varchar(10),
   INDATE               datetime,
   DISEASEDIAG          varchar(200),
   CHEWDIFFCULT         varchar(200),
   SWALLOWABILITY       varchar(200),
   EATPATTERN           varchar(200),
   DIGESTIONPROBLEM     varchar(200),
   FOODTABOO            varchar(200),
   ACTIVITYABILITY      varchar(200),
   PRESSURESORE         varchar(10),
   EDEMA                varchar(10),
   CURRENTDIET          varchar(200),
   EATAMOUNT            varchar(200),
   WATER                varchar(100),
   SUPPLEMENTS          varchar(200),
   SNACK                varchar(200),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_ORG                                               */
/*==============================================================*/
create table LTC_ORG
(
   ORGID                varchar(10) not null,
   GROUPID              varchar(10),
   ORGNAME              varchar(200),
   ORGTYPE              varchar(4),
   RESPONSIBLE          varchar(20),
   TEL                  varchar(20),
   FAX                  varchar(20),
   BEDCOUNT             int(11),
   EMAIL                varchar(50),
   WEBSITE              varchar(50),
   STATUS               tinyint(1),
   primary key (ORGID)
);

/*==============================================================*/
/* Table: LTC_ORGFLOOR                                          */
/*==============================================================*/
create table LTC_ORGFLOOR
(
   FLOORID              varchar(10) not null,
   FLOORNAME            varchar(60),
   SHOWNUMBER           int(11),
   ORGID                varchar(10),
   primary key (FLOORID)
);

/*==============================================================*/
/* Table: LTC_ORGROOM                                           */
/*==============================================================*/
create table LTC_ORGROOM
(
   ROOMNO               varchar(10) not null,
   FLOORID              varchar(10),
   ROOMNAME             varchar(60),
   SHOWNUMBER           int(11),
   ORGID                varchar(10),
   primary key (ROOMNO)
);

/*==============================================================*/
/* Table: LTC_OUTVALUE                                          */
/*==============================================================*/
create table LTC_OUTVALUE
(
   OUTNO                bigint(20) not null auto_increment,
   FEENO                bigint(20),
   RECDATE              datetime,
   CLASSTYPE            varchar(10),
   OUTTYPE              varchar(10),
   OUTVALUE             int(11),
   COMMDESC             varchar(200),
   RECORDBY             varchar(10),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (OUTNO)
);

/*==============================================================*/
/* Table: LTC_PAINBODYPART                                      */
/*==============================================================*/
create table LTC_PAINBODYPART
(
   ID                   bigint(20) not null auto_increment,
   SEQNO                bigint(20),
   PAINPART             varchar(20),
   PAINNATURE           varchar(20),
   OCCURCHANCE          varchar(20),
   OCCURREASON          varchar(30),
   PAINSEVERITY_CURRENT varchar(10),
   PAINSEVERITY_MOST    varchar(10),
   PAINSEVERITY_LOW     varchar(10),
   PAINSEVERITY_BEAR    varchar(10),
   PAINDEPTH            varchar(20),
   PAINEXTENSION        varchar(20),
   STARTPAIN            varchar(20),
   PAINFREQUENCY        varchar(40),
   PAINDURATION         varchar(40),
   MOSTPAIN1DAY         varchar(40),
   EASEPAINWAY          varchar(40),
   PAINSERIOUSFACTOR    varchar(40),
   SYMPTOM              varchar(40),
   AFFECT_SLEEP         varchar(40),
   AFFECT_ACTIVITY      varchar(40),
   AFFECT_EATING        varchar(40),
   AFFECT_ATTENTION     varchar(40),
   AFFECT_EMOTION       varchar(40),
   AFFECT_RELATIONS     varchar(40),
   AFFECT_OTHERS        varchar(40),
   CANCELFLAG           tinyint(1),
   CANCELDATE           datetime,
   CANCELREASON         varchar(50),
   PICTURE              varchar(80),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_PAINEVALREC                                       */
/*==============================================================*/
create table LTC_PAINEVALREC
(
   SEQNO                bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   EVALDATE             datetime,
   NEXTEVALDATE         datetime,
   NEXTEVALUATEBY       varchar(10),
   DIAGDESC             varchar(254),
   TRANSFERPART         varchar(254),
   COMA_E               varchar(50),
   COMA_M               varchar(50),
   COMA_V               varchar(50),
   GCS                  varchar(50),
   CONSCIOUSNESS        varchar(50),
   PAINEXPRESSION       varchar(50),
   PAINREACTION         varchar(50),
   CANCELFLAG           tinyint(1),
   CANCELDATE           datetime,
   CANCELREASON         varchar(50),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: LTC_PAYBILL                                           */
/*==============================================================*/
create table LTC_PAYBILL
(
   ID                   bigint(20) not null auto_increment,
   BILLID               bigint(20) not null,
   PAYBILLNO            varchar(20) not null,
   PAYBILLTIME          datetime,
   PAYOR                varchar(20),
   COST                 decimal(18,2),
   INVOICENO            varchar(50),
   ACCOUNTANTCODE       varchar(50),
   RECRODBY             varchar(10),
   SUMMARY              decimal(18,2),
   RECEIVED             decimal(18,2),
   BILLSTATUS           varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_PHARMACISTEVAL                                    */
/*==============================================================*/
create table LTC_PHARMACISTEVAL
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   EVALDATE             datetime,
   INTERVALDAY          int(11),
   NEXTEVALDATE         datetime,
   EVALUATEBY           varchar(10),
   DISEASEDESC          text,
   PIPLELINEDESC        varchar(200),
   VITALSIGNS           varchar(200),
   M3VISITREC           varchar(200),
   DRUGRECORDS          text,
   YEARSOLD85           tinyint(1),
   DRUG9TYPE            tinyint(1),
   ADRSFLAG             tinyint(1),
   SPECTYPEDRGFLAG      tinyint(1),
   SPECTYPEDRUGDESC     varchar(254),
   SPECDRUGFLAG         tinyint(1),
   SPECDRUGDESC         varchar(254),
   INTERACTION          text,
   SUGGESTION           text,
   MILLSFLAG            tinyint(1),
   DEPTVISIT3           tinyint(1),
   NEXTEVALUATEBY       varchar(10),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   MULTIPLEFLAG         varchar(20),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_PINMONEY                                          */
/*==============================================================*/
create table LTC_PINMONEY
(
   ID                   int(11) not null auto_increment,
   IENO                 varchar(20),
   ITEMSUMMARY          varchar(50),
   IETPYE               varchar(10),
   COST                 decimal(18,2),
   IEDATE               datetime,
   FACTDATE             datetime,
   DESCRIPTION          varchar(200),
   REGNO                int(11) not null,
   FEENO                bigint(20) not null,
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_PIPELINE                                          */
/*==============================================================*/
create table LTC_PIPELINE
(
   SEQ                  bigint(20) not null,
   KIND                 varchar(500),
   FEATHER              varchar(50),
   AGUGE                varchar(10),
   NAME                 varchar(50),
   primary key (SEQ)
);

/*==============================================================*/
/* Table: LTC_PIPELINEEVAL                                      */
/*==============================================================*/
create table LTC_PIPELINEEVAL
(
   ID                   bigint(20) not null auto_increment,
   EVALDATE             datetime,
   RECENTDATE           datetime,
   PIPELINETYPE         varchar(10),
   INTERVALDAY          int(11),
   NEXTDATE             datetime,
   STATE                varchar(20),
   OPERATOR             varchar(10),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   SEQNO                int(11),
   SEQ                  bigint(20),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_PIPELINEREC                                       */
/*==============================================================*/
create table LTC_PIPELINEREC
(
   SEQNO                int(11) not null auto_increment,
   FEENO                bigint(20),
   RECORDDATE           datetime,
   PIPELINENAME         varchar(50),
   OPERATOR             varchar(10),
   REMOVEFLAG           tinyint(1),
   REMOVETRAIN          varchar(50),
   REMOVEDFLAG          tinyint(1),
   REMOVEDATE           datetime,
   REMOVEREASON         varchar(200),
   REMOVEBY             varchar(10),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: LTC_PREIPD                                            */
/*==============================================================*/
create table LTC_PREIPD
(
   PREFEENO             bigint(20) not null auto_increment,
   RECSTATUS            char(1),
   SOURCETYPE           varchar(10),
   INSMARK              varchar(10),
   PREDATE              datetime,
   DEPTNO               varchar(10),
   BEDNO                varchar(10),
   COMMDESC             varchar(500),
   WAITFLAG             tinyint(1),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   ORGID                varchar(10) not null,
   CONTACTTEL           varchar(50),
   CONTACTNAME          varchar(50),
   CANCELREASON         varchar(50),
   PNAME                varchar(50),
   SEX                  varchar(10) not null,
   AGE                  int(11),
   IDNO                 varchar(18),
   Phone                varchar(50),
   primary key (PREFEENO)
);

/*==============================================================*/
/* Table: LTC_PROPOSALDISSCUSS                                  */
/*==============================================================*/
create table LTC_PROPOSALDISSCUSS
(
   RECORDID             bigint(20) not null,
   FEENO                bigint(20),
   REGNO                int(11),
   PROPOSALDATE         datetime,
   PROPOSALBY           varchar(10),
   NEXTPROPOSALDATE     datetime,
   PERSONALHIST         text,
   PROBLEMANALYSIS      text,
   RESOURCEEVAL         text,
   PROCESSPLAN          text,
   ASSESSMENTS          text,
   DESCRIPTION          text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   TITLE                varchar(200),
   ORGID                varchar(10),
   primary key (RECORDID)
);

/*==============================================================*/
/* Table: LTC_PROPOSALDISSCUSSREC                               */
/*==============================================================*/
create table LTC_PROPOSALDISSCUSSREC
(
   ID                   bigint(20) not null,
   MEETINGDATE          datetime,
   RECORDBY             varchar(20),
   MEETCHAIRMAN         varchar(10),
   ATTENDANCE           text,
   GUIDTEACHER          varchar(10),
   MEETINGADDRESS       varchar(100),
   MEETINGTITLE         varchar(500),
   QUESTION             text,
   QUESTIONDIAG         text,
   TEACHERSUGGESTION    text,
   QUESTIONDISSCUSSION  text,
   DECISIONS            text,
   MOTION               text,
   DESCRIPTION          text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   RECORDID             bigint(20),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_PUSHTOKEN                                         */
/*==============================================================*/
create table LTC_PUSHTOKEN
(
   ID                   int(11) not null auto_increment,
   USERID               int(11),
   OS                   smallint(6),
   TOKEN                varchar(256),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_QUESTION                                          */
/*==============================================================*/
create table LTC_QUESTION
(
   ID                   int(8) not null auto_increment,
   QUESTIONID           int(11) not null,
   QUESTIONNAME         varchar(200),
   SHOWNUMBER           int(11),
   ISSHOW               tinyint(1),
   QUESTIONDESC         varchar(500),
   CATEGORYID           int(11),
   CODE                 varchar(10),
   SCOREFLAG            tinyint(1),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Index: QUESTIONID                                            */
/*==============================================================*/
create index QUESTIONID on LTC_QUESTION
(
   QUESTIONID
);

/*==============================================================*/
/* Table: LTC_QUESTIONRESULTS                                   */
/*==============================================================*/
create table LTC_QUESTIONRESULTS
(
   RESULTID             int(11) not null auto_increment,
   QUESTIONID           int(11),
   LOWBOUND             decimal(18,2),
   UPBOUND              decimal(18,2),
   RESULTNAME           varchar(100),
   primary key (RESULTID)
);

/*==============================================================*/
/* Index: R_24                                                  */
/*==============================================================*/
create index R_24 on LTC_QUESTIONRESULTS
(
   QUESTIONID
);

/*==============================================================*/
/* Table: LTC_RECEIPTS                                          */
/*==============================================================*/
create table LTC_RECEIPTS
(
   ID                   bigint(20) not null auto_increment,
   IDNO                 varchar(20),
   UNDERTAKER           varchar(10),
   ITEMSUMMARY          varchar(50),
   IETPYE               varchar(10),
   COST                 decimal(18,2),
   INCOMETYPE           varchar(10),
   PAYOR                varchar(10),
   IEDATE               datetime,
   FACTDATE             datetime,
   DESCRIPTION          varchar(200),
   REGNO                int(11) not null,
   FEENO                bigint(20) not null,
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_REFERRALREC                                       */
/*==============================================================*/
create table LTC_REFERRALREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   RECDATE              datetime,
   UNITNAME             varchar(80),
   UNITCONTACTOR        varchar(20),
   UNITADDRESS          varchar(80),
   UNITTEL              varchar(30),
   REASON               varchar(200),
   QUESTIONSUMMARY      text,
   SERVICESUMMARY       text,
   SUGGESTION           text,
   EMPNO                varchar(10),
   REPLYDATE            datetime,
   REPLYDESC            varchar(200),
   REPLYTYPE            varchar(50),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_REGACTIVITYREQUEVAL                               */
/*==============================================================*/
create table LTC_REGACTIVITYREQUEVAL
(
   ID                   bigint(20) not null auto_increment,
   INDATE               datetime,
   EVALDATE             datetime,
   CARER                varchar(10),
   VISION               varchar(10),
   SMELL                varchar(10),
   SENSATION            varchar(10),
   TASTE                varchar(10),
   HEARING              varchar(10),
   UPPERLIMB            varchar(10),
   LOWERLIMB            varchar(10),
   HALLUCINATION        varchar(10),
   DELUSION             varchar(10),
   ATTENTION            varchar(10),
   DIRECTIONSENSE       varchar(10),
   DIRECTMAN            tinyint(4),
   DIRECTTIME           tinyint(4),
   DIRECTADDRESS        tinyint(4),
   COMPREHENSION        varchar(10),
   MEMORY               varchar(10),
   MEMORYFLAG           tinyint(4),
   EXPRESSION           varchar(10),
   OTHERNARRATIVE       varchar(50),
   EMOTION              varchar(200),
   SELF                 varchar(200),
   BEHAVIORCONTENT      varchar(200),
   BEHAVIORFREQ         varchar(200),
   ACTIVITY             varchar(200),
   TALKEDWILLING        varchar(200),
   NOTTALKED            varchar(200),
   ARTACTIVITY          varchar(200),
   AIDSACTIVITY         varchar(200),
   SEVEREACTIVITY       varchar(200),
   TALKEDNOWILLING      varchar(10),
   FEENO                bigint(20),
   REGNO                int(11),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_REGATTACHFILE                                     */
/*==============================================================*/
create table LTC_REGATTACHFILE
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20) not null,
   DOCTYPE              varchar(10),
   DOCPATH              varchar(200),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   REGNO                int(11),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_REGDEMAND                                         */
/*==============================================================*/
create table LTC_REGDEMAND
(
   ID                   int(11) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   REGNAME              varchar(50),
   RECORDDATE           datetime,
   DEMANDTYPE           varchar(50),
   CONTENT              varchar(500),
   EXECUTEBY            varchar(10),
   EXECUTORNAME         varchar(50),
   FINISHFLAG           tinyint(1),
   FINISHDATE           datetime,
   UNFINISHREASON       varchar(200),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10) not null,
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_REGDISEASEHIS                                     */
/*==============================================================*/
create table LTC_REGDISEASEHIS
(
   REGNO                int(11) not null comment '同REGFILE表 REGNO',
   HAVEOPERATION        tinyint(1) comment '有無手術 0：無 1：有',
   OPERATION            varchar(100) comment '手術, 当手术栏位勾选“有”时填写',
   HAVEDRUGALLERGY      tinyint(1) comment '有無药物过敏 0：無1：有',
   DRUGALLERGY          varchar(100) comment '药物过敏',
   HAVEFOODALLERGY      tinyint(1) comment '有無食物过敏 0：無 1：有',
   FOODALLERGY          varchar(100) comment '食物过敏',
   HAVETRANSFUSION      tinyint(1) comment '有無输血 0：無 1：有',
   ORIGMEDICALHOS       varchar(50) comment '原就医及领药医院',
   EMERGENCYTRANSTO     varchar(50) comment '紧急转介至',
   ISAGREETRANSFER      tinyint(1) comment '是否同意 紧急转介至 指定转介医院 1：同意 0：不同意',
   MEDICALHIS           varchar(50) comment '过去药物史，记录code 逗号分隔',
   OTHERS               varchar(200) comment '其他',
   primary key (REGNO)
);

/*==============================================================*/
/* Table: LTC_REGDISEASEHISDTL                                  */
/*==============================================================*/
create table LTC_REGDISEASEHISDTL
(
   ID                   int(11) not null auto_increment,
   REGNO                int(11) comment '住民编号',
   CATEGORYID           int(11) comment '对应LTC_SCENARIO_ITEM表CATEGORY',
   ITEMID               int(11) comment '对应LTC_SCENARIO_ITEM表ITEMID',
   OTHERITEMNAME        varchar(20) comment '其他疾病名称，手动输入疾病项目',
   SCENARIOOPTIONIDS    varchar(10) comment '对应LTC_SCENARIO_ITEM_OPTION表ID集合',
   SICKTIME             date comment '患病时间',
   ORGITREATMENTHOS     varchar(50) comment '过去治疗医院',
   EXPECTTRANSFERTO     varchar(50) comment '预计转诊医院',
   HAVECURE             tinyint(1) comment '是否治愈 1：治愈 0：未治愈',
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_REGEVALUATE                                       */
/*==============================================================*/
create table LTC_REGEVALUATE
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   EVALDATE             datetime,
   NEXTEVALDATE         datetime,
   COUNT                int(11),
   EVALUATEBY           varchar(10),
   NEXTEVALUATEBY       varchar(10),
   AIDTOOLS             text,
   BOOKTYPE             text,
   BOOKDEGREE           varchar(10),
   ILLCARDNAME          text,
   SERVICENAME          text,
   MINDSTATE            varchar(10),
   COGNITIVESTATE       text,
   EXPRESSIONSTATE      varchar(10),
   NONEXPRESSIONSTATE   varchar(10),
   LANGUAGESTATE        varchar(10),
   EMOTIONSTATE         varchar(10),
   PERSONALITY          varchar(10),
   ATTENTION            varchar(10),
   REALISTICSENSE       varchar(10),
   SOCIALPARTICIPATION  varchar(10),
   SOCIALATTITUDE       varchar(10),
   SOCIALSKILLS         varchar(10),
   COMMSKILLS           varchar(10),
   RESPONSESKILLS       varchar(10),
   FIXISSUESKILLS       varchar(10),
   HOBBY                text,
   EXPERTISE            text,
   SPECIALBEHAVIOR      text,
   ADMISSIONREASON      text,
   MEDICALHISTORY       text,
   PSYCHOLOGYDESC       text,
   FAMILYSUBSYSTEM      text,
   SOCIALRELATIONSHIP   text,
   KEYQUESTION          text,
   PROCESSPLAN_S        text,
   RELATIVESDISCUSS     varchar(200),
   DESCRIPTION          text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   VERIFYDATE           datetime,
   VERIFYBY             varchar(10),
   FAMILYFUNROLE        text,
   FAMILYSUPPORT        text,
   FAMILYFINANCIAL      text,
   SOCIALSUPPORT        text,
   SOCIALFORMALRESOURCE text,
   PROCESSPLAN_M        text,
   PROCESSPLAN_L        text,
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_REGFILE                                           */
/*==============================================================*/
create table LTC_REGFILE
(
   REGNO                int(11) not null,
   NAME                 varchar(50),
   NICKNAME             varchar(50),
   RESIDENGNO           varchar(20),
   SEX                  varchar(10),
   BRITHDATE            datetime,
   AGE                  int(11),
   IDNO                 varchar(18),
   EDUCATION            varchar(10),
   SKILL                varchar(200),
   WORKCODE             varchar(10),
   EXPERIENCE           varchar(200),
   TITLE                varchar(10),
   HABIT                varchar(200),
   RELIGIONCODE         varchar(10),
   LANGUAGE             varchar(100),
   COMMUNICATETYPE      varchar(10),
   BRITHPLACE           varchar(20),
   MERRYFLAG            varchar(10),
   CHILD_BOY            int(11),
   CHILD_GIRL           int(11),
   CAREGIVER            varchar(10),
   LIVECONDITION        varchar(10),
   CONSTELLATIONS       varchar(10),
   BLOODTYPE            varchar(10),
   HEIGHT               decimal(5,2),
   WEIGHT               decimal(5,2),
   PERSONALHISTORY      text,
   FAMILYHISTORY        varchar(254),
   INFECFLAG            text,
   RHTYPE               varchar(10),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   ORGID                varchar(10) not null,
   DISEASEDIAG          text,
   primary key (REGNO)
);

/*==============================================================*/
/* Table: LTC_REGHEALTH                                         */
/*==============================================================*/
create table LTC_REGHEALTH
(
   FEENO                bigint(20) not null,
   REGNO                int(11),
   ALLERGY              text,
   HEALTHINFO           text,
   EATHABITS            varchar(10),
   CAL                  decimal(14,2),
   COOKFLAG             tinyint(1),
   NOTCOOKREASON        varchar(10),
   STARTDATE            datetime,
   ENDDATE              datetime,
   CATHETERFLAG         tinyint(1),
   TRACHEOSTOMYFLAG     tinyint(1),
   NASOGASTRICFLAG      tinyint(1),
   DEMANDDESC           varchar(500),
   ORGID                varchar(10) not null,
   CATHETERSIZE         varchar(20),
   TRACHEOSTOMYSIZE     varchar(20),
   NASOGASTRICSIZE      varchar(20),
   STOMAFLAG            tinyint(1),
   STOMASIZE            varchar(20),
   primary key (FEENO)
);

/*==============================================================*/
/* Table: LTC_REGINSDTL                                         */
/*==============================================================*/
create table LTC_REGINSDTL
(
   FEENO                bigint(20) not null,
   SOURCETYPE           varchar(10),
   INSTYPE              varchar(10),
   INREASON             varchar(10),
   INTYPE               varchar(10),
   ECONOMYFLAG          varchar(10),
   CASETYPE             varchar(10),
   HOSPFILE             varchar(200),
   INSMARK              varchar(10),
   CUSTODYFLAG          tinyint(1),
   ASSISTFLAG           tinyint(1),
   LPAPPDATE            datetime,
   LPCHECKDATE          datetime,
   APPDOCNO             varchar(100),
   ILLFLAG              text,
   CERTIFYNO            varchar(50),
   NEEDCERTIFY          tinyint(1),
   CERTIFYDATE          datetime,
   BOOKFLAG             tinyint(1),
   BOOKNO               varchar(50),
   BOOKISSUEDATE        datetime,
   BOOKEXPDATE          datetime,
   BOOKTYPE             text,
   DISABDEGREE          varchar(4),
   IQ                   varchar(50),
   DISABCAUSE           text,
   DISABTYPEDTL         text,
   DISABCHECKDESC       text,
   PROCDOC              text,
   INSFLAG              tinyint(1),
   INSURANCEDESC        varchar(500),
   DOCLACKDESC          text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10) not null,
   PTYPE                varchar(10),
   REGNO                int(11),
   DISABILITYEXPDATE    date comment '有效期限',
   DISABILITYGRADE      varchar(10) comment '障碍等级（新制）',
   DISABILITYEVALDATE   date comment '障碍鉴定日期（新制）',
   ISREEVAL             tinyint(1) comment '是否需要重新鉴定 1：需要 0：不需要',
   DISABILITYREEVALDATE date comment '重新鉴定日期（新制）',
   ISHOUSEMIGRATION1    tinyint(1) comment '是否户籍迁移 1：是 0：否（新制）',
   ISHOUSEMIGRATION2    tinyint(1) comment '是否户籍迁移 1：是 0：否（新制）',
   INMIGRATIONDATE1     date comment '迁入日期1（新制）',
   INMIGRATIONDATE2     date comment '迁入日期2（新制）',
   DISABILITYCATE1      tinyint(1) comment '障碍类别1 1:选中 0:未选中',
   DISABILITYCATE2      tinyint(1),
   DISABILITYCATE3      tinyint(1),
   DISABILITYCATE4      tinyint(1),
   DISABILITYCATE5      tinyint(1),
   DISABILITYCATE6      tinyint(1),
   DISABILITYCATE7      tinyint(1),
   DISABILITYCATE8      tinyint(1),
   ICFCODE1             varchar(50) comment 'ICF编码 对应DISABILITYCATE1，多个时使用逗号分隔',
   ICFCODE2             varchar(50),
   ICFCODE3             varchar(50),
   ICFCODE4             varchar(50),
   ICFCODE5             varchar(50),
   ICFCODE6             varchar(50),
   ICFCODE7             varchar(50),
   ICFCODE8             varchar(50),
   ICDDIAGNOSI          varchar(50) comment 'ICD诊断',
   primary key (FEENO)
);

/*==============================================================*/
/* Table: LTC_REGQUESTION                                       */
/*==============================================================*/
create table LTC_REGQUESTION
(
   RECORDID             bigint(20) not null auto_increment,
   RECORDDATE           datetime,
   FEENO                bigint(20),
   REGNO                int(11),
   QUESTIONID           int(11),
   EVALNUMBER           int(11),
   SCORE                decimal(8,2),
   ENVRESULTS           varchar(200),
   DESCRIPTION          text,
   EVALUATEBY           varchar(10),
   EVALDATE             datetime,
   NEXTEVALDATE         datetime,
   NEXTEVALUATEBY       varchar(10),
   NOTEVALREASON        text,
   ORGID                varchar(10),
   CAUSEREASON          varchar(50),
   LASTCOMPARE          varchar(50),
   primary key (RECORDID)
);

/*==============================================================*/
/* Table: LTC_REGQUESTIONDATA                                   */
/*==============================================================*/
create table LTC_REGQUESTIONDATA
(
   ID                   bigint(20) not null auto_increment,
   RECORDID             bigint(20),
   QUESTIONID           int(11),
   MAKERID              int(11),
   MAKERVALUE           decimal(18,2),
   LIMITEDVALUEID       int(11),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_REGRELATION                                       */
/*==============================================================*/
create table LTC_REGRELATION
(
   FEENO                bigint(20) not null,
   REGNO                int(11),
   ZIP1                 varchar(10),
   ADDRESS1             varchar(50),
   ADDRESS1DTL          varchar(100),
   ZIP2                 varchar(10),
   ADDRESS2             varchar(50),
   ADDRESS2DTL          varchar(100),
   HOUSEHOLDFLAG        tinyint(1),
   CONTACTPHONE         varchar(200),
   FAX                  varchar(50),
   MOBILE               varchar(50),
   EMAIL                varchar(50),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   PHOTOPATH            varchar(200),
   FAMILYPATH           varchar(200),
   CITY1                varchar(50),
   CITY2                varchar(50),
   PAYMENTPERSON        varchar(50),
   KINSHIP              varchar(10),
   BILLADDRESS          varchar(200),
   primary key (FEENO)
);

/*==============================================================*/
/* Table: LTC_REGRELATIONDTL                                    */
/*==============================================================*/
create table LTC_REGRELATIONDTL
(
   ID                   bigint(20) not null auto_increment,
   NAME                 varchar(20),
   SEX                  varchar(10),
   BIRTHDATE            datetime,
   CONTREL              varchar(10),
   KINSHIP              varchar(10),
   RELATIONTYPE         varchar(10),
   LIVINGFLAG           tinyint(1),
   EDUCATION            varchar(10),
   MERRYFLAG            varchar(10),
   HEALTHFLAG           varchar(10),
   DEATHFLAG            varchar(10),
   WORKCODE             varchar(10),
   ECONOMYFLAG          tinyint(1),
   IDNO                 varchar(18),
   ZIP                  varchar(10),
   ADDRESS              varchar(200),
   ADDRESS2             varchar(200),
   PHONE                varchar(500),
   FAX                  varchar(20),
   SKYPE                varchar(20),
   MSN                  varchar(20),
   EMAIL                varchar(20),
   FEENO                bigint(20),
   ECONOMYABILITY       varchar(50),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Index: R_49                                                  */
/*==============================================================*/
create index R_49 on LTC_REGRELATIONDTL
(
   FEENO
);

/*==============================================================*/
/* Table: LTC_REHABILITREC                                      */
/*==============================================================*/
create table LTC_REHABILITREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   RECDATE              datetime,
   INTERVALDAY          int(11),
   RECORDBY             varchar(10),
   NEXTRECORDBY         varchar(10),
   HOSPNAME             varchar(200),
   ITEMNAME             varchar(200),
   ASSESSMENT           text,
   DESCRIPTION          text,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   NEXTRECDATE          datetime,
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_RESOURCELINKREC                                   */
/*==============================================================*/
create table LTC_RESOURCELINKREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   RECORDBY             varchar(10),
   CONTACTDATE          datetime,
   FINISHDATE           datetime,
   TYPE                 varchar(50),
   NAME                 varchar(100),
   EVALRESULT           varchar(20),
   UNIT                 varchar(200),
   RESOURCESTATUS       varchar(50),
   REASON               varchar(50),
   REGSTATE             varchar(20),
   DESCRIPTION          varchar(254),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_ROLEMODULES                                       */
/*==============================================================*/
create table LTC_ROLEMODULES
(
   ROLEID               varchar(10) not null,
   MODULEID             varchar(40) not null,
   primary key (MODULEID, ROLEID)
);

/*==============================================================*/
/* Table: LTC_ROLES                                             */
/*==============================================================*/
create table LTC_ROLES
(
   ROLEID               varchar(10) not null,
   ROLENAME             varchar(50),
   ROLETYPE             varchar(10),
   DESCRIPTION          varchar(256),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   STATUS               tinyint(1),
   ORGID                varchar(10) not null,
   SYSTYPE              varchar(10),
   primary key (ROLEID)
);

/*==============================================================*/
/* Table: LTC_SCENARIO                                          */
/*==============================================================*/
create table LTC_SCENARIO
(
   ID                   int(11) not null,
   SCENARIO             int(11) default 1 comment '场景类别 1: 疾病史',
   CATEGORYID           int(11) not null auto_increment comment '类别',
   CATEGORYNAME         varchar(20) comment '类别名称',
   primary key (CATEGORYID)
);

/*==============================================================*/
/* Table: LTC_SCENARIO_ITEM                                     */
/*==============================================================*/
create table LTC_SCENARIO_ITEM
(
   ID                   int(11) not null auto_increment,
   CATEGORYID           int(11) not null comment '对应LTC_SCENARIO表CATEGORYID',
   GROUPID              int(4) default 0 comment '分组 0：非正常 1：正常',
   ITEMID               int(11) comment '项目ID（病种ID）',
   ITEMNAME             varchar(20) comment '项目名称（病种ID）',
   primary key (ID)
);

/*==============================================================*/
/* Index: ITEMID                                                */
/*==============================================================*/
create index ITEMID on LTC_SCENARIO_ITEM
(
   ITEMID
);

/*==============================================================*/
/* Table: LTC_SCENARIO_ITEM_OPTION                              */
/*==============================================================*/
create table LTC_SCENARIO_ITEM_OPTION
(
   ID                   int(11) not null auto_increment,
   SCENARIOITEMID       int(11) comment '对应LTC_SCENARIO_ITEM表ID',
   OPTIONID             int(11) comment 'LTC_SCENARIO_ITEM中ITEM子项',
   OPTIONNAME           varchar(20),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_SUBSIDYREC                                        */
/*==============================================================*/
create table LTC_SUBSIDYREC
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   ITEMNAME             varchar(200),
   APPLYDATE            datetime,
   NEXTAPPLYDATE        datetime,
   DESCRIPTION          text,
   APPLYBY              varchar(10),
   NEXTAPPLYBY          varchar(10),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_SYMPOTMITEM                                       */
/*==============================================================*/
create table LTC_SYMPOTMITEM
(
   ID                   varchar(20) not null,
   SYMPOTMCODE          varchar(20),
   SYMPOTMNAME          varchar(200),
   ITEMCODE             varchar(20),
   SCORE                int(11),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_TRANSFERVISIT                                     */
/*==============================================================*/
create table LTC_TRANSFERVISIT
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   TRANSDATE            datetime,
   DEPTNO               varchar(10),
   DIAGNOSIS            varchar(300),
   DISEASECOUNT         int(11),
   CURRENTISSUE         text,
   HEIGHT               int(11),
   WEIGHT               int(11),
   KNEELENGTH           int(11),
   WEIGHT_S             int(11),
   BMI                  int(11),
   BODYTEMP             int(11),
   PULSE                int(11),
   BP                   int(11),
   BLOODOXYGEN          int(11),
   BS                   int(11),
   EKG                  int(11),
   EAT                  varchar(20),
   SWALLOW              varchar(20),
   DIETMODE             varchar(20),
   STOMACH              varchar(20),
   FOODTABOO            varchar(20),
   ACTIVITY             varchar(20),
   CURRENTDIET          varchar(20),
   CURRENTDIETSTATE     varchar(20),
   FOODINTAKE           varchar(20),
   WATER                varchar(20),
   NUTRITION            varchar(20),
   SNACKS               varchar(20),
   PIPLELINE            varchar(20),
   MEDICINE             varchar(20),
   CHECKDESC            varchar(20),
   ASSESSDATE           datetime,
   ASSESSBY             varchar(20),
   REPLYDATE            datetime,
   SUPERVISOR           varchar(10),
   SUGGESTSUMMARY       varchar(20),
   PROCESSDESC          varchar(20),
   PROCESSRESULTS       varchar(20),
   CONSULTDEPT          varchar(20),
   CONSULTTYPE          varchar(20),
   CONSULTDATE          datetime,
   CONSULTITEM          varchar(20),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_UNPLANEDIPD                                       */
/*==============================================================*/
create table LTC_UNPLANEDIPD
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   RECORDBY             varchar(10),
   INDATE               datetime,
   CHARGEFLAG           tinyint(1),
   IPDDIAG              varchar(200),
   EPDFLAG              tinyint(1),
   IPDCAUSE             varchar(200),
   H72IPD               tinyint(1),
   UNPLANFLAG           tinyint(1),
   CATHETERFLAG         tinyint(1),
   HOSPNAME             varchar(50),
   ESCORTPEOPLE         varchar(10),
   ESCORTRELATION       varchar(50),
   IPDDESC              varchar(500),
   MRSUMMARY            varchar(500),
   DEPOSITAMT           decimal(10,2),
   DEPOSITDESC          varchar(20),
   OUTREASON            varchar(20),
   OUTFLAG              tinyint(1),
   OUTDATE              datetime,
   IPDDAYS              int(11),
   PAYOUTWAY            varchar(20),
   PAYAMT               decimal(20,2),
   BEDTYPE              varchar(20),
   DESCRIPTION          text,
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_UNPLANWEIGHTIND                                   */
/*==============================================================*/
create table LTC_UNPLANWEIGHTIND
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   THISWEIGHT           decimal(5,1),
   THISHEIGHT           decimal(5,1),
   WAIST                decimal(5,1),
   HIPLINE              decimal(5,1),
   THISRECDATE          datetime,
   LASTWEIGHT           decimal(5,1),
   DIFFVALUE            decimal(5,1),
   DIFFVALUERATE        decimal(5,2),
   LASTRECDATE          datetime,
   BMI                  decimal(5,1),
   BMIRESULTS           varchar(50),
   B3WEIGHT             decimal(5,1),
   B3DIFFVALUE          decimal(5,1),
   B3DIFFVALUERATE      decimal(5,2),
   B6WEIGHT             decimal(5,1),
   B6DIFFVALUE          decimal(5,1),
   B6DIFFVALUERATE      decimal(5,2),
   UNPLANFLAG           tinyint(1),
   RECORDBY             varchar(10),
   ORGID                varchar(10),
   KNEELEN              decimal(5,1),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_USERORG                                           */
/*==============================================================*/
create table LTC_USERORG
(
   USERID               int(11) not null,
   ORGID                varchar(10) not null,
   primary key (ORGID, USERID)
);

/*==============================================================*/
/* Table: LTC_USERROLES                                         */
/*==============================================================*/
create table LTC_USERROLES
(
   USERID               int(11) not null,
   ROLEID               varchar(10) not null,
   primary key (ROLEID, USERID)
);

/*==============================================================*/
/* Table: LTC_USERS                                             */
/*==============================================================*/
create table LTC_USERS
(
   USERID               int(11) not null auto_increment,
   LOGONNAME            varchar(50),
   PWD                  varchar(256),
   PWDEXPDATE           datetime,
   PWDDURATION          int(11),
   LASTLOGONDATE        datetime,
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   UPDATEDATE           datetime,
   UPDATEBY             varchar(10),
   STATUS               tinyint(1),
   EMPNO                varchar(10) not null,
   ISORGCTRL            tinyint(1) not null,
   PWDENABLE            tinyint(1),
   EMAIL                varchar(50),
   primary key (USERID)
);

/*==============================================================*/
/* Table: LTC_VACCINEINJECT                                     */
/*==============================================================*/
create table LTC_VACCINEINJECT
(
   ID                   bigint(20) not null auto_increment,
   FEENO                bigint(20),
   REGNO                int(11),
   ITEMTYPE             varchar(50),
   STATE                varchar(50),
   TRACESTATE           varchar(50),
   INJECTDATE           datetime,
   INTERVALDAY          int(11),
   NEXTINJECTDATE       datetime,
   DESCRIPTION          text,
   OPERATOR             varchar(10),
   CREATEDATE           datetime,
   CREATEBY             varchar(10),
   NEXTOPERATEBY        varchar(10),
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: LTC_VISITDEPT                                         */
/*==============================================================*/
create table LTC_VISITDEPT
(
   DEPTNO               varchar(10) not null,
   DEPTNAME             varchar(50),
   HOSPNO               varchar(10),
   primary key (DEPTNO)
);

/*==============================================================*/
/* Table: LTC_VISITDOCRECORDS                                   */
/*==============================================================*/
create table LTC_VISITDOCRECORDS
(
   SEQNO                int(11) not null auto_increment,
   VISITDATE            datetime,
   VISITTYPE            varchar(20),
   RECORDBY             varchar(10),
   VISITHOSP            varchar(50),
   VISITDEPT            varchar(50),
   VISITDOCTOR          varchar(50),
   VISITREASON          varchar(50),
   INFECTFLAG           tinyint(1),
   PLANVISITFLAG        tinyint(1),
   TAKEDAYS             int(11),
   STARTDATE            datetime,
   ENDDATE              datetime,
   DISEASETYPE          varchar(50),
   DISEASENAME          varchar(500),
   SYMPTOMS             varchar(200),
   OBJECTIVEDESCRIP     varchar(500),
   DIAGNOSTICEVAL       varchar(500),
   TREATMENT            varchar(500),
   DESCRIPTION          varchar(500),
   NEXTVISITFLAG        tinyint(1),
   NEXTVISITHINT        varchar(200),
   PREREGINFO           varchar(100),
   INTERVALDAY          int(11),
   NEXTVISITDATE        datetime,
   NEXTVISITTYPE        varchar(20),
   ORGID                varchar(10) not null,
   INFECTVISITREASON    varchar(50),
   FEENO                bigint(20),
   REGNO                int(11),
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: LTC_VISITDOCTOR                                       */
/*==============================================================*/
create table LTC_VISITDOCTOR
(
   DOCNO                varchar(10) not null,
   DOCNAME              varchar(50),
   HOSPNO               varchar(10),
   DEPTNO               varchar(10),
   primary key (DOCNO)
);

/*==============================================================*/
/* Table: LTC_VISITHOSPITAL                                     */
/*==============================================================*/
create table LTC_VISITHOSPITAL
(
   HOSPNO               varchar(10) not null,
   HOSPNAME             varchar(50),
   ORGID                varchar(10),
   primary key (HOSPNO)
);

/*==============================================================*/
/* Table: LTC_VISITPRESCRIPTION                                 */
/*==============================================================*/
create table LTC_VISITPRESCRIPTION
(
   PID                  int(11) not null auto_increment,
   SEQNO                int(11),
   MEDID                int(11),
   DOSAGE               varchar(20),
   QTY                  decimal(9,2),
   TAKEQTY              varchar(20),
   TAKEWAY              varchar(20),
   FREQ                 varchar(20),
   FREQTIME             varchar(50),
   FREQDAY              int(11),
   FREQQTY              int(11),
   LONGFLAG             tinyint(1),
   USEFLAG              tinyint(1),
   STARTDATE            datetime,
   ENDDATE              datetime,
   DESCRIPTION          varchar(10),
   ORGID                varchar(10),
   primary key (PID)
);

/*==============================================================*/
/* Table: LTC_VITALSIGN                                         */
/*==============================================================*/
create table LTC_VITALSIGN
(
   SEQNO                bigint(20) not null auto_increment,
   FEENO                bigint(20),
   SBP                  int(11),
   DBP                  int(11),
   PULSE                int(11),
   BODYTEMP             decimal(5,2),
   BREATHE              int(11),
   OXYGEN               int(11),
   BLOODSUGAR           decimal(5,2),
   BSTYPE               varchar(4),
   HEIGHT               decimal(5,2),
   WEIGHT               decimal(5,2),
   COMA                 varchar(6),
   PAIN                 int(11),
   BOWELS               int(11),
   CLASSTYPE            varchar(4),
   RECORDDATE           datetime,
   ORGID                varchar(10) not null,
   primary key (SEQNO)
);

/*==============================================================*/
/* Table: LTC_ZIPFILE                                           */
/*==============================================================*/
create table LTC_ZIPFILE
(
   ID                   int(10) not null,
   CITY                 varchar(50),
   TOWN                 varchar(50),
   POSTCODE             varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: SYS_CODERULE                                          */
/*==============================================================*/
create table SYS_CODERULE
(
   ORGID                varchar(10) not null,
   CODEKEY              varchar(20) not null,
   FLAGRULE             varchar(50) not null,
   GENERATERULE         varchar(50) not null,
   FLAG                 varchar(50),
   SERIALNUMBER         int(11),
   primary key (ORGID, CODEKEY)
);

/*==============================================================*/
/* Table: SYS_REPORT                                            */
/*==============================================================*/
create table SYS_REPORT
(
   ID                   int(11) not null auto_increment,
   CODE                 varchar(20),
   NAME                 varchar(50) not null,
   MAJORTYPE            varchar(20),
   REPORTTYPE           varchar(10),
   FilterType           varchar(10),
   IsFeeNoRequired      tinyint(1),
   SYSTYPE              varchar(2),
   STATUS               tinyint(1) default 1,
   ORGID                varchar(10),
   primary key (ID)
);

/*==============================================================*/
/* Table: TSG_ANSWER                                            */
/*==============================================================*/
create table TSG_ANSWER
(
   ID                   int(11) not null auto_increment,
   QUESTIONID           int(11) comment '问题ID',
   NAME                 varchar(100) comment '答案名称',
   DESCRIPTION          text comment '答案详细描述',
   ORDERSEQ             int(11) comment '答案序号',
   CREATER              varchar(10) comment '创建人',
   CREATETIME           datetime comment '创建时间',
   UPDATER              varchar(10) comment '更新人',
   UPDATETIME           datetime comment '更新时间',
   STATUS               tinyint(1) default 1 comment '是否启用 0:不启用,1:启用',
   primary key (ID)
);

/*==============================================================*/
/* Table: TSG_ANSWERDTL                                         */
/*==============================================================*/
create table TSG_ANSWERDTL
(
   ID                   int(11) not null auto_increment,
   ANSWERID             int(11) comment '对应表TSG_ANSWER ID',
   NAME                 varchar(100) comment '详细答案名称',
   ANSDTLIMAGEURL       varchar(50) comment '详细答案图片路径',
   DESCRIPTION          text comment '详细答案描述',
   ORDERSEQ             int(11) comment '详细答案序号',
   STATUS               tinyint(1) default 1 comment '是否启用 0:不启用,1:启用',
   primary key (ID)
);

/*==============================================================*/
/* Table: TSG_CATEGORY                                          */
/*==============================================================*/
create table TSG_CATEGORY
(
   ID                   int(11) not null,
   TYPE                 int(4) comment '1:问题问答类',
   CODE                 varchar(10) not null comment '类别Code',
   NAME                 varchar(100) comment '类别名称',
   CREATER              varchar(10) comment '创建人',
   CREATETIME           datetime comment '创建时间',
   STATUS               tinyint(1) default 1 comment '是否启用 0:不启用,1:启用',
   primary key (CODE)
);

/*==============================================================*/
/* Table: TSG_QUESTION                                          */
/*==============================================================*/
create table TSG_QUESTION
(
   ID                   int(11) not null auto_increment,
   CATEGORYCODE         varchar(10) not null comment '对应表TSG_CATEGORY 问题类别Code',
   NAME                 varchar(100) comment '问题名称',
   QUEIMAGEURL          varchar(50) comment '问题图片路径',
   DESCRIPTION          text comment '问题详细内容',
   ORDERSEQ             int(11) comment '问题序号',
   CREATER              varchar(10) comment '创建人',
   CREATETIME           datetime comment '创建时间',
   UPDATER              varchar(10) comment '更新人',
   UPDATETIME           datetime comment '更新时间',
   STATUS               tinyint(1) default 1 comment '是否启用 0:不启用,1:启用',
   primary key (ID)
);

alter table CARE_PLANACTIVITY add constraint R_35 foreign key (CP_NO)
      references CARE_PLANPROBLEM (CP_NO);

alter table CARE_PLANCHECKPOINT add constraint R_38 foreign key (CP_NO)
      references CARE_PLANPROBLEM (CP_NO);

alter table CARE_PLANDATA add constraint R_36 foreign key (CP_NO)
      references CARE_PLANPROBLEM (CP_NO);

alter table CARE_PLANEVAL add constraint R_34 foreign key (CP_NO)
      references CARE_PLANPROBLEM (CP_NO);

alter table CARE_PLANFOCUS add constraint R_37 foreign key (CP_NO)
      references CARE_PLANPROBLEM (CP_NO);

alter table CARE_PLANGOAL add constraint R_32 foreign key (CP_NO)
      references CARE_PLANPROBLEM (CP_NO);

alter table CARE_PLANREASON add constraint R_31 foreign key (CP_NO)
      references CARE_PLANPROBLEM (CP_NO);

alter table CARE_PLANTRAIN add constraint R_33 foreign key (CP_NO)
      references CARE_PLANPROBLEM (CP_NO);

alter table CHECKTEMPLATEITEM add constraint R0_41 foreign key (ORGID, CHECKTEMPLATECODE)
      references CHECKTEMPLATE (CHECKTEMPLATECODE, ORGID);

alter table DC_ASSESSVALUE add constraint R0_36 foreign key (SEQNO)
      references DC_REGCPL (SEQNO);

alter table DC_CAREPLANACTIVITY add constraint R0_34 foreign key (CPNO)
      references DC_CAREPLANPROBLEM (CPNO);

alter table DC_CAREPLANDATA add constraint R0_38 foreign key (CPNO)
      references DC_CAREPLANPROBLEM (CPNO);

alter table DC_CAREPLANEVAL add constraint R0_33 foreign key (CPNO)
      references DC_CAREPLANPROBLEM (CPNO);

alter table DC_CAREPLANGOAL add constraint R0_32 foreign key (CPNO)
      references DC_CAREPLANPROBLEM (CPNO);

alter table DC_CAREPLANREASON add constraint R0_39 foreign key (CPNO)
      references DC_CAREPLANPROBLEM (CPNO);

alter table DC_DAYLIFECAREDTL add constraint FK__DC_DAYLIFECA__ID__74CE504D foreign key (ID)
      references DC_DAYLIFECAREREC (ID);

alter table DC_EVALQUESTION add constraint R_024 foreign key (ID)
      references DC_NURSEINGPLANEVAL (ID);

alter table DC_EVALQUESTIONRESULT add constraint R_23 foreign key (RECORDID)
      references DC_EVALQUESTION (RECORDID);

alter table DC_IPDREG add constraint FK__DC_IPDREG__REGNO__75C27486 foreign key (REGNO)
      references DC_REGFILE (REGNO);

alter table DC_MULTITEAMCAREPLAN add constraint R_25 foreign key (SEQNO)
      references DC_MULTITEAMCAREPLANREC (SEQNO);

alter table DC_MULTITEAMCAREPLANEVAL add constraint R0_43 foreign key (SEQNO)
      references DC_MULTITEAMCAREPLANREC (SEQNO);

alter table DC_NSCPLACTIVITY add constraint R0_37 foreign key (SEQNO)
      references DC_REGCPL (SEQNO);

alter table DC_NSCPLGOAL add constraint R0_35 foreign key (SEQNO)
      references DC_REGCPL (SEQNO);

alter table DC_NURSEINGLIFECAREDTL add constraint FK__DC_NURSEINGL__ID__76B698BF foreign key (ID)
      references DC_NURSEINGLIFECAREREC (ID);

alter table DC_QUESTIONITEM add constraint R_9 foreign key (QUESTIONID)
      references DC_QUESTION (QUESTIONID);

alter table DC_QUESTIONVALUE add constraint R_8 foreign key (QUESTIONID)
      references DC_QUESTION (QUESTIONID);

alter table DC_REFERRALLISTS add constraint FK__DC_REFERR__FEENO__77AABCF8 foreign key (FEENO)
      references DC_IPDREG (FEENO);

alter table DC_REGCASECAREPLAN add constraint R_27 foreign key (SEQNO)
      references DC_REGCASECAREPLANREC (SEQNO);

alter table DC_REGCHECKRECORDDATA add constraint R0_40 foreign key (RECORDID)
      references DC_REGCHECKRECORD (RECORDID);

alter table DC_REGCPL add constraint R_018 foreign key (ID)
      references DC_NURSEINGPLANEVAL (ID);

alter table DC_REGEVALQUESTION add constraint R_012 foreign key (EVALRECID)
      references DC_REGQUESTIONEVALREC (EVALRECID);

alter table DC_REGQUESTIONDATA add constraint R_11 foreign key (SEQ)
      references DC_REGEVALQUESTION (SEQ);

alter table DC_TASKGOALSSTRATEGY add constraint R_03 foreign key (EVALPLANID)
      references DC_SWREGEVALPLAN (EVALPLANID);

alter table DC_TEAMACTIVITYDTL add constraint R_021 foreign key (SEQNO)
      references DC_TEAMACTIVITY (SEQNO);

alter table LTC_ACTIVEPERIOD add constraint R_10 foreign key (ORGID)
      references LTC_ORG (ORGID);

alter table LTC_ASSESSVALUE add constraint R_53 foreign key (SEQNO)
      references LTC_NSCPL (SEQNO);

alter table LTC_BEDSORECHGREC add constraint R_45 foreign key (SEQ)
      references LTC_BEDSOREREC (SEQ);

alter table LTC_CHECKITEM add constraint R_60 foreign key (TYPECODE)
      references LTC_CHECKTYPE (TYPECODE);

alter table LTC_CHECKITEM add constraint R_62 foreign key (GROUPCODE)
      references LTC_CHECKGROUP (GROUPCODE);

alter table LTC_CHECKRECDTL add constraint R_30 foreign key (RECORDID)
      references LTC_CHECKREC (RECORDID);

alter table LTC_CONSTRAINSBEVAL add constraint R_43 foreign key (SEQNO)
      references LTC_CONSTRAINTREC (SEQNO);

alter table LTC_COSTGROUPDTL add constraint FK__LTC_COSTG__GROUP__0C50D423 foreign key (GROUPID)
      references LTC_COSTGROUP (ID);

alter table LTC_FIXEDCOST add constraint FK__LTC_FIXED__COSTI__3F073C79 foreign key (COSTITEMID)
      references LTC_COSTITEM (ID);

alter table LTC_INFECTIONSYMPOTM add constraint R_41 foreign key (SEQNO)
      references LTC_INFECTIONIND (SEQNO);

alter table LTC_IPDREG add constraint R_15 foreign key (REGNO)
      references LTC_REGFILE (REGNO);

alter table LTC_IPDREGOUT add constraint R_18 foreign key (FEENO)
      references LTC_IPDREG (FEENO);

alter table LTC_LABEXAMREC add constraint R_42 foreign key (SEQNO)
      references LTC_INFECTIONIND (SEQNO);

alter table LTC_LEAVEHOSP add constraint R_16 foreign key (FEENO)
      references LTC_IPDREG (FEENO);

alter table LTC_NSCPLACTIVITY add constraint R_52 foreign key (SEQNO)
      references LTC_NSCPL (SEQNO);

alter table LTC_NSCPLGOAL add constraint R_51 foreign key (SEQNO)
      references LTC_NSCPL (SEQNO);

alter table LTC_ORG add constraint R_1 foreign key (GROUPID)
      references LTC_GROUP (GROUPID);

alter table LTC_ORGROOM add constraint R_57 foreign key (FLOORID)
      references LTC_ORGFLOOR (FLOORID);

alter table LTC_PAINBODYPART add constraint R_44 foreign key (SEQNO)
      references LTC_PAINEVALREC (SEQNO);

alter table LTC_PAYBILL add constraint FK__LTC_PAYBI__BILLI__18B6AB08 foreign key (BILLID)
      references LTC_BILL (ID);

alter table LTC_PROPOSALDISSCUSSREC add constraint R_28 foreign key (RECORDID)
      references LTC_PROPOSALDISSCUSS (RECORDID);

alter table LTC_REGDISEASEHIS add constraint R1 foreign key (REGNO)
      references LTC_REGFILE (REGNO);

alter table LTC_REGDISEASEHISDTL add constraint R2 foreign key (REGNO)
      references LTC_REGDISEASEHIS (REGNO);

alter table LTC_ROLEMODULES add constraint R_5 foreign key (ROLEID)
      references LTC_ROLES (ROLEID);

alter table LTC_ROLEMODULES add constraint R_6 foreign key (MODULEID)
      references LTC_MODULES (MODULEID);

alter table LTC_SCENARIO_ITEM add constraint S1 foreign key (CATEGORYID)
      references LTC_SCENARIO (CATEGORYID);

alter table LTC_SCENARIO_ITEM_OPTION add constraint S2 foreign key (SCENARIOITEMID)
      references LTC_SCENARIO_ITEM (ID);

alter table LTC_USERORG add constraint R_12 foreign key (USERID)
      references LTC_USERS (USERID);

alter table LTC_USERORG add constraint R_13 foreign key (ORGID)
      references LTC_ORG (ORGID);

alter table LTC_USERROLES add constraint R_3 foreign key (USERID)
      references LTC_USERS (USERID);

alter table LTC_USERROLES add constraint R_4 foreign key (ROLEID)
      references LTC_ROLES (ROLEID);

alter table LTC_VISITDEPT add constraint R_46 foreign key (HOSPNO)
      references LTC_VISITHOSPITAL (HOSPNO);

alter table LTC_VISITDOCTOR add constraint R_47 foreign key (HOSPNO)
      references LTC_VISITHOSPITAL (HOSPNO);

alter table LTC_VISITDOCTOR add constraint R_48 foreign key (DEPTNO)
      references LTC_VISITDEPT (DEPTNO);

alter table LTC_VISITPRESCRIPTION add constraint R_39 foreign key (SEQNO)
      references LTC_VISITDOCRECORDS (SEQNO);

alter table LTC_VISITPRESCRIPTION add constraint R_40 foreign key (MEDID)
      references LTC_MEDICINE (MEDID);

alter table TSG_ANSWER add constraint TSG2 foreign key (QUESTIONID)
      references TSG_QUESTION (ID);

alter table TSG_ANSWERDTL add constraint TSG3 foreign key (ANSWERID)
      references TSG_ANSWER (ID);

alter table TSG_QUESTION add constraint TSG1 foreign key (CATEGORYCODE)
      references TSG_CATEGORY (CODE);
