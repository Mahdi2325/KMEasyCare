//创建人：吴晓波
//创建日期：2017-02-12
//首次入住记录
angular.module("sltcApp").controller('fstRegRecCtrl', ['$scope', '$state', '$stateParams', 'FstRegRecRes', 'residentV2Res', 'utility', 'cloudAdminUi', 'EnumConstants',
    function ($scope, $state, $stateParams, FstRegRecRes, residentV2Res, utility, cloudAdminUi, EnumConstants) {
        $scope.FstRegRec = {};
        $scope.AddressDtls = "";
        $scope.currentItem = {};
        $scope.FeeNo = $state.params.id;
        $scope.curResident = { RegNo: 0, FeeNo: 0 };

        //if ($scope.FeeNo != undefined && $scope.FeeNo != null && $scope.FeeNo != "") {
        //    $scope.GetInitFstRegRec($scope.FeeNo);
        //};

        $scope.txtResidentIDChange = function (resident) {
            $scope.curResident = resident;
            $scope.FeeNo = $scope.curResident.FeeNo;
            $scope.GetInitFstRegRec($scope.curResident.FeeNo);
        };

        $scope.GetInitFstRegRec = function (feeNo) {
            residentV2Res.get({ id: feeNo }, function (response) {
                if (response.Data[0] != null) {
                    $scope.currentItem = response.Data[0];
                    $scope.currentItem.City2 = $scope.currentItem.City2 ? $scope.currentItem.City2 : "";
                    $scope.currentItem.Address2 = $scope.currentItem.Address2 ? $scope.currentItem.Address2 : "";
                    $scope.currentItem.Address2Dtl = $scope.currentItem.Address2Dtl ? $scope.currentItem.Address2Dtl : "";

                    $scope.AddressDtls = $scope.currentItem.City2 + $scope.currentItem.Address2 + $scope.currentItem.Address2Dtl;
                };
                $scope.GetFstRegRec();
            });
        };

        cloudAdminUi.handleGoToTop();

        //加载住民信息首次登陆信息
        $scope.GetFstRegRec = function () {
            FstRegRecRes.get({ regNo: $scope.curResident.RegNo == "" ? -1 : $scope.curResident.RegNo }, function (data) {
                $scope.FstRegRec = data.Data;
                if ($scope.FstRegRec) {
                    //既往史否
                    if ($scope.FstRegRec.PrevhxFlag == false) {
                        $scope.FstRegRec.fHaveOperation = true;
                        $scope.FstRegRec.tHaveOperation = false;
                    } else if ($scope.FstRegRec.PrevhxFlag == true) {
                        $scope.FstRegRec.fHaveOperation = false;
                        $scope.FstRegRec.tHaveOperation = true;
                    };

                    if ($scope.FstRegRec.Weight != undefined && $scope.FstRegRec.Weight != null && $scope.FstRegRec.Weight != "" && $scope.FstRegRec.Height != undefined && $scope.FstRegRec.Height != null && $scope.FstRegRec.Height != "") {
                        $scope.FstRegRec.BMI = utility.BMI($scope.FstRegRec.Weight, $scope.FstRegRec.Height);
                        $scope.FstRegRec.BMIFruit = utility.BMIResult($scope.FstRegRec.BMI);
                    };

                    //婚育史 结婚否
                    if ($scope.FstRegRec.MarryFlag == false) {
                        $scope.FstRegRec.fMarryFlag = true;
                        $scope.FstRegRec.tMarryFlag = false;
                    } else if ($scope.FstRegRec.MarryFlag == true) {
                        $scope.FstRegRec.fMarryFlag = false;
                        $scope.FstRegRec.tMarryFlag = true;
                    };

                    //婚育史 有子女否
                    if ($scope.FstRegRec.OffspringFlag == false) {
                        $scope.FstRegRec.fOffspringFlag = true;
                        $scope.FstRegRec.tOffspringFlag = false;
                    } else if ($scope.FstRegRec.OffspringFlag == true) {
                        $scope.FstRegRec.fOffspringFlag = false;
                        $scope.FstRegRec.tOffspringFlag = true;
                    };

                    //婚育史 停经
                    if ($scope.FstRegRec.Menelipsis == EnumConstants.MenelipsisEnum['Inapplicability']) {
                        $scope.FstRegRec.Menelipsis1 = true;
                        $scope.FstRegRec.Menelipsis2 = false;
                        $scope.FstRegRec.Menelipsis3 = false;
                    } else if ($scope.FstRegRec.Menelipsis == EnumConstants.MenelipsisEnum['No']) {
                        $scope.FstRegRec.Menelipsis1 = false;
                        $scope.FstRegRec.Menelipsis2 = true;
                        $scope.FstRegRec.Menelipsis3 = false;
                    } else if ($scope.FstRegRec.Menelipsis == EnumConstants.MenelipsisEnum['Yes']) {
                        $scope.FstRegRec.Menelipsis1 = false;
                        $scope.FstRegRec.Menelipsis2 = false;
                        $scope.FstRegRec.Menelipsis3 = true;
                    };

                    //用药史否
                    if ($scope.FstRegRec.MedhFlag == false) {
                        $scope.FstRegRec.fMedhFlag = true;
                        $scope.FstRegRec.tMedhFlag = false;
                    } else if ($scope.FstRegRec.MedhFlag == true) {
                        $scope.FstRegRec.fMedhFlag = false;
                        $scope.FstRegRec.tMedhFlag = true;
                    };

                    //药物/食物过敏否
                    if ($scope.FstRegRec.AllergichFlag == false) {
                        $scope.FstRegRec.fAllergichFlag = true;
                        $scope.FstRegRec.tAllergichFlag = false;
                    } else if ($scope.FstRegRec.AllergichFlag == true) {
                        $scope.FstRegRec.fAllergichFlag = false;
                        $scope.FstRegRec.tAllergichFlag = true;
                    };

                    //手术史否
                    if ($scope.FstRegRec.OperationhxFlag == false) {
                        $scope.FstRegRec.fOperationhxFlag = true;
                        $scope.FstRegRec.tOperationhxFlag = false;
                    } else if ($scope.FstRegRec.OperationhxFlag == true) {
                        $scope.FstRegRec.fOperationhxFlag = false;
                        $scope.FstRegRec.tOperationhxFlag = true;
                    };

                    //个人史否
                    if ($scope.FstRegRec.PersonalhxFlag == false) {
                        $scope.FstRegRec.fPersonalhxFlag = true;
                        $scope.FstRegRec.tPersonalhxFlag = false;
                    } else if ($scope.FstRegRec.PersonalhxFlag == true) {
                        $scope.FstRegRec.fPersonalhxFlag = false;
                        $scope.FstRegRec.tPersonalhxFlag = true;
                    };

                    //家族史否
                    if ($scope.FstRegRec.FhxFlag == false) {
                        $scope.FstRegRec.fFhxFlag = true;
                        $scope.FstRegRec.tFhxFlag = false;
                    } else if ($scope.FstRegRec.FhxFlag == true) {
                        $scope.FstRegRec.fFhxFlag = false;
                        $scope.FstRegRec.tFhxFlag = true;
                    };


                    //皮肤黏膜红润否
                    if ($scope.FstRegRec.SkinmucosaHRunFlag == undefined || $scope.FstRegRec.SkinmucosaHRunFlag == null) {
                        $scope.FstRegRec.SkinmucosaHRun = false;
                    } else if ($scope.FstRegRec.SkinmucosaHRunFlag == true) {
                        $scope.FstRegRec.SkinmucosaHRun = true;
                    } else {
                        $scope.FstRegRec.SkinmucosaHRun = false;
                    };

                    //皮肤黏膜黄染否
                    if ($scope.FstRegRec.SkinmucosaHRanFlag == undefined || $scope.FstRegRec.SkinmucosaHRanFlag == null) {
                        $scope.FstRegRec.SkinmucosaHRan = false;
                    } else if ($scope.FstRegRec.SkinmucosaHRanFlag == true) {
                        $scope.FstRegRec.SkinmucosaHRan = true;
                    } else {
                        $scope.FstRegRec.SkinmucosaHRan = false;
                    };

                    //皮肤黏膜苍白否
                    if ($scope.FstRegRec.SkinmucosaCBFlag == undefined || $scope.FstRegRec.SkinmucosaCBFlag == null) {
                        $scope.FstRegRec.SkinmucosaCB = false;
                    } else if ($scope.FstRegRec.SkinmucosaCBFlag == true) {
                        $scope.FstRegRec.SkinmucosaCB = true;
                    } else {
                        $scope.FstRegRec.SkinmucosaCB = false;
                    };

                    //皮肤黏膜千瘪否
                    if ($scope.FstRegRec.SkinmucosaQBFlag == undefined || $scope.FstRegRec.SkinmucosaQBFlag == null) {
                        $scope.FstRegRec.SkinmucosaQB = false;
                    } else if ($scope.FstRegRec.SkinmucosaQBFlag == true) {
                        $scope.FstRegRec.SkinmucosaQB = true;
                    } else {
                        $scope.FstRegRec.SkinmucosaQB = false;
                    };

                    //皮肤黏膜皮疹否
                    if ($scope.FstRegRec.SkinmucosaPZFlag == undefined || $scope.FstRegRec.SkinmucosaPZFlag == null) {
                        $scope.FstRegRec.SkinmucosaPZ = false;
                    } else if ($scope.FstRegRec.SkinmucosaPZFlag == true) {
                        $scope.FstRegRec.SkinmucosaPZ = true;
                    } else {
                        $scope.FstRegRec.SkinmucosaPZ = false;
                    };


                    //其他皮肤黏膜信息
                    if ($scope.FstRegRec.OtherSkinmucosaFlag == undefined || $scope.FstRegRec.OtherSkinmucosaFlag == null) {
                        $scope.FstRegRec.OtherSkinmucosaF = false;
                    } else if ($scope.FstRegRec.OtherSkinmucosaFlag == true) {
                        $scope.FstRegRec.OtherSkinmucosaF = true;
                    } else {
                        $scope.FstRegRec.OtherSkinmucosaF = false;
                    };


                    //淋巴结肿大否
                    if ($scope.FstRegRec.LymphGlandFlag == false) {
                        $scope.FstRegRec.fLymphGlandFlag = true;
                        $scope.FstRegRec.tLymphGlandFlag = false;
                    } else if ($scope.FstRegRec.LymphGlandFlag == true) {
                        $scope.FstRegRec.fLymphGlandFlag = false;
                        $scope.FstRegRec.tLymphGlandFlag = true;
                    };

                    //头颅五官正常否
                    if ($scope.FstRegRec.HeadFlag == false) {
                        $scope.FstRegRec.fHeadFlag = true;
                        $scope.FstRegRec.tHeadFlag = false;
                    } else if ($scope.FstRegRec.HeadFlag == true) {
                        $scope.FstRegRec.fHeadFlag = false;
                        $scope.FstRegRec.tHeadFlag = true;
                    };

                    //眼：巩膜
                    if ($scope.FstRegRec.ScleraFlag == false) {
                        $scope.FstRegRec.fScleraFlag = true;
                        $scope.FstRegRec.tScleraFlag = false;
                    } else if ($scope.FstRegRec.ScleraFlag == true) {
                        $scope.FstRegRec.fScleraFlag = false;
                        $scope.FstRegRec.tScleraFlag = true;
                    };

                    //眼：空桶
                    if ($scope.FstRegRec.EBFlag == false) {
                        $scope.FstRegRec.fEBFlag = true;
                        $scope.FstRegRec.tEBFlag = false;
                    } else if ($scope.FstRegRec.EBFlag == true) {
                        $scope.FstRegRec.fEBFlag = false;
                        $scope.FstRegRec.tEBFlag = true;
                    };

                    //眼：结膜
                    if ($scope.FstRegRec.ConjunctivaFlag == false) {
                        $scope.FstRegRec.fConjunctivaFlag = true;
                        $scope.FstRegRec.tConjunctivaFlag = false;
                    } else if ($scope.FstRegRec.ConjunctivaFlag == true) {
                        $scope.FstRegRec.fConjunctivaFlag = false;
                        $scope.FstRegRec.tConjunctivaFlag = true;
                    };

                    //耳：耳廊形态
                    if ($scope.FstRegRec.EARFlag == false) {
                        $scope.FstRegRec.fEARFlag = true;
                        $scope.FstRegRec.tEARFlag = false;
                    } else if ($scope.FstRegRec.EARFlag == true) {
                        $scope.FstRegRec.fEARFlag = false;
                        $scope.FstRegRec.tEARFlag = true;
                    };


                    //外耳道
                    if ($scope.FstRegRec.AcousticDuctPro == EnumConstants.AcousticDuctPro['TongChang']) {
                        $scope.FstRegRec.AcousticDuctPro1 = true;
                        $scope.FstRegRec.AcousticDuctPro2 = false;
                    } else if ($scope.FstRegRec.AcousticDuctPro == EnumConstants.AcousticDuctPro['FenMiWu']) {
                        $scope.FstRegRec.AcousticDuctPro1 = false;
                        $scope.FstRegRec.AcousticDuctPro2 = true;
                    };

                    //鼻中隔
                    if ($scope.FstRegRec.NasalseptumPro == EnumConstants.NasalseptumPro['JuZhong']) {
                        $scope.FstRegRec.NasalseptumPro1 = true;
                        $scope.FstRegRec.NasalseptumPro2 = false;
                    } else if ($scope.FstRegRec.NasalseptumPro == EnumConstants.NasalseptumPro['PianQu']) {
                        $scope.FstRegRec.NasalseptumPro1 = false;
                        $scope.FstRegRec.NasalseptumPro2 = true;
                    };

                    //鼻腔黏膜
                    if ($scope.FstRegRec.Nasalmucosa == EnumConstants.Nasalmucosa['ZhengChang']) {
                        $scope.FstRegRec.Nasalmucosa1 = true;
                        $scope.FstRegRec.Nasalmucosa2 = false;
                    } else if ($scope.FstRegRec.Nasalmucosa == EnumConstants.Nasalmucosa['ChongXue']) {
                        $scope.FstRegRec.Nasalmucosa1 = false;
                        $scope.FstRegRec.Nasalmucosa2 = true;
                    };

                    //鼻胛
                    if ($scope.FstRegRec.Nasalshoulder == EnumConstants.Nasalshoulder['ZhengChang']) {
                        $scope.FstRegRec.Nasalshoulder1 = true;
                        $scope.FstRegRec.Nasalshoulder2 = false;
                    } else if ($scope.FstRegRec.Nasalshoulder == EnumConstants.Nasalshoulder['FeiDa']) {
                        $scope.FstRegRec.Nasalshoulder1 = false;
                        $scope.FstRegRec.Nasalshoulder2 = true;
                    };

                    //口：唇
                    if ($scope.FstRegRec.LIP == EnumConstants.LIP['HongRun']) {
                        $scope.FstRegRec.LIP1 = true;
                        $scope.FstRegRec.LIP2 = false;
                        $scope.FstRegRec.LIP3 = false;
                    } else if ($scope.FstRegRec.LIP == EnumConstants.LIP['ZiGan']) {
                        $scope.FstRegRec.LIP1 = false;
                        $scope.FstRegRec.LIP2 = true;
                        $scope.FstRegRec.LIP3 = false;
                    } else if ($scope.FstRegRec.LIP == EnumConstants.LIP['CangBai']) {
                        $scope.FstRegRec.LIP1 = false;
                        $scope.FstRegRec.LIP2 = false;
                        $scope.FstRegRec.LIP3 = true;
                    };

                    //舌头
                    if ($scope.FstRegRec.Tongue == EnumConstants.Tongue['SSJZ']) {
                        $scope.FstRegRec.Tongue1 = true;
                        $scope.FstRegRec.Tongue2 = false;
                        $scope.FstRegRec.Tongue3 = false;
                    } else if ($scope.FstRegRec.Tongue == EnumConstants.Tongue['SSZP']) {
                        $scope.FstRegRec.Tongue1 = false;
                        $scope.FstRegRec.Tongue2 = true;
                        $scope.FstRegRec.Tongue3 = false;
                    } else if ($scope.FstRegRec.Tongue == EnumConstants.Tongue['SSYP']) {
                        $scope.FstRegRec.Tongue1 = false;
                        $scope.FstRegRec.Tongue2 = false;
                        $scope.FstRegRec.Tongue3 = true;
                    };

                    //口腔黏膜
                    if ($scope.FstRegRec.OralmucosaPro == EnumConstants.OralmucosaPro['HongRun']) {
                        $scope.FstRegRec.OralmucosaPro1 = true;
                        $scope.FstRegRec.OralmucosaPro2 = false;
                    } else if ($scope.FstRegRec.OralmucosaPro == EnumConstants.OralmucosaPro['KuiYang']) {
                        $scope.FstRegRec.OralmucosaPro1 = false;
                        $scope.FstRegRec.OralmucosaPro2 = true;
                    };

                    //咽部：牙齿
                    if ($scope.FstRegRec.Tooth == EnumConstants.Tooth['WuChongXue']) {
                        $scope.FstRegRec.Tooth1 = true;
                        $scope.FstRegRec.Tooth2 = false;
                    } else if ($scope.FstRegRec.Tooth == EnumConstants.Tooth['ChongXue']) {
                        $scope.FstRegRec.Tooth1 = false;
                        $scope.FstRegRec.Tooth2 = true;
                    };

                    //咽部：扁桃体 肿大
                    if ($scope.FstRegRec.Tonsil == EnumConstants.Tonsil['WuZhongDa']) {
                        $scope.FstRegRec.Tonsil1 = true;
                        $scope.FstRegRec.Tonsil2 = false;
                    } else if ($scope.FstRegRec.Tonsil == EnumConstants.Tonsil['ZhongDa']) {
                        $scope.FstRegRec.Tonsil1 = false;
                        $scope.FstRegRec.Tonsil2 = true;
                    };

                    //咽部：扁桃体 化脓
                    if ($scope.FstRegRec.TonsilHN == EnumConstants.TonsilHN['WuHuaNong']) {
                        $scope.FstRegRec.TonsilHN1 = true;
                        $scope.FstRegRec.TonsilHN2 = false;
                    } else if ($scope.FstRegRec.TonsilHN == EnumConstants.TonsilHN['HuaNong']) {
                        $scope.FstRegRec.TonsilHN1 = false;
                        $scope.FstRegRec.TonsilHN2 = true;
                    };

                    //颈部：气管
                    if ($scope.FstRegRec.Organ == EnumConstants.Organ['JZ']) {
                        $scope.FstRegRec.Organ1 = true;
                        $scope.FstRegRec.Organ2 = false;
                        $scope.FstRegRec.Organ3 = false;
                    } else if ($scope.FstRegRec.Organ == EnumConstants.Organ['ZP']) {
                        $scope.FstRegRec.Organ1 = false;
                        $scope.FstRegRec.Organ2 = true;
                        $scope.FstRegRec.Organ3 = false;
                    } else if ($scope.FstRegRec.Organ == EnumConstants.Organ['YP']) {
                        $scope.FstRegRec.Organ1 = false;
                        $scope.FstRegRec.Organ2 = false;
                        $scope.FstRegRec.Organ3 = true;
                    };


                    //甲状腺
                    if ($scope.FstRegRec.ThyroidGland == EnumConstants.ThyroidGland['WuZhogDa']) {
                        $scope.FstRegRec.ThyroidGland1 = true;
                        $scope.FstRegRec.ThyroidGland2 = false;
                    } else if ($scope.FstRegRec.ThyroidGland == EnumConstants.ThyroidGland['ZhogDa']) {
                        $scope.FstRegRec.ThyroidGland1 = false;
                        $scope.FstRegRec.ThyroidGland2 = true;
                    };

                    //胸廓正常否
                    if ($scope.FstRegRec.ThoraxFlag == false) {
                        $scope.FstRegRec.fThoraxFlag = true;
                        $scope.FstRegRec.tThoraxFlag = false;
                    } else if ($scope.FstRegRec.ThoraxFlag == true) {
                        $scope.FstRegRec.fThoraxFlag = false;
                        $scope.FstRegRec.tThoraxFlag = true;
                    };

                    //肺
                    if ($scope.FstRegRec.LungPro == EnumConstants.LungPro['HXYQX']) {
                        $scope.FstRegRec.LungPro1 = true;
                        $scope.FstRegRec.LungPro2 = false;
                        $scope.FstRegRec.LungPro3 = false;
                    } else if ($scope.FstRegRec.LungPro == EnumConstants.LungPro['HXYC']) {
                        $scope.FstRegRec.LungPro1 = false;
                        $scope.FstRegRec.LungPro2 = true;
                        $scope.FstRegRec.LungPro3 = false;
                    } else if ($scope.FstRegRec.LungPro == EnumConstants.LungPro['HYYD']) {
                        $scope.FstRegRec.LungPro1 = false;
                        $scope.FstRegRec.LungPro2 = false;
                        $scope.FstRegRec.LungPro3 = true;
                    };


                    //肺：干罗音 否
                    if ($scope.FstRegRec.LungGLYFlag == undefined || $scope.FstRegRec.LungGLYFlag == null) {
                        $scope.FstRegRec.LungGLY = false;
                    } else if ($scope.FstRegRec.LungGLYFlag == true) {
                        $scope.FstRegRec.LungGLY = true;
                    } else {
                        $scope.FstRegRec.LungGLY = false;
                    };


                    //肺：湿罗音 否
                    if ($scope.FstRegRec.LungSLYFlag == undefined || $scope.FstRegRec.LungSLYFlag == null) {
                        $scope.FstRegRec.LungSLY = false;
                    } else if ($scope.FstRegRec.LungSLYFlag == true) {
                        $scope.FstRegRec.LungSLY = true;
                    } else {
                        $scope.FstRegRec.LungSLY = false;
                    };

                    //心律齐否
                    if ($scope.FstRegRec.HeartrhythmFlag == false) {
                        $scope.FstRegRec.fHeartrhythmFlag = true;
                        $scope.FstRegRec.tHeartrhythmFlag = false;
                    } else if ($scope.FstRegRec.HeartrhythmFlag == true) {
                        $scope.FstRegRec.fHeartrhythmFlag = false;
                        $scope.FstRegRec.tHeartrhythmFlag = true;
                    };

                    //心音
                    if ($scope.FstRegRec.HeartSound == EnumConstants.HeartSound['YL']) {
                        $scope.FstRegRec.HeartSound1 = true;
                        $scope.FstRegRec.HeartSound2 = false;
                        $scope.FstRegRec.HeartSound3 = false;
                    } else if ($scope.FstRegRec.HeartSound == EnumConstants.HeartSound['DD']) {
                        $scope.FstRegRec.HeartSound1 = false;
                        $scope.FstRegRec.HeartSound2 = true;
                        $scope.FstRegRec.HeartSound3 = false;
                    } else if ($scope.FstRegRec.HeartSound == EnumConstants.HeartSound['QRBD']) {
                        $scope.FstRegRec.HeartSound1 = false;
                        $scope.FstRegRec.HeartSound2 = false;
                        $scope.FstRegRec.HeartSound3 = true;
                    };


                    //乳房对称否
                    if ($scope.FstRegRec.BreastDCFlag == undefined || $scope.FstRegRec.BreastDCFlag == null) {
                        $scope.FstRegRec.BreastDC = false;
                    } else if ($scope.FstRegRec.BreastDCFlag == true) {
                        $scope.FstRegRec.BreastDC = true;
                    } else {
                        $scope.FstRegRec.BreastDC = false;
                    };

                    //乳房包块否
                    if ($scope.FstRegRec.BreastBKFlag == undefined || $scope.FstRegRec.BreastBKFlag == null) {
                        $scope.FstRegRec.BreastBK = false;
                    } else if ($scope.FstRegRec.BreastBKFlag == true) {
                        $scope.FstRegRec.BreastBK = true;
                    } else {
                        $scope.FstRegRec.BreastBK = false;
                    };


                    //乳头
                    if ($scope.FstRegRec.NipplePro == EnumConstants.NipplePro['ZC']) {
                        $scope.FstRegRec.NipplePro1 = true;
                        $scope.FstRegRec.NipplePro2 = false;
                    } else if ($scope.FstRegRec.NipplePro == EnumConstants.NipplePro['NX']) {
                        $scope.FstRegRec.NipplePro1 = false;
                        $scope.FstRegRec.NipplePro2 = true;
                    };


                    //乳头
                    if ($scope.FstRegRec.NippleFMWPro == EnumConstants.NippleFMWPro['WFMW']) {
                        $scope.FstRegRec.NippleFMWPro1 = true;
                        $scope.FstRegRec.NippleFMWPro2 = false;
                    } else if ($scope.FstRegRec.NippleFMWPro == EnumConstants.NippleFMWPro['YFMW']) {
                        $scope.FstRegRec.NippleFMWPro1 = false;
                        $scope.FstRegRec.NippleFMWPro2 = true;
                    };

                    //腹部
                    if ($scope.FstRegRec.AbdomenPro == EnumConstants.AbdomenPro['FRWYT']) {
                        $scope.FstRegRec.AbdomenPro1 = true;
                        $scope.FstRegRec.AbdomenPro2 = false;
                    } else if ($scope.FstRegRec.AbdomenPro == EnumConstants.AbdomenPro['YT']) {
                        $scope.FstRegRec.AbdomenPro1 = false;
                        $scope.FstRegRec.AbdomenPro2 = true;
                    };

                    //腹部板状腹否
                    if ($scope.FstRegRec.AbdomenBZFFlag == undefined || $scope.FstRegRec.AbdomenBZFFlag == null) {
                        $scope.FstRegRec.AbdomenBZF = false;
                    } else if ($scope.FstRegRec.AbdomenBZFFlag == true) {
                        $scope.FstRegRec.AbdomenBZF = true;
                    } else {
                        $scope.FstRegRec.AbdomenBZF = false;
                    };

                    //肠鸣音
                    if ($scope.FstRegRec.BowelSounDPro == EnumConstants.BowelSounDPro['ZC']) {
                        $scope.FstRegRec.BowelSounDPro1 = true;
                        $scope.FstRegRec.BowelSounDPro2 = false;
                        $scope.FstRegRec.BowelSounDPro3 = false;
                    } else if ($scope.FstRegRec.BowelSounDPro == EnumConstants.BowelSounDPro['KJ']) {
                        $scope.FstRegRec.BowelSounDPro1 = false;
                        $scope.FstRegRec.BowelSounDPro2 = true;
                        $scope.FstRegRec.BowelSounDPro3 = false;
                    } else if ($scope.FstRegRec.BowelSounDPro == EnumConstants.BowelSounDPro['JR']) {
                        $scope.FstRegRec.BowelSounDPro1 = false;
                        $scope.FstRegRec.BowelSounDPro2 = false;
                        $scope.FstRegRec.BowelSounDPro3 = true;
                    };

                    //脊柱四肢
                    if ($scope.FstRegRec.SpinelimbsPro == EnumConstants.SpinelimbsPro['HDZR']) {
                        $scope.FstRegRec.SpinelimbsPro1 = true;
                        $scope.FstRegRec.SpinelimbsPro2 = false;
                    } else if ($scope.FstRegRec.SpinelimbsPro == EnumConstants.SpinelimbsPro['HDSX']) {
                        $scope.FstRegRec.SpinelimbsPro1 = false;
                        $scope.FstRegRec.SpinelimbsPro2 = true;
                    };

                    if ($scope.FstRegRec.SpinelimbsJXPro == EnumConstants.SpinelimbsJXPro['WJX']) {
                        $scope.FstRegRec.SpinelimbsJXPro1 = true;
                        $scope.FstRegRec.SpinelimbsJXPro2 = false;
                    } else if ($scope.FstRegRec.SpinelimbsJXPro == EnumConstants.SpinelimbsJXPro['JX']) {
                        $scope.FstRegRec.SpinelimbsJXPro1 = false;
                        $scope.FstRegRec.SpinelimbsJXPro2 = true;
                    };


                    //肛门及外生殖器
                    if ($scope.FstRegRec.AnusgenitaliaPro == EnumConstants.AnusgenitaliaPro['WGZC']) {
                        $scope.FstRegRec.AnusgenitaliaPro1 = true;
                        $scope.FstRegRec.AnusgenitaliaPro2 = false;
                        $scope.FstRegRec.AnusgenitaliaPro3 = false;
                        $scope.FstRegRec.AnusgenitaliaPro4 = false;
                    } else if ($scope.FstRegRec.AnusgenitaliaPro == EnumConstants.AnusgenitaliaPro['NZ']) {
                        $scope.FstRegRec.AnusgenitaliaPro1 = false;
                        $scope.FstRegRec.AnusgenitaliaPro2 = true;
                        $scope.FstRegRec.AnusgenitaliaPro3 = false;
                        $scope.FstRegRec.AnusgenitaliaPro4 = false;
                    } else if ($scope.FstRegRec.AnusgenitaliaPro == EnumConstants.AnusgenitaliaPro['WZ']) {
                        $scope.FstRegRec.AnusgenitaliaPro1 = false;
                        $scope.FstRegRec.AnusgenitaliaPro2 = false;
                        $scope.FstRegRec.AnusgenitaliaPro3 = true;
                        $scope.FstRegRec.AnusgenitaliaPro4 = false;
                    } else if ($scope.FstRegRec.AnusgenitaliaPro == EnumConstants.AnusgenitaliaPro['QT']) {
                        $scope.FstRegRec.AnusgenitaliaPro1 = false;
                        $scope.FstRegRec.AnusgenitaliaPro2 = false;
                        $scope.FstRegRec.AnusgenitaliaPro3 = false;
                        $scope.FstRegRec.AnusgenitaliaPro4 = true;
                    };

                    //神经系统
                    if ($scope.FstRegRec.Nervous == EnumConstants.Nervous['JLZC']) {
                        $scope.FstRegRec.Nervous1 = true;
                        $scope.FstRegRec.Nervous2 = false;
                        $scope.FstRegRec.Nervous3 = false;
                    } else if ($scope.FstRegRec.Nervous == EnumConstants.Nervous['JLJR']) {
                        $scope.FstRegRec.Nervous1 = false;
                        $scope.FstRegRec.Nervous2 = true;
                        $scope.FstRegRec.Nervous3 = false;
                    } else if ($scope.FstRegRec.Nervous == EnumConstants.Nervous['QT']) {
                        $scope.FstRegRec.Nervous1 = false;
                        $scope.FstRegRec.Nervous2 = false;
                        $scope.FstRegRec.Nervous3 = true;
                    };

                    //疼痛否
                    if ($scope.FstRegRec.PainFlag == false) {
                        $scope.FstRegRec.fPainFlag = true;
                        $scope.FstRegRec.tPainFlag = false;
                    } else if ($scope.FstRegRec.PainFlag == true) {
                        $scope.FstRegRec.fPainFlag = false;
                        $scope.FstRegRec.tPainFlag = true;
                    };

                    //营养需求
                    if ($scope.FstRegRec.NutritionalFlag == false) {
                        $scope.FstRegRec.fNutritionalFlag = true;
                        $scope.FstRegRec.tNutritionalFlag = false;
                    } else if ($scope.FstRegRec.NutritionalFlag == true) {
                        $scope.FstRegRec.fNutritionalFlag = false;
                        $scope.FstRegRec.tNutritionalFlag = true;
                    };


                    //康复需求
                    if ($scope.FstRegRec.RehagNeeds == false) {
                        $scope.FstRegRec.fRehagNeeds = true;
                        $scope.FstRegRec.tRehagNeeds = false;
                    } else if ($scope.FstRegRec.RehagNeeds == true) {
                        $scope.FstRegRec.fRehagNeeds = false;
                        $scope.FstRegRec.tRehagNeeds = true;
                    };

                    //特殊需求 牙齿
                    if ($scope.FstRegRec.SpecialNeedsTooth == EnumConstants.SpecialNeedsTooth['ZC']) {
                        $scope.FstRegRec.SpecialNeedsTooth1 = true;
                        $scope.FstRegRec.SpecialNeedsTooth2 = false;
                        $scope.FstRegRec.SpecialNeedsTooth3 = false;
                    } else if ($scope.FstRegRec.SpecialNeedsTooth == EnumConstants.SpecialNeedsTooth['YC']) {
                        $scope.FstRegRec.SpecialNeedsTooth1 = false;
                        $scope.FstRegRec.SpecialNeedsTooth2 = true;
                        $scope.FstRegRec.SpecialNeedsTooth3 = false;
                    } else if ($scope.FstRegRec.SpecialNeedsTooth == EnumConstants.SpecialNeedsTooth['JY']) {
                        $scope.FstRegRec.SpecialNeedsTooth1 = false;
                        $scope.FstRegRec.SpecialNeedsTooth2 = false;
                        $scope.FstRegRec.SpecialNeedsTooth3 = true;
                    };

                    //听力
                    if ($scope.FstRegRec.SpecialNeedHear == EnumConstants.SpecialNeedHear['ZC']) {
                        $scope.FstRegRec.SpecialNeedHear1 = true;
                        $scope.FstRegRec.SpecialNeedHear2 = false;
                    } else if ($scope.FstRegRec.SpecialNeedHear == EnumConstants.SpecialNeedHear['YC']) {
                        $scope.FstRegRec.SpecialNeedHear1 = false;
                        $scope.FstRegRec.SpecialNeedHear2 = true;
                    };

                    //听力 左右
                    if ($scope.FstRegRec.SpecialNeedHearpos == EnumConstants.SpecialNeedHearpos['Z']) {
                        $scope.FstRegRec.SpecialNeedHearpos1 = true;
                        $scope.FstRegRec.SpecialNeedHearpos2 = false;
                    } else if ($scope.FstRegRec.SpecialNeedHearpos == EnumConstants.SpecialNeedHearpos['Y']) {
                        $scope.FstRegRec.SpecialNeedHearpos1 = false;
                        $scope.FstRegRec.SpecialNeedHearpos2 = true;
                    };


                    //视力
                    if ($scope.FstRegRec.SpecialNeedVision == EnumConstants.SpecialNeedVision['ZC']) {
                        $scope.FstRegRec.SpecialNeedVision1 = true;
                        $scope.FstRegRec.SpecialNeedVision2 = false;
                    } else if ($scope.FstRegRec.SpecialNeedVision == EnumConstants.SpecialNeedVision['YC']) {
                        $scope.FstRegRec.SpecialNeedVision1 = false;
                        $scope.FstRegRec.SpecialNeedVision2 = true;
                    };


                    //视力 左右
                    if ($scope.FstRegRec.SpecialNeedVisionPos == EnumConstants.SpecialNeedVisionPos['Z']) {
                        $scope.FstRegRec.SpecialNeedVisionPos1 = true;
                        $scope.FstRegRec.SpecialNeedVisionPos2 = false;
                    } else if ($scope.FstRegRec.SpecialNeedVisionPos == EnumConstants.SpecialNeedVisionPos['Y']) {
                        $scope.FstRegRec.SpecialNeedVisionPos1 = false;
                        $scope.FstRegRec.SpecialNeedVisionPos2 = true;
                    };

                    //宣教对象
                    if ($scope.FstRegRec.Missionobj == EnumConstants.Missionobj['JS']) {
                        $scope.FstRegRec.Missionobj1 = true;
                        $scope.FstRegRec.Missionobj2 = false;
                    } else if ($scope.FstRegRec.Missionobj == EnumConstants.Missionobj['KH']) {
                        $scope.FstRegRec.Missionobj1 = false;
                        $scope.FstRegRec.Missionobj2 = true;
                    };

                    //学习能力
                    if ($scope.FstRegRec.LearnAbility == false) {
                        $scope.FstRegRec.fLearnAbility = true;
                        $scope.FstRegRec.tLearnAbility = false;
                    } else if ($scope.FstRegRec.LearnAbility == true) {
                        $scope.FstRegRec.fLearnAbility = false;
                        $scope.FstRegRec.tLearnAbility = true;
                    };

                    //宣教对象
                    if ($scope.FstRegRec.LearnWish == EnumConstants.LearnWish['G']) {
                        $scope.FstRegRec.LearnWish1 = true;
                        $scope.FstRegRec.LearnWish2 = false;
                    } else if ($scope.FstRegRec.LearnWish == EnumConstants.LearnWish['D']) {
                        $scope.FstRegRec.LearnWish1 = false;
                        $scope.FstRegRec.LearnWish2 = true;
                    };

                    //学习需求
                    if ($scope.FstRegRec.LearnNeedFlag == false) {
                        $scope.FstRegRec.fLearnNeedFlag = true;
                        $scope.FstRegRec.tLearnNeedFlag = false;
                    } else if ($scope.FstRegRec.LearnNeedFlag == true) {
                        $scope.FstRegRec.fLearnNeedFlag = false;
                        $scope.FstRegRec.tLearnNeedFlag = true;
                    };
                } else {
                    $scope.FstRegRec = {};
                };
            });
        };
        
        $scope.init = function () {
            //加载住民信息
            residentV2Res.get({ id: $scope.curResident.FeeNo }, function (response) {
                if (response.Data[0] != null) {
                    $scope.currentItem = response.Data[0];
                    $scope.AddressDtls = $scope.currentItem.City2 + $scope.currentItem.Address2 + $scope.currentItem.Address2Dtl;
                };
            });
            
            $scope.GetFstRegRec();
        };


        $scope.tPrevhxFlag = function () {
            $scope.FstRegRec.PrevhxFlag = true;
            $scope.FstRegRec.fHaveOperation = false;
            $scope.FstRegRec.tHaveOperation = true;
        };

        $scope.fPrevhxFlag = function () {
            $scope.FstRegRec.PrevhxFlag = false;
            $scope.FstRegRec.fHaveOperation = true;
            $scope.FstRegRec.tHaveOperation = false;
        };

        $scope.tMarryFlag = function () {
            $scope.FstRegRec.MarryFlag = true;
            $scope.FstRegRec.fMarryFlag = false;
            $scope.FstRegRec.tMarryFlag = true;
        };

        $scope.fMarryFlag = function () {
            $scope.FstRegRec.MarryFlag = false;
            $scope.FstRegRec.fMarryFlag = true;
            $scope.FstRegRec.tMarryFlag = false;
        };

        $scope.tOffspringFlag = function () {
            $scope.FstRegRec.OffspringFlag = true;
            $scope.FstRegRec.fOffspringFlag = false;
            $scope.FstRegRec.tOffspringFlag = true;
        };

        $scope.fOffspringFlag = function () {
            $scope.FstRegRec.OffspringFlag = false;
            $scope.FstRegRec.fOffspringFlag = true;
            $scope.FstRegRec.tOffspringFlag = false;
        };

        $scope.sMenelipsis1 = function () {
            $scope.FstRegRec.Menelipsis = EnumConstants.MenelipsisEnum['Inapplicability'];
            $scope.FstRegRec.Menelipsis1 = true;
            $scope.FstRegRec.Menelipsis2 = false;
            $scope.FstRegRec.Menelipsis3 = false;
        };

        $scope.sMenelipsis2 = function () {
            $scope.FstRegRec.Menelipsis = EnumConstants.MenelipsisEnum['No'];
            $scope.FstRegRec.Menelipsis1 = false;
            $scope.FstRegRec.Menelipsis2 = true;
            $scope.FstRegRec.Menelipsis3 = false;
        };
        $scope.sMenelipsis3 = function () {
            $scope.FstRegRec.Menelipsis = EnumConstants.MenelipsisEnum['Yes'];
            $scope.FstRegRec.Menelipsis1 = false;
            $scope.FstRegRec.Menelipsis2 = false;
            $scope.FstRegRec.Menelipsis3 = true;
        };

        $scope.tMedhFlag = function () {
            $scope.FstRegRec.MedhFlag = true;
            $scope.FstRegRec.fMedhFlag = false;
            $scope.FstRegRec.tMedhFlag = true;
        };

        $scope.fMedhFlag = function () {
            $scope.FstRegRec.MedhFlag = false;
            $scope.FstRegRec.fMedhFlag = true;
            $scope.FstRegRec.tMedhFlag = false;
        };

        $scope.tAllergichFlag = function () {
            $scope.FstRegRec.AllergichFlag = true;
            $scope.FstRegRec.fAllergichFlag = false;
            $scope.FstRegRec.tAllergichFlag = true;
        };

        $scope.fAllergichFlag = function () {
            $scope.FstRegRec.AllergichFlag = false;
            $scope.FstRegRec.fAllergichFlag = true;
            $scope.FstRegRec.tAllergichFlag = false;
        };

        $scope.tOperationhxFlag = function () {
            $scope.FstRegRec.OperationhxFlag = true;
            $scope.FstRegRec.fOperationhxFlag = false;
            $scope.FstRegRec.tOperationhxFlag = true;
        };

        $scope.fOperationhxFlag = function () {
            $scope.FstRegRec.OperationhxFlag = false;
            $scope.FstRegRec.fOperationhxFlag = true;
            $scope.FstRegRec.tOperationhxFlag = false;
        };

        $scope.tPersonalhxFlag = function () {
            $scope.FstRegRec.PersonalhxFlag = true;
            $scope.FstRegRec.fPersonalhxFlag = false;
            $scope.FstRegRec.tPersonalhxFlag = true;
        };

        $scope.fPersonalhxFlag = function () {
            $scope.FstRegRec.PersonalhxFlag = false;
            $scope.FstRegRec.fPersonalhxFlag = true;
            $scope.FstRegRec.tPersonalhxFlag = false;
        };

        $scope.tFhxFlag = function () {
            $scope.FstRegRec.FhxFlag = true;
            $scope.FstRegRec.fFhxFlag = false;
            $scope.FstRegRec.tFhxFlag = true;
        };

        $scope.fFhxFlag = function () {
            $scope.FstRegRec.FhxFlag = false;
            $scope.FstRegRec.fFhxFlag = true;
            $scope.FstRegRec.tFhxFlag = false;
        };

        $scope.SkinmucosaHRunFlag = function () {
            if ($scope.FstRegRec.SkinmucosaHRunFlag == undefined || $scope.FstRegRec.SkinmucosaHRunFlag == null) {
                $scope.FstRegRec.SkinmucosaHRun = true;
                $scope.FstRegRec.SkinmucosaHRunFlag = true;
            } else if ($scope.FstRegRec.SkinmucosaHRunFlag == false) {
                $scope.FstRegRec.SkinmucosaHRun = true;
                $scope.FstRegRec.SkinmucosaHRunFlag = true;
            } else {
                $scope.FstRegRec.SkinmucosaHRun = false;
                $scope.FstRegRec.SkinmucosaHRunFlag = false;
            };
        };

        $scope.SkinmucosaHRanFlag = function () {
            if ($scope.FstRegRec.SkinmucosaHRanFlag == undefined || $scope.FstRegRec.SkinmucosaHRanFlag == null) {
                $scope.FstRegRec.SkinmucosaHRan = true;
                $scope.FstRegRec.SkinmucosaHRanFlag = true;
            } else if ($scope.FstRegRec.SkinmucosaHRanFlag == false) {
                $scope.FstRegRec.SkinmucosaHRan = true;
                $scope.FstRegRec.SkinmucosaHRanFlag = true;
            } else {
                $scope.FstRegRec.SkinmucosaHRan = false;
                $scope.FstRegRec.SkinmucosaHRanFlag = false;
            };
        };

        $scope.SkinmucosaCBFlag = function () {
            if ($scope.FstRegRec.SkinmucosaCBFlag == undefined || $scope.FstRegRec.SkinmucosaCBFlag == null) {
                $scope.FstRegRec.SkinmucosaCB = true;
                $scope.FstRegRec.SkinmucosaCBFlag = true;
            } else if ($scope.FstRegRec.SkinmucosaCBFlag == false) {
                $scope.FstRegRec.SkinmucosaCB = true;
                $scope.FstRegRec.SkinmucosaCBFlag = true;
            } else {
                $scope.FstRegRec.SkinmucosaCB = false;
                $scope.FstRegRec.SkinmucosaCBFlag = false;
            };
        };

        $scope.SkinmucosaQBFlag = function () {
            if ($scope.FstRegRec.SkinmucosaQBFlag == undefined || $scope.FstRegRec.SkinmucosaQBFlag == null) {
                $scope.FstRegRec.SkinmucosaQB = true;
                $scope.FstRegRec.SkinmucosaQBFlag = true;
            } else if ($scope.FstRegRec.SkinmucosaQBFlag == false) {
                $scope.FstRegRec.SkinmucosaQB = true;
                $scope.FstRegRec.SkinmucosaQBFlag = true;
            } else {
                $scope.FstRegRec.SkinmucosaQB = false;
                $scope.FstRegRec.SkinmucosaQBFlag = false;
            };
        };


        $scope.SkinmucosaPZFlag = function () {
            if ($scope.FstRegRec.SkinmucosaPZFlag == undefined || $scope.FstRegRec.SkinmucosaPZFlag == null) {
                $scope.FstRegRec.SkinmucosaPZ = true;
                $scope.FstRegRec.SkinmucosaPZFlag = true;
            } else if ($scope.FstRegRec.SkinmucosaPZFlag == false) {
                $scope.FstRegRec.SkinmucosaPZ = true;
                $scope.FstRegRec.SkinmucosaPZFlag = true;
            } else {
                $scope.FstRegRec.SkinmucosaPZ = false;
                $scope.FstRegRec.SkinmucosaPZFlag = false;
            };
        };

        $scope.OtherSkinmucosaFlag = function () {
            if ($scope.FstRegRec.OtherSkinmucosaFlag == undefined || $scope.FstRegRec.OtherSkinmucosaFlag == null) {
                $scope.FstRegRec.OtherSkinmucosaF = true;
                $scope.FstRegRec.OtherSkinmucosaFlag = true;
            } else if ($scope.FstRegRec.OtherSkinmucosaFlag == false) {
                $scope.FstRegRec.OtherSkinmucosaF = true;
                $scope.FstRegRec.OtherSkinmucosaFlag = true;
            } else {
                $scope.FstRegRec.OtherSkinmucosaF = false;
                $scope.FstRegRec.OtherSkinmucosaFlag = false;
            };
        };


        $scope.LungGLYFlag = function () {
            if ($scope.FstRegRec.LungGLYFlag == undefined || $scope.FstRegRec.LungGLYFlag == null) {
                $scope.FstRegRec.LungGLY = true;
                $scope.FstRegRec.LungGLYFlag = true;
            } else if ($scope.FstRegRec.LungGLYFlag == false) {
                $scope.FstRegRec.LungGLY = true;
                $scope.FstRegRec.LungGLYFlag = true;
            } else {
                $scope.FstRegRec.LungGLY = false;
                $scope.FstRegRec.LungGLYFlag = false;
            };
        };

        $scope.LungSLYFlag = function () {
            if ($scope.FstRegRec.LungSLYFlag == undefined || $scope.FstRegRec.LungSLYFlag == null) {
                $scope.FstRegRec.LungSLY = true;
                $scope.FstRegRec.LungSLYFlag = true;
            } else if ($scope.FstRegRec.LungSLYFlag == false) {
                $scope.FstRegRec.LungSLY = true;
                $scope.FstRegRec.LungSLYFlag = true;
            } else {
                $scope.FstRegRec.LungSLY = false;
                $scope.FstRegRec.LungSLYFlag = false;
            };
        };

        $scope.BreastDCFlag = function () {
            if ($scope.FstRegRec.BreastDCFlag == undefined || $scope.FstRegRec.BreastDCFlag == null) {
                $scope.FstRegRec.BreastDC = true;
                $scope.FstRegRec.BreastDCFlag = true;
            } else if ($scope.FstRegRec.BreastDCFlag == false) {
                $scope.FstRegRec.BreastDC = true;
                $scope.FstRegRec.BreastDCFlag = true;
            } else {
                $scope.FstRegRec.BreastDC = false;
                $scope.FstRegRec.BreastDCFlag = false;
            };
        };

        $scope.BreastBKFlag = function () {
            if ($scope.FstRegRec.BreastBKFlag == undefined || $scope.FstRegRec.BreastBKFlag == null) {
                $scope.FstRegRec.BreastBK = true;
                $scope.FstRegRec.BreastBKFlag = true;
            } else if ($scope.FstRegRec.BreastBKFlag == false) {
                $scope.FstRegRec.BreastBK = true;
                $scope.FstRegRec.BreastBKFlag = true;
            } else {
                $scope.FstRegRec.BreastBK = false;
                $scope.FstRegRec.BreastBKFlag = false;
            };
        };

        $scope.AbdomenBZFFlag = function () {
            if ($scope.FstRegRec.AbdomenBZFFlag == undefined || $scope.FstRegRec.AbdomenBZFFlag == null) {
                $scope.FstRegRec.AbdomenBZF = true;
                $scope.FstRegRec.AbdomenBZFFlag = true;
            } else if ($scope.FstRegRec.AbdomenBZFFlag == false) {
                $scope.FstRegRec.AbdomenBZF = true;
                $scope.FstRegRec.AbdomenBZFFlag = true;
            } else {
                $scope.FstRegRec.AbdomenBZF = false;
                $scope.FstRegRec.AbdomenBZFFlag = false;
            };
        };

        $scope.tLymphGlandFlag = function () {
            $scope.FstRegRec.LymphGlandFlag = true;
            $scope.FstRegRec.fLymphGlandFlag = false;
            $scope.FstRegRec.tLymphGlandFlag = true;
        };

        $scope.fLymphGlandFlag = function () {
            $scope.FstRegRec.LymphGlandFlag = false;
            $scope.FstRegRec.fLymphGlandFlag = true;
            $scope.FstRegRec.tLymphGlandFlag = false;
        };


        $scope.tHeadFlag = function () {
            $scope.FstRegRec.HeadFlag = true;
            $scope.FstRegRec.fHeadFlag = false;
            $scope.FstRegRec.tHeadFlag = true;
        };

        $scope.fHeadFlag = function () {
            $scope.FstRegRec.HeadFlag = false;
            $scope.FstRegRec.fHeadFlag = true;
            $scope.FstRegRec.tHeadFlag = false;
        };

        $scope.tScleraFlag = function () {
            $scope.FstRegRec.ScleraFlag = true;
            $scope.FstRegRec.fScleraFlag = false;
            $scope.FstRegRec.tScleraFlag = true;
        };

        $scope.fScleraFlag = function () {
            $scope.FstRegRec.ScleraFlag = false;
            $scope.FstRegRec.fScleraFlag = true;
            $scope.FstRegRec.tScleraFlag = false;
        };

        $scope.tEBFlag = function () {
            $scope.FstRegRec.EBFlag = true;
            $scope.FstRegRec.fEBFlag = false;
            $scope.FstRegRec.tEBFlag = true;
        };

        $scope.fEBFlag = function () {
            $scope.FstRegRec.EBFlag = false;
            $scope.FstRegRec.fEBFlag = true;
            $scope.FstRegRec.tEBFlag = false;
        };

        $scope.tConjunctivaFlag = function () {
            $scope.FstRegRec.ConjunctivaFlag = true;
            $scope.FstRegRec.fConjunctivaFlag = false;
            $scope.FstRegRec.tConjunctivaFlag = true;
        };

        $scope.fConjunctivaFlag = function () {
            $scope.FstRegRec.ConjunctivaFlag = false;
            $scope.FstRegRec.fConjunctivaFlag = true;
            $scope.FstRegRec.tConjunctivaFlag = false;
        };

        $scope.tEARFlag = function () {
            $scope.FstRegRec.EARFlag = true;
            $scope.FstRegRec.fEARFlag = false;
            $scope.FstRegRec.tEARFlag = true;
        };

        $scope.fEARFlag = function () {
            $scope.FstRegRec.EARFlag = false;
            $scope.FstRegRec.fEARFlag = true;
            $scope.FstRegRec.tEARFlag = false;
        };

        $scope.AcousticDuctPro1 = function () {
            $scope.FstRegRec.AcousticDuctPro = EnumConstants.AcousticDuctPro['TongChang'];
            $scope.FstRegRec.AcousticDuctPro1 = true;
            $scope.FstRegRec.AcousticDuctPro2 = false;
        };

        $scope.AcousticDuctPro2 = function () {
            $scope.FstRegRec.AcousticDuctPro = EnumConstants.AcousticDuctPro['FenMiWu'];
            $scope.FstRegRec.AcousticDuctPro1 = false;
            $scope.FstRegRec.AcousticDuctPro2 = true;
        };


        $scope.NasalseptumPro1 = function () {
            $scope.FstRegRec.NasalseptumPro = EnumConstants.NasalseptumPro['JuZhong'];
            $scope.FstRegRec.NasalseptumPro1 = true;
            $scope.FstRegRec.NasalseptumPro2 = false;
        };

        $scope.NasalseptumPro2 = function () {
            $scope.FstRegRec.NasalseptumPro = EnumConstants.NasalseptumPro['PianQu'];
            $scope.FstRegRec.NasalseptumPro1 = false;
            $scope.FstRegRec.NasalseptumPro2 = true;
        };

        $scope.Nasalmucosa1 = function () {
            $scope.FstRegRec.Nasalmucosa = EnumConstants.Nasalmucosa['ZhengChang'];
            $scope.FstRegRec.Nasalmucosa1 = true;
            $scope.FstRegRec.Nasalmucosa2 = false;
        };

        $scope.Nasalmucosa2 = function () {
            $scope.FstRegRec.Nasalmucosa = EnumConstants.Nasalmucosa['ChongXue'];
            $scope.FstRegRec.Nasalmucosa1 = false;
            $scope.FstRegRec.Nasalmucosa2 = true;
        };

        $scope.Nasalshoulder1 = function () {
            $scope.FstRegRec.Nasalshoulder = EnumConstants.Nasalmucosa['ZhengChang'];
            $scope.FstRegRec.Nasalshoulder1 = true;
            $scope.FstRegRec.Nasalshoulder2 = false;
        };

        $scope.Nasalshoulder2 = function () {
            $scope.FstRegRec.Nasalshoulder = EnumConstants.Nasalmucosa['FeiDa'];
            $scope.FstRegRec.Nasalshoulder1 = false;
            $scope.FstRegRec.Nasalshoulder2 = true;
        };

        $scope.LIP1 = function () {
            $scope.FstRegRec.LIP = EnumConstants.LIP['HongRun'];
            $scope.FstRegRec.LIP1 = true;
            $scope.FstRegRec.LIP2 = false;
            $scope.FstRegRec.LIP3 = false;
        };

        $scope.LIP2 = function () {
            $scope.FstRegRec.LIP = EnumConstants.LIP['ZiGan'];
            $scope.FstRegRec.LIP1 = false;
            $scope.FstRegRec.LIP2 = true;
            $scope.FstRegRec.LIP3 = false;
        };

        $scope.LIP3 = function () {
            $scope.FstRegRec.LIP = EnumConstants.LIP['CangBai'];
            $scope.FstRegRec.LIP1 = false;
            $scope.FstRegRec.LIP2 = false;
            $scope.FstRegRec.LIP3 = true;
        };

        $scope.Tongue1 = function () {
            $scope.FstRegRec.Tongue = EnumConstants.Tongue['SSJZ'];
            $scope.FstRegRec.Tongue1 = true;
            $scope.FstRegRec.Tongue2 = false;
            $scope.FstRegRec.Tongue3 = false;
        };

        $scope.Tongue2 = function () {
            $scope.FstRegRec.Tongue = EnumConstants.Tongue['SSZP'];
            $scope.FstRegRec.Tongue1 = false;
            $scope.FstRegRec.Tongue2 = true;
            $scope.FstRegRec.Tongue3 = false;
        };

        $scope.Tongue3 = function () {
            $scope.FstRegRec.Tongue = EnumConstants.Tongue['SSYP'];
            $scope.FstRegRec.Tongue1 = false;
            $scope.FstRegRec.Tongue2 = false;
            $scope.FstRegRec.Tongue3 = true;
        };

        $scope.OralmucosaPro1 = function () {
            $scope.FstRegRec.OralmucosaPro = EnumConstants.OralmucosaPro['HongRun'];
            $scope.FstRegRec.OralmucosaPro1 = true;
            $scope.FstRegRec.OralmucosaPro2 = false;
        };

        $scope.OralmucosaPro2 = function () {
            $scope.FstRegRec.OralmucosaPro = EnumConstants.OralmucosaPro['KuiYang'];
            $scope.FstRegRec.OralmucosaPro1 = false;
            $scope.FstRegRec.OralmucosaPro2 = true;
        };

        $scope.Tooth1 = function () {
            $scope.FstRegRec.Tooth = EnumConstants.Tooth['WuChongXue'];
            $scope.FstRegRec.Tooth1 = true;
            $scope.FstRegRec.Tooth2 = false;
        };

        $scope.Tooth2 = function () {
            $scope.FstRegRec.Tooth = EnumConstants.Tooth['ChongXue'];
            $scope.FstRegRec.Tooth1 = false;
            $scope.FstRegRec.Tooth2 = true;
        };

        $scope.Tonsil1 = function () {
            $scope.FstRegRec.Tonsil = EnumConstants.Tonsil['WuZhongDa'];
            $scope.FstRegRec.Tonsil1 = true;
            $scope.FstRegRec.Tonsil2 = false;
        };

        $scope.Tonsil2 = function () {
            $scope.FstRegRec.Tonsil = EnumConstants.Tonsil['ZhongDa'];
            $scope.FstRegRec.Tonsil1 = false;
            $scope.FstRegRec.Tonsil2 = true;
        };

        $scope.TonsilHN1 = function () {
            $scope.FstRegRec.TonsilHN = EnumConstants.TonsilHN['WuHuaNong'];
            $scope.FstRegRec.TonsilHN1 = true;
            $scope.FstRegRec.TonsilHN2 = false;
        };

        $scope.TonsilHN2 = function () {
            $scope.FstRegRec.TonsilHN = EnumConstants.TonsilHN['HuaNong'];
            $scope.FstRegRec.TonsilHN1 = false;
            $scope.FstRegRec.TonsilHN2 = true;
        };

        $scope.Organ1 = function () {
            $scope.FstRegRec.Organ = EnumConstants.Organ['JZ'];
            $scope.FstRegRec.Organ1 = true;
            $scope.FstRegRec.Organ2 = false;
            $scope.FstRegRec.Organ3 = false;
        };

        $scope.Organ2 = function () {
            $scope.FstRegRec.Organ = EnumConstants.Organ['ZP'];
            $scope.FstRegRec.Organ1 = false;
            $scope.FstRegRec.Organ2 = true;
            $scope.FstRegRec.Organ3 = false;
        };

        $scope.Organ3 = function () {
            $scope.FstRegRec.Organ = EnumConstants.Organ['YP'];
            $scope.FstRegRec.Organ1 = false;
            $scope.FstRegRec.Organ2 = false;
            $scope.FstRegRec.Organ3 = true;
        };

        $scope.ThyroidGland1 = function () {
            $scope.FstRegRec.ThyroidGland = EnumConstants.ThyroidGland['WuZhogDa'];
            $scope.FstRegRec.ThyroidGland1 = true;
            $scope.FstRegRec.ThyroidGland2 = false;
        };

        $scope.ThyroidGland2 = function () {
            $scope.FstRegRec.ThyroidGland = EnumConstants.ThyroidGland['ZhogDa'];
            $scope.FstRegRec.ThyroidGland1 = false;
            $scope.FstRegRec.ThyroidGland2 = true;
        };


        $scope.tThoraxFlag = function () {
            $scope.FstRegRec.ThoraxFlag = true;
            $scope.FstRegRec.fThoraxFlag = false;
            $scope.FstRegRec.tThoraxFlag = true;
        };

        $scope.fThoraxFlag = function () {
            $scope.FstRegRec.ThoraxFlag = false;
            $scope.FstRegRec.fThoraxFlag = true;
            $scope.FstRegRec.tThoraxFlag = false;
        };

        $scope.LungPro1 = function () {
            $scope.FstRegRec.LungPro = EnumConstants.LungPro['HXYQX'];
            $scope.FstRegRec.LungPro1 = true;
            $scope.FstRegRec.LungPro2 = false;
            $scope.FstRegRec.LungPro3 = false;
        };

        $scope.LungPro2 = function () {
            $scope.FstRegRec.LungPro = EnumConstants.LungPro['HXYC'];
            $scope.FstRegRec.LungPro1 = false;
            $scope.FstRegRec.LungPro2 = true;
            $scope.FstRegRec.LungPro3 = false;
        };

        $scope.LungPro3 = function () {
            $scope.FstRegRec.LungPro = EnumConstants.LungPro['HXYD'];
            $scope.FstRegRec.LungPro1 = false;
            $scope.FstRegRec.LungPro2 = false;
            $scope.FstRegRec.LungPro3 = true;
        };


        $scope.tHeartrhythmFlag = function () {
            $scope.FstRegRec.HeartrhythmFlag = true;
            $scope.FstRegRec.fHeartrhythmFlag = false;
            $scope.FstRegRec.tHeartrhythmFlag = true;
        };

        $scope.fHeartrhythmFlag = function () {
            $scope.FstRegRec.HeartrhythmFlag = false;
            $scope.FstRegRec.fHeartrhythmFlag = true;
            $scope.FstRegRec.tHeartrhythmFlag = false;
        };

        $scope.HeartSound1 = function () {
            $scope.FstRegRec.HeartSound = EnumConstants.HeartSound['YL'];
            $scope.FstRegRec.HeartSound1 = true;
            $scope.FstRegRec.HeartSound2 = false;
            $scope.FstRegRec.HeartSound3 = false;
        };

        $scope.HeartSound2 = function () {
            $scope.FstRegRec.HeartSound = EnumConstants.HeartSound['DD'];
            $scope.FstRegRec.HeartSound1 = false;
            $scope.FstRegRec.HeartSound2 = true;
            $scope.FstRegRec.HeartSound3 = false;
        };

        $scope.HeartSound3 = function () {
            $scope.FstRegRec.HeartSound = EnumConstants.HeartSound['QRBD'];
            $scope.FstRegRec.HeartSound1 = false;
            $scope.FstRegRec.HeartSound2 = false;
            $scope.FstRegRec.HeartSound3 = true;
        };


        $scope.NipplePro1 = function () {
            $scope.FstRegRec.NipplePro = EnumConstants.NipplePro['ZC'];
            $scope.FstRegRec.NipplePro1 = true;
            $scope.FstRegRec.NipplePro2 = false;
        };

        $scope.NipplePro2 = function () {
            $scope.FstRegRec.NipplePro = EnumConstants.NipplePro['NX'];
            $scope.FstRegRec.NipplePro1 = false;
            $scope.FstRegRec.NipplePro2 = true;
        };

        $scope.NippleFMWPro1 = function () {
            $scope.FstRegRec.NippleFMWPro = EnumConstants.NippleFMWPro['WFMW'];
            $scope.FstRegRec.NippleFMWPro1 = true;
            $scope.FstRegRec.NippleFMWPro2 = false;
        };

        $scope.NippleFMWPro2 = function () {
            $scope.FstRegRec.NippleFMWPro = EnumConstants.NippleFMWPro['YFMW'];
            $scope.FstRegRec.NippleFMWPro1 = false;
            $scope.FstRegRec.NippleFMWPro2 = true;
        };

        $scope.AbdomenPro1 = function () {
            $scope.FstRegRec.AbdomenPro = EnumConstants.AbdomenPro['FRWYT'];
            $scope.FstRegRec.AbdomenPro1 = true;
            $scope.FstRegRec.AbdomenPro2 = false;
        };

        $scope.AbdomenPro2= function () {
            $scope.FstRegRec.AbdomenPro = EnumConstants.AbdomenPro['YT'];
            $scope.FstRegRec.AbdomenPro1 = false;
            $scope.FstRegRec.AbdomenPro2 = true;
        };

        $scope.BowelSounDPro1 = function () {
            $scope.FstRegRec.BowelSounDPro = EnumConstants.BowelSounDPro['ZC'];
            $scope.FstRegRec.BowelSounDPro1 = true;
            $scope.FstRegRec.BowelSounDPro2 = false;
            $scope.FstRegRec.BowelSounDPro3 = false;
        };

        $scope.BowelSounDPro2 = function () {
            $scope.FstRegRec.BowelSounDPro = EnumConstants.BowelSounDPro['KJ'];
            $scope.FstRegRec.BowelSounDPro1 = false;
            $scope.FstRegRec.BowelSounDPro2 = true;
            $scope.FstRegRec.BowelSounDPro3 = false;
        };

        $scope.BowelSounDPro3 = function () {
            $scope.FstRegRec.BowelSounDPro = EnumConstants.BowelSounDPro['JR'];
            $scope.FstRegRec.BowelSounDPro1 = false;
            $scope.FstRegRec.BowelSounDPro2 = false;
            $scope.FstRegRec.BowelSounDPro3 = true;
        };

        $scope.SpinelimbsPro1 = function () {
            $scope.FstRegRec.SpinelimbsPro = EnumConstants.SpinelimbsPro['HDZR'];
            $scope.FstRegRec.SpinelimbsPro1 = true;
            $scope.FstRegRec.SpinelimbsPro2 = false;
        };

        $scope.SpinelimbsPro2 = function () {
            $scope.FstRegRec.SpinelimbsPro = EnumConstants.SpinelimbsPro['HDSX'];
            $scope.FstRegRec.SpinelimbsPro1 = false;
            $scope.FstRegRec.SpinelimbsPro2 = true;
        };


        $scope.SpinelimbsJXPro1 = function () {
            $scope.FstRegRec.SpinelimbsJXPro = EnumConstants.SpinelimbsJXPro['WJX'];
            $scope.FstRegRec.SpinelimbsJXPro1 = true;
            $scope.FstRegRec.SpinelimbsJXPro2 = false;
        };

        $scope.SpinelimbsJXPro2 = function () {
            $scope.FstRegRec.SpinelimbsJXPro = EnumConstants.SpinelimbsJXPro['JX'];
            $scope.FstRegRec.SpinelimbsJXPro1 = false;
            $scope.FstRegRec.SpinelimbsJXPro2 = true;
        };


        $scope.AnusgenitaliaPro1 = function () {
            $scope.FstRegRec.AnusgenitaliaPro = EnumConstants.AnusgenitaliaPro['WGZC'];
            $scope.FstRegRec.AnusgenitaliaPro1 = true;
            $scope.FstRegRec.AnusgenitaliaPro2 = false;
            $scope.FstRegRec.AnusgenitaliaPro3 = false;
            $scope.FstRegRec.AnusgenitaliaPro4 = false;
        };

        $scope.AnusgenitaliaPro2 = function () {
            $scope.FstRegRec.AnusgenitaliaPro = EnumConstants.AnusgenitaliaPro['WZ'];
            $scope.FstRegRec.AnusgenitaliaPro1 = false;
            $scope.FstRegRec.AnusgenitaliaPro2 = true;
            $scope.FstRegRec.AnusgenitaliaPro3 = false;
            $scope.FstRegRec.AnusgenitaliaPro4 = false;
        };

        $scope.AnusgenitaliaPro3 = function () {
            $scope.FstRegRec.AnusgenitaliaPro = EnumConstants.AnusgenitaliaPro['NZ'];
            $scope.FstRegRec.AnusgenitaliaPro1 = false;
            $scope.FstRegRec.AnusgenitaliaPro2 = false;
            $scope.FstRegRec.AnusgenitaliaPro3 = true;
            $scope.FstRegRec.AnusgenitaliaPro4 = false;
        };

        $scope.AnusgenitaliaPro4 = function () {
            $scope.FstRegRec.AnusgenitaliaPro = EnumConstants.AnusgenitaliaPro['QT'];
            $scope.FstRegRec.AnusgenitaliaPro1 = false;
            $scope.FstRegRec.AnusgenitaliaPro2 = false;
            $scope.FstRegRec.AnusgenitaliaPro3 = false;
            $scope.FstRegRec.AnusgenitaliaPro4 = true;
        };

        $scope.Nervous1 = function () {
            $scope.FstRegRec.Nervous = EnumConstants.Nervous['JLZC'];
            $scope.FstRegRec.Nervous1 = true;
            $scope.FstRegRec.Nervous2 = false;
            $scope.FstRegRec.Nervous3 = false;
        };

        $scope.Nervous2 = function () {
            $scope.FstRegRec.Nervous = EnumConstants.Nervous['JLJR'];
            $scope.FstRegRec.Nervous1 = false;
            $scope.FstRegRec.Nervous2 = true;
            $scope.FstRegRec.Nervous3 = false;
        };

        $scope.Nervous3 = function () {
            $scope.FstRegRec.Nervous = EnumConstants.Nervous['QT'];
            $scope.FstRegRec.Nervous1 = false;
            $scope.FstRegRec.Nervous2 = false;
            $scope.FstRegRec.Nervous3 = true;
        };

        $scope.tPainFlag = function () {
            $scope.FstRegRec.PainFlag = true;
            $scope.FstRegRec.fPainFlag = false;
            $scope.FstRegRec.tPainFlag = true;
        };

        $scope.fPainFlag = function () {
            $scope.FstRegRec.PainFlag = false;
            $scope.FstRegRec.fPainFlag = true;
            $scope.FstRegRec.tPainFlag = false;
        };

        $scope.tNutritionalFlag = function () {
            $scope.FstRegRec.NutritionalFlag = true;
            $scope.FstRegRec.fNutritionalFlag = false;
            $scope.FstRegRec.tNutritionalFlag = true;
        };

        $scope.fNutritionalFlag = function () {
            $scope.FstRegRec.NutritionalFlag = false;
            $scope.FstRegRec.fNutritionalFlag = true;
            $scope.FstRegRec.tNutritionalFlag = false;
        };

        $scope.tRehagNeeds = function () {
            $scope.FstRegRec.RehagNeeds = true;
            $scope.FstRegRec.fRehagNeeds = false;
            $scope.FstRegRec.tRehagNeeds = true;
        };

        $scope.fRehagNeeds = function () {
            $scope.FstRegRec.RehagNeeds = false;
            $scope.FstRegRec.fRehagNeeds = true;
            $scope.FstRegRec.tRehagNeeds = false;
        };


        $scope.SpecialNeedsTooth1 = function () {
            $scope.FstRegRec.SpecialNeedsTooth = EnumConstants.SpecialNeedsTooth['ZC'];
            $scope.FstRegRec.SpecialNeedsTooth1 = true;
            $scope.FstRegRec.SpecialNeedsTooth2 = false;
            $scope.FstRegRec.SpecialNeedsTooth3 = false;
        };

        $scope.SpecialNeedsTooth2 = function () {
            $scope.FstRegRec.SpecialNeedsTooth = EnumConstants.SpecialNeedsTooth['YC'];
            $scope.FstRegRec.SpecialNeedsTooth1 = false;
            $scope.FstRegRec.SpecialNeedsTooth2 = true;
            $scope.FstRegRec.SpecialNeedsTooth3 = false;
        };

        $scope.SpecialNeedsTooth3 = function () {
            $scope.FstRegRec.SpecialNeedsTooth = EnumConstants.SpecialNeedsTooth['JY'];
            $scope.FstRegRec.SpecialNeedsTooth1 = false;
            $scope.FstRegRec.SpecialNeedsTooth2 = false;
            $scope.FstRegRec.SpecialNeedsTooth3 = true;
        };

        $scope.SpecialNeedHear1 = function () {
            $scope.FstRegRec.SpecialNeedHear = EnumConstants.SpecialNeedHear['ZC'];
            $scope.FstRegRec.SpecialNeedHear1 = true;
            $scope.FstRegRec.SpecialNeedHear2 = false;
        };

        $scope.SpecialNeedHear2 = function () {
            $scope.FstRegRec.SpecialNeedHear = EnumConstants.SpecialNeedHear['YC'];
            $scope.FstRegRec.SpecialNeedHear1 = false;
            $scope.FstRegRec.SpecialNeedHear2 = true;
        };

        $scope.SpecialNeedHearpos1 = function () {
            $scope.FstRegRec.SpecialNeedHearpos = EnumConstants.SpecialNeedHearpos['Z'];
            $scope.FstRegRec.SpecialNeedHearpos1 = true;
            $scope.FstRegRec.SpecialNeedHearpos2 = false;
        };

        $scope.SpecialNeedHearpos2 = function () {
            $scope.FstRegRec.SpecialNeedHearpos = EnumConstants.SpecialNeedHearpos['Y'];
            $scope.FstRegRec.SpecialNeedHearpos1 = false;
            $scope.FstRegRec.SpecialNeedHearpos2 = true;
        };


        $scope.SpecialNeedVision1 = function () {
            $scope.FstRegRec.SpecialNeedVision = EnumConstants.SpecialNeedVision['ZC'];
            $scope.FstRegRec.SpecialNeedVision1 = true;
            $scope.FstRegRec.SpecialNeedVision2 = false;
        };

        $scope.SpecialNeedVision2 = function () {
            $scope.FstRegRec.SpecialNeedVision = EnumConstants.SpecialNeedVision['YC'];
            $scope.FstRegRec.SpecialNeedVision1 = false;
            $scope.FstRegRec.SpecialNeedVision2 = true;
        };

        $scope.SpecialNeedVisionPos1 = function () {
            $scope.FstRegRec.SpecialNeedVisionPos = EnumConstants.SpecialNeedVisionPos['Z'];
            $scope.FstRegRec.SpecialNeedVisionPos1 = true;
            $scope.FstRegRec.SpecialNeedVisionPos2 = false;
        };

        $scope.SpecialNeedVisionPos2 = function () {
            $scope.FstRegRec.SpecialNeedVisionPos = EnumConstants.SpecialNeedVisionPos['Y'];
            $scope.FstRegRec.SpecialNeedVisionPos1 = false;
            $scope.FstRegRec.SpecialNeedVisionPos2 = true;
        };

        $scope.Missionobj1 = function () {
            $scope.FstRegRec.Missionobj = EnumConstants.Missionobj['JS'];
            $scope.FstRegRec.Missionobj1 = true;
            $scope.FstRegRec.Missionobj2 = false;
        };

        $scope.Missionobj2 = function () {
            $scope.FstRegRec.Missionobj = EnumConstants.Missionobj['KH'];
            $scope.FstRegRec.Missionobj1 = false;
            $scope.FstRegRec.Missionobj2 = true;
        };

        $scope.tLearnAbility = function () {
            $scope.FstRegRec.LearnAbility = true;
            $scope.FstRegRec.fLearnAbility = false;
            $scope.FstRegRec.tLearnAbility = true;
        };

        $scope.fLearnAbility = function () {
            $scope.FstRegRec.LearnAbility = false;
            $scope.FstRegRec.fLearnAbility = true;
            $scope.FstRegRec.tLearnAbility = false;
        };

        $scope.LearnWish1 = function () {
            $scope.FstRegRec.LearnWish = EnumConstants.LearnWish['G'];
            $scope.FstRegRec.LearnWish1 = true;
            $scope.FstRegRec.LearnWish2 = false;
        };

        $scope.LearnWish2 = function () {
            $scope.FstRegRec.LearnWish = EnumConstants.LearnWish['D'];
            $scope.FstRegRec.LearnWish1 = false;
            $scope.FstRegRec.LearnWish2 = true;
        };


        $scope.tLearnNeedFlag = function () {
            $scope.FstRegRec.LearnNeedFlag = true;
            $scope.FstRegRec.fLearnNeedFlag = false;
            $scope.FstRegRec.tLearnNeedFlag = true;
        };

        $scope.fLearnNeedFlag = function () {
            $scope.FstRegRec.LearnNeedFlag = false;
            $scope.FstRegRec.fLearnNeedFlag = true;
            $scope.FstRegRec.tLearnNeedFlag = false;
        };


        $scope.save = function () {
            if ($scope.FeeNo == undefined || $scope.FeeNo == null || $scope.FeeNo == "") {
                utility.msgwarning("请选择一个住民!");
                return;
            };

            if ($scope.FstRegRec) {
                $scope.FstRegRec.RegNo = $scope.curResident.RegNo;
                $scope.FstRegRec.IsDelete = false;

                if ($scope.currentItem.InDate != undefined && $scope.currentItem.InDate != null && $scope.currentItem.InDate != "") {
                    $scope.FstRegRec.RegTime = $scope.currentItem.InDate;
                };

                FstRegRecRes.save($scope.FstRegRec, function (data) {
                    if (data.ResultCode == -1) {
                        utility.msgwarning(data.ResultMessage);
                    }
                    else {
                        if (data.Data.Weight != undefined && data.Data.Weight != null && data.Data.Weight != "" && data.Data.Height != undefined && data.Data.Height != null && data.Data.Height != "") {
                            $scope.FstRegRec.BMI = utility.BMI(data.Data.Weight, data.Data.Height);
                            $scope.FstRegRec.BMIFruit = utility.BMIResult($scope.FstRegRec.BMI);
                        };
                        $scope.FstRegRec.Id = data.Data.Id;
                        utility.message("保存成功!");
                    };
                });
            }
        }

        $scope.init();
}]);
