/*
创建人:张正泉
创建日期:2016-03-15
说明: 压疮
*/
angular.module("sltcApp")
.controller("prsSoreCtrl", ['$scope', '$state', '$filter', 'prsSoreRes', 'sorechgRes', 'webUploader', 'utility', '$timeout',function ($scope, $state, $filter, prsSoreRes, sorechgRes, webUploader, utility, $timeout) {
    $scope.FeeNo = $state.params.FeeNo;
    $scope.viewPhotos = [];
    $scope.init = function () {
        var nowDate = $filter("date")(new Date(), "yyyy-MM-dd HH:mm:ss");
        $scope.curUser = utility.getUserInfo();
        $scope.curResident = $scope.curResident || {};
        $scope.curItem = {
            OccurDate: nowDate, EvaluteBy: $scope.curUser.EmpNo,
            RegNo: $scope.curResident.RegNo,
            FeeNo: $scope.curResident.FeeNo
        };
        $scope.initDel();
        var path = "";
        webUploader.init('#WoundPathPicker', { category: 'WoundPhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
            if (data.length > 0) {
                path = path + data[0].SavedLocation+",";
            }},true,function (data) {
            if (data.uploadsSuccessful > 0) {
                $scope.curDelItem.Picture = path.substr(0,path.length-1);
                $scope.$apply();
            }
       });

        webUploader.init('#PhothoPathPicker1', { category: 'WoundPhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
            if (data.length > 0) {
                $scope.curItem.Pict1 = data[0].SavedLocation;
                $scope.$apply();
            }
        });

        webUploader.init('#PhothoPathPicker2', { category: 'WoundPhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
            if (data.length > 0) {
                $scope.curItem.Pict2 = data[0].SavedLocation;
                $scope.$apply();
            }
        });

        webUploader.init('#PhothoPathPicker3', { category: 'WoundPhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
            if (data.length > 0) {
                $scope.curItem.Pict3 = data[0].SavedLocation;
                $scope.$apply();
            }
        });

        webUploader.init('#ArchiveFilePicker', { category: 'ArchiveFile' }, '文件', 'doc,docx', 'doc/*', function (data) {
            if (data.length > 0) {
                $scope.curItem.Pict4 = data[0].SavedLocation;
                $scope.$apply();
            }
        });
        
    }

    $scope.initDel = function () {
        $scope.curDelItem = { EcalDate: $filter("date")(new Date(), "yyyy-MM-dd HH:mm:ss"), Nurse: $scope.curUser.EmpNo };
        $scope.isAdd = true;
    }
    
    $scope.$watch("curItem.RecoveryFalg", function (newValue) {
        if (newValue) {
            $scope.curItem.RecoveryDate = $filter("date")(new Date(), "yyyy-MM-dd");
        } else {
            $scope.curItem.RecoveryDate = null;
        }

    });

    $scope.$watch("curItem.FeeNo", function (newValue) {
        $scope.search(newValue);
    });

    //选中住民
    $scope.residentSelected = function (resident) {
        $scope.curResident = resident;//获取当前住民信息
        angular.extend($scope.curItem, { FeeNo: resident.FeeNo, RegNo: resident.RegNo });
    }

    $scope.staffSelected = function (item, t) {
        if (!t) {
            $scope.curItem.EvaluteBy = item.EmpNo;
        } else {
            $scope.curDelItem.Nurse = item.EmpNo;
        }
        
    }

        $scope.search = function(feeNo) {
            if (feeNo) {
                prsSoreRes.get({ feeno: feeNo }, function(data) {
                    if (data.Data) {
                        $scope.curItem = data.Data;
                        $scope.initDel();
                    } else {
                        $scope.init();
                    }
                });
            }
        };

        $scope.modify = function(item) {
            $scope.curItem = item;
            sorechgRes.query({ Seq: item.Seq }, function(data) {
                $scope.curItem.Detail = data;
            });
            $("#modalAll").modal("toggle");
        };

        $('#modalViewPhoto').on('show.bs.modal', function (event) {
            var type = $(event.relatedTarget).data('type');
            var photos = $scope.$eval(type);
            if (angular.isString(photos) && photos.length>0) {
                $scope.viewPhotos = photos.split(",");
            } else {
                $scope.viewPhotos = [];
            }
            $scope.$apply();
        });

    //删除压疮记录
    $scope.deleteItem = function (item) {
        if (confirm("您确定要删除该住民的压疮记录吗?")) {
            $scope.curItem.Detail.splice($scope.curItem.Detail.indexOf(item), 1);
            sorechgRes.delete({id:item.Id});
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
                $scope.curItem.Pict4 = "";
                break;
            case "PedigreeFile":
                $scope.Detail.PedigreeFileUrl = "";
                $scope.Detail.PedigreeFileName = "";
                break;
        }
    }


    $scope.editOrCreate = function (item) {
        if (item) {
            $scope.curDelItem = item;// angular.copy(item);
            $scope.isAdd = false;
        } else {
            $scope.isAdd = true;
            $scope.curDelItem = { Seq: $scope.curItem.Seq };
        }
    }
    
    $scope.searchAll = function () {
        var feeNo = $scope.curItem.FeeNo;
        if (feeNo) {
            prsSoreRes.query({ CurrentPage: 1, PageSize: 10,  FeeNo: feeNo }, function (data) {
                if (data.length > 0) {
                    $scope.prsSoreList = data;
                }
            });
        }
    }
    
    $scope.save= function () {
        prsSoreRes.save($scope.curItem);
        utility.message("主文档保存成功！");
    }

    $scope.mEditOrAdd = function (type) {
        if (type == "add") {
            $scope.curItem.Seq = 0;
            prsSoreRes.save($scope.curItem, function (data) {
                $scope.curItem = data.Data;
                $scope.curDelItem = {};
                utility.message("文档保存成功！");
            });
        } else {
            if ($scope.curItem.Seq) {
                var detail = $scope.curItem.Detail;
                $scope.curItem.Detail = [];//只保存主文档时,传到后端数据过滤掉detail
                prsSoreRes.save($scope.curItem, function (data) {
                    $scope.curItem = data.Data;
                    utility.message("文档保存成功！");
                });
                $scope.curItem.Detail = detail;
            } else {
                prsSoreRes.save($scope.curItem, function (data) {
                    $scope.curItem = data.Data;
                    $scope.curDelItem = {};
                    utility.message("文档保存成功！");
                });
            }
        }
        
        
    }

    $scope.mAdd = function () {
        prsSoreRes.save($scope.curItem, function (data) {
            $scope.curItem = data.Data;
            $scope.curDelItem = {};
            utility.message("文档保存成功！");
        });
    }


    $scope.dEditOrAdd = function () {
        if (!$scope.curItem.Seq) {
            $scope.curItem.Detail = [];
            if (!$scope.curDelItem.Id) {
                $scope.curItem.Detail.push($scope.curDelItem);
            }
            $scope.mEditOrAdd();
        } else {
            $scope.curDelItem.Seq = $scope.curItem.Seq;
            sorechgRes.save($scope.curDelItem, function (data) {
                if (!$scope.curDelItem.Id) {
                    $scope.curItem.Detail.push(data.Data);
                } else {
                    for (var i = $scope.curItem.Detail.length - 1; i > -1; i--) {
                        if ($scope.curItem.Detail[i].Id == data.Data.Id) {
                            $scope.curItem.Detail[i] = data.Data;
                        }
                    }
                }
                $scope.curDelItem = {};
            });
        }
        $scope.isAdd = true;
    }

    $scope.saveEdit = function () {
        prsSoreRes.save($scope.curItem, function () {
            $scope.isAdd = true;
            $scope.curDelItem = {};
            utility.message("修改压疮信息保存成功！");
        });
    };

    $scope.init();
}]);





