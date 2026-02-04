using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("turno")]
public class Turno
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [JsonPropertyName("pacienteId")]
    [Column("paciente_id")]
    public int PacienteId { get; set; }

    [JsonPropertyName("medicoId")]
    [Column("medico_id")]
    public int MedicoId { get; set; }

    [JsonPropertyName("fecha")]
    [Required]
    [Column("fecha")]
    public DateTime Fecha { get; set; }

    [JsonPropertyName("hora")]
    [Required]
    [Column("hora")]
    public TimeSpan Hora { get; set; } = TimeSpan.Zero;

    [JsonPropertyName("estado")]
    [Column("estado")]
    public string Estado { get; set; } = "pendiente";

    [JsonPropertyName("ultimaActualizacion")]
    [Column("ultima_actualizacion")]
    public DateTime UltimaActualizacion { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("syncKey")]
    [NotMapped]
    public string SyncKey { get; set; } = string.Empty;

    [JsonPropertyName("updatedAt")]
    [NotMapped]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
