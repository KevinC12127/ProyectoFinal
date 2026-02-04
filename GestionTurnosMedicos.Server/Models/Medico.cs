using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("medico")]
public class Medico
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("especialidad")]
    public string Especialidad { get; set; } = string.Empty;
}
