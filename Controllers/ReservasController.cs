using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VetClinic.Data;
using VetClinic.Models;

namespace VetClinic.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listar reservas (con filtros opcionales por usuario o espacio)
        public async Task<IActionResult> Index(int? usuarioId, int? espacioId)
        {
            var consulta = _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.EspacioDeportivo)
                .AsQueryable();

            if (usuarioId.HasValue) consulta = consulta.Where(r => r.UsuarioId == usuarioId);
            if (espacioId.HasValue) consulta = consulta.Where(r => r.EspacioId == espacioId);

            ViewBag.Usuarios = new SelectList(await _context.Usuarios.ToListAsync(), "Id", "Nombre");
            ViewBag.Espacios = new SelectList(await _context.EspaciosDeportivos.ToListAsync(), "Id", "Nombre");

            return View(await consulta.ToListAsync());
        }

        // Mostrar formulario de creación
        public async Task<IActionResult> Create()
        {
            // Cargamos las listas para los select del formulario
            ViewBag.UsuarioId = new SelectList(await _context.Usuarios.ToListAsync(), "Id", "Nombre");
            ViewBag.EspacioId = new SelectList(await _context.EspaciosDeportivos.ToListAsync(), "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            if (ModelState.IsValid)
            {
                // 1. VALIDACIÓN: Hora de fin debe ser mayor a hora de inicio
                if (reserva.HoraFin <= reserva.HoraInicio)
                {
                    ModelState.AddModelError("", "La hora de fin debe ser posterior a la hora de inicio.");
                }

                // 2. VALIDACIÓN: No fechas/horas pasadas
                var ahora = DateTime.Now;
                var fechaReserva = reserva.Fecha.ToDateTime(reserva.HoraInicio);
                if (fechaReserva < ahora)
                {
                    ModelState.AddModelError("", "No se pueden crear reservas en fechas u horas pasadas.");
                }

                // 3. VALIDACIÓN: Cruce de horarios en el ESPACIO
                var cruceEspacio = await _context.Reservas
                    .AnyAsync(r => r.EspacioId == reserva.EspacioId && 
                                   r.Fecha == reserva.Fecha && 
                                   r.Estado == "Activa" &&
                                   r.HoraInicio < reserva.HoraFin && 
                                   reserva.HoraInicio < r.HoraFin);

                if (cruceEspacio)
                {
                    ModelState.AddModelError("", "El espacio deportivo ya tiene una reserva en este horario.");
                }

                // 4. VALIDACIÓN: Cruce de horarios del USUARIO
                var cruceUsuario = await _context.Reservas
                    .AnyAsync(r => r.UsuarioId == reserva.UsuarioId && 
                                   r.Fecha == reserva.Fecha && 
                                   r.Estado == "Activa" &&
                                   r.HoraInicio < reserva.HoraFin && 
                                   reserva.HoraInicio < r.HoraFin);

                if (cruceUsuario)
                {
                    ModelState.AddModelError("", "El usuario ya tiene otra reserva en este horario.");
                }

                // Si no hay errores, guardamos
                if (ModelState.ErrorCount == 0)
                {
                    reserva.Estado = "Activa";
                    _context.Add(reserva);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }

            // Si falló algo, recargamos los selects
            ViewBag.UsuarioId = new SelectList(await _context.Usuarios.ToListAsync(), "Id", "Nombre", reserva.UsuarioId);
            ViewBag.EspacioId = new SelectList(await _context.EspaciosDeportivos.ToListAsync(), "Id", "Nombre", reserva.EspacioId);
            return View(reserva);
        }

        // Cancelar una reserva (Cambiar estado)
        public async Task<IActionResult> Cancelar(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                reserva.Estado = "Cancelada";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
