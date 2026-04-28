using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetClinic.Models
{
    [Table("EspaciosDeportivos")]
    public class EspacioDeportivo
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        // Mapeamos a tipo_espacio
        [Column("tipo_espacio")]
        [Required(ErrorMessage = "El tipo de espacio es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Tipo de Espacio")]
        public string TipoEspacio { get; set; } = string.Empty;

        [Column("capacidad")]
        [Required(ErrorMessage = "La capacidad es obligatoria")]
        [Range(1, 1000, ErrorMessage = "La capacidad debe ser al menos 1")]
        public int Capacidad { get; set; }

        public virtual ICollection<Reserva>? Reservas { get; set; }
    }
}
