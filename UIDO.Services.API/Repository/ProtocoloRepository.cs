using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIDO.Database;
using UIDO.Domain.Protocolo;
using UIDO.Domain.Protocolo.DTO;

namespace UIDO.Services.API.Repository
{
    
    public class ProtocoloRepository : IProtocoloRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public ProtocoloRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProtocoloDTO> CreateUpdateProtocolo(ProtocoloDTO productDto)
        {
            Protocolo protocolo = _mapper.Map<ProtocoloDTO, Protocolo>(productDto);
            if (!string.IsNullOrEmpty(protocolo.Nombre))
            {
                _db.Protocolo.Update(protocolo);
            }
            else
            {
                _db.Protocolo.Add(protocolo);
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<Protocolo, ProtocoloDTO>(protocolo);
        }

        public async Task<bool> DeleteProtocolo(string nombre)
        {
            try{
                Protocolo protocolo = await _db.Protocolo.FirstOrDefaultAsync(u => u.Nombre == nombre);
                if (protocolo == null)
                {
                    return false;
                }
                _db.Protocolo.Remove(protocolo);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ProtocoloDTO> GetProtocoloById(string nombre)
        {
            Protocolo protocolo = await _db.Protocolo.Where(x=>x.Nombre==nombre).FirstOrDefaultAsync();
            return _mapper.Map<ProtocoloDTO>(protocolo);
        }

        public async Task<IEnumerable<ProtocoloDTO>> GetProtocolos()
        {
            List<Protocolo> protocoloList = await _db.Protocolo.ToListAsync();
            return _mapper.Map<List<ProtocoloDTO>>(protocoloList);

        }
    }
}
