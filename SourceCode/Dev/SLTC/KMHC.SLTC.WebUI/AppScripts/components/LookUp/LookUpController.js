angular.module("extentComponent")
.controller("LookUpController", ['$scope', '$injector', function ($scope, $injector) {
    var _vm = $scope.vm = {};
    var params = $scope.kmIncludeParams;
    _vm.colList = params.colList;
    var factoryName = $injector.get(params.factoryName);
    _vm.searchParams = params.searchParams;
    var searchCode = _vm.searchParams.Code;
    var searchOpeionsStatus = {};
    if (params.searchOptionsParams==undefined) {
        searchOpeionsStatus = {};
    } else {
        _vm.searchOptionsParams = params.searchOptionsParams;
        searchOpeionsStatus = _vm.searchOptionsParams;
    };
    _vm.isCanChoose = params.isCanChoose;

    _vm.selectedRows = [];
    _vm.itemProperty = params.itemProperty;
    //if(null)
    _vm.selectType = params.selectType;
    $scope.options = {
        buttons: [],//需要打印按钮时设置
        ajaxObject: factoryName,//异步请求的res
        params: searchOpeionsStatus,
        success: function (data) {//请求成功时执行函数
            _vm.data = data.Data;
        },
        pageInfo: {//分页信息
            CurrentPage: 1, PageSize: 10
        }
    }
   
    $scope.select = function (item) {
        if (_vm.selectType == 'single') {
            $scope.result = { id: item[_vm.itemProperty.key], name: item[_vm.itemProperty.name], data: item };
            $scope.$emit("km-on-lookup-confirm-click", $scope.result);
            $scope.$emit("km-on-lookup-dialog-close-click");
        } else {
            _vm.selectedRows.push({ key: item[_vm.itemProperty.key], name: item[_vm.itemProperty.name], data: item });
            _vm.selectedRows = _.uniq(_vm.selectedRows, "key");
        }
       
    }
    $scope.remove = function (item) {
        _.remove(_vm.selectedRows, function (r) {
            return r.key == item.key;
        });
    }
    $scope.confirm = function () {
        $scope.result = _vm.selectedRows;
        $scope.$emit("km-on-lookup-confirm-click", $scope.result);
        $scope.$emit("km-on-lookup-dialog-close-click");
    }
    $scope.searchInfo = function () {
        $scope.options.params[searchCode] = _vm.searchText;
        $scope.options.search();
    };
    Array.prototype.contains = function (obj) {  
        var i = this.length;  
        while (i--) {  
            if (this[i] === obj) {  
                return true;  
            }  
        }  
        return false;  
    }  
    $scope.vm.ngIfCondition = function (row) {
        var ngIfResult = false;
        var ngIfConArr = [];
        _.forEach(params.isCanChoose, function (name) {
            ngIfConArr.push(row[name.key] === name.value);
        });
        if (ngIfConArr.contains(true)) {
            ngIfResult = true;
        }
        return ngIfResult;
    }
}])