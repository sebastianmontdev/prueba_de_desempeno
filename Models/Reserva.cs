using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VetClinic.Models
{
    [Table("Reservas")]
    public class Reserva
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("usuario_id")]
        [Required(ErrorMessage = "Debe seleccionar un usuario")]
        [Display(Name = "Usuario")]
        public int UsuarioId { get; set; }
        
        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }

        [Column("espacio_id")]
        [Required(ErrorMessage = "Debe seleccionar un espacio")]
        [Display(Name = "Espacio Deportivo")]
        public int EspacioId { get; set; }

        [ForeignKey("EspacioId")]
        public virtual EspacioDeportivo? EspacioDeportivo { get; set; }

        [Column("fecha")]
        [Required(ErrorMessage = "La fecha es obligatoria")]
        [DataType(DataType.Date)]
        public DateOnly Fecha { get; set; }

        [Column("hora_inicio")]
        [Required(ErrorMessage = "La hora de inicio es obligatoria")]
        [DataType(DataType.Time)]
        [Display(Name = "Hora de Inicio")]
        public TimeOnly HoraInicio { get; set; }

        [Column("hora_fin")]
        [Required(ErrorMessage = "La hora de fin es obligatoria")]
        [DataType(DataType.Time)]
        [Display(Name = "Hora de Fin")]
        public TimeOnly HoraFin { get; set; }

        [Column("estado")]
        [StringLength(20)]
        public string Estado { get; set; } = "Activa";
    }
}
