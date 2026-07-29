using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Paradigmas_MVC.servicios;
using Projecto_paradigmas.Models;

namespace Projecto_paradigmas.Controllers
{
    public class ReservaCoworkingController : Controller
    {
        SCoworking _service = new SCoworking();
        // GET: ReservaCoworking
        public ActionResult Index()
        {
            var reservations = _service.ListarReservasPorUsuario(20192001399);
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
            if (!ModelState.IsValid)
            {
                return View();
            }
            reserva.ReservedBy = 20192001399; // ID/Cuenta en sesión

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
            TimeSpan horaCierre = new TimeSpan(19, 0, 0);

            if (reserva.Start.TimeOfDay < horaApertura || reserva.End.TimeOfDay > horaCierre)
            {
                ViewBag.Error = "El área de coworking solo está disponible entre las 7:00 a.m. y las 7:00 p.m.";
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
                return View(); 
            }
            try
            {
                _service.CheckInReserva(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(int id)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            try
            {
                _service.CheckOutReserva(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
