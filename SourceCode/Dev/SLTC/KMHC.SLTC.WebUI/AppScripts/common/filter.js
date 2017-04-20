///创建人:肖国栋
///创建日期:2016-03-24
///说明:自定义filter

(function () {
    var app = angular.module("extentFilter", []);

    app.filter('dateFormat', function () {
        return function (input, capitalize_index, specified_char) {
            input = input || '';
            if (input == "" || input == null) {
                return "";
            }
            var output = (newDate(input)).format("yyyy-MM-dd");
            return output;
        };
    });

    app.filter('timeFormat', function () {
        return function (input, capitalize_index, specified_char) {
            input = input || '';
            if (input == "" || input == null) {
                return "";
            }
            var output = (newDate(input)).format("yyyy-MM-dd hh:mm:ss");
            return output;
        };
    });

    app.filter('ageFormat', function () {
        return function (input, capitalize_index, specified_char) {
            input = input || '';
            if (input == "" || input == null) {
                return "";
            }
            var output = (new Date().getFullYear() - newDate(input).getFullYear());
            return output;
        };
    });

    app.filter('sexFormat', [function () {
        return function (input) {
            var output = "";
            switch (input) {
                case "F":
                    output = "女";
                    break;
                case "M":
                    output = "男";
                    break;
                default:
            }
            return output;
        }
    }]);

    app.filter('fileNameFormat', function () {
        return function (input, capitalize_index, specified_char) {
            input = input || '';
            if (input == "" || input == null) {
                return "";
            }
            var fi = input.split('|$|');
            var output = fi[0];
            return output;
        };
    });
    //add by Duke on 20160906 
    app.filter('filePathFormat', function () {
        return function (input, capitalize_index, specified_char) {
            input = input || '';
            if (input == "" || input == null) {
                return "";
            }
            var fi = input.split('|$|');
            if (fi.length < 1) {
                return "";
            }
            var output = fi[1];
            return output;
        };
    });
    app.filter('twTimeFormat', function () {
        return function (input, capitalize_index, specified_char) {
            input = input || '';
            if (input == "" || input == null) {
                return "";
            }
            return input.toTwDate();
        };
    });
    app.filter('trustHtml', ['$sce', function ($sce) {
        return function (input) {
            return $sce.trustAsHtml(input);
        }
    }]);
    app.filter('cut', function () {
        return function (value, wordwise, max, tail) {
            if (!value) return '';

            max = parseInt(max, 10);
            if (!max) return value;
            if (value.length <= max) return value;

            value = value.substr(0, max);
            if (wordwise) {
                var lastspace = value.lastIndexOf(' ');
                if (lastspace != -1) {
                    value = value.substr(0, lastspace);
                }
            }

            return value + (tail || ' …');
        };
    });
    app.filter('codeText', ['$rootScope', '$timeout', '$http', function ($rootScope, $timeout, $http) {
        return function (input, codeId) {
            if (!angular.isDefined(input)) {
                return "";
            }
            if (!angular.isDefined(codeId)) {
                return input;
            }
            input = input || '';
            var output = '';
            var tmpDics = $rootScope.TmpDics;
            var dics = $rootScope.Dics;//缩写
            if (!angular.isDefined(dics[codeId])) {
                dics[codeId] = {};
                if (tmpDics.length === 0) {
                    $timeout(function () {
                        $http.post("api/Code", { ItemTypes: tmpDics }).success(function (response, status, headers, config) {
                            $.each(response.Data, function (key, value) {
                                dics[key] = response.Data[key];
                            });
                            tmpDics.splice(0, tmpDics.length);
                        });
                    }, 100);
                }
                tmpDics.push(codeId);
            }
            if (dics[codeId].length > 0) {
                if (angular.isDefined(dics[codeId])) {
                    var codeName = "";
                    var arrayVals = input.split(",");
                    angular.forEach(dics[codeId], function (e) {
                        angular.forEach(arrayVals, function (key) {
                            if (e.ItemCode === key) {
                                codeName += e.ItemName + ",";
                            }
                        });
                    });
                    if (codeName != "") {
                        codeName = codeName.substr(0, codeName.length - 1);
                    }
                    output = codeName === "" ? input : codeName;
                }
            }
            return output;
        };
    }]);

    app.filter('htmlContent', ['$sce', function ($sce) {
        return function (input) {
            return $sce.trustAsHtml(input);
        }
    }]);


    app.filter('recordTypeName', [function () {
        return function (input) {
            var output = "";
            switch (input) {
                case "D":
                    output = "药品";
                    break;
                case "M":
                    output = "耗材";
                    break;
                case "S":
                    output = "服务";
                    break;
                default:
            }
            return output;
        }
    }]);

    app.filter('boolName', [function () {
        return function (input) {
            var output = "";
            switch (input) {
                case true:
                    output = "是";
                    break;
                case false:
                    output = "否";
                    break;
                default:
            }
            return output;
        }
    }]);

    app.filter('ChargeRecordTypeFormat', function () {
        return function (input) {
            var output = "";
            switch (input) {
                case 1:
                    output = "药品";
                    break;
                case 2:
                    output = "耗材";
                    break;
                case 3:
                    output = "服务";
                    break;
                case 4:
                    output = "套餐";
                    break;
            }
            return output;
        };
    });

    app.filter('ChargeTypeId', function () {
        return function (input) {
            var output = "";
            switch (input) {
                case "001":
                    output = "床位费";
                    break;
                case "002":
                    output = "护理费";
                    break;
                case "003":
                    output = "西药费";
                    break;
                case "004":
                    output = "中药费";
                    break;
                case "005":
                    output = "化验费";
                    break;
                case "006":
                    output = "诊疗费";
                    break;
                case "007":
                    output = "手术费";
                    break;
                case "008":
                    output = "检查费";
                    break;
                case "009":
                    output = "材料费";
                    break;
                case "010":
                    output = "其他费用";
                    break;
            }
            return output;
        };
    });

    app.filter('ChargeGroupPeriodFormat', function () {
        return function (input) {
            var output = "";
            switch (input) {
                case "001":
                    output = "日";
                    break;
                case "002":
                    output = "月";
                    break;
                case "003":
                    output = "季度";
                    break;
                case "004":
                    output = "年";
                    break;
                case "999":
                    output = "未填";
                    break;
            }
            return output;
        };
    });

    app.filter('PayType', function () {
        return function (input) {
            var output = "";
            switch (input) {
                case "001":
                    output = "现金";
                    break;
                case "002":
                    output = "刷卡";
                    break;
                case "003":
                    output = "支票";
                    break;
                case "004":
                    output = "汇款";
                    break;
            }
            return output;
        };
    });

    app.filter('ChargeRecordStatusFormat', function () {
        return function (input) {
            var output = "";
            switch (input) {
                case 0:
                    output = "未缴费";
                    break;
                case 1:
                    output = "生成账单";
                    break;
                case 2:
                    output = "已缴费";
                    break;
                case 8:
                    output = "已退款";
                    break;
                case 20:
                    output = "已上传";
                    break;
            }
            return output;
        };
    });

    app.filter('MonthFeeStatusFormat', function () {
        return function (input) {
            var output = "";
            switch (input) {
                case 10:
                    output = "待审核";
                    break;
                case 20:
                    output = "审核通过";
                    break;
                case 30:
                    output = "已拨款";
                    break;
                case 90:
                    output = "审核不通过";
                    break;
            }
            return output;
        };
    });
    app.filter('feeTypeFormat', function () {
        return function (input) {
            var output = "";
            switch (input) {
                case "1":
                    output = "药品";
                    break;
                case "2":
                    output = "耗材";
                    break;
                case "3":
                    output = "服务";
                    break;
            }
            return output;
        };
    });
    app.filter('primaryNurseFormat', function () {
        return function (input) {
            var output = "";
            if (!input || !input.EmpName) {
                return output;
            }
            output = "主责护士: " + input.EmpName + "/" + (input.HomeTelNo ? input.HomeTelNo : "");
            return output;
        };
    });
    app.filter('personPhotoFormat', function () {
        return function (input) {
            return input != null && input != '' ? input : '/Images/defaultavatar.png';
        };
    });

    app.filter('bedStatusFormat', function () {
        return function (input) {
            var output = "";
            switch (input) {
                case "Used":
                    output = "已使用";
                    break;
                case "Empty":
                    output = "空置";
                    break;
                case "Subscribe":
                    output = "已预定";
                    break;
                case "Disable":
                    output = "停用";
                    break;
            }
            return output;
        };
    });

    app.filter('insbedFlagFormat', function () {
        return function (input) {
            var output = "";
            switch (input) {
                case "N":
                    output = "否";
                    break;
                case "Y":
                    output = "是";
                    break;
                default:
                    output = "否";
            }
            return output;
        };
    });
    app.filter('earnestStatusFormat', function () {
        return function (input) {
            var output = "";
            switch (input) {
                case "1":
                    output = "未交订金";
                    break;
                case "2":
                    output = "已交订金";
                    break;
                case "3":
                    output = "已退订金";
                    break;
            }
            return output;
        };
    });
    app.filter('careTypeFormat', function () {
        return function (input) {
            var output = "";
            switch (input) {
                case "1":
                    output = "一级专护";
                    break;
                case "2":
                    output = "二级专护";
                    break;
                case "3":
                    output = "机构护理";
                    break;
                case "9":
                    output = "无";
                    break;
            }
            return output;
        };
    });
    app.filter('MonFeeStatusFormat', function () {
        return function (input) {
            var output = "";
            switch (input) {
                case 0:
                    output = "待审核";
                    break;
                case 1:
                    output = "已撤回";
                    break;
                case 10:
                    output = "待审核";
                    break;
                case 20:
                    output = "审核通过";
                    break;
                case 30:
                    output = "已拨款";
                    break;
                case 90:
                    output = "审核不通过";
                    break;


            }
            return output;
        };
    });
})();