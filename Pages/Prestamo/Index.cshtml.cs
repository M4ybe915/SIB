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
            using var conn = new NpgsqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));

            conn.Open();

            using var cmd = new NpgsqlCommand(
                "SELECT p.idprestamo, p.fechaprestamo, p.estado, " +
                "u.idusuario, u.nombre, " +
                "l.idlibro, l.titulo " +
                "FROM prestamo p " +
                "INNER JOIN usuario u ON p.idusuario = u.idusuario " +
                "INNER JOIN libro l ON p.idlibro = l.idlibro",
                conn);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Lista.Add(new PrestamoView
                {
                    IdPrestamo = reader.GetInt32(0),

                    FechaPrestamo = reader.GetDateTime(1),

                    Estado = reader.IsDBNull(2)
                        ? ""
                        : reader.GetString(2),

                    IdUsuario = reader.GetInt32(3),

                    Usuario = reader.IsDBNull(4)
                        ? ""
                        : reader.GetString(4),

                    IdLibro = reader.GetInt32(5),

                    Libro = reader.IsDBNull(6)
                        ? ""
                        : reader.GetString(6)
                });
            }
        }
    }

    public class PrestamoView
    {
        public int IdPrestamo { get; set; }

        public DateTime FechaPrestamo { get; set; }

        public string Estado { get; set; }

        public int IdUsuario { get; set; }

        public string Usuario { get; set; }

        public int IdLibro { get; set; }

        public string Libro { get; set; }
    }
}