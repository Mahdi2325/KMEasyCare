angular.module("sltcApp")
.controller('evalPersonCtrl', ['$scope', '$http', '$location', '$state', 'dictionary', 'utility', 'evalPersonRes',
    function ($scope, $http, $location, $state, dictionary, utility, evalPersonRes) {
        var id = $state.params.id;
        $scope.init = function () {
            $(".datepicker").datepicker({
                dateFormat: "yy-mm-dd",
                changeMonth: true,
                changeYear: true,
                maxDate: "0d"
            });

            var dicType = ["Gender", "ResponseSkills"];
            dictionary.get(dicType, function (dic) {
                $scope.Dic = dic;
                for (var name in dicType) {
                    if ($scope.Dic.hasOwnProperty(name)) {
                        if ($scope.Dic[name][0] != undefined) {
                            $scope.Data[name] = $scope.Dic[name][0].Value;
                        }
                    }
                }
            });
        }
        $scope.init();//初始化
        //tab改变保存
        $('#tabEvaluate li').on('click', function () {
  
            if ($scope.PreTab !== "") {

            }
            $scope.PreTab = $(this).children("a").attr("ui-sref");

        });
    }
])
.controller('evalPersonListCtrl', function ($scope, $http, $state, $location, evalPersonRes) {
    $scope.Data = {};
    evalPersonRes.query({}, function (data) {
        
        $scope.Data.persons = data;
    
    });
    $scope.search = $scope.reload = function () {
        evalPersonRes.query({ Name: $scope.keyword }, function (data) {
            $scope.Data.persons = data;
        })
    }
})
.controller("psychologyInfoCtrl", ['$scope', '$http', '$location', '$state', 'utility', 'dictionary', 'evalPersonRes',
    function ($scope, $http, $location, $state,utility, dictionary, evalPersonRes) {
        $scope.Data = {};
        if ($state.params.id) {
            
            evalPersonRes.get({ id: $state.params.id }, function (data) {
                
                $scope.Data.psychologyPerson = data;
            });
            $scope.isAdd = false;
        }
        else {
            $scope.isAdd = true;
        }
        
            //加载评估人员字典列表
            $scope.Data.Evaluators = [{ value: "张三", text: "张三" }, { value: "李四", text: "李四" }, { value: "王五", text: "王五" }, { value: "赵六", text: "赵六" }];
            //意识状态
            $scope.Data.MindState = [{ value: "1", text: "正常" }, { value: "2", text: "清醒" }, { value: "3", text: "混乱" }];
            //加载障碍等级字典
            $scope.Data.BookDegree = [{ value: "中度", text: "中度" }, { value: "多障/重度", text: "多障/重度" }, { value: "多障/极重度", text: "多障/极重度" }, { value: "重度", text: "重度" }, { value: "智障/重度", text: "智障/重度" }, { value: "智障/极重度", text: "智障/极重度" }, { value: "极重度", text: "极重度" }, { value: "精障/重度", text: "精障/重度" }, { value: "轻度", text: "轻度" }];
            //口语表达能力字典
            $scope.Data.ExpressionState = [{ value: "正常", text: "正常" }, { value: "其他", text: "其他" }, { value: "单字", text: "单字" }, { value: "模仿", text: "模仿" }, { value: "整句", text: "整句" }];
            //非口语表达能力
            $scope.Data.NonExpressionState = [{ value: "其他", text: "其他" }, { value: "肢体动作", text: "肢体动作" }, { value: "眼神", text: "眼神" }, { value: "脸部表情", text: "脸部表情" }];
            //语言理解能力
            $scope.Data.LanguageState = [{ value: "不佳", text: "不佳" }, { value: "佳", text: "佳" }, { value: "普通", text: "普通" }];
            //情绪状况
            $scope.Data.EmotionState = [{ value: "稳定", text: "稳定" }, { value: "一般", text: "一般" }, { value: "沿称稳定", text: "沿称稳定" }, { value: "不稳定", text: "不稳定" }];
            //外显人格
            $scope.Data.Personality = [{ value: "成熟型", text: "成熟型" }, { value: "自怨自艾型", text: "自怨自艾型" }, { value: "沉默型", text: "沉默型" }, { value: "防卫型", text: "防卫型" }, { value: "其他", text: "其他" }, { value: "绝望型", text: "绝望型" }, { value: "边缘型(冲动,易怒)", text: "边缘型(冲动,易怒)" }];
            //注意力
            $scope.Data.Attention = [{ value: "易分心", text: "易分心" }, { value: "无法测知", text: "无法测知" }, { value: "集中", text: "集中" }];
            //现实感
            $scope.Data.Realisticsense = [{ value: "佳", text: "佳" }, { value: "差", text: "差" }, { value: "普通", text: "普通" }, { value: "无法测知", text: "无法测知" }];
            //社会参与度
            $scope.Data.SocialParticipation = [{ value: "抗拒", text: "抗拒" }, { value: "其他", text: "其他" }, { value: "活跃", text: "活跃" }, { value: "被动", text: "被动" }, { value: "普通", text: "普通" }];
            //社交态度
            $scope.Data.SocialAttitude = [{ value: "沉默", text: "沉默" }, { value: "其他", text: "其他" }, { value: "封闭", text: "封闭" }, { value: "被动", text: "被动" }, { value: "普通", text: "普通" }, { value: "积极主动", text: "积极主动" }];
            //社交能力
            $scope.Data.SocialSkills = [{ value: "佳,会主动结交朋友", text: "佳,会主动结交朋友" }, { value: "其他", text: "其他" }, { value: "习惯依赖他人", text: "习惯依赖他人" }, { value: "普通", text: "普通" }, { value: "5", text: "独来独往" }];
            //沟通技巧
            $scope.Data.CommSkills = [{ value: "佳", text: "佳" }, { value: "差", text: "差" }, { value: "普通", text: "普通" }];
            $scope.Data.ResponseSkills = [{ value: "佳", text: "佳" }, { value: "差", text: "差" }, { value: "普通", text: "普通" }];
            //社交-解决问题能力
            $scope.Data.FixissueSkills = [{ value: "佳", text: "佳" }, { value: "差", text: "差" }, { value: "普通", text: "普通" }];
            //协助输入问题
            //$scope.Data.FixssueSkills = [{ value: "1", text: "佳" }, { value: "2", text: "差" }, { value: "3", text: "普通" }];
            $(".datepicker").datepicker({
                dateFormat: "yy-mm-dd",
                changeMonth: true,
                changeYear: true
            });
        
       
        //下次评估存档操作
        $scope.saveEvaluate = function () {
            //if ($scope.Data.psychologyPerson.NextEvalDate !== "") {
            //    alert("存档成功!");
            //}
            evalPersonRes.save($scope.Data.psychologyPerson, function (data) {
                alert("资料保存成功!");
                //$location.url('EvaluateAdd.PsychologyInfo');

            })
        }
    }
]);

