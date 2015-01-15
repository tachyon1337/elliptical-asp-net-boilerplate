using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EllipticalTemplate.Models;

namespace EllipticalTemplate.ApiControllers
{
    public class ContactController : ApiController
    {
        public HttpResponseMessage post(ContactViewModel contact)
        {
           
            return Request.CreateResponse<ContactViewModel>(HttpStatusCode.OK, contact);
        }
    }

    public class SubscribeController : ApiController
    {
        public HttpResponseMessage post(SubscribeViewModel contact)
        {

            return Request.CreateResponse<SubscribeViewModel>(HttpStatusCode.OK, contact);
        }
    }
}
