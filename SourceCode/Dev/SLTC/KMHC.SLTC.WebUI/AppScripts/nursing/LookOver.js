/*****************************************************************************
 * Filename: handoverV2
 * Creator:	Lei Chen
 * Create Date: 2017-02-10
 * Modifier:
 * Modify Date:
 * Description:锦欣行政交班
 ******************************************************************************/
angular.module("sltcApp")
.controller("LookOverCtrl", ['$scope', '$compile', 'utility', 'workItemRes', 'lookOverRes', 'floorRes', function ($scope, $compile, utility, workItemRes, lookOverRes, floorRes) {

    $scope.Data = {};

    $scope.Search = function () {
   
    }


    $scope.init = function () {
        $scope.options = {
            buttons: [],//需要打印按钮时设置
            ajaxObject: lookOverRes,//异步请求的res
            params: {  },
            success: function (data) {//请求成功时执行函数
                $scope.ItemList = data.Data;
            },
            pageInfo: {//分页信息
                CurrentPage: 1, PageSize: 10
            }
        }

        floorRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {
            $scope.Data.floors = data.Data;

        });
        workItemRes.get({ itemType: 'L' }, function (data) {
            $scope.WorkItems = data.Data;
        });
    }

    $scope.deleteItem = function (item) {
        utility.confirm("您确定删除该信息吗?", function (result) {
            if (result) {
                lookOverRes.delete({ id: item.ID }, function (data) {
                    if (data.IsSuccess) {
                        $scope.options.search();
                        utility.message("成功刪除！");
                    } else {
                        utility.message("删除失败！");
                    }
                });
            }
        });
    };
    $scope.init();
}])
.controller("LookOverEditCtrl", ['$scope', '$location', '$stateParams', 'utility', 'webUploader', 'floorRes', 'lookOverRes', function ($scope, $location, $stateParams, utility, webUploader, floorRes, lookOverRes) {
    $scope.init = function () {
        $scope.Data = {};

        floorRes.get({ CurrentPage: 1, PageSize: 100 }, function (data) {
            $scope.floors = data.Data;

        });

        if ($stateParams.id) {
            $scope.isAdd = false;
            lookOverRes.get({ ID: $stateParams.id }, function (data) {
                $scope.Data = data.Data;
                $scope.textCounter(500);
            });
        } else {
            $scope.Data.RecordBy = currentUser.EmpNo;
            $scope.isAdd = true;
        }
    }

    $scope.Save = function (item) {
        if (!utility.Validation($scope.LookOverEdit.$error)) {
            return;
        }
        item.ItemCode = "006";
        lookOverRes.save(item, function (newItem) {
            if (newItem.IsSuccess) {
                utility.message("记录保存成功");
                $location.url("/angular/LookOver");
            }
            else {
                utility.message(newItem.ResultMessage);
            }
        });
    };

    webUploader.init('#PicturePathPicker', { category: 'LookoverPhotos' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', FileUploadFinish, undefined, true, function (data) {
        
    });
    function FileUploadFinish(data) {
        if ($scope.Data.PhotoList == undefined || $scope.Data.PhotoList ==null) {
            $scope.Data.PhotoList = [];
        }
        if (data.length > 0) {
            $scope.Data.PhotoList.push(data[0].SavedLocation);
            $scope.Data.LookOverPhotos = $scope.Data.PhotoList.join(";");
            $scope.$apply();
        }
    }

    $scope.cancelEdit = function () {
        $location.url("/angular/LookOver");
    };

    $scope.remocePic = function(index){
        $scope.Data.PhotoList.splice(index, 1);
        $scope.Data.LookOverPhotos = $scope.Data.PhotoList.join(";");
    }

    $scope.textCounter = function (maxlimit) {
        // 函数，3个参数，表单名字，表单域元素名，限制字符；  
        if ($scope.Data.Content.length > maxlimit)
            //如果元素区字符数大于最大字符数，按照最大字符数截断；  
            $("#Content").html($scope.Data.Content.substring(0, maxlimit));
        else
            //在记数区文本框内显示剩余的字符数；  
            $("#remLen").html(maxlimit - $scope.Data.Content.length);
    }

    $scope.init();
}]).filter('cut', function () {
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