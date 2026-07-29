using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Projecto_paradigmas.Models;
using System.Security.Claims;
using Projecto_paradigmas.Services;
using Microsoft.AspNetCore.Authorization;

namespace Projecto_paradigmas.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        // Inyectamos el servicio
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {

            var usuario = await _accountService.ObtenerPorNumeroCuentaAsync(model.Id);

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuario.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "Correo o contraseña incorrectos.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Correo)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties { IsPersistent = model.Recordarme });

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var usuarioExiste = await _accountService.ObtenerPorNumeroCuentaAsync(model.Id);
            if (usuarioExiste != null)
            {
                ModelState.AddModelError(string.Empty, "El número de cuenta ya se encuentra registrado.");
                return View(model);
            }

            var correoExiste = await _accountService.ExisteUsuarioPorCorreoAsync(model.Correo);
            if (correoExiste)
            {
                ModelState.AddModelError(string.Empty, "El correo electrónico ya se encuentra registrado.");
                return View(model);
            }

            bool registrado = await _accountService.RegistrarUsuarioAsync(model);

            if (registrado)
            {
                TempData["MensajeExito"] = "Cuenta creada con éxito. Ya puedes iniciar sesión.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, "Ocurrió un error al registrar la cuenta. Intenta de nuevo.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private bool VerificarPassword(string passwordIngresada, string hashAlmacenado)
        {
            // Reemplazar con tu algoritmo real (ej: BCrypt.Verify)
            return passwordIngresada == hashAlmacenado;
        }
    }
}