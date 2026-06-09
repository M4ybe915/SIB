using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Npgsql;
using System.ComponentModel.DataAnnotations;

namespace sistema_de_informacion_bibliotecaria_sib.Pages.Libros
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;


        [BindProperty]
        public NuevoLibro NuevoLibro { get; set; }

        public IEnumerable<SelectListItem> ListaCategorias { get; set; }

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            ListaCategorias = ObtenerCategorias();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                ListaCategorias = ObtenerCategorias();
                return Page();
            }

            string conexion = _configuration.GetConnectionString("DefaultConnection");

            using (var conn = new NpgsqlConnection(conexion))
            {
                conn.Open();

                string query = "INSERT INTO libro (titulo, autor, editorial, año, cantidad, idcategoria) VALUES (@t, @a, @e, @y, @c, @cat)";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@t", NuevoLibro.Titulo);
                    cmd.Parameters.AddWithValue("@a", (object?)NuevoLibro.Autor ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@e", (object?)NuevoLibro.Editorial ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@y", NuevoLibro.Año);
                    cmd.Parameters.AddWithValue("@c", NuevoLibro.Cantidad);
                    cmd.Parameters.AddWithValue("@cat", NuevoLibro.Idcategoria);

                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToPage("Index");
        }

        private List<SelectListItem> ObtenerCategorias()
        {
            var categorias = new List<SelectListItem>();
            string conexion = _configuration.GetConnectionString("DefaultConnection");

            using (var conn = new NpgsqlConnection(conexion))
            {
                conn.Open();

                string query = "SELECT idcategoria, nombre FROM categoria";

                using (var cmd = new NpgsqlCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categorias.Add(new SelectListItem
                            {
                                Value = reader["idcategoria"].ToString(),
                                Text = reader["nombre"].ToString()
                            });
                        }
                    }
                }
            }
            return categorias;
        }
    }


    public class NuevoLibro
    {
        public int IdLibro { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(150, ErrorMessage = "Máximo 150 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio")]
        [StringLength(100)]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "La editorial es obligatoria")]
        [StringLength(100)]
        public string Editorial { get; set; } = string.Empty;

        [Required(ErrorMessage = "El año es obligatorio")]
        [Range(1000, 2100, ErrorMessage = "Ingrese un año válido")]
        public int Año { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, 1000, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        public int Idcategoria { get; set; }
    }


}
