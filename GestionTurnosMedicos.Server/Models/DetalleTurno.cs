using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class DetalleTurno
{
    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement("turno_id")]
    public int TurnoId { get; set; }

    [BsonElement("estado")]
    public string Estado { get; set; }

    [BsonElement("ultima_actualizacion")]
    public DateTime UltimaActualizacion { get; set; }
}