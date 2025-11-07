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
    public class HorariosController : ControllerBase
    {
        private readonly ControlAsistenciasContext _context;

        public HorariosController(ControlAsistenciasContext context)
        {
            _context = context;
        }

        // GET: api/Horarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HorarioDto>>> GetHorarios()
        {
            var lista = await _context.Horarios
                .Select(h => new HorarioDto
                {
                    IdHorario = h.IdHorario,
                    FechaInicioSemestre = h.FechaInicioSemestre,
                    FechaFinSemestre = h.FechaFinSemestre,
                    NumeroControl = h.NumeroControl
                })
                .ToListAsync();

            return Ok(lista);
        }

        // GET: api/Horarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HorarioDto>> GetHorario(int id)
        {
            var dto = await _context.Horarios
                .Where(h => h.IdHorario == id)
                .Select(h => new HorarioDto
                {
                    IdHorario = h.IdHorario,
                    FechaInicioSemestre = h.FechaInicioSemestre,
                    FechaFinSemestre = h.FechaFinSemestre,
                    NumeroControl = h.NumeroControl
                })
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        // PUT: api/Horarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHorario(int id, HorarioDto dto)
        {
            if (id != dto.IdHorario)
            {
                return BadRequest();
            }

            var entidad = await _context.Horarios.FindAsync(id);
            if (entidad == null)
            {
                return NotFound();
            }

            entidad.FechaInicioSemestre = dto.FechaInicioSemestre;
            entidad.FechaFinSemestre = dto.FechaFinSemestre;
            entidad.NumeroControl = dto.NumeroControl;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorarioExists(id))
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

        // POST: api/Horarios
        [HttpPost]
        public async Task<ActionResult<HorarioDto>> PostHorario(HorarioDto dto)
        {
            var entidad = new Horario
            {
                FechaInicioSemestre = dto.FechaInicioSemestre,
                FechaFinSemestre = dto.FechaFinSemestre,
                NumeroControl = dto.NumeroControl
            };

            _context.Horarios.Add(entidad);
            await _context.SaveChangesAsync();

            dto.IdHorario = entidad.IdHorario;

            return CreatedAtAction(nameof(GetHorario), new { id = dto.IdHorario }, dto);
        }

        // DELETE: api/Horarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorario(int id)
        {
            var entidad = await _context.Horarios.FindAsync(id);
            if (entidad == null)
            {
                return NotFound();
            }

            _context.Horarios.Remove(entidad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HorarioExists(int id)
        {
            return _context.Horarios.Any(e => e.IdHorario == id);
        }
    }
}