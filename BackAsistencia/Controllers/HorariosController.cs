    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using BackAsistencia.Models;
    using Microsoft.AspNetCore.Authorization;
    using System.Security.Claims;

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


        // Comienza el metodo para el Horario del alumno
        [HttpGet("mi-horario")]
        //[Authorize(Roles = "Alumno")]
        public async Task<ActionResult<IEnumerable<HorarioItemDTO>>> GetMiHorario()
        {
            // 1. Lee el N.C. del alumno desde el token
            var numeroControlStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(numeroControlStr))
            {
                return Unauthorized("Token inválido.");
            }

            var horario = await _context.Horarios
                .Where(h => h.NumeroControl == numeroControlStr)
                .SelectMany(h => h.HorarioMateriaSalons)
                .Select(hms => new HorarioItemDTO
                {
                    NombreMateria = hms.IdMateriaSalonNavigation.IdMateriaNavigation.Descripcion,
                    Salon = hms.IdMateriaSalonNavigation.IdSalonNavigation.Descripcion,
                    Profesor = hms.IdMateriaSalonNavigation.IdMateriaNavigation.ProfesorMateria
                                  .Select(pm => pm.IdProfesorNavigation.Nombre)
                                  .FirstOrDefault() ?? "Sin Asignar",

                    HorarioTexto1 = hms.HlunJuv,
                    HorarioDias1 = !string.IsNullOrEmpty(hms.HlunJuv) ? "L - J" : null,
                    HorarioTexto2 = hms.Hviernes,
                    HorarioDias2 = !string.IsNullOrEmpty(hms.Hviernes) ? "V" : null,
                    HorarioTexto3 = hms.Hsabados,
                    HorarioDias3 = !string.IsNullOrEmpty(hms.Hsabados) ? "S" : null
                })
                .ToListAsync();

            return Ok(horario);
        }
    }
    }