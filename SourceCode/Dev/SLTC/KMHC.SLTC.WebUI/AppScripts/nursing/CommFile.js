
angular.module("sltcApp").controller("CommfilelistsCtr", ['$scope', '$http', '$location', '$state', 'dictionary', 'Commfilelists', 'utility',
    function ($scope, $http, $location, $state, dictionary, Commfilelists, utility) {

        //首次加载页面执行的方法
        $scope.init = function () {

            $scope.CurrentPage = 1;

            $scope.Name = "";

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: Commfilelists,//异步请求的res
                success: function (object) {//请求成功时执行函数

                    $scope.item = object.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                },
                params: {
                    // 姓名
                    Name: $scope.Name
                }
            }

        };

        //这边是查询的方法
        $scope.search = function () {

            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.params.Name = $scope.Name;
            $scope.options.search();
        };

        $scope.delete = function (id) {

            if (confirm("确定删除该信息吗?")) {
                Commfilelists.delete({ id: id }, function (data) {

                    utility.message("删除成功");
                    $scope.search();
                });
            }
        }
        $scope.init();
    }
]);
//这边是添加的房子的东西
angular.module("sltcApp").controller("COMMFILEAddCtrl", ['$scope', '$http', '$location', '$state', 'dictionary', 'Commfilelists', 'utility',
    function ($scope, $http, $location, $state, dictionary, Commfilelists, utility) {

        $scope.tbEdit = "ui-inline-hide";
        $scope.classEdit = "ui-inline-hide";
        $scope.tt = [];
        $scope.btn_cc = false;

        $scope.init = function () {

        };
        $scope.SaveItem = function (CheckRecdtl) {
            if (angular.isDefined(CheckRecdtl)) {
                var par = {
                    //TYPECODE: CheckRecdtl.TYPECODE,
                    TYPENAME: $scope.TYPENAME,
                    ITEMNAME: CheckRecdtl.ITEMNAME
                }

                $scope.tt.push(par);

                $scope.Commfile = null;
            }
            else {
                utility.message("请输入项目名称");
            }
        }
        $scope.insertCommfile = function () {
            var par = $scope.tt;
            if (par.length > 0) {
                Commfilelists.save(par, function (obj) {
                    // 这边是删除的东西
                    //这边的id是取到的
                    //items.RECORDID = bj.ResultCode;
                    utility.message("操作成功");

                    $location.url("/angular/Commfilelists");
                    //这边方法是多余的。
                });
            }
            else {

                utility.message("请点击 + 确认添加");

            }
        }
        $scope.DeleteItem = function (CheckRecdtl) {
            $scope.tt.splice($scope.tt.indexOf(CheckRecdtl), 1);

        }

        $scope.init();
    }
]);

//常用语编辑

angular.module("sltcApp").controller("COMMFILEeditCtrl", ['$scope', '$http', '$location', '$state', '$window', 'dictionary', 'Commfilelists', 'utility',
    function ($scope, $http, $location, $state,$window, dictionary, Commfilelists, utility) {


        $scope.tbEdit = "ui-inline-hide";
        $scope.classEdit = "ui-inline-hide";

        //加载事件
        var id = $state.params.id;

        $scope.init = function () {
            //调用函
            Commfilelists.get({ id: id }, function (obj) {


                if (obj.Data.length > 0) {
                    $scope.xx = [];

                    $scope.Commitem = obj.Data;

                    $scope.TYPENAME = $scope.Commitem[0].TYPENAME;

                    for (var t = 0; t < $scope.Commitem.length; t++) {

                        var cd = {

                            TYPECODE: $scope.Commitem[t].TYPECODE, ITEMNAME: $scope.Commitem[t].ITEMNAME, TYPENAME: $scope.TYPENAME, ID: $scope.Commitem[t].ID,

                        };

                        $scope.xx.push(cd);
                    }
                }
                else
                {
                    $scope.xx = [];
                    $scope.TYPENAME = id;
                }
            });

            $scope.insertCommfile = function () {

                var par = $scope.xx;
                Commfilelists.save(par, function (obj) {
                    // 这边是删除的东西
                    //这边的id是取到的
                    //items.RECORDID = bj.ResultCode;
                    utility.message("操作成功");

                   // $location.url("/angular/Commfilelists");
                    //这边方法是多余的。
                });

            }
            $scope.deleteCommfile = function () {

                var par = $scope.xx;
                Commfilelists.mulSave(par, function (obj) {
                    // 这边是删除的东西
                    //这边的id是取到的
                    //items.RECORDID = bj.ResultCode;
                    utility.message("操作成功");

                    $location.url("/angular/Commfilelists");
                    //这边方法是多余的。
                });

            }
            $scope.cancelCommfile = function () {
                $window.history.back();
            }
            $scope.SaveItem = function (CheckRecdtl) {
                if (angular.isDefined(CheckRecdtl)) {
                    if (CheckRecdtl.ITEMNAME.length > 100) {
                        utility.message("项目名称长度过长！");
                        return;
                    }

                    var par = {
                        //TYPECODE: CheckRecdtl.TYPECODE,
                        TYPENAME: $scope.TYPENAME,
                        ITEMNAME: CheckRecdtl.ITEMNAME
                    }

                    $scope.xx.push(par);

                    $scope.Commfile = null;
                } else {
                    utility.message("请输入项目名称");
                }
            }
            //这边是删除的工单
            $scope.DeleteItem = function (CheckRecdtl) {
                //这边有的删除，没有
                if (CheckRecdtl.ID > 0) {

                    Commfilelists.delete({ id: CheckRecdtl.ID }, function (data) {

                        $scope.xx.splice($scope.xx.indexOf(CheckRecdtl), 1);
                    });

                }
                else {

                    $scope.xx.splice($scope.xx.indexOf(CheckRecdtl), 1);
                }
            }
            $scope.SaveAll = function (currentItem) {
                $scope.tbEdit = "ui-inline-hide";
                $scope.tbCancle = "ui-inline-show";
                $scope.classEdit = "ui-inline-hide";
                $scope.classEditAll = "ui-inline-show";

            }

            $scope.EditAll = function () {

                $scope.tbEdit = "ui-inline-show";
                $scope.tbCancle = "ui-inline-hide";
                $scope.classEdit = "ui-inline-show";
                $scope.classEditAll = "ui-inline-hide";
            }
            $scope.CancleEdit = function () {

                $scope.tbEdit = "ui-inline-hide";
                $scope.tbCancle = "ui-inline-show";
                $scope.classEdit = "ui-inline-hide";
                $scope.classEditAll = "ui-inline-show";
            }

        };
        // 事件
        $scope.init();
    }
]);

