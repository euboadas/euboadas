using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UIDO.Database;
using UIDO.Domain.Indentity;
using UIDO.Services.API.Models;
using UIDO.Services.API.Models.DTO;
using UIDO.Services.Models;

namespace UIDO.Services.API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class IdentityController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private Dictionary<string, string> rollist = new Dictionary<string, string>();

        public IdentityController(
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("authentication")]
        [AllowAnonymous]
        public async Task<IActionResult> Authentication([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = new ResponseDto();

                //var user = await _context.Users.SingleAsync(x => x.Email == model.Email);
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

                if (user == null)
                {
                    result.IsSuccess = false;
                    result.DisplayMessage = "Acceso Denagado";
                    return BadRequest(result);
                }

                var response = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                if (response.Succeeded)
                {
                    result.IsSuccess = true;
                    result.DisplayMessage = "Bienvenido a UIDO";
                    await GenerateToken(user, result);
                    return Ok(result);
                }
                result.IsSuccess = false;
                result.DisplayMessage = "Acceso Denagado";
                return BadRequest(result);


            }

            return BadRequest();
        }


        //[HttpPost("authentication")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Authentication([FromBody] LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        var result = new ResponseDto();

        //        //var user = await _context.Users.SingleAsync(x => x.Email == model.Email);
        //        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

        //        if (user == null)
        //        {
        //            result.IsSuccess = false;
        //            return Unauthorized("Access denied");
        //        }

        //        var response = await _signInManager.CheckPasswordSignInAsync(user,model.Password, false);

        //        if (response.Succeeded)
        //        {
        //            result.IsSuccess = true;
        //            await GenerateToken(user, result);
        //            return Ok(result);
        //        }

        //        return BadRequest("Access denied");


        //    }

        //    return BadRequest();
        //}
       
        
        [HttpPost("verify_token")]
        public async Task<IActionResult> verify_token([FromBody] ResponseDto identity, ApplicationUser user)
        {
            var roles = await _context.Roles.ToListAsync();
            var usuarioRol = await _context.UserRoles.ToListAsync();

            var rol = usuarioRol.FirstOrDefault(u => u.UserId == user.Id);
            if (rol == null)
            {
                user.Rol = "Ninguno";
            }
            else
            {
                user.Rol = roles.FirstOrDefault(u => u.Id == rol.RoleId).Name;
            }

            return Json(user);

        }
        
        private async Task GenerateToken(ApplicationUser user, ResponseDto identity)
        {
            //var secretKey = _configuration.GetValue<string>("SecretKey");
            var secretKey = _configuration["SecretKey"];
            var key = Encoding.ASCII.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Nombre),
                //new Claim(ClaimTypes.Surname, user.LastName)
            };

            var roles = await _context.Roles.ToListAsync();
            var usuarioRol = await _context.UserRoles.ToListAsync();

            var rol = usuarioRol.FirstOrDefault(u => u.UserId == user.Id);
            if (rol == null)
            {
                user.Rol = "Ninguno";
            }
            else
            {
                user.Rol = roles.FirstOrDefault(u => u.Id == rol.RoleId).Name;
            }


            //foreach (var role in roles)
            //{
            //    claims.Add(
            //        new Claim(ClaimTypes.Role, role.Name)
            //    );
            //}

            claims.Add(
                new Claim(ClaimTypes.Role, user.Rol));


        var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            identity.api_token= tokenHandler.WriteToken(createdToken);
            identity.Result = tokenHandler.WriteToken(createdToken);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> logout()
        {
            await _signInManager.SignOutAsync();
            //return RedirectToAction(nameof(HomeController.Index), "Home");

            var result = new ResponseDto() {IsSuccess=true };
            
            return Ok(result);
        }


        #region Solo Administradores
        //[Authorize]
        [HttpGet("CrearUsuario")]
        public async Task<IActionResult> RegisterAdmin(string returnurl = null)
        {

            var Roles = new List<string>() { "Registrado", "Administrador", "SuperAdministrador", "Especialista", "Regulatorio", "Coordinador", "Financiero" };
            var listaRol = new List<string>();

            foreach (string rol in Roles)
            {
                if (!await _roleManager.RoleExistsAsync(rol))
                    await _roleManager.CreateAsync(new IdentityRole(rol));

                listaRol.Add(rol);
            }

            ViewData["ReturnUrl"] = returnurl;
            RegisterViewModel model = new()
            {
                ListaRol = listaRol,
            };

            var result = new ResponseDto();

            result.IsSuccess = true;
            result.Result = model;

            return Ok(result);
        }

       // [Authorize]
        [HttpPost("CrearUsuario")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");

            var result = new ResponseDto();

            if (ModelState.IsValid)
            {
                var registroUsuario = new ApplicationUser { Email = model.Email, UserName = model.Email, Nombre = model.Nombre };
                var resultado = await _userManager.CreateAsync(registroUsuario, model.Password);

                if (resultado.Succeeded)
                {
                    result.IsSuccess = true;
                    string RolNuevo = "Registrado";

                    if (model.RolSeleccionado != null && model.RolSeleccionado.Length > 0)
                        RolNuevo = model.RolSeleccionado;
                    
                    await _userManager.AddToRoleAsync(registroUsuario, RolNuevo);

                    model.RolSeleccionado = RolNuevo;

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(registroUsuario);
                    //var UrlRetorno = Url.Action("ConfirmEmail", "Account", new { userId = registroUsuario.Id, code = code }, protocol: HttpContext.Request.Scheme);

                    //var htmlCorreo = string.Format("Favor active su cuenta haciendo clic :<a href=\"{0}\"> aqui</a> ", UrlRetorno);

                    //await _emailSender.SendEmailAsync(registroUsuario.Email, "Activacion Cuenta UIDO", htmlCorreo);


                    await _signInManager.SignInAsync(registroUsuario, isPersistent: false);

                    result.Result = model;

                    //return LocalRedirect(returnurl);
                    return Ok(result);
                }

                result.IsSuccess = false;
                result.ErrorMessages =  ValidarErrores(resultado);
                result.Result = model;

            }


            #region Actualiza lista en caso de postback
            List<string> listaRol = new();

            foreach (var rol in _roleManager.Roles)
            {
                listaRol.Add(rol.Name);
            }
            model.ListaRol = listaRol;
            #endregion

            //return View(model);
            result.Result = model;
            result.IsSuccess = false;
            return Ok(result);
        }

        #endregion

        [AllowAnonymous]
        private List<string> ValidarErrores(IdentityResult resultado)
        {
            var lista = new List<string>();
            foreach (var item in resultado.Errors)
            {
                ModelState.AddModelError(String.Empty, item.Description);
                lista.Add(item.Description);
            }
            return lista;
        }


    }
}
