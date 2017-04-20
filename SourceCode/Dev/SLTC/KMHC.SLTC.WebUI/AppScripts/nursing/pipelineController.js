/*
 * Author : Dennis yang(杨金高)
 * Date   : 2016-03-16
 * Desc   : PipelineRec(管路)
 */
angular.module("sltcApp")
.controller("pipelineCtrl", ['$scope', '$state', 'dictionary', 'utility', 'pipelinerecRes', 'pipelineevalRes',
function ($scope, $state, dictionary, utility, pipelinerecRes, pipelineevalRes) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.Data = {};
    $scope.currentItem = null;
    $scope.RemovedFlag = false;
    //当前住民管路对象
    $scope.currentPipeline = {};

    $scope.curUser = utility.getUserInfo();
    if (typeof ($scope.curUser) != 'undefined') {
        $scope.currentPipelineEval = { Operator: $scope.curUser.EmpNo };
    }
    //当前住民
    $scope.currentResident = {};
    //部分控件是否可见
    $scope.buttonShow = false;
    $scope.button1Show = false;
    $scope.btnEnable1 = false;
    $scope.btnEnable2 = false;
    $scope.btnEnable3 = false;
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
    $scope.init = function () {

        $scope.EvalDateGap = [{ ITEMNAME: "7", ITEMCODE: 7 }, { ITEMNAME: "14", ITEMCODE: 14 }, { ITEMNAME: "28", ITEMCODE: 28 }, { ITEMNAME: "30", ITEMCODE: 30 }, { ITEMNAME: "60", ITEMCODE: 60 }, { ITEMNAME: "90", ITEMCODE: 90 }];

        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: pipelinerecRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.pipelineRecs = data.Data;
                for (var n = 0; n < data.Data.length; n++) {
                    switch (data.Data[n].PipelineName) {
                        case "001":
                            if (data.Data[n].RemovedFlag == false)//已经移除的可以再次添加此管路
                                $scope.btnEnable1 = true;
                            break;
                        case "002":
                            if (data.Data[n].RemovedFlag == false) {
                                $scope.btnEnable2 = true;
                                $scope.btnEnable3 = true;
                            }
                            break;
                        case "003":
                            if (data.Data[n].RemovedFlag == false) {
                                $scope.btnEnable2 = true;
                                $scope.btnEnable3 = true;
                            }
                            break;
                        case "004":
                            if (data.Data[n].RemovedFlag == false)
                                $scope.btnEnable4 = true;
                            break;
                        case "005":
                            if (data.Data[n].RemovedFlag == false)
                                $scope.btnEnable5 = true;
                            break;
                    }
                }
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
                removed: true
            }
        }

        $scope.detailOptions = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: pipelineevalRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.pipelineEvals = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                seqNo: -1
            }
        }


    }
    $scope.search = function () {
        $scope.getPipelineRecByNo($scope.currentResident.FeeNo);
    }

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.button1Show = false;
        $scope.currentResident = resident;
        $scope.currentResident.FeeNo = resident.FeeNo;
        $scope.currentPipeline.FeeNo = resident.FeeNo;
        // $scope.currentItem = {};//清空编辑项
        if (angular.isDefined($scope.currentResident)) {
            $scope.buttonShow = true;
        }

        $scope.currentPipeline.RecordDate = getNowFormatDate();
        $scope.Data.currentPipelineName = "";
        $scope.Data.pipelineEvals = {};

        $scope.getPipelineRecByNo(resident.FeeNo);//加载当前住民的管路记录列表
        $scope.btnEnable1 = false;
        $scope.btnEnable2 = false;
        $scope.btnEnable3 = false;
        $scope.btnEnable4 = false;
        $scope.btnEnable5 = false;
    };
    //管路选中显示下方明细数据
    $scope.pipelineSelect = function (item) {
        if (item.RemovedFlag != true) {
            $scope.getPipelineEvalByNo(item.SeqNo, item.PipelineName);
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.currentPipelineEval = { Operator: $scope.curUser.EmpNo };
            }
            $scope.currentPipelineEval.SeqNo = item.SeqNo;
            $scope.currentPipelineEval.FeeNo = item.FeeNo;
            var curDate = new Date();
            $scope.currentPipelineEval.EvalDate = getNowFormatDate();
            $scope.currentPipelineEval.RecentDate = getNowFormatDate();
            $scope.button1Show = true;
        }
        else {
            $scope.button1Show = false;
        }
    };
    function getNowFormatDate() {
        var date = new Date();
        var seperator1 = "-";
        var seperator2 = ":";
        var month = date.getMonth() + 1;
        var strDate = date.getDate();
        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strDate >= 0 && strDate <= 9) {
            strDate = "0" + strDate;
        }
        var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate;
        return currentdate;
    }
    //获取住民管路记录
    $scope.getPipelineRecByNo = function (feeNo) {
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.params.feeNo = feeNo;
        $scope.options.params.removed = $scope.RemovedFlag;
        $scope.options.search();
    }

    //根据住民regNo获取管路明细
    $scope.getPipelineEvalByNo = function (seqNo, pipelineName) {
        $scope.Data.pipelineEvals = {};

        $scope.detailOptions.pageInfo.CurrentPage = 1;
        $scope.detailOptions.params.seqNo = seqNo;
        $scope.detailOptions.search();
        $scope.Data.currentPipelineName = pipelineName;
    }





    //选择填写人员
    $scope.staffSelected = function (item) {

        $scope.currentPipelineEval.Operator = item.EmpNo;
        $scope.currentPipelineEval.OperatorName = item.EmpName;
    }
    //$scope.$watch('day', function (newValue) {

    //    if (newValue) {
    //        var d = new Date(Date.parse($scope.currentPipelineEval.RecentDate.replace(/-/g, "/")));

    //        d.setDate(d.getDate() + newValue.value * 1); //console.log($scope.currentPipelineEval.EvalDate);
    //        $scope.currentPipelineEval.NextDate = d.format("yyyy-MM-dd");
    //        $scope.currentPipelineEval.Interval = newValue.value;
    //    }
    //});

    $scope.setNextValDate = function (gap) {
        var currentDate = $scope.currentPipelineEval.RecentDate;
        currentDate = currentDate.substring(0, 10);
        $scope.currentPipelineEval.NextDate = GetNextEvalDate(currentDate, gap);
        $scope.currentPipelineEval.IntervalDay = gap;
    };

    $scope.rowSelect = function (item) {
        $scope.currentPipelineEval = item;

    }

    //删除管路更换记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的管路更换记录吗?")) {
            pipelineevalRes.delete({ id: item.Id }, function () {
                $scope.detailOptions.pageInfo.CurrentPage = 1;
                $scope.detailOptions.search();
                //$scope.Data.pipelineEvals.splice($scope.Data.pipelineEvals.indexOf(item), 1);
                utility.message($scope.currentResident.Name + "的管路更换记录信息删除成功！");
            });
        }
    };

    //使用管路事件
    $scope.usePipeline = function (obj) {
        switch (obj) {
            case 1:
                $scope.currentPipeline.PipelineName = "001";
                $scope.btnEnable1 = true;
                break;
            case 2:
                $scope.currentPipeline.PipelineName = "002";
                $scope.btnEnable2 = true;
                $scope.btnEnable3 = true;
                break;
            case 3:
                $scope.currentPipeline.PipelineName = "003";
                $scope.btnEnable2 = true;
                $scope.btnEnable3 = true;
                break;
            case 4:
                $scope.currentPipeline.PipelineName = "004";
                $scope.btnEnable4 = true;
                break;
            case 5:
                $scope.currentPipeline.PipelineName = "005";
                $scope.btnEnable5 = true;
                break;
        }
        $scope.createpipelineRec($scope.currentPipeline);
    }
    //添加一条新管路
    $scope.createpipelineRec = function (item) {

        if (confirm("请先确定使用此管路是否具有移除的可能性，是请点击确定，否则点击取消!")) {
            $scope.currentPipeline.RemoveFlag = true;
        } else {
            $scope.currentPipeline.RemoveFlag = false;
        }
        $scope.currentPipeline.RecentDate = "";
        $scope.currentPipeline.RemoveReason = "";
        $scope.currentPipeline.RemoveTrain = "";
        $scope.currentPipeline.RemoveDate = "";
        $scope.currentPipeline.RecordDate = $scope.currentPipeline.RecordDate;
        $scope.currentPipeline.SeqNo = 0;
        $scope.currentPipeline.FeeNo = $scope.currentResident.FeeNo;
        $scope.currentPipeline.RemovedFlag = false;
        //添加管路时默认添加一条空的评估子表数据
        $scope.currentPipelineEval.RegNo = $scope.currentResident.RegNo;
        $scope.currentPipelineEval.EvalDate = $scope.currentPipeline.RecordDate;
        $scope.currentPipelineEval.RecentDate = getNowFormatDate();
        $scope.currentPipelineEval.IntervalDay = "";
        $scope.currentPipelineEval.NextDate = "";
        $scope.currentPipelineEval.State = "";
        $scope.currentPipelineEval.FeeNo = item.FeeNo;
        $scope.currentPipelineEval.PipelineName = $scope.currentPipeline.PipelineName;

        $scope.currentPipelineEval.Id = 0;
        pipelinerecRes.save($scope.currentPipeline, function (data) {
            if (data.ResultCode == 1001) {
                utility.message(data.ResultMessage);
            }

            //$scope.Data.pipelineRecs.push(data.Data);
            $scope.getPipelineRecByNo($scope.currentResident.FeeNo);
            if (data.Data.SeqNo != '') {

                $scope.currentPipelineEval.SeqNo = data.Data.SeqNo;

                //pipelineevalRes.save($scope.currentPipelineEval, function (data) {

                //$scope.Data.pipelineEvals.push(data);
                //});
                $scope.getPipelineEvalByNo(data.Data.SeqNo, $scope.currentPipeline.PipelineName);
                $scope.button1Show = true;
            }

            if ($scope.currentPipeline.PipelineName == "001")
                $scope.Data.pName = "胃造管";
            if ($scope.currentPipeline.PipelineName == "002")
                $scope.Data.pName = "导尿管(膀胱)";
            if ($scope.currentPipeline.PipelineName == "003")
                $scope.Data.pName = "导尿管(尿道)";
            if ($scope.currentPipeline.PipelineName == "004")
                $scope.Data.pName = "气切管";
            if ($scope.currentPipeline.PipelineName == "005")
                $scope.Data.pName = "鼻胃管";
            utility.message($scope.currentResident.Name + "使用了一条 '" + $scope.Data.pName + "' 管路！");

            $scope.currentPipeline = {};
            $scope.currentPipeline.RecordDate = getNowFormatDate();
        });
    };
    //移除管路
    $scope.removePipelineRec = function (item) {
        $scope.currentPipeline = item ? item : {};
        $scope.currentPipeline.RemoveDate = getNowFormatDate();
        $("#modalRemovePipeline").modal("toggle");
    };
    //保存管路移除信息
    $scope.saveRemoveInfo = function (item) {
        $scope.currentPipeline.RemovedFlag = true;
        $scope.currentPipeline.RemoveFlag = true;
        $scope.currentPipeline.RemoveDate = $scope.currentPipeline.RemoveDate

        if (angular.isDefined(item.SeqNo)) {
            pipelinerecRes.save(item, function (data) {
                if (data.ResultCode == 0) {
                    $("#modalRemovePipeline").modal("toggle");
                    $scope.resetUse(item.PipelineName);//重置可设置动作
                    $scope.search();
                    utility.message($scope.currentResident.Name + "移除了一条" + $scope.currentPipeline.PipelineName + "管路！");
                    $scope.currentPipeline.RecordDate = getNowFormatDate();
                }
                else
                    utility.message($scope.currentResident.Name + "移除" + $scope.currentPipeline.PipelineName + "管路失败，请联系管理员！");
                $scope.currentPipeline.RecordDate = getNowFormatDate();
            });
        }
    }

    $scope.deletePipelineRec = function (item) {

        if (confirm("请确定删除此条管路信息？")) {
            if (angular.isDefined(item.SeqNo)) {
                pipelineevalRes.get({ seqNo: item.SeqNo, CurrentPage: 1, PageSize: 10 }, function (data) {
                    if (angular.isDefined(data.Data)) {
                        if (data.Data.length > 0) {
                            //  拥有记录 ，-- 需要移除
                            utility.message("当前管路拥有管路更换与评估记录，请点击移除！");
                            return;
                        }
                        else {
                            //  未拥有记录 ， -- 可删除
                            pipelinerecRes.delete({ id: item.SeqNo }, function (date) {
                                if (data.ResultCode == 0) {
                                    $scope.search();
                                    $scope.resetUse(item.PipelineName);//重置可设置动作
                                    $scope.currentPipeline.RecordDate = getNowFormatDate();
                                    $scope.currentPipelineEval = {};
                                    $scope.Data.currentPipelineName = "";
                                    utility.message($scope.currentResident.Name + "成功删除了" + $scope.getPipelineName(item.PipelineName) + "管路信息！");
                                }
                                else {
                                    utility.message($scope.currentResident.Name + "删除" + $scope.getPipelineName(item.PipelineName) + "管路失败，请联系管理员！");
                                }
                            });
                        }
                    }
                });
            }
        }
    }

    $scope.getPipelineName = function (code) {
        var PipelineName = "";
        if (code == "001")
            PipelineName = "胃造管";
        if (code == "002")
            PipelineName = "导尿管(膀胱)";
        if (code == "003")
            PipelineName = "导尿管(尿道)";
        if (code == "004")
            PipelineName = "气切管";
        if (code == "005")
            PipelineName = "鼻胃管";
        return PipelineName;
    };


    //如果移除某条管路，那麽需要将此管路设置为可以再次添加
    $scope.resetUse = function (code) {
        if (code == "001") $scope.btnEnable1 = false;
        if (code == "002") $scope.btnEnable2 = false; $scope.btnEnable3 = false;
        if (code == "003") $scope.btnEnable3 = false; $scope.btnEnable2 = false;
        if (code == "004") $scope.btnEnable4 = false;
        if (code == "005") $scope.btnEnable5 = false;
    }
    //更新管路移除记录
    $scope.updatePipelineRec = function (item) {
        item.$save();
        $scope.currentPipeline = {};
        utility.message($scope.currentResident.Name + " 的 '" + item.PipelineName + " '移除成功！");
        $("#modalRemovePipeline").modal("toggle");
    };

    //窗口关闭操作
    $scope.cancelEdit = function () {
        if ($scope.currentPipeline && $scope.currentPipeline.$get) {
            $scope.currentPipeline.$get();
        }

        $scope.currentPipeline.RemoveTrain = '';
        $scope.currentPipeline.RemoveReason = '';
        $("#modalRemovePipeline").modal("toggle");
    };

    //添加新的管路评估记录
    $scope.savePipelineEval = function (item) {
        item.FeeNo = $scope.currentResident.FeeNo;
        $scope.currentPipelineEval.PipelineName = $scope.Data.currentPipelineName;
        pipelineevalRes.save(item, function (data) {

            if (angular.isDefined(item.Id) && item.Id != 0) {
                utility.message("资料更新成功！");
            }
            else {
                $scope.detailOptions.pageInfo.CurrentPage = 1;
                $scope.detailOptions.search();
                utility.message("资料保存成功！");
            }
        });
        $scope.resetPipelineEval(item);
    }

    $scope.resetPipelineEval = function (currentPipelineEval) {

        $scope.currentPipelineEval = {};
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentPipelineEval = { Operator: $scope.curUser.EmpNo };
        }
        $scope.currentPipelineEval.FeeNo = currentPipelineEval.FeeNo; $scope.currentPipelineEval.SeqNo = currentPipelineEval.SeqNo;
        $scope.currentPipelineEval.EvalDate = getNowFormatDate();
        $scope.currentPipelineEval.RecentDate = getNowFormatDate();
    }
    $scope.init();

}]);


