using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MashZavod.Models.DbModels;
using MashZavod.Models.MissionsModels;

namespace MashZavod.Controllers
{
    /// <summary>
    /// Контроллер осуществляет взаимодействие с моделью БД - Поручения
    /// </summary>
    public class MissionController : Controller
    {
        /// <summary>
        /// Возвращает страницу с поручениями
        /// </summary>
        public ActionResult Index()
        {
            //Если пользователь не авторизован - отправляем на страницу авторизации
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //Первоначальные определения для формирования модели представления
            List<MissionModel> inModel = new List<MissionModel>();
            bool isInModel = false;
            List<MissionModel> outModel = new List<MissionModel>();
            bool isOutModel = false;
            List<string> usersModel = new List<string>();

            //Получаем данные из БД
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                //Получение идентификатора пользователя
                users user = db.users.FirstOrDefault(u => u.Login == User.Identity.Name);
                int id = 0;
                if (user != null)
                    id = user.id_users;

                //Формирование списков поручений
                if (id != 0)
                {
                    //Извлечение полного списка поручений для пользователя
                    List<Missions> tmpMissionIn = db.Missions.Where(u => u.IdRecipient == id && u.Visible == true).ToList();
                    if (tmpMissionIn.Count > 0)
                    {
                        isInModel = true;
                        foreach (Missions element in tmpMissionIn)
                        {
                            inModel.Add(new MissionModel()
                            {
                                Id = element.Id,
                                DateMission = element.DateMission,
                                Text = element.Text,
                                Login = db.users.FirstOrDefault(u => u.id_users == element.IdSender).Login,
                                Done = false
                            });
                        }
                    }

                    //Извлечение полного списка поручений от пользователя
                    List<Missions> tmpMissionOut = db.Missions.Where(u => u.IdSender == id && u.Done == true).ToList();
                    if (tmpMissionOut.Count > 0)
                    {
                        isOutModel = true;
                        foreach (Missions element in tmpMissionOut)
                        {
                            outModel.Add(new MissionModel()
                            {
                                Id = element.Id,
                                DateMission = element.DateMission,
                                Text = element.Text,
                                Login = db.users.FirstOrDefault(u => u.id_users == element.IdRecipient).Login,
                                Done = (element.Visible) ? false : true
                            });
                        }
                    }
                }
                //Извлечение полного списка пользователей
                foreach (users elem in db.users)
                    usersModel.Add(elem.Login);
            }

            //Формирование конечной модели
            MissionViewModel missionViewModel = new MissionViewModel()
            {
                InMissionModels = inModel,
                IsInMissionModels = isInModel,
                OutMissionModels = outModel,
                IsOutMissionModels = isOutModel,
                Users = usersModel
            };
            ViewBag.MissionViewModel = missionViewModel;
            return View();
        }

        /// <summary>
        /// Создание нового поручения
        /// </summary>
        [HttpPost]
        public ActionResult Add(MissionModelAdd model)
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
                        ModelState.AddModelError("", "Вы не можете добавлять поручения");
                        return RedirectToAction("Index", "Mission", model);
                    }

                    id_sender = user_sender.id_users;

                    //Получение идентификатора получателя
                   users user_recipient = db.users.FirstOrDefault(u => u.Login == model.Login);
                    int id_recipient = 0;
                    if (user_recipient == null)
                    {
                        ModelState.AddModelError("", "Пользователя, которому адресовано поручение, не существует");
                        return RedirectToAction("Index", "Mission", model);
                    }

                    id_recipient = user_recipient.id_users;

                    //Создаем новую запись
                    db.Missions.Add(new Missions()
                    {
                        IdSender = id_sender,
                        IdRecipient = id_recipient,
                        IdDocument = 0,
                        DateMission = DateTime.Now,
                        Text = model.Text,
                        Done = true,
                        Visible = true
                    });
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index", "Mission");
        }

        /// <summary>
        /// Функция, позволяющая скрыть поручение, если пользователь посчитает, что выполнил его
        /// </summary>
        /// <param name="id">Идентификатор поручения</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HideMission(int id)
        {
            //Если пользователь не авторизован - отправляем на страницу авторизации
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //Начинаем изменение состояния поручения
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                //проверяем есть ли такое поручение
                Missions mission = db.Missions.FirstOrDefault(u => u.Id == id);

                if (mission == null)
                {
                    ModelState.AddModelError("", "Такого поручения не существует");
                    return RedirectToAction("Index", "Mission");
                }

                //Получение идентификатора получателя
                users user_recipient = db.users.FirstOrDefault(u => u.Login == User.Identity.Name);
                int id_recipient = 0;
                if (user_recipient != null)
                    id_recipient = user_recipient.id_users;

                //Проверяем принадлежит ли поручение пользователю
                if (mission.IdRecipient != id_recipient)
                {
                    ModelState.AddModelError("", "У вас нет доступа к данному поручению");
                    return RedirectToAction("Index", "Mission");
                }

                mission.Visible = false;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Mission");
        }

        [HttpGet]
        public ActionResult DoneMission(int id)
        {
            //Если пользователь не авторизован - отправляем на страницу авторизации
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //Начинаем изменение состояния поручения
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                //проверяем есть ли такое поручение
                Missions mission = db.Missions.FirstOrDefault(u => u.Id == id);

                if (mission == null)
                {
                    ModelState.AddModelError("", "Такого поручения не существует");
                    return RedirectToAction("Index", "Mission");
                }

                //Получение идентификатора отправителя
                users user_sender = db.users.FirstOrDefault(u => u.Login == User.Identity.Name);
                int id_sender = 0;
                if (user_sender != null)
                    id_sender = user_sender.id_users;

                //Проверяем принадлежит ли поручение пользователю
                if (mission.IdSender != id_sender)
                {
                    ModelState.AddModelError("", "У вас нет доступа к данному поручению");
                    return RedirectToAction("Index", "Mission");
                }

                mission.Done = false;
                mission.Visible = false;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Mission");
        }

        [HttpGet]
        public ActionResult NoDoneMission(int id)
        {
            //Если пользователь не авторизован - отправляем на страницу авторизации
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            //Начинаем изменение состояния поручения
            using (database_murom_factory2Entities1 db = new database_murom_factory2Entities1())
            {
                //проверяем есть ли такое поручение
                Missions mission = db.Missions.FirstOrDefault(u => u.Id == id);

                if (mission == null)
                {
                    ModelState.AddModelError("", "Такого поручения не существует");
                    return RedirectToAction("Index", "Mission");
                }

                //Получение идентификатора отправителя
                users user_sender = db.users.FirstOrDefault(u => u.Login == User.Identity.Name);
                int id_sender = 0;
                if (user_sender != null)
                    id_sender = user_sender.id_users;

                //Проверяем принадлежит ли поручение пользователю
                if (mission.IdSender != id_sender)
                {
                    ModelState.AddModelError("", "У вас нет доступа к данному поручению");
                    return RedirectToAction("Index", "Mission");
                }

                mission.Visible = true;
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Mission");
        }
    }
}