using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UIDO.Domain.Protocolo.DTO;
using UIDO.Services.API.Models.DTO;
using UIDO.Services.API.Repository;

namespace UIDO.Services.API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("v1/protocolo")]
    public class ProtocoloAPIController : ControllerBase
    {
        protected ResponseDto _response;
        private IProtocoloRepository _protocoloRepository;

        public ProtocoloAPIController(IProtocoloRepository protocoloRepository)
        {
            _protocoloRepository = protocoloRepository;
            this._response = new ResponseDto();
        }

        [HttpGet]
        public async Task<object> GetAll()
        {
            try
            {
                IEnumerable<ProtocoloDTO> productDtos = await _protocoloRepository.GetProtocolos();
                _response.Result = productDtos;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpGet]
        [Route("{nombre}")]
        public async Task<object> Get(string nombre)
        {
            try
            {
                ProtocoloDTO productDto = await _protocoloRepository.GetProtocoloById(nombre);
                _response.Result = productDto;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPost]
        [Authorize]
        public async Task<object> Post([FromBody] ProtocoloDTO protocoloDto)
        {
            try
            {
                ProtocoloDTO model = await _protocoloRepository.CreateUpdateProtocolo(protocoloDto);
                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut]
        [Authorize]
        public async Task<object> Put([FromBody] ProtocoloDTO protocoloDto)
        {
            try
            {
                ProtocoloDTO model = await _protocoloRepository.CreateUpdateProtocolo(protocoloDto);
                _response.Result = model;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{nombre}")]
        public async Task<object> Delete(string nombre)
        {
            try
            {
                bool isSuccess = await _protocoloRepository.DeleteProtocolo(nombre);
                _response.Result = isSuccess;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
