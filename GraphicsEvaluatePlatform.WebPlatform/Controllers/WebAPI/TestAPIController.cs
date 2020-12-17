using GraphicsEvaluatePlatform.Model;
using System.Collections.Generic;
using System.Web.Http;

namespace GraphicsEvaluatePlatform.WebPlatform.Controllers.WebAPI
{
    public class TestAPIController : ApiController
    {
        [HttpPost]
        public string Test([FromBody] dynamic obj)
        {
            var re = obj;

            return "";
        }

        [HttpGet]
        public string TestSend()
        {
            string message = "";
            BootstrapPager pager = new BootstrapPager();
            pager.PageIndex = 1;
            pager.PageSize = 100;
            pager.filter = ",UID:";
           // var data = UserService.GetUserList(pager);
            LetsChat.notify("来自后台数据啊！");
            SignalrServerToClient.BroadcastMessage("来自后台数据哈！");
            return "";
        }

        // GET: api/TestAPI
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/TestAPI/5

        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TestAPI
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/TestAPI/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/TestAPI/5
        public void Delete(int id)
        {
        }
    }
}
