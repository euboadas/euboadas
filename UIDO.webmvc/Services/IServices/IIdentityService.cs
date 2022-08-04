using UIDO.webmvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIDO.webmvc.Services.IServices
{
    public interface IIdentityService
    {
       public Task<T> Authentication<T>(LoginViewModel model);
       public Task<T> Logout<T>(string token);
    }
}
