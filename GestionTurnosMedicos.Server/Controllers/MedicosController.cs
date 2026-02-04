using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class MedicosController : ControllerBase
{
    private readonly TurnosContext _context;

    public MedicosController(TurnosContext context)
    {
        _context = context;
    }

    // GET: api/Medicos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Medico>>> GetMedicos()
    {
        return await _context.Medicos.ToListAsync();
    }

    // GET: api/Medicos/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Medico>> GetMedico(int id)
    {
        var medico = await _context.Medicos.FindAsync(id);
        if (medico == null) return NotFound();
        return medico;
    }

    // POST: api/Medicos
    [HttpPost]
    public async Task<ActionResult<Medico>> PostMedico([FromBody] Medico medico)
    {
        if (medico == null || string.IsNullOrEmpty(medico.Nombre))
            return BadRequest("Datos inválidos.");

        _context.Medicos.Add(medico);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetMedico), new { id = medico.Id }, medico);
    }

    // PUT: api/Medicos/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutMedico(int id, [FromBody] Medico medico)
    {
        if (medico == null || id != medico.Id)
            return BadRequest("Datos inválidos.");

        var existing = await _context.Medicos.FindAsync(id);
        if (existing == null) return NotFound();

        existing.Nombre = medico.Nombre;
        existing.Especialidad = medico.Especialidad;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Medicos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMedico(int id)
    {
        var medico = await _context.Medicos.FindAsync(id);
        if (medico == null) return NotFound();

        _context.Medicos.Remove(medico);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
