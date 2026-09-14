using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Agenda.Entity;

/// <summary>
///     Entidad de base de datos.
/// </summary>
[Table("Contactos")]
public class ContactosEntity {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] [MaxLength(50)] public string Nombre { get; set; } = "";
    [Required] [MaxLength(50)] public string Alias { get; set; } = "";
    [Required] [MaxLength(50)] public int Telefono { get; set; }
    [Required] [MaxLength(50)] public string Email { get; set; } = "";
    [Column(TypeName = "datetime")] public DateTime CreateAt { get; init; } = DateTime.UtcNow;
    [Column(TypeName = "datetime")] public DateTime UpdateAt { get; set; } = DateTime.UtcNow;
}