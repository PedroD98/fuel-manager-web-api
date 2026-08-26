using fuel_manager_web_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace fuel_manager_web_api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConsumosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Consumos.ToListAsync();
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Consumo model)
        {
            var veiculo = await _context.Veiculos.FindAsync(model.VeiculoId);
            if (veiculo == null) return NotFound(new {error = new { message = "Veículo não encontrado."}});

            _context.Consumos.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = model.Id }, model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Consumos.FirstOrDefaultAsync(c => c.Id == id);

            if (model == null) return NotFound(new { status = 404, errors = new { message = "Consumo não encontrado." } });

            GerarLinks(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Consumo model)
        {
            if (id != model.Id) return BadRequest();

            var modelDb = await _context.Consumos.AsNoTracking()
                                .FirstOrDefaultAsync(c => c.Id == id);

            if (modelDb == null) return NotFound(new { status = 404, errors = new { message = "Consumo não encontrado." } });

            var veiculo = await _context.Veiculos.FindAsync(model.VeiculoId);
            if (veiculo == null) return NotFound(new {error = new { message = "Veículo não encontrado."}});

            _context.Consumos.Update(model);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Consumos.FindAsync(id);

            if (model == null) return NotFound(new { status = 404, errors = new { message = "Consumo não encontrado." } });

            _context.Consumos.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();;
        }

        private void GerarLinks(Consumo model)
        {
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "self", "GET"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "self", "PUT"));
            model.Links.Add(new LinkDto(model.Id, Url.ActionLink(), "self", "DELETE"));
        }
    }
}
