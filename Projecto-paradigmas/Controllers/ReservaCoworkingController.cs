using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Paradigmas_MVC.servicios;
using Projecto_paradigmas.Models;
using System.Security.Claims;

namespace Projecto_paradigmas.Controllers
{
    [Authorize]
    public class ReservaCoworkingController : Controller
    {
        SCoworking _service = new SCoworking();
        // GET: ReservaCoworking
        public ActionResult Index()
        {
            var userId = User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier) ? long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value) : 0;
            var reservations = _service.ListarReservasPorUsuario(userId);
            return View(reservations);
        }

        // GET: ReservaCoworking/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReservaCoworking/Create
        public ActionResult Create()
        {
            var areas = _service.ListarAreas();
            ViewBag.Areas = areas; // Para llenar el dropdown en la vista
            return View();
        }

        // POST: ReservaCoworking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CoworkingReservationViewModel reserva)
        {
            reserva.ReservedBy = User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier) ? long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value) : 0;
            if (!ModelState.IsValid)
            {
                return View();
            }

            CoworkingAreas areaSeleccionada = _service.ObtenerAreaPorId(reserva.AreaId);

            if (areaSeleccionada == null)
            {
                ViewBag.Error = "El área seleccionada no existe.";
                ViewBag.Areas = _service.ListarAreas(); // Para recargar el dropdown
                return View(reserva);
            }

            if (reserva.Participants < areaSeleccionada.MinParticipants ||
                reserva.Participants > areaSeleccionada.MaxParticipants)
            {
                ViewBag.Error = $"Para '{areaSeleccionada.Name}', el número de participantes debe estar entre {areaSeleccionada.MinParticipants} y {areaSeleccionada.MaxParticipants} personas.";
                ViewBag.Areas = _service.ListarAreas();
                return View(reserva);
            }

            TimeSpan horaApertura = new TimeSpan(7, 0, 0);
            TimeSpan horaCierre = new TimeSpan(21, 0, 0);

            if (reserva.Start.TimeOfDay < horaApertura || reserva.End.TimeOfDay > horaCierre)
            {
                ViewBag.Error = "El área de coworking solo está disponible entre las 7:00 a.m. y las 9:00 p.m.";
                ViewBag.Areas = _service.ListarAreas();
                return View(reserva);
            }

            double horasHoy = _service.ObtenerHorasReservadasHoy(reserva.ReservedBy, reserva.Start);
            double duracionNueva = (reserva.End - reserva.Start).TotalHours;

            if ((horasHoy + duracionNueva) > 4.0)
            {
                ViewBag.Error = $"Excedes el límite diario de 4 horas. Ya tienes {horasHoy} horas reservadas hoy.";
                ViewBag.Areas = _service.ListarAreas();
                return View(reserva);
            }

            if (_service.ExisteSolapamiento(reserva.AreaId, reserva.Start, reserva.End))
            {
                ViewBag.Error = $"El área '{areaSeleccionada.Name}' ya se encuentra ocupada en el horario seleccionado.";
                ViewBag.Areas = _service.ListarAreas();
                return View(reserva);
            }

            reserva.Status = "Pendiente";
            _service.AgregarReserva(reserva);

            return RedirectToAction("Index");
        }

        // POST: ReservaCoworking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _service.CheckInReserva(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _service.CheckOutReserva(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ReservaCoworking/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _service.CancelarReserva(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
