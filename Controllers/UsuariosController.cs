using BCrypt.Net;
using fuel_manager_web_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace fuel_manager_web_api.Controllers
{
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

        private void GerarLinks(Usuario model)
        {
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "self", "GET"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "self", "PUT"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "self", "DELETE"));
        }
    }
}
