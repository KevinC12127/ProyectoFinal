using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

public class Turno
{
    public int Id { get; set; }

    [JsonPropertyName("nombrePaciente")]
    [Required]
    public string NombrePaciente { get; set; } = string.Empty;

    [JsonPropertyName("cedula")]
    [Required]
    public string Cedula { get; set; } = string.Empty;

    [JsonPropertyName("especialidad")]
    [Required]
    public string Especialidad { get; set; } = string.Empty;

    [JsonPropertyName("fecha")]
    [Required]
    public DateTime Fecha { get; set; }

    [JsonPropertyName("hora")]
    [Required]
    public string Hora { get; set; } = string.Empty;

    [JsonPropertyName("fechaRegistro")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;

    [JsonPropertyName("estado")]
    public string Estado { get; set; } = "pendiente";

    [JsonPropertyName("ultimaActualizacion")]
    public DateTime UltimaActualizacion { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("syncKey")]
    public string SyncKey { get; set; } = string.Empty;

    [JsonPropertyName("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
