using Pithline.FMS.Framework.Web.Models;
using Pithline.FMS.Framework.Web.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Pithline.FMS.Framework.Web.Controllers
{
    [EnableCors("*", "*", "*")]
    [Authorize]
    public class DataAccessController : ApiController
    {
        // GET api/da
        
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// This method would take the call object
        /// and determine what kind of call to be made to which object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// 

        public HttpResponseMessage Post(CallObject obj)
        {
            try
            {
                switch (obj.CallType)
                {
                    case "obj":
                        return this.Request.CreateResponse(HttpStatusCode.OK);
                    case "service":
                        return this.Request.CreateResponse(HttpStatusCode.OK);
                    default:
                        return DefaultProcess(obj);
                }
            }
            catch (Exception ex)
            {
                MefHelper.Error(ex);
                
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, new HttpError(ex,true));
                
               // return this.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        private HttpResponseMessage DefaultProcess(CallObject obj)
        {
            var dataManager = MefHelper.Helper.DataAccess;

            object response;
            switch (obj.Method)
            {

                case "single":
                    response = dataManager.GetSingle(obj.Target, obj.Parameters, obj.IsCached);
                    break;
                case "save":
                    response = dataManager.Save(obj.Target, obj.Parameters);
                    break;
                case "delete":
                    response = dataManager.Delete(obj.Target, obj.Parameters);
                    break;
                default:
                    response = dataManager.GetList(obj.Target, obj.Parameters, obj.IsCached);
                    break;
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
