/*
创建人: 张正泉
创建日期:2016-02-20
说明:楼层管理
*/

angular.module("sltcApp")
    .controller("floorListCtrl", ['$scope', '$http', '$location', '$state', 'dictionary', 'floorRes', 'utility', function($scope, $http, $location, $state, dictionary, floorRes, utility) {

        $scope.init = function() {
            $scope.Data = {};
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: floorRes,//异步请求的res
                params: { floorName: "" },
                success: function (data) {//请求成功时执行函数
                    $scope.Data.floors = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
            }
        };

        $scope.delete = function(id) {
            if (confirm("确定删除该楼层信息吗?")) {
                floorRes.delete({ id: id }, function (data) {
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                    utility.message(data.ResultMessage);
                });
            }
        };

        $scope.init();
    }])
    .controller("floorEditCtrl", ['$scope', '$http', '$location', '$stateParams', 'dictionary', 'floorRes', function ($scope, $http, $location, $stateParams, dictionary, floorRes) {

        $scope.init = function() {
            $scope.Data = {};
            $scope.FloorId = "null";

            //orgRes.get({ CurrentPage: 1, PageSize: 10 }, function (data) {
            //    $scope.Data.orgs = data.Data;
            //});

            if ($stateParams.id) {
                floorRes.get({ id: $stateParams.id }, function(data) {
                    $scope.Data.floor = data;
                    $scope.FloorId = data.FloorId;
                });
                $scope.isAdd = false;
            } else {
                $scope.isAdd = true;
            }

            $('#form1').validate({
                doNotHideMessage: false,
                errorClass: 'error-span',
                errorElement: 'span',
                rules: {
                    /* Create Account */
                    email: {
                        required: true,
                        email: true
                    },
                    password: {
                        minlength: 3,
                        required: true
                    },
                    name: {
                        required: true
                    },
                    gender: {
                        digits: true,
                        maxlength: 3
                    },
                    location: {
                        required: true
                    },
                    country: {
                        required: true
                    },
                    phone: {
                        required: true
                    },
                    /* Payment Details */
                    card_number: {
                        required: true,
                        digits: true,
                        minlength: 16,
                        maxlength: 16
                    },
                    IdNo: {
                        required: true,
                        minlength: 10,
                        maxlength: 10
                    },
                    card_expirydate: {
                        required: true
                    },
                    card_holder_name: {
                        required: true
                    }
                },

                invalidHandler: function (event, validator) {
                    alert_success.hide();
                    alert_error.show();
                },

                highlight: function (element) {
                    $(element)
                        .closest('.form-group').removeClass('has-success').addClass('has-error');
                },

                unhighlight: function (element) {
                    $(element)
                        .closest('.form-group').removeClass('has-error');
                },

                success: function (label) {
                    if (label.attr("for") == "gender") {
                        label.closest('.form-group').removeClass('has-error').addClass('has-success');
                        label.remove();
                    } else {
                        label.addClass('valid')
                        .closest('.form-group').removeClass('has-error').addClass('has-success');
                    }
                }
            });
        };

        $scope.submit = function() {
            floorRes.save($scope.Data.floor, function(data) {
                $location.url("/angular/floorList");
            });
        };
        $scope.init();
    }]);