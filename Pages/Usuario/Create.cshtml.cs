using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace sistema_de_informacion_bibliotecaria_sib.Pages.Usuario
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Usuarios Usuario { get; set; }

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

            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();

            string query = "INSERT INTO usuario (nombre, apellido) VALUES (@n, @a)";

            using var cmd = new NpgsqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@n", Usuario.Nombre);
            cmd.Parameters.AddWithValue("@a", Usuario.Apellido);

            cmd.ExecuteNonQuery();

            return RedirectToPage("Index");
        }
    }

    public class Usuarios
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Apellido { get; set; }
    }
}