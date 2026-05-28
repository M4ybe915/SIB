using System;
using System.ComponentModel.DataAnnotations;

namespace sistema_de_informacion_bibliotecaria_sib.Models
{
    public class Prestamo
    {
        public int IdPrestamo { get; set; }

        
        [Required(ErrorMessage = "La fecha de préstamo es obligatoria")]
        [Display(Name = "Fecha de Préstamo")]
        [DataType(DataType.Date)]
        public DateTime FechaPrestamo { get; set; } = DateTime.Today;

        
        [Required(ErrorMessage = "La fecha límite es obligatoria")]
        [Display(Name = "Fecha Límite")]
        [DataType(DataType.Date)]
        public DateTime FechaLimite { get; set; } = DateTime.Today.AddDays(7);

        
        [Display(Name = "Fecha de Devolución")]
        [DataType(DataType.Date)]
        public DateTime? FechaDevolucion { get; set; }

       
        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo"; 

        
        [Required(ErrorMessage = "Debe seleccionar un usuario")]
        public int Idusuario { get; set; }
        public Usuario usuario { get; set; }

        
        [Required(ErrorMessage = "Debe seleccionar un libro")]
        public int Idlibro { get; set; }
        public Libro libro { get; set; }
    }
}
