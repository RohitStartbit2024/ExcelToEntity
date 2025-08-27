using ExcelToEntity;
using ExcelToEntityApiUsage.Context;
using ExcelToEntityApiUsage.Models.DbModels;
using Microsoft.AspNetCore.Mvc;

namespace ExcelToEntityApiUsage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportStudents(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // Save file temporarily with correct extension
            var tempFilePath = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");

            using (var stream = new FileStream(tempFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var importer = new ExcelImporter();
            var result = importer.Import<Student>(tempFilePath);

            if (result.Data.Any())
            {
                _context.Students.AddRange(result.Data);
                await _context.SaveChangesAsync();
            }

            System.IO.File.Delete(tempFilePath);

            return Ok(new
            {
                ImportedCount = result.Data.Count,
                Errors = result.Errors.Select(e => new
                {
                    e.RowNumber,
                    e.ColumnName,
                    e.Message
                })
            });
        }

    }
}
