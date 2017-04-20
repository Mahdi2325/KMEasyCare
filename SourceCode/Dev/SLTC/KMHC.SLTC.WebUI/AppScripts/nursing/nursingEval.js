angular.module("sltcApp").controller("NursingEvalCtrl", ['$scope', '$http', '$filter', 'NursingCareEvalRes', '$location', '$state', 'dictionary', 'utility',
    function ($scope, $http, $filter, NursingCareEvalRes, $location, $state, dictionary, utility) {
        $scope.FeeNo = -1;
        $scope.QuesList = [];
        $scope.EvalRecord = {};
        $scope.QuesList.question = {};
        $scope.currentResident = {};
        $scope.totalScore = 0;
        $scope.evaluateid = -1;
        $scope.LoadInfo = function () {

            NursingCareEvalRes.get({ Code: "VL", feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo, evaluateid: $scope.evaluateid }, function (response) {
                $scope.QuesList = response.Data;
                if ($scope.QuesList.Starttime == null) {
                    $scope.QuesList.Starttime = $filter("date")(new Date(), "yyyy-MM-dd HH:mm:ss");
                }

                $scope.QuesList.question = response.Data.nursingEval;
                if (response.Data == null && response.ResultMessage != '') {
                    utility.message(response.ResultMessage);
                    return;
                }
            });
        };

        $scope.init = function () {
            $scope.LoadInfo();
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: NursingCareEvalRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.EvalRecord = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo,
                }
            }
        };

        $scope.loadoption = function () {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.params.feeNo = $scope.FeeNo == "" ? -1 : $scope.FeeNo;
            $scope.options.search();
        }

        //选中住民
        $scope.residentSelected = function (resident) {
            $scope.evaluateid = -1;
            $scope.currentResident = resident;//获取当前住民信息
            $scope.FeeNo = resident.FeeNo;
            $scope.LoadInfo();
            $scope.loadoption();
        }

        $scope.Save = function () {
            if ($scope.FeeNo == "" || $scope.FeeNo == -1) {
                utility.msgwarning("请先选择住民！");
                return;
            }

            NursingCareEvalRes.save($scope.QuesList, function (data) {
                utility.message($scope.QuesList.Name + "的护理长期护理保险评价保存成功！");
                $scope.evaluateid = -1;
                $scope.LoadInfo();
                $scope.loadoption();
            });
        };

        $scope.Edit = function (item) {
            $scope.evaluateid = item.NCIEvaluateid;
            $scope.LoadInfo();
        }

        $scope.Delete = function (Item) {
            if (confirm("确定删除该评估记录吗?")) {
                NursingCareEvalRes.delete({ evalId: Item.NCIEvaluateid }, function (data) {
                    if (data.ResultCode == 0) {
                        utility.message("删除成功！");
                        $scope.evaluateid = -1;
                        $scope.LoadInfo();
                        $scope.loadoption();
                    }
                });
            }
        }


        $scope.Print = function (item) {
            if (angular.isDefined(item.NCIEvaluateid)) {
                if (item.NCIEvaluateid == 0) {
                    utility.message("无打印数据！");
                    return;
                }
                window.open("/Report/Preview?templateName={0}&key={1}&startDate={2}&endDate={3}".format("CareEvalReport", item.NCIEvaluateid, "", ""), "_blank");
            } else {
                utility.message("无打印数据！");
            }
        };

        $scope.init();
    }
]);
