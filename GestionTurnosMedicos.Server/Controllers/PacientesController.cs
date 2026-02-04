using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly TurnosContext _context;

    public PacientesController(TurnosContext context)
    {
        _context = context;
    }

    // GET: api/Pacientes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientes()
    {
        return await _context.Pacientes.ToListAsync();
    }

    // GET: api/Pacientes/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Paciente>> GetPaciente(int id)
    {
        var paciente = await _context.Pacientes.FindAsync(id);
        if (paciente == null) return NotFound();
        return paciente;
    }

    // POST: api/Pacientes
    [HttpPost]
    public async Task<ActionResult<Paciente>> PostPaciente([FromBody] Paciente paciente)
    {
        if (paciente == null || string.IsNullOrEmpty(paciente.Nombre))
            return BadRequest("Datos inválidos.");

        _context.Pacientes.Add(paciente);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPaciente), new { id = paciente.Id }, paciente);
    }

    // PUT: api/Pacientes/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutPaciente(int id, [FromBody] Paciente paciente)
    {
        if (paciente == null || id != paciente.Id)
            return BadRequest("Datos inválidos.");

        var existing = await _context.Pacientes.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Nombre = paciente.Nombre;
        existing.Cedula = paciente.Cedula;
        existing.Telefono = paciente.Telefono;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Pacientes/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePaciente(int id)
    {
        var paciente = await _context.Pacientes.FindAsync(id);
        if (paciente == null) return NotFound();

        _context.Pacientes.Remove(paciente);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
