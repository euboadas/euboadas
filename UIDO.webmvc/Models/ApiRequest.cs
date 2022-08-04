using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static UIDO.webmvc.SD;

namespace UIDO.webmvc.Models
{
    public class ApiRequest
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
