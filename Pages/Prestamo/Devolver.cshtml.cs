using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;

namespace sistema_de_informacion_bibliotecaria_sib.Pages.Prestamo
{
    public class DevolverModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public DevolverModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet(int id)
        {
            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();

            int idLibro = 0;

           
            using (var cmd = new NpgsqlCommand("SELECT IdLibro FROM Prestamo WHERE IdPrestamo=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                idLibro = Convert.ToInt32(cmd.ExecuteScalar());
            }

            
            using (var cmd = new NpgsqlCommand(
                "UPDATE Prestamo SET Estado='Devuelto', FechaDevolucion=@f WHERE IdPrestamo=@id", conn))
            {
                cmd.Parameters.AddWithValue("@f", DateTime.Now);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

           
            using (var cmd = new NpgsqlCommand(
                "UPDATE Libro SET Cantidad = Cantidad + 1 WHERE IdLibro=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", idLibro);
                cmd.ExecuteNonQuery();
            }

            return RedirectToPage("/Prestamo/Index");
        }
    }
}
