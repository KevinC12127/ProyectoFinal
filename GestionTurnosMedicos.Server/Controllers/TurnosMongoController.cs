using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/turnos-mongo")]
public class TurnosMongoController : ControllerBase
{
    private readonly IMongoTurnoRepository _mongoRepo;

    public TurnosMongoController(IMongoTurnoRepository mongoRepo)
    {
        _mongoRepo = mongoRepo;
    }

    // GET: api/turnos-mongo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TurnoDocument>>> GetAll()
    {
        return await _mongoRepo.GetAllAsync();
    }

    // GET: api/turnos-mongo/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<TurnoDocument>> GetById(string id)
    {
        var turno = await _mongoRepo.GetByIdAsync(id);
        if (turno == null) return NotFound();
        return turno;
    }

    // POST: api/turnos-mongo
    [HttpPost]
    public async Task<ActionResult<TurnoDocument>> Create([FromBody] TurnoDocument turno)
    {
        if (turno == null)
            return BadRequest("Datos inválidos.");

        if (turno.PacienteId <= 0 ||
            turno.MedicoId <= 0 ||
            turno.Fecha == default)
        {
            return BadRequest("pacienteId, medicoId y fecha son obligatorios.");
        }

        turno.SyncKey = TurnoMapper.BuildSyncKey(turno.PacienteId, turno.MedicoId, turno.Fecha);
        turno.UpdatedAt = DateTime.UtcNow;

        await _mongoRepo.CreateAsync(turno);
        return Ok(turno);
    }

    // PUT: api/turnos-mongo/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] TurnoDocument turno)
    {
        if (turno == null) return BadRequest("Datos inválidos.");

        var existing = await _mongoRepo.GetByIdAsync(id);
        if (existing == null) return NotFound();

        turno.Id = existing.Id;
        turno.SyncKey = TurnoMapper.BuildSyncKey(turno.PacienteId, turno.MedicoId, turno.Fecha);
        turno.UpdatedAt = DateTime.UtcNow;

        await _mongoRepo.UpdateAsync(id, turno);
        return NoContent();
    }

    // DELETE: api/turnos-mongo/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var existing = await _mongoRepo.GetByIdAsync(id);
        if (existing == null) return NotFound();

        await _mongoRepo.DeleteAsync(id);
        return NoContent();
    }
}
