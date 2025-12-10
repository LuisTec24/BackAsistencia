using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BackAsistencia.Models;
using BCrypt.Net;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
namespace BackAsistencia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesorsController : ControllerBase
    {
        private readonly ControlAsistenciasContext _context;

        public ProfesorsController(ControlAsistenciasContext context)
        {
            _context = context;
        }

        // 🔄 Métodos de mapeo entre entidad y DTO
        private ProfesorDTO MapToDTO(Profesor profesor)
        {
            return new ProfesorDTO
            {
                IdProfesor = profesor.IdProfesor,
                Nombre = profesor.Nombre,
                Departamento = profesor.Departamento,
                Contrasena = profesor.Contrasena,
                Correo = profesor.Correo
            };
        }

        private Profesor MapToEntity(ProfesorDTO dto)
        {
            return new Profesor
            {
                IdProfesor = dto.IdProfesor,
                Nombre = dto.Nombre,
                Departamento = dto.Departamento,
                Contrasena = dto.Contrasena,
                Correo = dto.Correo
            };
        }

        // GET: api/Profesors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfesorDTO>>> GetProfesors()
        {
            var profesores = await _context.Profesors.ToListAsync();
            return profesores.Select(p => MapToDTO(p)).ToList();
        }

        // GET: api/Profesors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfesorDTO>> GetProfesor(int id)
        {
            var profesor = await _context.Profesors.FindAsync(id);

            if (profesor == null)
            {
                return NotFound();
            }

            return MapToDTO(profesor);
        }

    
    [HttpPost]
    public async Task<ActionResult<ProfesorDTO>> PostProfesor(ProfesorDTO dto)
    {
        var profesor = new Profesor
        {
            Nombre = dto.Nombre,
            Departamento = dto.Departamento,
            Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena), // 🔐 Hashear contraseña
            Correo = dto.Correo
        };

        _context.Profesors.Add(profesor);
        await _context.SaveChangesAsync();

        dto.IdProfesor = profesor.IdProfesor;
        dto.Contrasena = null!; // Opcional: no devolver el hash

        return CreatedAtAction(nameof(GetProfesor), new { id = dto.IdProfesor }, dto);
    }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfesor(int id, ProfesorDTO dto)
        {
            if (id != dto.IdProfesor)
            {
                return BadRequest();
            }

            var profesor = await _context.Profesors.FindAsync(id);
            if (profesor == null)
            {
                return NotFound();
            }

            // Actualizar campos
            profesor.Nombre = dto.Nombre;
            profesor.Departamento = dto.Departamento;
            profesor.Correo = dto.Correo;

            // Encriptar solo si la contraseña fue modificada
            if (!string.IsNullOrWhiteSpace(dto.Contrasena) &&
                !BCrypt.Net.BCrypt.Verify(dto.Contrasena, profesor.Contrasena))
            {
                profesor.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfesorExists(id))
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


        // DELETE: api/Profesors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfesor(int id)
        {
            var profesor = await _context.Profesors.FindAsync(id);
            if (profesor == null)
            {
                return NotFound();
            }

            _context.Profesors.Remove(profesor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfesorExists(int id)
        {
            return _context.Profesors.Any(e => e.IdProfesor == id);
        }

        //cosas que hice pa asistencias
        [HttpGet("MisGrupos/{idProfesor}")]
        public async Task<ActionResult<IEnumerable<GrupoDocenteDTO>>> GetMisGrupos(int idProfesor)
        {
            var query = from pm in _context.ProfesorMateria
                        join ms in _context.MateriaSalons on pm.IdMateriaSalon equals ms.IdMateriaSalon
                        join m in _context.Materia on ms.IdMateria equals m.IdMateria
                        join s in _context.Salons on ms.IdSalon equals s.IdSalon
                        where pm.IdProfesor == idProfesor
                        select new GrupoDocenteDTO
                        {
                            IdMateriaSalon = ms.IdMateriaSalon,
                            Materia = m.Descripcion,
                            Salon = s.Descripcion,
                            Horario = $"Lun-Juv:{ms.HlunJuv}) (V:{ms.Hviernes}) (S:{ms.Hsabados}"
                        };

            return await query.ToListAsync();
        }

        // GET: api/Profesors/AsistenciasGrupo
        [HttpGet("AsistenciasGrupo")]
        public async Task<ActionResult<IEnumerable<AsistenciaDocenteItemDTO>>> GetAsistenciasPorGrupo(
            [FromQuery] int idMateriaSalon,
            [FromQuery] DateTime fecha) // Pasamos DateOnly o DateTime
        {
            var fechaOnly = DateOnly.FromDateTime(fecha);

            var query = from a in _context.Asistencia
                        join hms in _context.HorarioMateriaSalons on a.ID_HorarioMateriaSalon equals hms.IdHorarioMateriaSalon
                        join h in _context.Horarios on hms.IdHorario equals h.IdHorario
                        join al in _context.Alumnos on h.NumeroControl equals al.NumeroControl

                        where hms.IdMateriaSalon == idMateriaSalon && a.Fecha == fechaOnly

                        select new AsistenciaDocenteItemDTO
                        {
                            IdAsistencia = a.IdAsistencia,
                            NumeroControl = al.NumeroControl,
                            NombreAlumno = al.Nombre,
                            Estatus = a.Estatus,
                            Hora = a.Hora.ToString()
                        };

            return await query.ToListAsync();
        }
        [HttpPut("ActualizarEstatus")]
        public async Task<IActionResult> ActualizarEstatus([FromBody] ActualizarAsistenciaDTO dto)
        {
            var asistencia = await _context.Asistencia.FindAsync(dto.IdAsistencia);

            if (asistencia == null)
            {
                return NotFound("No se encontró el registro de asistencia.");
            }

            asistencia.Estatus = dto.NuevoEstatus;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Estatus actualizado correctamente" });
        }
    }
}