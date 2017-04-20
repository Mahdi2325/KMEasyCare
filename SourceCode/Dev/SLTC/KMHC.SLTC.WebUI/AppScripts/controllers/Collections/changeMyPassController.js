/*
创建人:Bob Du
创建日期:2016-09-22
说明:修改密码控制器
*/

(function () {
    'use strict';
    angular.module('sltcApp').controller("changeMyPassController", ['$scope', '$compile', '$state', 'userRes', changeMyPassController]);
    function changeMyPassController($scope, $compile, $state, userRes) {
        _init();

        function _init() {
           

            $scope.change = function () {
                _checkBeforeChange() && _change();
            };
        }

        function _checkBeforeChange() {
            var form = $scope.changePass_form;

            if (!form.$valid) {
                if (form.oldPass_input.$error.required) {
                    $scope.errorMessage = "请输入旧密码。";
                }
                else if (form.newPass1_input.$error.required || form.newPass2_input.$error.required) {
                    $scope.errorMessage = "请输入新密码。";
                }
                else {
                    $scope.errorMessage = "登录异常，请联系系统管理员。";
                }

                return false;
            }
            else if($scope.newPass1 != $scope.newPass2){
                $scope.errorMessage = "两次输入的新密码不一致，请重新输入。";

                return false;
            }

            return true;
        }

        function _change() {
            var data = {
                oldPwd: $scope.oldPass,
                newPwd: $scope.newPass1
            };
            userRes.ChangePassWord(data, function (res) {
                if (res.ResultCode == 0) {
                    $scope.$parent.hide();
                } else {
                    $scope.errorMessage = res.ResultMessage;
                }
                
            });
        }
    }
})();

