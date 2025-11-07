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
    public class SalonController : ControllerBase
    {
        private readonly ControlAsistenciasContext _context;

        public SalonController(ControlAsistenciasContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalonDTO>>> GetSalons()
        {
            var salons = await _context.Salons
                .Select(s => new SalonDTO
                {
                    IdSalon = s.IdSalon,
                    IdEscaner = s.IdEscaner,
                    Descripcion = s.Descripcion
                })
                .ToListAsync();

            return salons;
        }

        // GET: api/Salons1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalonDTO>> GetSalon(int id)
        {
            var salon = await _context.Salons.FindAsync(id);

            if (salon == null)
            {
                return NotFound();
            }

            var dto = new SalonDTO
            {
                IdSalon = salon.IdSalon,
                IdEscaner = salon.IdEscaner,
                Descripcion = salon.Descripcion
            };

            return dto;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalon(int id, SalonDTO salonDto)
        {
            if (id != salonDto.IdSalon)
            {
                return BadRequest();
            }

            var salon = await _context.Salons.FindAsync(id);
            if (salon == null)
            {
                return NotFound();
            }

            salon.IdEscaner = salonDto.IdEscaner;
            salon.Descripcion = salonDto.Descripcion;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalonExists(id))
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
        public async Task<ActionResult<SalonDTO>> PostSalon(SalonDTO salonDto)
        {
            var salon = new Salon
            {
                Descripcion = salonDto.Descripcion,
                IdEscaner = salonDto.IdEscaner
            };

            _context.Salons.Add(salon);
            await _context.SaveChangesAsync();

            salonDto.IdSalon = salon.IdSalon;

            return CreatedAtAction(nameof(GetSalon), new { id = salon.IdSalon }, salonDto);
        }



        // DELETE: api/Salons1/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalon(int id)
        {
            var salon = await _context.Salons.FindAsync(id);
            if (salon == null)
            {
                return NotFound();
            }
            _context.Salons.Remove(salon);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool SalonExists(int id)
        {
            return _context.Salons.Any(e => e.IdSalon == id);
        }
    }
}
