using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UIDO.webmvc;
using UIDO.webmvc.Config;
using UIDO.webmvc.Models;
using UIDO.webmvc.Services;
using UIDO.webmvc.Services.IServices;

namespace UIDO.webmvc.Services
{
    public class ProtocoloService : BaseService, IProtocoloService
    {
        private readonly IHttpClientFactory _clientFactory;


        public ProtocoloService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<T> CreateProtocoloAsync<T>(ProtocoloDTO protocoloDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = protocoloDto,
                Url = SD.ProtocoloAPIBase + "/v1/protocolo",
                AccessToken = token
            });
        }

        public async Task<T> DeleteProtocoloAsync<T>(string name, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.ProtocoloAPIBase + "/v1/protocolo/"+name,
                AccessToken = token
            });
        }

        public async Task<T> GetAllProtocoloAsync<T>(string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProtocoloAPIBase + "/v1/protocolo",
                AccessToken = token
            });
        }

        public async Task<T> GetProtocoloByIdAsync<T>(string name, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.ProtocoloAPIBase + "/vi/protocolo/"+name,
                AccessToken = token
            });
        }

        public async Task<T> UpdateProtocoloAsync<T>(ProtocoloDTO protocoloDto, string token)
        {
            return await this.SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = protocoloDto,
                Url = SD.ProtocoloAPIBase + "/vi/protocolo",
                AccessToken = token
            });
        }
    }
}
