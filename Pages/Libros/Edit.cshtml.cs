using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace sistema_de_informacion_bibliotecaria_sib.Pages.Libros
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Libro Libro { get; set; }

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // 🔹 Cargar datos del libro
        public void OnGet(int id)
        {
            string conexion = _configuration.GetConnectionString("DefaultConnection");

            using (var conn = new NpgsqlConnection(conexion))
            {
                conn.Open();

                string query = "SELECT IdLibro, Titulo, Autor, Cantidad FROM Libro WHERE IdLibro = @id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Libro = new Libro
                            {
                                IdLibro = reader.GetInt32(0),
                                Titulo = reader.GetString(1),
                                Autor = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                Cantidad = reader.GetInt32(3)
                            };
                        }
                    }
                }
            }
        }

        
        public IActionResult OnPost()
        {
            string conexion = _configuration.GetConnectionString("DefaultConnection");

            using (var conn = new NpgsqlConnection(conexion))
            {
                conn.Open();

                string query = "UPDATE Libro SET Titulo=@t, Autor=@a, Cantidad=@c WHERE IdLibro=@id";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@t", Libro.Titulo);
                    cmd.Parameters.AddWithValue("@a", (object?)Libro.Autor ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@c", Libro.Cantidad);
                    cmd.Parameters.AddWithValue("@id", Libro.IdLibro);

                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToPage("Index");
        }
    }
}