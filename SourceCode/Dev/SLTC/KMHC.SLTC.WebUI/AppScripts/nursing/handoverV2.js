/*****************************************************************************
 * Filename: handoverV2
 * Creator:	Lei Chen
 * Create Date: 2017-02-10
 * Modifier:
 * Modify Date:
 * Description:锦欣行政交班
 ******************************************************************************/
angular.module("sltcApp")
.controller("handoverV2Ctrl", ['$scope','$filter', '$compile', 'utility', 'workItemRes', 'handoverRecordRes', function ($scope,$filter, $compile, utility, workItemRes, handoverRecordRes) {
    $scope.init = function () {
        $scope.DtlEdit = false;
        $scope.SearchDate = $filter("date")(new Date(), "yyyy-MM-dd");
        $scope.Search();
    }

    $scope.Search = function () {
        handoverRecordRes.get({ date: $scope.SearchDate }, function (data) {
            $scope.Data = data.Data;
        });
    }

    $scope.HideDialog = function () {
        $scope.dialog.close();
    }

    $scope.AddHandoverRecord = function (itemCode) {
        var html = '<div km-include km-template="Views/NurseStation/HandoverV2Edit.html" km-controller="handoverV2EditCtrl"  km-include-params="{itemCode:\'' + itemCode + '\'}" ></div>';
        $scope.dialog = BootstrapDialog.show({
            title: '<h4>行政交班</h4>',
            type: BootstrapDialog.TYPE_DEFAULT,
            message: html,
            size: BootstrapDialog.SIZE_WIDE,
            onshow: function (dialog) {
                var obj = dialog.getModalBody().contents();
                $compile(obj)($scope);
            }
        });
    }

    $scope.AddWorkItem = function (item) {
        if (!$scope.DtlEdit) {
            $scope.Data.LTC_HandoverDtl.push(item);
        }
        $scope.DtlEdit = false;
    }

    $scope.EditHandoverDtl = function (item) {
        $scope.DtlEdit = true;
        $scope.CurrentEditDtl = item;
        $scope.AddHandoverRecord(item.ItemCode);
    }


    $scope.init();
}])
.controller("handoverV2EditCtrl", ['$scope', 'utility', 'workItemRes', 'handoverRecordRes', function ($scope, utility, workItemRes, handoverRecordRes) {
    $scope.init = function () {
        $scope.Data = {};
        if ($scope.$parent.DtlEdit) {
            $scope.Data = $scope.$parent.CurrentEditDtl;
        } else {
           
            $scope.Data.RecordId = $scope.$parent.Data.Id;
            $scope.Data.ItemCode = $scope.kmIncludeParams.itemCode;
            $scope.Data.RecordBy = currentUser.EmpNo;
        }
        $scope.Data.RecordDate = $scope.$parent.SearchDate;
        workItemRes.get({ itemType: 'H' }, function (data) {
            $scope.WorkItems = data.Data;
        })
    }

    $scope.Save = function () {
        if (!utility.Validation($scope.myhandoverEdit.$error)) {
            return;
        }
        handoverRecordRes.SaveHandoverRecord($scope.Data, function (data) {
            $scope.$parent.AddWorkItem(data.Data);
        })

        $scope.$parent.HideDialog();
    };

    $scope.init();
}])