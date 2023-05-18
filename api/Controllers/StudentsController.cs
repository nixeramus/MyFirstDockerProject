using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Data;
using api.Models;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(AppDbContext context, ILogger<StudentsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/students
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Student>), 200)]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            _logger.LogInformation("Getting all students");

            return await _context.Students.ToListAsync();
        }

        // GET: api/students/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Student), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            _logger.LogInformation("Getting student with ID: {id}", id);

            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                _logger.LogWarning("Student with ID {id} not found", id);
                return NotFound();
            }

            return student;
        }

        // POST: api/students
        [HttpPost]
        [ProducesResponseType(typeof(Student), 201)]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            _logger.LogInformation("Creating a new student");

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            _logger.LogInformation("New student created with ID: {id}", student.ID);

            return CreatedAtAction(nameof(GetStudent), new { id = student.ID }, student);
        }

        // PUT: api/students/{id}
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            _logger.LogInformation("Updating student with ID: {id}", id);

            if (id != student.ID)
            {
                _logger.LogWarning("Request ID does not match student ID");
                return BadRequest();
            }

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("Student with ID {id} updated successfully", id);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    _logger.LogWarning("Student with ID {id} not found", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/students/{id}
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            _logger.LogInformation("Deleting student with ID: {id}", id);

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Student with ID {id} not found", id);
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Student with ID {id} deleted successfully", id);

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
