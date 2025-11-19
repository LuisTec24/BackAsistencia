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

        [HttpGet("mis-grupos")]
        [Authorize(Roles = "Maestro")]
        public async Task<ActionResult<IEnumerable<ClaseConHorarioDTO>>> GetMisGrupos()
        {
            var idProfesorStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(idProfesorStr, out int idProfesor)) return Unauthorized();

            // 1. Obtener IDs de las materias que da el profe
            var materiasDelProfe = await _context.ProfesorMateria
                .Where(pm => pm.IdProfesor == idProfesor)
                .Select(pm => pm.IdMateria)
                .ToListAsync();

            // 2. Obtener TODOS los grupos (HorarioMateriaSalon) de esas materias
                var grupos = await _context.HorarioMateriaSalons
                .Include(h => h.IdMateriaSalonNavigation.IdMateriaNavigation)
                .Include(h => h.IdMateriaSalonNavigation.IdSalonNavigation)
                .Where(h => materiasDelProfe.Contains(h.IdMateriaSalonNavigation.IdMateria))
                .Select(h => new ClaseConHorarioDTO
                {
                    IdGrupo = h.IdHorarioMateriaSalon,
                    DescripcionCompleta = h.IdMateriaSalonNavigation.IdMateriaNavigation.Descripcion +
                                          " - " + h.IdMateriaSalonNavigation.IdSalonNavigation.Descripcion +
                                             " (" + (h.HlunJuv ?? h.Hsabados ?? h.Hviernes ?? "S/H") + ")"
                })
                .ToListAsync();

            return Ok(grupos);
        }
    }
}