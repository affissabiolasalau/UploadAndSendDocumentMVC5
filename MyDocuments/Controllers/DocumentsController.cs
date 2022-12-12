using MyDocuments.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using MyDocuments.Services;
using System.Threading.Tasks;

namespace MyDocuments.Controllers
{
    public class DocumentsController : Controller
    {
        //Connection String
        public readonly ConnectionStrings db;
        public DocumentsController(ConnectionStrings Db)
        {
            db = Db;
        }

        public IActionResult Index()
        {
            try
            {
                var docs = db.Transactions.OrderByDescending(x => x.Id).Where(x => x.Status == 1).ToList();

                ViewBag.Docs = docs;
            }
            catch (Exception ex)
            {
                ViewBag.Docs = null;
               Console.WriteLine(ex.ToString());
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Search)
        {
            try
            {
                var Docs_ = db.Transactions.OrderByDescending(x => x.Id);

                var Docs = Docs_
                .Where(x => (x.Status == 1 && x.TransactionId.Contains(Search)) || (x.Status == 1 && x.Email.Contains(Search)))
                .ToList();

                ViewBag.Docs = Docs;

            }
            catch (Exception ex)
            {
                ViewBag.Docs = null;
            }

            return View();

        }

        [HttpGet]
        public ActionResult ViewDocument(string Id)
        {
            try
            {
                var files = db.Files.OrderByDescending(x => x.Id).Where(x => x.Status == 1 && x.TransactionReference == Id).ToList();
                ViewBag.Files = files;
            }
            catch (Exception)
            {
                ViewBag.Files = null;
            }
           
            return View();
        }

        [HttpGet]
        public ActionResult ViewAllDocuments(int Id)
        {
            try
            {
                var files = db.Files.OrderByDescending(x => x.Id).Where(x => x.Status == 1 && x.UserId == Id).ToList();
                ViewBag.Files = files;
            }
            catch (Exception)
            {
                ViewBag.Files = null;
            }

            return View();
        }

        
        [HttpGet]
        public ActionResult GetUserDocuments(string Id)
        {
            try
            {
                var Docs_ = db.Transactions.OrderByDescending(x => x.Id);

                var Docs = Docs_
                .Where(x => (x.Status == 1 && x.TransactionId.Contains(Id)) || (x.Status == 1 && x.Email.Contains(Id)) )
                .ToList();
                
                ViewBag.Docs = Docs;                
                
            }
            catch (Exception ex)
            {
                ViewBag.Docs = null;
            }

            return Json(new { ViewBag.Docs });

        }



        [HttpGet]
        public ActionResult DeleteDocumentRows(string Id)
        {
            
            try
            {
                var files = db.Files.OrderByDescending(x => x.Id).Where(x => x.Status == 1 && x.TransactionReference == Id).ToList();
                var trans = db.Transactions.OrderByDescending(x => x.Id).Where(x => x.Status == 1 && x.TransactionId == Id).FirstOrDefault();

                if (trans != null)
                {
                   
                    foreach (var item in files)
                    {
                        //delete from files db
                        db.Files.Remove(item);
                        db.SaveChangesAsync();
                        //delete from transactions db
                        db.Transactions.Remove(trans);
                        db.SaveChangesAsync();
                        //delete from folder
                        DocumentServices.DeleteFile(item.FilePath);
                    }

                    if(files.Count == 0)
                    {
                        //delete from transactions db
                        db.Transactions.Remove(trans);
                        db.SaveChangesAsync();
                    }
                }
                ViewBag.Status = 1;
                ViewBag.Message = "Deleted!";

            }
            catch (Exception)
            {
                ViewBag.Status = 0;
                ViewBag.Message = "Failed. Please try again.";
            }

            ResponseModel response = new ResponseModel()
            {
                Status = ViewBag.Status,
                Message = ViewBag.Message
            };

            return Json(new { response });
            //return View();
        }


        [HttpGet]
        public async Task<ActionResult> DeleteOneDocument(int Id)
        {

            try
            {
                var files = db.Files.OrderByDescending(x => x.Id).Where(x => x.Status == 1 && x.Id == Id);
                var file = files.FirstOrDefault();
                var trans = db.Transactions.OrderByDescending(x => x.Id).Where(x => x.Status == 1 && x.TransactionId == file.TransactionReference).FirstOrDefault();

                if (files != null)
                {
                    //delete from folder
                    var deleteFile = DocumentServices.DeleteFile(file.FilePath);

                        //delete from files db
                        db.Files.Remove(file);
                        await db.SaveChangesAsync();
                    //check if file is left, else, delete transaction, else remove from count
                    var files_ = db.Files.Where(x => x.Status == 1 && x.TransactionReference == file.TransactionReference).Count();
                    if (files_ < 1)
                        {
                            //delete from transactions db
                            db.Transactions.Remove(trans);
                            await db.SaveChangesAsync();
                    }
                    else
                    {
                        TransactionsModel transactions = db.Transactions.Where(x => x.TransactionId == file.TransactionReference).FirstOrDefault();
                        transactions.FileCount -= 1;
                        db.SaveChanges();
                    }

                }
                ViewBag.Status = 1;
                ViewBag.Message = "Deleted!";

            }
            catch (Exception)
            {
                ViewBag.Status = 0;
                ViewBag.Message = "Failed. Please try again.";
            }

            ResponseModel response = new ResponseModel()
            {
                Status = ViewBag.Status,
                Message = ViewBag.Message
            };

            return Json(new { response });
            //return View();
        }
    }
}
