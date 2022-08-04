using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Text.Json;
using UIDO.webmvc.Config;
using UIDO.webmvc.Models;
using UIDO.webmvc.Services.IServices;

namespace UIDO.webmvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityService _identityService;


        public AccountController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var resultado = await _identityService.Authentication<ResponseDto>(model);
                //var resultado = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);

                if (resultado.IsSuccess)
                {
                    var token = resultado.Result.ToString();

                    await Connect(token);

                    //if (string.IsNullOrEmpty(returnurl))
                    //    return RedirectToAction("Index", "Home");
                    //else
                    return LocalRedirect(returnurl);
                }
                //else if (resultado.)
                //{
                //    ModelState.AddModelError(String.Empty, "Multiple intentos. Bloqueado");
                //    return View(model);
                //    //return View("Denied");
                //}
                else
                {
                    ModelState.AddModelError(String.Empty, "Acceso Invalido");
                    return View(model);
                }

            }

            return View(model);
        }


        public async Task Connect(string access_token)
        {
            var token = access_token.Split('.');
            var base64Content = Convert.FromBase64String(token[1]);

            var user = JsonSerializer.Deserialize<AccessTokenUserInformation>(base64Content);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.nameid),
                new Claim(ClaimTypes.Name, user.unique_name),
                new Claim(ClaimTypes.Email, user.email),
                new Claim("access_token", access_token),
                new Claim("username", user.email),
                new Claim("name", user.unique_name),
                new Claim("email", user.email)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IssuedUtc = DateTime.UtcNow.AddHours(10)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            //HttpContext.Request.Headers.TryAdd("access_token", token);

            //HttpContext.Request.Headers.TryGetValue("access_token", out var accessToken);

            //return Redirect("~/");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> logout()
        {
            //await _signInManager.SignOutAsync();
            string accessToken = HttpClientTokenExtension.GetToken(HttpContext);
            await HttpContext.SignOutAsync("Cookies");
            var resultado = await _identityService.Logout<ResponseDto>(accessToken);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> Register(string returnurl = null)
        //{

        //    var Roles = new List<string>() { "Registrado", "Administrador", "SuperAdministrador", "Especialista", "Regulatorio", "Coordinador", "Financiero" };

        //    foreach (string rol in Roles)
        //    {
        //        if (!await _roleManager.RoleExistsAsync(rol))
        //            await _roleManager.CreateAsync(new IdentityRole(rol));
        //    }


        //    ViewData["ReturnUrl"] = returnurl;
        //    RegisterViewModel model = new();
        //    return View(model);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AllowAnonymous]
        //public async Task<IActionResult> Register(RegisterViewModel model, string returnurl = null)
        //{
        //    ViewData["ReturnUrl"] = returnurl;
        //    returnurl = returnurl ?? Url.Content("~/");

        //    if (ModelState.IsValid)
        //    {
        //        var registroUsuario = new ApplicationUser { Email = model.Email, UserName = model.Email, Nombre = model.Nombre };
        //        var resultado = await _userManager.CreateAsync(registroUsuario, model.Password);

        //        if (resultado.Succeeded)
        //        {

        //            await _userManager.AddToRoleAsync(registroUsuario, "Registrado");

        //            var code = await _userManager.GenerateEmailConfirmationTokenAsync(registroUsuario);
        //            var UrlRetorno = Url.Action("ConfirmEmail", "Account", new { userId = registroUsuario.Id, code = code }, protocol: HttpContext.Request.Scheme);

        //            var htmlCorreo = string.Format("Favor active su cuenta haciendo clic :<a href=\"{0}\"> aqui</a> ", UrlRetorno);

        //            await _emailSender.SendEmailAsync(registroUsuario.Email, "Activacion Cuenta UIDO", htmlCorreo);


        //            await _signInManager.SignInAsync(registroUsuario, isPersistent: false);

        //            return LocalRedirect(returnurl);

        //        }

        //        ValidarErrores(resultado);

        //    }

        //    return View(model);
        //}

        #region Solo Administradores

        [HttpGet]
        public async Task<IActionResult> RegisterAdmin(string returnurl = null)
        {

            var Roles = new List<string>() { "Registrado", "Administrador", "SuperAdministrador", "Especialista", "Regulatorio", "Coordinador", "Financiero" };
            List<SelectListItem> listaRol = new();

            foreach (string rol in Roles)
            {
                //if (!await _roleManager.RoleExistsAsync(rol))
                //    await _roleManager.CreateAsync(new IdentityRole(rol));

                listaRol.Add(new SelectListItem() { Value = rol, Text = rol });
            }



            ViewData["ReturnUrl"] = returnurl;
            RegisterViewModel model = new()
            {
                ListaRol = listaRol,
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");


            //if (ModelState.IsValid)
            //{
            //    var registroUsuario = new ApplicationUser { Email = model.Email, UserName = model.Email, Nombre = model.Nombre };
            //    var resultado = await _userManager.CreateAsync(registroUsuario, model.Password);

            //    if (resultado.Succeeded)
            //    {

            //        if (model.RolSeleccionado != null && model.RolSeleccionado.Length > 0)
            //            await _userManager.AddToRoleAsync(registroUsuario, model.RolSeleccionado);
            //        else
            //            await _userManager.AddToRoleAsync(registroUsuario, "Registrado");


            //        await _userManager.AddToRoleAsync(registroUsuario, "Registrado");

            //        var code = await _userManager.GenerateEmailConfirmationTokenAsync(registroUsuario);
            //        var UrlRetorno = Url.Action("ConfirmEmail", "Account", new { userId = registroUsuario.Id, code = code }, protocol: HttpContext.Request.Scheme);

            //        var htmlCorreo = string.Format("Favor active su cuenta haciendo clic :<a href=\"{0}\"> aqui</a> ", UrlRetorno);

            //        await _emailSender.SendEmailAsync(registroUsuario.Email, "Activacion Cuenta UIDO", htmlCorreo);


            //        await _signInManager.SignInAsync(registroUsuario, isPersistent: false);

            //        return LocalRedirect(returnurl);

            //    }

            //    ValidarErrores(resultado);

            //}


            //#region Actualiza lista en caso de postback
            //List<SelectListItem> listaRol = new();

            //foreach (var rol in _roleManager.Roles)
            //{
            //    listaRol.Add(new SelectListItem(rol.Name, rol.Name));
            //}
            //model.ListaRol = listaRol;
            //#endregion

            return View(model);
        }

        #endregion

        //[AllowAnonymous]
        //private void ValidarErrores(IdentityResult resultado)
        //{
        //    foreach (var item in resultado.Errors)
        //    {
        //        ModelState.AddModelError(String.Empty, item.Description);
        //    }

        //}



    }
}
