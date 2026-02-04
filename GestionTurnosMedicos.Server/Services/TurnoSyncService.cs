using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class TurnoSyncService
{
    private readonly TurnosContext _context;
    private readonly IMongoTurnoRepository _mongoRepo;

    public TurnoSyncService(TurnosContext context, IMongoTurnoRepository mongoRepo)
    {
        _context = context;
        _mongoRepo = mongoRepo;
    }

    public async Task<SyncLog> SyncSqlToMongoAsync()
    {
        var sqlTurnos = await _context.Turnos.AsNoTracking().ToListAsync();
        var mongoTurnos = await _mongoRepo.GetAllAsync();
        var mongoByKey = mongoTurnos.ToDictionary(t => t.SyncKey, t => t);

        int created = 0, updated = 0, conflicts = 0;

        foreach (var sql in sqlTurnos)
        {
            if (string.IsNullOrWhiteSpace(sql.SyncKey))
            {
                sql.SyncKey = TurnoMapper.BuildSyncKey(sql.PacienteId, sql.MedicoId, sql.Fecha);
            }

            if (!mongoByKey.TryGetValue(sql.SyncKey, out var mongo))
            {
                await _mongoRepo.CreateAsync(TurnoMapper.ToDocument(sql));
                created++;
                continue;
            }

            if (sql.UltimaActualizacion >= mongo.UpdatedAt)
            {
                var updatedDoc = TurnoMapper.ToDocument(sql);
                updatedDoc.Id = mongo.Id;
                await _mongoRepo.UpdateAsync(mongo.Id!, updatedDoc);
                updated++;
                if (sql.UltimaActualizacion != mongo.UpdatedAt) conflicts++;
            }
        }

        var log = new SyncLog
        {
            Direction = "sql-to-mongo",
            SyncedAt = DateTime.UtcNow,
            ItemsCreated = created,
            ItemsUpdated = updated,
            ConflictsResolved = conflicts
        };

        _context.SyncLogs.Add(log);
        await _context.SaveChangesAsync();
        return log;
    }

    public async Task<SyncLog> SyncMongoToSqlAsync()
    {
        var mongoTurnos = await _mongoRepo.GetAllAsync();
        var mongoDetalle = await _mongoRepo.GetAllDetalleAsync();
        var sqlTurnos = await _context.Turnos.ToListAsync();

        foreach (var sql in sqlTurnos)
        {
            if (string.IsNullOrWhiteSpace(sql.SyncKey))
            {
                sql.SyncKey = TurnoMapper.BuildSyncKey(sql.PacienteId, sql.MedicoId, sql.Fecha);
            }
        }

        var sqlByKey = sqlTurnos.ToDictionary(t => t.SyncKey, t => t);
        var sqlById = sqlTurnos.ToDictionary(t => t.Id, t => t);

        int created = 0, updated = 0, conflicts = 0;

        // Sincronización desde detalle_turno (Mongo) hacia SQL por TurnoId
        foreach (var detalle in mongoDetalle)
        {
            if (!sqlById.TryGetValue(detalle.TurnoId, out var sql))
            {
                continue;
            }

            if (detalle.UltimaActualizacion >= sql.UltimaActualizacion)
            {
                sql.Estado = detalle.Estado;
                if (detalle.UltimaActualizacion != sql.UltimaActualizacion)
                {
                    conflicts++;
                }
                sql.UltimaActualizacion = detalle.UltimaActualizacion;
                updated++;
            }
        }

        foreach (var mongo in mongoTurnos)
        {
            if (string.IsNullOrWhiteSpace(mongo.SyncKey))
            {
                mongo.SyncKey = TurnoMapper.BuildSyncKey(mongo.PacienteId, mongo.MedicoId, mongo.Fecha);
            }

            if (!sqlByKey.TryGetValue(mongo.SyncKey, out var sql))
            {
                var entity = TurnoMapper.ToEntity(mongo);
                entity.UpdatedAt = mongo.UpdatedAt;
                _context.Turnos.Add(entity);
                created++;
                continue;
            }

            if (mongo.UpdatedAt >= sql.UltimaActualizacion)
            {
                sql.PacienteId = mongo.PacienteId;
                sql.MedicoId = mongo.MedicoId;
                sql.Fecha = mongo.Fecha;
                sql.Hora = mongo.Hora;
                sql.Estado = mongo.Estado;
                sql.UltimaActualizacion = mongo.UpdatedAt;
                updated++;
                if (mongo.UpdatedAt != sql.UltimaActualizacion) conflicts++;
            }
        }

        var log = new SyncLog
        {
            Direction = "mongo-to-sql",
            SyncedAt = DateTime.UtcNow,
            ItemsCreated = created,
            ItemsUpdated = updated,
            ConflictsResolved = conflicts
        };

        _context.SyncLogs.Add(log);
        await _context.SaveChangesAsync();
        return log;
    }

    public async Task<SyncLog> SyncBidirectionalAsync()
    {
        var sqlTurnos = await _context.Turnos.ToListAsync();
        var mongoTurnos = await _mongoRepo.GetAllAsync();

        foreach (var sql in sqlTurnos)
        {
            if (string.IsNullOrWhiteSpace(sql.SyncKey))
            {
                sql.SyncKey = TurnoMapper.BuildSyncKey(sql.PacienteId, sql.MedicoId, sql.Fecha);
            }
        }

        foreach (var mongo in mongoTurnos)
        {
            if (string.IsNullOrWhiteSpace(mongo.SyncKey))
            {
                mongo.SyncKey = TurnoMapper.BuildSyncKey(mongo.PacienteId, mongo.MedicoId, mongo.Fecha);
            }
        }

        var sqlByKey = sqlTurnos.ToDictionary(t => t.SyncKey, t => t);
        var mongoByKey = mongoTurnos.ToDictionary(t => t.SyncKey, t => t);

        int created = 0, updated = 0, conflicts = 0;

        foreach (var sql in sqlTurnos)
        {
            if (string.IsNullOrWhiteSpace(sql.SyncKey))
            {
                sql.SyncKey = TurnoMapper.BuildSyncKey(sql.PacienteId, sql.MedicoId, sql.Fecha);
            }

            if (!mongoByKey.TryGetValue(sql.SyncKey, out var mongo))
            {
                await _mongoRepo.CreateAsync(TurnoMapper.ToDocument(sql));
                created++;
                continue;
            }

            if (sql.UltimaActualizacion > mongo.UpdatedAt)
            {
                var updatedDoc = TurnoMapper.ToDocument(sql);
                updatedDoc.Id = mongo.Id;
                await _mongoRepo.UpdateAsync(mongo.Id!, updatedDoc);
                updated++;
                conflicts++;
            }
        }

        foreach (var mongo in mongoTurnos)
        {
            if (string.IsNullOrWhiteSpace(mongo.SyncKey))
            {
                mongo.SyncKey = TurnoMapper.BuildSyncKey(mongo.PacienteId, mongo.MedicoId, mongo.Fecha);
            }

            if (!sqlByKey.TryGetValue(mongo.SyncKey, out var sql))
            {
                _context.Turnos.Add(TurnoMapper.ToEntity(mongo));
                created++;
                continue;
            }

            if (mongo.UpdatedAt > sql.UltimaActualizacion)
            {
                sql.PacienteId = mongo.PacienteId;
                sql.MedicoId = mongo.MedicoId;
                sql.Fecha = mongo.Fecha;
                sql.Hora = mongo.Hora;
                sql.Estado = mongo.Estado;
                sql.UltimaActualizacion = mongo.UpdatedAt;
                updated++;
                conflicts++;
            }
        }

        var log = new SyncLog
        {
            Direction = "bidirectional",
            SyncedAt = DateTime.UtcNow,
            ItemsCreated = created,
            ItemsUpdated = updated,
            ConflictsResolved = conflicts
        };

        _context.SyncLogs.Add(log);
        await _context.SaveChangesAsync();
        return log;
    }
}
