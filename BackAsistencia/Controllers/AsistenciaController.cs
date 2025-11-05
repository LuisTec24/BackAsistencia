using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackAsistencia.Models;

namespace BackAsistencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenciaController : ControllerBase
    {
        private readonly ControlAsistenciasContext _context;

        public AsistenciaController(ControlAsistenciasContext context)
        {
            _context = context;
        }

        // GET: api/Asistenciums
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Asistencia>>> GetAsistencia()
        {
            return await _context.Asistencia.ToListAsync();
        }

        // GET: api/Asistenciums/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Asistencia>> GetAsistencium(int id)
        {
            var asistencium = await _context.Asistencia.FindAsync(id);

            if (asistencium == null)
            {
                return NotFound();
            }

            return asistencium;
        }

        // PUT: api/Asistenciums/5x
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsistencium(int id, Asistencia asistencium)
        {
            if (id != asistencium.IdAsistencia)
            {
                return BadRequest();
            }

            _context.Entry(asistencium).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AsistenciumExists(id))
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


        [HttpPost]
        public async Task<ActionResult<Asistencia>> PostAsistencium([FromBody] CrearAsistenciaDTO dto)
        {
            // Validación opcional
            var alumnoExiste = await _context.Alumnos.AnyAsync(a => a.NumeroControl == dto.NumeroControl);
            var materiaSalonExiste = await _context.MateriaSalons.AnyAsync(ms => ms.IdMateriaSalon == dto.ID_HorarioMateriaSalon);

            if (!alumnoExiste || !materiaSalonExiste)
                return BadRequest("Alumno o Materia-Salón no encontrados.");

            var nueva = new Asistencia
            {
                ID_HorarioMateriaSalon = dto.ID_HorarioMateriaSalon,
                NumeroControl = dto.NumeroControl,
                Fecha = dto.Fecha,
                Hora = dto.Hora
            };

            _context.Asistencia.Add(nueva);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetAsistencium", new { id = nueva.IdAsistencia }, nueva);
        }

        //[HttpPost]

        //public async Task<ActionResult<Asistencium>> PostAsistencium(Asistencium asistencium)
        //{
        //    _context.Asistencia.Add(asistencium);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetAsistencium", new { id = asistencium.IdAsistencia }, asistencium);
        //}

        // DELETE: api/Asistenciums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsistencium(int id)
        {
            var asistencium = await _context.Asistencia.FindAsync(id);
            if (asistencium == null)
            {
                return NotFound();
            }

            _context.Asistencia.Remove(asistencium);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AsistenciumExists(int id)
        {
            return _context.Asistencia.Any(e => e.IdAsistencia == id);
        }
    }
}
