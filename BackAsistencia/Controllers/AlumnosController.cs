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
        public async Task<ActionResult<AlumnoDTO>> GetAlumno(int id)
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
        public async Task<IActionResult> PutAlumno(int id, [FromBody] AlumnoDTO dto)
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

            // Actualizar solo los campos definidos en el DTO
            alumno.Nombre = dto.Nombre;
            alumno.Carrera = dto.Carrera;
            alumno.Semestre = dto.Semestre;
            alumno.Contrasena = dto.Contrasena; // Considera encriptarla si es sensible

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

        //// POST: api/Alumnoes
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Alumno>> PostAlumno(Alumno alumno)
        //{
        //    _context.Alumnos.Add(alumno);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (AlumnoExists(alumno.NumeroControl))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }


        //    return CreatedAtAction("GetAlumno", new { id = alumno.NumeroControl }, alumno);
        //}

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
                Contrasena = dto.Contrasena // Considera encriptarla si es sensible
            };

            _context.Alumnos.Add(nuevo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAlumno", new { id = nuevo.NumeroControl }, nuevo);
        }

        // DELETE: api/Alumnoes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlumno(int id)
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

        private bool AlumnoExists(int id)
        {
            return _context.Alumnos.Any(e => e.NumeroControl == id);
        }
    }
}
