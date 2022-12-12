using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using MyDocuments.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using System.Linq;
using MyDocuments.Utilities;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace MyDocuments.Controllers
{
    public class UploadController : Controller
    {
        private IHostingEnvironment Environment;

        //Connection String
        public readonly ConnectionStrings db;
        public UploadController(ConnectionStrings Db, IHostingEnvironment _environment)
        {
            db = Db;

            Environment = _environment;
        }

        public IActionResult Index()
        {
            try
            {
                var users = db.Users.OrderByDescending(x => x.Id).ToList();

                if (users != null)
                {
                    foreach (var us in users)
                    {
                        ViewBag.UsersEmail += "<option value=" + us.Email + ">" + us.Email + "</option>";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(List<IFormFile> uploadFiles, string UserEmail)
        {
            ResponseModel response = new ResponseModel()
            {
                Status = 0,
                Message = null
            };

            int pk;
            
            string TransactionId = RandomKeys.String(10);

            FilesModel files = new FilesModel();
            //MailerModel mailer = new MailerModel();
            

                try
            {
                var checkTransaction = db.Transactions.Where(y => y.TransactionId == TransactionId).Count();
                
                var users = db.Users.OrderByDescending(x => x.Id).ToList();

                if (users != null)
                {
                    foreach (var us in users)
                    {
                        ViewBag.UsersEmail += "<option value=" + us.Email + ">" + us.Email + "</option>";
                    }
                }

                var user = db.Users.Where(x => x.Email == UserEmail).FirstOrDefault();

                if (uploadFiles.Count < 1)
                {
                    ViewBag.Status = 0;
                    ViewBag.Message = "No file was selected.";

                }
                else if (uploadFiles.Count > 5)
                {
                    ViewBag.Status = 0;
                    ViewBag.Message = "Files should not be more than 5.";

                }
                else if (checkTransaction > 0)
                {
                    ViewBag.Status = 0;
                    ViewBag.Message = "Duplicate transaction. Please try again.";

                }
                else if (UserEmail == null || UserEmail.Length < 1)
                {
                    ViewBag.Status = 0;
                    ViewBag.Message = "Please, select a email address";
                }
                else if (UserEmail == null)
                {
                    ViewBag.Status = 0;
                    ViewBag.Message = "Please, select a email address";
                }
                else if (user == null)
                {
                    ViewBag.Status = 0;
                    ViewBag.Message = "Account does not exist.";
                }
                else
                {

                    //save to transactions                    
                    try
                    {
                        int max = db.Transactions.Max(p => p.Id);
                        pk = max + 1;

                    }
                    catch (Exception)
                    {
                        pk = 1;
                    }
                    TransactionsModel tr = new TransactionsModel()
                    {
                        TransactionId = TransactionId,
                        FileCount = uploadFiles.Count,
                        Email = user.Email,
                        UserId = user.Id,
                        Id = pk,
                        Status = 1
                    };

                    db.Transactions.Add(tr);
                    db.SaveChanges();


                    string path = Path.Combine(Environment.WebRootPath, "Documents");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    //Upload files
                    List<string> uploadedFiles = new List<string>();
                    foreach (IFormFile UploadFile in uploadFiles)
                    {
                        string fileName = Path.GetFileName(UploadFile.FileName);
                        string fileType = Path.GetFileName(UploadFile.ContentType);

                        //checking for duplicate file
                        //var checkDublicate = db.Files.Where(y => y.Email == UserEmail && y.FileTitle == fileName).Count();
                        /*
                        if(checkDublicate > 0)
                        {
                            ViewBag.Status = 0;
                            ViewBag.Message += string.Format("{0} {1} file already uploaded", fileName, fileType);
                            return View();
                        }
                        */



                        using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            UploadFile.CopyTo(stream);
                            uploadedFiles.Add(fileName);


                            try
                            {
                                int max = db.Files.Max(p => p.Id);
                                pk = max + 1;

                            }
                            catch (Exception)
                            {
                                pk = 1;
                            }
                            files.TransactionReference = TransactionId;
                            files.FileType = fileType;
                            files.FilePath = stream.Name;
                            files.Id = pk;
                            files.FileTitle = fileName;
                            files.Status = 1;
                            files.Email = user.Email;
                            files.UserId = user.Id;

                            //save files
                            db.Files.Add(files);
                            db.SaveChanges();

                            ViewBag.Status = 1;
                            ViewBag.Message += string.Format("{0} UPLOADED!<br>", fileName);
                        }

                    }

                    
                    string senderMail = ""; //username
                    string senderPassword = ""; //email password
                    string mailerHost = " "; //email host
                    int mailerPort = 26; //your email port
                    bool enableSSl = false; //true or false

                    using (MailMessage mail = new MailMessage(senderMail, UserEmail))
                    {
                        mail.Subject = "Document Uploads";
                        mail.Body = "Find the attached document";

                        foreach (IFormFile UploadFile in uploadFiles)
                        {
                            string fileName = Path.GetFileName(UploadFile.FileName);
                            string fileType = Path.GetFileName(UploadFile.ContentType);
                            string fullPath = Path.Combine(path, fileName);
                            string fileNameFromPath = Path.GetFileName(fullPath);

                            var fileForEmail = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                            mail.Attachments.Add(new Attachment(fileForEmail, fileName, MediaTypeNames.Application.Octet));

                        }

                        //sending mail
                        try
                        {
                            mail.IsBodyHtml = true;
                            using (SmtpClient smtp = new SmtpClient())
                            {
                                smtp.Host = mailerHost;
                                smtp.EnableSsl = enableSSl;
                                NetworkCredential networkCredential = new NetworkCredential(senderMail, senderPassword);
                                smtp.UseDefaultCredentials = false;
                                smtp.Credentials = networkCredential;
                                smtp.Port = mailerPort;
                                smtp.Send(mail);
                            };
                            
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ViewBag.Status = 0;
                ViewBag.Message = "Upload failed. Please try again. ";
            }
            
            return View();
        }
    }
}
