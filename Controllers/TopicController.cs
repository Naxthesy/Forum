using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Controllers
{
    public class TopicController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public TopicController(ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public JsonResult AddTopic(string Title, string Descriprion)
        {
            Topic t = new Topic();
            t.Title = Title;
            t.Description = Descriprion;
            t.CreatedDate = DateTime.Now;
            t.Views = 0;
            _db.Topics.Add(t);
            _db.SaveChanges();
            return new JsonResult(_db.Topics.ToList());
        }
        public JsonResult OrderByDate(bool descending = true)
        {
            try
            {
                if (descending)
                {
                    return Json(new { data = _db.Topics.OrderByDescending(x => x.CreatedDate) });
                }
                else
                {
                    return Json(new { data = _db.Topics.OrderBy(x => x.CreatedDate) });
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Topics = _db.Topics.ToList();
            return View();
        }

        public JsonResult OrderByDateInterval(DateTime dt1, DateTime? dt2)
        {
            try
            {
                if (dt2 != null)
                {
                    return Json(_db.Topics.Where(x => x.CreatedDate >= dt1 && x.CreatedDate <= dt2)); //тут в джейсонку кидать все топики созданные в промежутке от dt1 до dt2
                }
                else
                {
                    return Json(_db.Topics.Where(x => x.CreatedDate > dt1));
                }
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
        public JsonResult OrderByDescription(string description)
        {
            try
            {
                return Json(_db.Topics.Where(x => x.Description.Contains(description)));

            }
            catch (Exception)
            {

                return Json(false);
            }
        }
        public JsonResult OrderByTitle(string description)
        {
            try
            {
                return Json(_db.Topics.Where(x => x.Title.Contains(description)));
            }
            catch (Exception)
            {

                return Json(false);
            }
        }
        public JsonResult OrderByViews(bool descending = true)
        {
            try
            {
                if (descending)
                {
                    return Json(_db.Topics.OrderBy(x => x.Views));
                }
                else
                {
                    return Json(_db.Topics.OrderByDescending(x => x.Views));
                }
            }
            catch (Exception)
            {

                return Json(false);
            }
        }
        public JsonResult OrderByTitleAndDescription(string description)
        {
            try
            {
                if (description != null)
                {
                    return Json(_db.Topics.Where(x => x.Title.Contains(description) || x.Description.Contains(description)));
                }
                else
                {
                    return Json(false);
                }
            }
            catch (Exception)
            {

                return Json(false);
            }
        }
    }
}
