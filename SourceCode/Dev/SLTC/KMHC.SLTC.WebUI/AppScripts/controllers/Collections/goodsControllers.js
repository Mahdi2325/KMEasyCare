/*
创建人: 刘美方
创建日期:2016-03-8
说明:物品管理
*/

angular.module("sltcApp")
    .controller("goodsListCtrl", ['$scope', '$http', '$location', 'goodsRes', 'utility', function ($scope, $http, $location, goodsRes, utility) {
        $scope.init = function () {
            $scope.Data = {};
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: goodsRes,//异步请求的res
                params: { Name: "", Type: "", No: "" },
                success: function (data) {//请求成功时执行函数
                    $scope.Data.goodses = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
            }
        };
        $scope.delete = function (goods) {
            if (goods.IsUsed)
            {
                utility.message("当前物品正在使用中，请勿删除！");
                return;
            }

            if (confirm("确定删除该信息吗?")) {
                goodsRes.delete({ id: goods.Id }, function (data) {
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                    utility.message("删除成功");
                });
            }
        };
        $scope.init();
    }])
    .controller("goodsEditCtrl", ['$scope', '$http', '$location', '$state', 'utility', 'goodsRes', function ($scope, $http, $location, $state, utility, goodsRes) {
        var id = $state.params.id;;
        $scope.init = function () {
            $scope.Data = {};
            if (id !== '0') { //修改
                goodsRes.get({ id: id }, function (data) {
                    $scope.Data.goods = data;
                });
                $scope.isAdd = false;
            } else {
                $scope.isAdd = true;
                $("#tabContent").hide();
            }
        };
        $scope.submit = function () {


            if (angular.isDefined($scope.form1.$error.required)) {
                for (var i = 0; i < $scope.form1.$error.required.length; i++) {
                    utility.msgwarning($scope.form1.$error.required[i].$name + "为必填项！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.form1.$error.maxlength)) {
                for (var i = 0; i < $scope.form1.$error.maxlength.length; i++) {
                    utility.msgwarning($scope.form1.$error.maxlength[i].$name + "超过设定长度！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }


            if (angular.isDefined($scope.form1.$error.number)) {
                for (var i = 0; i < $scope.form1.$error.number.length; i++) {
                    utility.msgwarning($scope.form1.$error.number[i].$name + "格式不正确！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }
            if (angular.isDefined($scope.form1.$error.pattern)) {
                for (var i = 0; i < $scope.form1.$error.pattern.length; i++) {
                    utility.msgwarning($scope.form1.$error.pattern[i].$name + "格式不正确！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }
            goodsRes.save($scope.Data.goods, function (data) {
                utility.message("保存成功");
                if ($scope.isAdd) {
                    $location.url("/angular/goodsList");
                };
            });
        };
        $scope.$on("goodsChange", function (event, data) {
            $scope.Data.goods.IsUsed = true;
            if (data.type === "loan") {
                if (angular.isDefined(data.amount)) {
                    $scope.Data.goods.TotalLoanAmount += data.amount;
                    $scope.Data.goods.InventoryQuantity += data.amount;
                }
                if (angular.isDefined(data.price)) {
                    $scope.Data.goods.LoanPrice = data.price;
                }

            } else {
                if (angular.isDefined(data.amount)) {
                   
                    $scope.Data.goods.TotalSaleAmount += data.amount;
                    if ($scope.Data.goods.TotalSaleAmount < 0) {
                        $scope.Data.goods.TotalSaleAmount = 0;
                    }
                    $scope.Data.goods.InventoryQuantity = $scope.Data.goods.TotalLoanAmount - $scope.Data.goods.TotalSaleAmount;
                    if ($scope.Data.goods.InventoryQuantity < 0) {
                        $scope.Data.goods.InventoryQuantity = 0;
                    }
                }

            }
            goodsRes.save($scope.Data.goods, function (data) { });
        });
        $scope.init();
    }])
    .controller("loanCtrl", ['$scope', '$http', '$location', '$state', 'utility', 'cloudAdminUi', 'loanrecordsRes', 'manufactureRes',
        function ($scope, $http, $location, $state, utility, cloudAdminUi, loanrecordsRes, manufactureRes) {
            var goodsId = $state.params.id;
            $scope.init = function () {
                cloudAdminUi.handleGoToTop();
                $scope.currentItem = {};
                $scope.Loans = {};
                //$scope.filter = { startDate: "", endDate: "" };
                $scope.CurrentPage = 1;
                $scope.currentItem = {};
                $scope.sendItem = { type: "loan", amount: 0, price: 0 };
                $scope.manufactures = manufactureRes.query();
                $scope.loanOptions = {
                    buttons: [],
                    ajaxObject: loanrecordsRes,
                    params: { goodsId: goodsId, startDate: "", endDate: "" },
                    success: function(data) {
                        $scope.Loans = data.Data;
                        //if ($scope.Loans.length > 0) {
                        //    if (angular.isDefined($scope.currentItem.Id)) {
                        //        $scope.rowSelect($scope.currentItem);
                        //    } else {
                        //        $scope.rowSelect($scope.Loans[0]);
                        //    }
                        //} else {
                        //    $scope.add();
                        //}
                    },
                    pageInfo: {
                        CurrentPage: 1,
                        PageSize: 10
                    }
                };
            };
            $scope.searchText = {};
            $scope.searchText.startDate = "";
            $scope.searchText.endDate = "";
            var page = {
                currentPage: 1,
                pageSize:10
            };
            $scope.search = function () {
                if ($scope.searchText.startDate != "" && $scope.searchText.endDate != "" && $scope.searchText.startDate > $scope.searchText.endDate) {
                    utility.msgwarning("开始日期不能大于结束日期");
                    return;
                }
                $http({
                    method: 'GET',
                    url: "/api/goodsLoan?currentPage=" + page.currentPage + "&endDate=" + $scope.searchText.endDate + "&goodsId=" + goodsId + "&pageSize=" + page.pageSize + "&startDate=" + $scope.searchText.startDate + ""
                }).then(function successCallback(response) {
                    $scope.Loans = response.data.Data;
                    //if ($scope.Loans.length > 0) {
                    //    if (angular.isDefined($scope.currentItem.Id)) {
                    //        $scope.rowSelect($scope.currentItem);
                    //    } else {
                    //        $scope.rowSelect($scope.Loans[0]);
                    //    }
                    //} else {
                    //    $scope.add();
                    //}
                }, function errorCallback(response) {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
            }
            $scope.showManufactur = function (id) {
                if ($scope.manufactures === undefined || id == null) {
                    return "";
                }
                for (var i = 0; i < $scope.manufactures.length; i++) {
                    if ($scope.manufactures[i].Id === id) {
                        return $scope.manufactures[i].Name;
                    }
                }
            };
            $scope.delete = function (id) {
                if (confirm("确定删除该信息吗?")) {
                    loanrecordsRes.delete({ id: id }, function (data) {
                        $scope.loanOptions.pageInfo.CurrentPage = 1;
                        $scope.loanOptions.search();
                        //utility.message("删除成功");
                        $scope.add();
                    });
                }
            };

            $scope.$watch("currentItem.Price", function () {
                var currentItem = $scope.currentItem;
                currentItem.Sum = currentItem.Amount * currentItem.Price;
            });

            $scope.$watch("currentItem.Amount", function () {
                var currentItem = $scope.currentItem;
                currentItem.Sum = currentItem.Amount * currentItem.Price;
            });

            $scope.createItem = function (item) {
                $scope.currentItem.GoodsId = goodsId;
                loanrecordsRes.save(item, function (data) {
                    $scope.sendItem.price = item.Price;
                    $scope.sendItem.amount = item.Amount;
                    $scope.$emit('goodsChange', $scope.sendItem);
                    $scope.currentItem = {};
                    $scope.loanOptions.pageInfo.CurrentPage = 1;
                    $scope.loanOptions.search();
                    $scope.add();
                    //utility.message("新增成功");
                });
            };

            $scope.updateItem = function (item) {
                loanrecordsRes.save(item, function (data) {
                    $scope.sendItem.price = item.Price;
                    if (angular.isDefined($scope.sendItem.amount)) {
                        $scope.sendItem.amount = item.Amount - $scope.sendItem.amount;
                    }
                    $scope.$emit('goodsChange', $scope.sendItem);
                    //utility.message("修改成功");
                    $scope.loanOptions.search();
                    $scope.add();
                });
            };
            $scope.rowSelect = function (item) {
                $scope.sendItem.amount = item.Amount;
                $scope.currentItem = item;
                $("#txtNo").focus();
            };
            $scope.saveEdit = function (item) {

                if (angular.isDefined($scope.formLoan.$error.required)) {
                    for (var i = 0; i < $scope.formLoan.$error.required.length; i++) {
                        utility.msgwarning($scope.formLoan.$error.required[i].$name + "为必填项！");
                        if (i > 1) {
                            return;
                        }
                    }
                    return;
                }

                if (angular.isDefined($scope.formLoan.$error.maxlength)) {
                    for (var i = 0; i < $scope.formLoan.$error.maxlength.length; i++) {
                        utility.msgwarning($scope.formLoan.$error.maxlength[i].$name + "超过设定长度！");
                        if (i > 1) {
                            return;
                        }
                    }
                    return;
                }


                if (angular.isDefined($scope.formLoan.$error.number)) {
                    for (var i = 0; i < $scope.formLoan.$error.number.length; i++) {
                        utility.msgwarning($scope.formLoan.$error.number[i].$name + "格式不正确！");
                        if (i > 1) {
                            return;
                        }
                    }
                    return;
                }

                if (angular.isDefined(item.Id)) {
                    $scope.updateItem(item);
                } else {
                    $scope.createItem(item);
                }
            };
            $scope.add = function () {
                $scope.sendItem = { type: "loan", amount: 0, price: 0 };
                $scope.currentItem = {};
            };
            //$scope.dateChange = function(day) {
            //    if (day == null)
            //        return;
            //    var now = new Date();
            //    $scope.currentItem.NextDate = now.dateAdd("d", day).format("yyyy-MM-dd");
            //};
            //$scope.staffSelected = function(item) {
            //    $scope.currentItem.Purchaser = item.EmpNo;
            //};
            $scope.dateChange = function () {
                if (angular.isString($scope.currentItem.NextDate) && angular.isString($scope.currentItem.LoanDate)) {
                    var indate = new Date($scope.currentItem.LoanDate);
                    var outDate = new Date($scope.currentItem.NextDate);
                    var days = indate.dateDiff('d', outDate);
                    if (days <= 0) {
                        $scope.currentItem.NextDate = "";
                        utility.message("下次进货日期不能小于本次进货日期！");
                        return;
                    }
                    $scope.currentItem.IntervalDay = days;
                }
            };

            $scope.init();

        }])
    .controller("saleCtrl", ['$scope', '$http', '$location', '$state', 'utility', 'cloudAdminUi', 'salerecordsRes', 'goodsRes',
        function ($scope, $http, $location, $state, utility, cloudAdminUi, salerecordsRes, goodsRes) {
            var goodsId = $state.params.id;
            $scope.init = function () {
                cloudAdminUi.handleGoToTop();
                $scope.Sales = {};
                $scope.currentItem = {};
                //$scope.filter = { startDate: "", endDate: "" };
                $scope.CurrentPage = 1;
                $scope.currentItem = {};
                $scope.sendItem = { type: "sale", amount: 0, price: 0 };
                //$scope.currentItem = {};
                if (goodsId !== '0') {
                    goodsRes.get({ id: goodsId }, function (data) {
                        $scope.currentGoods = data;
                     
                        //$scope.saleOptions.search();
                        //$scope.add();
                    });
                }
                $scope.saleOptions = {
                    buttons: [],
                    ajaxObject: salerecordsRes,
                    params: { goodsId: goodsId, startDate: "", endDate: "" },
                    success: function(data) {
                        $scope.Sales = data.Data;
                        //if ($scope.Sales.length > 0) {
                        //    if (angular.isDefined($scope.currentItem.Id)) {
                        //        $scope.rowSelect($scope.currentItem);
                        //    } else {
                        //        $scope.rowSelect($scope.Sales[0]);
                        //    }
                        //} else {
                        //    $scope.add();
                        //}

                        $scope.add();
                    },
                    pageInfo: {
                        CurrentPage: 1,
                        PageSize: 10
                    }
                };
               
            };
            $scope.searchText = {};
            $scope.searchText.startDate = "";
            $scope.searchText.endDate = "";
            var page = {
                currentPage: 1,
                pageSize: 10
            };
            $scope.search = function () {
                if ($scope.searchText.startDate != "" && $scope.searchText.endDate != "" && $scope.searchText.startDate > $scope.searchText.endDate) {
                    utility.msgwarning("开始日期不能大于结束日期");
                    return;
                }
                $http({
                    method: 'GET',
                    url: "/api/goodsSale?currentPage=" + page.currentPage + "&endDate=" + $scope.searchText.endDate + "&goodsId=" + goodsId + "&pageSize=" + page.pageSize + "&startDate=" + $scope.searchText.startDate + ""
                }).then(function successCallback(response) {
                    $scope.Sales = response.data.Data;
                    //if ($scope.Sales.length > 0) {
                    //    if (angular.isDefined($scope.currentItem.Id)) {
                    //        $scope.rowSelect($scope.currentItem);
                    //    } else {
                    //        $scope.rowSelect($scope.Sales[0]);
                    //    }
                    //} else {
                    //    $scope.add();
                    //}
                }, function errorCallback(response) {
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
            }
            $scope.delete = function (id) {
                if (confirm("确定删除该信息吗?")) {
                    salerecordsRes.delete({ id: id }, function (data) {
                        $scope.saleOptions.pageInfo.CurrentPage = 1;
                        $scope.saleOptions.search();
                        //utility.message("删除成功");
                        $scope.add();
                    });
                }
            };

            $scope.$watch("currentItem.Price", function () {
                var currentItem = $scope.currentItem;
                currentItem.Sum = currentItem.Amount * currentItem.Price;
            });

            $scope.$watch("currentItem.Amount", function () {
                var currentItem = $scope.currentItem;
                currentItem.Sum = currentItem.Amount * currentItem.Price;
            });

            $scope.init();
            $scope.createItem = function (item) {
                //if ($scope.currentGoods.InventoryQuantity < item.Amount) {
                //    utility.msgwarning("当前库存不足！");
                //    return;
                //}

                $scope.currentItem.GoodsId = goodsId;
                salerecordsRes.save(item, function (data) {
                    if (data.ResultCode == 1001)
                    {
                        utility.msgwarning("当前库存不足！");
                        return;
                    }

                    $scope.sendItem.price = item.Price;
                    $scope.sendItem.amount = item.Amount;
                    $scope.$emit('goodsChange', $scope.sendItem);
                    //utility.message("新增成功");
                    $scope.saleOptions.pageInfo.CurrentPage = 1;
                    $scope.saleOptions.search();
                    $scope.add();
                    $scope.currentGoods.InventoryQuantity = $scope.currentGoods.InventoryQuantity - item.Amount;
                });
            };

            $scope.updateItem = function (item) {
                //if (($scope.sendItem.amount + $scope.currentGoods.InventoryQuantity) < item.Amount) {
                //    utility.msgwarning("当前库存不足！");
                //    return;
                //}

                salerecordsRes.save(item, function (data) {
                    if (data.ResultCode == 1001) {
                        utility.msgwarning("当前库存不足！");
                        return;
                    }

                    $scope.sendItem.price = item.Price;
                    if (angular.isDefined($scope.sendItem.amount)) {
                        $scope.sendItem.amount = item.Amount - $scope.sendItem.amount;
                    }
                    $scope.$emit('goodsChange', $scope.sendItem);
                    $scope.saleOptions.pageInfo.CurrentPage = 1;
                    $scope.saleOptions.search();
                    $scope.add();
                    $scope.currentGoods.InventoryQuantity = $scope.currentGoods.InventoryQuantity - item.Amount;
                });
            };
            $scope.rowSelect = function (item) {
                $scope.sendItem.amount = item.Amount;
                $scope.currentItem = item;
                $("#txtNo").focus();
            };
            $scope.saveEdit = function (item) {

                if (angular.isDefined($scope.formSale.$error.required)) {
                    for (var i = 0; i < $scope.formSale.$error.required.length; i++) {
                        utility.msgwarning($scope.formSale.$error.required[i].$name + "为必填项！");
                        if (i > 1) {
                            return;
                        }
                    }
                    return;
                }

                if (angular.isDefined($scope.formSale.$error.maxlength)) {
                    for (var i = 0; i < $scope.formSale.$error.maxlength.length; i++) {
                        utility.msgwarning($scope.formSale.$error.maxlength[i].$name + "超过设定长度！");
                        if (i > 1) {
                            return;
                        }
                    }
                    return;
                }


                if (angular.isDefined($scope.formSale.$error.number)) {
                    for (var i = 0; i < $scope.formSale.$error.number.length; i++) {
                        utility.msgwarning($scope.formSale.$error.number[i].$name + "格式不正确！");
                        if (i > 1) {
                            return;
                        }
                    }
                    return;
                }

                if (angular.isDefined(item.Id)) {
                    $scope.updateItem(item);
                } else {
                    $scope.createItem(item);
                }
            };
            $scope.add = function () {
                $scope.sendItem = { type: "sale", amount: 0, price: 0 };
                $scope.currentItem = {};
                $scope.currentItem.GoodsId = $scope.currentGoods.id;
                $scope.currentItem.GoodsName = $scope.currentGoods.Name;
                $scope.currentItem.Price = $scope.currentGoods.SellingPrice;
            }
        }])
    .controller("storeListCtrl", ['$scope', '$http', '$location', 'goodsRes', 'utility', function ($scope, $http, $location, goodsRes, utility) {
        $scope.init = function () {
            $scope.Data = {};
            $scope.options = {
                buttons: [],//需要打印按钮时设置
                ajaxObject: goodsRes,//异步请求的res
                params: { Name: "", Type: "", No: "" },
                success: function (data) {//请求成功时执行函数
                    $scope.Data.goodses = data.Data;
                },
                pageInfo: {//分页信息
                    CurrentPage: 1, PageSize: 10
                }
            }
        };
        $scope.delete = function (goods) {
            if (goods.IsUsed) {
                utility.message("当前物品正在使用中，请勿删除！");
                return;
            }

            if (confirm("确定删除该信息吗?")) {
                goodsRes.delete({ id: goods.Id }, function (data) {
                    $scope.options.pageInfo.CurrentPage = 1;
                    $scope.options.search();
                    utility.message("删除成功");
                });
            }
        };
        $scope.init();
    }])
    .controller("storeEditCtrl", ['$scope', '$http', '$location', '$stateParams', 'utility', 'goodsRes', function ($scope, $http, $location, $stateParams, utility, goodsRes) {
        $scope.init = function () {
            $scope.Data = {};
            if ($stateParams.id) {
                goodsRes.get({ id: $stateParams.id }, function (data) {
                    $scope.Data.goods = data;
                });
                $scope.isAdd = false;
            } else {
                $scope.isAdd = true;
            }
        };
        $scope.submit = function () {
            if (angular.isDefined($scope.form1.$error.required)) {
                for (var i = 0; i < $scope.form1.$error.required.length; i++) {
                    utility.msgwarning($scope.form1.$error.required[i].$name + "为必填项！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }

            if (angular.isDefined($scope.form1.$error.maxlength)) {
                for (var i = 0; i < $scope.form1.$error.maxlength.length; i++) {
                    utility.msgwarning($scope.form1.$error.maxlength[i].$name + "超过设定长度！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }


            if (angular.isDefined($scope.form1.$error.number)) {
                for (var i = 0; i < $scope.form1.$error.number.length; i++) {
                    utility.msgwarning($scope.form1.$error.number[i].$name + "格式不正确！");
                    if (i > 1) {
                        return;
                    }
                }
                return;
            }
            goodsRes.save($scope.Data.goods, function (data) {
                $location.url("/angular/storeList");
            });
        };
        $scope.init();
    }]);
