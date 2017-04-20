using System.Web;
using System.Web.Optimization;

namespace KMHC.SLTC.WebUI
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //加载首页样式
            bundles.Add(new StyleBundle("~/Content/Index/css").Include(
                        "~/Content/CloudAdmin/css/responsive.css"
                        , "~/Content/CloudAdmin/css/animatecss/animate.min.css" // ANIMATE
                        , "~/Content/CloudAdmin/js/jquery-todo/css/styles.css" // TODO
                        , "~/Content/CloudAdmin/js/fullcalendar/fullcalendar.min.css" // FULL CALENDAR
                        , "~/Content/CloudAdmin/js/gritter/css/jquery.gritter.css" // GRITTER
                        , "~/Content/CloudAdmin/js/bootstrap-switch/bootstrap-switch.min.css" // 
                        , "~/Content/user.css" // 
                        //, "~/Content/CloudAdmin/js/datatables/media/css/jquery.dataTables.min.css" // DATA TABLES
                        , "~/Content/CloudAdmin/js/datatables/media/assets/css/datatables.min.css" // 
                        , "~/Content/CloudAdmin/js/datatables/extras/TableTools/media/css/TableTools.min.css" // 
                        , "~/Content/CloudAdmin/js/tablecloth/css/tablecloth.min.css" // TABLE CLOTH
                        , "~/Content/CloudAdmin/js/select2/select2.min.css" // 
                        , "~/Content/CloudAdmin/js/jquery-ui-1.10.3.custom/css/custom-theme/jquery-ui-1.10.3.custom.min.css" // JQUERY UI
                        , "~/Content/CloudAdmin/js/hubspot-messenger/css/messenger.min.css" // HUBSPOT MESSENGER
                        , "~/Content/CloudAdmin/js/hubspot-messenger/css/messenger-spinner.min.css" // 
                        , "~/Content/CloudAdmin/js/hubspot-messenger/css/messenger-theme-future.min.css" // 
                        , "~/Content/CloudAdmin/js/hubspot-messenger/css/messenger-theme-block.min.css" // 
                        , "~/Content/CloudAdmin/js/uniform/css/uniform.default.min.css" // 
                        , "~/Content/CloudAdmin/js/bootstrap-treeview/bootstrap-treeview.min.css" // TREE VIEW
                        , "~/Content/CloudAdmin/js/magic-suggest/magicsuggest-2.1.4-min.css" // MAGIC SUGGEST
                        , "~/Content/CloudAdmin/js/select2/select2.min.css" // select2
                        , "~/Content/loading-bar/loading-bar.min.css"
                        , "~/Content/fullcalendar-2.9.1/fullcalendar.css"
                        , "~/Content/bootstrap-dialog/bootstrap-dialog.min.css"
                //, "http://fonts.useso.com/css?family=Open+Sans:300,400,600,700"
                        ));

            //加载首页JS
            bundles.Add(new ScriptBundle("~/Content/Index/js").Include(
                        "~/Content/CloudAdmin/js/jquery/jquery-2.0.3.min.js" // JQUERY
                        , "~/Content/CloudAdmin/js/jquery-ui-1.10.3.custom/js/jquery-ui-1.10.3.custom.min.js" // JQUERY UI
                        , "~/Content/CloudAdmin/bootstrap-dist/js/bootstrap.js" // BOOTSTRAP
                        , "~/Content/CloudAdmin/js/jQuery-slimScroll-1.3.0/jquery.slimscroll.min.js" // SLIMSCROLL
                        , "~/Content/CloudAdmin/js/jQuery-slimScroll-1.3.0/slimScrollHorizontal.min.js" // 
                        , "~/Content/CloudAdmin/js/jQuery-BlockUI/jquery.blockUI.min.js" // BLOCK UI
                        , "~/Content/CloudAdmin/js/sparklines/jquery.sparkline.min.js" // SPARKLINES
                        , "~/Content/CloudAdmin/js/jquery-easing/jquery.easing.min.js" // EASY PIE CHART
                        , "~/Content/CloudAdmin/js/easypiechart/jquery.easypiechart.min.js" // 
                //, "~/Content/CloudAdmin/js/flot/jquery.flot.min.js" // FLOT CHARTS
                //, "~/Content/CloudAdmin/js/flot/jquery.flot.time.min.js" // 
                //, "~/Content/CloudAdmin/js/flot/jquery.flot.selection.min.js" // 
                //, "~/Content/CloudAdmin/js/flot/jquery.flot.resize.min.js" // 
                //, "~/Content/CloudAdmin/js/flot/jquery.flot.pie.min.js" // 
                //, "~/Content/CloudAdmin/js/flot/jquery.flot.stack.min.js" // 
                //, "~/Content/CloudAdmin/js/flot/jquery.flot.crosshair.min.js" // 
                        , "~/Content/CloudAdmin/js/jquery-todo/js/paddystodolist.js" // TODO
                        , "~/Content/CloudAdmin/js/timeago/jquery.timeago.js" // TIMEAGO
                        , "~/Content/CloudAdmin/js/fullcalendar/fullcalendar.js" // FULL CALENDAR
                        , "~/Content/CloudAdmin/js/datatables/media/js/jquery.dataTables.js" // DATA TABLES
                        , "~/Content/CloudAdmin/js/datatables/media/assets/js/datatables.min.js" // 
                        , "~/Content/CloudAdmin/js/datatables/extras/TableTools/media/js/TableTools.js" // 
                        , "~/Content/CloudAdmin/js/datatables/extras/TableTools/media/js/ZeroClipboard.min.js" // 
                        , "~/Content/CloudAdmin/js/jQuery-Cookie/jquery.cookie.min.js" // COOKIE
                        , "~/Content/CloudAdmin/js/gritter/js/jquery.gritter.min.js" // GRITTER
                        , "~/Content/CloudAdmin/js/select2/select2.min.js" // 
                        , "~/Content/CloudAdmin/js/tablecloth/js/jquery.tablecloth.js" // TABLE CLOTH
                        , "~/Content/CloudAdmin/js/tablecloth/js/jquery.tablesorter.js" // 
                        , "~/Content/CloudAdmin/js/jQuery-slimScroll-1.3.0/jquery.slimscroll.min.js" // 
                        , "~/Content/CloudAdmin/js/jQuery-slimScroll-1.3.0/slimScrollHorizontal.min.js" // 
                        , "~/Content/CloudAdmin/js/bootstrap-switch/bootstrap-switch.min.js" // 
                        , "~/Content/CloudAdmin/js/bootbox/bootbox.min.js" // BOOTBOX
                        , "~/Content/CloudAdmin/js/hubspot-messenger/js/messenger.min.js" // HUBSPOT MESSENGER
                        , "~/Content/CloudAdmin/js/hubspot-messenger/js/messenger-theme-flat.js" // 
                        , "~/Scripts/paging.js" // 
                        , "~/Scripts/jquery.mask.min.js" // 
                        , "~/Content/CloudAdmin/js/uniform/jquery.uniform.min.js" // UNIFORM
                        , "~/Content/CloudAdmin/js/jquery-validate/jquery.validate.js" // 
                        , "~/Content/CloudAdmin/js/jquery-validate/additional-methods.js" // 
                        , "~/Content/CloudAdmin/js/bootstrap-treeview/bootstrap-treeview.min.js" // TREE VIEW
                        , "~/Content/CloudAdmin/js/magic-suggest/magicsuggest-2.1.4-min.js" // MAGIC SUGGEST
                        , "~/Content/CloudAdmin/js/select2/select2.min.js" // select2
                        , "~/Scripts/highcharts/highcharts.js"
                        , "~/Scripts/highcharts/modules/exporting.js"
                        , "~/Content/fullcalendar-2.9.1/lib/moment.min.js"
                        , "~/Content/fullcalendar-2.9.1/fullcalendar.min.js"
                        , "~/Content/fullcalendar-2.9.1/lang/zh-cn.js"
                        , "~/Content/CloudAdmin/js/bootstrap-wizard/jquery.bootstrap.wizard.js"
                        , "~/Content/lodash-3.10.1/lodash.min.js"
                        , "~/Content/kindeditor/kindeditor-4.1.5/kindeditor-all-min.js"
                        , "~/Content/kindeditor/kindeditor-4.1.5/lang/zh-CN.js"
                        , "~/Content/bootstrap-dialog/bootstrap-dialog.min.js"
                         , "~/Content/echart-3.3.1/echarts.min.js" 
                //, "~/Content/CloudAdmin/js/isotope/jquery.isotope.js" // ISOTOPE
                //, "~/Content/CloudAdmin/js/isotope/imagesloaded.pkgd.js" // 
                        ));

            //加载angular框架库
            bundles.Add(new ScriptBundle("~/Scripts/angular").Include(
                        "~/Scripts/angular.js"
                        , "~/Scripts/angular-ui-router.js"
                        , "~/Scripts/angular-resource.js"
                        , "~/Scripts/angular-animate.js"
                        , "~/Scripts/angular-cookies.js"
                        , "~/Scripts/angular-messages.js"
                        , "~/Content/loading-bar/loading-bar.min.js"
                        , "~/Content/kindeditor/angular-kindeditor.js"
                        ));

            //加载公用脚本
            bundles.Add(new ScriptBundle("~/Scripts/AppCommon").Include(
                        "~/AppScripts/common/*.js"
                        , "~/AppScripts/components/inputChargeItem/*.js"
                        , "~/AppScripts/components/inputResident/*.js"
                        , "~/AppScripts/components/selectCode/*.js"
                        , "~/AppScripts/components/selectItems/*.js"
                //, "~/AppScripts/components/inputStaff/*.js"
                        , "~/AppScripts/components/inputStaff2/*.js"
                        , "~/AppScripts/components/inputStaff3/*.js"
                        , "~/AppScripts/components/inputOrg/*.js"
                        , "~/AppScripts/components/inputBed/*.js"
                        , "~/AppScripts/components/inputICD9/*.js"
                        , "~/AppScripts/components/residentCard/*.js"
                        , "~/AppScripts/components/inputResident/*.js"
                        , "~/AppScripts/components/inputResidentCheck/*.js"
                        , "~/AppScripts/components/residentList/*.js"
                        , "~/AppScripts/components/selectMultipleCode/*.js"
                        , "~/AppScripts/components/selectMultipleValue/*.js"
                        , "~/AppScripts/components/inputSelect/*.js"
                        , "~/AppScripts/components/DC/selectDCode/*.js"
                        , "~/AppScripts/components/inputFreq/*.js"
                        , "~/AppScripts/components/inputPostCode/*.js"
                         , "~/AppScripts/components/LookUp/*.js"
                       ));

            //加载长照业务
            bundles.Add(new ScriptBundle("~/Scripts/LC").Include(
                        "~/AppScripts/home/LCApp.js"
                        , "~/AppScripts/route/LCRoute.js"
                        , "~/AppScripts/controllers/EVM/*.js"
                        , "~/AppScripts/controllers/resident/*.js"
                        , "~/AppScripts/socialworker/*.js"
                        , "~/AppScripts/controllers/Collections/*.js"
                        , "~/AppScripts/controllers/OrganizationManage/*.js"
                        , "~/AppScripts/carePlans/*.js"
                        , "~/AppScripts/nursing/*.js"
                        , "~/AppScripts/KPI/*.js"
                        , "~/AppScripts/TSG/*.js"
                        , "~/AppScripts/Report/*.js"
                        , "~/Content/angular-file-upload/angular-file-upload-shim.js"
                        , "~/Content/angular-file-upload/angular-file-upload.js"
                        , "~/AppScripts/LTC_Report/*.js"
                        , "~/AppScripts/controllers/chargeInput/*.js"
                         , "~/AppScripts/controllers/PackageRelated/*.js"
                        , "~/AppScripts/controllers/chargeItemSetting/*.js"
                        , "~/AppScripts/controllers/billManagement/*.js"
                        , "~/AppScripts/controllers/financialManagement/*.js"
                        , "~/AppScripts/controllers/ServiceDeposit/*.js"
                        , "~/AppScripts/controllers/MedicalWork/*.js"
                        , "~/AppScripts/controllers/receptionManagement/*.js"
                        ));

            //加载日照脚本
            bundles.Add(new ScriptBundle("~/Scripts/DC").Include(
                        "~/AppScripts/home/DCApp.js"
                        , "~/AppScripts/route/DCRoute.js"
                        , "~/AppScripts/controllers/Collections/indexCtrl.js"
                        , "~/AppScripts/controllers/Collections/*.js"
                        , "~/AppScripts/DC/NurseCare/*.js"
                        , "~/AppScripts/DC/Resident/*.js"
                        , "~/AppScripts/DC/SocialWorker/*.js"
                        , "~/AppScripts/DC/SysAdmin/*.js"
                        , "~/AppScripts/DC/CrossSpeciality/*.js"
                        , "~/AppScripts/DC/FamilyDoctor/*.js"
                         , "~/AppScripts/DC/CasesWorkStation/*.js"
                       ));
        }
    }
}
