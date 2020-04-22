using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Subscrib_er.Data;
using Subscrib_er.Services.Services;
using Subscrib_er.Web.ViewModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Lend_er.Web.Controllers
{
    //[Route("Account")]
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.logger = logger;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        //[Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        //[Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (model.photo != null)
                {
                    string uploadfolder = Path.Combine(webHostEnvironment.WebRootPath, "ProfilePhoto");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.photo.FileName;
                    string filepath = Path.Combine(uploadfolder, uniqueFileName);
                    model.photo.CopyTo(new FileStream(filepath, FileMode.Create));
                }
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    Gender = model.Gender,
                    State = model.State,
                    PhotoPath = uniqueFileName,
                };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //generate email confirmation token
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    //build the confirmation link
                    var confirmationUrl = Url.Action("ConfirmEmail", "Account", new { userid = user.Id, token = token }, Request.Scheme);

                    //log the link to a file
                    string folder = Path.Combine(webHostEnvironment.WebRootPath, "tokens");
                    string filename = model.Email + "_" + DateTime.Today.Day + "_" + DateTime.Today.Month + "_" + DateTime.Today.Year + ".txt";
                    MyLogger log = new MyLogger(folder, filename);
                    log.WriteTokenToFile(confirmationUrl);
                    

                    if (signInManager.IsSignedIn(User) && User.IsInRole("admin"))
                    {
                        return RedirectToAction("AllUsers", "Administration");
                    }
                    ViewBag.ErrorTitle = "REGISTRATION SUCCESSFUL";
                    ViewBag.ErrorMessage = "Please click on the link we emailed you to confirm your email";
                    return View("Error");
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    //return RedirectToAction("index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        //[Route("")]
        //[Route("login")]
        //[Route("~/")]
        [HttpGet]
        public IActionResult Login()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("index", "Home");
            }
            return View();
        }

        //[Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                //block user if email is not confirmed
                if (user != null && user.EmailConfirmed.Equals(false) && (await userManager.CheckPasswordAsync(user,model.Password)))
                {
                    ModelState.AddModelError("", "Email not confirmed");
                    return View(model);
                }

                //allow user if email is confirmed.
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: model.RememberMe, true);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "Home");
                    }
                }
                if (result.IsLockedOut)
                {
                    return View("AccountLocked");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            return View(model);
        }

        //[Route("Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        //[Route("AccessDenied")]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        //[Route("ConfimEmail/{id?}")]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userid, string token)
        {
            if (userid == null || token == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var user = await userManager.FindByIdAsync(userid);
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"The user with id {userid} was not found";
                    return View("Error");
                }

                //if the user is found
                var result = await userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return View();
                }
                ViewBag.ErrorTitle = "Confirmation Failed";
                ViewBag.ErrorMessage = "Sorry your email failed to confirm.";
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && (await userManager.IsEmailConfirmedAsync(user)))
                {
                    //generate password reset token
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    
                    //generate password reset link
                    var passwordResetLink = Url.Action("ResetPassword", "Account", new { email = user.Email, token = token }, Request.Scheme);
                    //log passwordResetLink to a file
                    string folder = Path.Combine(webHostEnvironment.WebRootPath, "PasswordReset");
                    //filename generation
                    string filename = "RestPassword_"+ model.Email + "_" + DateTime.Today.Day + "_" + DateTime.Today.Month + "_" + DateTime.Today.Year + ".txt";

                    //log link to file
                    MyLogger log = new MyLogger(folder, filename);
                    if (log.WriteTokenToFile(passwordResetLink))
                    {
                        ViewBag.ErrorTitle = "Password Reset";
                        ViewBag.ErrorMessage = "We have emailed you a link to reset your password";
                        return View("Error");
                    }
                    ViewBag.ErrorTitle = "Failed";
                    ViewBag.ErrorMessage = "User not found or email not confirmed";
                    return View("Error");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            if (email == null || token == null)
            {
                //return RedirectToAction("login", "Account");
                ModelState.AddModelError("", "Invalid password reset token");
                return View();
            }
            else
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"The user with email {email} was not found";
                    return View("Error");
                }

                var model = new ResetPasswordViewModel()
                {
                    Email = email,
                    token = token
                };
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"The user with email {model.Email} was not found";
                    return View("Error");
                }

                var result = await userManager.ResetPasswordAsync(user, model.token, model.Password);
                if (result.Succeeded)
                {
                    if (await userManager.IsLockedOutAsync(user))
                    {
                        await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);
                    }
                    return View("ResetPasswordConfirmation");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }            
            ModelState.AddModelError("", "Invalid Password");
            return View(model);            
        }
    }
}