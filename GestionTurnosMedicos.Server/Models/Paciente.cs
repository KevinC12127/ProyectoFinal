using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("paciente")]
public class Paciente
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("cedula")]
    public string Cedula { get; set; } = string.Empty;

    [Column("telefono")]
    public string Telefono { get; set; } = string.Empty;
}
