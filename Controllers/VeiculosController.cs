using fuel_manager_web_api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace fuel_manager_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VeiculosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Veiculos.ToListAsync();
            return Ok(model);
        }

        [HttpPost]
        public async Task<ActionResult> Create(Veiculo model)
        {
            _context.Veiculos.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = model.Id }, model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Veiculos.FirstOrDefaultAsync(c => c.Id == id);

            if (model == null) return NotFound(new { status = 404, errors = new { message = "Veículo não encontrado." } });

            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Veiculo model)
        {
            if (id != model.Id) return BadRequest();

            var modelDb = await _context.Veiculos.AsNoTracking()
                                .FirstOrDefaultAsync(c => c.Id == id);

            if (modelDb == null) return NotFound(new { status = 404, errors = new { message = "Veículo não encontrado." } });

            _context.Veiculos.Update(model);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Veiculos.FindAsync(id);

            if (model == null) return NotFound(new { status = 404, errors = new { message = "Veículo não encontrado." } });

            _context.Veiculos.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();;
        }


    }
}
