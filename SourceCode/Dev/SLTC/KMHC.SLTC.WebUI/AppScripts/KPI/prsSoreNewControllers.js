/*
创建人:刘美方
创建日期:2016-05-4
说明: 压疮
*/
angular.module("sltcApp")
.controller("prsSoreNewCtrl", ["$scope", "$state", "$filter", "prsSoreRes", "sorechgRes", "webUploader", "utility",
    function ($scope, $state, $filter, prsSoreRes, sorechgRes, webUploader, utility) {
        $scope.FeeNo = $state.params.FeeNo;
        $scope.viewPhotos = [];
        var nowDate = $filter("date")(new Date(), "yyyy-MM-dd HH:mm:ss");
        $scope.init = function () {
            $scope.curUser = utility.getUserInfo();
            $scope.curResident = $scope.curResident || {};
            $scope.curItem = {
                OccurDate: nowDate,
                EvaluteBy: $scope.curUser.EmpNo,
                RegNo: $scope.curResident.RegNo,
                FeeNo: $scope.curResident.FeeNo
            };
            $scope.editShow = false;
            $scope.editDetailShow = false;
            $scope.prsSoreList = [];
            $scope.options = {
                buttons: [], //需要打印按钮时设置
                ajaxObject: prsSoreRes, //异步请求的res
                params: { feeNo: $scope.FeeNo == "" ? -1 : $scope.FeeNo },
                success: function (data) { //请求成功时执行函数
                    $scope.prsSoreList = data.Data;
                    //if ($scope.prsSoreList.length > 0) {
                    //    $scope.editDetailShow = true;
                    //    if (angular.isDefined($scope.curItem.Seq)) {
                    //        $scope.edit($scope.curItem);
                    //    } else {
                    //        $scope.edit($scope.prsSoreList[0]);
                    //    }
                    //} else {
                    //    $scope.editDetailShow = false;
                    //    $scope.curItem = {
                    //        OccurDate: nowDate,
                    //        EvaluteBy: $scope.curUser.EmpNo,
                    //        RegNo: $scope.curResident.RegNo,
                    //        FeeNo: $scope.curResident.FeeNo
                    //    };
                    //    $scope.edit($scope.curItem);
                    //}
                },
                pageInfo: {
                    //分页信息
                    CurrentPage: 1,
                    PageSize: 10
                }
            }
            $scope.curItem.Detail = [];
            $scope.detailOptions = {
                buttons: [], //需要打印按钮时设置
                ajaxObject: sorechgRes, //异步请求的res
                params: { seq: 0 },
                success: function (data) { //请求成功时执行函数
                    $scope.curItem.Detail = data.Data;
                },
                pageInfo: {
                    //分页信息
                    CurrentPage: 1,
                    PageSize: 10
                }
            }

            webUploader.init('#WoundPathPicker', { category: 'WoundPhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
                if (data.length > 0) {
                    $scope.curDelItem.Picture = data[0].SavedLocation;
                    $scope.$apply();
                }
            });

            var pathPict2 = "";
            webUploader.init('#PhothoPathPicker2', { category: 'WoundPhoto' }, '治疗照片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
                if (data.length > 0) {
                    pathPict2 = pathPict2 + data[0].SavedLocation + ",";
                }
            }, 100, true, function (data) {
                if (data.uploadsSuccessful > 0) {
                    $scope.curItem.Pict2 = pathPict2.substr(0, pathPict2.length - 1);
                    $scope.$apply();
                    pathPict2 = "";
                }
            });

            var pathPict3 = "";
            webUploader.init('#PhothoPathPicker3', { category: 'WoundPhoto' }, '结案照片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
                if (data.length > 0) {
                    pathPict3 = pathPict3 + data[0].SavedLocation + ",";
                }
            }, 100, true, function (data) {
                if (data.uploadsSuccessful > 0) {
                    $scope.curItem.Pict3 = pathPict3.substr(0, pathPict3.length - 1);
                    $scope.$apply();
                    pathPict3 = "";
                }
            });

            webUploader.init('#ArchiveFilePicker', { category: 'ArchiveFile' }, '文件文件（限Word档）', 'doc,docx', 'doc/*', function (data) {
                if (data.length > 0) {
                    $scope.curItem.SavedLocation = data[0].SavedLocation;
                    $scope.curItem.FileName = data[0].FileName;
                    $scope.$apply();
                }
            }, 180);
        };
        //选中住民
        $scope.residentSelected = function (resident) {
            $scope.curResident = resident; //获取当前住民信息
            //angular.extend($scope.curItem, { FeeNo: resident.FeeNo, RegNo: resident.RegNo });
            $scope.curItem = {
                OccurDate: nowDate,
                EvaluteBy: $scope.curUser.EmpNo,
                RegNo: $scope.curResident.RegNo,
                FeeNo: $scope.curResident.FeeNo
            };
            $scope.options.params.feeNo = $scope.curItem.FeeNo;
            $scope.options.search();
            $scope.editShow = true;
            $scope.editDetailShow = false;

        };

        $scope.staffSelected = function (item, t) {
            if (!t) {
                $scope.curItem.EvaluteBy = item.EmpNo;
                $scope.curItem.EvaluteNameBy = item.EmpName;
            } else {
                $scope.curDelItem.Nurse = item.EmpNo;
                $scope.curDelItem.NurseName = item.EmpName;
            }
        };

        $scope.clear = function (type) {
            switch (type) {
                case "Picture":
                    $scope.curDelItem.Picture = "";
                    break;
                case "Pict1":
                    $scope.curItem.Pict1 = "";
                    break;
                case "Pict2":
                    $scope.curItem.Pict2 = "";
                    break;
                case "Pict3":
                    $scope.curItem.Pict3 = "";
                    break;
                case "Pict4":
                    $scope.curItem.SavedLocation = "";
                    $scope.curItem.FileName = "";
                    break;
                case "PedigreeFile":
                    $scope.Detail.PedigreeFileUrl = "";
                    $scope.Detail.PedigreeFileName = "";
                    break;
            }
        };

        $scope.edit = function (item) {
            $scope.curItem = item;

            if ($scope.curItem.Pict4 != null && typeof ($scope.curItem.Pict4) != "undefined") {
                var fi = $scope.curItem.Pict4.split('|$|');
                if (fi.length == 2) {
                    $scope.curItem.SavedLocation = fi[1];
                    $scope.curItem.FileName = fi[0];
                } else if (fi.length == 1) {
                    $scope.curItem.SavedLocation = fi[0];
                    $scope.curItem.FileName = fi[0];
                }
            }
            $scope.detailOptions.params.seq = item.Seq;
            $scope.detailOptions.search();

            $scope.editDetailShow = true;
        };

        $scope.delete = function (id) {
            if (confirm("确定删除该信息吗?")) {
                prsSoreRes.delete({ id: id }, function (data) {
                    $scope.curItem = {
                        OccurDate: nowDate, EvaluteBy: $scope.curUser.EmpNo,
                        RegNo: $scope.curResident.RegNo,
                        FeeNo: $scope.curResident.FeeNo
                    };
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                 //   $scope.save();
                    utility.message("删除成功");
                });
            }
        };


        $scope.checkDate = function() {
            if ($scope.curItem.OccurDate != null && $scope.curItem.RevoceryDate) {
                var days = DateDiff($scope.curItem.RevoceryDate, $scope.curItem.OccurDate);
                if (days < 0) {
                    utility.message("结束日期不能小于发现时间");
                    $scope.curItem.RevoceryDate = "";
                    return;
                }
            }
        };

        $scope.save = function () {
            $scope.checkDate();
            $scope.curItem.Pict4 = '{0}|$|{1}'.format($scope.curItem.FileName, $scope.curItem.SavedLocation);
            prsSoreRes.save($scope.curItem, function (data) {
                $scope.curItem = data.Data;
                $scope.options.search();
                $scope.reset();
                utility.message("保存成功！");
            });
        };

        $scope.reset = function () {
            $scope.curItem = {
                OccurDate:  nowDate, EvaluteBy: $scope.curUser.EmpNo,
                RegNo: $scope.curResident.RegNo,
                FeeNo: $scope.curResident.FeeNo
            };
            $scope.editDetailShow = false;
        };

        $scope.editDetail = function (item) {
            $scope.curUser = utility.getUserInfo();
            $scope.curDelItem = item;
            if (angular.isUndefined($scope.curDelItem.WoundDirection)) {
                $scope.curDelItem.WoundDirection = "270-360";
            }

            if (angular.isUndefined($scope.curDelItem.Nurse)) {
                $scope.curDelItem.Nurse = $scope.curUser.EmpNo;
            }
            $("#modalDetail").modal("toggle");
        };

        $scope.delDetail = function (item) {
            if (confirm("您确定要删除该住民的压疮记录吗?")) {
                sorechgRes.delete({ id: item.Id }, function (data) {
                    $scope.detailOptions.pageInfo.CurrentPage = 1;
                    $scope.detailOptions.search();
                    utility.message("删除成功");
                });
            }
        };

        $scope.saveDetail = function () {
            if (angular.isDefined($scope.form2.$error.required)) {
                for (var i = 0; i < $scope.form2.$error.required.length; i++) {
                    utility.msgwarning($scope.form2.$error.required[i].$name + "为必填项！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.form2.$error.maxlength)) {
                for (var i = 0; i < $scope.form2.$error.maxlength.length; i++) {
                    utility.msgwarning($scope.form2.$error.maxlength[i].$name + "超过设定长度！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            $scope.curDelItem.Seq = $scope.curItem.Seq;
            sorechgRes.save($scope.curDelItem, function (data) {
                $("#modalDetail").modal("toggle");
                $scope.detailOptions.search();
            });
        };

        $('#modalViewPhoto').on('show.bs.modal', function (event) {
            var type = $(event.relatedTarget).data('type');
            var photos = $scope.$eval(type);
            if (angular.isString(photos) && photos.length > 0) {
                $scope.viewPhotos = photos.split(",");
            } else {
                $scope.viewPhotos = [];
            }
            $scope.$apply();
        });
        $('#modalViewPhoto').on('hidden.bs.modal', function (event) {
            $scope.viewPhotos = [];
            $scope.$apply();
        });

        $scope.init();

    }]);





