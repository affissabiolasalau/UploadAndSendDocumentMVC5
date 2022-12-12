using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyDocuments.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MyDocuments.Utilities;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace MyDocuments.Controllers
{
    public class HomeController : Controller
    {
       
        //Connection String
        public readonly ConnectionStrings db;
        public HomeController(ConnectionStrings Db)
        {
            db = Db;
        }

        public IActionResult Index()
        {
            ViewData["Greetings"] = Greetings.GreetingByTime();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([FromBody] UsersModel user)
        {
            ResponseModel response = new ResponseModel()
            {
                Status = 0,
                Message = null
            };

            try
            {
                
                int pk;

                try
                {
                    int max = db.Users.Max(p => p.Id);
                    pk = max + 1;

                }
                catch (Exception)
                {
                    pk = 1;
                }

                UsersModel usersModel = new UsersModel()
                {
                    Id = pk,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                user.Id = pk;

                var checkEmail = db.Users.Where(x => x.Email == user.Email).Count();

                 if (checkEmail > 0)
                {
                    response.Message = "Email address is already in use.";
                }
                else if (user.Email != null && Validate.Email(user.Email) == false)
                {
                    response.Message = "Email address format not acceptable";
                }
               else if (ModelState.IsValid)
                {
                    db.Users.Add(usersModel);
                    var success = await db.SaveChangesAsync();
                    if (success == 1)
                    {
                        response.Status = 1;
                        response.Message = "Account created!";
                    }
                    else
                    {
                        response.Message = "Internal server error. Please try again later.";
                        //return Json(new { response.Status, response.Message });
                    }
                    
                }
                else
                {

                    response.Message = "Information can not be processed, please check your input and try again.";
                    

                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            //ViewBag.Status = response.Status;
            //ViewData["Message"] = response.Status;
            //return View();
            return Json(new { response });
        }

        
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
