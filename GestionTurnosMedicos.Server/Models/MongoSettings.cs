public class MongoSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string Database { get; set; } = "turnos_db";
    public string TurnosCollection { get; set; } = "turnos";
}
