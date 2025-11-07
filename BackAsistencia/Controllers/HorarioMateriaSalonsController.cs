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
    public class HorarioMateriaSalonsController : ControllerBase
    {
        private readonly ControlAsistenciasContext _context;

        public HorarioMateriaSalonsController(ControlAsistenciasContext context)
        {
            _context = context;
        }

        // GET: api/HorarioMateriaSalons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HorarioMateriaSalonDto>>> GetHorarioMateriaSalons()
        {
            var lista = await _context.HorarioMateriaSalons
                .Select(h => new HorarioMateriaSalonDto
                {
                    IdHorarioMateriaSalon = h.IdHorarioMateriaSalon,
                    IdMateriaSalon = h.IdMateriaSalon,
                    IdHorario = h.IdHorario,
                    HlunJuv = h.HlunJuv,
                    Hviernes = h.Hviernes,
                    Hsabados = h.Hsabados
                })
                .ToListAsync();

            return Ok(lista);
        }

        // GET: api/HorarioMateriaSalons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HorarioMateriaSalonDto>> GetHorarioMateriaSalon(int id)
        {
            var dto = await _context.HorarioMateriaSalons
                .Where(h => h.IdHorarioMateriaSalon == id)
                .Select(h => new HorarioMateriaSalonDto
                {
                    IdHorarioMateriaSalon = h.IdHorarioMateriaSalon,
                    IdMateriaSalon = h.IdMateriaSalon,
                    IdHorario = h.IdHorario,
                    HlunJuv = h.HlunJuv,
                    Hviernes = h.Hviernes,
                    Hsabados = h.Hsabados
                })
                .FirstOrDefaultAsync();

            if (dto == null)
            {
                return NotFound();
            }

            return Ok(dto);
        }

        // PUT: api/HorarioMateriaSalons/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHorarioMateriaSalon(int id, HorarioMateriaSalonDto dto)
        {
            if (id != dto.IdHorarioMateriaSalon)
            {
                return BadRequest();
            }

            var entidad = await _context.HorarioMateriaSalons.FindAsync(id);
            if (entidad == null)
            {
                return NotFound();
            }

            entidad.IdMateriaSalon = dto.IdMateriaSalon;
            entidad.IdHorario = dto.IdHorario;
            entidad.HlunJuv = dto.HlunJuv;
            entidad.Hviernes = dto.Hviernes;
            entidad.Hsabados = dto.Hsabados;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorarioMateriaSalonExists(id))
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

        // POST: api/HorarioMateriaSalons
        [HttpPost]
        public async Task<ActionResult<HorarioMateriaSalonDto>> PostHorarioMateriaSalon(HorarioMateriaSalonDto dto)
        {
            var entidad = new HorarioMateriaSalon
            {
                IdMateriaSalon = dto.IdMateriaSalon,
                IdHorario = dto.IdHorario,
                HlunJuv = dto.HlunJuv,
                Hviernes = dto.Hviernes,
                Hsabados = dto.Hsabados
            };

            _context.HorarioMateriaSalons.Add(entidad);
            await _context.SaveChangesAsync();

            dto.IdHorarioMateriaSalon = entidad.IdHorarioMateriaSalon;

            return CreatedAtAction(nameof(GetHorarioMateriaSalon), new { id = dto.IdHorarioMateriaSalon }, dto);
        }

        // DELETE: api/HorarioMateriaSalons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorarioMateriaSalon(int id)
        {
            var entidad = await _context.HorarioMateriaSalons.FindAsync(id);
            if (entidad == null)
            {
                return NotFound();
            }

            _context.HorarioMateriaSalons.Remove(entidad);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HorarioMateriaSalonExists(int id)
        {
            return _context.HorarioMateriaSalons.Any(e => e.IdHorarioMateriaSalon == id);
        }
    }
}