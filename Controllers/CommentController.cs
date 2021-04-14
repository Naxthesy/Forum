using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Forum.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Forum.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationContext _db;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public CommentController(ApplicationContext db, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _db = db;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<JsonResult> PostComment(int questionId, string text)
        {
            try
            {
                if (text == null)
                {
                    return Json(false);
                }
                Comment c = new Comment();
                c.Changed = false;
                c.CreatedDate = DateTime.Now;
                c.IsAnswer = false;
                c.Text = text;
                string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                c.user = await _userManager.FindByIdAsync(id);
                c.QuestionId = questionId;
                var q = _db.Questions.Find(questionId);
                if (c.user.Id == null)
                {
                    return Json(false);
                }
                q.Comments.Add(c);
                _db.Comments.Add(c);
                await _db.SaveChangesAsync();
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }

        }
        public JsonResult DeleteComment(int id)
        {
            try
            {
                var deletedComment = _db.Comments.Include(x => x.user).First(x => x.Id == id);
                string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (deletedComment.user.Id == Id)
                {
                    _db.Comments.Remove(deletedComment);
                    _db.SaveChanges();
                    return Json(true);
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
        public JsonResult ChangeComment(int id, string text)
        {
            try
            {
                if (text == null)
                {
                    return Json(false);
                }
                var comment = _db.Comments.Include(x => x.user).First(x => x.Id == id);
                string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (comment.user.Id == Id)
                {
                    comment.Text = text;
                    _db.SaveChanges();
                    return Json(true);
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
        public JsonResult PostLike(int id)
        {
            try
            {

                var comment = _db.Comments.Find(id);
                string Id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                try
                {
                    var CanceledLike = _db.Likes.Include(x => x.User).First(x => x.CommentId == id && x.User.Id == Id);
                    _db.Likes.Remove(CanceledLike);
                    _db.SaveChanges();
                    return Json("deleted");
                }
                catch (Exception)
                {
                    Like like = new Like();
                    like.CommentId = id;
                    like.UserId = Id;
                    try
                    {
                        _db.Likes.Add(like);
                        //comment.Likes.Add(like);

                        _db.SaveChanges();
                        return Json("added");

                    }
                    catch (Exception)
                    {

                    }

                }
                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }
    }
}
