using System.Collections.Generic;
using System.Threading.Tasks;

public interface IMongoTurnoRepository
{
    Task<List<TurnoDocument>> GetAllAsync();
    Task<TurnoDocument?> GetByIdAsync(string id);
    Task<TurnoDocument?> GetBySyncKeyAsync(string syncKey);
    Task CreateAsync(TurnoDocument turno);
    Task UpdateAsync(string id, TurnoDocument turno);
    Task DeleteAsync(string id);
}
