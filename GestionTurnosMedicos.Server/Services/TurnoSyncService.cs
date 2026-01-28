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
                sql.SyncKey = TurnoMapper.BuildSyncKey(sql.Cedula, sql.Fecha, sql.Hora, sql.Especialidad);
            }

            if (!mongoByKey.TryGetValue(sql.SyncKey, out var mongo))
            {
                await _mongoRepo.CreateAsync(TurnoMapper.ToDocument(sql));
                created++;
                continue;
            }

            if (sql.UpdatedAt >= mongo.UpdatedAt)
            {
                var updatedDoc = TurnoMapper.ToDocument(sql);
                updatedDoc.Id = mongo.Id;
                await _mongoRepo.UpdateAsync(mongo.Id!, updatedDoc);
                updated++;
                if (sql.UpdatedAt != mongo.UpdatedAt) conflicts++;
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
        var sqlTurnos = await _context.Turnos.ToListAsync();

        foreach (var sql in sqlTurnos)
        {
            if (string.IsNullOrWhiteSpace(sql.SyncKey))
            {
                sql.SyncKey = TurnoMapper.BuildSyncKey(sql.Cedula, sql.Fecha, sql.Hora, sql.Especialidad);
            }
        }

        var sqlByKey = sqlTurnos.ToDictionary(t => t.SyncKey, t => t);

        int created = 0, updated = 0, conflicts = 0;

        foreach (var mongo in mongoTurnos)
        {
            if (string.IsNullOrWhiteSpace(mongo.SyncKey))
            {
                mongo.SyncKey = TurnoMapper.BuildSyncKey(mongo.Cedula, mongo.Fecha, mongo.Hora, mongo.Especialidad);
            }

            if (!sqlByKey.TryGetValue(mongo.SyncKey, out var sql))
            {
                var entity = TurnoMapper.ToEntity(mongo);
                entity.FechaRegistro = mongo.FechaRegistro;
                entity.UpdatedAt = mongo.UpdatedAt;
                _context.Turnos.Add(entity);
                created++;
                continue;
            }

            if (mongo.UpdatedAt >= sql.UpdatedAt)
            {
                sql.NombrePaciente = mongo.NombrePaciente;
                sql.Cedula = mongo.Cedula;
                sql.Especialidad = mongo.Especialidad;
                sql.Fecha = mongo.Fecha;
                sql.Hora = mongo.Hora;
                sql.FechaRegistro = mongo.FechaRegistro;
                sql.UpdatedAt = mongo.UpdatedAt;
                updated++;
                if (mongo.UpdatedAt != sql.UpdatedAt) conflicts++;
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
                sql.SyncKey = TurnoMapper.BuildSyncKey(sql.Cedula, sql.Fecha, sql.Hora, sql.Especialidad);
            }
        }

        foreach (var mongo in mongoTurnos)
        {
            if (string.IsNullOrWhiteSpace(mongo.SyncKey))
            {
                mongo.SyncKey = TurnoMapper.BuildSyncKey(mongo.Cedula, mongo.Fecha, mongo.Hora, mongo.Especialidad);
            }
        }

        var sqlByKey = sqlTurnos.ToDictionary(t => t.SyncKey, t => t);
        var mongoByKey = mongoTurnos.ToDictionary(t => t.SyncKey, t => t);

        int created = 0, updated = 0, conflicts = 0;

        foreach (var sql in sqlTurnos)
        {
            if (string.IsNullOrWhiteSpace(sql.SyncKey))
            {
                sql.SyncKey = TurnoMapper.BuildSyncKey(sql.Cedula, sql.Fecha, sql.Hora, sql.Especialidad);
            }

            if (!mongoByKey.TryGetValue(sql.SyncKey, out var mongo))
            {
                await _mongoRepo.CreateAsync(TurnoMapper.ToDocument(sql));
                created++;
                continue;
            }

            if (sql.UpdatedAt > mongo.UpdatedAt)
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
                mongo.SyncKey = TurnoMapper.BuildSyncKey(mongo.Cedula, mongo.Fecha, mongo.Hora, mongo.Especialidad);
            }

            if (!sqlByKey.TryGetValue(mongo.SyncKey, out var sql))
            {
                _context.Turnos.Add(TurnoMapper.ToEntity(mongo));
                created++;
                continue;
            }

            if (mongo.UpdatedAt > sql.UpdatedAt)
            {
                sql.NombrePaciente = mongo.NombrePaciente;
                sql.Cedula = mongo.Cedula;
                sql.Especialidad = mongo.Especialidad;
                sql.Fecha = mongo.Fecha;
                sql.Hora = mongo.Hora;
                sql.FechaRegistro = mongo.FechaRegistro;
                sql.UpdatedAt = mongo.UpdatedAt;
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
