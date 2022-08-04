using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIDO.webmvc
{
    public static class SD
    {
        public static string IdentityAPIBase { get; set; }
        public static string ProtocoloAPIBase { get; set; }
        public static string ProductAPIBase { get; set; }
        public static string ShoppingCartAPIBase { get; set; }
        public static string CouponAPIBase { get; set; }
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
