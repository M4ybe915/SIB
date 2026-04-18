using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sistema_de_informacion_bibliotecaria_sib.Models
{
    public class Categoria
    {
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio")]
        [Display(Name = "Nombre de la Categoría")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string NombreCategoria { get; set; } = string.Empty;

        // Relación (1 categoría → muchos libros)
        public ICollection<Libro> Libros { get; set; } = new List<Libro>();
    }
}