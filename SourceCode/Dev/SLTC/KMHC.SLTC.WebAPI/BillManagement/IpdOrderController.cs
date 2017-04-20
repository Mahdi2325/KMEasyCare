using KM.Common;
using KMHC.Infrastructure;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.BillManagement;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI.BillManagement
{
    [RoutePrefix("api/docOrder")]
    public class IpdOrderController : BaseController
    {
        IIpdOrderService service = IOCContainer.Instance.Resolve<IIpdOrderService>();

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int feeNo, int orderType, int confirmFlag, int checkFlag, int stopFlag, int cancelFlag, int timeFlag, DateTime startDate, DateTime endDate, int loadType, int sortType)
        {
            BaseRequest<IpdOrderFilter> request = new BaseRequest<IpdOrderFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data =
                {
                    FeeNo = feeNo,
                    OrderType = orderType,
                    ConfirmFlag = confirmFlag,
                    CheckFlag = checkFlag,
                    StopFlag = stopFlag,
                    CancelFlag=cancelFlag,
                    StartDate = startDate,
                    EndDate = endDate,
                    TimeFlag = timeFlag,
                    LoadType=loadType,
                    SortType=sortType
                }
            };
            var response = service.QueryIpdOrder(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(long orderNo)
        {
            BaseRequest<IpdOrderFilter> request = new BaseRequest<IpdOrderFilter>
            {
                Data = { OrderNo = orderNo }
            };
            var response = service.QueryIpdOrderDtl(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, long orderNo, int feeCode, string chargeGroupId, int itemType)
        {
            BaseRequest<IpdOrderFilter> request = new BaseRequest<IpdOrderFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { FeeCode = feeCode, OrderNo = orderNo, ChargeGroupId = chargeGroupId, ItemType = itemType }
            };
            var response = service.QueryChargeItem(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(int feeNo, int orderType, int confirmFlag, int checkFlag, int stopFlag, int cancelFlag, int timeFlag, DateTime startDate, DateTime endDate, int loadType)
        {
            BaseRequest<IpdOrderFilter> request = new BaseRequest<IpdOrderFilter>
            {
                Data =
                {
                    FeeNo = feeNo,
                    OrderType = orderType,
                    ConfirmFlag = confirmFlag,
                    CheckFlag = checkFlag,
                    StopFlag = stopFlag,
                    CancelFlag=cancelFlag,
                    StartDate = startDate,
                    TimeFlag = timeFlag,
                    EndDate = endDate,
                    LoadType=loadType
                }
            };
            var response = service.QueryLoadOrder(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, int feeNo, int loadType)
        {
            BaseRequest<IpdOrderFilter> request = new BaseRequest<IpdOrderFilter>
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                Data = { FeeNo = feeNo, LoadType = loadType }
            };
            var response = service.QueryOrderPostRec(request);
            return Ok(response);
        }

        [Route("")]
        public IHttpActionResult Save(IpdOrder baseRequest)
        {
            var response = service.SaveIpdOrder(baseRequest);
            return Ok(response);
        }

        [Route("saveOrders")]
        public IHttpActionResult Post(IpdOrderList baseRequest)
        {
            var response = service.SaveIpdOrderList(baseRequest);
            return Ok(response);
        }

        [Route("deleteSentOrders")]
        public IHttpActionResult Post(IpdOrder baseRequest)
        {
            var response = service.Delete(baseRequest);
            return Ok(response);
        }

        [Route("saveSendOrders")]
        public IHttpActionResult Post(NoSendIpdOrderList baseRequest)
        {
            var response = service.SaveNoSendOrders(baseRequest);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult Query(int currentPage, int pageSize, string keyWord)
        {
            BaseRequest<IpdOrderFilter> request = new BaseRequest<IpdOrderFilter>
            {
                Data = { KeyWord = keyWord }
            };
            var response = service.QueryAllChargeItem(request);
            return Ok(response);
        }
    }
}
