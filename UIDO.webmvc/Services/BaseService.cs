using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using UIDO.webmvc.Models;
using UIDO.webmvc.Services.IServices;

namespace UIDO.webmvc.Services
{
    public class BaseService : IBaseService
    {
        public ResponseDto responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new ResponseDto();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("UIDOAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);
                client.DefaultRequestHeaders.Clear();
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8,"application/json");
                }

                if (!string.IsNullOrEmpty(apiRequest.AccessToken))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.AccessToken);
                }

                HttpResponseMessage apiResponse = null;
                switch (apiRequest.ApiType)
                {
                    case SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default :
                        message.Method = HttpMethod.Get;
                        break;
                }
                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);


                // fix temporal
                if (apiRequest.Url.Contains("authentication"))
                {
                    dynamic aux = JsonConvert.DeserializeObject<T>(apiContent);
                    var auxToken = aux.Result;
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auxToken);
                }



                return apiResponseDto;

            }
            catch(Exception e)
            {
                var dto = new ResponseDto
                {
                    DisplayMessage = "Error",
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
                return apiResponseDto;
            }
        }

        public async Task<T> PostAsync<T>(ApiRequest apiRequest)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(
                    System.Text.Json.JsonSerializer.Serialize(apiRequest.Data),
                    Encoding.UTF8,
                    "application/json"
                );

               

                var apiResponse = await client.PostAsync(apiRequest.Url, content);

                if (!apiResponse.IsSuccessStatusCode)
                {
                    //HasInvalidAccess = true;
                    //return Page();
                    var apiContent = await apiResponse.Content.ReadAsStringAsync();
                    var apiResponseDto = JsonConvert.DeserializeObject<T>(apiContent);
                    return apiResponseDto;
                }
                else
                {
                    var dto = new ResponseDto
                    {
                        DisplayMessage = "Error",
                        ErrorMessages = new List<string> { Convert.ToString("error!") },
                        IsSuccess = false
                    };
                    var res = JsonConvert.SerializeObject(dto);
                    var apiResponseDto = JsonConvert.DeserializeObject<T>(res);
                    return apiResponseDto;
                }


            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(true);
        }
    }
}
