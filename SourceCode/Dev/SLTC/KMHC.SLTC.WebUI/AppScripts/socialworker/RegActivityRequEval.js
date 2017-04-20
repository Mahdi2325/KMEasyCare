/*
创建人:张祥
创建日期:2016-06-22
说明: 个别化活动需求及计划
*/
angular.module('sltcApp').controller("RegActivityRequEvalCtrl", ['$scope', 'utility', 'RegActivityRequEvalRes', '$state', function ($scope, utility, RegActivityRequEvalRes, $state) {
    $scope.FeeNo = $state.params.FeeNo;

    $scope.Data = {};
    $scope.currentPage = 1;
    $scope.curUser = utility.getUserInfo();
    if (typeof ($scope.curUser) != 'undefined') {
        $scope.currentItem = { Carer: $scope.curUser.EmpNo };
    }
    // 当前住民
    $scope.currentResident = {};
    $scope.buttonShow = false;
    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: RegActivityRequEvalRes,//异步请求的res
            success: function (data) {//请求成功时执行函数
                $scope.Data.items = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            },
            params: {
                feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo
            }
        }
    }

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.currentResident = resident;//获取当前住民信息
        $scope.listItem($scope.currentResident.FeeNo);//加载当前住民的跌倒记录
        $scope.clear();//清空编辑项
        $scope.InDate = resident.InDate;
        if (angular.isDefined($scope.currentResident.FeeNo)) {
            $scope.buttonShow = true;
        }
    }

    $scope.staffSelected = function (item) {
        $scope.currentItem.Carer = item.EmpNo;
        $scope.currentItem.CarerName = item.EmpName;
    }

    $scope.listItem = function (FeeNo) {

        $scope.buttonShow = true;
        $scope.Data.items = {};
        $scope.options.pageInfo.CurrentPage = 1;
        $scope.options.params.feeNo = FeeNo;
        $scope.options.search();
        //RegActivityRequEvalRes.get({ feeNo: FeeNo, currentPage: $scope.currentPage, pageSize: 5 }, function (data) {

        //    $scope.Data.items = data.Data;
        //    var pager = new Pager('pager', $scope.currentPage, data.PagesCount, function (curPage) {

        //        $scope.currentPage = curPage;
        //        $scope.listItem(FeeNo);
        //    });
        //});
    }
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的评估记录吗?")) {
            RegActivityRequEvalRes.delete({ id: item.Id }, function () {
                $scope.options.pageInfo.CurrentPage = 1;
                $scope.options.search();
                //$scope.Data.items.splice($scope.Data.items.indexOf(item), 1);
            });
        }
    };
    $scope.PrintPreview = function (item) {
        window.open('/DC_Report/PreviewLTC_SocialReport?templateName=DLN1.1&id=' + item.Id + '&feeName=' + $scope.currentResident.Name);
    }
    $scope.saveActivityRequEvalEdit = function (item) {
        if ($scope.ActivityReauEvalForm.评估日期.$invalid) {
            utility.msgwarning("评估日期为必填项！");
            return;
        }
        if (!angular.isDefined($scope.currentItem.Carer)) {
            utility.msgwarning("社工为必填项！");
            return;
        }
        if (!checkDate($scope.InDate, $scope.currentItem.EvalDate)) {
            utility.msgwarning("评估日期不能在入住日期之前！");
            return;
        }
        if (angular.isDefined($scope.ActivityReauEvalForm.$error.maxlength)) {
            for (var i = 0; i < $scope.ActivityReauEvalForm.$error.maxlength.length; i++) {
                utility.msgwarning($scope.ActivityReauEvalForm.$error.maxlength[i].$name + "超过设定长度！");
                if (i > 1) {
                    return;
                }
            }
            return;
        }

        if ($scope.Directman == true) {
            item.Directman = 1
        }
        else {
            item.Directman = 0
        }

        if ($scope.Directtime == true) {
            item.Directtime = 1
        }
        else {
            item.Directtime = 0
        }
        if ($scope.Directaddress == true) {
            item.Directaddress = 1
        }
        else {
            item.Directaddress = 0
        }

        if (angular.isDefined(item.Id)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        utility.message($scope.currentResident.Name + "的评估信息保存成功！");
    };

    $scope.createItem = function (item) {
        $scope.currentItem.InDate = $scope.InDate;
        $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;//住院序号
        $scope.currentItem.RegNo = $scope.currentResident.RegNo;//病例号
        RegActivityRequEvalRes.save($scope.currentItem, function (data) {
            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.search();
            //$scope.Data.items.push(data);
        });
        $scope.clear();
    };

    $scope.rowSelect = function (item) {

        $scope.currentItem = item;
        if ($scope.currentItem.Talkednowilling == "True") {
            $scope.currentItem.Talkednowilling = true;
        }
        else if ($scope.currentItem.Talkednowilling == true) {

        }
        else {
            $scope.currentItem.Talkednowilling = false;
        }
        if ($scope.currentItem.Talkedwilling == "True") {
            $scope.currentItem.Talkedwilling = true;
        }
        else if ($scope.currentItem.Talkedwilling == true) {

        }
        else {
            $scope.currentItem.Talkedwilling = false;
        }

        if ($scope.currentItem.Nottalked == "True") {
            $scope.currentItem.Nottalked = true;
        }
        else if ($scope.currentItem.Nottalked == true) {

        }
        else {
            $scope.currentItem.Nottalked = false;
        }

        if (item.Directman == 1) {
            $scope.Directman = true;
        }
        else {
            $scope.Directman = false;
        }

        if (item.Directtime == 1) {
            $scope.Directtime = true;
        }
        else {
            $scope.Directtime = false;
        }

        if (item.Directaddress == 1) {
            $scope.Directaddress = true;
        }
        else {
            $scope.Directaddress = false;
        }

        if (item.Memoryflag == 1) {
            $scope.LongMemory = true;
            $scope.ShortMemory = false;
        }
        else if (item.Memoryflag == -1) {
            $scope.LongMemory = false;
            $scope.ShortMemory = true;
        }
        else {
            $scope.LongMemory = false;
            $scope.ShortMemory = false;
        }
    };
    $scope.checkTalkedwilling = function () {
        if ($scope.currentItem.Talkednowilling == true) {
            $scope.currentItem.Talkednowilling = false;
            $scope.currentItem.Talkedwilling = true;
        }
    }
    $scope.checkTalkednowilling = function () {
        if ($scope.currentItem.Talkedwilling == true) {
            $scope.currentItem.Talkednowilling = true;
            $scope.currentItem.Talkedwilling = false;
        }
    }

    $scope.checkLongMemory = function () {
        if ($scope.LongMemory == true) {
            $scope.ShortMemory = false;
            $scope.LongMemory = true;
            $scope.currentItem.Memoryflag = 1;
        }
    }
    $scope.checkShortMemory = function () {
        if ($scope.ShortMemory == true) {
            $scope.LongMemory = false;
            $scope.ShortMemory = true;
            $scope.currentItem.Memoryflag = -1;
        }
    }

    $scope.clear = function () {
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { Carer: $scope.curUser.EmpNo };
        }
        $scope.Directman = false;
        $scope.Directtime = false;
        $scope.Directaddress = false;
        $scope.LongMemory = false;
        $scope.ShortMemory = false;
    }

    $scope.updateItem = function (item) {
        $scope.currentItem.InDate = $scope.InDate;
        RegActivityRequEvalRes.save(item, function (data) {
            $scope.clear();
        });
    };
    $scope.init();

}]);
