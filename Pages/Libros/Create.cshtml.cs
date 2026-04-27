using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace sistema_de_informacion_bibliotecaria_sib.Pages.Libros
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Libro Libro { get; set; }

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnPost()
        {
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string conexion = _configuration.GetConnectionString("DefaultConnection");

            using (var conn = new NpgsqlConnection(conexion)) 
            {
                conn.Open();

                string query = "INSERT INTO Libro (Titulo, Autor, Cantidad) VALUES (@t, @a, @c)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@t", Libro.Titulo);
                    cmd.Parameters.AddWithValue("@a", (object?)Libro.Autor ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@c", Libro.Cantidad);

                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToPage("Index");
        }
    }

    public class Libros
    {
        public int IdLibro { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(100)]
        public string Autor { get; set; }

        [Range(1, 1000, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }
    }
}