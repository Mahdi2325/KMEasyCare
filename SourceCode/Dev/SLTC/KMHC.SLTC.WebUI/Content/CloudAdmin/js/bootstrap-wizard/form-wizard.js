var FormWizard = function () {
    return {
        init: function () {
            if (!jQuery().bootstrapWizard) {
                return;
            }
			///*-----------------------------------------------------------------------------------*/
			///*	Show country list in Uniform style
			///*-----------------------------------------------------------------------------------*/
            //$("#country_select").select2({
            //    placeholder: "Select your country"
            //});

            var $wizform = $('[name=wizform]');
            var alert_success = $('.alert-success', $wizform);
            var alert_error = $('.alert-danger', $wizform);
            
			/*-----------------------------------------------------------------------------------*/
			/*	Validate the form elements
			/*-----------------------------------------------------------------------------------*/
            /*
            $wizform.validate({
                rules: {},

                invalidHandler: function (event, validator) {
                    alert_success.hide();
                    alert_error.show();
                },

                highlight: function (element) { 
                    $(element)
                        .closest('.form-group').removeClass('has-success').addClass('has-error'); 
                },

                unhighlight: function (element) { 
                    $(element)
                        .closest('.form-group').removeClass('has-error'); 
                },

                success: function (label) {
                    if (label.attr("for") == "gender") { 
                        label.closest('.form-group').removeClass('has-error').addClass('has-success');
                        label.remove(); 
                    } else { 
                        label.addClass('valid') 
                        .closest('.form-group').removeClass('has-error').addClass('has-success'); 
                    }
                }
            });
            */
            var element = angular.element(document.getElementById("regResidentContent"));
            var $scope = element.scope();


            //通过DOM操作获取app对象
         
            //得到app对象，可以获取app的controller
            //var controller = element.controller();
            //得到app对象，可以获取app的$scope
          
            //调用$scope中的方法
            //var appElement = document.querySelector('[ng-controller=regResidentCtrl]');
            //var $scope = angular.element(appElement).scope();
            /*-----------------------------------------------------------------------------------*/
			/*	Initialize Bootstrap Wizard
			/*-----------------------------------------------------------------------------------*/
            $('#formWizard').bootstrapWizard({
                'nextSelector': '.nextBtn',
                'previousSelector': '.prevBtn',
                onNext: function (tab, navigation, index) {
                    alert_success.hide();
                    alert_error.hide();
                    //if ($wizform.valid() == false) {
                    //    return false;
                    //}
                    var total = navigation.find('li').length;
                    var current = index + 1;
                    if (current == 2) {//保存基本信息
                        if ($scope.valid(current, alert_success, alert_error) == false) {
                            return false;
                        }
                    }
                    $('.stepHeader', $('#formWizard')).text('Step ' + (index + 1) + ' of ' + total);
                    jQuery('li', $('#formWizard')).removeClass("done");
                    var li_list = navigation.find('li');
                    for (var i = 0; i < index; i++) {
                        jQuery(li_list[i]).addClass("done");
                    }
                    if (current == 1) {
                        $('#formWizard').find('.prevBtn').hide();
                    } else {
                        $('#formWizard').find('.prevBtn').show();
                    }
                    if (current >= total) {
                        $('#formWizard').find('.nextBtn').hide();
                        $('#formWizard').find('.submitBtn').show();
                    } else {
                        $('#formWizard').find('.nextBtn').show();
                        $('#formWizard').find('.submitBtn').hide();
                    }
                    
                
                    //if (current == 4) {
                    //    $scope.saveReginsdtl();
                    //}
                },
                onPrevious: function (tab, navigation, index) {
                    alert_success.hide();
                    alert_error.hide();
                    var total = navigation.find('li').length;
                    var current = index + 1;
                    $('.stepHeader', $('#formWizard')).text('Step ' + (index + 1) + ' of ' + total);
                    jQuery('li', $('#formWizard')).removeClass("done");
                    var li_list = navigation.find('li');
                    for (var i = 0; i < index; i++) {
                        jQuery(li_list[i]).addClass("done");
                    }
                    if (current == 1) {
                        $('#formWizard').find('.prevBtn').hide();
                    } else {
                        $('#formWizard').find('.prevBtn').show();
                    }
                    if (current >= total) {
                        $('#formWizard').find('.nextBtn').hide();
                        $('#formWizard').find('.submitBtn').show();
                    } else {
                        $('#formWizard').find('.nextBtn').show();
                        $('#formWizard').find('.submitBtn').hide();
                    }
                },
				onTabClick: function (tab, navigation, index) {
                    //bootbox.alert('On Tab click is disabled');
                    return false;
                },
                onTabShow: function (tab, navigation, index) {
                    var total = navigation.find('li').length;
                    var current = index + 1;
                    var $percent = (current / total) * 100;
                    $('#formWizard').find('.progress-bar').css({
                        width: $percent + '%'
                    });
                }
            });

            $('#formWizard').find('.prevBtn').hide();
            $('#formWizard .submitBtn').unbind('click').click(function () {
                if ($scope.saveIpdreg()) {
                    $(this).hide();
                } else {
                    $(this).show();
                }
                
            }).hide();
        }
    };
}();