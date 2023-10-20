using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using VibeScopyAPI.Infrastructure;
using VibeScopyAPI.Models;
using VibeScopyAPI2.Dto;

namespace VibeScopyAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilePropositionController : ControllerBase
    {
        private readonly VibeScopUnitOfWork _context;

        public ProfilePropositionController(VibeScopUnitOfWork context)
        {
            _context = context;
        }

        // GET: api/ProfileProposition
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfileProposition>>> GetProfilePropositions()
        {
          if (_context.ProfilePropositions == null)
          {
              return NotFound();
          }
          var test = await _context.ProfilePropositions.ToListAsync();
          return test;
        }

        // GET: api/ProfileProposition/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileProposition>> GetProfileProposition(Guid id)
        {
          if (_context.ProfilePropositions == null)
          {
              return NotFound();
          }
            var profileProposition = await _context.ProfilePropositions.FindAsync(id);

            if (profileProposition == null)
            {
                return NotFound();
            }

            return profileProposition;
        }

        // POST: api/ProfileProposition
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProfileProposition>> PostProfileProposition(ProfilePropositionCreateDto profilePropositionDto)
        {
            if (_context.ProfilePropositions == null)
            {
                return Problem("Entity set 'UnitOfWorkToto.ProfilePropositions'  is null.");
            }

            ProfileProposition profileProposition = new ProfileProposition()
            {
                AnswersFilaments = profilePropositionDto.AnswersFilaments,
                LastLocation = new Point(new Coordinate(3.3,3.5))
            };

            _context.ProfilePropositions.Add(profileProposition);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProfileProposition", new { id = profileProposition.Id }, profileProposition);
        }

        // DELETE: api/ProfileProposition/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfileProposition(Guid id)
        {
            if (_context.ProfilePropositions == null)
            {
                return NotFound();
            }
            var profileProposition = await _context.ProfilePropositions.FindAsync(id);
            if (profileProposition == null)
            {
                return NotFound();
            }

            _context.ProfilePropositions.Remove(profileProposition);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //private bool ProfilePropositionExists(Guid id)
        //{
        //    return (_context.ProfilePropositions?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
