using System.Linq;
using System.Web.Mvc;
using MashZavod.Models;
using System.Web.Security;
using MashZavod.Models.DbModels;

namespace MashZavod.Controllers
{
    [RoutePrefix("Account")]
    public class AccountController : Controller
    {
        //Регистрация нового пользователя.
        //Во входных параметрах должен поступать объект типа user
        public ActionResult Register()
        {
            if (User.Identity.Name != "Admin")
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AccountRegisterModel model)
        {
            if (User.Identity.Name != "Admin")
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                users user = null;

                //Проверяем существует ли такой пользователь в БД
                using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
                {
                    user = db.users.FirstOrDefault(u => u.Login == model.Login);
                }

                //Если такого пользователя нет, то заносим его в БД
                if (user == null)
                {
                    using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
                    {
                        user = db.users.Add(new users()
                        {
                            Email = model.Email,
                            access = null,
                            group_rank = null,
                            id_group_rank = null,
                            id_rank = null,
                            Login = model.Login,
                            Name = model.Name,
                            Password = model.Password,
                            Patronymic = model.Patronymic,
                            Phone = model.Phone,
                            Position = model.Position,
                            rank = null,
                            Surname = model.Surname,
                            User_chat = null
                        });

                        // Проверяем, что пользователь успешно добавился
                        user = db.users.Where(u => u.Login == model.Login && u.Password == model.Password).FirstOrDefault();
                    }

                    //Если пользователь успешно добавлен - возвращаемся
                    if (user != null)
                        return RedirectToAction("AdminIndex", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с таким логином уже существует");
                }
            }
            return View(model);
        }

        //Вход пользователя
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AccountLoginModel model)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                users user = null;

                //Ищем есть ли такой пользователь в БД и совпадают ли введенные пароли
                using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
                {
                    user = db.users.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);
                }

                //Если данные введены верно
                //Заносим в куки Логин пользователя
                if (user != null)
                {
                    //Файл куки может сохраняться между сеансами браузера
                    HttpContext.Response.Cookies.Clear();
                    FormsAuthentication.SetAuthCookie(model.Login, false);
                    string name = User.Identity.Name;
                    //Если Логин принадлежит суперпользователю
                    if (model.Login == "Admin")
                        return RedirectToAction("AdminIndex", "Admin");
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Такой связки Логин + Пароль не существует");
                }
            }
            return View(model);
        }

        //Выход пользователя из аккаунта
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}