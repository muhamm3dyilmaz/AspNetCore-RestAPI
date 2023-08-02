using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.ActionFilters
{
    //Hateoas için ekledik
    public class ValidateMediaTypeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //Request'te Header'da istenilen media type için (json,xml) Accept var mı diye bakar
            var acceptHeaderPresent = context.HttpContext.Request.Headers.ContainsKey("Accept");

            //Yoksa hata döner
            if (!acceptHeaderPresent)
            {
                context.Result = new BadRequestObjectResult($"Accept header is missing!");
                return;
            }

            //Varsa içeriğini alır
            var mediaType = context.HttpContext.Request.Headers["Accept"].FirstOrDefault();

            //Desteklenmeyen bir media type ise hata döner
            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? outMediaType))
            {
                context.Result = new BadRequestObjectResult($"Media type not present." 
                    + $"Please add 'Accept' header with required media type.");
                return;
            }

            context.HttpContext.Items.Add("AcceptHeaderMediaType", outMediaType);
        }
    }
}
