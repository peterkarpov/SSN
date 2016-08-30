namespace WebUI.Controllers
{
    using Domain;
    using Domain.Entities;
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Mvc;
    using WebUI.Infrastructure;
    using WebUI.Infrastructure.Abstract;
    using WebUI.Models;

    public class AuthenticationController : Controller
    {
        IAuthProvider authProvider;
        IRepository repository;

        public AuthenticationController(IAuthProvider auth, IRepository repo)
        {
            authProvider = auth;
            repository = repo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(RegistrationViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                model.Password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(model.Password)));
                model.ConfirmPassword = null;

                User user = new User
                {
                    email = model.Email,
                    login = model.UserName,
                    password = model.Password,
                    UserId = Guid.NewGuid(),
                };

                if (authProvider.Registrate(user))
                {
                    return SignIn(new AuthenticationViewModel { UserName = model.UserName, Password = model.Password }, returnUrl);
                    //return Redirect(returnUrl ?? Url.Action("ShowOne", "Profile", new { login = user.login }));
                }
                else
                {
                    ModelState.AddModelError("error", "Can't registrate user");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }


        public ViewResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(AuthenticationViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                model.Password = BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(model.Password)));

                if (authProvider.Authenticate(model.UserName, model.Password))
                {
                    OnLineLogic.AddOnLineList((Guid)LoginHelpers.GetIdByLogin(model.UserName));


                    TempData["message-complete"] = String.Format("Hello {0}, welcome to SSN", model.UserName);
                    return Redirect(returnUrl ?? Url.Action("ShowOne", "Profile", new { login = model.UserName }));
                }
                else
                {
                    ModelState.AddModelError("error", "Wrong login or password");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult SignOut(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                OnLineLogic.RemoveOnLineList(Guid.Parse(User.Identity?.Name.Split('|')[1]));

            authProvider.SignOut();

            TempData["message-complete"] = String.Format("Fare you well!");
            return Redirect(returnUrl ?? Url.Action("Index", "Home"));
        }

        public ActionResult UserEdit(string login, string returnUrl)
        {
            RegistrationViewModel model = new RegistrationViewModel();

            User user = repository.Users.FirstOrDefault(u => u.login == login);

            model.UserName = user.login;
            model.Email = user.email;

            return View(model);
        }

        [HttpPost]
        public ActionResult UserEdit(RegistrationViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                model.Password = model.Password.GetHashCode().ToString();
                model.ConfirmPassword = null;

                User user = new User
                {
                    UserId = Guid.Parse(User.Identity.Name.Split('|')[1]),
                    email = model.Email,
                    login = model.UserName,
                    password = model.Password,
                };

                if (repository.EditUser(user))
                {
                    TempData["message-complete"] = String.Format("Changes saved");
                    return Redirect(returnUrl ?? Url.Action("ShowOne", "Profile", new { login = user.login }));
                }
                else
                {
                    ModelState.AddModelError("error", "wrong data");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public JsonResult CheckUserName(string username)
        {
            var result = repository.Users.Where(u => u.login == username).Count() == 0;
            //var result = Membership.FindUsersByName(username).Count == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}