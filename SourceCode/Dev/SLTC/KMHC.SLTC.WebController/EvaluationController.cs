namespace KMHC.SLTC.WebController
{
    using KM.Common;
    using KMHC.SLTC.Business.Entity.Base;
    using KMHC.SLTC.Business.Entity.Model;
    using KMHC.SLTC.Business.Interface;
    using Newtonsoft.Json;
    using System;
    using System.Web.Mvc;

    public class EvaluationController : Controller
    {
        private INursingManageService nursingSvc = IOCContainer.Instance.Resolve<INursingManageService>();

        public ActionResult CalcResult()
        {
            string str = base.Request["question"];
            if (string.IsNullOrWhiteSpace(str))
            {
                BaseResponse<string> response = new BaseResponse<string>
                {
                    ResultCode = -1,
                    ResultMessage = "参数为空"
                };
                return base.Content(JsonConvert.SerializeObject(response));
            }
            try
            {
                Calculation calculation = this.nursingSvc.CalcEvaluation((QUESTION)JsonConvert.DeserializeObject(str, typeof(QUESTION)));
                return base.Content(JsonConvert.SerializeObject(calculation));
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception.ToString());
                BaseResponse<string> response2 = new BaseResponse<string>
                {
                    ResultCode = -1,
                    ResultMessage = "操作异常"
                };
                return base.Content(JsonConvert.SerializeObject(response2));
            }
        }

        public ActionResult Delete(int recId)
        {
            try
            {
                BaseResponse response = this.nursingSvc.DeleteEvaluation((long)recId);
                return base.Content(JsonConvert.SerializeObject(response));
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception.ToString());
                BaseResponse<string> response2 = new BaseResponse<string>
                {
                    ResultCode = -1,
                    ResultMessage = "操作异常"
                };
                return base.Content(JsonConvert.SerializeObject(response2));
            }
        }

        public ActionResult Record()
        {
            string str = base.Request["question"];
            if (string.IsNullOrWhiteSpace(str))
            {
                BaseResponse<string> response = new BaseResponse<string>
                {
                    ResultCode = -1,
                    ResultMessage = "参数为空"
                };
                return base.Content(JsonConvert.SerializeObject(response));
            }
            try
            {
                BaseResponse response2 = this.nursingSvc.SaveQuetion((REGQUESTION)JsonConvert.DeserializeObject(str, typeof(REGQUESTION)));
                return base.Content(JsonConvert.SerializeObject(response2));
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception.ToString());
                BaseResponse<string> response3 = new BaseResponse<string>
                {
                    ResultCode = -1,
                    ResultMessage = "操作异常"
                };
                return base.Content(JsonConvert.SerializeObject(response3));
            }
        }
    }
}

