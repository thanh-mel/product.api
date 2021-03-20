using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Product.API.Helpers;
using System;
using System.Net;
using System.Net.Http;

namespace Product.API.Filters
{
    /// <summary>
    /// Return an ETag which contains a hash of the requested object. 
    /// This ETag will then be used along with If-Match to prevent the LOST UPDATE issue.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class ETagFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            if (request.Method == HttpMethod.Get.Method && response.StatusCode == (int)HttpStatusCode.OK)
            {
                var result = context.Result as OkObjectResult;                
                var etag = HashFactory.GetHashSHA256(JsonConvert.SerializeObject(result?.Value));
                if (request.Headers.Keys.Contains(HeaderNames.IfMatch))
                {
                    var incomingEtag = request.Headers[HeaderNames.IfMatch];

                    // If both the etags are equal
                    // Return a 304 Not Modified Response
                    if (incomingEtag.Equals(etag))
                    {
                        context.Result = new StatusCodeResult((int)HttpStatusCode.NotModified);
                    }
                }

                // Add ETag response header 
                response.Headers.Add(HeaderNames.ETag, etag);
            }

            base.OnActionExecuted(context);
        }
    }
}
