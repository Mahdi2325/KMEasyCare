///创建人:BobDu
///创建日期:2017-02-03
///说明:自定义枚举

(function () {
    var app = angular.module("extentEnum", []);

    app.constant('EnumConstants', {
        NCIPStatusEnum: {
            /// <summary>
            /// 已创建
            /// </summary>
            Created: 0,
            /// <summary>
            /// 已撤回
            /// </summary>
            Withdrawn: 1,
            /// <summary>
            /// 待审核
            /// </summary>
            Pending: 10,
            /// <summary>
            /// 审核通过
            /// </summary>
            passed: 20,
            /// <summary>
            /// 已拨款
            /// </summary>
            Appropriated: 30,
            /// <summary>
            /// 审核不通过
            /// </summary>
            notPassed: 90
        },
        MenelipsisEnum: {
            /// <summary>
            /// 不适用
            /// </summary>
            Inapplicability: 1,
            /// <summary>
            /// 否
            /// </summary>
            No: 2,
            /// <summary>
            /// 是
            /// </summary>
            Yes: 3

        },
        AcousticDuctPro: {
            /// <summary>
            /// 通畅
            /// </summary>
            TongChang: 1,
            /// <summary>
            /// 分泌物
            /// </summary>
            FenMiWu: 2
        },
        NasalseptumPro: {
            /// <summary>
            /// 居中
            /// </summary>
            JuZhong: 1,
            /// <summary>
            /// 偏曲
            /// </summary>
            PianQu: 2
        },
        Nasalmucosa: {
            /// <summary>
            /// 正常
            /// </summary>
            ZhengChang: 1,
            /// <summary>
            /// 充血
            /// </summary>
            ChongXue: 2
        },
        Nasalshoulder: {
            /// <summary>
            /// 正常
            /// </summary>
            ZhengChang: 1,
            /// <summary>
            /// 肥大
            /// </summary>
            FeiDa: 2
        },
        LIP: {
            /// <summary>
            /// 红润
            /// </summary>
            HongRun: 1,
            /// <summary>
            /// 紫绀
            /// </summary>
            ZiGan: 2,
            /// <summary>
            /// 苍白
            /// </summary>
            CangBai: 3
        },
        Tongue: {
            /// <summary>
            /// 伸舌居中
            /// </summary>
            SSJZ: 1,
            /// <summary>
            /// 伸舌左偏
            /// </summary>
            SSZP: 2,
            /// <summary>
            /// 伸舌右偏
            /// </summary>
            SSYP: 3
        },
        OralmucosaPro: {
            /// <summary>
            /// 红润
            /// </summary>
            HongRun: 1,
            /// <summary>
            /// 溃疡
            /// </summary>
            KuiYang: 2
        },
        Tooth: {
            /// <summary>
            /// 无充血
            /// </summary>
            WuChongXue: 1,
            /// <summary>
            /// 充血
            /// </summary>
            ChongXue: 2
        },
        Tonsil: {
            /// <summary>
            /// 无肿大
            /// </summary>
            WuZhongDa: 1,
            /// <summary>
            /// 肿大
            /// </summary>
            ZhongDa: 2
        },
        TonsilHN: {
            /// <summary>
            /// 无化脓
            /// </summary>
            WuHuaNong: 1,
            /// <summary>
            /// 化脓
            /// </summary>
            HuaNong: 2
        },
        Organ: {
            /// <summary>
            /// 居中
            /// </summary>
            JZ: 1,
            /// <summary>
            /// 左偏
            /// </summary>
            ZP: 2,
            /// <summary>
            /// 右偏
            /// </summary>
            YP: 3
        },
        ThyroidGland: {
            /// <summary>
            /// 无肿大
            /// </summary>
            WuZhogDa: 1,
            /// <summary>
            /// 肿大
            /// </summary>
            ZhogDa: 2
        },
        LungPro: {
            /// <summary>
            /// 呼吸音清晰
            /// </summary>
            HXYQX: 1,
            /// <summary>
            /// 呼吸音粗
            /// </summary>
            HXYC: 2,
            /// <summary>
            /// 呼吸音低
            /// </summary>
            HXYC: 3
        },
        HeartSound: {
            /// <summary>
            /// 有力
            /// </summary>
            YL: 1,
            /// <summary>
            /// 低钝
            /// </summary>
            DD: 2,
            /// <summary>
            /// 强弱不等
            /// </summary>
            QRBD: 3
        },
        NipplePro: {
            /// <summary>
            /// 正常
            /// </summary>
            ZC: 1,
            /// <summary>
            /// 内陷
            /// </summary>
            NX: 2
        },
        NippleFMWPro: {
            /// <summary>
            /// 无分泌物
            /// </summary>
            WFMW: 1,
            /// <summary>
            /// 有分泌物
            /// </summary>
            YFMW: 2
        },
        AbdomenPro: {
            /// <summary>
            /// 服软无压痛
            /// </summary>
            FRWYT: 1,
            /// <summary>
            /// 压痛
            /// </summary>
            YT: 2
        },
        BowelSounDPro: {
            /// <summary>
            /// 正常
            /// </summary>
            ZC: 1,
            /// <summary>
            /// 亢进
            /// </summary>
            KJ: 2,
            /// <summary>
            /// 减弱
            /// </summary>
            JR: 3
        },
        SpinelimbsPro: {
            /// <summary>
            /// 活动自如
            /// </summary>
            HDZR: 1,
            /// <summary>
            /// 活动受限
            /// </summary>
            HDSX: 2
        },
        SpinelimbsJXPro: {
            /// <summary>
            /// 无畸形
            /// </summary>
            WJX: 1,
            /// <summary>
            /// 畸形
            /// </summary>
            JX: 2
        },
        AnusgenitaliaPro: {
            /// <summary>
            /// 外观正常
            /// </summary>
            WGZC: 1,
            /// <summary>
            /// 内痔
            /// </summary>
            NZ: 2,
            /// <summary>
            /// 外痔
            /// </summary>
            WZ: 3,
            /// <summary>
            /// 其它
            /// </summary>
            QT: 4
        },
        Nervous: {
            /// <summary>
            /// 肌力正常
            /// </summary>
            JLZC: 1,
            /// <summary>
            /// 肌力减弱
            /// </summary>
            JLJR: 2,
            /// <summary>
            /// 其它
            /// </summary>
            QT: 3
        },
        SpecialNeedsTooth: {
            /// <summary>
            /// 正常
            /// </summary>
            ZC: 1,
            /// <summary>
            /// 异常
            /// </summary>
            YC: 2,
            /// <summary>
            /// 假牙
            /// </summary>
            JY: 3
        },
        SpecialNeedHear: {
            /// <summary>
            /// 正常
            /// </summary>
            ZC: 1,
            /// <summary>
            /// 异常
            /// </summary>
            YC: 2
        },
        SpecialNeedHearpos: {
            /// <summary>
            /// 左
            /// </summary>
            Z: 1,
            /// <summary>
            /// 右
            /// </summary>
            Y: 2
        },
        SpecialNeedVision: {
            /// <summary>
            /// 正常
            /// </summary>
            ZC: 1,
            /// <summary>
            /// 异常
            /// </summary>
            YC: 2
        },
        SpecialNeedVisionPos: {
            /// <summary>
            /// 左
            /// </summary>
            Z: 1,
            /// <summary>
            /// 右
            /// </summary>
            Y: 2
        }, Missionobj: {
            /// <summary>
            /// 家属
            /// </summary>
            JS: 1,
            /// <summary>
            /// 客户
            /// </summary>
            KH: 2
        }, LearnWish: {
            /// <summary>
            /// 高
            /// </summary>
            G: 1,
            /// <summary>
            /// 低
            /// </summary>
            D: 2
        }, earnestStatus: {
            UnPain: 1,//未交订金
            Paid: 2,//已交订金
            Returned: 3//已退订金
        },
        //事业类型
        careerType: {
            Nurse: '001',
            Carer: '002',
            Doctor: '006',
            Physiotherapist: '005'
        }

    });
    app.run(['$rootScope', 'EnumConstants', function ($rootScope, EnumConstants) {
        $rootScope.enum = {};
        $rootScope.enum.earnestStatus = EnumConstants.earnestStatus;
        $rootScope.enum.careerType = EnumConstants.careerType;
        $rootScope.NCIPStatusEnum = EnumConstants.NCIPStatusEnum;
        $rootScope.MenelipsisEnum = EnumConstants.MenelipsisEnum;
        $rootScope.AcousticDuctPro = EnumConstants.AcousticDuctPro;
        $rootScope.NasalseptumPro = EnumConstants.NasalseptumPro;
        $rootScope.Nasalmucosa = EnumConstants.Nasalmucosa;
        $rootScope.Nasalshoulder = EnumConstants.Nasalshoulder;
        $rootScope.LIP = EnumConstants.LIP;
        $rootScope.Tongue = EnumConstants.Tongue;
        $rootScope.OralmucosaPro = EnumConstants.OralmucosaPro;
        $rootScope.Tooth = EnumConstants.Tooth;
        $rootScope.Tonsil = EnumConstants.Tonsil;
        $rootScope.TonsilHN = EnumConstants.TonsilHN;
        $rootScope.Organ = EnumConstants.Organ;
        $rootScope.ThyroidGland = EnumConstants.ThyroidGland;
        $rootScope.LungPro = EnumConstants.LungPro;
        $rootScope.HeartSound = EnumConstants.HeartSound;
        $rootScope.BreastPro = EnumConstants.BreastPro;
        $rootScope.NipplePro = EnumConstants.NipplePro;
        $rootScope.AbdomenPro = EnumConstants.AbdomenPro;
        $rootScope.BowelSounDPro = EnumConstants.BowelSounDPro;
        $rootScope.SpinelimbsPro = EnumConstants.SpinelimbsPro;
        $rootScope.AnusgenitaliaPro = EnumConstants.AnusgenitaliaPro;
        $rootScope.Nervous = EnumConstants.Nervous;
        $rootScope.SpecialNeedsTooth = EnumConstants.SpecialNeedsTooth;
        $rootScope.SpecialNeedHear = EnumConstants.SpecialNeedHear;
        $rootScope.SpecialNeedHearpos = EnumConstants.SpecialNeedHearpos;
        $rootScope.SpecialNeedVision = EnumConstants.SpecialNeedVision;
        $rootScope.SpecialNeedVisionPos = EnumConstants.SpecialNeedVisionPos;
        $rootScope.Missionobj = EnumConstants.Missionobj;
        $rootScope.LearnWish = EnumConstants.LearnWish;
    }]);
})();