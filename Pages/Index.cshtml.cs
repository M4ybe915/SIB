using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Collections.Generic;

namespace sistema_de_informacion_bibliotecaria_sib.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string Mensaje { get; set; }

        // 🔥 NUEVO: lista de libros
        public List<Libro> Libros { get; set; } = new List<Libro>();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            string conexion = "Host=localhost;Port=5432;Username=postgres;Password=wolf;Database=biblioteca_sib;";

            using (var conn = new NpgsqlConnection(conexion))
            {
                try
                {
                    conn.Open();
                    Mensaje = "Conexión exitosa 🚀";

                    // 🔥 NUEVO: consulta de libros
                    string query = "SELECT IdLibro, Titulo, Autor, Cantidad FROM Libro";

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
                catch (Exception ex)
                {
                    Mensaje = "Error: " + ex.Message;
                }
            }
        }
    }

    // 🔥 NUEVO: clase Libro
    public class Libro
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int Cantidad { get; set; }
    }
}