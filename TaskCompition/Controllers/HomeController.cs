using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TaskCompition.Bl;
using TaskCompition.Models;

namespace TaskCompition.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        TaskContext context;
        public HomeController(ILogger<HomeController> logger, IUser user, TaskContext context)
        {
            _logger = logger;
            ClsUser = user;
            this.context = context;
        }
        IUser ClsUser;
        public IActionResult Index(int? id )
        {
            var user = new User();
            //if (id != null)
            //{
            //    user = ClsUser.GetById(Convert.ToInt32(id));
            //}
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(User user, List<IFormFile> Files)
        {
            user.Image = await UploadImage(Files);
            if (ModelState.IsValid)
            {
                bool emailExists = CheckEmailInDatabase(user.Email);
                if (emailExists)
                {
                    // If email exists, add a custom error to the ModelState and return the view with the error
                    ModelState.AddModelError("Email", "This email is already in use.");
                    return View("Index", user);
                }

                ClsUser.Save(user);

                return RedirectToAction("Index");
            }
            return View("Index", user);

        }
        private bool CheckEmailInDatabase(string email)
        {
            // Replace this logic with actual database checking
            List<string> existingEmails =context.TbUsers.Select(a=>a.Email).ToList();
            return existingEmails.Contains(email);
        }
        async Task<string> UploadImage(List<IFormFile> Files)
        {
            foreach (var file in Files)
            {
                if (file.Length > 0)
                {
                    string ImageName = Guid.NewGuid().ToString() + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".jpg";
                    var filePaths = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads/", ImageName);
                    using (var stream = System.IO.File.Create(filePaths))
                    {
                        await file.CopyToAsync(stream);
                        return ImageName;
                    }
                }
            }
            return string.Empty;
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyEmail(string Email)
        {
            if (false)
            {
                return Json($"Email {Email} is already in use.");
            }

            return Json(true);
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult IsPhoneUnique(string phoneNumber)
        {
            if (false)
            {
                return Json($"Phone number '{phoneNumber}' is already in use.");
            }
            return Json(true);
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
