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
    public class MateriaSalonsController : ControllerBase
    {
        private readonly ControlAsistenciasContext _context;

        public MateriaSalonsController(ControlAsistenciasContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<MateriaSalonDTO>>> GetMateriaSalon()
        {
            var salons = await _context.MateriaSalons
                .Select(s => new MateriaSalonDTO
                {
                    IdMateriaSalon = s.IdMateriaSalon,
                    Idsalon = s.IdSalon,
                    IdMateria = s.IdMateria
                })
                .ToListAsync();
            return salons;
        }

        // GET: api/Salons1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MateriaSalonDTO>> GetMateriaSalon(int id)
        {
            var salon = await _context.MateriaSalons.FindAsync(id);

            if (salon == null)
            {
                return NotFound();
            }

            var dto = new MateriaSalonDTO
            {
                    Idsalon = salon.IdSalon,
                IdMateria = salon.IdMateria,
                IdMateriaSalon = salon.IdMateriaSalon
                };

            return dto;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutMateriaSalon(int id, MateriaSalonDTO MateriaSalonDto)
        {
            if (id != MateriaSalonDto.IdMateriaSalon)
            {
                return BadRequest();
            }

            var MateriaSalon = await _context.MateriaSalons.FindAsync(id);
            if (MateriaSalon == null)
            {
                return NotFound();
            }

            MateriaSalon.IdSalon = MateriaSalonDto.Idsalon;
            MateriaSalon.IdMateria = MateriaSalonDto.IdMateria;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MateriaSalonExists(id))
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
        public async Task<ActionResult<MateriaSalonDTO>> PostMateriaSalon(MateriaSalonDTO materiaSalonDto)
        {
            var materiaSalon = new MateriaSalon
            {
                IdMateria = materiaSalonDto.IdMateria,
                IdSalon = materiaSalonDto.Idsalon
            };

            _context.MateriaSalons.Add(materiaSalon);
            await _context.SaveChangesAsync();

            materiaSalonDto.IdMateriaSalon = materiaSalon.IdMateriaSalon;

            return CreatedAtAction(nameof(GetMateriaSalon), new { id = materiaSalon.IdMateriaSalon }, materiaSalonDto);
        }


        // DELETE: api/MateriaSalons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMateriaSalon(int id)
        {
            var materiaSalon = await _context.MateriaSalons.FindAsync(id);
            if (materiaSalon == null)
            {
                return NotFound();
            }

            _context.MateriaSalons.Remove(materiaSalon);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MateriaSalonExists(int id)
        {
            return _context.MateriaSalons.Any(e => e.IdMateriaSalon == id);
        }
    }
}
