using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class TurnoDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string SyncKey { get; set; } = string.Empty;
    public string NombrePaciente { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Especialidad { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string Hora { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
