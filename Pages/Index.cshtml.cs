using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.Collections.Generic;

namespace sistema_de_informacion_bibliotecaria_sib.Pages
{
    
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        
        public string Mensaje { get; set; } = "";

        public List<Libro> Libros { get; set; } = new List<Libro>();

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
         
        }

        public void OnGet()
        {
            string conexion = "Host=207.58.175.220;Port=5432;Username=keyjo;Password=keyjo2024;Database=libreria;";

            using (var conn = new NpgsqlConnection(conexion))
            {
                try
                {
                    conn.Open();
                    Mensaje = "Conexión exitosa 🚀";


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
                catch (Exception ex)
                {
                    Mensaje = "Error: " + ex.Message;
                }
            }
        }
    }
#pragma warning restore IDE0290

    
    public class Libro
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; } = "";
        public string Autor { get; set; } = "";
        public int Cantidad { get; set; }
    }
}