using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace sistema_de_informacion_bibliotecaria_sib.Pages.Prestamo
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<Usuario> usuarios { get; set; } = new();
        public List<Libro> libros { get; set; } = new();

        [BindProperty]
        public Prestamo Prestamo { get; set; }

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT idusuario, nombre FROM usuario", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    usuarios.Add(new Usuario
                    {
                        IdUsuario = reader.GetInt32(0),
                        Nombre = reader.GetString(1)
                    });
                }
            }

            conn.Close();
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT idlibro, titulo, cantidad FROM libro", conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    libros.Add(new Libro
                    {
                        IdLibro = reader.GetInt32(0),
                        Titulo = reader.GetString(1),
                        Cantidad = reader.GetInt32(2)
                    });
                }
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                OnGet();
                return Page();
            }

            using var conn = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            conn.Open();

            int stock;

            using (var cmd = new NpgsqlCommand("SELECT cantidad FROM libro WHERE idlibro=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", Prestamo.IdLibro);
                stock = Convert.ToInt32(cmd.ExecuteScalar());
            }

            if (stock <= 0)
            {
                ModelState.AddModelError("", "No hay libros disponibles");
                OnGet();
                return Page();
            }

            DateTime hoy = DateTime.Now;

            using (var cmd = new NpgsqlCommand(
                "INSERT INTO prestamo (fechaprestamo, estado, idusuario, idlibro) VALUES (@f, @e, @u, @l)", conn))
            {
                cmd.Parameters.AddWithValue("@f", hoy);
                cmd.Parameters.AddWithValue("@e", "Activo");
                cmd.Parameters.AddWithValue("@u", Prestamo.IdUsuario);
                cmd.Parameters.AddWithValue("@l", Prestamo.IdLibro);

                cmd.ExecuteNonQuery();
            }

            using (var cmd = new NpgsqlCommand(
                "UPDATE libro SET cantidad = cantidad - 1 WHERE idlibro=@id", conn))
            {
                cmd.Parameters.AddWithValue("@id", Prestamo.IdLibro);
                cmd.ExecuteNonQuery();
            }

            return RedirectToPage("index");
        }
    }

    public class Prestamo
    {
        public int IdPrestamo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un usuario")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un libro")]
        public int IdLibro { get; set; }
    }

    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
    }

    public class Libro
    {
        public int IdLibro { get; set; }
        public string Titulo { get; set; }
        public int Cantidad { get; set; }
    }
}