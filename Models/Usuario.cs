using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sistema_de_informacion_bibliotecaria_sib.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [Display(Name = "Apellido")]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Phone(ErrorMessage = "Número de teléfono no válido")]
        [Display(Name = "Teléfono")]
        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El carnet es obligatorio")]
        [Display(Name = "Carnet")]
        [StringLength(20)]
        public string Carnet { get; set; } = string.Empty;

       
        public ICollection<Prestamo> Prestamo { get; set; } = new List<Prestamo>();

       
        [Display(Name = "Nombre Completo")]
        public string NombreCompleto => $"{Nombre} {Apellido}";

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }

}
