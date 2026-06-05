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

            // Consulta SQL completa con tus campos agregados
            string query = "INSERT INTO usuario (nombre, apellido, correo, telefono, carnet) VALUES (@n, @a, @c, @t, @ca)";

            using var cmd = new NpgsqlCommand(query, conn);

            // Valores de tus compañeras
            cmd.Parameters.AddWithValue("@n", Usuario.Nombre);
            cmd.Parameters.AddWithValue("@a", Usuario.Apellido);

            // Tus valores asignados
            cmd.Parameters.AddWithValue("@c", Usuario.Correo);
            cmd.Parameters.AddWithValue("@t", Usuario.Telefono);
            cmd.Parameters.AddWithValue("@ca", Usuario.Carnet);

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

        // --- TUS PROPIEDADES ---
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo no válido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El carnet es obligatorio")]
        [StringLength(20, ErrorMessage = "Máximo 20 caracteres")]
        public string Carnet { get; set; }
    }
}