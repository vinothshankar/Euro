using Euro.Models;
using Euro.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Euro.Controllers
{
    public class AccountController : Controller
    {
        private IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.Clear();
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View(new LoginModel());
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public async Task<IActionResult> ValidateLoginAsync(LoginModel loginModel)
        {
            ActionResult actionResult = View("Login", loginModel);
            if (ModelState.IsValid)
            {
                try
                {
                    var userRecord = _accountService.ValidateLogin(loginModel);
                    if (userRecord != null)
                    {

                        var claims = new[]
                        {
                            new Claim(type : "UserId",value: userRecord.Id.ToString()),
                            new Claim(type: "FirstName", value: userRecord.FirstName
                            +" "+userRecord.LastName),
                             new Claim(type : "Email",value: userRecord.Email.ToString()),
                            new Claim(type :"UserName",value:userRecord.UserName),
                            };
                        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(principal: principal);
                        actionResult = RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        loginModel.ErrorMessage = "Login failed";
                        actionResult = View("Login", loginModel);
                    }
                }
                catch (Exception ex)
                {
                    loginModel.ErrorMessage = ex.Message.ToString();
                    actionResult = View("Login", loginModel);
                }
            }
            else
            {
                loginModel.ErrorMessage = "Login failed";
                actionResult = View("Login", loginModel);
            }
            return actionResult;
        }

        [HttpPost]
        public IActionResult SignUp(RegisterModel registerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (_accountService.SignUp(registerModel))
                    {
                        EmailSender emailSender = new EmailSender();
                        //emailSender.SendEmail(registerModel.Email);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        registerModel.ErrorMessage = "Sign Up failed";
                        return View("SignUp", registerModel);
                    }
                }
                else
                {
                    registerModel.ErrorMessage = "Sign Up failed";
                    return View("SignUp", registerModel);
                }
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message.ToString());
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync("Cookies");
            HttpContext.Response.Cookies.Delete("Cookies");
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
