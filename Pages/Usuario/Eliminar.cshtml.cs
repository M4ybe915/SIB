using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace sistema_de_informacion_bibliotecaria_sib.Pages.Usuario
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Usuario Usuario { get; set; } = new Usuario();

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet(int id)
        {
            string conexion = _configuration.GetConnectionString("DefaultConnection");

            using (var conn = new NpgsqlConnection(conexion))
            {
                conn.Open();

                string query = "SELECT idusuario, nombre, apellido FROM usuario WHERE idusuario = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.IsDBNull(2) ? "" : reader.GetString(2)
                            };
                        }
                        else
                        {
                            return RedirectToPage("Index");
                        }
                    }
                }
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            string? conexion = _configuration.GetConnectionString("DefaultConnection");

            using (var conn = new NpgsqlConnection(conexion))
            {
                conn.Open();

                string deletePrestamos = @"
                DELETE FROM prestamo
                WHERE idusuario = @id";

                using (var cmd = new NpgsqlCommand(deletePrestamos, conn))
                {
                    cmd.Parameters.AddWithValue("@id", Usuario.IdUsuario);
                    cmd.ExecuteNonQuery();
                }

                string deleteUsuario = @"
                DELETE FROM usuario
                WHERE idusuario = @id";

                using (var cmd = new NpgsqlCommand(deleteUsuario, conn))
                {
                    cmd.Parameters.AddWithValue("@id", Usuario.IdUsuario);
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToPage("Index");
        }
    }
}