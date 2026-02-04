using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/sync")]
public class SyncController : ControllerBase
{
    private readonly TurnoSyncService _syncService;
    private readonly TurnosContext _context;
    private readonly IMongoTurnoRepository _mongoRepo;

    public SyncController(TurnoSyncService syncService, TurnosContext context, IMongoTurnoRepository mongoRepo)
    {
        _syncService = syncService;
        _context = context;
        _mongoRepo = mongoRepo;
    }

    // POST: api/sync/sql-to-mongo
    [HttpPost("sql-to-mongo")]
    public async Task<ActionResult<SyncLog>> SyncSqlToMongo()
    {
        var result = await _syncService.SyncSqlToMongoAsync();
        return Ok(result);
    }

    // POST: api/sync/mongo-to-sql
    [HttpPost("mongo-to-sql")]
    public async Task<ActionResult<SyncLog>> SyncMongoToSql()
    {
        var result = await _syncService.SyncMongoToSqlAsync();
        return Ok(result);
    }

    // POST: api/sync/bidirectional
    [HttpPost("bidirectional")]
    public async Task<ActionResult<SyncLog>> SyncBidirectional()
    {
        var result = await _syncService.SyncBidirectionalAsync();
        return Ok(result);
    }

    // GET: api/sync/logs
    [HttpGet("logs")]
    public async Task<ActionResult<IEnumerable<SyncLog>>> GetLogs()
    {
        var logs = await _context.SyncLogs.AsNoTracking().ToListAsync();
        return Ok(logs);
    }

    // Nuevo endpoint: POST api/sync/mongo-a-postgres
    [HttpPost("mongo-a-postgres")]
    public async Task<IActionResult> SincronizarMongoAPostgres()
    {
        var documentosMongo = await _mongoRepo.GetAllDetalleAsync();

        foreach (var doc in documentosMongo)
        {
            var turnoPostgres = await _context.Turnos
                .FirstOrDefaultAsync(t => t.Id == doc.TurnoId);

            if (turnoPostgres == null)
            {
                // INSERT
                var nuevoTurno = new Turno
                {
                    Id = doc.TurnoId,
                    Estado = doc.Estado,
                    UltimaActualizacion = doc.UltimaActualizacion,
                    Fecha = DateTime.Today,
                    Hora = TimeSpan.Zero.ToString()
                };

                _context.Turnos.Add(nuevoTurno);
            }
            else
            {
                // UPDATE solo si Mongo es más reciente
                if (doc.UltimaActualizacion > turnoPostgres.UltimaActualizacion)
                {
                    turnoPostgres.Estado = doc.Estado;
                    turnoPostgres.UltimaActualizacion = doc.UltimaActualizacion;
                }
            }
        }

        await _context.SaveChangesAsync();

        return Ok("Sincronización MongoDB → PostgreSQL completada");
    }
}
