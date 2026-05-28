using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace sistema_de_informacion_bibliotecaria_sib.Pages.Usuario
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Usuario Usuario { get; set; }

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet(int id)
        {
            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT idusuario, nombre, apellido FROM usuario WHERE idusuario=@id", conn);

            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Usuario = new Usuario
                {
                    IdUsuario = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Apellido = reader.IsDBNull(2) ? "" : reader.GetString(2)
                };
            }
        }

        public IActionResult OnPost()
        {
            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();

            using var cmd = new NpgsqlCommand(
                "UPDATE usuario SET nombre=@n, apellido=@a WHERE idusuario=@id", conn);

            cmd.Parameters.AddWithValue("@n", Usuario.Nombre);
            cmd.Parameters.AddWithValue("@a", Usuario.Apellido ?? "");
            cmd.Parameters.AddWithValue("@id", Usuario.IdUsuario);

            cmd.ExecuteNonQuery();

            return RedirectToPage("Index");
        }
    }
}