using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace sistema_de_informacion_bibliotecaria_sib.Pages.Prestamo
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<Usuario> Usuarios { get; set; } = new();
        public List<Libro> Libros { get; set; } = new();

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

            using (var cmd = new NpgsqlCommand("SELECT IdUsuario, Nombre FROM Usuario", conn))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    Usuarios.Add(new Usuario
                    {
                        IdUsuario = reader.GetInt32(0),
                        Nombre = reader.GetString(1)
                    });
            conn.Close();
            conn.Open();

            using (var cmd = new NpgsqlCommand("SELECT IdLibro, Titulo FROM Libro", conn))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    Libros.Add(new Libro
                    {
                        IdLibro = reader.GetInt32(0),
                        Titulo = reader.GetString(1)
                    });
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

            string query = "INSERT INTO Prestamo (FechaPrestamo, Estado, IdUsuario, IdLibro) VALUES (@f, 'Activo', @u, @l)";

            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@f", DateTime.Now);
            cmd.Parameters.AddWithValue("@u", Prestamo.IdUsuario);
            cmd.Parameters.AddWithValue("@l", Prestamo.IdLibro);

            cmd.ExecuteNonQuery();

            return RedirectToPage("/Index");
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
    }
}