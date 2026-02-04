using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MongoTurnoRepository : IMongoTurnoRepository
{
    private readonly IMongoCollection<TurnoDocument> _collection;
    private readonly IMongoCollection<DetalleTurno> _detalleCollection;

    public MongoTurnoRepository(IMongoClient client, IOptions<MongoSettings> settings)
    {
        var database = client.GetDatabase(settings.Value.Database);
        _collection = database.GetCollection<TurnoDocument>(settings.Value.TurnosCollection);
        _detalleCollection = database.GetCollection<DetalleTurno>("detalle_turno");
    }

    public async Task<List<TurnoDocument>> GetAllAsync() =>
        await _collection.Find(_ => true).ToListAsync();

    public async Task<TurnoDocument?> GetByIdAsync(string id) =>
        await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task<TurnoDocument?> GetBySyncKeyAsync(string syncKey) =>
        await _collection.Find(x => x.SyncKey == syncKey).FirstOrDefaultAsync();

    public async Task CreateAsync(TurnoDocument turno) =>
        await _collection.InsertOneAsync(turno);

    public async Task UpdateAsync(string id, TurnoDocument turno) =>
        await _collection.ReplaceOneAsync(x => x.Id == id, turno);

    public async Task DeleteAsync(string id) =>
        await _collection.DeleteOneAsync(x => x.Id == id);

    // Nuevo método para DetalleTurno
    public async Task<List<DetalleTurno>> GetAllDetalleAsync() =>
        await _detalleCollection.Find(_ => true).ToListAsync();
}
