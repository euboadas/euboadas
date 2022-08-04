using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UIDO.Database;
using UIDO.Domain.Indentity;
using UIDO.Services.API.Models.DTO;

namespace UIDO.Services.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("v1/users")]
    public class UsersController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        protected ResponseDto _response;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
            this._response = new ResponseDto();
        }

        //[Authorize(Roles = "Administrador")]
        [Authorize]
        [HttpGet]
        public async Task<Object> Users()
        {
            var usuarios = await _context.ApplicationUser.ToListAsync();
            var usuarioRol = await _context.UserRoles.ToListAsync();
            var roles = await _context.Roles.ToListAsync();

            foreach (var usuario in usuarios)
            {
                var rol = usuarioRol.FirstOrDefault(u => u.UserId == usuario.Id);
                if (rol == null)
                {
                    usuario.Rol = "Ninguno";
                }
                else
                {
                    usuario.Rol = roles.FirstOrDefault(u => u.Id == rol.RoleId).Name;
                }
            }

            _response = new ResponseDto() { IsSuccess = (usuarios.Count > 0), Result = usuarios };

            return _response;

            //return View(usuarios);
        }


        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public Object GetProfile(string id)
        {

            if (string.IsNullOrEmpty(id) || _context.ApplicationUser.Find(id) == null)
                return NotFound();

            var usuarioBD = _context.ApplicationUser.Find(id);


            _response = new ResponseDto() { IsSuccess = (usuarioBD != null), Result = usuarioBD };

            return _response;

            //return View(usuarioBD);
        }

        [Authorize]
        [HttpGet]
        [Route("{email}")]
        public Object GetProfilebyEmail(string email)
        {

            if (string.IsNullOrEmpty(email) || _context.ApplicationUser.FirstOrDefault(u => u.Email.Equals(email)) != null)
                return NotFound();

            var usuarioBD = _context.ApplicationUser.FirstOrDefault(u => u.Email.Equals(email));


            _response = new ResponseDto() { IsSuccess = (usuarioBD != null), Result = usuarioBD };

            return _response;

            //return View(usuarioBD);
        }


        [Authorize]
        [HttpPut]
        //[ValidateAntiForgeryToken]
        public async Task<Object> ProfileEdit([FromBody] ApplicationUser user)
        {

            if (ModelState.IsValid)
            {
                var usuarioBD = await _context.ApplicationUser.FindAsync(user.Id);
                usuarioBD.Nombre = user.Nombre;
                await _userManager.UpdateAsync(usuarioBD);
                //RedirectToAction(nameof(Index), "Home");
                _response = new ResponseDto() { IsSuccess = (usuarioBD != null), Result = usuarioBD };

                return _response;
            }

            _response = new ResponseDto() { IsSuccess = false, Result = "No se actualiza" };

            return _response;

            //return View(user);
        }



    }
}
