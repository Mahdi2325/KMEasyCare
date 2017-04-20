using KMHC.SLTC.Business.Entity.Model;
using System;
using System.Web.Http;

namespace KMHC.SLTC.WebAPI
{
    [RoutePrefix("api/Contact")]
    public class ContactController : BaseController
    {
        //// GET api/resident
        //public IEnumerable<Resident> Get()
        //{
        //    return new Resident[]
        //    {
        //        new Resident() { Name = "何荣坤",Age=60,ImgUrl = "1.jpg",UserID = "11111",Bunk = "3-5",Gender = "1",Nickname = "nicktest",ServiceType = "2"},
        //        new Resident() { Name = "黄康玲",Age=70,ImgUrl = "2.jpg",UserID = "22222",Bunk = "3-5",Gender = "2",Nickname = "nicktest",ServiceType = "2"},
        //        new Resident() { Name = "胡王娇",Age=65,ImgUrl = "3.jpg",UserID = "33333",Bunk = "3-5",Gender = "2",Nickname = "nicktest",ServiceType = "2"},
        //        new Resident() { Name = "Jonh Zam",Age=55,ImgUrl = "4.jpg",UserID = "44444",Bunk = "3-5",Gender = "1",Nickname = "nicktest",ServiceType = "2"},
        //        new Resident() { Name = "Jeey Li",Age=72,ImgUrl = "5.jpg",UserID = "55555",Bunk = "3-5",Gender = "2",Nickname = "nicktest",ServiceType = "2"}
        //    };
        //}

        //[Route("")]
        //public Object Get([FromUri]Request request)
        //{
        //    Response response=new Response();
        //    response.Data= new []
        //    {
        //        new Resident() { Name = "何荣坤",Age=60,ImgUrl = "1.jpg",UserID = "11111",Bunk = "3-5",Gender = "1",Nickname = "nicktest",ServiceType = "2"},
        //        new Resident() { Name = "黄康玲",Age=70,ImgUrl = "2.jpg",UserID = "22222",Bunk = "3-5",Gender = "2",Nickname = "nicktest",ServiceType = "2"},
        //        new Resident() { Name = "胡王娇",Age=65,ImgUrl = "3.jpg",UserID = "33333",Bunk = "3-5",Gender = "2",Nickname = "nicktest",ServiceType = "2"},
        //        new Resident() { Name = "Jonh Zam",Age=55,ImgUrl = "4.jpg",UserID = "44444",Bunk = "3-5",Gender = "1",Nickname = "nicktest",ServiceType = "2"},
        //        new Resident() { Name = "Jeey Li",Age=72,ImgUrl = "5.jpg",UserID = "55555",Bunk = "3-5",Gender = "2",Nickname = "nicktest",ServiceType = "2"}
        //    };
        //    return Ok(response);
        //}

        // GET api/Contact/1
        [Route("{id:long}")]
        public IHttpActionResult Get(int id)
        {
            var contact = new Contact { CensusPostcode = "100010", CensusCity = "北京市", CensusRegion = "东城区", CensusStreet = "", IsCensusOfBeadhouse = true };
            return Ok(contact);
        }

        // POST api/resident
        public IHttpActionResult Post([FromBody] Resident obj)
        {
            try
            {
                //repository.Save(obj);
            }
            catch (Exception ex)
            {
                //LogHelper.WriteError(ex.ToString());
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        // PUT api/resident/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/resident/5
        public void Delete(int id)
        {
        }
    }
}
