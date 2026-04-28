using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetClinic.Models
{
    // Le indicamos a EF Core que la tabla se llama "Usuarios"
    [Table("Usuarios")]
    public class Usuario
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        // Mapeamos la propiedad al nombre real en la base de datos: documento_identidad
        [Column("documento_identidad")]
        [Required(ErrorMessage = "El documento de identidad es obligatorio")]
        [StringLength(20)]
        [Display(Name = "Documento de Identidad")]
        public string DocumentoIdentidad { get; set; } = string.Empty;

        [Column("telefono")]
        [StringLength(20)]
        public string? Telefono { get; set; }

        [Column("correo")]
        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo no válido")]
        [StringLength(100)]
        public string Correo { get; set; } = string.Empty;

        public virtual ICollection<Reserva>? Reservas { get; set; }
    }
}
