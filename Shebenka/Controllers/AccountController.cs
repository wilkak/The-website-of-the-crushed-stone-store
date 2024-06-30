using Shebenka.Models.Data;
using Shebenka.Models.Encoding;
using Shebenka.Models.ViewModels.Account;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Shebenka.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        [AllowAnonymous]
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        // GET: /account/login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            // подтвердить, что пользователь не авторизован
            string userEmail = User.Identity.Name;

            if (!string.IsNullOrEmpty(userEmail))
                return RedirectToAction("user-profile");

            // Вернуть представление
            return View();
        }
        // POST: /account/login
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginUserVM model)
        {
            // Проверяем модель на валидность
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Проеряем пользователя на валидность
            bool isValid = false;

            using (Db db = new Db())
            {
                string pass = Encrypt.GetMD5Hash(model.Password);
                if (db.Users.Any(x => x.EmailAdress.Equals(model.EmailAdress) && x.Password.Equals(pass)))
                {
                    isValid = true;
                }
            }

            if (!isValid)
            {
                ModelState.AddModelError("", "Неверное имя пользователя или пароль.");
                return View(model);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(model.EmailAdress, model.RememberMe);
               

                return Redirect(FormsAuthentication.GetRedirectUrl(model.EmailAdress, model.RememberMe));

            }

        }

        // GET: /account/logout
        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}