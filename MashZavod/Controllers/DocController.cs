using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;




using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

using Word = Microsoft.Office.Interop.Word;
using MashZavod.Models.DbModels;
using System.IO;

namespace MashZavod.Controllers
{
    public class DocController : Controller
    {
        //отображение формы загрузки документа на сервер
        public ActionResult AddDoc()
        {
            return View();
        }
        database_murom_factory2Entities1 db = new database_murom_factory2Entities1();
        //загрузка документа на сервер
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload, doc modelDoc, users recipient)
        {
            if (upload != null)
            {
                /*
                как-то сделать переименование файла при добавлении на сервер, 
                если что-то введено и добавление ему соответствующего расширения
                */
                // получаем имя файла
                if (modelDoc.name_doc == String.Empty || modelDoc.name_doc == null) //пользователь не задал название файла
                {
                    modelDoc.name_doc = System.IO.Path.GetFileName(upload.FileName);
                }
                else modelDoc.name_doc += System.IO.Path.GetExtension(upload.FileName);
                //поиск получателя
                if (recipient.Login == null || db.users.FirstOrDefault(u => u.Login == recipient.Login) == null)
                {
                    modelDoc.recipient = null;
                }
                else
                {
                    modelDoc.recipient = db.users.FirstOrDefault(u => u.Login == recipient.Login).id_users;
                }

                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Files/" + upload.FileName));
                /*заменить на id пользователя*/
                modelDoc.author = db.users.FirstOrDefault(u => u.Login == User.Identity.Name).id_users;// +" "+ db.users.FirstOrDefault(u => u.Login == User.Identity.Name).Name + " " + db.users.FirstOrDefault(u => u.Login == User.Identity.Name).Patronymic;

                modelDoc.data_of_create = DateTime.Now;
                // modelDoc.date_of_modify = DateTime.Now;
                //изменить при добавлении переименования документа
                modelDoc.url = Server.MapPath("~/Files/" + System.IO.Path.GetFileName(upload.FileName));
                modelDoc.text_doc = ExtractText(modelDoc.url);
                //получаем описание файла
                if ((modelDoc.description == String.Empty || modelDoc.description == null) && modelDoc.text_doc != null)
                {
                    if (modelDoc.text_doc.Length >= 150)
                    {
                        char[] textchar = new char[150];
                        modelDoc.text_doc.CopyTo(0, textchar, 0, 150);
                        modelDoc.description = new string(textchar);
                    }
                    else
                    {
                        char[] textchar = new char[modelDoc.text_doc.Length];
                        modelDoc.text_doc.CopyTo(0, textchar, 0, modelDoc.text_doc.Length);
                        modelDoc.description = new string(textchar);
                    }
                }
                /*при привязки одной модели к другой происходит вылет*/

                /*добавление в бд и сохранение*/
                //   db.doc.Add(modelDoc);
                // db.SaveChanges();
                //    modelInfoDoc.id_doc = modelDoc.id_doc;
                // modelInfoDoc.doc = modelDoc;
                //         modelDoc.infoFileDoc.Add(modelInfoDoc);
                db.doc.Add(modelDoc);
                //   db.SaveChanges();
                modelDoc.id_doc = modelDoc.id_doc;
                // db.infoFileDoc.Add(modelDoc);
                db.SaveChanges();
                return RedirectToAction("ViewDoc", "Doc");
                //   string n = modelDoc.infoFileDoc.ElementAtOrDefault(0).name_doc;
                //    modelInfoDoc.name_doc = modelDoc.infoFileDoc.Last().name_doc;
            }

            return RedirectToAction("AddDoc");
        }


