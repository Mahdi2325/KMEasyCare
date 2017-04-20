///创建人:张正泉
///创建日期:2016-03-15
///说明:通讯录管理

angular.module("sltcApp")
.controller('addressCtrl', ['$rootScope', '$scope', '$state', 'relationRes', function ($rootScope, $scope, $state, relationRes) {
    var loadData = function () {
        if (!$scope.person.Relation) {
            $scope.person.Relation = {};
            relationRes.get({ id: $scope.person.FeeNo }, function (data) {
                angular.copy(data.Data, $scope.person.Relation);
            });
        }
    }
    //var id = $state.params.id;
    $scope.init = function () {
        loadData();
        //$scope.$on("dataLoaded", function () {
        //    $scope.person = $scope.$parent.person;
        //});
        //$scope.relatives = relativeRes.query();
    }

    //$scope.currentItem = null;

    //$scope.deleteItem = function (item) {
    //    item.$delete().then(function () {
    //        $scope.relatives.splice($scope.relatives.indexOf(item), 1);
    //    });
    //};

    //$scope.createItem = function (item) {
    //    new relativeRes(item).$save().then(function (newItem) {
    //        $scope.relatives.push(newItem);
    //    });
    //};

    //$scope.updateItem = function (item) {
    //    item.$save();
    //};


    //$scope.editOrCreate = function (item) {
    //    $scope.currentItem = item ? item : {};
    //    $("#relativeModal").modal("toggle");
    //};

    //$scope.cancelEdit = function () {
    //    //还原
    //    if ($scope.currentItem && $scope.currentItem.$get) {
    //        $scope.currentItem.$get();
    //    }
    //    $scope.currentItem = {};
    //    $("#relativeModal").modal("toggle");
    //};

    //$scope.saveEdit = function (item) {
    //    if (angular.isDefined(item.id)) {
    //        $scope.updateItem(item);
    //    } else {
    //        $scope.createItem(item);
    //    }
    //    $("#relativeModal").modal("toggle");
    //};

    $scope.$on('LoadTabData', function (data) {
        loadData();
    });

    $scope.copyCensusInfo = function () {
        $scope.person.Relation.Zip2 = $scope.person.Relation.Zip1;
        $scope.person.Relation.HabitationCity = $scope.person.Relation.CensusCity;
        $scope.person.Relation.City2 = $scope.person.Relation.City1;
        $scope.person.Relation.Address2 = $scope.person.Relation.Address1;
        $scope.person.Relation.Address2dtl = $scope.person.Relation.Address1dtl;
    };
 
    $scope.habitationPostcodeChange = function (o) {
        if (typeof ($scope.person.Relation.HabitationRegion) == "undefined" || $scope.person.Relation.HabitationRegion == "") {
            $scope.person.Relation.HabitationCity = o.CityCode + "-" + o.City;
        }
        if (typeof ($scope.person.Relation.HabitationRegion) == "undefined" || $scope.person.Relation.HabitationRegion == "") {
            $scope.Relation.HabitationRegion = o.Region;
        }
    };

    $scope.init();


    $scope.PostCodeSelected = function (item)
    {
        $scope.person.Relation.Zip1 = item.PostCode;
        $scope.person.Relation.City1 = item.City;
        $scope.person.Relation.Address1 = item.Town;
    }

    $scope.PostSelected = function (item) {
        $scope.person.Relation.Zip2 = item.PostCode;
        $scope.person.Relation.City2 = item.City;
        $scope.person.Relation.Address2 = item.Town;
    }
}]);