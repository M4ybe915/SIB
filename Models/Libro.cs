using sistema_de_informacion_bibliotecaria_sib;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sistema_de_informacion_bibliotecaria_sib.Models
{
    public class Libro
    {
        [Required(ErrorMessage = "El título es obligatorio")]
        [Display(Name = "Título")]
        [StringLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio")]
        [Display(Name = "Autor")]
        [StringLength(100)]
        public string Autor { get; set; } = string.Empty;

        [Display(Name = "Editorial")]
        [StringLength(100)]
        public string Editorial { get; set; } = string.Empty;

        [Required(ErrorMessage = "El año es obligatorio")]
        [Display(Name = "Año de Publicación")]
        [Range(1500, 2100, ErrorMessage = "Ingrese un año válido")]
        public int Anio { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        [StringLength(20)]
        public string Estado { get; set; } = "Disponible"; // Disponible, Prestado

        // Relación con Categoría
        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        [ForeignKey("Categoria")]
        public int IdCategoria { get; set; }

        public Categoria? Categoria { get; set; }

        // Relación con Préstamos
        public ICollection<Prestamo> Prestamo { get; set; } = new List<Prestamo>();

        // 🔥 Propiedad calculada (no se guarda en BD)
        [NotMapped]
        public bool Disponible => Estado == "Disponible";

    }
}