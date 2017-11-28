using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MashZavod.Models.DbModels;

namespace MashZavod.Controllers
{
    [RoutePrefix("api/users")]
    public class usersController : ApiController
    {
        private database_murom_factory2Entities1 db = new database_murom_factory2Entities1();

        //Регистрация нового пользователя.
        //Во входных параметрах должен поступать объект типа user
        [HttpPost]
        public System.Web.Mvc.ActionResult Register(users user)
        {
            //Проверяем не содержит ли БД такого пользователя
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////Не хватает вида, который нужно отправить при такой ошибке////////////////////////////
            foreach (var elem in db.users)
                if (user.Login == elem.Login)
                    return null;
            //Добавляем нового пользователя в базу
            db.users.Add(user);
            db.SaveChanges();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////Не хватает вида, который нужно отправить при успехе//////////////////////////////////
            return null;
        }

        //Вход пользователя
        [HttpPost]
        public System.Web.Mvc.ActionResult Login(string login, string password)
        {
            //Проверяем существует ли связка логина и пароля в БД
            int id = 0;
            foreach(var elem in db.users)
                if(elem.Login == login && elem.Password == password)
                    id = elem.id_users;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////Не хватает вида, который нужно отправить при такой ошибке////////////////////////////
            if (id < 0)
                return null;
            //Проверяем является ли пользователь суперпользователем
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////Не хватает вида, который нужно отправить при успехе//////////////////////////////////
            if (id == 1)
                return null;
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////
            //////////////////////Не хватает вида, который нужно отправить при успехе//////////////////////////////////
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool usersExists(int id)
        {
            return db.users.Count(e => e.id_users == id) > 0;
        }
    }
}