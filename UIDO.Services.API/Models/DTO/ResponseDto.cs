using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UIDO.Services.API.Models.DTO
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
        
        public string refreshToken { get; set; } = "";
        public string api_token { get; set; } = "";
        
        public string DisplayMessage { get; set; } = "";
        public List<string> ErrorMessages { get; set; }
    }
}
