using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

public class TurnoDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string SyncKey { get; set; } = string.Empty;
    public int PacienteId { get; set; }
    public int MedicoId { get; set; }
    public DateTime Fecha { get; set; }
    public TimeSpan Hora { get; set; } = TimeSpan.Zero;
    public string Estado { get; set; } = "pendiente";    
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}