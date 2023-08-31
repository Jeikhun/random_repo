using Dolphin_Book.Areas.Admin.ViewModels;
using Dolphin_Book.Core.Constants;
using Dolphin_Book.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace Dolphin_Book.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginVM vm)
        {
            if (!ModelState.IsValid) return View();

            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Email or password is incorrect");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, vm.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or password is incorrect");
                return View();
            }

            if (!await HasAccessToAdminPanelAsync(user))
            {
                ModelState.AddModelError(string.Empty, "You don't have permission to admin panel");
                return View();
            }

            return RedirectToAction("index", "dashboard");







        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, Admin")]
        public async Task<IActionResult> Register(AccountRegisterVM model)
        {

            if (!ModelState.IsValid) return View();

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                Fullname = model.Fullname,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View();
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = Url.Action(nameof(ConfirmEmail), "account", new { token, email = user.Email }, Request.Scheme);
            var confirmationLink = Url.Action(action: "confirmemail", controller: "account", values: new { token = token, email = user.Email }, protocol: Request.Scheme);
            //var message = new Message(new string[] { user.Email }, "Email Confirmation", confirmationLink);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("jeikhunjalil@gmail.com");
            mail.To.Add(user.Email);
            mail.Subject = "Reset Password";
            mail.Body = $"<a href='{confirmationLink}'>Confirm Email</a>";
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential networkCredential = new NetworkCredential("jeikhunjalil@gmail.com", "fdgxcltipvqqujug");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(mail);
            //_emailSender.SendEmail(message);

            await _userManager.AddToRoleAsync(user, UserRoles.Admin.ToString());
            TempData["register"] = "Please,verify your email";

            return RedirectToAction(nameof(Login));
        }
        private async Task<bool> HasAccessToAdminPanelAsync(User user)
        {
            if (await _userManager.IsInRoleAsync(user, UserRoles.Admin.ToString()) ||
                await _userManager.IsInRoleAsync(user, UserRoles.SuperAdmin.ToString()))
            {
                return true;
            }

            return false;
        }
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                NotFound();
            }

            await _userManager.ConfirmEmailAsync(user, token);
            await _signInManager.SignInAsync(user, true);
            return RedirectToAction(nameof(Login));





        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return View();
            }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) { return NotFound(); }
            var token = _userManager.GeneratePasswordResetTokenAsync(user);
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Port = 7015;
            var result = Url.Action(action: "resetpassword", controller: "account", values: new { token = token.Result, email = email }, protocol: Request.Scheme);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("jeikhunjalil@gmail.com");
            mail.To.Add(user.Email);
            mail.Subject = "Reset Password";
            mail.Body = $"<a href='{result}'>Click here</a>";
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential networkCredential = new NetworkCredential("jeikhunjalil@gmail.com", "fdgxcltipvqqujug");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = networkCredential;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(mail);

            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) { return NotFound(); }
            ResetPasswordVM model = new ResetPasswordVM
            {
                Token = token,
                Email = email
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM vm)
        {
            var user = await _userManager.FindByEmailAsync(vm.Email);
            if(user == null) { return NotFound(); }

            var result = await _userManager.ResetPasswordAsync(user, vm.Token, vm.Password);

            if (!result.Succeeded)
            {
                return Json(result.Errors);
            }



            return RedirectToAction(nameof(Login));
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
            //return RedirectToAction("index", "home", new { area = "" });

        }
    }
}
