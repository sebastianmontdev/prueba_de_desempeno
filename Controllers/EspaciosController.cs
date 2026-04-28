using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Models;

namespace VetClinic.Controllers
{
    public class EspaciosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EspaciosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listar con opción de filtro por tipo
        public async Task<IActionResult> Index(string tipo)
        {
            // Iniciamos la consulta LINQ
            var consulta = _context.EspaciosDeportivos.AsQueryable();

            // Si el usuario envió un tipo por el filtro, lo aplicamos
            if (!string.IsNullOrEmpty(tipo))
            {
                consulta = consulta.Where(e => e.TipoEspacio == tipo);
            }

            var lista = await consulta.ToListAsync();
            
            // Enviamos el tipo actual para que el filtro se mantenga seleccionado
            ViewBag.TipoActual = tipo;
            
            return View(lista);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EspacioDeportivo espacio)
        {
            if (ModelState.IsValid)
            {
                // REGLA DE NEGOCIO: Validar nombre único
                var existe = await _context.EspaciosDeportivos
                    .AnyAsync(e => e.Nombre == espacio.Nombre);
                
                if (existe)
                {
                    ModelState.AddModelError("Nombre", "Ya existe un espacio deportivo con este nombre.");
                    return View(espacio);
                }

                _context.Add(espacio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(espacio);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var espacio = await _context.EspaciosDeportivos.FindAsync(id);
            if (espacio == null) return NotFound();
            return View(espacio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EspacioDeportivo espacio)
        {
            if (id != espacio.Id) return NotFound();

            if (ModelState.IsValid)
            {
                // Validar duplicado excluyéndose a sí mismo
                var existe = await _context.EspaciosDeportivos
                    .AnyAsync(e => e.Nombre == espacio.Nombre && e.Id != id);
                
                if (existe)
                {
                    ModelState.AddModelError("Nombre", "Otro espacio ya tiene este nombre.");
                    return View(espacio);
                }

                try
                {
                    _context.Update(espacio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EspacioExists(espacio.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(espacio);
        }

        private bool EspacioExists(int id)
        {
            return _context.EspaciosDeportivos.Any(e => e.Id == id);
        }
    }
}
