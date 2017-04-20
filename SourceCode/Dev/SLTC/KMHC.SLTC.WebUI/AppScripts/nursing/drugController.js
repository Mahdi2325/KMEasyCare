/*
 * Author : Dennis yang(杨金高)
 * Date   : 2016-03-03
 * Desc   : DrugList(药品管理)
 */


/*
修改人:张祥
修改日期:2016-04-06
说明: 药品管理
*/

/*
修改人:Bob Du
修改日期:2016-08-29
说明: 药品管理 修改了上传组件
*/
angular.module("sltcApp")
.controller("drugCtrl", ['$scope', 'webUploader', 'medicineRes', 'utility', '$upload', function ($scope, webUploader, medicineRes, utility, $upload) {

        $scope.Data = {};
        //当前住民数据对象
        $scope.currentItem = {};
        $scope.displayMode = "list";
        //页面初始化函数
        $scope.init = function () {
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: medicineRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.medicines = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    keyWord: ""
                }
            }
        }
        $scope.filesave = function (files) {
            $upload.upload({
                url: '/api/NgUpload',
                data: 'MedicinePhoto',
                file: files
            }).progress(function (evt) {
            }).success(function (data) {
                if (data.length > 0) {
                    $scope.currentItem.MedPict = data[0].SavedLocation;
                    $scope.currentItem.MedPictName = data[0].FileName;
                    
                }
            }).error(function (msg) {

            });
        }
        var firstLoader = false;
        $('#drugModal').on('shown.bs.modal', function (e) {
            if (!firstLoader) {
                //图片处理
                webUploader.init('#medPicPicker', { category: 'MedicinePhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
                    if (data.length > 0) {
                        $scope.currentItem.MedPict = data[0].SavedLocation;
                        $scope.currentItem.MedPictName = data[0].FileName;
                        $scope.$apply();
                    }
                });
                firstLoader = true;
            }
        });
        $scope.createItem = function (item) {
            medicineRes.save($scope.currentItem, function (data) {
                $scope.Data.medicines.push(data);
                $scope.options.search();
                utility.message("存档成功!");
            });
            $scope.displayMode = "list";
        };

        //删除一条记录
        $scope.deleteItem = function (item) {
            if (confirm('您确定要删除该条记录吗?')) {
                medicineRes.delete({ id: item.Medid }, function (data) {
                    if (data.ResultCode == 12580) {
                        utility.message("该药品不能删除！");
                    }
                    else {
                        $scope.options.search();
                    }
                });
            }
            $scope.displayMode = "list";
        };


        //更新一条记录
        $scope.updateItem = function (item) {
            //item.$save();
            medicineRes.save(item, function (data) {
                $scope.options.search();
                utility.message("存档成功!");
            });
            $scope.displayMode = "list";
        };
        //弹出添加或编辑窗体
        $scope.openWin = function (item) {
            $scope.currentItem = item ? item : {};
            $("#drugModal").modal("toggle");
            $scope.displayMode = "edit";
        };
        //关闭弹出窗体
        $scope.closeWin = function () {
            $scope.currentItem = {};
            $scope.options.search();
            $("#drugModal").modal("toggle");
            $scope.displayMode = "list";
        };
        //提交表单数据
        $scope.submit = function (item) {
            if (angular.isDefined($scope.DrugForm.$error.required)) {
                for (var i = 0; i < $scope.DrugForm.$error.required.length; i++) {
                    utility.msgwarning($scope.DrugForm.$error.required[i].$name + "为必填项！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.DrugForm.$error.maxlength)) {
                for (var i = 0; i < $scope.DrugForm.$error.maxlength.length; i++) {
                    utility.msgwarning($scope.DrugForm.$error.maxlength[i].$name + "超过设定长度！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }
            if (angular.isDefined($scope.DrugForm.$error.pattern)) {
                for (var i = 0; i < $scope.DrugForm.$error.pattern.length; i++) {
                    utility.msgwarning($scope.DrugForm.$error.pattern[i].$name + "格式错误！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined(item.Medid)) {
                $scope.updateItem(item);
            } else {
                $scope.createItem(item);
            }
            $("#drugModal").modal("toggle");
        };
        //加载页面字典数据
        $scope.clear = function () {

            $scope.currentItem.MedPict = "";
            $scope.currentItem.MedPictName = "";
        }
        $scope.init();

        $scope.search=function()
        {
            $scope.options.search();
        }
    }]);


