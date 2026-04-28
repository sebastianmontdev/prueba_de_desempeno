using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Models;

namespace VetClinic.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listar todos los usuarios
        public async Task<IActionResult> Index()
        {
            // Consultamos la lista de usuarios usando LINQ
            var usuarios = await _context.Usuarios.ToListAsync();
            return View(usuarios);
        }

        // Mostrar el formulario de creación
        public IActionResult Create()
        {
            return View();
        }

        // Procesar la creación del usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // REGLA DE NEGOCIO: Validar que el documento sea único
                var existeDocumento = await _context.Usuarios
                    .AnyAsync(u => u.DocumentoIdentidad == usuario.DocumentoIdentidad);
                
                if (existeDocumento)
                {
                    ModelState.AddModelError("DocumentoIdentidad", "Ya existe un usuario con este documento.");
                    return View(usuario);
                }

                // REGLA DE NEGOCIO: Validar que el correo sea único
                var existeCorreo = await _context.Usuarios
                    .AnyAsync(u => u.Correo == usuario.Correo);
                
                if (existeCorreo)
                {
                    ModelState.AddModelError("Correo", "Ya existe un usuario con este correo electrónico.");
                    return View(usuario);
                }

                try 
                {
                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Ocurrió un error al guardar los datos.");
                }
            }
            return View(usuario);
        }

        // Mostrar el formulario de edición
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();
            
            return View(usuario);
        }

        // Procesar la edición
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.Id) return NotFound();

            if (ModelState.IsValid)
            {
                // Validar duplicados pero excluyendo al usuario actual
                var existeDocumento = await _context.Usuarios
                    .AnyAsync(u => u.DocumentoIdentidad == usuario.DocumentoIdentidad && u.Id != id);
                
                if (existeDocumento)
                {
                    ModelState.AddModelError("DocumentoIdentidad", "Otro usuario ya tiene este documento.");
                    return View(usuario);
                }

                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }
    }
}
