using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Threading.Tasks;
using BackAsistencia.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EFCore.BulkExtensions; // este es el correcto


using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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

        // PUT: api/Asistenciums/5x
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("UpdateAsistencia")]
        public async Task<IActionResult> UpdateAsistencia(int idAsistencia, [FromBody] UpdateAsistenciaDTO asistencia)
        {
            var asistencium = await _context.Asistencia.FindAsync(idAsistencia);

            if (asistencium == null)
            {
                return NotFound("No se encontró la asistencia.");
            }

            // Actualizar campos
            if (asistencia.Estatus != null)
                asistencium.Estatus = asistencia.Estatus;

            // FIX: DateOnly is a non-nullable value type, so check for default value instead of null
            if (asistencia.Fecha != default)
                asistencium.Fecha = asistencia.Fecha;

            if (asistencia.Hora != default)
                asistencium.Hora = asistencia.Hora;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AsistenciumExists(idAsistencia))
                {
                    return NotFound("La asistencia ya no existe.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        private bool AsistenciumExists(int id)
        {
            return _context.Asistencia.Any(e => e.IdAsistencia == id);
        }



        //metodo verificar Materia Scaneer
        // sacar hora actual y fecha actual
        //sacar en que salon esta mediante el idscaner 
        //saco la materia con el que coincida el idsalon en materiasalon
        // saco la conicidencia con Horariomateriasalon y comparo el dia para
        // ver si coincide el horario con el que se ingreso (HLun,Hv,HS)
        // si coincide guardo la asistencia si no mando mensaje de error
        // buscar 
        [HttpPost("scanner")]
        public async Task<ActionResult<Salon>> Scanner(int IdScanner,string Nc)
        {
            //pruebas
            var ahora = DateTime.Today.AddHours(12).AddMinutes(30);
            // var ahora = new DateTime(2025, 11, 18, 12, 30, 0);
            // Hoy a las 9:30 AM
            //12:00-13:00 hay una materia de
            
            // var ahora = DateTime.Now;//
            // que dia de la semana es 1,2,3
            int grupoDia = ahora.DayOfWeek switch
            {
                DayOfWeek.Monday => 1,
                DayOfWeek.Tuesday => 1,
                DayOfWeek.Wednesday => 1,
                DayOfWeek.Thursday => 1,
                DayOfWeek.Friday => 2,
                DayOfWeek.Saturday => 3,
                _ => 0 // Domingo u otro caso no definido
            };
            TimeSpan hora = ahora.TimeOfDay;

            // saco la lista de horarios 
            int IdSalon = await _context.Salons.Where(a => a.IdEscaner == IdScanner).Select(a => a.IdSalon).FirstOrDefaultAsync();
            var IdMateriaSalon = await _context.MateriaSalons.Where(a => a.IdSalon == IdSalon).Select(a => a.IdMateriaSalon).ToListAsync();
            var listaHorariosMaterias = await _context.HorarioMateriaSalons.Where(a => IdMateriaSalon.Contains(a.IdMateriaSalon)).Select(a => a.IdHorarioMateriaSalon).ToListAsync();
            List<ScannerHorarioMateriaSalon> HoraDiaYIHMS=null!;
            int IdHorarioMateriaSalon=0;
            // busca el horario del dia correspondiente segun el dia 
            switch (grupoDia)
            {
                case 1:
                    HoraDiaYIHMS = await _context.HorarioMateriaSalons
                    .Where(a => IdMateriaSalon.Contains(a.IdMateriaSalon))
                    .Select(a => new ScannerHorarioMateriaSalon
                    {
                        HorarioDia = a.HlunJuv,
                        IdHorarioMateriaSalon = a.IdHorarioMateriaSalon
                    })
                    .ToListAsync();
                    break;

                case 2:
                    HoraDiaYIHMS = await _context.HorarioMateriaSalons
                    .Where(a => IdMateriaSalon.Contains(a.IdMateriaSalon))
                    .Select(a => new ScannerHorarioMateriaSalon
                    {
                    HorarioDia = a.Hviernes,
                    IdHorarioMateriaSalon = a.IdHorarioMateriaSalon
                    })
                    .ToListAsync();
                    break;


                case 3:
                    HoraDiaYIHMS = await _context.HorarioMateriaSalons
                    .Where(a => IdMateriaSalon.Contains(a.IdMateriaSalon))
                    .Select(a => new ScannerHorarioMateriaSalon
                    {
                        HorarioDia = a.Hsabados,
                        IdHorarioMateriaSalon = a.IdHorarioMateriaSalon
                    })
                    .ToListAsync();
                    break;
            }
            var Estatus="Ausente";
            int IdRefencia = 0;
            foreach (var horario in HoraDiaYIHMS)
            {  
            
            //}//for (int i = 0; i < HoraDiaYIHMS.Count; i++)//{//    var horario = HoraDiaYIHMS[i];
            // Asegúrate de que el formato sea "HH:mm-HH:mm"

                if (!string.IsNullOrEmpty(horario.HorarioDia) && horario.HorarioDia.Contains('-'))
                {
                    var partes = horario.HorarioDia.Split('-');
                    if (TimeSpan.TryParse(partes[0], out TimeSpan inicio) &&
                        TimeSpan.TryParse(partes[1], out TimeSpan fin))
                    {
                        // Comparar si la hora actual está dentro del rango
                        if (hora >= inicio && hora <= fin)
                        {

                            IdHorarioMateriaSalon = horario.IdHorarioMateriaSalon;

                            
                            var PrimeraAsistencia = await _context.Asistencia.AnyAsync(a => a.ID_HorarioMateriaSalon == IdHorarioMateriaSalon  && a.Fecha == DateOnly.FromDateTime(ahora));
                            
                            
                            if (!PrimeraAsistencia)
                            {
                                // entro si no hay ninguna asistencia registrada para esa materia


                                //saco la lista de alumnos inscritos en esa materia
                                var ListaIdHorario = await _context.HorarioMateriaSalons.Where(a => a.IdHorarioMateriaSalon == IdHorarioMateriaSalon).Select(a => a.IdHorario).ToListAsync();
                                var ListaAlumnos = await _context.Horarios.Where(a => ListaIdHorario.Contains(a.IdHorario)).Select(a => a.NumeroControl).ToListAsync();


                                //registrar faltas
                                var respuesta = await BulkAsistencias(IdHorarioMateriaSalon,ListaAlumnos);

                                
                            }
                            // si es o no es el primero lo unico que cambiaria seria el inicio, el final de editar la asistencia se mantiene siempre
                            //actualizar el status del alumno que dio check
                            Estatus = Estado(inicio, hora);
                            IdRefencia = 1;
                            break; // Si solo necesitas el primero que coincida

                            //var ExisteAsistencia = await _context.Asistencia.AnyAsync(a => a.ID_HorarioMateriaSalon == IdHorarioMateriaSalon && a.NumeroControl == Nc && a.Fecha == DateOnly.FromDateTime(ahora));
                            //if (ExisteAsistencia) 
                            //    break;

                        }

                    }
                }
            }


            //si hubo coincidencias entrass 
            if (IdRefencia == 1)
            {
                var NuevoDTO = new UpdateAsistenciaDTO
                {
                    Fecha = DateOnly.FromDateTime(ahora),
                    Hora = TimeOnly.FromDateTime(ahora),
                    Estatus = Estatus
        };


                var idAsistencia = await _context.Asistencia.Where(a => a.ID_HorarioMateriaSalon == IdHorarioMateriaSalon && a.NumeroControl == Nc && a.Fecha == DateOnly.FromDateTime(ahora)).Select(a => a.IdAsistencia).FirstAsync();

                // se cambiara por el metodo PutAsistencia
                var resp = await UpdateAsistencia(idAsistencia,NuevoDTO);
            }
            
                return Ok(IdHorarioMateriaSalon);
            }


        // metodo para registrar asistencias en bulk
        [HttpPost("bulk-asistencias")]
        public async Task<IActionResult> BulkAsistencias(int idmaterisalon,[FromBody] List<String> asistencias)
        {
            var ahora = DateTime.Now;//
            var entidades = asistencias.Select(a => new Asistencia
            {
                ID_HorarioMateriaSalon = idmaterisalon,
                NumeroControl = a,
                Estatus = "Falta",
                Fecha = DateOnly.FromDateTime(ahora),
                Hora = TimeOnly.FromDateTime(ahora),

            }).ToList();

            await _context.BulkInsertAsync(entidades);

            return Ok(new { count = entidades.Count });
        }


        // devuelve el estado de la asistencia
        private String Estado(TimeSpan inicio, TimeSpan hora)
        {
            string Estatus;
            var d = inicio.Add(TimeSpan.FromMinutes(15));
            if (hora <= d)
            {
                //    IdRefencia = 1;
                Estatus = "Presente";
            }
            else
            {
                //  IdRefencia = 1;
                Estatus = "Retardo";
            }

            return Estatus;
        }



        [HttpPost]
        public async Task<ActionResult<Asistencia>> PostAsistencium([FromBody] CrearAsistenciaDTO dto)
        {
            // Validación opcional
            var alumnoExiste = await _context.Alumnos.AnyAsync(a => a.NumeroControl == dto.NumeroControl);
            var materiaSalonExiste = await _context.HorarioMateriaSalons.AnyAsync(ms => ms.IdHorarioMateriaSalon == dto.ID_HorarioMateriaSalon);

            if (!alumnoExiste || !materiaSalonExiste)
                return BadRequest("Alumno o Materia-Salón no encontrados.");

            var nueva = new Asistencia
            {
                ID_HorarioMateriaSalon = dto.ID_HorarioMateriaSalon,
                NumeroControl = dto.NumeroControl,
                Fecha = dto.Fecha,
                Hora = dto.Hora,
                Estatus= dto.Estatus,
    };

            _context.Asistencia.Add(nueva);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetAsistencium", new { id = nueva.IdAsistencia }, nueva);
        }

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


        // Comienza el metodo para el Historial de Asistencias.
        [HttpGet("mi-historial-hoy")]
        [Authorize(Roles = "Alumno")]
        public async Task<ActionResult<IEnumerable<AsistenciaItemDTO>>> GetHistorialDeHoy()
        {
            var numeroControlStr = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(numeroControlStr))
            {
                return Unauthorized("Token inválido.");
            }

            // 2. Obtiene la fecha de hoy
            var hoy = DateOnly.FromDateTime(DateTime.Today);


            //var hoy = new DateOnly(2025, 11, 10);

            var asistencias = await _context.Asistencia
                .Where(a => a.NumeroControl == numeroControlStr && a.Fecha == hoy)
                .OrderBy(a => a.Hora) 
                .Select(a => new AsistenciaItemDTO
                {
                    Materia = a.ID_HorarioMateriaSalonNavigation.IdMateriaSalonNavigation.IdMateriaNavigation.Descripcion,
                    Fecha = a.Fecha,
                    Hora = a.Hora
                })
                .ToListAsync();

            return Ok(asistencias);
        }

        [HttpGet("reporte-por-grupo")]
        [Authorize(Roles = "Maestro")]
        public async Task<ActionResult<IEnumerable<AsistenciaReporteItemDTO>>> GetReportePorGrupo(
        [FromQuery] int grupoId, // Recibe el ID_HorarioMateriaSalon
        [FromQuery] string fecha)
        {
            if (!DateOnly.TryParse(fecha, out var fechaOnly)) return BadRequest("Fecha inválida.");

            // Consulta directa y exacta
            var reporte = await _context.Asistencia
                .Include(a => a.NumeroControlNavigation)
                .Where(a => a.ID_HorarioMateriaSalon == grupoId && a.Fecha == fechaOnly)
                .Select(a => new AsistenciaReporteItemDTO
                {
                    IdAsistencia = a.IdAsistencia,
                    NumeroControl = a.NumeroControl,
                    NombreAlumno = a.NumeroControlNavigation.Nombre,
                    Estatus = a.Estatus 
                })
                .ToListAsync();

            return Ok(reporte);
        }
    }
}
