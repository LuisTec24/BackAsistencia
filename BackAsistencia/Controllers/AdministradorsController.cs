using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackAsistencia.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackAsistencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministradorsController : ControllerBase
    {
        private readonly ControlAsistenciasContext _context;

        public AdministradorsController(ControlAsistenciasContext context)
        {
            _context = context;
        }

        // GET: api/Administradors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Administrador>>> GetAdministrador()
        {
            return await _context.Administrador.ToListAsync();
        }

        // GET: api/Administradors/{correo}
        [HttpGet("{correo}")]
        public async Task<ActionResult<Administrador>> GetAdministrador(string correo)
        {
            var administrador = await _context.Administrador
                .FirstOrDefaultAsync(a => a.Correo == correo);

            if (administrador == null)
            {
                return NotFound("No se encontró el Administrador.");
            }

            return administrador;
        }

        // PUT: api/Administradors/{correo}
        [HttpPut("{correo}")]
        public async Task<IActionResult> PutAdministrador(string correo, [FromBody] Administrador dto)
        {
            if (correo != dto.Correo)
            {
                return BadRequest("El Correo en la URL no coincide con el del cuerpo.");
            }

            var administrador = await _context.Administrador
                .FirstOrDefaultAsync(a => a.Correo == correo);

            if (administrador == null)
            {
                return NotFound("No se encontró el Administrador.");
            }

            administrador.Nombre = dto.Nombre;
            administrador.Correo = dto.Correo;

            // Encriptar solo si la contraseña fue modificada
            if (!string.IsNullOrWhiteSpace(dto.Contraseña) &&
                !BCrypt.Net.BCrypt.Verify(dto.Contraseña, administrador.Contraseña))
            {
                administrador.Contraseña = BCrypt.Net.BCrypt.HashPassword(dto.Contraseña);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdministradorExists(correo))
                {
                    return NotFound("El Administrador ya no existe.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Administradors
        [HttpPost]
        public async Task<ActionResult<Administrador>> PostAdministrador(Administrador administrador)
        {
            var existe = await _context.Administrador.AnyAsync(a => a.Correo == administrador.Correo);
            if (existe)
                return Conflict("Ya existe un administrador con ese correo.");

            var nuevo = new Administrador
            {
                Nombre = administrador.Nombre,
                Correo = administrador.Correo,
                Contraseña = BCrypt.Net.BCrypt.HashPassword(administrador.Contraseña),
                };

            _context.Administrador.Add(nuevo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAdministrador", new { correo = administrador.Correo }, nuevo);
        }

        // DELETE: api/Administradors/{correo}
        [HttpDelete("{correo}")]
        public async Task<IActionResult> DeleteAdministrador(string correo)
        {
            var administrador = await _context.Administrador
                .FirstOrDefaultAsync(a => a.Correo == correo);

            if (administrador == null)
            {
                return NotFound("No se encontró el Administrador.");
            }

            _context.Administrador.Remove(administrador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdministradorExists(string correo)
        {
            return _context.Administrador.Any(e => e.Correo == correo);
        }
    }
}