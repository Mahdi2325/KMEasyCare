using KM.Common;
using KMHC.SLTC.Business.Entity;
using KMHC.SLTC.Business.Entity.Base;
using KMHC.SLTC.Business.Entity.Filter;
using KMHC.SLTC.Business.Entity.Model;
using KMHC.SLTC.Business.Interface;
using System;
using System.Web.Http;
using KMHC.Infrastructure;
using System.Collections.Generic;
using KMHC.SLTC.Business.Entity.NursingWorkstation;
using System.Web;
using System.IO;
using System.Drawing;

namespace KMHC.SLTC.WebAPI.APP
{
    [RoutePrefix("api/lookover")]
    public class LookOverController : BaseController
    {
        INursingWorkstationService service = IOCContainer.Instance.Resolve<INursingWorkstationService>();

        [Route(""), HttpGet]
        public IHttpActionResult GetLookOverList(string FloorId="", [FromUri]DateTime? date = null, int currentPage = 1, int pageSize = 10)
        {
            BaseRequest<LookOverModel> request = new BaseRequest<LookOverModel>();
            request.CurrentPage = currentPage;
            request.PageSize = pageSize;
            request.Data.FloorId = FloorId;
            request.Data.LookOverTime = date;
            var response = service.GetLookOverList(request);
            return Ok(response);
        }

        [Route(""), HttpGet]
        public IHttpActionResult GetLookOverById(int id)
        {
            var response = service.GetLookOverById(id);
            return Ok(response);
        }

        [Route(""), HttpPost]
        public IHttpActionResult SaveLookOver([FromBody]LookOverModel request,[FromUri]string from="PC")
        {
            var response = new BaseResponse<string>();
            request.LookOverTime = DateTime.Now;
            if (from=="APP")
            {
                String ymd = DateTime.Now.ToString("yyyyMM", System.Globalization.DateTimeFormatInfo.InvariantInfo);
                string rootPath = HttpContext.Current.Server.MapPath("~/Uploads/LookoverPhotos/" + ymd+"/");
                if (Directory.Exists(rootPath) == false)
                {
                    Directory.CreateDirectory(rootPath);
                }

                var index = 0;
                string fileName = "";
                string path = "";
                List<string> pathArr = new List<string>();

                String newFileName = DateTime.Now.ToString("ddHHmmssffff", System.Globalization.DateTimeFormatInfo.InvariantInfo);

                foreach (var photoPath in request.PhotoList)
                {
                    fileName = string.Format(@"{0}_{1}.jpg", newFileName, index);
                    path = string.Format(@"{0}/{1}", rootPath, fileName);

                    try
                    {
                        byte[] arr = Convert.FromBase64String(photoPath);
                        using (MemoryStream ms = new MemoryStream(arr))
                        {
                            using (Bitmap bmp = new Bitmap(ms))
                            {
                                bmp.Save(path, System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
                        pathArr.Add(string.Format(@"/Uploads/LookoverPhotos/{0}/{1}", ymd,fileName));
                        index++;
                    }
                    catch (Exception ex)
                    {
                        response.Data = null;
                        response.ResultMessage = ex.Message;
                        response.IsSuccess = false;
                    }
                }

                request.LookOverPhotos = pathArr.Join(";");
            }
            response = service.SaveLookOver(request);
            return Ok(response);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            var response = service.DeleteLookOver(id);
            return Ok(response);
        }
    }
}
