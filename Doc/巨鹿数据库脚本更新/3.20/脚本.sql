
--  �ʸ�������Ա״̬  
UPDATE NCIA_APPCERT
SET CREATETIME = "2016-09-12 00:00:00"
WHERE `NAME` = "������";

UPDATE NCIA_APPCERT
SET CREATETIME = "2017-01-17 00:00:00"
WHERE `NAME` IN ( "������", "�����","������");

UPDATE NCIA_APPCERT
SET CREATETIME = "2016-10-24 00:00:00"
WHERE `NAME` IN ( "��˫��","�����");

UPDATE NCIA_APPCERT
SET CREATETIME = "2016-08-08 00:00:00"
WHERE
	`NAME` IN (
		"��·��",
		"����ƽ",
		"�信��",
		"����ƽ",
		"�信��",
		"������",
		"��־��",
		"������",
		"������",
		"������",
		"���ն�",
		"���齭",
		"�Ÿ�ƽ",
		"������",
		"����",
		"�쾮ȱ",
		"�澰��",
		"������",
		"����",
		"������",
		"�촺��",
		"������",
		"�����",
		"������",
		"��ѩ��",
		"�δ���",
		"��ʤ��",
		"������",
		"���¶�",
		"�����",
		"������",
		"��ʤï",
		"���",
		"�Ŵ�ƽ",
		"�ϼӱ�",
		"��־��",
		"��˫ӡ",
		"Ѧ����",
		"�Ϸ���",
		"��ȫ��",
		"������",
		"���",
		"������",
		"������",
		"����ȫ",
		"Ҧ����",
		"ۭ����",
		"�����",
		"��۸�",
		"��ѩ��",
		"������",
		"������",
		"����ɽ",
		"������",
		"���ɿ�",
		"������",
		"���쿭",
		"�︣��",
		"������",
		"���",
		"������",
		"������",
		"������",
		"�����",
		"Ԭ����"
	);
	
	--ҩƷ�������� 
	
	UPDATE  LTC_NSDRUG SET CREATETIME = "2017-03-05" WHERE  DRUGID <= 801;
	
	-- �����ƻ�
	
	UPDATE LTC_NSCPL SET  CREATEDATE = "2017-03-08 00:00:00"  WHERE  SEQNO IN (1,2);
	
	-- �½��ײ�
	
UPDATE LTC_CHARGEGROUP
SET CREATETIME = "2017-03-18 00:00:00"
WHERE
	CHARGEGROUPID IN (
"000000003",
"000000004",
"000000005",
"000000006",
"000000007",
"000000008",
"000000009",
"000000010",
"000000011",
"000000012",
"000000013",
"000000014",
"000000015",
"000000016",
"000000017",
"000000018",
"000000019",
"000000020"
);

--  �ײ�����ʱ��


update LTC_CHARGEGROUP_CHARGEITEM set
CREATETIME = "2017-03-18 00:00:00" WHERE 
CHARGEGROUPID IN  ("000000002" ,"000000003");

 
	
	
	
	
	
	