        //скачивание документа с сервера на клиент
        [HttpGet]
        public ActionResult DowloadDoc(int id)
        {
            //infoFileDoc modelInfoDoc = modelDoc.infoFileDoc.Last();
            // Путь к файлу
            doc modelDoc = db.doc.FirstOrDefault(u => u.id_doc == id);
            if (modelDoc == null)
            {
                return RedirectToAction("ViewDoc", "Doc");
            }
            string file_path = modelDoc.url;// Server.MapPath("~/Files/PDFIcon.pdf");
            // Тип файла
            string file_type = String.Empty;
            //получение типа файла MIME
            switch (System.IO.Path.GetExtension(modelDoc.url))
            {
                case ".pdf":
                    {
                        file_type = "application/pdf";
                        break;
                    }
                case ".doc":
                    {
                        file_type = "application/msword";
                        break;
                    }
                case ".docx":
                    {
                        file_type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    }
            }
            //     string file_type = "application/pdf";
            // Имя файла - необязательно
            string file_name = modelDoc.name_doc; //"PDFIcon.pdf";
            return File(file_path, file_type, file_name);


            //  return View();
        }
        //удаление файла
        [HttpGet]
        public ActionResult DeleteDoc(int id)
        {
            doc modelDoc = db.doc.FirstOrDefault(u => u.id_doc == id);
            if (modelDoc == null)
            {
                return RedirectToAction("ViewDoc", "Doc");
            }

            System.IO.File.Delete(modelDoc.url);
            /*удалить все данные из других таблиц, которые связаны с doc*/
            db.doc.Remove(modelDoc);
            db.SaveChanges();
            return RedirectToAction("ViewDoc", "Doc");
        }
        //открываем форму
        //  [HttpGet]
        public ActionResult EditDoc(int id)
        {
            doc modelDoc = db.doc.FirstOrDefault(u => u.id_doc == id);
            if (modelDoc == null)
            {
                return RedirectToAction("ViewDoc", "Doc");
            }
            ViewBag.docEdit = modelDoc;
            ViewBag.logUser = null;
            if (modelDoc.recipient != null)
            {
                ViewBag.logUser = db.users.FirstOrDefault(u => u.id_users == modelDoc.recipient).Login;
            }

            //  ViewBag
            return View();
        }
        /*  public ActionResult EditDoc()
          {
              return View();
          }*/

        [HttpPost]
        public ActionResult Reload(doc modelDoc, users recipient)
        {
            if (modelDoc == null)
            {
                return RedirectToAction("ViewDoc", "Doc");
            }
            doc defaultDoc = db.doc.FirstOrDefault(u => u.id_doc == modelDoc.id_doc);

            if (modelDoc.name_doc != defaultDoc.name_doc)
            {
                defaultDoc.name_doc = modelDoc.name_doc;
            }
            if (modelDoc.description != defaultDoc.description)
            {
                defaultDoc.description = modelDoc.description;
            }
            //   recipient.id_users = null;
            if (db.users.FirstOrDefault(u => u.Login == recipient.Login) != null)
            {
                recipient.id_users = db.users.FirstOrDefault(u => u.Login == recipient.Login).id_users;
                if (recipient.id_users != defaultDoc.recipient && recipient.id_users != 0)
                {
                    defaultDoc.recipient = recipient.id_users;
                }

                //else

            }
            //  db.doc.(defaultDoc.id_doc) = defaultDoc;
            //db.doc.Remove(defa+ultDoc);
            //   db.doc.
            db.SaveChanges();
            return RedirectToAction("ViewDoc", "Doc");
            /*  if (recipient.Login!=null)
              {

              }*/



        }

        /* [HttpPost]
         public ActionResult EditDoc(int id)
         {
             doc modelDoc = db.doc.FirstOrDefault(u => u.id_doc == id);
             if (modelDoc == null)
             {
                 return RedirectToAction("ViewDoc", "Doc");
             }
             ViewBag 
            // ViewBag
             return View();
         }*/

        //отображение формы показа всех доступных документов
        public ActionResult ViewDoc()
        {
            List<doc> docList = new List<doc>();
            // List<string> nameList = new List<string>();
            //if ()
            int y = 0;
            users us = db.users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (us != null)
                y = us.id_users;// db.users.FirstOrDefault(u => u.Login == User.Identity.Name).id_users;
            var req_doc = db.doc;//.f(u=>u.author==y || u.recipient== y);
            foreach (var el in req_doc)
            {
                if (el.author == y || el.recipient == y || el.recipient == null)
                {
                    docList.Add(el);
                    //     nameList.Add(el.infoFileDoc.ElementAtOrDefault(0).name_doc);
                }
            }
            ViewBag.listDoc = docList;
            //  ViewBag.listName = nameList;
            // nameList.e
            return View();
        }

        /*просмотр списка отправленных документов*/
        public ActionResult ViewSentDoc()
        {
            List<doc> docList = new List<doc>();
            int y = db.users.FirstOrDefault(u => u.Login == User.Identity.Name).id_users;
            var req_doc = db.doc;
            foreach (var el in req_doc)
            {
                if (el.author == y)
                {
                    docList.Add(el);
                }
            }
            ViewBag.listDoc = docList;
            return View();
        }
        /*просмотр списка полученных документов*/
        public ActionResult ViewReceivedDoc()
        {
            List<doc> docList = new List<doc>();
            int y = db.users.FirstOrDefault(u => u.Login == User.Identity.Name).id_users;
            var req_doc = db.doc;
            foreach (var el in req_doc)
            {
                if (el.recipient == y || (el.recipient == null && el.author != y))
                {
                    docList.Add(el);
                }
            }
            ViewBag.listDoc = docList;
            return View();
        }

