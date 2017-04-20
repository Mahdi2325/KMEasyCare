//居家服务督导
angular.module('sltcApp')
.controller("homeCareSuperviseCtrl",
    function ($scope, $http, $state, dictionary,utility, homeCareSuperviseRes) {
       
        $scope.Data = {};
        $scope.TWYear = null;
        $scope.TWMonth = null;

        //当前住民
        $scope.currentResident = {};
        $scope.buttonShow = false;

        $scope.init = function () {

            $scope.Data.Creator = [{ value: "张三", text: "张三" }, { value: "李四", text: "李四" }, { value: "王五", text: "王五" }, { value: "赵六", text: "赵六" }];

            $scope.Data.ContactType = [
                { value: '1', text: '电话访谈' },
                { value: '2', text: '家庭访谈' },
                { value: '3', text: '其他服务' }
            ];
            $scope.$watch('currentItem.SuperviseDate', function (newValue) {
                if (newValue) {
                    var d = new Date(Date.parse(newValue.replace(/-/g,"/")));
              
                    $scope.TWYear = parseInt(d.getFullYear() - 1911);
                    $scope.TWMonth = parseInt(d.getMonth()) + 1;
                }
            });
        }
        //选中住民
        $scope.residentSelected = function (resident) {

            $scope.currentResident = resident;

            $scope.gethomeCareSuperviseByNo(resident.FeeNo);//加载当前住民的生活记录列表

            $scope.currentItem = {};//清空编辑项
            if (angular.isDefined($scope.currentResident)) {
                $scope.buttonShow = true;
            }
            $scope.currentItem.RegNo = resident.RegNo;
            $scope.currentItem.FeeNo = resident.FeeNo;
        }
        //获取住民申诉记录
        $scope.gethomeCareSuperviseByNo = function (feeNo) {
            $scope.buttonShow = true;
            $scope.Data.homeCareSuperviseList = {};
            homeCareSuperviseRes.get({ feeNo: feeNo, currentPage: 1, pageSize: 10 }, function (data) {

                $scope.Data.homeCareSuperviseList = data.Data;
            });
        }

        $scope.savehomeCareSupervise = function (item) {
            homeCareSuperviseRes.save(item, function (data) {
     
                if (angular.isDefined(item.Id)) {
                    utility.message("资料更新成功！");
                }
                else {
                    $scope.Data.homeCareSuperviseList.push(data.Data);
                    utility.message("资料保存成功！");
                }
            });
            $scope.currentItem = {};
        };

        $scope.rowSelect = function (item) {
            $scope.currentItem = item;
        }

        $scope.deletehomeCareSupervise = function (item) {
            if (confirm("您确定要删除该条记录吗?")) {
  
                homeCareSuperviseRes.delete({ id: item.Id }, function (data) {
                    if (data.$resolved) {
                        var whatIndex = null;
                        angular.forEach($scope.Data.homeCareSuperviseList, function (cb, index) {
                            if (cb.id = item.Id) whatIndex = index;
                        });

                        if (data.ResultCode == 0)
                            utility.message("资料删除成功！");
                        $scope.Data.homeCareSuperviseList.splice(whatIndex, 1);

                    }
                });
            }
        }

        $scope.init();
    });

