using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace sistema_de_informacion_bibliotecaria_sib.Pages.Prestamo
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<PrestamoView> Lista { get; set; } = new();

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();

            string query = @"
            SELECT p.IdPrestamo, p.FechaPrestamo, p.FechaLimite, p.Estado,
                   u.Nombre, l.Titulo
            FROM Prestamo p
            JOIN Usuario u ON p.IdUsuario = u.IdUsuario
            JOIN Libro l ON p.IdLibro = l.IdLibro";

            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Lista.Add(new PrestamoView
                {
                    IdPrestamo = reader.GetInt32(0),
                    FechaPrestamo = reader.GetDateTime(1),

                    FechaLimite = reader.IsDBNull(2)
         ? DateTime.MinValue
         : reader.GetDateTime(2),

                    Estado = reader.IsDBNull(3)
         ? ""
         : reader.GetString(3),

                    Usuario = reader.GetString(4),
                    Libro = reader.GetString(5)
            });
            }
        }
    }

    public class PrestamoView
    {
        public int IdPrestamo { get; set; }
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaLimite { get; set; }
        public string Estado { get; set; }
        public string Usuario { get; set; }
        public string Libro { get; set; }
    }
}