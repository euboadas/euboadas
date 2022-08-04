using AutoMapper;
using UIDO.Domain.Protocolo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIDO.Domain.Protocolo;

namespace UIDO.Services.API
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProtocoloDTO, Protocolo>();
                config.CreateMap<Protocolo, ProtocoloDTO>();
            });

            return mappingConfig;
        }
    }
}
