using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Collections.Generic;

namespace sistema_de_informacion_bibliotecaria_sib.Pages.Usuario
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<Usuario> Usuarios { get; set; } = new();

        public IndexModel(IConfiguration configuration) => _configuration = configuration;


        public void OnGet()
        {
            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();


            string query = "SELECT idusuario, nombre, apellido, correo, telefono, carnet FROM usuario";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Usuarios.Add(new Usuario
                {
                    IdUsuario = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Apellido = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    Correo = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Telefono = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Carnet = reader.IsDBNull(5) ? "" : reader.GetString(5)
                });
            }
        }

    }

    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Carnet { get; set; }
    }
}