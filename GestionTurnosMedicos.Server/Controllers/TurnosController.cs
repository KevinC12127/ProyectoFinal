using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

[ApiController]
[Route("api/[controller]")]
public class TurnosController : ControllerBase
{
    private readonly TurnosContext _context;

    public TurnosController(TurnosContext context)
    {
        _context = context;
    }

    // GET: api/Turnos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Turno>>> GetTurnos()
    {
        return await _context.Turnos.ToListAsync();
    }

    // GET: api/Turnos/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Turno>> GetTurno(int id)
    {
        var turno = await _context.Turnos.FindAsync(id);
        if (turno == null) return NotFound();
        return turno;
    }

    // POST: api/Turnos
    [HttpPost]
    public async Task<ActionResult<Turno>> PostTurno([FromBody] Turno turno)
    {
        if (turno == null)
            return BadRequest("Datos inválidos.");

        // Validaciones de campos obligatorios
        if (string.IsNullOrWhiteSpace(turno.NombrePaciente) ||
            string.IsNullOrWhiteSpace(turno.Cedula) ||
            string.IsNullOrWhiteSpace(turno.Especialidad) ||
            turno.Fecha == default ||
            string.IsNullOrWhiteSpace(turno.Hora))
        {
            return BadRequest("Todos los campos son obligatorios.");
        }

        // Asignar fecha de registro si no viene desde el frontend
        turno.FechaRegistro = DateTime.Now;
        turno.UpdatedAt = DateTime.UtcNow;
        turno.SyncKey = TurnoMapper.BuildSyncKey(turno.Cedula, turno.Fecha, turno.Hora, turno.Especialidad);

        // Guardar en base de datos
        _context.Turnos.Add(turno);
        await _context.SaveChangesAsync();

        // Confirmación exitosa
        return Ok(turno); // ✅ Devuelve 200 OK con el turno registrado
    }

    // PUT: api/Turnos/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutTurno(int id, [FromBody] Turno turno)
    {
        if (turno == null || id != turno.Id)
            return BadRequest("Datos inválidos.");

        var existing = await _context.Turnos.FindAsync(id);
        if (existing == null) return NotFound();

        existing.NombrePaciente = turno.NombrePaciente;
        existing.Cedula = turno.Cedula;
        existing.Especialidad = turno.Especialidad;
        existing.Fecha = turno.Fecha;
        existing.Hora = turno.Hora;
        existing.SyncKey = TurnoMapper.BuildSyncKey(turno.Cedula, turno.Fecha, turno.Hora, turno.Especialidad);
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Turnos/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTurno(int id)
    {
        var turno = await _context.Turnos.FindAsync(id);
        if (turno == null) return NotFound();

        _context.Turnos.Remove(turno);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
