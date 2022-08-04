using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Net.Http;

namespace UIDO.webmvc.Config
{
    public static class HttpClientTokenExtension
    {
        //public static string AddBearerToken(this HttpClient client, IHttpContextAccessor context) 
        //{
        //    //if (context.HttpContext.User.Identity.IsAuthenticated && context.HttpContext.Request.Headers.ContainsKey("Authorization"))
        //    //{

        //    string token = "";

        //    foreach (var c in context.HttpContext.User.Claims)
        //    {
        //      if (c.Type.Equals("accces_token"))
        //            token = c.Value;
        //    }


        //        //var token = context.HttpContext.Request.Headers["Authorization"].ToString();

        //        if (!string.IsNullOrEmpty(token))
        //        {
        //            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token);
        //        }
        //    //}

        //        return token;
        //}

        public static string  GetToken(HttpContext context)
        {
            string token = "";

            foreach (var c in context.User.Claims)
            {
                if (c.Type.Equals("access_token"))
                    token = c.Value;
            }

            return token;



        }

        public static string GetName(HttpContext context)
        {
            string username =  context.User.Claims.FirstOrDefault(c => c.Type.Equals("username")).Value;

            //foreach (var c in context.User.Claims)
            //{
            //    if (c.Type.Equals("username"))
            //        username = c.Value;

                
            //}

            return username;



        }

    }
}
