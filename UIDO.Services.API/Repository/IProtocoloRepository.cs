using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIDO.Domain.Protocolo.DTO;

namespace UIDO.Services.API.Repository
{
    public interface IProtocoloRepository
    {
        Task<IEnumerable<ProtocoloDTO>> GetProtocolos();
        Task<ProtocoloDTO> GetProtocoloById(string nombre);
        Task<ProtocoloDTO> CreateUpdateProtocolo(ProtocoloDTO productDto);
        Task<bool> DeleteProtocolo(string nombre);
    }
}
