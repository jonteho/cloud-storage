using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CloudPhotoApp.WebApi.Controllers
{
    public class LogController : ApiController
    {
        private WorkerRoleLog _log;

        // GET api/values
        [AllowAnonymous]
        public IEnumerable<string> Get()
        {
            return WorkerRoleLog.WorkLog;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post(string message)
        {
            _log = new WorkerRoleLog();
            _log.AddMsg(message);
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
