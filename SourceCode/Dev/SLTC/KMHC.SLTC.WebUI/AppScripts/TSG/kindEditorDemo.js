angular.module("sltcApp").controller("kindeditorCtrl", ['$scope', '$interval',
    function ($scope, $interval) {
        $scope.currentItem = {};
        $scope.currentItem.Answer = {

        }
        $scope.currentItem.Answer.Description = "<img src=\"/Content/kindeditor/kindeditor-4.1.5/attached/image/20161107/20161107153119_2562.png\" alt=\"\" />";
        //$scope.info = {
        //    content: "<img src=\"/Content/kindeditor/kindeditor-4.1.5/attached/image/20161107/20161107153119_2562.png\" alt=\"\" />"
        //};
        $scope.config = {
            width: '100px',
            cssPath: '../Content/kindeditor/kindeditor-4.1.5/plugins/code/prettify.css',
            uploadJson: '../Content/kindeditor/kindeditor-4.1.5/asp.net/upload_json.ashx',
            fileManagerJson: '../Content/kindeditor/kindeditor-4.1.5/asp.net/file_manager_json.ashx',
            allowFileManager: true
        };

       // $scope.reg = /^\d+/g

        $scope.show=function()
        {
            $scope.info.content = "asdfasdfasdfasdf";
          
            //var val = $scope.info.content;
            //var val2 = document.getElementById("kindEditor");
            //alert(val2);

    
        }
    }
]);