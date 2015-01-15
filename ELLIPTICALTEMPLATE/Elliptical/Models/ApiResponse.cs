using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Elliptical.Mvc
{
    public class ApiResponse<T>
    {
        public string Message { get; set; }
        public T Model { get; set; }
    }
}