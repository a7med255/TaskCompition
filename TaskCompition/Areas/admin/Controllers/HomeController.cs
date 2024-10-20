
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskCompition.Bl;
using TaskCompition.Models;

namespace TaskCompition.Areas.admin.Controllers
{
    [Area("admin")]
    public class HomeController : Controller
    {
        public HomeController(IUser user)
        {
            ClsUser = user;
        }
        IUser ClsUser;
        public IActionResult List()
        {

            return View(ClsUser.GetAll());
        }
        public IActionResult Details(int id)
        {
            var user = ClsUser.GetById(id);
            return View(user);
        }
        public IActionResult Select()
        {
           ViewBag.lstUser = ClsUser.GetSelect(5);
            return View("Select");
        }
        public IActionResult Delete(int id)
        {
            ClsUser.Delete(id);
            return RedirectToAction("List");

        }

    }
}
