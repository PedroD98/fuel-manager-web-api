using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using fuel_manager_web_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;

namespace fuel_manager_web_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Usuarios.ToListAsync();
            return Ok(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Create(UsuarioDto model)
        {
            Usuario usuario = new()
            {
                Nome = model.Nome,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Perfil = model.Perfil
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = usuario.Id }, usuario);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);

            if (model == null) return NotFound(new { status = 404, errors = new { message = "Usuário não encontrado." } });

            GerarLinks(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Usuario model)
        {
            if (id != model.Id) return BadRequest();

            var usuario = await _context.Usuarios.AsNoTracking()
                                .FirstOrDefaultAsync(c => c.Id == id);

            if (usuario == null) return NotFound(new { status = 404, errors = new { message = "Usuário não encontrado." } });

            usuario.Nome = model.Nome;
            usuario.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            usuario.Perfil = model.Perfil;

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Usuarios.FindAsync(id);

            if (model == null) return NotFound(new { status = 404, errors = new { message = "Usuário não encontrado." } });

            _context.Usuarios.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent(); ;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Login(AuthenticateDto model)
        {
            var usuario = await _context.Usuarios.FindAsync(model.Id);

            // if (usuario == null) return NotFound(new { status = 404, errors = new { message = "Usuário não encontrado." } });
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.Password))
                return Unauthorized();

            var jwt = GenerateJwtToken(usuario);

            return Ok(new { jwtToken = jwt });
        }

        private static string GenerateJwtToken(Usuario model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("6CasHvUJ5etFa7oJ0pRyM7lInRmadeBt");
            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                new Claim(ClaimTypes.Role, model.Perfil.ToString())
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private void GerarLinks(Usuario model)
        {
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "self", "GET"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "self", "PUT"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "self", "DELETE"));
        }
    }
}
