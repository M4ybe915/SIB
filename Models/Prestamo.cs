using sistema_de_informacion_bibliotecaria_sib;
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

        [Required(ErrorMessage = "La fecha de devolución es obligatoria")]
        [Display(Name = "Fecha de Devolución")]
        [DataType(DataType.Date)]
        public DateTime FechaDevolucion { get; set; } = DateTime.Today.AddDays(7);

        [Required(ErrorMessage = "El estado del préstamo es obligatorio")]
        [Display(Name = "Estado")]
        public string Estado { get; set; } = "Activo"; // Activo, Devuelto, Retrasado

        // Relación con Usuario
        [Required(ErrorMessage = "Debe seleccionar un usuario")]
        public int IdUsuario { get; set; }

        public Usuario Usuario { get; set; }

        // Relación con Libro
        [Required(ErrorMessage = "Debe seleccionar un libro")]
        public int IdLibro { get; set; }

        public Libro Libro { get; set; }
    }
}