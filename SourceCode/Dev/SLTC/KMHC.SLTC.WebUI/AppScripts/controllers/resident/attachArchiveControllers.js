///创建人:肖国栋
///创建日期:2016-02-23
///说明:附加管理

angular.module("sltcApp")
.controller("attachArchiveCtrl", ['$scope', 'dictionary', 'webUploader', 'attachArchiveRes', 'utility', function ($scope, dictionary, webUploader, attachArchiveRes, utility) {
    var loadData = function () {
        if (!$scope.person.AttachArchives) {
            $scope.person.AttachArchives = [];
            if (!!$scope.person.FeeNo) {
                attachArchiveRes.get({ feeNo: $scope.person.FeeNo, currentPage: 1, pageSize: 10 }, function (data) {
                    if (data.Data.DocPath != null && typeof (data.Data.DocPath) != "undefined") {
                        var fi = data.Data.DocPath.split('|$|');
                        if (fi.length == 2) {
                            data.Data.SavedLocation = fi[1];
                            data.Data.FileName = fi[0];
                        } else if (fi.length == 1) {
                            data.Data.SavedLocation = fi[0];
                            data.Data.FileName = fi[0];
                        }
                    }
                    angular.copy(data.Data, $scope.person.AttachArchives);
                });
            }
        }
    }

    //$scope.displayMode = "list";
    $scope.currentItem = null;
    $scope.Keyword = "";

    $scope.init = function () {
        loadData();
        //$scope.AttachArchives = attachArchiveRes.query();
    }

    $scope.$on('LoadTabData', function (data) {
        loadData();
    });

    //删除附加文件
    $scope.deleteItem = function (item) {
        attachArchiveRes.delete({ id: item.Id }, function () {
            $scope.person.AttachArchives.splice($scope.person.AttachArchives.indexOf(item), 1);
            utility.message("删除成功");
        });
    };

    $scope.createItem = function (item) {
        $scope.currentItem.FeeNo = $scope.person.FeeNo;
        attachArchiveRes.save($scope.currentItem, function (data) {
            $scope.person.AttachArchives.push(data.Data);
            utility.message("保存成功!");
        });
        $scope.currentItem = {};
    };

    $scope.updateItem = function (item) {
        attachArchiveRes.save(item, function (data) {
            utility.message("保存成功！");
        });
        $scope.currentItem = {};
        //$scope.displayMode = "list";
    };


    $scope.edit = function (item) {
        $scope.currentItem = item ? item : {};
        $scope.currentItemCopy = {};
        $scope.currentItem.FileName = item.DocPath;
        $scope.currentItem.SavedLocation = item.DocPath;

        angular.copy($scope.currentItem, $scope.currentItemCopy);
        //$scope.displayMode = "edit";
        $("#attachArchiveModal").modal("toggle");
    };
    $scope.create = function () {
        $scope.currentItem = {};
        $scope.currentItemCopy = {};
        angular.copy($scope.currentItem, $scope.currentItemCopy);
        //$scope.displayMode = "edit";
        $("#attachArchiveModal").modal("toggle");
    };

    $scope.cancelEdit = function () {
        angular.copy($scope.currentItemCopy, $scope.currentItem);
        $scope.currentItem = {};
        $("#attachArchiveModal").modal("toggle");
    };

    $scope.saveEdit = function (item) {
        item.DocPath = '{0}|$|{1}'.format(item.FileName, item.SavedLocation);
        if (angular.isDefined(item.$$hashKey)) {
            $scope.updateItem(item);
        } else {
            $scope.createItem(item);
        }
        $("#attachArchiveModal").modal("toggle");
    };

    $scope.clear = function () {
        $scope.currentItem.SavedLocation = "";
        $scope.currentItem.FileName = "";
    }

    $scope.init();
}])
.controller("attachArchiveEditCtrl", ['$scope', 'webUploader', function ($scope, webUploader) {
    webUploader.init('#ArchiveFilePicker', { category: 'ArchiveFile' }, '文件', 'doc,docx,pdf,xls,xlsx,gif,jpg,jpeg,bmp,png', 'doc/*,application/pdf,image/*,xls/*', function (data) {
        if (data.length > 0) {
            //$scope.currentItem.DocPath = data[0].SavedLocation;
            $scope.currentItem.SavedLocation = data[0].SavedLocation;
            $scope.currentItem.FileName = data[0].FileName;
            $scope.$apply();
        }
    });
}]);


