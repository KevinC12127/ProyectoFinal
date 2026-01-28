using Microsoft.EntityFrameworkCore;

public class TurnosContext : DbContext
{
    public TurnosContext(DbContextOptions<TurnosContext> options) : base(options) { }

    public DbSet<Turno> Turnos { get; set; }
    public DbSet<SyncLog> SyncLogs { get; set; }
}
