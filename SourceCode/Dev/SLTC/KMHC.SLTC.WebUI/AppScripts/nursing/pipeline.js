/*****************************************************************************
 * Filename: pipeline
 * Creator:	Lei Chen
 * Create Date: 2016-03-01 13:34:19
 * Modifier:
 * Modify Date:
 * Description:管路
 ******************************************************************************/


function GetNextEvalDate(currentDate, gap) {
    if (!isEmpty(currentDate) && !isEmpty(gap)) {
        var d = new Date(Date.parse(currentDate.replace(/-/g, "/")));
        d.setDate(d.getDate() + gap * 1);
        return d.format("yyyy-MM-dd");
    }
};

angular.module("sltcApp")
.controller("pipelineCtr", ['$scope', '$http', '$location', '$state', 'dictionary', 'cloudAdminUi', 'residentRes', 
function ($scope, $http, $location, $state, dictionary, cloudAdminUi, pipelineRes) {
    //services.get({}, function (data) {
    //    $scope.Data = data.Data;

    //});
 
    var id = $state.params.id;
    $scope.init = function () {
        cloudAdminUi.handleGoToTop();
       
        $scope.saveClass = "ui-inline-show";
        $scope.updateClass = "ui-inline-hide";
        $scope.Dic = {};
        $scope.Data = {};
        $scope.TabContent = {};
       
        $(".datepicker").datepicker({
            dateFormat: "yy-mm-dd",
            changeMonth: true,
            changeYear: true
            //maxDate: "0d"
        });
        var dicType = ["EvalDateGap"];
        dictionary.get(dicType, function (dic) {
            $scope.Dic = dic;
        });
    }

    $scope.init(); //初始化
  
    $scope.setNextValDate = function (pipelineEval) {
     
        var gap = pipelineEval.INTERVAL.Value;
        var currentDate = pipelineEval.RECENTDATE;
        var t = GetNextEvalDate(currentDate, gap);
        $scope.Data.pipelineEval.NEXTDATE = GetNextEvalDate(currentDate, gap);
    }

    $scope.Data = { "pipelineEvalList": [{ "ID": 1, "FEENO": 101, "REGNO": 101, "EVALDATE": "2016-02-28", "RECENTDATE": "2016-02-25", "INTERVAL": 7, "NEXTDATE": "2016-03-09", "STATE": null, "OPERATOR": "OK", "CREATEDATE": null, "CREATEBY": "001", "ORGID": null, "SEQ": 10, "KIND": "纱管", "FEATHER": null, "AGUGE": "纱管", "PIPELINENAME": "鼻胃管", "LTC_PIPELINE": null }, { "ID": 1, "FEENO": 101, "REGNO": null, "EVALDATE": "2016-02-27", "RECENTDATE": null, "INTERVAL": null, "NEXTDATE": "2016-03-09", "STATE": null, "OPERATOR": "OK", "CREATEDATE": null, "CREATEBY": "001", "ORGID": null, "SEQ": 10, "KIND": "鼻胃管", "FEATHER": null, "AGUGE": "纱管", "PIPELINENAME": "鼻胃管", "LTC_PIPELINE": null }, { "ID": 1, "FEENO": 101, "REGNO": null, "EVALDATE": "2016-02-22", "RECENTDATE": "2016-02-25", "INTERVAL": null, "NEXTDATE": "2016-03-09", "STATE": null, "OPERATOR": null, "CREATEDATE": null, "CREATEBY": "001", "ORGID": null, "SEQ": 11, "KIND": "导尿管", "FEATHER": null, "AGUGE": "纱管", "PIPELINENAME": "鼻胃管", "LTC_PIPELINE": null }, { "ID": 1, "FEENO": 102, "REGNO": null, "EVALDATE": "2016-02-12T12:40:06.3573499+08:00", "RECENTDATE": "2016-02-25", "INTERVAL": null, "NEXTDATE": "2016-03-09", "STATE": null, "OPERATOR": null, "CREATEDATE": null, "CREATEBY": "001", "ORGID": null, "SEQ": 11, "KIND": "导尿管", "FEATHER": null, "AGUGE": "纱管", "PIPELINENAME": null, "LTC_PIPELINE": null }, { "ID": 1, "FEENO": 102, "REGNO": null, "EVALDATE": "2016-02-02", "RECENTDATE": "2016-02-25", "INTERVAL": null, "NEXTDATE": "2016-03-09", "STATE": null, "OPERATOR": null, "CREATEDATE": null, "CREATEBY": "001", "ORGID": null, "SEQ": 11, "KIND": "导尿管", "FEATHER": null, "AGUGE": "纱管", "PIPELINENAME": null, "LTC_PIPELINE": null }], "pipelines": [{ "SEQ": 10, "KIND": "鼻胃管", "FEATHER": null, "AGUGE": "纱管", "LTC_PIPELINEEVAL": [] }, { "SEQ": 11, "KIND": "导尿管", "FEATHER": null, "AGUGE": "纱管", "LTC_PIPELINEEVAL": [] }], "pipelineEval": { "ID": 0, "FEENO": null, "REGNO": null, "EVALDATE": null, "RECENTDATE": null, "INTERVAL": null, "NEXTDATE": null, "STATE": null, "OPERATOR": null, "CREATEDATE": null, "CREATEBY": null, "ORGID": null, "SEQ": null, "LTC_PIPELINE": null }, "pipeline": { "SEQ": 0, "KIND": null, "FEATHER": null, "AGUGE": null, "LTC_PIPELINEEVAL": [] } };
    $scope.Edit = function (currentItem) {
        $scope.saveClass = "ui-inline-hide";
        $scope.updateClass = "ui-inline-show";
        $scope.Data.pipelineEval = currentItem;

    }
    $scope.Delete = function (currentItem) {
        $scope.Data.pipelineEvalList.splice($scope.Data.pipelineEvalList.indexOf(currentItem), 1);
    }

    $scope.Save = function (currentItem) {
        var item = { 'ID': currentItem.ID, 'EVALDATE': currentItem.EVALDATE };
        $scope.Data.pipelineEvalList.push(item)

    }

    $scope.Update = function (currentItem) {
        $scope.Data.pipelineEval = null;
        $scope.saveClass = "ui-inline-show";
        $scope.updateClass = "ui-inline-hide";
    }

    $scope.AddPipeline = function (pipeline) {
        var item = { 'SEQ': pipeline.SEQ, 'KIND': pipeline.KIND };
        $scope.Data.pipelines.push(item)
        pipeline = null;
        $('#modalPipeline').modal('toggle');
    }
  

}
])
