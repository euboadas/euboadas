using UIDO.webmvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIDO.webmvc.Services.IServices
{
    public interface IProtocoloService : IBaseService
    {
        Task<T> GetAllProtocoloAsync<T>(string token);
        Task<T> GetProtocoloByIdAsync<T>(string nombre, string token);
        Task<T> CreateProtocoloAsync<T>(ProtocoloDTO productDto, string token);
        Task<T> UpdateProtocoloAsync<T>(ProtocoloDTO productDto, string token);
        Task<T> DeleteProtocoloAsync<T>(string nombre, string token);
    }
}
