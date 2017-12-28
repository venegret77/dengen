using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MashZavod.Models.DbModels;
using MashZavod.Models.RemindersModels;

namespace MashZavod.Controllers
{
    /// <summary>
    /// Контроллер осуществляет взаимодействие с моделью БД - Напоминания
    /// </summary>
    public class ReminderController : Controller
    {
        /// <summary>
        /// Возвращает страницу с напоминаниями
        /// </summary>
        public ActionResult Index()
        {
            //Если пользователь не авторизован - отправляем на страницу авторизации
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //Первоначальные определения для формирования модели представления
            List<ReminderModel> reminderModels = new List<ReminderModel>();
            bool isReminderModels = false;
            List<string> usersModel = new List<string>();

            //Получаем данные из БД
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                //Получение идентификатора пользователя
                users user = db.users.FirstOrDefault(u => u.Login == User.Identity.Name);
                int id = 0;
                if (user != null)
                    id = user.id_users;

                //Формирование списка напоминаний
                if (id != 0)
                {
                    //Извлечение полного списка напоминаний для пользователя
                    List<Reminders> tmpReminders = db.Reminders.Where(u => u.IdRecipient == id && u.Visible == true && u.IdDocument == 0).ToList();
                    if (tmpReminders.Count > 0)
                    {
                        isReminderModels = true;
                        foreach (Reminders element in tmpReminders)
                        {
                            reminderModels.Add(new ReminderModel()
                            {
                                Id = element.Id,
                                DateReminder = element.DateReminder,
                                Text = element.Text,
                                Login = db.users.FirstOrDefault(u => u.id_users == element.IdSender).Login
                            });
                        }
                    }
                }
                //Извлечение полного списка пользователей
                foreach (users elem in db.users)
                    usersModel.Add(elem.Login);
            }

            //Формирование конечной модели
            ReminderViewModel reminderViewModel = new ReminderViewModel()
            {
                IsReminderModels = isReminderModels,
                ReminderModels = reminderModels,
                Users = usersModel
            };
            ViewBag.ReminderViewModel = reminderViewModel;
            return View();
        }

        /// <summary>
        /// Создание нового напоминания
        /// </summary>
        [HttpPost]
        public ActionResult Add(ReminderModelAdd model)
        {
            //Если пользователь не авторизован - отправляем на страницу авторизации
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //Проверяем правильно ли заполнена информация в модели
            if (ModelState.IsValid)
            {
                //Начинаем добавление нового поручения
                using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
                {
                    //Получение идентификатора отправителя
                    users user_sender = db.users.FirstOrDefault(u => u.Login == User.Identity.Name);
                    int id_sender = 0;
                    if (user_sender == null)
                    {
                        ModelState.AddModelError("", "Вы не можете добавлять напоминания");
                        return View("Index", model);
                    }

                    id_sender = user_sender.id_users;

                    //Получение идентификатора получателя
                    users user_recipient = db.users.FirstOrDefault(u => u.Login == model.Login);
                    int id_recipient = 0;
                    if (user_recipient == null)
                    {
                        ModelState.AddModelError("", "Пользователя, которому адресовано напоминание, не существует");
                        return View("Index", model);
                    }

                    id_recipient = user_recipient.id_users;

                    //Создаем новую запись
                    db.Reminders.Add(new Reminders()
                    {
                        IdSender = id_sender,
                        IdRecipient = id_recipient,
                        IdDocument = 0,
                        DateReminder = DateTime.Now,
                        Text = model.Text,
                        Visible = true
                    });
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Reminder");
        }

        /// <summary>
        /// Функция, позволяющая скрыть напоминание
        /// </summary>
        /// <param name="id">Идентификатор напоминания</param>
        [HttpGet]
        public ActionResult HideReminder(int id)
        {
            //Если пользователь не авторизован - отправляем на страницу авторизации
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //Начинаем изменение состояния напоминания
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                //проверяем есть ли такое напоминание
                Reminders reminder = db.Reminders.FirstOrDefault(u => u.Id == id);

                if (reminder == null)
                {
                    ModelState.AddModelError("", "Такого напоминания не существует");
                    return RedirectToAction("Index", "Reminder");
                }

                //Получение идентификатора получателя
                users user_recipient = db.users.FirstOrDefault(u => u.Login == User.Identity.Name);
                int id_recipient = 0;
                if (user_recipient != null)
                    id_recipient = user_recipient.id_users;

                //Проверяем принадлежит ли поручение пользователю
                if (reminder.IdRecipient != id_recipient)
                {
                    ModelState.AddModelError("", "У вас нет доступа к данному напоминанию");
                    return RedirectToAction("Index", "Reminder");
                }

                reminder.Visible = false;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Reminder");
        }

        /// <summary>
        /// Функция, передающая частичное представление
        /// </summary>
        /// <param name="id">Идентификатор документа</param>
        /// <returns>Возвращает частичное представление. Требуется для обновления контента без перезагрузки формы</returns>
        [HttpGet]
        public ActionResult PartialIndex(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            //Первоначальные определения для формирования модели представления
            List<string> usersModel = new List<string>();

            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                //Извлечение полного списка пользователей
                foreach (users elem in db.users)
                    usersModel.Add(elem.Login);
            }

            //Формирование конечной модели
            ReminderViewModel reminderViewModel = new ReminderViewModel()
            {
                Users = usersModel
            };
            ViewBag.ReminderViewModel = reminderViewModel;
            ViewBag.IdDocument = id;
            return View("ReminderPartial");
        }

        /// <summary>
        /// Функция, передающая частичное представление с напоминаниями для документа
        /// </summary>
        /// <param name="id">Идентификатор документа</param>
        /// <returns>Возвращает частичное представление. Требуется для обновления контента без перезагрузки формы</returns>
        [HttpGet]
        public ActionResult PartialListReminders(int id)
        {
            //Если пользователь не авторизован - отправляем на страницу авторизации
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //Первоначальные определения для формирования модели представления
            List<ReminderModel> reminderModels = new List<ReminderModel>();
            bool isReminderModels = false;

            //Получаем данные из БД
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                //Получение идентификатора пользователя
                users user = db.users.FirstOrDefault(u => u.Login == User.Identity.Name);
                int id_user = 0;
                if (user != null)
                    id_user = user.id_users;

                //Формирование списка напоминаний
                if (id != 0)
                {
                    //Извлечение полного списка напоминаний для пользователя
                    List<Reminders> tmpReminders = db.Reminders.Where(u => u.IdRecipient == id_user && u.Visible == true && u.IdDocument == id).ToList();
                    if (tmpReminders.Count > 0)
                    {
                        isReminderModels = true;
                        foreach (Reminders element in tmpReminders)
                        {
                            reminderModels.Add(new ReminderModel()
                            {
                                Id = element.Id,
                                DateReminder = element.DateReminder,
                                Text = element.Text,
                                Login = db.users.FirstOrDefault(u => u.id_users == element.IdSender).Login
                            });
                        }
                    }
                }
            }
            //Формирование конечной модели
            ReminderViewModel reminderViewModel = new ReminderViewModel()
            {
                IsReminderModels = isReminderModels,
                ReminderModels = reminderModels
            };
            ViewBag.ReminderViewModel = reminderViewModel;
            ViewBag.IdDocument = id;
            return PartialView("ReminderPart");
        }

