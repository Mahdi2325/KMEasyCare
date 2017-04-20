angular.module('sltcApp')
.controller("resourceLinkCtrl",['$scope', '$http', '$location', '$state', 'utility', 'dictionary', 'resourcelinkRes', 'empFileRes',
    function ($scope, $http, $location, $state, utility, dictionary, resourcelinkRes, empFileRes) {
        $scope.FeeNo = $state.params.FeeNo;
        Date.prototype.format = function (format) {
            var date = {
                "M+": this.getMonth() + 1,
                "d+": this.getDate(),
                "h+": this.getHours(),
                "m+": this.getMinutes(),
                "s+": this.getSeconds(),
                "q+": Math.floor((this.getMonth() + 3) / 3),
                "S+": this.getMilliseconds()
            };
            if (/(y+)/i.test(format)) {
                format = format.replace(RegExp.$1, (this.getFullYear() + '').substr(4 - RegExp.$1.length));
            }
            for (var k in date) {
                if (new RegExp("(" + k + ")").test(format)) {
                    format = format.replace(RegExp.$1, RegExp.$1.length == 1
                           ? date[k] : ("00" + date[k]).substr(("" + date[k]).length));
                }
            }
            return format;
        }
        
 
        $scope.Data = {};
        $scope.curUser = utility.getUserInfo();
        if (typeof ($scope.curUser) != 'undefined') {
            $scope.currentItem = { RecordBy: $scope.curUser.EmpNo };
        }

        $scope.ChangeNextEvalDate = function () {
            
            if ($scope.currentItem.ContactDate != "" && $scope.currentItem.FinishDate != "") {
                var days = DateDiff($scope.currentItem.FinishDate, $scope.currentItem.ContactDate)
                if (days < 0) {
                    utility.message("连接完成日期不能小于首次联系日期");
                    $scope.currentItem.FinishDate = "";
            
                } else {
                  
                }
            };
        }
        //当前住民
        $scope.currentResident = {};
        $scope.buttonShow = false;
        //当前分页码
        $scope.currentPage = 1;


        $scope.init = function () {
            empFileRes.get({ empNo: '' }, function (data) {
                $scope.Data.EmpList = data.Data;
            })

            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: resourcelinkRes,//异步请求的res
                success: function (data) {//请求成功时执行函数
                    $scope.Data.resourceLinks = data.Data;
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

            $scope.currentResident = resident;
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.currentItem = { RecordBy: $scope.curUser.EmpNo };
            }
            

           
            if (angular.isDefined($scope.currentResident)) {
                $scope.buttonShow = true;
            }
          
            $scope.currentItem.RegNo = resident.RegNo;
            $scope.currentItem.FeeNo = resident.FeeNo;
            $scope.curUser = utility.getUserInfo();
            //$scope.currentItem.RecordBy = $scope.curUser.EmpNo;
            $scope.getResourceLinkByNo(resident.FeeNo);//加载当前住民的资源连接列表
            $scope.currentItem.ContactDate = new Date().format("yyyy-MM-dd");
        }
        //获取住民资源连接
        $scope.getResourceLinkByNo = function (feeNo) {
            $scope.Data.resourceLinks = {};


            $scope.options.pageInfo.CurrentPage = 1;
            $scope.options.params.feeNo = feeNo;
            $scope.options.search();

            //resourcelinkRes.get({ feeNo: feeNo, currentPage: $scope.currentPage, pageSize: 5 }, function (data) {
            //    $scope.Data.resourceLinks = data.Data;
            //    var pager = new Pager('pager', $scope.currentPage, data.PagesCount, function (curPage) {
            //        $scope.currentPage = curPage;
            //        $scope.getResourceLinkByNo(feeNo);
            //    });
            //});
        }



       
        //保存资源连接
        $scope.createItem = function (item) {
            if ($scope.rsForm.$valid) {//判断验证通过后才可以保存
                resourcelinkRes.save(item, function (data) {
                    utility.message("资料保存成功！");
                    $scope.getResourceLinkByNo($scope.currentResident.FeeNo);
                    $scope.curUser = utility.getUserInfo();
                    if (typeof ($scope.curUser) != 'undefined') {
                        $scope.currentItem = { RecordBy: $scope.curUser.EmpNo };
                    }
                    $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
                });
            }
            else {
                //验证没有通过
                $scope.getErrorMessage($scope.rsForm.$error);
                $scope.errs = $scope.errArr.reverse();
                for (var n = 0; n < $scope.errs.length; n++) {
                    if (n < 3) {
                        utility.msgwarning($scope.errs[n]);
                    }
                    //if (n > 2) break;
                }
            }
            
            

        };
        $scope.resetReL = function () {
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.currentItem = { RecordBy: $scope.curUser.EmpNo };
            }
            $scope.currentItem.FeeNo = $scope.currentResident.FeeNo;
        }
        //删除一条记录
        $scope.deleteItem = function (item) {
            if (confirm("您确定要删除该条记录吗?")) {
               resourcelinkRes.delete({ id: item.Id }, function (data) {
                
                    if (data.$resolved) {
                        var whatIndex = null;
                        angular.forEach($scope.Data.resourceLinks, function (cb, index) {
                            if (cb.Id = item.Id) whatIndex = index;
                        });

                        $scope.options.pageInfo.CurrentPage = 1;
                        $scope.options.search();
                        //$scope.Data.resourceLinks.splice(whatIndex, 1);
               
                        if(data.ResultCode==0)
                            utility.message("资料删除成功！");
                    }
                });
            }
        };
        //更新数据一条记录
        $scope.updateItem = function (item) {
            //resourcelinkRes.save(item, function (data) {
            //    utility.message("资料保存成功！");
            //});
            item.$save();
            $scope.curUser = utility.getUserInfo();
            if (typeof ($scope.curUser) != 'undefined') {
                $scope.currentItem = { RecordBy: $scope.curUser.EmpNo };
            }
            utility.message("资料更新成功！");
        };
        $scope.rowSelect = function (item) {
            $scope.currentItem = item;
        }
        //新增或编辑窗口的数据保存函数
        $scope.saveEdit = function (item) {
            if (angular.isDefined(item.id)) {
                $scope.updateItem(item);
            }
            else {
                $scope.createItem(item);
            }

        }
        //选择填写人员
        $scope.staffSelected = function (item) {
            $scope.currentItem.RecordByName = item.EmpName;
            $scope.currentItem.RecordBy = item.EmpNo;
            
        }
        ///编辑或新增时弹出窗
        $scope.editOrCreate = function (item) {
            $scope.currentItem = item ? item : {};
            $("#modalResourceLink").modal("toggle");
            $scope.buttonShow = true;
        };
        //窗口关闭操作
        $scope.cancelEdit = function () {
            if ($scope.currentItem && $scope.currentItem.$get) {
                $scope.currentItem.$get();
            }

            $scope.currentItem = {};
            $("#modalResourceLink").modal("toggle");
        };
        //验证信息提示
        $scope.getErrorMessage = function (error) {
            $scope.errArr = new Array();
            //var errorMsg = '';
            if (angular.isDefined(error)) {
                if (error.required) {
                    $.each(error.required, function (n, value) {
                        $scope.errArr.push(value.$name + "不能为空");
                    });
                }
                if (error.email) {
                    $.each(error.email, function (n, value) {
                        $scope.errArr.push(value.$name + "邮箱验证失败");
                    });
                }
                if (error.number, function (n, value) {
                    $scope.errArr.push(value.$name + "只能录入数字");
                });

                if (error.maxlength) {
                    $.each(error.maxlength, function (n, value) {
                        $scope.errArr.push(value.$name + "录入已超过最大设定长度！");

                    });
                }
                //return errorMsg;
            }
        }
        $scope.init();
    }]);

