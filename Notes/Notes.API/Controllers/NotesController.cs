using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.API.Data;
using Notes.API.Models.Entities;

namespace Notes.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : Controller
    {
        private readonly NotesDbContext notesDbContext;
        public NotesController(NotesDbContext notesDbContext)
        {
            this.notesDbContext = notesDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            // Notes from database
            return Ok(await notesDbContext.Notes.ToListAsync());
        }
        [HttpGet]
        [Route("{id}:Guid")]
        [ActionName("GetNoteById")]
        public async Task<IActionResult> GetNoteByID([FromRoute] Guid id)
        {
            // Notes from database
            var note = await notesDbContext.Notes.FindAsync(id);

            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }
        [HttpPost]
        [ActionName("AddNote")]
        public async Task<IActionResult> AddNote(Note note)
        {
            note.id = Guid.NewGuid();
            await notesDbContext.Notes.AddAsync(note);
            await notesDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNoteByID), new { Id = note.id }, note);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        [ActionName("UpdateNote")]
        public async Task<IActionResult> UpdateNote([FromRoute] Guid id, [FromBody] Note updatedNote)
        {
            var existingnote = await notesDbContext.Notes.FindAsync(id);
            if (existingnote == null)
            {
                return NotFound();
            }
            existingnote.Ttile = updatedNote.Ttile;
            existingnote.Description = updatedNote.Description;
            existingnote.IsVisible = updatedNote.IsVisible;
            await notesDbContext.SaveChangesAsync();
            return Ok(existingnote);
        }
        [HttpDelete]
        [Route("{id:Guid}")]
        [ActionName("DeleteNode")]
        public async Task<IActionResult> DeleteNote([FromRoute] Guid id)
        {
            var existingNote = await notesDbContext.Notes.FindAsync(id);
            if (existingNote == null)
            {
                return NotFound();
            }
            notesDbContext.Notes.Remove(existingNote);
            await notesDbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
