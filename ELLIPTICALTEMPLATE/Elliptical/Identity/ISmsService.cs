using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Elliptical.Mvc.Identity
{
    public interface ISmsService : IIdentityMessageService
    {
        string Id { get; }
    }
}
