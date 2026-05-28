using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Collections.Generic;

namespace sistema_de_informacion_bibliotecaria_sib.Pages.Libros
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<Libro> Libros { get; set; } = new List<Libro>();

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            string conexion = _configuration.GetConnectionString("DefaultConnection");

            using (var conn = new NpgsqlConnection(conexion))
            {
                conn.Open();

                string query = "SELECT idlibro, titulo, autor, cantidad FROM libro";

                using (var cmd = new NpgsqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Libros.Add(new Libro
                        {
                            IdLibro = reader.GetInt32(0),
                            Titulo = reader.GetString(1),
                            Autor = reader.IsDBNull(2) ? "" : reader.GetString(2),
                            Cantidad = reader.GetInt32(3)
                        });
                    }
                }
            }
        }
    }

    public class Libro
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int Cantidad { get; set; }
    }
}