using BackAsistencia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackAsistencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RevisaExistenteController : ControllerBase
    {
        private readonly ControlAsistenciasContext _context;
        private readonly string secretkey;
        private readonly IConfiguration _configuration;

        public RevisaExistenteController(ControlAsistenciasContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        [HttpPost("ValidarMaestro")]
        [AllowAnonymous]
        public async Task<IActionResult> VerificarMaestro([FromBody] LoginDTOM request)
        {

            var maestro = await _context.Profesors.FirstOrDefaultAsync(m => m.Correo == request.Correo);
            if (maestro != null && BCrypt.Net.BCrypt.Verify(request.PasswordHash,maestro.Contrasena))
            {
                var token = GenerarToken(maestro.IdProfesor, maestro.Nombre, "Maestro");
                return Ok(new { token, rol = "Maestro" });
            }
            else

            return Unauthorized("Credenciales inválidas");
        }


        [HttpPost("ValidarAlumno")]
        [AllowAnonymous]
        public async Task<IActionResult> VerificarAlumno([FromBody] LoginDTO request)
        {

            var alumno = await _context.Alumnos.FirstOrDefaultAsync(a => a.NumeroControl == request.NC);
            if (alumno != null && BCrypt.Net.BCrypt.Verify(request.PasswordHash, alumno.Contrasena))
            {
                var token = GenerarToken(alumno.NumeroControl, alumno.Nombre, "Alumno");
                return Ok(new { token, rol = "Alumno" });
            }
            return Unauthorized("Credenciales inválidas");
        }


        private string GenerarToken(int identificador, string nombre, string rol)
        {
            var secretKey = _configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
                throw new InvalidOperationException("La clave secreta JWT no está configurada.");

            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var claims = new ClaimsIdentity(new[]
            {
        new Claim(ClaimTypes.NameIdentifier, ""+identificador),
        new Claim(ClaimTypes.Name, nombre),
        new Claim(ClaimTypes.Role, rol)
    });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        //[HttpGet]
        //[Authorize(Roles = "1")]
        //public async Task<ActionResult<IEnumerable<Alumno>>> GetUsuarios()
        //{
        //    return await _context.Alumnos.ToListAsync();
        //}





        //[HttpPost]
        //[Authorize(Roles = "1")]
        //public async Task<IActionResult> CrearAlumno([FromBody] AlumnoDTO nuevoUsuario)
        //{
        //    if (await _context.Alumnos.AnyAsync(u => u.NumeroControl == nuevoUsuario.NumeroControl))
        //        return Conflict("El correo ya existe");

        //    nuevoUsuario.NumeroControl = 0;
        //    nuevoUsuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.Contrasena);

        //    //nuevoUsuario.Nombre ="";
        //    //nuevoUsuario.Carrera = "";
        //    //nuevoUsuario.Semestre = "";
        //    //nuevoUsuario.Contrasena = "";


        //    _context.Alumnos.Add(nuevoUsuario);
        //    await _context.SaveChangesAsync();
        //    return Ok(nuevoUsuario);
        //}

        //[HttpGet("VerificarCorreo")]
        //[Authorize(Roles = "1")]
        //public async Task<IActionResult> VerificarCorreo([FromQuery] string correo)
        //{
        //    if (string.IsNullOrWhiteSpace(correo))
        //    {
        //        return BadRequest("El correo no puede estar vacío");
        //    }

        //    // Verifica si el correo existe en la tabla de usuarios (o proveedores, según tu modelo)
        //    bool existe = await _context.Alumnos.AnyAsync(u => u.Correo == correo);

        //    return Ok(new { existe });
        //}


        //[HttpPut("{correo}")]
        //[Authorize(Roles = "1")]

        //public async Task<IActionResult> PutUsuarioPorCorreo(string correo, Usuarios usuarioActualizado)
        //{
        //    var usuarioExistente = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
        //    if (usuarioExistente == null)
        //        return NotFound();

        //    usuarioExistente.Nombre = usuarioActualizado.Nombre;
        //    usuarioExistente.Contraseña_hash = usuarioActualizado.Contraseña_hash;
        //    usuarioExistente.Rol_id = usuarioActualizado.Rol_id;

        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //[HttpDelete("{correo}")]
        //[Authorize(Roles = "1")]
        //public async Task<IActionResult> DeleteUsuario(string correo)
        //{
        //    var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
        //    if (usuario == null)
        //        return NotFound();

        //    _context.Usuarios.Remove(usuario);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
    }
}
