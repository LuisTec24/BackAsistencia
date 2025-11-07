using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackAsistencia.Models;

namespace BackAsistencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfesorMateriasController : ControllerBase
    {
        private readonly ControlAsistenciasContext _context;

        public ProfesorMateriasController(ControlAsistenciasContext context)
        {
            _context = context;
        }

        // GET: api/ProfesorMaterias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfesorMateriaDto>>> GetProfesorMaterias()
        {
            var lista = await _context.ProfesorMateria
                .Select(pm => new ProfesorMateriaDto
                {
                    IdProfesorMateria = pm.IdProfesorMateria,
                    IdProfesor = pm.IdProfesor,
                    IdMateria = pm.IdMateria
                })
                .ToListAsync();

            return Ok(lista);
        }

        // GET: api/ProfesorMaterias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfesorMateriaDto>> GetProfesorMateria(int id)
        {
            var dto = await _context.ProfesorMateria
                .Where(p => p.IdProfesorMateria == id)
                .Select(p => new ProfesorMateriaDto
                {
                    IdProfesorMateria = p.IdProfesorMateria,
                    IdProfesor = p.IdProfesor,
                    IdMateria = p.IdMateria
                })
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        // PUT: api/ProfesorMaterias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfesorMateria(int id, ProfesorMateriaDto dto)
        {
            if (id != dto.IdProfesorMateria)
            {
                return BadRequest();
            }

            var entidad = await _context.ProfesorMateria.FindAsync(id);
            if (entidad == null)
            {
                return NotFound();
            }

            entidad.IdProfesor = dto.IdProfesor;
            entidad.IdMateria = dto.IdMateria;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfesorMateriaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProfesorMaterias
        [HttpPost]
        public async Task<ActionResult<ProfesorMateriaDto>> PostProfesorMateria(ProfesorMateriaDto dto)
        {
            var entidad = new ProfesorMateria
            {
                IdProfesor = dto.IdProfesor,
                IdMateria = dto.IdMateria
            };

            _context.ProfesorMateria.Add(entidad);
            await _context.SaveChangesAsync();

            dto.IdProfesorMateria = entidad.IdProfesorMateria;

            return CreatedAtAction(nameof(GetProfesorMateria), new { id = dto.IdProfesorMateria }, dto);
        }

        // DELETE: api/ProfesorMaterias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfesorMateria(int id)
        {
            var entidad = await _context.ProfesorMateria.FindAsync(id);
            if (entidad == null)
            {
                return NotFound();
            }

            _context.ProfesorMateria.Remove(entidad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfesorMateriaExists(int id)
        {
            return _context.ProfesorMateria.Any(e => e.IdProfesorMateria == id);
        }
    }
}