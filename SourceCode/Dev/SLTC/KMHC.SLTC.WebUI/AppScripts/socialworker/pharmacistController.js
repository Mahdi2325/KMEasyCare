angular.module('sltcApp')
.controller('pharmacistEvalCtrl', ['$scope', '$state', 'utility', 'dictionary', 'pharmacistevalsRes', 'pipelinerecRes', 'visitdocrecordsRes', 'MedicationRecordRes',
function ($scope, $state, utility, dictionary, pharmacistevalsRes, pipelinerecRes, visitdocrecordsRes, MedicationRecordRes) {
    $scope.FeeNo = $state.params.FeeNo;
    //当前分页码
    $scope.currentPage = 1;
    $scope.Data = {};
    $scope.init = function () {
        $scope.EvalDateGap = [{ ITEMNAME: "7", ITEMCODE: 7 }, { ITEMNAME: "14", ITEMCODE: 14 }, { ITEMNAME: "28", ITEMCODE: 28 }, { ITEMNAME: "30", ITEMCODE: 30 }, { ITEMNAME: "60", ITEMCODE: 60 }, { ITEMNAME: "90", ITEMCODE: 90 }];
        Date.prototype.format = function (format) {
            var date = {
                "M+": this.getMonth() + 1,
                "d+": this.getDate(),
                "h+": this.getHours(),
                "m+": this.getMinutes(),
                "s+": this.getSeconds(),
                "q+": Math.floor((this.getMonth() + 3) / 3),
                "S+": this.getMilliseconds()
            };
            if (/(y+)/i.test(format)) {
                format = format.replace(RegExp.$1, (this.getFullYear() + '').substr(4 - RegExp.$1.length));
            }
            for (var k in date) {
                if (new RegExp("(" + k + ")").test(format)) {
                    format = format.replace(RegExp.$1, RegExp.$1.length == 1
                           ? date[k] : ("00" + date[k]).substr(("" + date[k]).length));
                }
            }
            return format;
        }

        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: pharmacistevalsRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.pharmacists = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
            }
        }

    }
 

    //当前生活记录
    $scope.curUser = utility.getUserInfo();
    $scope.currentItem = {
        EvaluateBy: $scope.curUser.EmpNo
    };
    //当前住民
    $scope.currentResident = {};

    $scope.buttonShow = false;

    //选中住民
    $scope.residentSelected = function (resident) {

        $scope.currentResident = resident;

        $scope.getPharmacistByNo(resident.FeeNo);//加载当前住民的管路记录列表

        $scope.currentItem = {};//清空编辑项
        if (angular.isDefined($scope.currentResident)) {
            $scope.buttonShow = true;
        }
        $scope.currentItem.FeeNo = resident.FeeNo;
        $scope.curUser = utility.getUserInfo();
        $scope.currentItem.EvaluateBy = $scope.curUser.EmpNo;
        $scope.currentItem.EvalDate = new Date().format("yyyy-MM-dd");
    }

    //获取住民药师评估记录
    $scope.getPharmacistByNo = function (feeNo) {

        $scope.Data.pharmacists = {};

        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.params.feeNo = feeNo;
        $scope.options.search();

        //pharmacistevalsRes.get({ feeNo: feeNo, currentPage: $scope.currentPage, pageSize: 5 }, function (data) {
        //    $scope.Data.pharmacists = data.Data;

        //    var pager = new Pager('pager', $scope.currentPage, data.PagesCount, function (curPage) {

        //        $scope.currentPage = curPage;
        //        $scope.getPharmacistByNo(feeNo);
        //    });
        //});


        //获取当前住民使用中的管路
        pipelinerecRes.get({ feeNo: feeNo, removed: false, currentPage: 1, pageSize: 10 }, function (data) {
            $scope.currentItem.PipleLineDesc = '';
            for (var n = 0; n < data.Data.length; n++) {
                switch (data.Data[n].PipelineName) {
                    case "001":
                        data.Data[n].PipelineName = "胃造管";
                        break;
                    case "002":
                        data.Data[n].PipelineName = "导尿管(膀胱)";
                        break;
                    case "003":
                        data.Data[n].PipelineName = "导尿管(尿道)";
                        break;
                    case "004":
                        data.Data[n].PipelineName = "气切管";
                        break;
                    case "005":
                        data.Data[n].PipelineName = "鼻胃管";
                        break;
                }
                if (data.Data[n].PipelineName != "undefined")
                    $scope.currentItem.PipleLineDesc += data.Data[n].PipelineName + ';';
            }
        });
        //获取当前住民近3个月的就医记录
        visitdocrecordsRes.get({ CurrentPage: 1, PageSize: 10, feeNo: feeNo }, function (data) {
            $scope.currentItem.M3visitRec = '';
            if (data.Data != null) {
                for (var n = 0; n < data.Data.length; n++) {
                    var vdate = new Date(data.Data[n].VisitDate).format("yyyy-MM-dd");
                    $scope.currentItem.M3visitRec += vdate + "=>>" + (data.Data[n].VisitType==null?"未用药":data.Data[n].VisitType) + ";";
                }
            }
        });
        //获取当前住民的用药记录
        MedicationRecordRes.get({ CurrentPage: 1, PageSize: 10, feeNo: feeNo, StartDate: '', EndDate: '' }, function (data) {
            $scope.currentItem.DrugRecords = '';
            if (data.Data != null) {
                for (var n = 0; n < data.Data.length; n++) {
                    var vdate = new Date(data.Data[n].StartDate).format("yyyy-MM-dd");
                    var edate = new Date(data.Data[n].EndDate).format("yyyy-MM-dd");
                    $scope.currentItem.DrugRecords += "药名：" + data.Data[n].EngName + "  服务周期:" + vdate + "~" + edate + ";  ";
                }
            }
        });
    }
    //  保存/更新多重用药
    $scope.savePharmacist = function (item) {

        if (!angular.isDefined($scope.currentItem.EvaluateBy)) {
            utility.msgwarning("填写人员为必填项！");
            return;
        }
        else {
            if ($scope.currentItem.RecordBy == "") {
                utility.msgwarning("填写人员为必填项！");
                return;
            }
        }

        if (angular.isDefined($scope.pharmacistForm.$error.required)) {
            for (var i = 0; i < $scope.pharmacistForm.$error.required.length; i++) {
                utility.msgwarning($scope.pharmacistForm.$error.required[i].$name + "为必填项！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if (angular.isDefined($scope.pharmacistForm.$error.maxlength)) {
            for (var i = 0; i < $scope.pharmacistForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.pharmacistForm.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        item.FeeNo = $scope.currentResident.FeeNo;
        pharmacistevalsRes.save(item, function (data) {
            if (angular.isDefined(item.Id)) {
                utility.message("资料更新成功！");
            }
            else {
                utility.message("资料保存成功！");
                $scope.getPharmacistByNo(item.FeeNo);
            }
        });
        $scope.currentItem = {};
        $scope.getPharmacistByNo($scope.currentResident.FeeNo);
    }

    $scope.deletePharmacist = function (item) {
        if (confirm("您确定要删除该条记录吗?")) {
            pharmacistevalsRes.delete({ id: item.Id }, function (data) {
                if (data.ResultCode == 0)
                {
                    utility.message("资料删除成功！");

                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                }
                    //$scope.Data.pharmacists.splice($scope.Data.pharmacists.indexOf(item), 1);
            });
        }
    };
    $scope.rowSelect = function (item) {
        if (item.NextEvalDate != null) {
            $scope.days = DateDiff(item.EvalDate.substring(0, 10), item.NextEvalDate.substring(0, 10));
            item.Interval = DateDiff(item.NextEvalDate.substring(0, 10), item.EvalDate.substring(0, 10));
        }

        $scope.currentItem = item;

    };
    $scope.staffSelected = function (item) {
        $scope.currentItem.EvaluateBy = item.EmpNo;
        $scope.currentItem.EvaluateByName = item.EmpName;
    }
    $scope.staffSelected1 = function (item) {

        $scope.currentItem.NextEvaluateBy = item.EmpNo;
        $scope.currentItem.NextEvaluateByName = item.EmpName;
    }
    $scope.setNextValDate = function (gap) {
        if (angular.isDefined($scope.currentItem.EvalDate)) {
            var currentDate = $scope.currentItem.EvalDate;
            currentDate = currentDate.substring(0, 10);
            $scope.currentItem.NextEvalDate = GetNextEvalDate(currentDate, gap);
        }
    };
    $scope.init();

}]);

