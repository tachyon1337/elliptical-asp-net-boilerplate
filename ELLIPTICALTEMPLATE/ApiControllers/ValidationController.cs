using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Elliptical.Mvc;
using Elliptical.Mvc.Validation;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace EllipticalTemplate.ApiControllers
{
    
    
    public class ModelValidationController : ApiController
    {
        private DataAnnotationsValidator validator { get; set; }
        public ModelValidationController()
        {
            this.validator = new DataAnnotationsValidator(true);
        }
       
        public async Task<IHttpActionResult> post(ModelValidation body)
        {
            var validator = this.validator;
            try
            {
                var result = await validator.TryValidate(body.Model, body.Type);
                var statusCode = (result.IsValid) ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
                return Content<ModelValidationSummary>(statusCode, result);
            }catch(Exception)
            {
                var errorResult = validator.ReturnInternalError(body.Model);
                return Content<ModelValidationSummary>(HttpStatusCode.InternalServerError, errorResult);
            }
           
        }
    }

    
}
