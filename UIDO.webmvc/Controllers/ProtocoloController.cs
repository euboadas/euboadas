using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UIDO.webmvc.Config;
using UIDO.webmvc.Models;
using UIDO.webmvc.Services.IServices;

namespace UIDO.webmvc.Controllers
{
    public class ProtocoloController : Controller
    {
        private readonly IProtocoloService _protocoloService;

        public ProtocoloController(IProtocoloService protocoloService)
        {
            _protocoloService = protocoloService;
        }

        //TODO log
        public async Task<IActionResult> ProtocoloIndex()
        {
            List<ProtocoloDTO> list = new();

            string accessToken = HttpClientTokenExtension.GetToken(HttpContext);

            //HttpContext.Request.Headers.TryGetValue("access_token", out var accessToken);

            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            //var accessToken = HttpClientTokenExtension.GetToken();
            var response = await _protocoloService.GetAllProtocoloAsync<ResponseDto>(accessToken);
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProtocoloDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public async Task<IActionResult> ProtocoloCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProtocoloCreate(ProtocoloDTO model)
        {
            if (ModelState.IsValid)
            {
                //var accessToken = await HttpContext.GetTokenAsync("access_token");
                string accessToken = HttpClientTokenExtension.GetToken(HttpContext);
                var response = await _protocoloService.CreateProtocoloAsync<ResponseDto>(model, accessToken);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProtocoloIndex));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ProtocoloEdit(string nombre)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var response = await _protocoloService.GetProtocoloByIdAsync<ResponseDto>(nombre, accessToken);
            if (response != null && response.IsSuccess)
            {
                ProtocoloDTO model = JsonConvert.DeserializeObject<ProtocoloDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProtocoloEdit(ProtocoloDTO model)
        {
            if (ModelState.IsValid)
            {
                //var accessToken = await HttpContext.GetTokenAsync("access_token");
                string accessToken = HttpClientTokenExtension.GetToken(HttpContext);
                var response = await _protocoloService.UpdateProtocoloAsync<ResponseDto>(model, accessToken);
                if (response != null && response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProtocoloIndex));
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ProtocoloDelete(string nombre)
        {
            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            string accessToken = HttpClientTokenExtension.GetToken(HttpContext);
            var response = await _protocoloService.GetProtocoloByIdAsync<ResponseDto>(nombre, accessToken);
            if (response != null && response.IsSuccess)
            {
                ProtocoloDTO model = JsonConvert.DeserializeObject<ProtocoloDTO>(Convert.ToString(response.Result));
                return View(model);
            }
            return NotFound();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProtocoloDelete(ProtocoloDTO model)
        {
            if (ModelState.IsValid)
            {
                //var accessToken = await HttpContext.GetTokenAsync("access_token");
                string accessToken = HttpClientTokenExtension.GetToken(HttpContext);
                var response = await _protocoloService.DeleteProtocoloAsync<ResponseDto>(model.Nombre, accessToken);
                if (response.IsSuccess)
                {
                    return RedirectToAction(nameof(ProtocoloIndex));
                }
            }
            return View(model);
        }


    }
}
