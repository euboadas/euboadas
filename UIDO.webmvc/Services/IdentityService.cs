using UIDO.webmvc.Models;
using UIDO.webmvc.Services.IServices;

namespace UIDO.webmvc.Services
{


    public class IdentityService : BaseService, IIdentityService
    {
        private readonly IHttpClientFactory _clientFactory;

        public IdentityService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<T> Authentication<T>(LoginViewModel model)
        {
            string ver = SD.IdentityAPIBase;
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = model,
                Url = SD.IdentityAPIBase + "/authentication"
            });
        }

        public async Task<T> Logout<T>(string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = null,
                AccessToken = token,
                Url = SD.IdentityAPIBase + "/logout"
            });
        }

    }
}