        [HttpPost]
        public ActionResult Search(string searchText)
        {
           // string textSearch = Request.Form["searchText.Value"];
            List<doc> docList = new List<doc>();
            // List<string> nameList = new List<string>();
            //if ()
            int y = 0;
            users us = db.users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (us != null)
                y = us.id_users;// db.users.FirstOrDefault(u => u.Login == User.Identity.Name).id_users;
            var req_doc = db.doc;//.f(u=>u.author==y || u.recipient== y);
            foreach (var el in req_doc)
            {
                if ((el.author == y || el.recipient == y || el.recipient == null) && (el.text_doc.Contains(searchText)|| el.name_doc.Contains(searchText)))
                {
                    docList.Add(el);
                    //     nameList.Add(el.infoFileDoc.ElementAtOrDefault(0).name_doc);
                }
            }
            ViewBag.listDoc = docList;
            //  ViewBag.listName = nameList;
            // nameList.e
            // return View();
            return View("SearchDoc");
            //return RedirectToAction("SearchResult?searchText="+ searchText, "Doc");

        }

        public ActionResult SearchDoc()
        {

            return View();
        }
       /* public ActionResult SearchResult()
        {
            string searchText = Request.QueryString["searchText"];
            List<doc> listDoc = new List<doc>();
            // List<string> nameList = new List<string>();
            //if ()
            int y = 0;
            users us = db.users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (us != null)
                y = us.id_users;// db.users.FirstOrDefault(u => u.Login == User.Identity.Name).id_users;
            var req_doc = db.doc;//.f(u=>u.author==y || u.recipient== y);
            foreach (var el in req_doc)
            {
                if ((el.author == y || el.recipient == y || el.recipient == null) && (el.text_doc.Contains(searchText) || el.name_doc.Contains(searchText)))
                {
                    listDoc.Add(el);
                    //     nameList.Add(el.infoFileDoc.ElementAtOrDefault(0).name_doc);
                }
            }
            ViewBag.listDoc = listDoc;
            return View();
        }*/
        public string ExtractText(string path) //извлечение текста
        {
            switch (System.IO.Path.GetExtension(path))
            {
                case ".doc":
                    {
                        string text = ExtractTextFromDoc(path);
                        return text;
                    }
                case ".docx":
                    {

                        string text = ExtractTextFromDoc(path);
                        return text;
                    }
                case ".pdf":
                    {
                        string text = ExtractTextFromPdf(path);
                        return text;
                    }
                default:
                    {
                        return String.Empty;
                    }
            }
        }

        /*парсинг pdf*/
        private string ExtractTextFromPdf(string path)
        {
            ITextExtractionStrategy its = new iTextSharp.text.pdf.parser.LocationTextExtractionStrategy();
            using (PdfReader reader = new PdfReader(path))
            {
                System.Text.StringBuilder text = new System.Text.StringBuilder();
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    string thePage = PdfTextExtractor.GetTextFromPage(reader, i, its);
                    string[] theLines = thePage.Split('\n');
                    foreach (var theLine in theLines)
                    {
                        if (text.Length + theLine.Length <= 8000)
                            text.AppendLine(theLine);
                    }
                }
                return text.ToString();
            }
        }

        /*конвертирование doc/docx в текст*/
        private string ExtractTextFromDoc(string path)
        {
            string output = "";
            Word.Application app = new Word.Application();
            Object fileName = path;
            app.Documents.Open(ref fileName);
            Word.Document doc = app.ActiveDocument;
            // Нумерация параграфов начинается с одного
            for (int i = 1; i < doc.Paragraphs.Count; i++)
            {
                string parText = doc.Paragraphs[i].Range.Text;
                if (output.Length + parText.Length <= 8000)
                    output += parText;
            }
            app.Quit();
            return output;
        }


        /*--------------------------------------------------------------------------------*/
        /*для отправки на клиент*/ /*доделать, чтобы извлекать из бд*/
                                   /* public FileResult GetFile()
                                    {

                                        // Путь к файлу
                                        string file_path = Server.MapPath("~/Files/PDFIcon.pdf");
                                        // Тип файла - content-type
                                        string file_type = "application/pdf";
                                        // Имя файла - необязательно
                                        string file_name = "PDFIcon.pdf";
                                        return File(file_path, file_type, file_name);
                                    }*/


        [HttpGet]
        public ActionResult GetDocInfo(int id)
        {
            ViewBag.Id = id;
            return View("DocDetails");
        }
    }
}