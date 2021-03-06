﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RegistroJATICS.Models;
using System.Net;
using System.Data.Entity;

namespace RegistroJATICS.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }            
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                /*ViewBag.Institucion = new SelectList(db.Institucions, "Nombre", "Nombre");
                ViewBag.Taller = new SelectList(db.Talleres, "ID_Taller", "Nombre_Taller");*/

                ViewBag.Institucion = new SelectList(db.Institucions.OrderBy(ins => ins.Nombre), "Nombre", "Nombre");

                Taller tallerDisponible1 = db.Talleres.ToList().FirstOrDefault(tall => tall.cantRegistrados < tall.CantidadParticipantes);
                Taller2 tallerDisponible2 = db.Taller2.ToList().FirstOrDefault(tall => tall.cantRegistrados < tall.CantidadParticipantes);

                //Datos para taller de 1er dia
                var talleres = db.Talleres.OrderBy(tall => tall.Nombre_Taller);

                string defDescripcion = tallerDisponible1.Descripcion;
                ViewBag.defDescripcion = defDescripcion;
                int CantidadParticipantes = tallerDisponible1.CantidadParticipantes;
                ViewBag.CantidadParticipantes = CantidadParticipantes;
                int cantRegistrados = tallerDisponible1.cantRegistrados;
                ViewBag.cantRegistrados = cantRegistrados;
                ViewBag.Taller = new SelectList(talleres, "ID_Taller", "Nombre_Taller", tallerDisponible1.ID_Taller);

                //Datos para taller de 2do dia
                var talleres2 = db.Taller2.OrderBy(tall => tall.Nombre_Taller);
                string defDescripcion2 = tallerDisponible2.Descripcion;
                ViewBag.defDescripcion2 = defDescripcion2;
                int CantidadParticipantes2 = tallerDisponible2.CantidadParticipantes;
                ViewBag.CantidadParticipantes2 = CantidadParticipantes2;
                int cantRegistrados2 = tallerDisponible2.cantRegistrados;
                ViewBag.cantRegistrados2 = cantRegistrados2;
                ViewBag.Taller2 = new SelectList(talleres2, "ID_Taller2", "Nombre_Taller", tallerDisponible2.ID_Taller2);

                return View();
            }            
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            var taller = db.Talleres.Find(model.ID_Taller);
            var taller2 = db.Taller2.Find(model.ID_Taller2);
            //Si todavia hay cupo
            if (ModelState.IsValid && taller.cantRegistrados < taller.CantidadParticipantes &&
                taller2.cantRegistrados < taller2.CantidadParticipantes)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email, ID_Taller = model.ID_Taller, ID_Taller2 = model.ID_Taller2, Nombre = model.Nombre_Institucion, NombreCompleto  = model.NombreAsistente };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    await UserManager.AddToRoleAsync(user.Id, "visitante");
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    string nombreTaller1 = db.Talleres.Find(model.ID_Taller).Nombre_Taller;
                    string nombreTaller2 = db.Taller2.Find(model.ID_Taller2).Nombre_Taller;
                    VMConfirmacionRegistro datosConfirmacion = new VMConfirmacionRegistro();

                    datosConfirmacion.nombreCompleto = model.NombreAsistente;
                    datosConfirmacion.institucion = model.Nombre_Institucion;
                    datosConfirmacion.taller1 = nombreTaller1;
                    datosConfirmacion.taller2 = nombreTaller2;
                    TempData["datosConfirmacion"] = datosConfirmacion;
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            //Datos para taller de 1er dia
            var talleres = db.Talleres.OrderBy(tall => tall.Nombre_Taller);
            string defDescripcion = talleres.FirstOrDefault().Descripcion;
            ViewBag.defDescripcion = defDescripcion;
            int CantidadParticipantes = talleres.FirstOrDefault().CantidadParticipantes;
            ViewBag.CantidadParticipantes = CantidadParticipantes;
            int cantRegistrados = talleres.FirstOrDefault().cantRegistrados;
            ViewBag.cantRegistrados = cantRegistrados;
            ViewBag.Taller = new SelectList(talleres, "ID_Taller", "Nombre_Taller");

            //Datos para taller de 2do dia
            var talleres2 = db.Taller2.OrderBy(tall => tall.Nombre_Taller);
            string defDescripcion2 = talleres2.FirstOrDefault().Descripcion;
            ViewBag.defDescripcion2 = defDescripcion2;
            int CantidadParticipantes2 = talleres2.FirstOrDefault().CantidadParticipantes;
            ViewBag.CantidadParticipantes2 = CantidadParticipantes2;
            int cantRegistrados2 = talleres2.FirstOrDefault().cantRegistrados;
            ViewBag.cantRegistrados2 = cantRegistrados2;
            ViewBag.Taller2 = new SelectList(talleres2, "ID_Taller2", "Nombre_Taller");

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET:/Account/Edit
        [Authorize(Roles = "admin,visitante")]
        public ActionResult Edit(string id)
        {
            var user = db.Users.Find(id);
            return View(user);
        }

        [Authorize(Roles = "admin,visitante")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Email,PasswordHash,SecurityStamp,Id,UserName,Nombre,NombreCompleto,ID_Taller")] ApplicationUser model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }//

        // GET: /Account/Delete/:id
        [Authorize(Roles = "admin")]
        public ActionResult Delete(string id)
        {
            var user = db.Users.Find(id);
            return View(user);
        }

        // POST: /Account/ConfirmDelete/:id
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult ConfirmDelete(string id)
        {
            var user = db.Users.Find(id);
            int tallerID = user.ID_Taller;
            db.Users.Remove(user);
            int removidos = db.SaveChanges();
            if (removidos > 0) { 
                return RedirectToAction("Details","Tallers",new { id = tallerID });
            }else
            {
                return View(user);
            }
        }

        // GET: /Account/Edit/:id
        [Authorize(Roles = "admin")]
        public ActionResult Edit2(string id)
        {
            var user = db.Users.Find(id);
            //SELECT LIST PARA NOMBRE DE INSTITUCION
            ViewBag.Nombre = new SelectList(db.Institucions.ToList(), "Nombre", "Nombre",user.Nombre);
            ViewBag.ID_Taller = new SelectList(db.Talleres.ToList(), "ID_Taller", "Nombre_Taller",user.ID_Taller);
            ViewBag.ID_Taller2 = new SelectList(db.Taller2.ToList(), "ID_Taller2", "Nombre_Taller", user.ID_Taller2);
            return View("Edit2",user);
        }

        // POST: /Account/Edit/
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit2(ApplicationUser user)
        {
            if (ModelState.IsValid) { 
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Tallers", new { id = user.ID_Taller });
            }
            //Algo salio mal y se muestra la misma ventana de edicion otravez
                //SELECT LIST PARA NOMBRE DE INSTITUCION
            ViewBag.Nombre = new SelectList(db.Institucions.ToList(), "Nombre", "Nombre");
                //SELECT LIST PARA NOMBRE DE TALLER
            ViewBag.ID_Taller = new SelectList(db.Talleres.ToList(), "ID_Taller", "Nombre_Taller", user.ID_Taller);
            ViewBag.ID_Taller2 = new SelectList(db.Taller2.ToList(), "ID_Taller2", "Nombre_Taller", user.ID_Taller2);
            return View("Edit2", user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}