        /// <summary>
        /// Создание нового напоминания в частичном представлении
        /// </summary>
        [HttpPost]
        public ActionResult AddPartial(ReminderPartialModelAdd model)
        {
            //Если пользователь не авторизован - отправляем на страницу авторизации
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //Первоначальные определения для формирования модели представления
            List<ReminderModel> reminderModels = new List<ReminderModel>();
            bool isReminderModels = false;

            //Проверяем правильно ли заполнена информация в модели
            if (ModelState.IsValid)
            {
                //Начинаем добавление нового поручения
                using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
                {
                    //Получение идентификатора отправителя
                    users user_sender = db.users.FirstOrDefault(u => u.Login == User.Identity.Name);
                    int id_sender = 0;
                    if (user_sender == null)
                    {
                        ModelState.AddModelError("", "Вы не можете добавлять напоминания");
                        return View("Index", model);
                    }

                    id_sender = user_sender.id_users;

                    //Получение идентификатора получателя
                    users user_recipient = db.users.FirstOrDefault(u => u.Login == model.Login);
                    int id_recipient = 0;
                    if (user_recipient == null)
                    {
                        ModelState.AddModelError("", "Пользователя, которому адресовано напоминание, не существует");
                        return View("Index", model);
                    }

                    id_recipient = user_recipient.id_users;

                    //Создаем новую запись
                    db.Reminders.Add(new Reminders()
                    {
                        IdSender = id_sender,
                        IdRecipient = id_recipient,
                        IdDocument = model.IdDocument,
                        DateReminder = DateTime.Now,
                        Text = model.Text,
                        Visible = true
                    });
                    db.SaveChanges();


                    //Получение идентификатора пользователя
                    users user = db.users.FirstOrDefault(u => u.Login == User.Identity.Name);
                    int id_user = 0;
                    if (user != null)
                        id_user = user.id_users;

                    //Формирование списка напоминаний
                    if (id_user != 0)
                    {
                        //Извлечение полного списка напоминаний для пользователя
                        List<Reminders> tmpReminders = db.Reminders.Where(u => u.IdRecipient == id_user && u.Visible == true && u.IdDocument == model.IdDocument).ToList();
                        if (tmpReminders.Count > 0)
                        {
                            isReminderModels = true;
                            foreach (Reminders element in tmpReminders)
                            {
                                reminderModels.Add(new ReminderModel()
                                {
                                    Id = element.Id,
                                    DateReminder = element.DateReminder,
                                    Text = element.Text,
                                    Login = db.users.FirstOrDefault(u => u.id_users == element.IdSender).Login
                                });
                            }
                        }
                    }
                }
            }
            //Формирование конечной модели
            ReminderViewModel reminderViewModel = new ReminderViewModel()
            {
                IsReminderModels = isReminderModels,
                ReminderModels = reminderModels
            };
            ViewBag.ReminderViewModel = reminderViewModel;
            ViewBag.IdDocument = model.IdDocument;
            return PartialView("ReminderPart");
        }
    }
}