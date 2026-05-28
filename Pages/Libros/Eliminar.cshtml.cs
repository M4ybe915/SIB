using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace sistema_de_informacion_bibliotecaria_sib.Pages.Libros
{
    public class EliminarModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Libro Libro { get; set; } = new Libro();

        public EliminarModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult OnGet(int id)
        {
            string? conexion = _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(conexion))
            {
                return RedirectToPage("Index");
            }

            using (var conn = new NpgsqlConnection(conexion))
            {
                conn.Open();

                string query = @"
                    SELECT IdLibro, Titulo, Autor, Cantidad
                    FROM Libro
                    WHERE IdLibro = @id";

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

            if (string.IsNullOrEmpty(conexion))
            {
                ModelState.AddModelError(string.Empty, "La cadena de conexión no está configurada.");
                return Page();
            }

            using (var conn = new NpgsqlConnection(conexion))
            {
                conn.Open();

                string deletePrestamos = @"
                    DELETE FROM Prestamo
                    WHERE IdLibro = @id";

                using (var cmd = new NpgsqlCommand(deletePrestamos, conn))
                {
                    cmd.Parameters.AddWithValue("@id", Libro.IdLibro);
                    cmd.ExecuteNonQuery();
                }

                string deleteLibro = @"
                    DELETE FROM Libro
                    WHERE IdLibro = @id";

                using (var cmd = new NpgsqlCommand(deleteLibro, conn))
                {
                    cmd.Parameters.AddWithValue("@id", Libro.IdLibro);

                    int filas = cmd.ExecuteNonQuery();

                    if (filas == 0)
                    {
                        ModelState.AddModelError(string.Empty, "No se pudo eliminar el libro.");
                        return Page();
                    }
                }
            }

            return RedirectToPage("Index");
        }
    }
}