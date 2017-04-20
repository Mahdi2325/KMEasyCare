///创建人:肖国栋
///创建日期:2016-02-26
///说明:家系图

angular.module("sltcApp")
.controller('homePhotoCtrl', ['$rootScope', '$scope', '$state', 'utility', 'webUploader', 'relationRes', function ($rootScope, $scope, $state, utility, webUploader, relationRes) {
    var loadData = function () {
        if (!$scope.person.Relation) {
            $scope.person.Relation = {};
            if (!!$scope.person.FeeNo) {
                relationRes.get({ id: $scope.person.FeeNo }, function (data) {
                    angular.copy(data.Data, $scope.person.Relation);
                });
            }
        }
    }
    $scope.init = function () {
        loadData();
        //seajs.use(['/Scripts/webuploader', '/Content/webuploader.css'], function () {
            //webUploader.init('#HeadPhotoPicker', { category: 'HomePhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
            //    if (data.length > 0) {
            //        $scope.Detail.HeadPhotoUrl = data[0].SavedLocation;
            //        $scope.Detail.HeadPhotoName = data[0].FileName;
            //        $scope.$apply();
            //    }
            //});

            //webUploader.init('#LivingPhotoPicker', { category: 'HomePhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
            //    if (data.length > 0) {
            //        $scope.Detail.LivingPhotoUrl = data[0].SavedLocation;
            //        $scope.Detail.LivingPhotoName = data[0].FileName;
            //        $scope.$apply();
            //    }
            //});

            //webUploader.init('#PedigreeChartPicker', { category: 'HomePhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
            //    if (data.length > 0) {
            //        $scope.Detail.PedigreeChartUrl = data[0].SavedLocation;
            //        $scope.Detail.PedigreeChartName = data[0].FileName;
            //        $scope.$apply();
            //    }
            //});

            //webUploader.init('#PedigreeFilePicker', { category: 'HomePhoto' }, '文件', 'doc,docx', 'doc/*', function (data) {
            //    if (data.length > 0) {
            //        $scope.Detail.PedigreeFileUrl = data[0].SavedLocation;
            //        $scope.Detail.PedigreeFileName = data[0].FileName;
            //        $scope.$apply();
            //    }
            //});

        //});

        webUploader.init('#PhothoPathPicker', { category: 'HomePhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
            if (data.length > 0) {
                //$scope.person.ImgUrl = data[0].SavedLocation;
                $scope.person.Relation.PhotoPath = data[0].SavedLocation;
                // $scope.Detail.LivingPhotoName = data[0].FileName;
                $scope.$apply();
            }
        });
        webUploader.init('#FamilyPathPicker', { category: 'HomePhoto' }, '图片', 'gif,jpg,jpeg,bmp,png', 'image/*', function (data) {
            if (data.length > 0) {
                $scope.person.Relation.FamilyPath = data[0].SavedLocation;
                //$scope.Detail.PedigreeFileName = data[0].FileName;
                $scope.$apply();
            }
        });

        $scope.$on('LoadTabData', function (data) {
            loadData();
        });

        //$rootScope.$on('refreshTabData', function (event, data) {
        //    $scope.Detail = residentData.HomePhoto;
        //});
    }

    $scope.clear = function (type) {
        switch (type)
        {
            case "PhotoPath":
                $scope.person.Relation.PhotoPath = "";
                //$scope.Detail.HeadPhotoName = "";
                break;
            case "FamilyPath":
                $scope.person.Relation.FamilyPath = "";
                //$scope.Detail.LivingPhotoName = "";
                break;
            //case "PedigreeChart":
            //    $scope.Detail.PedigreeChartUrl = "";
            //    $scope.Detail.PedigreeChartName = "";
            //    break;
            //case "PedigreeFile":
            //    $scope.Detail.PedigreeFileUrl = "";
            //    $scope.Detail.PedigreeFileName = "";
            //    break;
        }
    }

    $scope.init();
}]);