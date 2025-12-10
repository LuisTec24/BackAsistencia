using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using BackAsistencia.Models;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace BackAsistencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlumnosController : ControllerBase
    {
        private readonly ControlAsistenciasContext _context;

        public AlumnosController(ControlAsistenciasContext context)
        {
            _context = context;
        }

        // GET: api/Alumnos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlumnoDTO>>> GetAlumnos()
        {
            var alumnos = await _context.Alumnos
                .Select(a => new AlumnoDTO
                {
                    NumeroControl = a.NumeroControl,
                    Nombre = a.Nombre,
                    Carrera = a.Carrera,
                    Semestre = a.Semestre,
                    Contrasena = a.Contrasena // Inclúyelo solo si es necesario
                })
                .ToListAsync();

            return Ok(alumnos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AlumnoDTO>> GetAlumno(string id)
        {
            var alumno = await _context.Alumnos
                .Where(a => a.NumeroControl == id)
                .Select(a => new AlumnoDTO
                {
                    NumeroControl = a.NumeroControl,
                    Nombre = a.Nombre,
                    Carrera = a.Carrera,
                    Semestre = a.Semestre,
                    Contrasena = a.Contrasena // Inclúyelo solo si es necesario
                })
                .FirstOrDefaultAsync();

            if (alumno == null)
            {
                return NotFound();
            }

            return Ok(alumno);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAlumno(string id, [FromBody] AlumnoDTO dto)
        {
            if (id != dto.NumeroControl)
            {
                return BadRequest("El número de control en la URL no coincide con el del cuerpo.");
            }

            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
            {
                return NotFound("No se encontró el alumno.");
            }

            alumno.Nombre = dto.Nombre;
            alumno.Carrera = dto.Carrera;
            alumno.Semestre = dto.Semestre;

            // Encriptar solo si la contraseña fue modificada
            if (!string.IsNullOrWhiteSpace(dto.Contrasena) &&
                !BCrypt.Net.BCrypt.Verify(dto.Contrasena, alumno.Contrasena))
            {
                alumno.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlumnoExists(id))
                {
                    return NotFound("El alumno ya no existe.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<Alumno>> PostAlumno([FromBody] AlumnoDTO dto)
        {
            var existe = await _context.Alumnos.AnyAsync(a => a.NumeroControl == dto.NumeroControl);
            if (existe)
                return Conflict("Ya existe un alumno con ese número de control.");

            var nuevo = new Alumno
            {
                NumeroControl = dto.NumeroControl,
                Nombre = dto.Nombre,
                Carrera = dto.Carrera,
                Semestre = dto.Semestre,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena) 
            };

            _context.Alumnos.Add(nuevo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlumno", new { id = nuevo.NumeroControl }, nuevo);
        }

    // DELETE: api/Alumnoes/5
    [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlumno(string id)
        {
            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
            {
                return NotFound();
            }

            _context.Alumnos.Remove(alumno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlumnoExists(string id)
        {
            return _context.Alumnos.Any(e => e.NumeroControl == id);
        }


        [HttpGet("Horario/{numeroControl}")]
        public async Task<ActionResult<IEnumerable<HorarioAlumnoDTO>>> GetHorarioAlumno(string numeroControl)
        {


            var query = from h in _context.Horarios
                        where h.NumeroControl == numeroControl



                        join hms in _context.HorarioMateriaSalons on h.IdHorario equals hms.IdHorario

                        join ms in _context.MateriaSalons on hms.IdMateriaSalon equals ms.IdMateriaSalon

                        join m in _context.Materia on ms.IdMateria equals m.IdMateria

                        join s in _context.Salons on ms.IdSalon equals s.IdSalon

                        join pm in _context.ProfesorMateria on ms.IdMateriaSalon equals pm.IdMateriaSalon into pmGroup
                        from subPm in pmGroup.DefaultIfEmpty() 

                        join p in _context.Profesors on subPm.IdProfesor equals p.IdProfesor into pGroup
                        from subP in pGroup.DefaultIfEmpty() 

                        select new HorarioAlumnoDTO
                        {
                            Materia = m.Descripcion,
                            Salon = s.Descripcion,
                            Profesor = subP != null ? subP.Nombre : "Por Asignar",

                            LunesJueves = ms.HlunJuv,
                            Viernes = ms.Hviernes,
                            Sabado = ms.Hsabados
                        };

            return await query.ToListAsync();
        }
        [HttpGet("HistorialHoy/{numeroControl}")]
        public async Task<ActionResult<IEnumerable<AsistenciaAlumnoDTO>>> GetHistorialHoy(string numeroControl)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            //var hoy = new DateOnly(2025, 12, 05);

            var query = from a in _context.Asistencia
                        join hms in _context.HorarioMateriaSalons on a.ID_HorarioMateriaSalon equals hms.IdHorarioMateriaSalon

                        join h in _context.Horarios on hms.IdHorario equals h.IdHorario

                        join ms in _context.MateriaSalons on hms.IdMateriaSalon equals ms.IdMateriaSalon
                        join m in _context.Materia on ms.IdMateria equals m.IdMateria

                        where h.NumeroControl == numeroControl && a.Fecha == hoy

                        select new AsistenciaAlumnoDTO
                        {
                            Materia = m.Descripcion,
                            Hora = a.Hora.ToString(),
                            Estatus = a.Estatus,
                            Fecha = a.Fecha.ToDateTime(TimeOnly.MinValue)
                        };

            return await query.ToListAsync();
        }
    }


}
