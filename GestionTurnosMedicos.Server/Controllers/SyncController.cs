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

    // Nuevo endpoint: POST api/sync/mongo-a-postgres (usando TurnoSyncService)
    [HttpPost("mongo-a-postgres")]
    public async Task<IActionResult> SincronizarMongoAPostgres()
    {
        var result = await _syncService.SyncMongoToSqlAsync();
        return Ok(result);
    }
}
