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
    public class MateriasController : ControllerBase
    {
        private readonly ControlAsistenciasContext _context;

        public MateriasController(ControlAsistenciasContext context)
        {
            _context = context;
        }

        // GET: api/Materias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MateriaDto>>> GetMateria()
        {
            var materias = await _context.Materia
                .Select(m => new MateriaDto
                {
                    IdMateria = m.IdMateria,
                    Descripcion = m.Descripcion
                })
                .ToListAsync();

            return Ok(materias);
        }

        // GET: api/Materias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaDto>> GetMateria(int id)
        {
            var materia = await _context.Materia
                .Where(m => m.IdMateria == id)
                .Select(m => new MateriaDto
                {
                    IdMateria = m.IdMateria,
                    Descripcion = m.Descripcion
                })
                .FirstOrDefaultAsync();

            if (materia == null)
            {
                return NotFound();
            }

            return Ok(materia);
        }

        // PUT: api/Materias/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMateria(int id, MateriaDto dto)
        {
            if (id != dto.IdMateria)
            {
                return BadRequest();
            }

            var materia = await _context.Materia.FindAsync(id);
            if (materia == null)
            {
                return NotFound();
            }

            materia.Descripcion = dto.Descripcion;

            try
            {
                _context.Update(materia);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MateriaExists(id))
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

        // POST: api/Materias
        [HttpPost]
        public async Task<ActionResult<MateriaDto>> PostMateria(MateriaDto dto)
        {
            var materia = new Materia
            {
                Descripcion = dto.Descripcion
            };

            _context.Materia.Add(materia);
            await _context.SaveChangesAsync();

            dto.IdMateria = materia.IdMateria;

            return CreatedAtAction(nameof(GetMateria), new { id = dto.IdMateria }, dto);
        }

        // DELETE: api/Materias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMateria(int id)
        {
            var materia = await _context.Materia.FindAsync(id);
            if (materia == null)
            {
                return NotFound();
            }

            _context.Materia.Remove(materia);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MateriaExists(int id)
        {
            return _context.Materia.Any(e => e.IdMateria == id);
        }
    }
}